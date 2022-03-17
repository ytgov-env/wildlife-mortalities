using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities
{
    public class GameManagementUnit
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public List<GameManagementAreaSpecies> GameManagementAreaSpecies { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
    }
}
