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
        [Display(Name = "View harvest report")]
        public const string ViewHarvest = "ViewHarvestReport";

        [Display(Name = "View human-wildlife conflict report")]
        public const string ViewHumanWildlifeConflict = "ViewHumanWildlifeConflictReport";

        [Display(Name = "Edit human-wildlife conflict report")]
        public const string EditHumanWildlifeConflict = "EditHumanWildlifeConflictReport";

        [Display(Name = "Create harvest report")]
        public const string CreateHarvest = "CreateHarvestReport";

        [Display(Name = "Edit harvest report")]
        public const string EditHarvest = "EditHarvestReport";

        [Display(Name = "Delete harvest report")]
        public const string DeleteHarvest = "DeleteHarvestReport";

        [Display(Name = "Suppress violation")]
        public const string SuppressViolation = "SuppressViolation";

        [Display(Name = "Change species")]
        public const string ChangeSpecies = "ChangeSpecies";
    }

    [Display(Name = "Bio submissions")]
    public static class BioSubmissions
    {
        [Display(Name = "Reset bio submission samples")]
        public const string ResetSamples = "ResetBioSubmissionSamples";

        [Display(Name = "Reset bio submission analysis")]
        public const string ResetAnalysis = "ResetBioSubmissionAnalysis";

        [Display(Name = "Edit bio submission samples")]
        public const string EditSamples = "EditBioSubmissionSamples";

        [Display(Name = "Edit bio submission analysis")]
        public const string EditAnalysis = "EditBioSubmissionAnalysis";

        [Display(Name = "Edit bio submission analysis age (for species aside for sheep and goat)")]
        public const string EditAnalysisAgeForNonSheepAndGoat = "EditAnalysisAgeForNonSheepAndGoat";
    }

    [Display(Name = "Bag limits")]
    public static class BagLimits
    {
        [Display(Name = "Initialize new season")]
        public const string InitializeNewSeason = "InitializeNewSeason";

        [Display(Name = "Edit current season")]
        public const string EditCurrentSeason = "EditCurrentSeason";

        [Display(Name = "Edit past season")]
        public const string EditPastSeason = "EditPastSeason";
    }
}
