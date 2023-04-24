using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Reflection;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using FluentValidation;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public partial class OrganicMaterialForBioSubmissionDialog
{
    public class BioSubmissionMaterialCheckbox
    {
        public string Label { get; }

        public bool? IsChecked
        {
            get => (bool?)PropertyInfo.GetValue(BioSubmission);
            set => PropertyInfo.SetValue(BioSubmission, value);
        }

        public PropertyInfo PropertyInfo { get; }

        public BioSubmission BioSubmission { get; }

        public BioSubmissionMaterialCheckbox(
            string label,
            PropertyInfo propertyInfo,
            BioSubmission bioSubmission
        )
        {
            Label = label;
            PropertyInfo = propertyInfo;
            BioSubmission = bioSubmission;
        }
    }

    private OrganicMaterialForBioSubmissionDialogViewModel _viewModel = null!;
    private BioSubmissionDialogResult _result = BioSubmissionDialogResult.Cancel;

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public BioSubmission BioSubmission { get; set; } = null!;

    protected override void OnInitialized()
    {
        _viewModel = new(BioSubmission);
        base.OnInitialized();
    }

    private void CloseDialog()
    {
        MudDialog.Close(DialogResult.Ok(_result));
    }

    private void Cancel()
    {
        _result = BioSubmissionDialogResult.Cancel;
        CloseDialog();
    }
}

public enum BioSubmissionDialogResult
{
    Cancel,
    SubmitAndClose,
    SubmitAndProceedToAnalysis
}
