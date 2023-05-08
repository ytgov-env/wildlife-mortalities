using Microsoft.AspNetCore.Components;
using MudBlazor;
using WildlifeMortalities.App.Features.Reports;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public partial class MortalityComponent
{
    [CascadingParameter(Name = Constants.CascadingValues.ReportType)]
    public ReportType ReportType { get; set; }

    [Parameter]
    public bool DisableSpeciesSelection { get; set; }

    [Parameter]
    [EditorRequired]
    public MortalityWithSpeciesSelectionViewModel ViewModel { get; set; } = null!;

    public MortalityViewModel GetViewModel() => ViewModel.MortalityViewModel;

    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    private void SpeciesChanged(Species? value)
    {
        if (!value.HasValue)
        {
            return;
        }

        if (ViewModel.Species == value)
        {
            return;
        }

        ViewModel.Species = value;
        ViewModel.MortalityViewModel = MortalityViewModel.Create(
            value.Value,
            ViewModel.MortalityViewModel
        );
    }

    private async Task ChangeSpeciesClicked()
    {
        var message = ViewModel.MortalityViewModel switch
        {
            { Id: null }
            or {
                Id: not null,
                BioSubmission.RequiredOrganicMaterialsStatus: Data.Entities
                    .BiologicalSubmissions
                    .BioSubmissionRequiredOrganicMaterialsStatus
                    .NotStarted
            }
                => "This will reset all species specific mortality data. Do you want to continue?",
            _
                => "This will reset all species specific mortality data, and reset the bio submission. Do you want to continue?",
        };

        var result = await DialogService.ShowMessageBox(
            "Change species?",
            message,
            "Proceed",
            "Cancel"
        );

        if (result == true)
        {
            DisableSpeciesSelection = false;
        }
    }
}
