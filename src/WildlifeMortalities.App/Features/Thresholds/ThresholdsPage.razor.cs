using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.App.Features.Shared;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Extensions;

namespace WildlifeMortalities.App.Features.Thresholds;

public partial class ThresholdsPage : DbContextAwareComponent
{
    private string? _selectedSeason;
    private HarvestType _selectedHarvestType;
    private bool _showOnlyOpen;

    public IEnumerable<BagLimitEntry> BagLimitEntries { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        using var context = GetContext();

        BagLimitEntries = await context.BagLimitEntries
            .Include(x => ((HuntingBagLimitEntry)x).Season)
            .Include(x => ((TrappingBagLimitEntry)x).Season)
            .Include(x => ((HuntingBagLimitEntry)x).Areas)
            .Include(x => ((TrappingBagLimitEntry)x).Concessions)
            .Include(x => x.ActivityQueue)
            .ThenInclude(x => x.Activity)
            .ThenInclude(x => ((HuntedActivity)x).IndividualHuntedMortalityReport)
            .ThenInclude(x => x.Person)
            .Include(x => x.ActivityQueue)
            .ThenInclude(x => x.Activity)
            .ThenInclude(x => ((HuntedActivity)x).SpecialGuidedHuntReport)
            .ThenInclude(x => x.Client)
            .Include(x => x.ActivityQueue)
            .ThenInclude(x => x.Activity)
            .ThenInclude(x => ((HuntedActivity)x).OutfitterGuidedHuntReport)
            .ThenInclude(x => x.Client)
            .Include(x => x.ActivityQueue)
            .ThenInclude(x => x.Activity)
            .ThenInclude(x => x.Mortality)
            .Where(x => x.MaxValueForThreshold != null)
            .AsSplitQuery()
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

    private string GetAreas(BagLimitEntry bagLimitEntry)
    {
        if (bagLimitEntry is HuntingBagLimitEntry huntingBagLimitEntry)
        {
            return huntingBagLimitEntry.Areas.AreasToString();
        }
        else if (bagLimitEntry is TrappingBagLimitEntry trappingBagLimitEntry)
        {
            return trappingBagLimitEntry.Concessions.ConcessionsToString();
        }
        else
        {
            throw new Exception();
        }
    }

    private enum HarvestType
    {
        All,
        Hunting,
        Trapping
    }
}
