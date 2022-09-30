using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace WildlifeMortalities.App.Features.MortalityReports;

public partial class BaseMortalityComponent : IDisposable
{
    [Parameter]
    public MortalityViewModel ViewModel { get; set; } = null!;

    private EditContext _context;

    [CascadingParameter]
    public EditContext Context { get; set; }

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

    public void Dispose()
    {
        if (_context != null)
        {
            _context.OnFieldChanged -= Context_OnFieldChanged;
        }
    }
}
