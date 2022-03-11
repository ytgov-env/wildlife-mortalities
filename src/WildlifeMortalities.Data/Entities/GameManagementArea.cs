using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities
{
    public class GameManagementArea
    {
        public int Id { get; set; }
        public int Zone { get; set; }
        public int Subzone { get; set; }
        public int ZoneSubzone { get; }
    }
}
