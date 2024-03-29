﻿namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class IsRequiredOrganicMaterialForBioSubmissionAttribute : Attribute
{
    public IsRequiredOrganicMaterialForBioSubmissionAttribute(string displayName)
    {
        DisplayName = displayName;
    }

    public string DisplayName { get; }
}
