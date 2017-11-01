using ClientProxyCommon.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProxyCommon.Model;
using System.Net.WebSockets;
using System.Threading;
using ClientProxyCommon.Extensions;

namespace ClientProxyCommon.WebSocketCaller
{
    public class WebSocketServerEnhance : WebSocketServerBase
    {
        public WebSocketServerEnhance(WebSocket websocket, ISocketCaller socketCaller)
            :base(websocket, socketCaller)
        {
            base.EventSet();
        }

        public override Task<ActionData> SendWithCnfirmAsync(ActionData box, Action before = null)
        {
            var source = new TaskCompletionSource<ActionData>();
            return _websocket.SendAsync(
                new ArraySegment<byte>(box.AsBytes()),
                WebSocketMessageType.Binary,
                true, CancellationToken.None).ContinueWith(
                _ => {
                    _dic.AddOrUpdate(box.UniqueID, source, (key, old) => source);
                    before?.Invoke();
                    return source.Task;
                }).Unwrap();
        }
    }
}
