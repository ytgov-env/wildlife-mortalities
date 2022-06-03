using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Shared.Extensions;

public static class HuntedHarvestReportExtensions
{
    public static async Task<List<Violation>> GetViolations(
        this HuntedHarvestReport huntedHarvestReport
    )
    {
        var violations = new List<Violation>();
        switch (huntedHarvestReport.Mortality)
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
            default:
                break;
        }

        return violations;
    }
}
