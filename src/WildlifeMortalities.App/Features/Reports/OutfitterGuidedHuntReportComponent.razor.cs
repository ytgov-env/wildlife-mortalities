using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.App.Features.Reports.Activities;
using WildlifeMortalities.App.Features.Shared;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reports
{
    public partial class OutfitterGuidedHuntReportComponent : DbContextAwareComponent
    {
        private const int _maxAssistantGuides = 2;
        private IEnumerable<OutfitterArea> _outfitterAreas = Array.Empty<OutfitterArea>();

        [Parameter]
        [EditorRequired]
        public OutfitterGuidedHuntReportViewModel ViewModel { get; set; } = null!;

        [CascadingParameter(Name = Constants.CascadingValues.ReportType)]
        public ReportType ReportType { get; set; }

        [CascadingParameter(Name = Constants.CascadingValues.EditMode)]
        public bool EditMode { get; set; } = false;

        [CascadingParameter]
        public EditContext EditContext { get; set; } = null!;

        [Inject]
        private IDialogService DialogService { get; set; } = null!;

        [Inject]
        private ISnackbar SnackBar { get; set; } = null!;

        [Inject]
        private IMortalityService MortalityService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            _outfitterAreas = await MortalityService.GetOutfitterAreas();
        }

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

            if (EditContext.GetValidationMessages().Any())
            {
                EditContext.Validate();
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
            await dialog.Result;
            if (EditContext.GetValidationMessages().Any())
            {
                EditContext.NotifyFieldChanged(
                    FieldIdentifier.Create(() => ViewModel.HuntingDateRange)
                );
            }
        }

        private void Delete(HuntedActivityViewModel viewModel)
        {
            ViewModel.HuntedActivityViewModels.Remove(viewModel);
        }

        private async Task<IEnumerable<Client>> SearchClientByEnvClientIdOrLastName(
            string searchTerm
        )
        {
            using var context = GetContext();

            return await context.People
                .OfType<Client>()
                .SearchByEnvClientIdOrLastName(searchTerm)
                .ToArrayAsync();
        }

        private async Task<IEnumerable<OutfitterArea>> SearchOutfitterAreas(string input)
        {
            if (string.IsNullOrEmpty(input))
                return _outfitterAreas;
            return await Task.FromResult(_outfitterAreas.Where(y => y.Area.StartsWith(input)));
        }
    }
}
