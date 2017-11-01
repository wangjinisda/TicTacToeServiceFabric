using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProxyCommon.Model;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading;
using ClientProxyCommon.Extensions;
using System.Diagnostics;

namespace ClientProxyCommon.WebSocketCaller
{
    public class WebSocketFront : IWebSocket
    {
        private readonly ConcurrentDictionary<string, TaskCompletionSource<ActionData>> _dic =

                                new ConcurrentDictionary<string, TaskCompletionSource<ActionData>>();

        private readonly ClientWebSocket _websocket;

        private readonly ISocketCaller _socketCaller;

        private readonly ManualResetEventSlim _mre = new ManualResetEventSlim(false);

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public Delegates.ActionDelegate ActionDelegate { get; }

        public WebSocketFront(string uri, ISocketCaller socketCaller)
        {
            _socketCaller = socketCaller;
            _websocket = new ClientWebSocket();
            _websocket.Options.SetBuffer(4 * 1024, 4 * 1024);

            ActionDelegate = async action =>
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
                    // Console.WriteLine("Laputa says: " + e.Data);
                }
            };

            Task.Run(async()=>
            {
                await _websocket.ConnectAsync(new Uri(uri), CancellationToken.None);
                // _websocket.State
                //_websocket.State
                _mre.Set();
                var buffer = new byte[1024 * 4];
                WebSocketReceiveResult result = await _websocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                while (!result.CloseStatus.HasValue && !_cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        if (result.MessageType == WebSocketMessageType.Binary)
                        {
                            var content = buffer.AsActionData(result.Count);
                            await ActionDelegate(content);
                        }
                        // Console.WriteLine($"hellow world:   {ret}");

                        result = await _websocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    }  catch(Exception e)
                    {
                        Debug.WriteLine($"hellow world:   {e.Message}");
                        await _websocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                    }
                }

                await _websocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            });
        }

        public void Dispose()
        {
            _websocket.Dispose();
            _mre.Dispose();
        }

        public void SendResult(ActionData data, Action before = null)
        {
            _mre.Wait();
            _websocket.SendAsync(
                new ArraySegment<byte>(data.AsBytes()),
                WebSocketMessageType.Binary,
                true, CancellationToken.None).ContinueWith(
                _ => {
                    before?.Invoke();
                    return Task.CompletedTask;
                }).Unwrap().Wait();
        }

        public Task<ActionData> SendWithCnfirmAsync(ActionData data, Action before = null)
        {
            _mre.Wait();
            var source = new TaskCompletionSource<ActionData>();
            return _websocket.SendAsync(
                new ArraySegment<byte>(data.AsBytes()),
                WebSocketMessageType.Binary,
                true, CancellationToken.None).ContinueWith(
                _ => {
                    _dic.AddOrUpdate(data.UniqueID, source, (key, old) => source);
                    before?.Invoke();
                    return source.Task;
                }).Unwrap();
        }

        public async void CloseAsync()
        {
            _cancellationTokenSource.Cancel();
            await _websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "pls close!", CancellationToken.None);
        }
    }
}
