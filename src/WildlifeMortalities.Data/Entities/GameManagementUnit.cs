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
        public int GameManagementAreaSpeciesId { get; set; }
        public GameManagementAreaSpecies GameManagementAreaSpecies { get; set; }
        //Remove?? GMU groups bears
        public GmuSpecies Species { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
    }
}
