using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Extensions;

public static class DbContextExtensions
{
    public static async Task<BioSubmission?> GetBioSubmissionFromMortality(
        this DbSet<BioSubmission> bioSubmissions,
        IHasBioSubmission item
    )
    {
        return item switch
        {
            AmericanBlackBearMortality
                => await bioSubmissions
                    .OfType<AmericanBlackBearBioSubmission>()
                    .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
            CanadaLynxMortality
                => await bioSubmissions
                    .OfType<CanadaLynxBioSubmission>()
                    .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
            CaribouMortality
                => await bioSubmissions
                    .OfType<CaribouBioSubmission>()
                    .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
            ElkMortality
                => await bioSubmissions
                    .OfType<ElkBioSubmission>()
                    .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
            GreyWolfMortality
                => await bioSubmissions
                    .OfType<GreyWolfBioSubmission>()
                    .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
            GrizzlyBearMortality
                => await bioSubmissions
                    .OfType<GrizzlyBearBioSubmission>()
                    .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
            MountainGoatMortality
                => await bioSubmissions
                    .OfType<MountainGoatBioSubmission>()
                    .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
            MuleDeerMortality
                => await bioSubmissions
                    .OfType<MuleDeerBioSubmission>()
                    .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
            ThinhornSheepMortality
                => await bioSubmissions
                    .OfType<ThinhornSheepBioSubmission>()
                    .Include(x => x.HornMeasurementEntries)
                    .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
            WhiteTailedDeerMortality
                => await bioSubmissions
                    .OfType<WhiteTailedDeerBioSubmission>()
                    .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
            WolverineMortality
                => await bioSubmissions
                    .OfType<WolverineBioSubmission>()
                    .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
            WoodBisonMortality
                => await bioSubmissions
                    .OfType<WoodBisonBioSubmission>()
                    .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
            _ => throw new InvalidOperationException()
        };
    }
}
