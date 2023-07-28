using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBoolPropertiesForSheepPicturesAndDnaSample : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ThinhornSheepBioSubmission_IsDnaSampleTaken",
                table: "BioSubmissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ThinhornSheepBioSubmission_IsPicturesTaken",
                table: "BioSubmissions",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThinhornSheepBioSubmission_IsDnaSampleTaken",
                table: "BioSubmissions");

            migrationBuilder.DropColumn(
                name: "ThinhornSheepBioSubmission_IsPicturesTaken",
                table: "BioSubmissions");
        }
    }
}
