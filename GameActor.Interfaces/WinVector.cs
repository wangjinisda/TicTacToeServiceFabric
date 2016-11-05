using System.Runtime.Serialization;

namespace GameActor.Interfaces
{
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public enum WinVector
    {
        [EnumMember]
        NONE = 0,
        [EnumMember]
        TOP = 1,
        [EnumMember]
        CENTER = 2,
        [EnumMember]
        BOTTOM = 3,
        [EnumMember]
        LEFT = 4,
        [EnumMember]
        MIDDLE = 5,
        [EnumMember]
        RIGHT = 6,
        [EnumMember]
        BACK_DIAGONAL = 7,
        [EnumMember]
        FORWARD_DIAGONAL = 8,
    }

    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class GameStatus
    {
        [DataMember]
        public WinVector WinVector { get; set; }
        [DataMember]
        public PlayerType? Winner { get; set; }
        [DataMember]
        public bool IsDraw { get; set; }
        [DataMember]
        public MoveMetadata[][] Matrix { get; set; }
        [DataMember]
        public PlayerType NextPlayer { get; set; }
    }
}
