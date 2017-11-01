using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProxyCommon.Model;
using System.Net.WebSockets;
using System.Collections.Concurrent;
using ClientProxyCommon.Extensions;
using static ClientProxyCommon.Delegates;
using System.Threading;

namespace ClientProxyCommon.WebSocketCaller
{
    public class WebSocketServer: IWebSocket
    {
        private readonly ISocketCaller _socketCaller;

        private readonly WebSocket _websocket;

        private readonly ConcurrentDictionary<string, TaskCompletionSource<ActionData>> _dic =
            new ConcurrentDictionary<string, TaskCompletionSource<ActionData>>();

        public Delegates.ActionDelegate ActionDelegate { get; } = null;

        public WebSocketServer(WebSocket websocket, ISocketCaller socketCaller)
        {
            _socketCaller = socketCaller;
            _socketCaller.SetWebSocket(this);

            _websocket = websocket;

            ActionDelegate = action =>
            {
                if (action.ActionType == ActionType.Call)
                {
                    return _socketCaller.CallAsync(action, ret => {
                        SendResult(ret);
                        return Task.CompletedTask;
                    });
                }
                else
                {
                    _dic.TryRemove(action.UniqueID, out var task);
                    task?.SetResult(action);
                    return Task.CompletedTask;
                    // Console.WriteLine("Laputa says: " + e.Data);
                }
            };
        }

        public void SendResult(ActionData data, Action before = null)
        {
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

        public void Dispose()
        {
            _websocket.Dispose();
        }

        public async Task CloseAsync()
        {
            await _websocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure, 
                "pls close!", CancellationToken.None)
                .ContinueWith(_=> this.Dispose());
        }
    }
}
