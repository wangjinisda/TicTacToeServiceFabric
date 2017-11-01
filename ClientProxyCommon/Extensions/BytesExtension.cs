using ClientProxyCommon.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientProxyCommon.Extensions
{
    public static class BytesExtension
    {
        public static ContentBox AsContentBox(this byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ContentBox));
                var result = ser.ReadObject(ms);
                return (ContentBox)result;
            }
        }

        public static ContentBox AsContentBox(this byte[] bytes, int count)
        {
            using (var ms = new MemoryStream(bytes, 0, count))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ContentBox));
                var result = ser.ReadObject(ms);
                return (ContentBox)result;
            }
        }

        public static ActionData AsActionData(this byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ActionData));
                var result = ser.ReadObject(ms);
                return (ActionData)result;
            }
        }

        public static ActionData AsActionData(this byte[] bytes, int count)
        {
            using (var ms = new MemoryStream(bytes, 0, count))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ActionData));
                var result = ser.ReadObject(ms);
                return (ActionData)result;
            }
        }
    }
}
