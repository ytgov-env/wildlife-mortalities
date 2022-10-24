using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace WildlifeMortalities.App.Features.MortalityReports;

public abstract class ReportTypeComponent<T> : ComponentBase, IDisposable where T : new()
{
    protected EditContext Context = null!;
    protected T ViewModel { get; private set; }

    [Parameter] public EventCallback<bool> ValidationChanged { get; set; }

    [Parameter] public EventCallback<T> ViewModelChanged { get; set; }

    public void Dispose()
    {
        if (Context is not null)
        {
            Context.OnFieldChanged -= _context_OnFieldChanged;
        }
    }

    protected override void OnInitialized()
    {
        ViewModel ??= new T();

        Context = new EditContext(ViewModel);
        Context.OnFieldChanged += _context_OnFieldChanged;

        ViewModelChanged.InvokeAsync(ViewModel);
    }

    protected void SetViewModel(T viewModel, bool fireEvent)
    {
        if (Context is not null)
        {
            Context.OnFieldChanged -= _context_OnFieldChanged;
        }

        ViewModel = viewModel;
        Context = new EditContext(viewModel);

        Context.OnFieldChanged += _context_OnFieldChanged;

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
        Context.Validate();
        ValidationChanged.InvokeAsync(Context.GetValidationMessages().Any() == false);
        FieldsChanged();
    }
}
