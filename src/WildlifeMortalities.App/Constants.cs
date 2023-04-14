﻿namespace WildlifeMortalities.App;

public static class Constants
{
    public static class CascadingValues
    {
        public const string EditMode = "EditMode";
        public const string ReportType = "ReportType";
        public const string HasAttemptedFormSubmission = "HasAttemptedFormSubmission";
    }

    public static class Routes
    {
        public const string ReportDetailsPage =
            "/mortality-reports/{humanReadablePersonId}/{reportId:int}";

        public static string GetReportDetailsPageLink(string humanReadablePersonId, int reportId) =>
            $"/mortality-reports/{humanReadablePersonId}/{reportId}";

        public const string CreateReportPage = "/mortality-reports/{humanReadablePersonId}/new";

        public static string GetCreateReportPageLink(string humanReadablePersonId) =>
            $"/mortality-reports/{humanReadablePersonId}/new";

        public const string EditReportPage =
            "/mortality-reports/{humanReadablePersonId}/edit/{reportId:int}";

        public static string GetEditReportPageLink(string humanReadablePersonId, int reportId) =>
            $"/mortality-reports/{humanReadablePersonId}/edit/{reportId}";

        public const string EditDraftReportPage =
            "/mortality-reports/{humanReadablePersonId}/editdraft/{draftId:int}";

        public static string GetEditDraftReportPageLink(
            string humanReadablePersonId,
            int reportId
        ) => $"/mortality-reports/{humanReadablePersonId}/editdraft/{reportId}";

        public const string ClientOverviewPage = "/clients/{envClientId}";

        public static string GetClientOverviewPageLink(string envClientId) =>
            $"reporters/clients/{envClientId}";
    }
}
