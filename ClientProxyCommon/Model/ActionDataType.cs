using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClientProxyCommon.Model
{
    [DataContract]
    public enum ActionDataType
    {
        [EnumMember]
        Composed = 0,

        [EnumMember]
        Single = 1,
    }
}
