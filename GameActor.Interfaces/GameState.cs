using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GameActor.Interfaces
{
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class GameState
    {
        [DataMember]
        public MoveMetadata[][] Matrix { get; set; }

        [DataMember]
        public IList<PlayerType> Players { get; set; }
        [DataMember]
        public PlayerType NextPlayer { get; set; }
    }
}
