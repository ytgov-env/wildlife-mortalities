using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Shared.Extensions;
using WildlifeMortalities.Shared.Validators;

namespace WildlifeMortalities.Shared.Services;

public class HuntedHarvestReportService<T> where T : Mortality
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly MortalityService _mortalityService;

    public HuntedHarvestReportService(
        IDbContextFactory<AppDbContext> dbContextFactory,
        MortalityService mortalityService
    )
    {
        _dbContextFactory = dbContextFactory;
        _mortalityService = mortalityService;
    }

    public async Task<HuntedHarvestReport?> GetHarvestReportById(int id)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.HarvestReports
            .OfType<HuntedHarvestReport>()
            .Include(h => h.Mortality)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<Result<HuntedHarvestReport>> CreateHuntedHarvestReport(
        HuntedHarvestReport huntedHarvestReport
    )
    {
        var validator = new HuntedHarvestReportValidator<T>();
        var validation = await validator.ValidateAsync(huntedHarvestReport);
        if (!validation.IsValid)
        {
            return Result<HuntedHarvestReport>.Invalid(validation.AsErrors());
        }

        var result = await _mortalityService.CreateMortality((T)huntedHarvestReport.Mortality);
        if (result.IsSuccess)
        {
            var mortality = result.Value;
            huntedHarvestReport.Mortality = mortality;
            huntedHarvestReport.Violations = await mortality.GetViolations();
            huntedHarvestReport.Violations.AddRange(await huntedHarvestReport.GetViolations());

            var context = await _dbContextFactory.CreateDbContextAsync();
            context.Add(huntedHarvestReport);
            await context.SaveChangesAsync();
            return Result<HuntedHarvestReport>.Success(huntedHarvestReport);
        }
        else
        {
            return Result<HuntedHarvestReport>.Invalid(result.ValidationErrors);
        }
    }

    public async Task<Result<HuntedHarvestReport>> UpdateHuntedHarvestReport(
        HuntedHarvestReport huntedHarvestReport
    )
    {
        if (huntedHarvestReport.Mortality is null)
        {
            throw new ArgumentException(
                $"{nameof(huntedHarvestReport.Mortality)} property on {nameof(huntedHarvestReport)} cannot be null"
            );
        }

        var validator = new HuntedHarvestReportValidator<T>();
        var validation = await validator.ValidateAsync(huntedHarvestReport);
        if (!validation.IsValid)
        {
            return Result<HuntedHarvestReport>.Invalid(validation.AsErrors());
        }

        var result = await _mortalityService.UpdateMortality((T)huntedHarvestReport.Mortality);
        if (result.IsSuccess)
        {
            var mortality = result.Value;
            huntedHarvestReport.Mortality = mortality;
            huntedHarvestReport.Violations?.Clear();
            huntedHarvestReport.Violations = await mortality.GetViolations();
            huntedHarvestReport.Violations.AddRange(await huntedHarvestReport.GetViolations());

            var context = await _dbContextFactory.CreateDbContextAsync();
            context.Update(huntedHarvestReport);
            await context.SaveChangesAsync();
            return Result<HuntedHarvestReport>.Success(huntedHarvestReport);
        }
        else
        {
            return Result<HuntedHarvestReport>.Invalid(result.ValidationErrors);
        }
    }
}
