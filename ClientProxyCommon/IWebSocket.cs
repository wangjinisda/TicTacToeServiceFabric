using ClientProxyCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClientProxyCommon.Delegates;

namespace ClientProxyCommon
{
    public interface IWebSocket: IDisposable
    {
        Task<ActionData> SendWithCnfirmAsync(ActionData box, Action before = null);

        void SendResult(ActionData data, Action before = null);

        ActionDelegate ActionDelegate { get; }

        void CloseAsync();
    }
}
