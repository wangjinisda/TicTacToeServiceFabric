using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClientProxyCommon.Model
{
    [DataContract]
    public enum MethodType
    {
        [EnumMember]
        Move = 0,

        [EnumMember]
        Register = 1,

        [EnumMember]
        UnRegister = 2,

        [EnumMember]
        GameStarted = 3,

        [EnumMember]
        Moved = 4,

        [EnumMember]
        GameEnded = 5,
    }
}
