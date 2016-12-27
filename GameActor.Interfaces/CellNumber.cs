using System.Runtime.Serialization;

namespace GameActor.Interfaces
{
    /// <summary>
    /// Holds the cell number.
    /// </summary>
    [DataContract]
    public enum CellNumber
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        First = 1,
        [EnumMember]
        Second = 2,
        [EnumMember]
        Third = 3,
        [EnumMember]
        Forth = 4,
        [EnumMember]
        Fifth = 5,
        [EnumMember]
        Sixth = 6,
        [EnumMember]
        Seventh = 7,
        [EnumMember]
        Eighth = 8,
        [EnumMember]
        Ninth = 9
    }
}
