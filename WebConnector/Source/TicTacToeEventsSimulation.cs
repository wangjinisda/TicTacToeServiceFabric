using ClientProxyCommon;
using ClientProxyCommon.Model;
using GameActor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebConnector.Source
{
    public class TicTacToeEventsSimulation : ITicTacToeEvents
    {
        private readonly IWebSocket _webSocket;

        public TicTacToeEventsSimulation(IWebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        public void GameEnded(GameEndedInfo info)
        {
            var box = ContentBox.CreateFromObject(info);

            _webSocket.SendResult(new ActionData
            {
                ActionType = ActionType.Call,
                MethodType = MethodType.GameEnded,
                ContentBox = box
            });
        }

        public void GameStarted()
        {
            // var box = ContentBox.CreateFromObject(info);
            _webSocket.SendResult(new ActionData
            {
                ActionType = ActionType.Call,
                MethodType = MethodType.GameStarted
            });
        }

        public void Moved(PlayerType player, MoveMetadata[][] moveMatrix)
        {
            var param = new ContentBox[] {
                ContentBox.CreateFromObject(player),
                ContentBox.CreateFromObject(moveMatrix)
            };

            var box = ContentBox.CreateFromObject(param);

            _webSocket.SendResult(new ActionData
            {
                ActionType = ActionType.Call,
                MethodType = MethodType.Moved,
                ContentBox = box
            });
        }
    }
}
