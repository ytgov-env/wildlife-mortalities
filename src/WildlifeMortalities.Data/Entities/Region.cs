using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GameManagementArea> GameManagementAreas { get; set; }
    }
}
