using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClientProxyCommon.Model
{
    [DataContract]
    public class ActionData : IData
    {
        private string _id;

        public ActionData(string id)
        {
            _id = id;
        }

        public ActionData()
        {
            _id = Guid.NewGuid().ToString();
        }

        [DataMember]
        public ActionType ActionType { get; set; }

        [DataMember]
        public MethodType MethodType { get; set; }

        [DataMember]
        public ContentBox ContentBox { get; set; }

        [DataMember]
        public string UniqueID { get => _id; set { _id = value; }  }

        [DataMember]

        public ActionDataType ActionDataType { get; set; }
    }
}
