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
    public static class ActionDataExtension
    {
        public static byte[] AsBytes(this ActionData data)
        {
            using (var stream = new MemoryStream())
            {
                var Serializer = new DataContractJsonSerializer(typeof(ActionData));
                Serializer.WriteObject(stream, data);
                stream.Position = 0;
                byte[] json = stream.ToArray();
                return json;
            }
        }


        public static byte[] AsBigger(this ActionData[] datas)
        {

            foreach(var data in datas)
            {
            }

            using (var stream = new MemoryStream())
            {
                var Serializer = new DataContractJsonSerializer(typeof(ActionData));
                Serializer.WriteObject(stream, datas);
                stream.Position = 0;
                byte[] json = stream.ToArray();
                return json;
            }
        }
    }
}
