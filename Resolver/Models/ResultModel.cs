using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resolver.Models
{
    public class Result
    {
        public string outputStr { get; set; }
        public int outputValue()
        {
            int res;
            if (int.TryParse(outputStr, out res))
                return res;
            else
                throw new Exception("can't convert to int");        }
    }
}