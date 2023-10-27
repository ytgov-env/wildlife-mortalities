using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class MooseMortality : Mortality
{
    public override Species Species => Species.Moose;
    public int? GameManagementSubAreaId { get; set; }
    public GameManagementSubArea? GameManagementSubArea { get; set; }
}
