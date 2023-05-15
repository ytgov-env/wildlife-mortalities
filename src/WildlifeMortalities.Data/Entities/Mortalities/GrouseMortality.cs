namespace WildlifeMortalities.Data.Entities.Mortalities
{
    public class GrouseMortality : Mortality
    {
        public GrouseMortality() => Family = Enums.Family.Grouse;

        public override Species Species => Species.Grouse;
    }
}
