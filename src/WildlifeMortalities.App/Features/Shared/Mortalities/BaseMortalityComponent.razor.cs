using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public partial class BaseMortalityComponent : IDisposable
{
    private EditContext? _context;

    [Parameter] public MortalityViewModel ViewModel { get; set; } = default!;

    [CascadingParameter] public EditContext Context { get; set; } = default!;

    public void Dispose()
    {
        if (_context != null)
        {
            _context.OnFieldChanged -= Context_OnFieldChanged;
        }
    }

    protected override void OnParametersSet()
    {
        if (_context != null)
        {
            _context.OnFieldChanged -= Context_OnFieldChanged;
        }

        Context.OnFieldChanged += Context_OnFieldChanged;
        _context = Context;
    }

    private void Context_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        // TODO Causing stackoverflow exception
        //if (
        //    e.FieldIdentifier.Model == ViewModel
        //    && e.FieldIdentifier.FieldName == nameof(MortalityViewModel.Longitude)
        //)
        //{
        //    Context.NotifyFieldChanged(
        //        new FieldIdentifier(ViewModel, nameof(MortalityViewModel.Latitude))
        //    );
        //}
        //else if (
        //    e.FieldIdentifier.Model == ViewModel
        //    && e.FieldIdentifier.FieldName == nameof(MortalityViewModel.Latitude)
        //)
        //{
        //    Context.NotifyFieldChanged(
        //        new FieldIdentifier(ViewModel, nameof(MortalityViewModel.Longitude))
        //    );
        //}
    }
}
