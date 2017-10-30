using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientProxyCommon.Extensions
{
    public static class ContentBoxExtension
    {
        public static Object AsSpecific(this ContentBox content)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(content.PayLoad)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(Type.GetType(content.Type));
                var result = ser.ReadObject(ms);
                return result;
            }
        }

        public static byte[] AsBytes(this ContentBox content)
        {
            using (var stream = new MemoryStream())
            {
                var Serializer = new DataContractJsonSerializer(typeof(ContentBox));
                Serializer.WriteObject(stream, content);
                stream.Position = 0;
                byte[] json = stream.ToArray();
                return json;
            }
        }
    }
}
