using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.App.Features.HarvestReports;

public class AmericanBlackBearMortalityViewModel : MortalityViewModel
{
    public bool IsShotInConflict { get; set; }

    public AmericanBlackBearMortalityViewModel() : base(Data.Enums.AllSpecies.AmericanBlackBear)
    {

    }

    public override Mortality GetMortality()
    {
        var mortality = new AmericanBlackBearMortality
        {
            IsShotInConflict = IsShotInConflict,

        };

        SetBaseValues(mortality);

        return mortality;

    }

}
