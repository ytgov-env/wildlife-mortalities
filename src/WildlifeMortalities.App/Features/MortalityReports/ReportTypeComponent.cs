using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace WildlifeMortalities.App.Features.MortalityReports;

public abstract class ReportTypeComponent<T> : ComponentBase, IDisposable where T : new()
{
    protected EditContext _context = null!;
    protected T ViewModel { get; private set; }

    [Parameter] public EventCallback<bool> ValidationChanged { get; set; }

    [Parameter] public EventCallback<T> ViewModelChanged { get; set; }

    public void Dispose()
    {
        if (_context is not null)
        {
            _context.OnFieldChanged -= _context_OnFieldChanged;
        }
    }

    protected override void OnInitialized()
    {
        ViewModel ??= new T();

        _context = new EditContext(ViewModel);
        _context.OnFieldChanged += _context_OnFieldChanged;

        ViewModelChanged.InvokeAsync(ViewModel);
    }

    protected void SetViewModel(T viewModel, bool fireEvent)
    {
        if (_context is not null)
        {
            _context.OnFieldChanged -= _context_OnFieldChanged;
        }

        ViewModel = viewModel;
        _context = new EditContext(viewModel);

        _context.OnFieldChanged += _context_OnFieldChanged;

        if (fireEvent)
        {
            ViewModelChanged.InvokeAsync(ViewModel);
        }
    }

    protected virtual void FieldsChanged()
    {
    }

    private void _context_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _context.Validate();
        ValidationChanged.InvokeAsync(_context.GetValidationMessages().Any() == false);
        FieldsChanged();
    }
}
