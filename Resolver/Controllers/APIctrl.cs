using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Resolver.Models;

namespace Resolver
{
    public class APIctrl : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Result> GetResult(string expression)
        {
            Result[] r = new Result[1];
            InputResolver ir = new InputResolver();
            r[0].outputStr = ir.getResult(expression);
            return r;
        }

        // GET api/<controller>/5
        /*public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }*/
    }
}