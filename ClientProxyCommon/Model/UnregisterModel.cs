using GameActor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClientProxyCommon.Model
{
    [DataContract]
    public class UnregisterModel
    {
        [DataMember]
        public PlayerProfileModel PlayerProfileModel { get;set;}

        [DataMember]
        public bool IfEarlyBailOut { get;set;}
}
}
