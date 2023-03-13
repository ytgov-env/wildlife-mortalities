using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities.Authorizations
{
    public interface IHasOutfitterAreas
    {
        public List<OutfitterArea> OutfitterAreas { get; set; }
    }
}
