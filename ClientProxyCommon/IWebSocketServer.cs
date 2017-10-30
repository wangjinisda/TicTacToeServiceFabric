using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientProxyCommon
{
    public interface IWebSocketServer: IWebSocket
    {
        Task ConsumeActionDatas();

        Task ConsumeActionData();
    }
}
