using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Shared.Extensions;
using WildlifeMortalities.Shared.Validators;

namespace WildlifeMortalities.Shared.Services;

public class IndividualHuntReportService<T> where T : Mortality
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly MortalityService _mortalityService;

    public IndividualHuntReportService(
        IDbContextFactory<AppDbContext> dbContextFactory,
        MortalityService mortalityService
    )
    {
        _dbContextFactory = dbContextFactory;
        _mortalityService = mortalityService;
    }

    public async Task<IndividualHuntReport?> GetHarvestReportById(int id)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.MortalityReports
            .OfType<IndividualHuntReport>()
            .Include(h => h.Mortality)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<Result<IndividualHuntReport>> CreateIndividualHuntReport(
        IndividualHuntReport individualHuntReport
    )
    {
        var validator = new IndividualHuntReportValidator<T>();
        var validation = await validator.ValidateAsync(individualHuntReport);
        if (!validation.IsValid)
        {
            return Result<IndividualHuntReport>.Invalid(validation.AsErrors());
        }

        var result = await _mortalityService.CreateMortality((T)individualHuntReport.Mortality);
        if (result.IsSuccess)
        {
            var mortality = result.Value;
            individualHuntReport.Mortality = mortality;
            individualHuntReport.Violations = await mortality.GetViolations();
            individualHuntReport.Violations.AddRange(await individualHuntReport.GetViolations());

            var context = await _dbContextFactory.CreateDbContextAsync();
            context.Add(individualHuntReport);
            await context.SaveChangesAsync();
            return Result<IndividualHuntReport>.Success(individualHuntReport);
        }
        else
        {
            return Result<IndividualHuntReport>.Invalid(result.ValidationErrors);
        }
    }

    public async Task<Result<IndividualHuntReport>> UpdateIndividualHuntReport(
        IndividualHuntReport individualHuntReport
    )
    {
        if (individualHuntReport.Mortality is null)
        {
            throw new ArgumentException(
                $"{nameof(individualHuntReport.Mortality)} property on {nameof(individualHuntReport)} cannot be null"
            );
        }

        var validator = new IndividualHuntReportValidator<T>();
        var validation = await validator.ValidateAsync(individualHuntReport);
        if (!validation.IsValid)
        {
            return Result<IndividualHuntReport>.Invalid(validation.AsErrors());
        }

        var result = await _mortalityService.UpdateMortality((T)individualHuntReport.Mortality);
        if (result.IsSuccess)
        {
            var mortality = result.Value;
            individualHuntReport.Mortality = mortality;
            individualHuntReport.Violations?.Clear();
            individualHuntReport.Violations = await mortality.GetViolations();
            individualHuntReport.Violations.AddRange(await individualHuntReport.GetViolations());

            var context = await _dbContextFactory.CreateDbContextAsync();
            context.Update(individualHuntReport);
            await context.SaveChangesAsync();
            return Result<IndividualHuntReport>.Success(individualHuntReport);
        }
        else
        {
            return Result<IndividualHuntReport>.Invalid(result.ValidationErrors);
        }
    }
}
