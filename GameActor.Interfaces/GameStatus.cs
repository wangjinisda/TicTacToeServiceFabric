using System.Runtime.Serialization;

namespace GameActor.Interfaces
{
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class GameStatus
    {
        [DataMember]
        public WinVector WinVector { get; set; }
        [DataMember]
        public PlayerType? Winner { get; set; }
        [DataMember]
        public bool IsDraw { get; set; }
    }
}
