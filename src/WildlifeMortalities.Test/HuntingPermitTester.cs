using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.MortalityReports;

namespace WildlifeMortalities.Test;

public class HuntingPermitTester
{
    public class DummyHuntedMortalityReport : HuntedMortalityReport
    {
        public DummyHuntedMortalityReport()
        {

        }
    }

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
        var dummyReport = new DummyHuntedMortalityReport();
        dummyReport.Mortality = new AmericanBeaverMortality();

        var permit = new HuntingPermit();
        permit.Type = HuntingPermit.PermitType.ElkExclusion;

        var result = permit.GetResult(dummyReport);

        result.IsApplicable.Should().BeFalse();
    }

    [Fact]
    public void GetResult_WithEltTypeAndElkMortalityInCorrectGameArea_ShouldReturnAllowedResult()
    {
        var dummyReport = new DummyHuntedMortalityReport();
        dummyReport.Mortality = new ElkMortality();
        dummyReport.GameManagementArea = new GameManagementArea { Subzone = "11" };

        var permit = new HuntingPermit();
        permit.Type = HuntingPermit.PermitType.ElkExclusion;

        var result = permit.GetResult(dummyReport);

        result.IsApplicable.Should().BeTrue();
        result.HasViolations.Should().BeFalse();
    }

}
