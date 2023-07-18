using Microsoft.AspNetCore.Components;
using MudBlazor;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Extensions;
using WildlifeMortalities.App.Extensions;

namespace WildlifeMortalities.App.Features.Shared
{
    public partial class BioSubmissionStatusComponent
    {
        private List<BioSubmission> _bioSubmissions = new();
        private BioSubmissionRequiredOrganicMaterialsStatus _requiredOrganicMaterialsStatus;
        private BioSubmissionAnalysisStatus _analysisStatus;
        private bool _isLoading;

        [Parameter]
        [EditorRequired]
        public int ReportId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            var activityIds = await Context.Reports.GetActivityIds(ReportId);
            var mortalities = await Context.Mortalities
                .Where(x => activityIds.Contains(x.ActivityId))
                .ToArrayAsync();
            foreach (
                var mortality in mortalities
                    .OfType<IHasBioSubmission>()
                    .Where(x => x.SubTypeHasBioSubmission())
            )
            {
                var bioSubmission =
                    await Context.BioSubmissions.GetBioSubmissionFromMortality(mortality)
                    ?? throw new Exception(
                        "Expected mortality to have bio submission, but no bio submission found."
                    );
                _bioSubmissions.Add(bioSubmission);
            }

            if (_bioSubmissions.Count > 0)
            {
                _requiredOrganicMaterialsStatus = GetRequiredOrganicMaterialsStatus(
                    _bioSubmissions
                );
                _analysisStatus = GetBioSubmissionAnalysisStatus(_bioSubmissions);
            }

            _isLoading = false;
            Dispose();
        }

        private static BioSubmissionRequiredOrganicMaterialsStatus GetRequiredOrganicMaterialsStatus(
            IEnumerable<BioSubmission> bioSubmissions
        )
        {
            if (
                bioSubmissions.All(
                    x =>
                        x.RequiredOrganicMaterialsStatus
                        == BioSubmissionRequiredOrganicMaterialsStatus.Submitted
                )
            )
            {
                return BioSubmissionRequiredOrganicMaterialsStatus.Submitted;
            }
            else if (
                bioSubmissions.Any(
                    x =>
                        x.RequiredOrganicMaterialsStatus
                        == BioSubmissionRequiredOrganicMaterialsStatus.NotStarted
                )
            )
            {
                return BioSubmissionRequiredOrganicMaterialsStatus.NotStarted;
            }
            else if (
                bioSubmissions.Any(
                    x =>
                        x.RequiredOrganicMaterialsStatus
                        == BioSubmissionRequiredOrganicMaterialsStatus.PartiallySubmitted
                )
            )
            {
                return BioSubmissionRequiredOrganicMaterialsStatus.PartiallySubmitted;
            }
            else
            {
                return BioSubmissionRequiredOrganicMaterialsStatus.DidNotSubmit;
            }
        }

        private static BioSubmissionAnalysisStatus GetBioSubmissionAnalysisStatus(
            IEnumerable<BioSubmission> bioSubmissions
        )
        {
            if (
                bioSubmissions.All(
                    x => x.AnalysisStatus == BioSubmissionAnalysisStatus.NotApplicable
                )
            )
            {
                return BioSubmissionAnalysisStatus.NotApplicable;
            }
            else if (
                bioSubmissions.Any(x => x.AnalysisStatus == BioSubmissionAnalysisStatus.NotStarted)
            )
            {
                return BioSubmissionAnalysisStatus.NotStarted;
            }
            else
            {
                return BioSubmissionAnalysisStatus.Complete;
            }
        }
    }
}
