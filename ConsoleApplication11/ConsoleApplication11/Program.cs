using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication11
{
    class Program
    {
        static void Main(string[] args)
        {

            EventMessage obj = new EventMessage();
            obj.MessageEntity = "";
            MessageEntity messageEntity = obj.MessageEntity.ToMessageEntityEnum();

        }
    }

    public class EventMessage
    {
        public string MessageEntity { get; set; }

    }

    public enum MessageEntity
    {
        None = 0,
        GRM,
        Axis    
    }

    //public interface IMessageEntity
    //{
    //    string Name { get; set; }
    //}

    public static class Extensions
    {
        public static MessageEntity ToMessageEntityEnum(this string messageEntity)
        {
            if (!(MessageEntity)Enum.TryParse(typeof(MessageEntity), messageEntity))
                return MessageEntity.None;
            else
                // return the actual enum
        }

        public static string ToMessageEntityString(this MessageEntity messageEntity)
        {
            return messageEntity.ToString();
        }
    }



}
