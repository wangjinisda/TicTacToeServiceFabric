using ClientProxyCommon;
using ClientProxyCommon.Extensions;
using ClientProxyCommon.Model;
using ClientProxyCommon.WebSocketCaller;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebConnector.Source
{
    public static class SocketEntry
    {
        public static async Task Echo(WebSocket webSocket)
        {
            var caller = new ActorSocketCaller();
            var socket = new WebSocketServerEnhance(webSocket, caller);
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                var content = buffer.AsActionData(result.Count);

                await ThreadShell.LongRun(() => socket.ActionDelegate(content));
                try
                {
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }catch(Exception e)
                {
                    Debug.WriteLine($"hellow world server end:   {e.Message}");
                    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                }
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
