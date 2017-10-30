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

        private ManualResetEventSlim _mre = new ManualResetEventSlim(false);

        protected WebSocket _websocket;

        protected ISocketCaller _socketCaller;

        protected ActionDelegate _actionDelegate = null;

        protected ConcurrentDictionary<string, TaskCompletionSource<ActionData>> _dic =
            new ConcurrentDictionary<string, TaskCompletionSource<ActionData>>();

        protected ConcurrentQueue<ActionData> _queue = 
            new ConcurrentQueue<ActionData>();


        public virtual Delegates.ActionDelegate ActionDelegate => _actionDelegate;


        public WebSocketServerBase(WebSocket websocket, ISocketCaller socketCaller)
        {
            _websocket = websocket;
            _socketCaller = socketCaller;
            _socketCaller.SetWebSocket(this);

            ActionDelegateInitial();

            ThreadShell.LongRun(async() =>
            {
                while (true)
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
            });
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
            if(_queue.Count() > 0)
            {
                var ret = _queue.TryDequeue(out var data);

                //return processor(new[]{ data });
            }

            return Task.CompletedTask;
            
        }


        public virtual Task ConsumeActionData()
        {
            if (_queue.Count() > 0)
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

        public void SendResult(ActionData box, Action before = null)
        {
            EnequeueActionData(box);
        }

        public abstract Task<ActionData> SendWithCnfirmAsync(ActionData box, Action before = null);


        public virtual Task SendLogic(ActionData box)
        {
            _mre.Wait();
            return _websocket.SendAsync(
                new ArraySegment<byte>(box.AsBytes()),
                WebSocketMessageType.Binary,
                true, CancellationToken.None);

        }

        public async void CloseAsync()
        {
            await _websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "pls close!", CancellationToken.None);
        }
    }
}
