namespace WildlifeMortalities.Data.Entities.Mortalities;

public class DuskyGrouseMortality : Mortality
{
    public DuskyGrouseMortality() => Family = Enums.Family.Grouse;

    public override Species Species => Species.DuskyGrouse;
}
