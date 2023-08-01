using Microsoft.AspNetCore.Components;
using MudBlazor;
using WildlifeMortalities.App.Features.Reports;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public partial class MortalityComponent
{
    [CascadingParameter(Name = Constants.CascadingValues.ReportViewModel)]
    public MortalityReportViewModel ReportViewModel { get; set; } = null!;

    [Parameter]
    public bool DisableSpeciesSelection { get; set; }

    [Parameter]
    [EditorRequired]
    public MortalityWithSpeciesSelectionViewModel ViewModel { get; set; } = null!;

    [Parameter]
    public EventCallback SpeciesChanged { get; set; }

    public MortalityViewModel GetViewModel() => ViewModel.MortalityViewModel;

    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    private void SpeciesHasChanged(Species? species)
    {
        if (!species.HasValue)
        {
            return;
        }

        if (ViewModel.Species == species)
        {
            return;
        }

        var id = ViewModel.MortalityViewModel?.Id;

        if (id.HasValue && species.HasValue)
        {
            ReportViewModel.SpeciesChanged(id.Value, species.Value);
        }
        else
        {
            ViewModel.Species = species;
            ViewModel.MortalityViewModel = MortalityViewModel.Create(
                species.Value,
                ViewModel.MortalityViewModel
            );
        }

        SpeciesChanged.InvokeAsync();
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
