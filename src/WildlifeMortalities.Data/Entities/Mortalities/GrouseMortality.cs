using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class GrouseMortality : Mortality
{
    public override Species Species => Species.Grouse;
}
