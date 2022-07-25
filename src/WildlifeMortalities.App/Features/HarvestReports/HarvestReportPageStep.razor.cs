using Microsoft.AspNetCore.Components;

namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class HarvestReportPageStep
{
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public Int32 StepNumber { get; set; }

    [Parameter]
    public Boolean IsValid { get; set; }

    private Boolean CanGoBackwards()
    {
        return StepNumber >= 2;
    }

    private Boolean CanGoForward()
    {
        return IsValid == true;
    }

    [Parameter]
    public EventCallback OnNextClicked { get; set; }
}
