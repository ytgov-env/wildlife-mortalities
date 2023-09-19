namespace WildlifeMortalities.Shared;

public static class Constants
{
    public static class AppConfigurationService
    {
        public const string LastSuccessfulClientsSyncKey =
            "PosseSyncService.LastSuccessfulClientsSync";
        public const string LastSuccessfulAuthorizationsSyncKey =
            "PosseSyncService.LastSuccessfulAuthorizationsSync";
    }

    public static class FormatStrings
    {
        public const string StandardDateFormat = "MMM d, yyyy";
        public const string StandardDateFormatWithoutYear = "MMM d";
    }
}
