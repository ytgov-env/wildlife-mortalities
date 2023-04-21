using Microsoft.AspNetCore.Components;
using WildlifeMortalities.App.Features.Shared;
using MudBlazor;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.App.Features.Reports.Activities;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.App.Features.Reports
{
    public partial class SpecialGuidedHuntReportComponent : DbContextAwareComponent
    {
        [Parameter]
        [EditorRequired]
        public SpecialGuidedHuntReportViewModel ViewModel { get; set; } = null!;

        [CascadingParameter(Name = Constants.CascadingValues.ReportType)]
        public ReportType ReportType { get; set; }

        [CascadingParameter(Name = Constants.CascadingValues.EditMode)]
        public bool EditMode { get; set; } = false;

        [Inject]
        private IDialogService DialogService { get; set; } = null!;

        private async Task Add()
        {
            var parameters = new DialogParameters
            {
                [nameof(AddActivityDialog.ReportType)] = ReportType
            };
            var dialog = DialogService.Show<AddActivityDialog>("", parameters);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                ViewModel.HuntedActivityViewModels.Add(result.Data as HuntedActivityViewModel);
            }
        }

        private async Task Edit(HuntedActivityViewModel viewModel)
        {
            var parameters = new DialogParameters
            {
                [nameof(EditActivityDialog.ViewModel)] = viewModel,
                [nameof(EditActivityDialog.ReportType)] = ReportType
            };
            var dialog = DialogService.Show<EditActivityDialog>("", parameters);
            var result = await dialog.Result;
        }

        private void Delete(HuntedActivityViewModel viewModel)
        {
            ViewModel.HuntedActivityViewModels.Remove(viewModel);
        }

        private async Task<IEnumerable<Client>> SearchClientByEnvClientIdOrLastName(
            string searchTerm
        ) =>
            await Context.People
                .OfType<Client>()
                .SearchByEnvClientIdOrLastName(searchTerm)
                .ToArrayAsync();
    }
}
