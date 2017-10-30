using ClientProxyCommon;
using GameActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebConnector.Source
{
    public class ActorHelper
    {
        private ITicTacToeEvents _ticTacToeEvents;

        private Dictionary<string, ITicTacToe> _dic = new Dictionary<string, ITicTacToe>();

        public ActorHelper(IWebSocket webSocket)
        {
            _ticTacToeEvents = new TicTacToeEventsSimulation(webSocket);
        }

        public ITicTacToe GetActor(string name)
        {

            if(_dic.ContainsKey(name))
            {
                return _dic[name];
            }
            else
            {
                var gameId = new ActorId(name);
                var game = ActorProxy.Create<ITicTacToe>(gameId, "fabric:/TicTacToe");
                game.SubscribeAsync(_ticTacToeEvents);
                _dic.Add(name, game);
                return game;

            }


            /*
            var gameId = new ActorId(name);
            var game = ActorProxy.Create<ITicTacToe>(gameId, "fabric:/TicTacToe");
            game.SubscribeAsync(_ticTacToeEvents);
            _dic.Add(name, game);
            return game;
            */

        }


    }
}
