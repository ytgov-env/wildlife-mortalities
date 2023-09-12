using System.ComponentModel.DataAnnotations;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.App.Features.Reports;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class ReportTypeAttribute : Attribute
{
    public ReportTypeAttribute(Type reportType)
    {
        ReportType = reportType;
    }

    public Type ReportType { get; }
}

[AttributeUsage(AttributeTargets.Field)]
public class IsCreatable : Attribute { }

public enum ReportType
{
    [Display(Name = "Collared report")]
    [ReportType(typeof(CollaredMortalityReport))]
    CollaredMortalityReport = 10,

    [Display(Name = "Human-wildlife conflict report")]
    [ReportType(typeof(HumanWildlifeConflictMortalityReport))]
    HumanWildlifeConflictMortalityReport,

    [Display(Name = "Resident hunt report")]
    [IsCreatable]
    [ReportType(typeof(IndividualHuntedMortalityReport))]
    IndividualHuntedMortalityReport,

    [Display(Name = "Outfitter guided hunt report")]
    [IsCreatable]
    [ReportType(typeof(OutfitterGuidedHuntReport))]
    OutfitterGuidedHuntReport,

    [Display(Name = "Research report")]
    [ReportType(typeof(ResearchMortalityReport))]
    ResearchMortalityReport,

    [Display(Name = "Special guided hunt report")]
    [IsCreatable]
    [ReportType(typeof(SpecialGuidedHuntReport))]
    SpecialGuidedHuntReport,

    [Display(Name = "Trapping report")]
    // Todo: re-enable trapping after it's complete
    //[IsCreatable]
    [ReportType(typeof(TrappedMortalitiesReport))]
    TrappedMortalitiesReport,
}
