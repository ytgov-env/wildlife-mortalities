using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities
{
    public class Bison : Animal
    {
        public bool Pregnant { get; set; }
        public bool Wounded { get; set; }
    }
}
