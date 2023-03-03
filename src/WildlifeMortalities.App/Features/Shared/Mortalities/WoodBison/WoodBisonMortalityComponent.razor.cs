using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace WildlifeMortalities.App.Features.Shared.Mortalities.WoodBison
{
    public partial class WoodBisonMortalityComponent : IDisposable
    {
        [CascadingParameter]
        public EditContext EditContext { get; set; } = null!;

        [Parameter]
        public WoodBisonMortalityViewModel ViewModel { get; set; } = default!;

        public void Dispose()
        {
            if (EditContext != null)
            {
                EditContext.OnFieldChanged -= EditContext_OnFieldChanged;
            }
        }

        protected override void OnInitialized()
        {
            EditContext.OnFieldChanged += EditContext_OnFieldChanged;
        }

        private void EditContext_OnFieldChanged(object? sender, FieldChangedEventArgs e)
        {
            if (e.FieldIdentifier.FieldName == nameof(MortalityViewModel.Sex))
            {
                if (ViewModel.Sex != Data.Enums.Sex.Female)
                {
                    ViewModel.PregnancyStatus = null;
                }

                StateHasChanged();
            }
        }
    }
}
