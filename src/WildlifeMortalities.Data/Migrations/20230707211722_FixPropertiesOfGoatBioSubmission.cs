using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixPropertiesOfGoatBioSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_BioSubmissions_MountainGoatBioSubmission_BroomedStatus_Enum",
                table: "BioSubmissions");

            migrationBuilder.RenameColumn(
                name: "MountainGoatBioSubmission_BroomedStatus",
                table: "BioSubmissions",
                newName: "MountainGoatBioSubmission_HornLengthToThirdAnnulusMillimetres");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MountainGoatBioSubmission_HornLengthToThirdAnnulusMillimetres",
                table: "BioSubmissions",
                newName: "MountainGoatBioSubmission_BroomedStatus");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BioSubmissions_MountainGoatBioSubmission_BroomedStatus_Enum",
                table: "BioSubmissions",
                sql: "[MountainGoatBioSubmission_BroomedStatus] IN (10, 20, 30, 40)");
        }
    }
}
