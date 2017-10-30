using ClientProxyCommon;
using ClientProxyCommon.Extensions;
using ClientProxyCommon.Model;
using GameActor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebConnector.Source
{
    public static class Dispatcher
    {
        /*
        public static async Task Dispatch(ActionData data, Func<byte[], Task> sender)
        {

            if(data.ActionType == ActionType.Call)
            {

            }
            else
            {
                switch (data.MethodType)
                {
                    case MethodType.Move:
                        {
                            var meta = (MoveMetadata)data.ContentBox.AsSpecific();
                            var service = ActorHelper.GetActor(meta.PlayerProfileModel.GameRoom);
                            var ret = await service.Move(meta);
                            if (ret)
                            {
                                var outs = new ActionData(data.UniqueID);
                                outs.ActionType = ActionType.Back;
                                await sender?.Invoke(outs.AsBytes());
                            }
                        }
                        break;

                    case MethodType.Register:
                        {
                            var meta = (PlayerProfileModel)data.ContentBox.AsSpecific();
                            var service = ActorHelper.GetActor(meta.GameRoom);
                            var ret = await service.Register(meta);
                            if (ret)
                            {
                                var outs = new ActionData(data.UniqueID);
                                outs.ActionType = ActionType.Back;
                                await sender?.Invoke(outs.AsBytes());
                            }
                        }
                        break;

                    case MethodType.UnRegister:
                        {
                            var meta = (UnregisterModel)data.ContentBox.AsSpecific();
                            var service = ActorHelper.GetActor(meta.PlayerProfileModel.GameRoom);
                            var ret = await service.Unregister(meta.PlayerProfileModel, meta.IfEarlyBailOut);
                            if (ret)
                            {
                                var outs = new ActionData(data.UniqueID);
                                outs.ActionType = ActionType.Back;
                                await sender?.Invoke(outs.AsBytes());
                            }
                        }

                        break;

                }

            }
            

        }
        */
    }
}
