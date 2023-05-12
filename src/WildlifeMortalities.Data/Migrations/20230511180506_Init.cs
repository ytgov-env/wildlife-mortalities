using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppConfigurations",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConfigurations", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "GameManagementAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Zone = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Subzone = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, computedColumnSql: "[Zone] + '-' + [Subzone]", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameManagementAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutfitterAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Area = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutfitterAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastModifiedDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    BadgeNumber = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EnvPersonId = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegisteredTrappingConcessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Area = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredTrappingConcessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FriendlyName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    NameIdentifier = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Settings_IsDarkMode = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DraftReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    DateLastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateSubmitted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SerializedData = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftReports_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvalidAuthorizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PosseId = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Number = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ValidFromDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ValidToDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    ValidationErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvalidAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvalidAuthorizations_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvalidAuthorizations_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ReportId = table.Column<int>(type: "int", nullable: true),
                    HumanWildlifeConflictActivity_ReportId = table.Column<int>(type: "int", nullable: true),
                    GameManagementAreaId = table.Column<int>(type: "int", nullable: true),
                    Landmark = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IndividualHuntedMortalityReportId = table.Column<int>(type: "int", nullable: true),
                    OutfitterGuidedHuntReportId = table.Column<int>(type: "int", nullable: true),
                    SpecialGuidedHuntReportId = table.Column<int>(type: "int", nullable: true),
                    ResearchActivity_ReportId = table.Column<int>(type: "int", nullable: true),
                    TrappedMortalitiesReportId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_GameManagementAreas_GameManagementAreaId",
                        column: x => x.GameManagementAreaId,
                        principalTable: "GameManagementAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Authorizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PosseId = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Number = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ValidFromDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ValidToDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: true),
                    HuntingPermit_Type = table.Column<int>(type: "int", nullable: true),
                    HuntingSeal_Type = table.Column<int>(type: "int", nullable: true),
                    HuntedActivityyId = table.Column<int>(type: "int", nullable: true),
                    PhaHuntingPermit_Type = table.Column<int>(type: "int", nullable: true),
                    HuntCode = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SmallGameHuntingLicence_Type = table.Column<int>(type: "int", nullable: true),
                    GuidedClientId = table.Column<int>(type: "int", nullable: true),
                    TrappingLicence_Type = table.Column<int>(type: "int", nullable: true),
                    RegisteredTrappingConcessionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorizations", x => x.Id);
                    table.CheckConstraint("CK_Authorizations_HuntingPermit_Type_Enum", "[HuntingPermit_Type] IN (10, 20, 30, 40, 50, 60, 70, 80, 90)");
                    table.CheckConstraint("CK_Authorizations_HuntingSeal_Type_Enum", "[HuntingSeal_Type] IN (10, 20, 30, 40, 50, 60, 70, 80, 90)");
                    table.CheckConstraint("CK_Authorizations_PhaHuntingPermit_Type_Enum", "[PhaHuntingPermit_Type] IN (10, 20, 30, 40, 50, 60, 70)");
                    table.CheckConstraint("CK_Authorizations_SmallGameHuntingLicence_Type_Enum", "[SmallGameHuntingLicence_Type] IN (10, 20, 30, 40, 50, 60, 70, 80)");
                    table.CheckConstraint("CK_Authorizations_TrappingLicence_Type_Enum", "[TrappingLicence_Type] IN (10, 20, 30, 40, 50, 60)");
                    table.CheckConstraint("CK_Authorizations_Type_Enum", "[Type] IN (10, 20, 30, 40, 50, 60, 70, 80)");
                    table.ForeignKey(
                        name: "FK_Authorizations_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Authorizations_Activities_HuntedActivityyId",
                        column: x => x.HuntedActivityyId,
                        principalTable: "Activities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Authorizations_People_GuidedClientId",
                        column: x => x.GuidedClientId,
                        principalTable: "People",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Authorizations_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Authorizations_RegisteredTrappingConcessions_RegisteredTrappingConcessionId",
                        column: x => x.RegisteredTrappingConcessionId,
                        principalTable: "RegisteredTrappingConcessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Authorizations_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mortalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    DateOfDeath = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,8)", precision: 10, scale: 8, nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(11,8)", precision: 11, scale: 8, nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    Family = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsShotInConflict = table.Column<bool>(type: "bit", nullable: true),
                    CaribouHerd = table.Column<int>(type: "int", nullable: true),
                    ElkHerd = table.Column<int>(type: "int", nullable: true),
                    BodyColour = table.Column<int>(type: "int", nullable: true),
                    TailColour = table.Column<int>(type: "int", nullable: true),
                    PregnancyStatus = table.Column<int>(type: "int", nullable: true),
                    IsWounded = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mortalities", x => x.Id);
                    table.CheckConstraint("CK_Mortalities_BodyColour_Enum", "[BodyColour] IN (10, 20, 30)");
                    table.CheckConstraint("CK_Mortalities_CaribouHerd_Enum", "[CaribouHerd] IN (10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 290, 300)");
                    table.CheckConstraint("CK_Mortalities_ElkHerd_Enum", "[ElkHerd] IN (10, 20)");
                    table.CheckConstraint("CK_Mortalities_Family_Enum", "[Family] IN (10)");
                    table.CheckConstraint("CK_Mortalities_PregnancyStatus_Enum", "[PregnancyStatus] IN (10, 20, 30)");
                    table.CheckConstraint("CK_Mortalities_Sex_Enum", "[Sex] IN (10, 20, 30)");
                    table.CheckConstraint("CK_Mortalities_TailColour_Enum", "[TailColour] IN (10, 20)");
                    table.ForeignKey(
                        name: "FK_Mortalities_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Violations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Violations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Violations_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BigGameHuntingLicenceOutfitterArea",
                columns: table => new
                {
                    BigGameHuntingLicencesId = table.Column<int>(type: "int", nullable: false),
                    OutfitterAreasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BigGameHuntingLicenceOutfitterArea", x => new { x.BigGameHuntingLicencesId, x.OutfitterAreasId });
                    table.ForeignKey(
                        name: "FK_BigGameHuntingLicenceOutfitterArea_Authorizations_BigGameHuntingLicencesId",
                        column: x => x.BigGameHuntingLicencesId,
                        principalTable: "Authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BigGameHuntingLicenceOutfitterArea_OutfitterAreas_OutfitterAreasId",
                        column: x => x.OutfitterAreasId,
                        principalTable: "OutfitterAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutfitterAreaOutfitterAssistantGuideLicence",
                columns: table => new
                {
                    OutfitterAreasId = table.Column<int>(type: "int", nullable: false),
                    OutfitterAssistantGuideLicencesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutfitterAreaOutfitterAssistantGuideLicence", x => new { x.OutfitterAreasId, x.OutfitterAssistantGuideLicencesId });
                    table.ForeignKey(
                        name: "FK_OutfitterAreaOutfitterAssistantGuideLicence_Authorizations_OutfitterAssistantGuideLicencesId",
                        column: x => x.OutfitterAssistantGuideLicencesId,
                        principalTable: "Authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutfitterAreaOutfitterAssistantGuideLicence_OutfitterAreas_OutfitterAreasId",
                        column: x => x.OutfitterAreasId,
                        principalTable: "OutfitterAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutfitterAreaOutfitterChiefGuideLicence",
                columns: table => new
                {
                    OutfitterAreasId = table.Column<int>(type: "int", nullable: false),
                    OutfitterChiefGuideLicencesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutfitterAreaOutfitterChiefGuideLicence", x => new { x.OutfitterAreasId, x.OutfitterChiefGuideLicencesId });
                    table.ForeignKey(
                        name: "FK_OutfitterAreaOutfitterChiefGuideLicence_Authorizations_OutfitterChiefGuideLicencesId",
                        column: x => x.OutfitterChiefGuideLicencesId,
                        principalTable: "Authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutfitterAreaOutfitterChiefGuideLicence_OutfitterAreas_OutfitterAreasId",
                        column: x => x.OutfitterAreasId,
                        principalTable: "OutfitterAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutfitterAreaSmallGameHuntingLicence",
                columns: table => new
                {
                    OutfitterAreasId = table.Column<int>(type: "int", nullable: false),
                    SmallGameHuntingLicencesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutfitterAreaSmallGameHuntingLicence", x => new { x.OutfitterAreasId, x.SmallGameHuntingLicencesId });
                    table.ForeignKey(
                        name: "FK_OutfitterAreaSmallGameHuntingLicence_Authorizations_SmallGameHuntingLicencesId",
                        column: x => x.SmallGameHuntingLicencesId,
                        principalTable: "Authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutfitterAreaSmallGameHuntingLicence_OutfitterAreas_OutfitterAreasId",
                        column: x => x.OutfitterAreasId,
                        principalTable: "OutfitterAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BioSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequiredOrganicMaterialsStatus = table.Column<int>(type: "int", nullable: false),
                    AnalysisStatus = table.Column<int>(type: "int", nullable: false),
                    DateSubmitted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Age_Years = table.Column<int>(type: "int", nullable: true),
                    Age_Confidence = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SkullCondition = table.Column<int>(type: "int", nullable: true),
                    SkullLengthMillimetres = table.Column<int>(type: "int", nullable: true),
                    SkullWidthMillimetres = table.Column<int>(type: "int", nullable: true),
                    IsSkullProvided = table.Column<bool>(type: "bit", nullable: true),
                    MortalityId = table.Column<int>(type: "int", nullable: true),
                    IsPeltProvided = table.Column<bool>(type: "bit", nullable: true),
                    PeltLengthMillimetres = table.Column<int>(type: "int", nullable: true),
                    PeltWidthMillimetres = table.Column<int>(type: "int", nullable: true),
                    FurbearerSealNumber = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CanadaLynxBioSubmission_MortalityId = table.Column<int>(type: "int", nullable: true),
                    IsIncisorBarProvided = table.Column<bool>(type: "bit", nullable: true),
                    CaribouBioSubmission_MortalityId = table.Column<int>(type: "int", nullable: true),
                    IsHideProvided = table.Column<bool>(type: "bit", nullable: true),
                    IsHeadProvided = table.Column<bool>(type: "bit", nullable: true),
                    ElkBioSubmission_MortalityId = table.Column<int>(type: "int", nullable: true),
                    PeltColour = table.Column<int>(type: "int", nullable: true),
                    GreyWolfBioSubmission_IsPeltProvided = table.Column<bool>(type: "bit", nullable: true),
                    GreyWolfBioSubmission_MortalityId = table.Column<int>(type: "int", nullable: true),
                    GrizzlyBearBioSubmission_SkullCondition = table.Column<int>(type: "int", nullable: true),
                    GrizzlyBearBioSubmission_SkullLengthMillimetres = table.Column<int>(type: "int", nullable: true),
                    GrizzlyBearBioSubmission_SkullWidthMillimetres = table.Column<int>(type: "int", nullable: true),
                    IsEvidenceOfSexAttached = table.Column<bool>(type: "bit", nullable: true),
                    GrizzlyBearBioSubmission_IsSkullProvided = table.Column<bool>(type: "bit", nullable: true),
                    GrizzlyBearBioSubmission_MortalityId = table.Column<int>(type: "int", nullable: true),
                    IsHornsProvided = table.Column<bool>(type: "bit", nullable: true),
                    MountainGoatBioSubmission_IsHeadProvided = table.Column<bool>(type: "bit", nullable: true),
                    HornMeasured = table.Column<int>(type: "int", nullable: true),
                    BroomedStatus = table.Column<int>(type: "int", nullable: true),
                    HornTotalLengthMillimetres = table.Column<int>(type: "int", nullable: true),
                    HornBaseCircumferenceMillimetres = table.Column<int>(type: "int", nullable: true),
                    HornTipSpreadMillimetres = table.Column<int>(type: "int", nullable: true),
                    MountainGoatBioSubmission_MortalityId = table.Column<int>(type: "int", nullable: true),
                    MuleDeerBioSubmission_IsHideProvided = table.Column<bool>(type: "bit", nullable: true),
                    MuleDeerBioSubmission_IsHeadProvided = table.Column<bool>(type: "bit", nullable: true),
                    IsAntlersProvided = table.Column<bool>(type: "bit", nullable: true),
                    MuleDeerBioSubmission_MortalityId = table.Column<int>(type: "int", nullable: true),
                    ThinhornSheepBioSubmission_IsHornsProvided = table.Column<bool>(type: "bit", nullable: true),
                    ThinhornSheepBioSubmission_IsHeadProvided = table.Column<bool>(type: "bit", nullable: true),
                    HornLengthToThirdAnnulusMillimetres = table.Column<int>(type: "int", nullable: true),
                    IsFullCurl = table.Column<bool>(type: "bit", nullable: true),
                    PlugNumber = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ThinhornSheepBioSubmission_HornMeasured = table.Column<int>(type: "int", nullable: true),
                    ThinhornSheepBioSubmission_BroomedStatus = table.Column<int>(type: "int", nullable: true),
                    ThinhornSheepBioSubmission_HornTotalLengthMillimetres = table.Column<int>(type: "int", nullable: true),
                    ThinhornSheepBioSubmission_HornBaseCircumferenceMillimetres = table.Column<int>(type: "int", nullable: true),
                    ThinhornSheepBioSubmission_HornTipSpreadMillimetres = table.Column<int>(type: "int", nullable: true),
                    ThinhornSheepBioSubmission_MortalityId = table.Column<int>(type: "int", nullable: true),
                    WhiteTailedDeerBioSubmission_IsHideProvided = table.Column<bool>(type: "bit", nullable: true),
                    WhiteTailedDeerBioSubmission_IsHeadProvided = table.Column<bool>(type: "bit", nullable: true),
                    WhiteTailedDeerBioSubmission_IsAntlersProvided = table.Column<bool>(type: "bit", nullable: true),
                    WhiteTailedDeerBioSubmission_MortalityId = table.Column<int>(type: "int", nullable: true),
                    WolverineBioSubmission_IsPeltProvided = table.Column<bool>(type: "bit", nullable: true),
                    WolverineBioSubmission_MortalityId = table.Column<int>(type: "int", nullable: true),
                    WoodBisonBioSubmission_IsIncisorBarProvided = table.Column<bool>(type: "bit", nullable: true),
                    WoodBisonBioSubmission_MortalityId = table.Column<int>(type: "int", nullable: true),
                    MountainGoatBioSubmission_HornMeasurementEntries = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThinhornSheepBioSubmission_HornMeasurementEntries = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BioSubmissions", x => x.Id);
                    table.CheckConstraint("CK_BioSubmissions_Age_Confidence_Enum", "[Age_Confidence] IN (10, 20, 30)");
                    table.CheckConstraint("CK_BioSubmissions_AnalysisStatus_Enum", "[AnalysisStatus] IN (10, 20, 30)");
                    table.CheckConstraint("CK_BioSubmissions_BroomedStatus_Enum", "[BroomedStatus] IN (10, 20, 30, 40)");
                    table.CheckConstraint("CK_BioSubmissions_GrizzlyBearBioSubmission_SkullCondition_Enum", "[GrizzlyBearBioSubmission_SkullCondition] IN (10, 20, 30, 40)");
                    table.CheckConstraint("CK_BioSubmissions_HornMeasured_Enum", "[HornMeasured] IN (10, 20)");
                    table.CheckConstraint("CK_BioSubmissions_PeltColour_Enum", "[PeltColour] IN (10, 20, 30, 40)");
                    table.CheckConstraint("CK_BioSubmissions_RequiredOrganicMaterialsStatus_Enum", "[RequiredOrganicMaterialsStatus] IN (10, 20, 30, 40)");
                    table.CheckConstraint("CK_BioSubmissions_SkullCondition_Enum", "[SkullCondition] IN (10, 20, 30, 40)");
                    table.CheckConstraint("CK_BioSubmissions_ThinhornSheepBioSubmission_BroomedStatus_Enum", "[ThinhornSheepBioSubmission_BroomedStatus] IN (10, 20, 30, 40)");
                    table.CheckConstraint("CK_BioSubmissions_ThinhornSheepBioSubmission_HornMeasured_Enum", "[ThinhornSheepBioSubmission_HornMeasured] IN (10, 20)");
                    table.ForeignKey(
                        name: "FK_BioSubmissions_Mortalities_CanadaLynxBioSubmission_MortalityId",
                        column: x => x.CanadaLynxBioSubmission_MortalityId,
                        principalTable: "Mortalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BioSubmissions_Mortalities_CaribouBioSubmission_MortalityId",
                        column: x => x.CaribouBioSubmission_MortalityId,
                        principalTable: "Mortalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BioSubmissions_Mortalities_ElkBioSubmission_MortalityId",
                        column: x => x.ElkBioSubmission_MortalityId,
                        principalTable: "Mortalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BioSubmissions_Mortalities_GreyWolfBioSubmission_MortalityId",
                        column: x => x.GreyWolfBioSubmission_MortalityId,
                        principalTable: "Mortalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BioSubmissions_Mortalities_GrizzlyBearBioSubmission_MortalityId",
                        column: x => x.GrizzlyBearBioSubmission_MortalityId,
                        principalTable: "Mortalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BioSubmissions_Mortalities_MortalityId",
                        column: x => x.MortalityId,
                        principalTable: "Mortalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BioSubmissions_Mortalities_MountainGoatBioSubmission_MortalityId",
                        column: x => x.MountainGoatBioSubmission_MortalityId,
                        principalTable: "Mortalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BioSubmissions_Mortalities_MuleDeerBioSubmission_MortalityId",
                        column: x => x.MuleDeerBioSubmission_MortalityId,
                        principalTable: "Mortalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BioSubmissions_Mortalities_ThinhornSheepBioSubmission_MortalityId",
                        column: x => x.ThinhornSheepBioSubmission_MortalityId,
                        principalTable: "Mortalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BioSubmissions_Mortalities_WhiteTailedDeerBioSubmission_MortalityId",
                        column: x => x.WhiteTailedDeerBioSubmission_MortalityId,
                        principalTable: "Mortalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BioSubmissions_Mortalities_WolverineBioSubmission_MortalityId",
                        column: x => x.WolverineBioSubmission_MortalityId,
                        principalTable: "Mortalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BioSubmissions_Mortalities_WoodBisonBioSubmission_MortalityId",
                        column: x => x.WoodBisonBioSubmission_MortalityId,
                        principalTable: "Mortalities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    HumanReadableId = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    DateSubmitted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedById = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ViolationId = table.Column<int>(type: "int", nullable: true),
                    ConservationOfficerId = table.Column<int>(type: "int", nullable: true),
                    HumanWildlifeConflictNumber = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OutfitterGuidedHuntReport_HuntStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OutfitterGuidedHuntReport_HuntEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChiefGuideId = table.Column<int>(type: "int", nullable: true),
                    OutfitterAreaId = table.Column<int>(type: "int", nullable: true),
                    OutfitterGuidedHuntReport_Result = table.Column<int>(type: "int", nullable: true),
                    OutfitterGuidedHuntReport_ClientId = table.Column<int>(type: "int", nullable: true),
                    HuntStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HuntEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GuideId = table.Column<int>(type: "int", nullable: true),
                    Result = table.Column<int>(type: "int", nullable: true),
                    SpecialGuidedHuntReport_ClientId = table.Column<int>(type: "int", nullable: true),
                    RegisteredTrappingConcessionId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    CollarNumber = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    IndividualHuntedMortalityReport_ClientId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.CheckConstraint("CK_Reports_OutfitterGuidedHuntReport_Result_Enum", "[OutfitterGuidedHuntReport_Result] IN (10, 20, 30)");
                    table.CheckConstraint("CK_Reports_Result_Enum", "[Result] IN (10, 20, 30)");
                    table.ForeignKey(
                        name: "FK_Reports_OutfitterAreas_OutfitterAreaId",
                        column: x => x.OutfitterAreaId,
                        principalTable: "OutfitterAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_People_ChiefGuideId",
                        column: x => x.ChiefGuideId,
                        principalTable: "People",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_People_ClientId",
                        column: x => x.ClientId,
                        principalTable: "People",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_People_ConservationOfficerId",
                        column: x => x.ConservationOfficerId,
                        principalTable: "People",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_People_GuideId",
                        column: x => x.GuideId,
                        principalTable: "People",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_People_IndividualHuntedMortalityReport_ClientId",
                        column: x => x.IndividualHuntedMortalityReport_ClientId,
                        principalTable: "People",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_People_OutfitterGuidedHuntReport_ClientId",
                        column: x => x.OutfitterGuidedHuntReport_ClientId,
                        principalTable: "People",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_People_SpecialGuidedHuntReport_ClientId",
                        column: x => x.SpecialGuidedHuntReport_ClientId,
                        principalTable: "People",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_RegisteredTrappingConcessions_RegisteredTrappingConcessionId",
                        column: x => x.RegisteredTrappingConcessionId,
                        principalTable: "RegisteredTrappingConcessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Users_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Violations_ViolationId",
                        column: x => x.ViolationId,
                        principalTable: "Violations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AssistantGuideOutfitterGuidedHuntReport",
                columns: table => new
                {
                    AssistantGuidesId = table.Column<int>(type: "int", nullable: false),
                    OutfitterGuidedHuntReportsAsAssistantGuideId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssistantGuideOutfitterGuidedHuntReport", x => new { x.AssistantGuidesId, x.OutfitterGuidedHuntReportsAsAssistantGuideId });
                    table.ForeignKey(
                        name: "FK_AssistantGuideOutfitterGuidedHuntReport_People_AssistantGuidesId",
                        column: x => x.AssistantGuidesId,
                        principalTable: "People",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AssistantGuideOutfitterGuidedHuntReport_Reports_OutfitterGuidedHuntReportsAsAssistantGuideId",
                        column: x => x.OutfitterGuidedHuntReportsAsAssistantGuideId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReportPdf",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportPdf", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportPdf_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_GameManagementAreaId",
                table: "Activities",
                column: "GameManagementAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_HumanWildlifeConflictActivity_ReportId",
                table: "Activities",
                column: "HumanWildlifeConflictActivity_ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_IndividualHuntedMortalityReportId",
                table: "Activities",
                column: "IndividualHuntedMortalityReportId",
                unique: true,
                filter: "[IndividualHuntedMortalityReportId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_OutfitterGuidedHuntReportId",
                table: "Activities",
                column: "OutfitterGuidedHuntReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ReportId",
                table: "Activities",
                column: "ReportId",
                unique: true,
                filter: "[ReportId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ResearchActivity_ReportId",
                table: "Activities",
                column: "ResearchActivity_ReportId",
                unique: true,
                filter: "[ReportId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_SpecialGuidedHuntReportId",
                table: "Activities",
                column: "SpecialGuidedHuntReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_TrappedMortalitiesReportId",
                table: "Activities",
                column: "TrappedMortalitiesReportId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistantGuideOutfitterGuidedHuntReport_OutfitterGuidedHuntReportsAsAssistantGuideId",
                table: "AssistantGuideOutfitterGuidedHuntReport",
                column: "OutfitterGuidedHuntReportsAsAssistantGuideId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_ActivityId",
                table: "Authorizations",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_GuidedClientId",
                table: "Authorizations",
                column: "GuidedClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_HuntedActivityyId",
                table: "Authorizations",
                column: "HuntedActivityyId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_PersonId",
                table: "Authorizations",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_RegisteredTrappingConcessionId",
                table: "Authorizations",
                column: "RegisteredTrappingConcessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_SeasonId",
                table: "Authorizations",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_BigGameHuntingLicenceOutfitterArea_OutfitterAreasId",
                table: "BigGameHuntingLicenceOutfitterArea",
                column: "OutfitterAreasId");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_CanadaLynxBioSubmission_MortalityId",
                table: "BioSubmissions",
                column: "CanadaLynxBioSubmission_MortalityId",
                unique: true,
                filter: "[CanadaLynxBioSubmission_MortalityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_CaribouBioSubmission_MortalityId",
                table: "BioSubmissions",
                column: "CaribouBioSubmission_MortalityId",
                unique: true,
                filter: "[CaribouBioSubmission_MortalityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_ElkBioSubmission_MortalityId",
                table: "BioSubmissions",
                column: "ElkBioSubmission_MortalityId",
                unique: true,
                filter: "[ElkBioSubmission_MortalityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_FurbearerSealNumber",
                table: "BioSubmissions",
                column: "FurbearerSealNumber",
                unique: true,
                filter: "[FurbearerSealNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_GreyWolfBioSubmission_MortalityId",
                table: "BioSubmissions",
                column: "GreyWolfBioSubmission_MortalityId",
                unique: true,
                filter: "[GreyWolfBioSubmission_MortalityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_GrizzlyBearBioSubmission_MortalityId",
                table: "BioSubmissions",
                column: "GrizzlyBearBioSubmission_MortalityId",
                unique: true,
                filter: "[GrizzlyBearBioSubmission_MortalityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_MortalityId",
                table: "BioSubmissions",
                column: "MortalityId",
                unique: true,
                filter: "[MortalityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_MountainGoatBioSubmission_MortalityId",
                table: "BioSubmissions",
                column: "MountainGoatBioSubmission_MortalityId",
                unique: true,
                filter: "[MountainGoatBioSubmission_MortalityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_MuleDeerBioSubmission_MortalityId",
                table: "BioSubmissions",
                column: "MuleDeerBioSubmission_MortalityId",
                unique: true,
                filter: "[MuleDeerBioSubmission_MortalityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_ThinhornSheepBioSubmission_MortalityId",
                table: "BioSubmissions",
                column: "ThinhornSheepBioSubmission_MortalityId",
                unique: true,
                filter: "[ThinhornSheepBioSubmission_MortalityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_WhiteTailedDeerBioSubmission_MortalityId",
                table: "BioSubmissions",
                column: "WhiteTailedDeerBioSubmission_MortalityId",
                unique: true,
                filter: "[WhiteTailedDeerBioSubmission_MortalityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_WolverineBioSubmission_MortalityId",
                table: "BioSubmissions",
                column: "WolverineBioSubmission_MortalityId",
                unique: true,
                filter: "[WolverineBioSubmission_MortalityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_WoodBisonBioSubmission_MortalityId",
                table: "BioSubmissions",
                column: "WoodBisonBioSubmission_MortalityId",
                unique: true,
                filter: "[WoodBisonBioSubmission_MortalityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DraftReports_PersonId",
                table: "DraftReports",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_InvalidAuthorizations_PersonId",
                table: "InvalidAuthorizations",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_InvalidAuthorizations_SeasonId",
                table: "InvalidAuthorizations",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Mortalities_ActivityId",
                table: "Mortalities",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutfitterAreaOutfitterAssistantGuideLicence_OutfitterAssistantGuideLicencesId",
                table: "OutfitterAreaOutfitterAssistantGuideLicence",
                column: "OutfitterAssistantGuideLicencesId");

            migrationBuilder.CreateIndex(
                name: "IX_OutfitterAreaOutfitterChiefGuideLicence_OutfitterChiefGuideLicencesId",
                table: "OutfitterAreaOutfitterChiefGuideLicence",
                column: "OutfitterChiefGuideLicencesId");

            migrationBuilder.CreateIndex(
                name: "IX_OutfitterAreas_Area",
                table: "OutfitterAreas",
                column: "Area",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutfitterAreaSmallGameHuntingLicence_SmallGameHuntingLicencesId",
                table: "OutfitterAreaSmallGameHuntingLicence",
                column: "SmallGameHuntingLicencesId");

            migrationBuilder.CreateIndex(
                name: "IX_People_BadgeNumber",
                table: "People",
                column: "BadgeNumber",
                unique: true,
                filter: "[BadgeNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_People_EnvPersonId",
                table: "People",
                column: "EnvPersonId",
                unique: true,
                filter: "[EnvPersonId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ReportPdf_ReportId",
                table: "ReportPdf",
                column: "ReportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ChiefGuideId",
                table: "Reports",
                column: "ChiefGuideId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ClientId",
                table: "Reports",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ConservationOfficerId",
                table: "Reports",
                column: "ConservationOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CreatedById",
                table: "Reports",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_GuideId",
                table: "Reports",
                column: "GuideId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_HumanReadableId",
                table: "Reports",
                column: "HumanReadableId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_IndividualHuntedMortalityReport_ClientId",
                table: "Reports",
                column: "IndividualHuntedMortalityReport_ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_LastModifiedById",
                table: "Reports",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_OutfitterAreaId",
                table: "Reports",
                column: "OutfitterAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_OutfitterGuidedHuntReport_ClientId",
                table: "Reports",
                column: "OutfitterGuidedHuntReport_ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PersonId",
                table: "Reports",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_RegisteredTrappingConcessionId",
                table: "Reports",
                column: "RegisteredTrappingConcessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SeasonId",
                table: "Reports",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SpecialGuidedHuntReport_ClientId",
                table: "Reports",
                column: "SpecialGuidedHuntReport_ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ViolationId",
                table: "Reports",
                column: "ViolationId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_StartDate_EndDate",
                table: "Seasons",
                columns: new[] { "StartDate", "EndDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Violations_ActivityId",
                table: "Violations",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Reports_HumanWildlifeConflictActivity_ReportId",
                table: "Activities",
                column: "HumanWildlifeConflictActivity_ReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Reports_IndividualHuntedMortalityReportId",
                table: "Activities",
                column: "IndividualHuntedMortalityReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Reports_OutfitterGuidedHuntReportId",
                table: "Activities",
                column: "OutfitterGuidedHuntReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Reports_ReportId",
                table: "Activities",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Reports_ResearchActivity_ReportId",
                table: "Activities",
                column: "ResearchActivity_ReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Reports_SpecialGuidedHuntReportId",
                table: "Activities",
                column: "SpecialGuidedHuntReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Reports_TrappedMortalitiesReportId",
                table: "Activities",
                column: "TrappedMortalitiesReportId",
                principalTable: "Reports",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_GameManagementAreas_GameManagementAreaId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Reports_HumanWildlifeConflictActivity_ReportId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Reports_IndividualHuntedMortalityReportId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Reports_OutfitterGuidedHuntReportId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Reports_ReportId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Reports_ResearchActivity_ReportId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Reports_SpecialGuidedHuntReportId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Reports_TrappedMortalitiesReportId",
                table: "Activities");

            migrationBuilder.DropTable(
                name: "AppConfigurations");

            migrationBuilder.DropTable(
                name: "AssistantGuideOutfitterGuidedHuntReport");

            migrationBuilder.DropTable(
                name: "BigGameHuntingLicenceOutfitterArea");

            migrationBuilder.DropTable(
                name: "BioSubmissions");

            migrationBuilder.DropTable(
                name: "DraftReports");

            migrationBuilder.DropTable(
                name: "InvalidAuthorizations");

            migrationBuilder.DropTable(
                name: "OutfitterAreaOutfitterAssistantGuideLicence");

            migrationBuilder.DropTable(
                name: "OutfitterAreaOutfitterChiefGuideLicence");

            migrationBuilder.DropTable(
                name: "OutfitterAreaSmallGameHuntingLicence");

            migrationBuilder.DropTable(
                name: "ReportPdf");

            migrationBuilder.DropTable(
                name: "Mortalities");

            migrationBuilder.DropTable(
                name: "Authorizations");

            migrationBuilder.DropTable(
                name: "GameManagementAreas");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "OutfitterAreas");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "RegisteredTrappingConcessions");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Violations");

            migrationBuilder.DropTable(
                name: "Activities");
        }
    }
}
