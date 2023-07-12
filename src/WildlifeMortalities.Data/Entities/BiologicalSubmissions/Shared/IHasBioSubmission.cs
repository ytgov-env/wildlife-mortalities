namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

public interface IHasBioSubmission
{
    int Id { get; }

    public bool SubTypeHasBioSubmission()
    {
        return true;
    }

    BioSubmission? CreateDefaultBioSubmission();
}
