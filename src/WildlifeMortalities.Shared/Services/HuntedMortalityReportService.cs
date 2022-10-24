using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Shared.Extensions;
using WildlifeMortalities.Shared.Validators;

namespace WildlifeMortalities.Shared.Services;

public class HuntedMortalityReportService<T> where T : Mortality
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
        var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.MortalityReports
            .OfType<HuntedMortalityReport>()
            .Include(h => h.Mortality)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<Result<HuntedMortalityReport>> CreateHuntedMortalityReport(
        HuntedMortalityReport huntedMortalityReport
    )
    {
        var validator = new HuntedMortalityReportValidator<T>();
        var validation = await validator.ValidateAsync(huntedMortalityReport);
        if (!validation.IsValid)
        {
            return Result<HuntedMortalityReport>.Invalid(validation.AsErrors());
        }

        var result = await _mortalityService.CreateMortality((T)huntedMortalityReport.Mortality);
        if (result.IsSuccess)
        {
            var mortality = result.Value;
            huntedMortalityReport.Mortality = mortality;
            huntedMortalityReport.Violations = await mortality.GetViolations();
            huntedMortalityReport.Violations.AddRange(await huntedMortalityReport.GetViolations());

            var context = await _dbContextFactory.CreateDbContextAsync();
            context.Add(huntedMortalityReport);
            await context.SaveChangesAsync();
            return Result<HuntedMortalityReport>.Success(huntedMortalityReport);
        }

        return Result<HuntedMortalityReport>.Invalid(result.ValidationErrors);
    }

    public async Task<Result<HuntedMortalityReport>> UpdateHuntedMortalityReport(
        HuntedMortalityReport huntedMortalityReport
    )
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

        var result = await _mortalityService.UpdateMortality((T)huntedMortalityReport.Mortality);
        if (result.IsSuccess)
        {
            var mortality = result.Value;
            huntedMortalityReport.Mortality = mortality;
            huntedMortalityReport.Violations?.Clear();
            huntedMortalityReport.Violations = await mortality.GetViolations();
            huntedMortalityReport.Violations.AddRange(await huntedMortalityReport.GetViolations());

            var context = await _dbContextFactory.CreateDbContextAsync();
            context.Update(huntedMortalityReport);
            await context.SaveChangesAsync();
            return Result<HuntedMortalityReport>.Success(huntedMortalityReport);
        }

        return Result<HuntedMortalityReport>.Invalid(result.ValidationErrors);
    }
}
