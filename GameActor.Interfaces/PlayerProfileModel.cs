using GameActor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GameActor.Interfaces
{
    [DataContract]
    public class PlayerProfileModel
    {
        [DataMember]
        public  PlayerType PlayerType { get; set; }

        [DataMember]
        public string GameRoom { get; set; }

        [DataMember]
        public string PlayerName { get; set; }
    }
}
