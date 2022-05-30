using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities.Mortalities;

internal interface IHasSex
{
    public Sex Sex { get; set; }
}
