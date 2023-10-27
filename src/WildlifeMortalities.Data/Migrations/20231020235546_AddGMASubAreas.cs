using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGMASubAreas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameManagementSubAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubArea = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    GameManagementAreaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameManagementSubAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameManagementSubAreas_GameManagementAreas_GameManagementAreaId",
                        column: x => x.GameManagementAreaId,
                        principalTable: "GameManagementAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameManagementSubAreas_GameManagementAreaId",
                table: "GameManagementSubAreas",
                column: "GameManagementAreaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameManagementSubAreas");
        }
    }
}
