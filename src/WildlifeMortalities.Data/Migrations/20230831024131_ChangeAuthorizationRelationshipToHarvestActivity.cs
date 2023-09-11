using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAuthorizationRelationshipToHarvestActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityAuthorization");

            migrationBuilder.CreateTable(
                name: "AuthorizationHarvestActivity",
                columns: table => new
                {
                    ActivitiesId = table.Column<int>(type: "int", nullable: false),
                    AuthorizationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationHarvestActivity", x => new { x.ActivitiesId, x.AuthorizationsId });
                    table.ForeignKey(
                        name: "FK_AuthorizationHarvestActivity_Activities_ActivitiesId",
                        column: x => x.ActivitiesId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorizationHarvestActivity_Authorizations_AuthorizationsId",
                        column: x => x.AuthorizationsId,
                        principalTable: "Authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationHarvestActivity_AuthorizationsId",
                table: "AuthorizationHarvestActivity",
                column: "AuthorizationsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorizationHarvestActivity");

            migrationBuilder.CreateTable(
                name: "ActivityAuthorization",
                columns: table => new
                {
                    ActivitiesId = table.Column<int>(type: "int", nullable: false),
                    AuthorizationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityAuthorization", x => new { x.ActivitiesId, x.AuthorizationsId });
                    table.ForeignKey(
                        name: "FK_ActivityAuthorization_Activities_ActivitiesId",
                        column: x => x.ActivitiesId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityAuthorization_Authorizations_AuthorizationsId",
                        column: x => x.AuthorizationsId,
                        principalTable: "Authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAuthorization_AuthorizationsId",
                table: "ActivityAuthorization",
                column: "AuthorizationsId");
        }
    }
}
