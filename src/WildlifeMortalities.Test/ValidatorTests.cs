using FluentValidation.TestHelper;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Validators;

namespace WildlifeMortalities.Test;

public class ValidatorTests
{
    [Fact]
    public void CanCreateMortalityWithMaleSex()
    {
        var model = new AmericanBlackBearMortality { Sex = Sex.Male };

        var validator = new MortalityValidator<AmericanBlackBearMortality>();

        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(m => m.Sex);
    }

    [Fact]
    public void CannotCreateMortalityWithUnitializedSex()
    {
        var model = new AmericanBlackBearMortality { Sex = Sex.Uninitialized };

        var validator = new MortalityValidator<AmericanBlackBearMortality>();
        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(m => m.Sex);
    }

    [Theory]
    [InlineData(-60)]
    [InlineData(-65)]
    [InlineData(-70)]
    [InlineData(73)]
    [InlineData(40)]
    public void CannotCreateMortalityWithLatitudeFarOutsideTheYukon(decimal latitude)
    {
        var model = new AmericanBlackBearMortality { Latitude = latitude, Longitude = -134.00m };

        var validator = new MortalityValidator<AmericanBlackBearMortality>();
        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(m => m.Latitude);
    }

    [Theory]
    [InlineData(60)]
    [InlineData(65)]
    [InlineData(70)]
    public void CanCreateMortalityWithLatitudeNearTheYukon(decimal latitude)
    {
        var model = new AmericanBlackBearMortality { Latitude = latitude, Longitude = -134.00m };

        var validator = new MortalityValidator<AmericanBlackBearMortality>();
        var result = validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(m => m.Latitude);
    }

    [Theory]
    [InlineData(-70)]
    [InlineData(-145)]
    [InlineData(134)]
    [InlineData(30)]
    public void CannotCreateMortalityWithLongitudeFarOutsideTheYukon(decimal longitude)
    {
        var model = new AmericanBlackBearMortality { Latitude = 62.42m, Longitude = longitude };

        var validator = new MortalityValidator<AmericanBlackBearMortality>();
        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(m => m.Longitude);
    }

    [Theory]
    [InlineData(-142)]
    [InlineData(-138)]
    [InlineData(-134)]
    [InlineData(-129)]
    public void CanCreateMortalityWithLongitudeNearTheYukon(decimal longitude)
    {
        var model = new AmericanBlackBearMortality { Latitude = 62.42m, Longitude = longitude };

        var validator = new MortalityValidator<AmericanBlackBearMortality>();
        var result = validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(m => m.Longitude);
    }

    [Fact]
    public void LatitudeCannotBeSetWhileLongitudeIsNull()
    {
        var model = new AmericanBlackBearMortality { Latitude = 62.42m, Longitude = null };

        var validator = new MortalityValidator<AmericanBlackBearMortality>();
        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(m => m.Latitude);
    }

    [Fact]
    public void LongitudeCannotBeSetWhileLatitudeIsNull()
    {
        var model = new AmericanBlackBearMortality { Latitude = null, Longitude = -134.52m };

        var validator = new MortalityValidator<AmericanBlackBearMortality>();
        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(m => m.Longitude);
    }

    [Fact]
    public void CannotCreateHuntedMortalityReportWithoutSeal()
    {
        var model = new HuntedMortalityReport();

        var validator = new HuntedMortalityReportValidator<AmericanBlackBearMortality>();
        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(h => h.Seal);
    }

    [Fact]
    public void CanCreateHuntedMortalityReportWithSeal()
    {
        var model = new HuntedMortalityReport { Seal = new Seal() };

        var validator = new HuntedMortalityReportValidator<AmericanBlackBearMortality>();
        var result = validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(h => h.Seal);
    }
}
