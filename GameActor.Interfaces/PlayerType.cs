using System.Runtime.Serialization;

namespace GameActor.Interfaces
{
    /// <summary>
    /// Holds the player type.
    /// </summary>
    [DataContract]
    public enum PlayerType
    {
        [EnumMember]
        Zero = 0,
        [EnumMember]
        Cross = 1
    }
}
