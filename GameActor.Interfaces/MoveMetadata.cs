using System.Runtime.Serialization;

namespace GameActor.Interfaces
{
    /// <summary>
    /// Holds the move metadata. This class is immutable. http://en.wikipedia.org/wiki/Immutable_object
    /// </summary>
    [DataContract]
    public class MoveMetadata
    {
        public MoveMetadata(PlayerType playerType, CellNumber cellNumber)
        {
            this.Player = playerType;
            this.CellNumber = cellNumber;
        }

        [DataMember]
        public PlayerType Player { get; private set; }

        [DataMember]
        public CellNumber CellNumber { get; private set; }


        [DataMember]
        public PlayerProfileModel PlayerProfileModel { get; set; }
    }
}
