using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Test.Helpers;

namespace WildlifeMortalities.Test.Unit;

public class EntityConfigTester
{
    [Fact]
    public void Entities_HaveStandardizedColumnNamesInEfCoreConfig()
    {
        var baseClasses = typeof(AppDbContext).Assembly
            .GetTypes()
            .Where(
                x =>
                    x.IsAbstract
                    && !x.IsGenericType
                    && x.BaseType == typeof(object)
                    && x.Namespace?.StartsWith("WildlifeMortalities.Data.Entities") == true
            )
            .ToArray();

        var derivedPropertiesBoundToASharedColumn = new List<(string, Type)>
        {
            (nameof(CanadaLynxBioSubmission.FurbearerSealNumber), typeof(CanadaLynxBioSubmission)),
            (nameof(GreyWolfBioSubmission.FurbearerSealNumber), typeof(GreyWolfBioSubmission)),
            (nameof(WolverineBioSubmission.FurbearerSealNumber), typeof(WolverineBioSubmission)),
            (nameof(HuntingBagLimitEntry.SeasonId), typeof(HuntingBagLimitEntry)),
            (nameof(TrappingBagLimitEntry.SeasonId), typeof(TrappingBagLimitEntry))
        };

        var excludedProperties = new[]
        {
            nameof(AmericanBlackBearBioSubmission.MortalityId),
            nameof(HarvestActivity.HrbsNumber)
        };

        var context = new AppDbContext();

        var counter1 = 0;
        var counter2 = 0;

        foreach (var baseClass in baseClasses)
        {
            var assembly = baseClass.Assembly;
            foreach (
                var entityType in assembly
                    .GetTypes()
                    .Where(x => x.IsAssignableTo(baseClass) && !x.IsAbstract)
            )
            {
                var entity = context.Model.FindEntityTypes(entityType).FirstOrDefault();
                entity.Should().NotBeNull();

                foreach (
                    var item in entityType
                        .GetPublicNoneBaseProperties()
                        .Where(
                            x =>
                                x.CanWrite
                                && (x.PropertyType.IsValueType || x.PropertyType == typeof(string))
                        )
                )
                {
                    var property = entity!.FindProperty(item);
                    property.Should().NotBeNull();

                    if (
                        derivedPropertiesBoundToASharedColumn.Any(
                            x => x.Item1 == item.Name && x.Item2 == entityType
                        )
                    )
                    {
                        property!.GetColumnName().Should().Be($"{item.Name}");
                    }
                    else
                    {
                        property!.GetColumnName().Should().Be($"{entityType.Name}_{item.Name}");
                    }

                    counter1++;
                }

                foreach (
                    var item in entityType
                        .GetPublicBaseOnlyProperties()
                        .Where(
                            x =>
                                x.CanWrite
                                && (x.PropertyType.IsValueType || x.PropertyType == typeof(string))
                        )
                )
                {
                    var property = entity!.FindProperty(item);
                    property.Should().NotBeNull();

                    if (excludedProperties.Contains(item.Name))
                        continue;

                    property!.GetColumnName().Should().Be($"{item.Name}");

                    counter2++;
                }
            }
        }

        counter1.Should().BeGreaterThan(0);
        counter2.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Entities_StoredPropertiesAreExplicitlyMappedViaAColumnAttribute()
    {
        var baseClasses = typeof(AppDbContext).Assembly
            .GetTypes()
            .Where(
                x =>
                    x.IsAbstract
                    && !x.IsGenericType
                    && x.BaseType == typeof(object)
                    && x.Namespace?.StartsWith("WildlifeMortalities.Data.Entities") == true
            )
            .ToArray();

        var derivedPropertiesBoundToASharedColumn = new List<(string, Type)>
        {
            (nameof(CanadaLynxBioSubmission.FurbearerSealNumber), typeof(CanadaLynxBioSubmission)),
            (nameof(GreyWolfBioSubmission.FurbearerSealNumber), typeof(GreyWolfBioSubmission)),
            (nameof(WolverineBioSubmission.FurbearerSealNumber), typeof(WolverineBioSubmission)),
            (nameof(HuntingBagLimitEntry.SeasonId), typeof(HuntingBagLimitEntry)),
            (nameof(TrappingBagLimitEntry.SeasonId), typeof(TrappingBagLimitEntry))
        };

        foreach (var baseClass in baseClasses)
        {
            var assembly = baseClass.Assembly;
            foreach (
                var entityType in assembly
                    .GetTypes()
                    .Where(x => x.IsAssignableTo(baseClass) && !x.IsAbstract)
            )
            {
                foreach (
                    var item in entityType
                        .GetPublicNoneBaseProperties()
                        .Where(
                            x =>
                                x.CanWrite
                                && (x.PropertyType.IsValueType || x.PropertyType == typeof(string))
                        )
                )
                {
                    var columnAttribute = item.GetCustomAttribute<ColumnAttribute>();
                    columnAttribute
                        .Should()
                        .NotBeNull(
                            "Type {0} with property {1} doesn't have a column attribute",
                            entityType.Name,
                            item.Name
                        );

                    if (
                        derivedPropertiesBoundToASharedColumn.Any(
                            x => x.Item1 == item.Name && x.Item2 == entityType
                        )
                    )
                    {
                        columnAttribute!.Name.Should().Be($"{item.Name}");
                    }
                    else
                    {
                        columnAttribute!.Name.Should().Be($"{entityType.Name}_{item.Name}");
                    }
                }
            }
        }
    }
}
