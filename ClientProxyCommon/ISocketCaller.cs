using ClientProxyCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientProxyCommon
{
    public interface ISocketCaller
    {
        Task CallAsync(ActionData actionData, Func<ActionData, Task> back = null);

        IWebSocket WebSocket { get;}


        void SetWebSocket(IWebSocket webSocket);
    }
}
