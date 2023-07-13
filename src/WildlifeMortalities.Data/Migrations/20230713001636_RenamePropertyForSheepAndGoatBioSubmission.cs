using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenamePropertyForSheepAndGoatBioSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThinhornSheepBioSubmission_HornLengthToThirdAnnulusMillimetres",
                table: "BioSubmissions",
                newName: "ThinhornSheepBioSubmission_HornLengthToThirdAnnulusOnShorterHornMillimetres");

            migrationBuilder.RenameColumn(
                name: "MountainGoatBioSubmission_HornLengthToThirdAnnulusMillimetres",
                table: "BioSubmissions",
                newName: "MountainGoatBioSubmission_HornLengthToThirdAnnulusOnShorterHornMillimetres");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThinhornSheepBioSubmission_HornLengthToThirdAnnulusOnShorterHornMillimetres",
                table: "BioSubmissions",
                newName: "ThinhornSheepBioSubmission_HornLengthToThirdAnnulusMillimetres");

            migrationBuilder.RenameColumn(
                name: "MountainGoatBioSubmission_HornLengthToThirdAnnulusOnShorterHornMillimetres",
                table: "BioSubmissions",
                newName: "MountainGoatBioSubmission_HornLengthToThirdAnnulusMillimetres");
        }
    }
}
