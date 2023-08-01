using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using static WildlifeMortalities.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Entities.Users;

public class Permission
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;

    public List<User> Users { get; set; } = null!;
}

public class PermissionConfig : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableNameConstants.Permissions);
    }
}

public static class PermissionConstants
{
    [Display(Name = "User")]
    public static class UserManagement
    {
        [Display(Name = "Create user")]
        public const string Create = "CreateUser";

        [Display(Name = "Edit user")]
        public const string Edit = "EditUser";

        [Display(Name = "Delete user")]
        public const string Delete = "DeleteUser";
    }

    [Display(Name = "Reports")]
    public static class Reports
    {
        [Display(Name = "")]
        public const string ViewHarvest = "ViewHarvestReport";

        [Display(Name = "")]
        public const string ViewHumanWildlifeConflict = "ViewHumanWildlifeConflictReport";

        [Display(Name = "")]
        public const string EditHumanWildlifeConflict = "EditHumanWildlifeConflictReport";

        [Display(Name = "")]
        public const string CreateHarvest = "CreateHarvestReport";

        [Display(Name = "")]
        public const string EditHarvest = "EditHarvestReport";

        [Display(Name = "")]
        public const string DeleteHarvest = "DeleteHarvestReport";

        [Display(Name = "")]
        public const string SuppressViolation = "SuppressViolation";

        [Display(Name = "")]
        public const string ChangeSpecies = "ChangeSpecies";
    }

    [Display(Name = "Bio submissions")]
    public static class BioSubmissions
    {
        [Display(Name = "")]
        public const string ResetSamples = "ResetBioSubmissionSamples";

        [Display(Name = "")]
        public const string ResetAnalysis = "ResetBioSubmissionAnalysis";

        [Display(Name = "")]
        public const string EditSamples = "EditBioSubmissionSamples";

        [Display(Name = "")]
        public const string EditAnalysis = "EditBioSubmissionAnalysis";

        [Display(Name = "")]
        public const string EditAnalysisAgeForNonSheepAndGoat = "EditAnalysisAgeForNonSheepAndGoat";
    }

    [Display(Name = "Bag limits")]
    public static class BagLimits
    {
        [Display(Name = "")]
        public const string InitializeNewSeason = "InitializeNewSeason";

        [Display(Name = "")]
        public const string EditCurrentSeason = "EditCurrentSeason";

        [Display(Name = "")]
        public const string EditPastSeason = "EditPastSeason";
    }
}
