using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.App.Features.HarvestReports;

public class CreateWoodBisonMortalityViewModel : CreateMortalityViewModel
{
    public PregnancyStatus PregnancyStatus { get; set; }
    public bool IsWounded { get; set; }

    public CreateWoodBisonMortalityViewModel() : base(Data.Enums.AllSpecies.WoodBison)
    {
    }

    public override Mortality GetMortality()
    {
        var mortality = new WoodBisonMortality
        {
            PregnancyStatus = PregnancyStatus,
            IsWounded = IsWounded
        };

        SetBaseValues(mortality);
        return mortality;
    }
}
