using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities
{
    public class GameManagementAreaSchedule
    {
        public int Id { get; set; }
        public int GameManagementAreaSpeciesId { get; set; }
        public GameManagementAreaSpecies GameManagementAreaSpecies { get; set; }
        public bool IsOpen { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
