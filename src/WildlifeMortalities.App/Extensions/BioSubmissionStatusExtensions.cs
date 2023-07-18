using System.Diagnostics;
using MudBlazor;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.App.Extensions;

public static class BioSubmissionStatusExtensions
{
    public static Color GetChipColor(this BioSubmissionRequiredOrganicMaterialsStatus status)
    {
        return status switch
        {
            BioSubmissionRequiredOrganicMaterialsStatus.NotStarted => Color.Warning,
            BioSubmissionRequiredOrganicMaterialsStatus.PartiallySubmitted => Color.Error,
            BioSubmissionRequiredOrganicMaterialsStatus.DidNotSubmit => Color.Error,
            BioSubmissionRequiredOrganicMaterialsStatus.Submitted => Color.Success,
            _ => throw new UnreachableException()
        };
    }

    public static Color GetChipColor(this BioSubmissionAnalysisStatus status)
    {
        return status switch
        {
            BioSubmissionAnalysisStatus.NotApplicable => Color.Default,
            BioSubmissionAnalysisStatus.NotStarted => Color.Warning,
            BioSubmissionAnalysisStatus.Complete => Color.Success,
            _ => throw new UnreachableException()
        };
    }
}
