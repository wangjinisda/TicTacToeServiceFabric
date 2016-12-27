using System.Runtime.Serialization;

namespace GameActor.Interfaces
{
    [DataContract]
    public enum GameEndedEventType
    {
        [EnumMember]
        TimedOut = 1,
        [EnumMember]
        BailedOutEarly = 2,
        [EnumMember]
        Won = 3,
        [EnumMember]
        Tie = 4
    }
}