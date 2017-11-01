using ClientProxyCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActor.Interfaces;
using ClientProxyCommon.Model;
using ClientProxyCommon.WebSocketCaller;
using ClientProxyCommon.Extensions;
using System.Net.WebSockets;
using System.Threading;

namespace TicTacToe.Client.source.Layer
{
    public class WebSocketTicTacToeProxy : ITicTacToeProxy
    {
        private IWebSocket _webSocketClient;

        public WebSocketTicTacToeProxy(string socketUrl, ITicTacToeEvents ticTacToeEvents) 
        {
            _webSocketClient = new WebSocketFront(socketUrl, new SocketCaller(ticTacToeEvents));
            
        }

        public Task CloseAsync()
        {
            return _webSocketClient.CloseAsync();
        }

        public void Dispose()
        {
            _webSocketClient.Dispose();
        }

        public async Task<bool> Move(MoveMetadata moveMetadata)
        {
            var box = ContentBox.CreateFromObject(moveMetadata);
            var data =  await _webSocketClient.SendWithCnfirmAsync(new ActionData
            {
                ActionType = ActionType.Call,
                MethodType = MethodType.Move,
                ContentBox = box
            },
            () => { }
            );

            var ret = data.ContentBox.AsSpecific();
            return (bool)ret;
        }

        public async Task<bool> RegisterAsync(PlayerProfileModel model)
        {
            var box = ContentBox.CreateFromObject(model);

            var data =  await _webSocketClient.SendWithCnfirmAsync(new ActionData
            {
                ActionType = ActionType.Call,
                MethodType = MethodType.Register,
                ContentBox = box
            });

            var ret = data.ContentBox.AsSpecific();
            return (bool)ret;
        }

        public async Task<bool> Unregister(PlayerProfileModel model, bool earlyBailOut)
        {
            var box = ContentBox.CreateFromObject(new UnregisterModel
            {
                PlayerProfileModel = model,
                IfEarlyBailOut = earlyBailOut
            });

            var data = await _webSocketClient.SendWithCnfirmAsync(new ActionData
            {
                ActionType = ActionType.Call,
                MethodType = MethodType.UnRegister,
                ContentBox = box
            });

            var ret = data.ContentBox.AsSpecific();
            return (bool)ret;
        }
    }
}
