using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.MortalityReports;
using WildlifeMortalities.Shared.Validators;

namespace WildlifeMortalities.Shared.Services;

public class HuntedMortalityReportService : IDisposable
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly MortalityService _mortalityService;

    public HuntedMortalityReportService(
        IDbContextFactory<AppDbContext> dbContextFactory,
        MortalityService mortalityService
    )
    {
        _dbContextFactory = dbContextFactory;
        _mortalityService = mortalityService;
    }

    public async Task<HuntedMortalityReport?> GetHarvestReportById(int id)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.MortalityReports
            .OfType<HuntedMortalityReport>()
            .Include(h => h.Mortality)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<Result<HuntedMortalityReport>> CreateHuntedMortalityReport<T>(
        HuntedMortalityReport huntedMortalityReport
    ) where T : Mortality
    {
        var validator = new HuntedMortalityReportValidator<T>();
        var validation = await validator.ValidateAsync(huntedMortalityReport);
        if (!validation.IsValid)
        {
            return Result<HuntedMortalityReport>.Invalid(validation.AsErrors());
        }

        var result = await _mortalityService.CreateMortality(huntedMortalityReport.Mortality);
        if (!result.IsSuccess)
        {
            return Result<HuntedMortalityReport>.Invalid(result.ValidationErrors);
        }

        var mortality = result.Value;
        huntedMortalityReport.Mortality = mortality;
        huntedMortalityReport.Violations = await GetMortalityViolations(mortality);
        huntedMortalityReport.Violations.AddRange(await GetHuntedMortalityReportViolations(huntedMortalityReport));

        await using var context = await _dbContextFactory.CreateDbContextAsync();
        context.Add(huntedMortalityReport);
        await context.SaveChangesAsync();
        return Result<HuntedMortalityReport>.Success(huntedMortalityReport);
    }

    public async Task<Result<HuntedMortalityReport>> UpdateHuntedMortalityReport<T>(
        HuntedMortalityReport huntedMortalityReport
    ) where T : Mortality
    {
        if (huntedMortalityReport.Mortality is null)
        {
            throw new ArgumentException(
                $"{nameof(huntedMortalityReport.Mortality)} property on {nameof(huntedMortalityReport)} cannot be null"
            );
        }

        var validator = new HuntedMortalityReportValidator<T>();
        var validation = await validator.ValidateAsync(huntedMortalityReport);
        if (!validation.IsValid)
        {
            return Result<HuntedMortalityReport>.Invalid(validation.AsErrors());
        }

        var result = await _mortalityService.UpdateMortality(huntedMortalityReport.Mortality);
        if (!result.IsSuccess)
        {
            return Result<HuntedMortalityReport>.Invalid(result.ValidationErrors);
        }

        var mortality = result.Value;
        huntedMortalityReport.Mortality = mortality;
        huntedMortalityReport.Violations.Clear();
        huntedMortalityReport.Violations = await GetMortalityViolations(mortality);
        huntedMortalityReport.Violations.AddRange(await GetHuntedMortalityReportViolations(huntedMortalityReport));

        await using var context = await _dbContextFactory.CreateDbContextAsync();
        context.Update(huntedMortalityReport);
        await context.SaveChangesAsync();
        return Result<HuntedMortalityReport>.Success(huntedMortalityReport);
    }

    private async Task<List<Violation>> GetHuntedMortalityReportViolations(HuntedMortalityReport report)
    {
        var violations = new List<Violation>();
        switch (report.Mortality)
        {
            case BirdMortality bird:
                break;

            case AmericanBlackBearMortality americanBlackBear:
                break;

            case BarrenGroundCaribouMortality barrenGroundCaribou:
                break;

            case CoyoteMortality coyote:
                break;

            case ElkMortality elk:
                break;

            case GreyWolfMortality greyWolf:
                break;

            case GrizzlyBearMortality grizzlyBear:
                break;

            case MooseMortality moose:
                break;

            case MountainGoatMortality mountainGoat:
                break;

            case MuleDeerMortality muleDeer:
                break;

            case ThinhornSheepMortality thinhornSheep:
                break;

            case WolverineMortality wolverine:
                break;

            case WoodBisonMortality woodBison:
                break;

            case WoodlandCaribouMortality woodlandCaribou:
                break;
        }

        return violations;
    }

    private async Task<List<Violation>> GetMortalityViolations(Mortality mortality)
    {
        var violations = new List<Violation>();
        switch (mortality)
        {
            case BirdMortality bird:
                break;

            case AmericanBlackBearMortality americanBlackBear:
                break;

            case BarrenGroundCaribouMortality barrenGroundCaribou:
                break;

            case CoyoteMortality coyote:
                break;

            case ElkMortality elk:
                break;

            case GreyWolfMortality greyWolf:
                break;

            case GrizzlyBearMortality grizzlyBear:
                break;

            case MooseMortality moose:
                break;

            case MountainGoatMortality mountainGoat:
                break;

            case MuleDeerMortality muleDeer:
                break;

            case ThinhornSheepMortality thinhornSheep:
                break;

            case WolverineMortality wolverine:
                break;

            case WoodBisonMortality woodBison:
                break;

            case WoodlandCaribouMortality woodlandCaribou:
                break;
        }

        return violations;
    }

    public void Dispose()
    {
    }
}
