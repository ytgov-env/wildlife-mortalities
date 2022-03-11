using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities
{
    public class Licence
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public LicenceType Type { get; set; }
        public List<Seal> Seals { get; set; }
        public List<TrappedMortality> TrappedMortalities { get; set; }
    }
}
