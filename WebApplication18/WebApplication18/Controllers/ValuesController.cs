using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication18.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public Dictionary<SampleKey, string> Get()
        {
            var dict = new Dictionary<SampleKey, string>();
            dict.Add(new SampleKey() { Key1 = "Nelson", Key2 = "Paily" }, "34");
            dict.Add(new SampleKey() { Key1 = "Nelson", Key2 = "Varghese" }, "34");

            return dict;
        }

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }

    public class SampleKey
    {
        public override string ToString()
        {
            return Key1 + Key2;
        }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
    }
}
