using System.Reflection;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Test.Unit;

public class BioSubmissionTester
{
    [Fact]
    public void IsRequiredOrganicMaterialForBioSubmissionAttribute_IsOnlyAppliedOnNullableBooleanProperties()
    {
        // Arrange
        var bioSubmissionType = typeof(BioSubmission);
        var relevantAssembly = bioSubmissionType.Assembly;
        foreach (var type in relevantAssembly.GetTypes())
        {
            if (!type.IsSubclassOf(bioSubmissionType))
                continue;

            var properties = type.GetProperties()
                .Where(
                    p =>
                        p.GetCustomAttribute<IsRequiredOrganicMaterialForBioSubmissionAttribute>()
                        != null
                );

            // Assert
            foreach (var property in properties)
            {
                property.PropertyType.Should().Be(typeof(bool?));
            }
        }
    }
}
