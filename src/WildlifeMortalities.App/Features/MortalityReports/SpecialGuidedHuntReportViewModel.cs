using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class SpecialGuidedHuntReportViewModel
{
    public List<HuntedMortalityReportViewModel> HuntedMortalityReportViewModels { get; set; } =
        new List<HuntedMortalityReportViewModel>();
}
