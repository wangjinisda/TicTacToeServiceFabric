using ClientProxyCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProxyCommon.Model;
using GameActor.Interfaces;
using ClientProxyCommon.Extensions;
using System.Reflection;

namespace TicTacToe.Client.source.Layer
{
    public class SocketCaller : ISocketCaller
    {
        private ITicTacToeEvents _ticTacToeEvents;

        private IWebSocket _webSocket;

        public SocketCaller(ITicTacToeEvents ticTacToeEvents)
        {
            _ticTacToeEvents = ticTacToeEvents;
        }

        public Task CallAsync(ActionData actionData, Func<ActionData, Task> back = null)
        {

            ActionData ret = null;
            if (actionData.ActionType == ActionType.Call)
            {
                switch (actionData.MethodType)
                {
                    case MethodType.GameStarted:
                        {
                           // var data = actionData.ContentBox.AsSpecific();
                            _ticTacToeEvents.GameStarted();
                            ret = new ActionData(actionData.UniqueID);
                            ret.ActionType = ActionType.Back;
                        }
                        break;
                    case MethodType.Moved:
                        var methods = typeof(ITicTacToeEvents).GetMethod(MethodType.Moved.ToString());
                        {
                            var data = actionData.ContentBox.AsSpecific();
                            if(Type.GetType(actionData.ContentBox.Type) == typeof(ContentBox[]))
                            {
                                var realParams = ((ContentBox[])data).Select(x => x.AsSpecific()).ToArray();
                                methods.Invoke(_ticTacToeEvents, realParams);
                            }
                            else
                            {
                                methods.Invoke(_ticTacToeEvents, new[] { data });
                            }
                            
                            ret = new ActionData(actionData.UniqueID);
                            ret.ActionType = ActionType.Back;
                        }

                        break;
                    case MethodType.GameEnded:

                        {
                            var data = actionData.ContentBox.AsSpecific();
                            _ticTacToeEvents.GameEnded((GameEndedInfo)data);
                            ret = new ActionData(actionData.UniqueID);
                            ret.ActionType = ActionType.Back;
                            
                        }
                        break;
                }

            }

            return back(ret);
        }

        public void SetWebSocket(IWebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        public IWebSocket WebSocket { get => _webSocket; }
    }
}
