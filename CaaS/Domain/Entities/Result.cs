using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Serializable]
    public class Result
    {
        public string res { get; set; }
        public Result(string res)
        {
            this.res = res;
        }
    }
}
