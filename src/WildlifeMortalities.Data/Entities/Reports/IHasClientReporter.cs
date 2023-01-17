using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Reports;

internal interface IHasClientReporter
{
    public int ClientId { get; set; }
    public Client Client { get; set; }
}
