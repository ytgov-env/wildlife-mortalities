using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.App.Features.Shared;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;

namespace WildlifeMortalities.App.Features.Home;

public partial class ThresholdComponent : DbContextAwareComponent
{
    public IEnumerable<BagLimitEntry> BagLimitEntries { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        using var context = GetContext();

        BagLimitEntries = await context.BagLimitEntries
            .Include(x => x.ActivityQueue)
            .ThenInclude(x => x.Activity)
            .ThenInclude(x => ((HuntedActivity)x).IndividualHuntedMortalityReport)
            .Include(x => x.ActivityQueue)
            .ThenInclude(x => x.Activity)
            .ThenInclude(x => ((HuntedActivity)x).SpecialGuidedHuntReport)
            .Include(x => x.ActivityQueue)
            .ThenInclude(x => x.Activity)
            .ThenInclude(x => ((HuntedActivity)x).OutfitterGuidedHuntReport)
            .Include(x => x.ActivityQueue)
            .ThenInclude(x => x.Activity)
            .ThenInclude(x => x.Mortality)
            .Where(x => x.MaxValueForThreshold != null)
            .ToListAsync();
        Console.WriteLine();
    }

    private Report GetReport(HuntedActivity huntedActivity)
    {
        if (huntedActivity.IndividualHuntedMortalityReport != null)
        {
            return huntedActivity.IndividualHuntedMortalityReport;
        }
        else if (huntedActivity.SpecialGuidedHuntReport != null)
        {
            return huntedActivity.SpecialGuidedHuntReport;
        }
        else if (huntedActivity.OutfitterGuidedHuntReport != null)
        {
            return huntedActivity.OutfitterGuidedHuntReport;
        }
        else
        {
            throw new Exception();
        }
    }
}
