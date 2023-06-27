using System;
using System.Diagnostics;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Rules.Authorizations;
using Xunit;

namespace WildlifeMortalities.Test.Rules.Authorizations;

public class GenericTester
{
    #region DummyClasses

    public class DummyAuthorization : Authorization
    {
        public override string GetAuthorizationType()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateInternal(Authorization authorization)
        {
            throw new NotImplementedException();
        }
    }

    public class DummyAuthorizationRulePipelineItemReturnTrueAndUseAuthorization
        : AuthorizationRulePipelineItem
    {
        public override Task<bool> Process(
            Report report,
            AuthorizationRulePipelineContext pipelineContext
        )
        {
            pipelineContext.RelevantAuthorizations.Add(new DummyAuthorization());

            return Task.FromResult(true);
        }
    }

    public class DummyAuthorizationRulePipelineItemReturnTrueAndNoAuthorization
        : AuthorizationRulePipelineItem
    {
        public override Task<bool> Process(
            Report report,
            AuthorizationRulePipelineContext pipelineContext
        )
        {
            return Task.FromResult(true);
        }
    }

    public class DummyAuthorizationRulePipelineItemReturnTrueHasViolation
        : AuthorizationRulePipelineItem
    {
        public override Task<bool> Process(
            Report report,
            AuthorizationRulePipelineContext pipelineContext
        )
        {
            pipelineContext.Violations.Add(
                new Violation(
                    new HuntedActivity(),
                    Violation.RuleType.Authorization,
                    Violation.SeverityType.Illegal,
                    "test"
                )
            );

            return Task.FromResult(true);
        }
    }

    public class DummyAuthorizationRulePipelineItemReturnFalse : AuthorizationRulePipelineItem
    {
        public override Task<bool> Process(
            Report report,
            AuthorizationRulePipelineContext pipelineContext
        )
        {
            return Task.FromResult(false);
        }
    }

    public class DummyAuthorizationRulePipelineItemThrowsException : AuthorizationRulePipelineItem
    {
        public override Task<bool> Process(
            Report report,
            AuthorizationRulePipelineContext pipelineContext
        )
        {
            throw new Exception();
        }
    }

    public class DummyAuthorizationRulePipeline : AuthorizationRulePipeline
    {
        public DummyAuthorizationRulePipeline(
            AuthorizationRulePipelineContext context,
            params AuthorizationRulePipelineItem[] rules
        )
            : base(context)
        {
            Items = rules;
        }
    }

    #endregion

    [Fact]
    public async Task AuthorizationRule_WithNoViolationsAndUsedAuthorizations_IsLegal()
    {
        var context = new AppDbContext();

        var report = new IndividualHuntedMortalityReport();

        var authorizationRule = new AuthorizationRule(
            x =>
                new DummyAuthorizationRulePipeline(
                    x,
                    new DummyAuthorizationRulePipelineItemReturnTrueAndUseAuthorization()
                )
        );

        var result = await authorizationRule.Process(report, context);

        result.Violations.Should().BeEmpty();
        result.IsApplicable.Should().BeTrue();
    }

    [Fact]
    public async Task AuthorizationRule_WithNoViolationsAndNoAuthorization_IsNotApplicable()
    {
        var context = new AppDbContext();

        var report = new IndividualHuntedMortalityReport();

        var authorizationRule = new AuthorizationRule(
            x =>
                new DummyAuthorizationRulePipeline(
                    x,
                    new DummyAuthorizationRulePipelineItemReturnTrueAndNoAuthorization()
                )
        );

        var result = await authorizationRule.Process(report, context);

        result.Violations.Should().BeEmpty();
        result.IsApplicable.Should().BeFalse();
    }

    [Fact]
    public async Task AuthorizationRule_WithViolation_IsIllegal()
    {
        var context = new AppDbContext();

        var report = new IndividualHuntedMortalityReport();

        var authorizationRule = new AuthorizationRule(
            x =>
                new DummyAuthorizationRulePipeline(
                    x,
                    new DummyAuthorizationRulePipelineItemReturnTrueHasViolation()
                )
        );

        var result = await authorizationRule.Process(report, context);

        result.Violations.Should().ContainSingle();
    }

    [Fact]
    public async Task AuthorizationRule_WithMultipleViolations_IsIllegal()
    {
        var context = new AppDbContext();

        var report = new IndividualHuntedMortalityReport();

        var authorizationRule = new AuthorizationRule(
            x =>
                new DummyAuthorizationRulePipeline(
                    x,
                    new DummyAuthorizationRulePipelineItemReturnTrueHasViolation(),
                    new DummyAuthorizationRulePipelineItemReturnTrueHasViolation()
                )
        );

        var result = await authorizationRule.Process(report, context);

        result.Violations.Should().HaveCount(2);
    }

    [Fact]
    public async Task AuthorizationRule_PipelineStopsOnFalse_IsNotApplicable()
    {
        var context = new AppDbContext();

        var report = new IndividualHuntedMortalityReport();

        var authorizationRule = new AuthorizationRule(
            x =>
                new DummyAuthorizationRulePipeline(
                    x,
                    new DummyAuthorizationRulePipelineItemReturnFalse(),
                    new DummyAuthorizationRulePipelineItemThrowsException()
                )
        );

        var result = await authorizationRule.Process(report, context);

        result.Violations.Should().BeEmpty();
        result.IsApplicable.Should().BeFalse();
    }
}
