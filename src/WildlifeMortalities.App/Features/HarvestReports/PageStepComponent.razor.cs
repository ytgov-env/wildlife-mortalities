using Microsoft.AspNetCore.Components;

namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class PageStepComponent
{
    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    [Parameter]
    public int StepNumber { get; set; }

    [Parameter]
    public bool IsValid { get; set; }

    private bool CanGoBackwards()
    {
        return StepNumber >= 2;
    }

    private bool CanGoForward()
    {
        return IsValid;
    }

    [Parameter]
    public EventCallback OnNextClicked { get; set; }

    [Parameter]
    public EventCallback OnPreviousClicked { get; set; }
}
