using ClientProxyCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientProxyCommon.Model;
using GameActor.Interfaces;
using ClientProxyCommon.Extensions;

namespace WebConnector.Source
{
    public class ActorSocketCaller : ISocketCaller
    {
        private IWebSocket _webSocket;

        private ActorHelper _actorHelper;

        public ActorSocketCaller()
        {
            //_webSocket = webSocket;

           // _ticTacToeEvents = new TicTacToeEventsSimulation(_webSocket);

            //_actorHelper = new ActorHelper(_webSocket);
        }

        public async Task CallAsync(ActionData actionData, Func<ActionData, Task> back = null)
        {
            ActionData outs = null;
            switch (actionData.MethodType)
            {
                case MethodType.Move:
                    {
                        var meta = (MoveMetadata)actionData.ContentBox.AsSpecific();
                        var service = _actorHelper.GetActor(meta.PlayerProfileModel.GameRoom);
                        var ret = await service.Move(meta);
                        outs = new ActionData(actionData.UniqueID);
                        outs.ActionType = ActionType.Back;
                        outs.MethodType = MethodType.Move;
                        if (ret)
                        {
                            outs.ContentBox = ContentBox.CreateFromObject(true);
                        }
                        else
                        {
                            outs.ContentBox = ContentBox.CreateFromObject(false);
                        }
                    }
                    break;

                case MethodType.Register:
                    {
                        var meta = (PlayerProfileModel)actionData.ContentBox.AsSpecific();
                        var service = _actorHelper.GetActor(meta.GameRoom);
                        var ret = await service.Register(meta);
                        outs = new ActionData(actionData.UniqueID);
                        outs.ActionType = ActionType.Back;
                        outs.MethodType = MethodType.Register;
                        if (ret)
                        {
                            outs.ContentBox = ContentBox.CreateFromObject(true);
                        }
                        else
                        {
                            outs.ContentBox = ContentBox.CreateFromObject(false);
                        }
                    }
                    break;

                case MethodType.UnRegister:
                    {
                        var meta = (UnregisterModel)actionData.ContentBox.AsSpecific();
                        var service = _actorHelper.GetActor(meta.PlayerProfileModel.GameRoom);
                        var ret = await service.Unregister(meta.PlayerProfileModel, meta.IfEarlyBailOut);
                        outs = new ActionData(actionData.UniqueID);
                        outs.ActionType = ActionType.Back;
                        outs.MethodType = MethodType.UnRegister;
                        if (ret)
                        {
                            outs.ContentBox = ContentBox.CreateFromObject(true);
                        }
                        else
                        {
                            outs.ContentBox = ContentBox.CreateFromObject(false);
                        }
                    }

                    break;

            }

            await back?.Invoke(outs);
        }

        public void SetWebSocket(IWebSocket webSocket)
        {
            _webSocket = webSocket;
            _actorHelper = new ActorHelper(_webSocket);
        }

        public IWebSocket WebSocket { get => _webSocket; }
    }
}
