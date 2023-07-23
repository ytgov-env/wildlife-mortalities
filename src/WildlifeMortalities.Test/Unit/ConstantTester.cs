using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using Xunit;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Test.Unit;

public class ConstantTester
{
    [Fact]
    public void TableNameConstants_FieldNameMatchesValue()
    {
        var type = typeof(TableNameConstants);
        foreach (var field in type.GetFields())
        {
            var fieldName = field.Name;
            var fieldValue = field.GetRawConstantValue();
            fieldValue.Should().NotBeNull();
            fieldName.Should().Be(fieldValue!.ToString());
        }
    }

    [Fact]
    public void TableNameConstants_AllDbContextTableNamesAreDeclaredInTableNameConstants()
    {
        var context = new AppDbContext();

        var efTableNames = context.Model
            .GetEntityTypes()
            .Select(x => x.GetTableName())
            .Distinct()
            .OrderBy(x => x)
            .ToArray();
        var constantsTableNames = typeof(TableNameConstants)
            .GetFields()
            .Where(f => f.IsLiteral && !f.IsInitOnly)
            .Select(f => f.GetRawConstantValue()?.ToString())
            .OrderBy(x => x)
            .ToArray();

        efTableNames.Should().BeEquivalentTo(constantsTableNames, c => c.WithStrictOrdering());
    }

    //[Fact]
    //public void EfEntities_WithEntityTypeConfiguration_ShouldHaveTableNameSet()
    //{
    //    var dbContext = new AppDbContext();

    //    var entityTypes = dbContext.Model
    //        .GetEntityTypes()
    //        .Where(x => typeof(DbContext).IsAssignableFrom(x.ClrType))
    //        .Where(
    //            x =>
    //                x.GetAnnotation(
    //                    "Microsoft.EntityFrameworkCore.EntityTypeConfigurationConfigurationSource"
    //                ) != null
    //        )
    //        .Select(x => x.ClrType);
    //}
}
