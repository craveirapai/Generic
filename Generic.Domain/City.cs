using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Domain
{
    public class City
    {
        public virtual Int32 Id { get; set; }
        public Int32 StateId { get; set; }
        public String Name  { get; set; }
        public State State { get; set; }
    }

}
