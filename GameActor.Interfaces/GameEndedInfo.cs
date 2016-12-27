using System.Runtime.Serialization;

namespace GameActor.Interfaces
{
    [DataContract]
    public class GameEndedInfo
    {
        [DataMember]
        public GameEndedEventType EventType { get; set; }

        [DataMember]
        public PlayerType Player { get; set; }

        [DataMember]
        public WinVector WinVector { get; set; }
    }
}