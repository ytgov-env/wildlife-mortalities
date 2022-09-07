using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace WildlifeMortalities.App.Features.HarvestReports;

public abstract class ReportTypeComponent<T> : ComponentBase, IDisposable where T : new()
{
    protected T _viewModel;

    protected EditContext _context = null!;

    protected override void OnInitialized()
    {
        _viewModel = new();
        _context = new EditContext(_viewModel);
        _context.OnFieldChanged += _context_OnFieldChanged;
    }

    protected virtual void FieldsChanged() { }

    private void _context_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _context.Validate();
        ValidationChanged.InvokeAsync(!_context.GetValidationMessages().Any());
        FieldsChanged();
        //if (_selectHarvestReportTypeViewModel.HarvestReportType.HasValue == true)
        //{
        //    HarvestReportTypeChanged.InvokeAsync(_selectHarvestReportTypeViewModel.HarvestReportType.Value);
        //}
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
}
