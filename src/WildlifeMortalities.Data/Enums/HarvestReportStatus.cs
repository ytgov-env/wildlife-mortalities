namespace WildlifeMortalities.Data.Enums;

public enum HarvestReportStatus
{
    Uninitialized = 0,
    Complete = 1,
    CompleteWithViolations = 2,
    WaitingOnClient = 3,
    WaitingOnValidSeal = 4
}
