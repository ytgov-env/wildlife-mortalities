using System.Reflection;
using FluentValidation;
using FluentValidation.Validators;
using WildlifeMortalities.App.Features.Shared.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reports;

public abstract class ActivityViewModel
{
    protected ActivityViewModel() =>
        MortalityWithSpeciesSelectionViewModel = new MortalityWithSpeciesSelectionViewModel();

    protected ActivityViewModel(Activity activity, ReportDetail? reportDetail = null)
    {
        MortalityWithSpeciesSelectionViewModel = new MortalityWithSpeciesSelectionViewModel
        {
            Species = activity.Mortality.Species,
            MortalityViewModel = new MortalityViewModel(activity.Mortality, reportDetail)
        };

        Comment = activity.Comment;
    }

    public bool IsCompleted { get; set; }
    public string Comment { get; set; } = string.Empty;

    public MortalityWithSpeciesSelectionViewModel MortalityWithSpeciesSelectionViewModel { get; set; }
}

public abstract class ActivityViewModelValidator<T> : AbstractValidator<T>
    where T : ActivityViewModel
{
    protected ActivityViewModelValidator()
    {
        RuleFor(x => x.Comment).MaximumLength(1000);

        RuleFor(x => x.MortalityWithSpeciesSelectionViewModel.Species)
            .NotNull()
            .WithMessage("Please select a species.");

        RuleFor(x => x.MortalityWithSpeciesSelectionViewModel.MortalityViewModel)
            .NotNull()
            .When(x => x.MortalityWithSpeciesSelectionViewModel.Species != null)
            .SetInheritanceValidator(x => x.AddMortalityValidators());
    }
}

public static class FluentValidationExtensions
{
    private static readonly Dictionary<
        Type,
        List<(MethodInfo, ConstructorInfo)>
    > _mortalityValidatorFactories = new();

    static FluentValidationExtensions() { }

    public static void AddMortalityValidators<T>(
        this PolymorphicValidator<T, MortalityViewModel> builder
    )
    {
        builder.Add(new MortalityViewModelValidator());

        if (_mortalityValidatorFactories.ContainsKey(typeof(T)) == false)
        {
            List<(MethodInfo, ConstructorInfo)> values = new();

            var mortalityViewModelType = typeof(MortalityViewModel);
            var relevantAssembly = mortalityViewModelType.Assembly;
            var allTypes = relevantAssembly.GetTypes();

            List<(Type, Type)> _mortalityViewModelTypes = new();
            foreach (var item in allTypes)
            {
                if (!item.IsSubclassOf(mortalityViewModelType))
                {
                    continue;
                }

                var validatorType = typeof(AbstractValidator<>).MakeGenericType(item);

                _mortalityViewModelTypes.Add((item, validatorType));
            }

            foreach (var item in allTypes)
            {
                foreach (var (vmType, validatorType) in _mortalityViewModelTypes)
                {
                    if (!item.IsSubclassOf(validatorType))
                    {
                        continue;
                    }

                    var defaultConstructor = item.GetConstructor(Array.Empty<Type>());

                    var type = typeof(PolymorphicValidator<T, MortalityViewModel>);
                    var addMethod = type.GetMethods()
                        .Where(
                            x => x.Name == nameof(PolymorphicValidator<T, MortalityViewModel>.Add)
                        )
                        .Select(x => new { Method = x, parameters = x.GetParameters() })
                        .FirstOrDefault(
                            x =>
                                x.parameters.Length == 2
                                && x.parameters[0].ParameterType.IsAssignableTo(typeof(IValidator))
                        )
                        ?.Method;

                    var genericAddMethod = addMethod?.MakeGenericMethod(vmType);

                    if (genericAddMethod != null)
                    {
                        values.Add((genericAddMethod!, defaultConstructor!));
                    }
                }
            }

            _mortalityValidatorFactories.Add(typeof(T), values);
        }

        foreach (var (addMethod, validatorFactory) in _mortalityValidatorFactories[typeof(T)])
        {
            addMethod?.Invoke(
                builder,
                new[] { validatorFactory.Invoke(null), Array.Empty<string>() }
            );
        }
    }
}
