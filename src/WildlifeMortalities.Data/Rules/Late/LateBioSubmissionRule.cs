using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Rules.Late;

public class LateBioSubmissionRule : Rule
{
    public override async Task<RuleResult> Process(Report report, AppDbContext context)
    {
        throw new NotImplementedException();
    }

    private static bool BioSubmissionMoreThanFifteenDaysAfterTheEndOfTheMonthInWhichTheAnimalWasKilled(
        BioSubmission bioSubmission,
        Mortality mortality
    )
    {
        //if (bioSubmission.DateSubmitted.HasValue)
        //{
        //    return OccuredMoreThanFifteenDaysAfterTheEndOfTheMonthInWhichTheAnimalWasKilled(
        //        bioSubmission.DateSubmitted.Value,
        //        mortality
        //    );
        //}

        throw new NotImplementedException();
    }
}
