using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class OutfitterGuidedHuntReportViewModel
{
    public List<HuntedMortalityReportViewModel> HuntedMortalityReportViewModels { get; set; } =
        new List<HuntedMortalityReportViewModel>();
}
