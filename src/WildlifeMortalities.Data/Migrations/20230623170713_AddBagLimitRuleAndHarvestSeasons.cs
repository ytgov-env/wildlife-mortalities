using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBagLimitRuleAndHarvestSeasons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authorizations_Activities_ActivityId",
                table: "Authorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Violations_ViolationId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Violations_Activities_ActivityId",
                table: "Violations");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ViolationId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Authorizations_ActivityId",
                table: "Authorizations");

            migrationBuilder.DropColumn(
                name: "ViolationId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Authorizations");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Violations",
                newName: "Severity");

            migrationBuilder.AlterColumn<int>(
                name: "ActivityId",
                table: "Violations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rule",
                table: "Violations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrappingBagLimitEntryId",
                table: "RegisteredTrappingConcessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HuntingBagLimitEntryId",
                table: "GameManagementAreas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedTimestamp",
                table: "Activities",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

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

            migrationBuilder.CreateTable(
                name: "BagLimitEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Species = table.Column<int>(type: "int", nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: true),
                    PeriodStart = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PeriodEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    MaxValuePerPerson = table.Column<int>(type: "int", nullable: false),
                    MaxValueForThreshold = table.Column<int>(type: "int", nullable: true),
                    BagLimitEntryId = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: true),
                    TrappingBagLimitEntry_SeasonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BagLimitEntries", x => x.Id);
                    table.CheckConstraint("CK_BagLimitEntries_Sex_Enum", "[Sex] IN (10, 20, 30)");
                    table.CheckConstraint("CK_BagLimitEntries_Species_Enum", "[Species] IN (100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1201, 1300, 1400, 1500, 1600, 1700, 1701, 1800, 1900, 2000, 2100, 2200, 2300, 2400, 2500, 2600, 2700, 2800, 2900, 3000, 3100, 3200, 3300, 3400, 3500, 3600, 3601, 3602, 3603, 3700, 3800, 3801, 3900, 4000, 4100, 4101, 4102, 4200, 4300, 4400, 4500)");
                    table.ForeignKey(
                        name: "FK_BagLimitEntries_BagLimitEntries_BagLimitEntryId",
                        column: x => x.BagLimitEntryId,
                        principalTable: "BagLimitEntries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BagLimitEntries_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BagLimitEntries_Seasons_TrappingBagLimitEntry_SeasonId",
                        column: x => x.TrappingBagLimitEntry_SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityQueueItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    BagLimitEntryId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityQueueItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityQueueItems_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityQueueItems_BagLimitEntries_BagLimitEntryId",
                        column: x => x.BagLimitEntryId,
                        principalTable: "BagLimitEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BagEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentValue = table.Column<int>(type: "int", nullable: false),
                    SharedValue = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    BagLimitEntryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BagEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BagEntries_BagLimitEntries_BagLimitEntryId",
                        column: x => x.BagLimitEntryId,
                        principalTable: "BagLimitEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BagEntries_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations",
                sql: "[Rule] IN (10, 20, 30)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Violations_Severity_Enum",
                table: "Violations",
                sql: "[Severity] IN (10, 20, 30)");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredTrappingConcessions_TrappingBagLimitEntryId",
                table: "RegisteredTrappingConcessions",
                column: "TrappingBagLimitEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_GameManagementAreas_HuntingBagLimitEntryId",
                table: "GameManagementAreas",
                column: "HuntingBagLimitEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAuthorization_AuthorizationsId",
                table: "ActivityAuthorization",
                column: "AuthorizationsId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityQueueItems_ActivityId",
                table: "ActivityQueueItems",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityQueueItems_BagLimitEntryId",
                table: "ActivityQueueItems",
                column: "BagLimitEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_BagEntries_BagLimitEntryId",
                table: "BagEntries",
                column: "BagLimitEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_BagEntries_PersonId",
                table: "BagEntries",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_BagLimitEntries_BagLimitEntryId",
                table: "BagLimitEntries",
                column: "BagLimitEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_BagLimitEntries_SeasonId",
                table: "BagLimitEntries",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_BagLimitEntries_TrappingBagLimitEntry_SeasonId",
                table: "BagLimitEntries",
                column: "TrappingBagLimitEntry_SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameManagementAreas_BagLimitEntries_HuntingBagLimitEntryId",
                table: "GameManagementAreas",
                column: "HuntingBagLimitEntryId",
                principalTable: "BagLimitEntries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredTrappingConcessions_BagLimitEntries_TrappingBagLimitEntryId",
                table: "RegisteredTrappingConcessions",
                column: "TrappingBagLimitEntryId",
                principalTable: "BagLimitEntries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Violations_Activities_ActivityId",
                table: "Violations",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameManagementAreas_BagLimitEntries_HuntingBagLimitEntryId",
                table: "GameManagementAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredTrappingConcessions_BagLimitEntries_TrappingBagLimitEntryId",
                table: "RegisteredTrappingConcessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Violations_Activities_ActivityId",
                table: "Violations");

            migrationBuilder.DropTable(
                name: "ActivityAuthorization");

            migrationBuilder.DropTable(
                name: "ActivityQueueItems");

            migrationBuilder.DropTable(
                name: "BagEntries");

            migrationBuilder.DropTable(
                name: "BagLimitEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Violations_Rule_Enum",
                table: "Violations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Violations_Severity_Enum",
                table: "Violations");

            migrationBuilder.DropIndex(
                name: "IX_RegisteredTrappingConcessions_TrappingBagLimitEntryId",
                table: "RegisteredTrappingConcessions");

            migrationBuilder.DropIndex(
                name: "IX_GameManagementAreas_HuntingBagLimitEntryId",
                table: "GameManagementAreas");

            migrationBuilder.DropColumn(
                name: "Rule",
                table: "Violations");

            migrationBuilder.DropColumn(
                name: "TrappingBagLimitEntryId",
                table: "RegisteredTrappingConcessions");

            migrationBuilder.DropColumn(
                name: "HuntingBagLimitEntryId",
                table: "GameManagementAreas");

            migrationBuilder.DropColumn(
                name: "CreatedTimestamp",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "Severity",
                table: "Violations",
                newName: "Type");

            migrationBuilder.AlterColumn<int>(
                name: "ActivityId",
                table: "Violations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ViolationId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "Authorizations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ViolationId",
                table: "Reports",
                column: "ViolationId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_ActivityId",
                table: "Authorizations",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_Activities_ActivityId",
                table: "Authorizations",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Violations_ViolationId",
                table: "Reports",
                column: "ViolationId",
                principalTable: "Violations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Violations_Activities_ActivityId",
                table: "Violations",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");
        }
    }
}
