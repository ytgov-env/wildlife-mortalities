using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedReportEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeletedReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    HumanReadableId = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    DateLastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateSubmitted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: false),
                    SerializedData = table.Column<string>(type: "nvarchar(MAX)", maxLength: 1073741824, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeletedReports_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeletedReports_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeletedReports_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeletedReports_DeletedById",
                table: "DeletedReports",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_DeletedReports_PersonId",
                table: "DeletedReports",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_DeletedReports_SeasonId",
                table: "DeletedReports",
                column: "SeasonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletedReports");
        }
    }
}
