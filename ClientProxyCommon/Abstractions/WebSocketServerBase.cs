using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProxyCommon.Model;
using System.Net.WebSockets;
using System.Threading;
using System.Collections.Concurrent;
using ClientProxyCommon.Extensions;
using static ClientProxyCommon.Delegates;
using System.Diagnostics;

namespace ClientProxyCommon.Abstractions
{
    public abstract class WebSocketServerBase : IWebSocketServer
    {
        private readonly ManualResetEventSlim _mre = new ManualResetEventSlim(false);

        protected WebSocket _websocket;

        protected ISocketCaller _socketCaller;

        protected ActionDelegate _actionDelegate = null;

        protected ConcurrentDictionary<string, TaskCompletionSource<ActionData>> _dic =
            new ConcurrentDictionary<string, TaskCompletionSource<ActionData>>();

        protected ConcurrentQueue<ActionData> _queue =
            new ConcurrentQueue<ActionData>();

        public virtual Delegates.ActionDelegate ActionDelegate => _actionDelegate;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        protected WebSocketServerBase(WebSocket websocket, ISocketCaller socketCaller)
        {
            _websocket = websocket;
            _socketCaller = socketCaller;
            _socketCaller.SetWebSocket(this);

            ActionDelegateInitial();

            ThreadShell.LongRun(async() =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        await ConsumeActionData();

                        await Task.Delay(400);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"hellow world server:   {e.Message}");
                        continue;
                    }
                }
            }, _cancellationTokenSource);
        }

        public virtual void ActionDelegateInitial()
        {
            _actionDelegate = async action =>
            {
                if (action.ActionType == ActionType.Call)
                {
                    await _socketCaller.CallAsync(action, ret => {
                        SendResult(ret);
                        return Task.CompletedTask;
                    });
                }
                else
                {
                    _dic.TryRemove(action.UniqueID, out var task);
                    task?.SetResult(action);
                    await Task.CompletedTask;
                    //Console.WriteLine("Laputa says: " + e.Data);
                }
            };
        }

        public virtual void EventSet()
        {
            _mre.Set();
        }

        public virtual void WaitOne()
        {
            _mre.Wait();
        }

        public virtual void EnequeueActionData(ActionData data)
        {
            data.ActionDataType = ActionDataType.Single;
            _queue.Enqueue(data);
        }

        public virtual Task ConsumeActionDatas()
        {
            if(_queue.Count > 0)
            {
                var ret = _queue.TryDequeue(out var data);

                // return processor(new[]{ data });
            }

            return Task.CompletedTask;
        }

        public virtual Task ConsumeActionData()
        {
            if (_queue.Count > 0)
            {
                var ret = _queue.TryDequeue(out var data);

                return SendLogic(data);
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _websocket.Dispose();
        }

        public abstract Task<ActionData> SendWithCnfirmAsync(ActionData box, Action before = null);

        public virtual Task SendLogic(ActionData box)
        {
            _mre.Wait();
            if(_websocket.State == WebSocketState.Open)
            {
                return _websocket.SendAsync(
                new ArraySegment<byte>(box.AsBytes()),
                WebSocketMessageType.Binary,
                true, CancellationToken.None);
            }
            else
            {
                throw new InvalidOperationException("websocket error.");
            }
            
        }

        public async Task CloseAsync()
        {
            _cancellationTokenSource.Cancel();
            await _websocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "pls close!", CancellationToken.None)
                .ContinueWith(_ => this.Dispose());
        }

        public void SendResult(ActionData data, Action before = null)
        {
            EnequeueActionData(data);
            before?.Invoke();
        }
    }
}
