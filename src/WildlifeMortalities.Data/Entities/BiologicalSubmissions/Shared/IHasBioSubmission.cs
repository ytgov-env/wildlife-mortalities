namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

public interface IHasBioSubmission
{
    int Id { get; }

    BioSubmission CreateDefaultBioSubmission();
}
