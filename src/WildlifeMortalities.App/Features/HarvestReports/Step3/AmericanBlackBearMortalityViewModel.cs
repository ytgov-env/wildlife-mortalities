using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reporters;

namespace WildlifeMortalities.App.Features.HarvestReports;

public class AmericanBlackBearMortalityViewModel : MortalityViewModel
{
    public bool IsShotInConflict { get; set; }

    public AmericanBlackBearMortalityViewModel() : base(Data.Enums.AllSpecies.AmericanBlackBear)
    {

    }

    public override Mortality GetMortality(int reporterId)
    {
        var mortality = new AmericanBlackBearMortality
        {
            IsShotInConflict = IsShotInConflict,

        };

        SetBaseValues(mortality, reporterId);

        return mortality;

    }

}
