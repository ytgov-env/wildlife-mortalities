using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Reflection;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using FluentValidation;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Reports;

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

        public bool IsDisabled
        {
            get =>
                PropertyInfo.GetCustomAttribute<IsPrerequisiteOrganicMaterialForBioSubmissionAnalysisAttribute>()
                    != null
                && BioSubmission.AnalysisStatus == BioSubmissionAnalysisStatus.Complete;
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
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public BioSubmission BioSubmission { get; set; } = null!;

    [Parameter, EditorRequired]
    public Report Report { get; set; } = null!;

    protected override void OnInitialized()
    {
        _viewModel = new(BioSubmission, Report);
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
