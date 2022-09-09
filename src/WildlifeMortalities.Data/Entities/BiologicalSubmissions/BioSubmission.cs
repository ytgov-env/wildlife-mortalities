namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class BioSubmission
{
    public int Id { get; set; }
    public Age Age { get; set; } = null!;
}

public class Age
{
    public int Years { get; set; }
    public ConfidenceInAge Confidence { get; set; }
}

public enum ConfidenceInAge
{
    Uninitialized = 0,
    Fair = 1,
    Good = 2,
    Poor = 3
}
