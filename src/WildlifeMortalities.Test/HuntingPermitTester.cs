using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Test;

public class HuntingPermitTester
{
    [Fact]
    public void GetResult_WithNonApplicableHuntedMortalityReport_ShouldReturnNotApplicableResult()
    {
        var permit = new HuntingPermit();
        var result = permit.GetResult(new HumanWildlifeConflictMortalityReport());

        result.IsApplicable.Should().BeFalse();
    }

    [Fact]
    public void GetResult_WithEltTypeAndNoneElkMortality_ShouldReturnNotApplicableResult()
    {
        var report = new IndividualHuntedMortalityReport { HuntedActivity = new HuntedActivity() };
        report.HuntedActivity.Mortality = new AmericanBeaverMortality();

        var permit = new HuntingPermit();
        permit.Type = HuntingPermit.PermitType.ElkExclusion;

        var result = permit.GetResult(report);

        result.IsApplicable.Should().BeFalse();
    }

    [Fact]
    public void GetResult_WithElkTypeAndElkMortalityInCorrectGameArea_ShouldReturnAllowedResult()
    {
        var report = new IndividualHuntedMortalityReport { HuntedActivity = new HuntedActivity() };
        report.HuntedActivity.Mortality = new ElkMortality();
        report.HuntedActivity.GameManagementArea = new GameManagementArea { Subzone = "11" };

        var permit = new HuntingPermit();
        permit.Type = HuntingPermit.PermitType.ElkExclusion;

        var result = permit.GetResult(report);

        result.IsApplicable.Should().BeTrue();
        result.HasViolations.Should().BeFalse();
    }
}
