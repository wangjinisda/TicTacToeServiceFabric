using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication16.Controllers
{
    public class CustomKey
    {
        public string Key { get; set; }

        public override string ToString()
        {
            return Key;
        }
    }
    public class CustomDictionary
    {
        public IDictionary<CustomKey, string> MyProperty { get; set; }
    }
    public class ValuesController : ApiController
    {
        // GET api/values
        public IDictionary<CustomKey, string> Get()
        {
            var dict = new Dictionary<CustomKey, string>();
            dict.Add(new CustomKey { Key = "Name" }, "Nelson");
            dict.Add(new CustomKey { Key = "Age" }, "34");

            return dict;//new CustomDictionary { MyProperty = dict };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
