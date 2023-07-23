using WildlifeMortalities.App.Features.Shared.Mortalities;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Shared.Services;
using WildlifeMortalities.Test.Helpers;

namespace WildlifeMortalities.Test.Unit;

public class ViewModelTester
{
    private static List<Type> GetMortalityTypesWithAdditionalProperties()
    {
        var mortalityType = typeof(Mortality);
        var relevantAssembly = mortalityType.Assembly;
        List<Type> mortalityTypesWithAdditionalProperties = new();
        foreach (var type in relevantAssembly.GetTypes())
        {
            if (!type.IsSubclassOf(mortalityType))
                continue;

            var ownProperties = type.GetPublicNoneBaseProperties()
                .Where(
                    x =>
                        x.CanWrite == true
                        && (x.PropertyType == typeof(string) || x.PropertyType.IsValueType)
                )
                .ToArray();

            if (ownProperties.Length == 0)
                continue;

            mortalityTypesWithAdditionalProperties.Add(type);
        }

        return mortalityTypesWithAdditionalProperties;
    }

    [Fact]
    public void MortalityViewModel_HasConstructorAcceptingMortality_WithMortalityParameter()
    {
        var relevantTypes = GetMortalityTypesWithAdditionalProperties();
        relevantTypes.Should().NotBeEmpty();

        var mortalityViewModelType = typeof(MortalityViewModel);
        var relevantAssembly = mortalityViewModelType.Assembly;
        List<Type> viewModelTypes = new();
        foreach (var type in relevantAssembly.GetTypes())
        {
            if (!type.IsSubclassOf(mortalityViewModelType))
                continue;

            viewModelTypes.Add(type);
        }

        foreach (var type in relevantTypes)
        {
            var correspondingVm = viewModelTypes.Find(
                x => x.GetConstructor(new[] { type, typeof(ReportDetail) }) != null
            );

            correspondingVm.Should().NotBeNull();
        }
    }
}
