using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientProxyCommon
{
    public class ContentBox
    {
        public string Type { get; set; }

        public string PayLoad { get; set; }

        public string UniqueID => Guid.NewGuid().ToString();

        public static ContentBox CreateFromObject(object obj)
        {
            using (var stream = new MemoryStream())
            {
                var Serializer = new DataContractJsonSerializer(obj.GetType());
                Serializer.WriteObject(stream, obj);
                stream.Position = 0;
                byte[] json = stream.ToArray();
                return new ContentBox
                {
                    Type = obj.GetType().AssemblyQualifiedName,
                    PayLoad = Encoding.UTF8.GetString(json, 0, json.Length)
                };
            }
        }
    }
}
