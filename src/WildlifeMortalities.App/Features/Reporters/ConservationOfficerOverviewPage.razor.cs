using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WildlifeMortalities.Data.Entities.Reporters;

namespace WildlifeMortalities.App.Features.Reporters
{
    public partial class ConservationOfficerOverviewPage : IDisposable
    {
        private EditContext _context = null!;

        protected override void OnInitialized()
        {
            _selectedConservationOfficerViewModel = new();
            _context = new EditContext(_selectedConservationOfficerViewModel);
            _context.OnFieldChanged += _context_OnFieldChanged;
        }

        private void _context_OnFieldChanged(object? sender, FieldChangedEventArgs e)
        {
            _context.Validate();
            ValidationChanged.InvokeAsync(!_context.GetValidationMessages().Any());
        }

        private SelectConservationOfficerViewModel _selectedConservationOfficerViewModel = null!;

        private async Task<IEnumerable<ConservationOfficer>> SearchConservationOfficerByBadgeNumberOrLastName(
        string input
    ) => throw new NotImplementedException();

        public void Dispose()
        {
            if (_context is not null)
            {
                _context.OnFieldChanged -= _context_OnFieldChanged;
            }
        }

        [Parameter]
        public EventCallback<bool> ValidationChanged { get; set; }

        [Parameter]
        public int Id { get; set; }
    }
}
