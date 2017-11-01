using ClientProxyCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientProxyCommon
{
    public static class Delegates
    {
        public delegate Task ActionDelegate(ActionData data);
    }
}
