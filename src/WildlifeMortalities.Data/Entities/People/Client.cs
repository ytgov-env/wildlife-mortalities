using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.People;

public class Client : PersonWithAuthorizations
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }

    public List<SpecialGuideLicence> SpecialGuideLicencesAsClient { get; set; } = null!;
    public List<IndividualHuntedMortalityReport> IndividualHuntedMortalityReports { get; set; } =
        null!;
    public List<OutfitterGuidedHuntReport> OutfitterGuidedHuntReportsAsClient { get; set; } = null!;
    public List<OutfitterGuidedHuntReport> OutfitterGuidedHuntReportsAsChiefGuide { get; set; } =
        null!;
    public List<OutfitterGuidedHuntReport> OutfitterGuidedHuntReportsAsAssistantGuide { get; set; } =
        null!;
    public List<SpecialGuidedHuntReport> SpecialGuidedHuntReportsAsClient { get; set; } = null!;
    public List<SpecialGuidedHuntReport> SpecialGuidedHuntReportsAsGuide { get; set; } = null!;
    public List<TrappedMortalitiesReport> TrappedMortalitiesReports { get; set; } = null!;

    public override void Update(PersonWithAuthorizations person)
    {
        if (person is Client client)
        {
            Update(client);
        }
    }

    public void Update(Client client)
    {
        FirstName = client.FirstName;
        LastName = client.LastName;
        BirthDate = client.BirthDate;
        LastModifiedDateTime = client.LastModifiedDateTime;
    }

    public override string ToString() => $"{FirstName} {LastName} ({EnvPersonId})";

    public override bool Merge(PersonWithAuthorizations person)
    {
        if (person is Client client)
        {
            return Merge(client);
        }
        return false;
    }

    public bool Merge(Client clientToBeMerged)
    {
        // This check was added because posse returned clients who had previousEnvClientIds that matched the EnvClientId of that client,
        // which is invalid and shouldn't result in a merge
        if (EnvPersonId == clientToBeMerged.EnvPersonId)
        {
            return false;
        }

        Authorizations.AddRange(clientToBeMerged.Authorizations);
        SpecialGuideLicencesAsClient.AddRange(clientToBeMerged.SpecialGuideLicencesAsClient);

        IndividualHuntedMortalityReports.AddRange(
            clientToBeMerged.IndividualHuntedMortalityReports
        );
        OutfitterGuidedHuntReportsAsClient.AddRange(
            clientToBeMerged.OutfitterGuidedHuntReportsAsClient
        );
        OutfitterGuidedHuntReportsAsChiefGuide.AddRange(
            clientToBeMerged.OutfitterGuidedHuntReportsAsChiefGuide
        );
        OutfitterGuidedHuntReportsAsAssistantGuide.AddRange(
            clientToBeMerged.OutfitterGuidedHuntReportsAsAssistantGuide
        );
        SpecialGuidedHuntReportsAsClient.AddRange(
            clientToBeMerged.SpecialGuidedHuntReportsAsClient
        );
        SpecialGuidedHuntReportsAsGuide.AddRange(clientToBeMerged.SpecialGuidedHuntReportsAsGuide);
        TrappedMortalitiesReports.AddRange(clientToBeMerged.TrappedMortalitiesReports);

        DraftReports.AddRange(clientToBeMerged.DraftReports);

        return true;
    }
}

public class ClientConfig : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasIndex(c => c.EnvPersonId).IsUnique();
        builder.Property(c => c.FirstName).HasColumnName(nameof(Client.FirstName));
        builder.Property(c => c.LastName).HasColumnName(nameof(Client.LastName));
    }
}
