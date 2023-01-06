// using WildlifeMortalities.Data.Entities.Authorizations;
// using WildlifeMortalities.Data.Entities.MortalityReports;
//
// namespace WildlifeMortalities.Test;
//
// public class AuthorizationTester
// {
//     private class DummyAuthorization : Authorization
//     {
//         private readonly bool _shouldBeApplicable;
//         private readonly bool _shouldHaveViolation;
//         public const string FailedResponse = "my failed response";
//
//         public DummyAuthorization(bool shouldBeApplicable, bool shouldHaveViolation)
//         {
//             _shouldBeApplicable = shouldBeApplicable;
//             _shouldHaveViolation = shouldHaveViolation;
//         }
//
//         public override AuthorizationResult GetResult(MortalityReport report) =>
//             _shouldBeApplicable == false
//                 ? AuthorizationResult.NotApplicable(this)
//                 : (_shouldHaveViolation == true
//                     ? AuthorizationResult.ApplicableWithViolations(this, FailedResponse)
//                     : AuthorizationResult.ApplicableWithNoViolations(this));
//     }
//
//     [Fact]
//     public void GetSummary_WithEmptyAuthorizations_ShouldReturnEmptyIEnumerable()
//     {
//         var summary = Authorization.GetSummary(Array.Empty<Authorization>(), null);
//         summary.ApplicableAuthorizationResults.Should().BeEmpty();
//     }
//
//     [Fact]
//     public void GetSummary_WithNoApplicableAuthorizations_ShouldReturnEmptyIEnumerable()
//     {
//         var firstRule = new DummyAuthorization(false, false);
//         var secondRule = new DummyAuthorization(false, true);
//
//         var summary = Authorization.GetSummary(new[] { firstRule, secondRule }, null);
//         summary.ApplicableAuthorizationResults.Should().BeEmpty();
//     }
//
//     [Theory]
//     [InlineData(true)]
//     [InlineData(false)]
//     public void GetSummary_WithSingleApplicableAuthorization_ShouldReturnSingleApplicableAuthorization(bool shouldHaveViolation)
//     {
//         var firstRule = new DummyAuthorization(false, false);
//         var secondRule = new DummyAuthorization(false, true);
//         var thirdRule = new DummyAuthorization(true, shouldHaveViolation);
//
//         var summary = Authorization.GetSummary(new[] { firstRule, secondRule, thirdRule }, null);
//
//         summary.ApplicableAuthorizationResults.Should().ContainSingle();
//         summary.ApplicableAuthorizationResults.First().Authorization.Should().Be(thirdRule);
//         summary.ApplicableAuthorizationResults.First().HasViolations.Should().Be(shouldHaveViolation);
//     }
//
//
//     [Fact]
//     public void GetSummary_WithMultipleApplicableAuthorizations_ShouldReturnMultipleApplicableAuthorizations()
//     {
//         var firstRule = new DummyAuthorization(false, false);
//         var secondRule = new DummyAuthorization(true, true);
//         var thirdRule = new DummyAuthorization(true, false);
//
//         var summary = Authorization.GetSummary(new[] { firstRule, secondRule, thirdRule }, null);
//
//         summary.ApplicableAuthorizationResults.Should().BeEquivalentTo(new[]
//         {
//             AuthorizationResult.ApplicableWithViolations(secondRule, DummyAuthorization.FailedResponse),
//             AuthorizationResult.ApplicableWithNoViolations(thirdRule),
//         });
//     }
// }



