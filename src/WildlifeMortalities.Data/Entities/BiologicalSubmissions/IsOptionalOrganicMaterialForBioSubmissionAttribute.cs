namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class IsOptionalOrganicMaterialForBioSubmissionAttribute : Attribute
{
    public IsOptionalOrganicMaterialForBioSubmissionAttribute(string displayName)
    {
        DisplayName = displayName;
    }

    public string DisplayName { get; }
}
