using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Reports;

public interface IHasConservationOfficerReporter
{
    public int ConservationOfficerId { get; set; }
    public ConservationOfficer ConservationOfficer { get; set; }
}
