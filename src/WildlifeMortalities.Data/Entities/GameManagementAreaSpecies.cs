using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities
{
    public class GameManagementAreaSpecies
    {
        public int Id { get; set; }
        public int GameManagementAreaId { get; set; }
        public GameManagementArea GameManagementArea { get; set; }
        public GmaSpecies Species { get; set; }
        public List<GameManagementAreaSchedule> Schedules { get; set; }
        public List<GameManagementUnit?> GameManagementUnits { get; set; }
    }
}
