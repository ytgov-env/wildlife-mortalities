using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities
{
    public class Seal
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int LicenceId { get; set; }
        public Licence Licence { get; set; }
        public List<HuntedMortality> HuntedMortalities { get; set; }
    }
}
