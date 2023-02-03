using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.People;

public abstract class Person
{
    public int Id { get; set; }
    public List<DraftReport> DraftReports { get; set; } = null!;
}

public class PersonConfig : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder) => builder.ToTable("People");
}
