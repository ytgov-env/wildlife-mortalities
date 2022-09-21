using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities;

public class ConflictReport : MortalityReport
{
    public int ConservationOfficerId { get; set; }
    public ConservationOfficer ConservationOfficer { get; set; } = null!;
    public string HumanWildlifeConflictNumber { get; set; } = string.Empty;
}
