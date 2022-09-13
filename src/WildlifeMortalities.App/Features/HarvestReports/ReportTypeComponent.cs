using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace WildlifeMortalities.App.Features.HarvestReports;

public abstract class ReportTypeComponent<T> : ComponentBase, IDisposable where T : new()
{
    protected T ViewModel { get; private set; }

    protected EditContext _context = null!;

    protected override void OnInitialized()
    {
        ViewModel = new();
        _context = new EditContext(ViewModel);
        _context.OnFieldChanged += _context_OnFieldChanged;

        ViewModelChanged.InvokeAsync(ViewModel);
    }

    protected void SetViewModel(T viewModel)
    {
        if (_context != null)
        {
            _context.OnFieldChanged -= _context_OnFieldChanged;
        }

        ViewModel = viewModel;
        _context = new EditContext(viewModel);

        _context.OnFieldChanged += _context_OnFieldChanged;

        ViewModelChanged.InvokeAsync(ViewModel);
    }

    protected virtual void FieldsChanged() { }

    private void _context_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _context.Validate();
        ValidationChanged.InvokeAsync(_context.GetValidationMessages().Any() == false);
        FieldsChanged();
    }

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
    public EventCallback<T> ViewModelChanged { get; set; }
}
