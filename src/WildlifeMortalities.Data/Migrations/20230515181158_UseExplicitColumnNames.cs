using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WildlifeMortalities.Data.Migrations
{
    /// <inheritdoc />
    public partial class UseExplicitColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_GameManagementAreas_GameManagementAreaId",
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
                name: "FK_Activities_Reports_SpecialGuidedHuntReportId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Reports_TrappedMortalitiesReportId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Authorizations_Activities_HuntedActivityId",
                table: "Authorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Authorizations_People_GuidedClientId",
                table: "Authorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Authorizations_RegisteredTrappingConcessions_RegisteredTrappingConcessionId",
                table: "Authorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_BioSubmissions_Mortalities_MortalityId",
                table: "BioSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_OutfitterAreas_OutfitterAreaId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_People_ChiefGuideId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_People_ConservationOfficerId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_People_GuideId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_People_IndividualHuntedMortalityReport_ClientId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_People_PersonId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_RegisteredTrappingConcessions_RegisteredTrappingConcessionId",
                table: "Reports");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Reports_Result_Enum",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_People_BadgeNumber",
                table: "People");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_BodyColour_Enum",
                table: "Mortalities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_CaribouHerd_Enum",
                table: "Mortalities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_ElkHerd_Enum",
                table: "Mortalities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_PregnancyStatus_Enum",
                table: "Mortalities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_TailColour_Enum",
                table: "Mortalities");

            migrationBuilder.DropIndex(
                name: "IX_BioSubmissions_MortalityId",
                table: "BioSubmissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BioSubmissions_BroomedStatus_Enum",
                table: "BioSubmissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BioSubmissions_HornMeasured_Enum",
                table: "BioSubmissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BioSubmissions_PeltColour_Enum",
                table: "BioSubmissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BioSubmissions_SkullCondition_Enum",
                table: "BioSubmissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Authorizations_Type_Enum",
                table: "Authorizations");

            migrationBuilder.DropIndex(
                name: "IX_Activities_IndividualHuntedMortalityReportId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ReportId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ResearchActivity_ReportId",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "Result",
                table: "Reports",
                newName: "SpecialGuidedHuntReport_Result");

            migrationBuilder.RenameColumn(
                name: "RegisteredTrappingConcessionId",
                table: "Reports",
                newName: "TrappedMortalitiesReport_RegisteredTrappingConcessionId");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Reports",
                newName: "IndividualHuntedMortalityReport_PersonId");

            migrationBuilder.RenameColumn(
                name: "OutfitterAreaId",
                table: "Reports",
                newName: "OutfitterGuidedHuntReport_OutfitterAreaId");

            migrationBuilder.RenameColumn(
                name: "HuntStartDate",
                table: "Reports",
                newName: "SpecialGuidedHuntReport_HuntStartDate");

            migrationBuilder.RenameColumn(
                name: "HuntEndDate",
                table: "Reports",
                newName: "SpecialGuidedHuntReport_HuntEndDate");

            migrationBuilder.RenameColumn(
                name: "HumanWildlifeConflictNumber",
                table: "Reports",
                newName: "HumanWildlifeConflictMortalityReport_HumanWildlifeConflictNumber");

            migrationBuilder.RenameColumn(
                name: "GuideId",
                table: "Reports",
                newName: "SpecialGuidedHuntReport_GuideId");

            migrationBuilder.RenameColumn(
                name: "ConservationOfficerId",
                table: "Reports",
                newName: "HumanWildlifeConflictMortalityReport_ConservationOfficerId");

            migrationBuilder.RenameColumn(
                name: "CollarNumber",
                table: "Reports",
                newName: "CollaredMortalityReport_CollarNumber");

            migrationBuilder.RenameColumn(
                name: "ChiefGuideId",
                table: "Reports",
                newName: "OutfitterGuidedHuntReport_ChiefGuideId");

            migrationBuilder.RenameColumn(
                name: "IndividualHuntedMortalityReport_ClientId",
                table: "Reports",
                newName: "TrappedMortalitiesReport_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_RegisteredTrappingConcessionId",
                table: "Reports",
                newName: "IX_Reports_TrappedMortalitiesReport_RegisteredTrappingConcessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_PersonId",
                table: "Reports",
                newName: "IX_Reports_IndividualHuntedMortalityReport_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_OutfitterAreaId",
                table: "Reports",
                newName: "IX_Reports_OutfitterGuidedHuntReport_OutfitterAreaId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_IndividualHuntedMortalityReport_ClientId",
                table: "Reports",
                newName: "IX_Reports_TrappedMortalitiesReport_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_GuideId",
                table: "Reports",
                newName: "IX_Reports_SpecialGuidedHuntReport_GuideId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ConservationOfficerId",
                table: "Reports",
                newName: "IX_Reports_HumanWildlifeConflictMortalityReport_ConservationOfficerId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ChiefGuideId",
                table: "Reports",
                newName: "IX_Reports_OutfitterGuidedHuntReport_ChiefGuideId");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "People",
                newName: "ConservationOfficer_LastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "People",
                newName: "ConservationOfficer_FirstName");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "People",
                newName: "Client_BirthDate");

            migrationBuilder.RenameColumn(
                name: "BadgeNumber",
                table: "People",
                newName: "ConservationOfficer_BadgeNumber");

            migrationBuilder.RenameColumn(
                name: "TailColour",
                table: "Mortalities",
                newName: "ThinhornSheepMortality_TailColour");

            migrationBuilder.RenameColumn(
                name: "PregnancyStatus",
                table: "Mortalities",
                newName: "WoodBisonMortality_PregnancyStatus");

            migrationBuilder.RenameColumn(
                name: "IsWounded",
                table: "Mortalities",
                newName: "WoodBisonMortality_IsWounded");

            migrationBuilder.RenameColumn(
                name: "IsShotInConflict",
                table: "Mortalities",
                newName: "GrizzlyBearMortality_IsShotInConflict");

            migrationBuilder.RenameColumn(
                name: "ElkHerd",
                table: "Mortalities",
                newName: "ElkMortality_Herd");

            migrationBuilder.RenameColumn(
                name: "CaribouHerd",
                table: "Mortalities",
                newName: "CaribouMortality_Herd");

            migrationBuilder.RenameColumn(
                name: "BodyColour",
                table: "Mortalities",
                newName: "ThinhornSheepMortality_BodyColour");

            migrationBuilder.RenameColumn(
                name: "SkullWidthMillimetres",
                table: "BioSubmissions",
                newName: "AmericanBlackBearBioSubmission_SkullWidthMillimetres");

            migrationBuilder.RenameColumn(
                name: "SkullLengthMillimetres",
                table: "BioSubmissions",
                newName: "AmericanBlackBearBioSubmission_SkullLengthMillimetres");

            migrationBuilder.RenameColumn(
                name: "SkullCondition",
                table: "BioSubmissions",
                newName: "AmericanBlackBearBioSubmission_SkullCondition");

            migrationBuilder.RenameColumn(
                name: "PlugNumber",
                table: "BioSubmissions",
                newName: "ThinhornSheepBioSubmission_PlugNumber");

            migrationBuilder.RenameColumn(
                name: "PeltWidthMillimetres",
                table: "BioSubmissions",
                newName: "CanadaLynxBioSubmission_PeltWidthMillimetres");

            migrationBuilder.RenameColumn(
                name: "PeltLengthMillimetres",
                table: "BioSubmissions",
                newName: "CanadaLynxBioSubmission_PeltLengthMillimetres");

            migrationBuilder.RenameColumn(
                name: "PeltColour",
                table: "BioSubmissions",
                newName: "GreyWolfBioSubmission_PeltColour");

            migrationBuilder.RenameColumn(
                name: "MortalityId",
                table: "BioSubmissions",
                newName: "AmericanBlackBearBioSubmission_MortalityId");

            migrationBuilder.RenameColumn(
                name: "IsSkullProvided",
                table: "BioSubmissions",
                newName: "AmericanBlackBearBioSubmission_IsSkullProvided");

            migrationBuilder.RenameColumn(
                name: "IsPeltProvided",
                table: "BioSubmissions",
                newName: "CanadaLynxBioSubmission_IsPeltProvided");

            migrationBuilder.RenameColumn(
                name: "IsIncisorBarProvided",
                table: "BioSubmissions",
                newName: "CaribouBioSubmission_IsIncisorBarProvided");

            migrationBuilder.RenameColumn(
                name: "IsHornsProvided",
                table: "BioSubmissions",
                newName: "MountainGoatBioSubmission_IsHornsProvided");

            migrationBuilder.RenameColumn(
                name: "IsHideProvided",
                table: "BioSubmissions",
                newName: "ElkBioSubmission_IsHideProvided");

            migrationBuilder.RenameColumn(
                name: "IsHeadProvided",
                table: "BioSubmissions",
                newName: "ElkBioSubmission_IsHeadProvided");

            migrationBuilder.RenameColumn(
                name: "IsFullCurl",
                table: "BioSubmissions",
                newName: "ThinhornSheepBioSubmission_IsFullCurl");

            migrationBuilder.RenameColumn(
                name: "IsEvidenceOfSexAttached",
                table: "BioSubmissions",
                newName: "GrizzlyBearBioSubmission_IsEvidenceOfSexAttached");

            migrationBuilder.RenameColumn(
                name: "IsAntlersProvided",
                table: "BioSubmissions",
                newName: "MuleDeerBioSubmission_IsAntlersProvided");

            migrationBuilder.RenameColumn(
                name: "HornTotalLengthMillimetres",
                table: "BioSubmissions",
                newName: "MountainGoatBioSubmission_HornTotalLengthMillimetres");

            migrationBuilder.RenameColumn(
                name: "HornTipSpreadMillimetres",
                table: "BioSubmissions",
                newName: "MountainGoatBioSubmission_HornTipSpreadMillimetres");

            migrationBuilder.RenameColumn(
                name: "HornMeasured",
                table: "BioSubmissions",
                newName: "MountainGoatBioSubmission_HornMeasured");

            migrationBuilder.RenameColumn(
                name: "HornLengthToThirdAnnulusMillimetres",
                table: "BioSubmissions",
                newName: "ThinhornSheepBioSubmission_HornLengthToThirdAnnulusMillimetres");

            migrationBuilder.RenameColumn(
                name: "HornBaseCircumferenceMillimetres",
                table: "BioSubmissions",
                newName: "MountainGoatBioSubmission_HornBaseCircumferenceMillimetres");

            migrationBuilder.RenameColumn(
                name: "BroomedStatus",
                table: "BioSubmissions",
                newName: "MountainGoatBioSubmission_BroomedStatus");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Authorizations",
                newName: "BigGameHuntingLicence_Type");

            migrationBuilder.RenameColumn(
                name: "RegisteredTrappingConcessionId",
                table: "Authorizations",
                newName: "TrappingLicence_RegisteredTrappingConcessionId");

            migrationBuilder.RenameColumn(
                name: "HuntedActivityId",
                table: "Authorizations",
                newName: "HuntingSeal_HuntedActivityId");

            migrationBuilder.RenameColumn(
                name: "HuntCode",
                table: "Authorizations",
                newName: "PhaHuntingPermit_HuntCode");

            migrationBuilder.RenameColumn(
                name: "GuidedClientId",
                table: "Authorizations",
                newName: "SpecialGuideLicence_GuidedClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Authorizations_RegisteredTrappingConcessionId",
                table: "Authorizations",
                newName: "IX_Authorizations_TrappingLicence_RegisteredTrappingConcessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Authorizations_HuntedActivityId",
                table: "Authorizations",
                newName: "IX_Authorizations_HuntingSeal_HuntedActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Authorizations_GuidedClientId",
                table: "Authorizations",
                newName: "IX_Authorizations_SpecialGuideLicence_GuidedClientId");

            migrationBuilder.RenameColumn(
                name: "TrappedMortalitiesReportId",
                table: "Activities",
                newName: "TrappedActivity_TrappedMortalitiesReportId");

            migrationBuilder.RenameColumn(
                name: "SpecialGuidedHuntReportId",
                table: "Activities",
                newName: "HuntedActivity_SpecialGuidedHuntReportId");

            migrationBuilder.RenameColumn(
                name: "ReportId",
                table: "Activities",
                newName: "CollaredActivity_ReportId");

            migrationBuilder.RenameColumn(
                name: "OutfitterGuidedHuntReportId",
                table: "Activities",
                newName: "HuntedActivity_OutfitterGuidedHuntReportId");

            migrationBuilder.RenameColumn(
                name: "Landmark",
                table: "Activities",
                newName: "HuntedActivity_Landmark");

            migrationBuilder.RenameColumn(
                name: "IndividualHuntedMortalityReportId",
                table: "Activities",
                newName: "HuntedActivity_IndividualHuntedMortalityReportId");

            migrationBuilder.RenameColumn(
                name: "GameManagementAreaId",
                table: "Activities",
                newName: "HuntedActivity_GameManagementAreaId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_TrappedMortalitiesReportId",
                table: "Activities",
                newName: "IX_Activities_TrappedActivity_TrappedMortalitiesReportId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_SpecialGuidedHuntReportId",
                table: "Activities",
                newName: "IX_Activities_HuntedActivity_SpecialGuidedHuntReportId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_OutfitterGuidedHuntReportId",
                table: "Activities",
                newName: "IX_Activities_HuntedActivity_OutfitterGuidedHuntReportId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_GameManagementAreaId",
                table: "Activities",
                newName: "IX_Activities_HuntedActivity_GameManagementAreaId");

            migrationBuilder.AddColumn<string>(
                name: "Client_FirstName",
                table: "People",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Client_LastName",
                table: "People",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AmericanBlackBearMortality_IsShotInConflict",
                table: "Mortalities",
                type: "bit",
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Reports_SpecialGuidedHuntReport_Result_Enum",
                table: "Reports",
                sql: "[SpecialGuidedHuntReport_Result] IN (10, 20, 30)");

            migrationBuilder.CreateIndex(
                name: "IX_People_ConservationOfficer_BadgeNumber",
                table: "People",
                column: "ConservationOfficer_BadgeNumber",
                unique: true,
                filter: "[ConservationOfficer_BadgeNumber] IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_CaribouMortality_Herd_Enum",
                table: "Mortalities",
                sql: "[CaribouMortality_Herd] IN (10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 290, 300)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_ElkMortality_Herd_Enum",
                table: "Mortalities",
                sql: "[ElkMortality_Herd] IN (10, 20)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_ThinhornSheepMortality_BodyColour_Enum",
                table: "Mortalities",
                sql: "[ThinhornSheepMortality_BodyColour] IN (10, 20, 30)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_ThinhornSheepMortality_TailColour_Enum",
                table: "Mortalities",
                sql: "[ThinhornSheepMortality_TailColour] IN (10, 20)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_WoodBisonMortality_PregnancyStatus_Enum",
                table: "Mortalities",
                sql: "[WoodBisonMortality_PregnancyStatus] IN (10, 20, 30)");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_AmericanBlackBearBioSubmission_MortalityId",
                table: "BioSubmissions",
                column: "AmericanBlackBearBioSubmission_MortalityId",
                unique: true,
                filter: "[AmericanBlackBearBioSubmission_MortalityId] IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BioSubmissions_AmericanBlackBearBioSubmission_SkullCondition_Enum",
                table: "BioSubmissions",
                sql: "[AmericanBlackBearBioSubmission_SkullCondition] IN (10, 20, 30, 40)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BioSubmissions_GreyWolfBioSubmission_PeltColour_Enum",
                table: "BioSubmissions",
                sql: "[GreyWolfBioSubmission_PeltColour] IN (10, 20, 30, 40)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BioSubmissions_MountainGoatBioSubmission_BroomedStatus_Enum",
                table: "BioSubmissions",
                sql: "[MountainGoatBioSubmission_BroomedStatus] IN (10, 20, 30, 40)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BioSubmissions_MountainGoatBioSubmission_HornMeasured_Enum",
                table: "BioSubmissions",
                sql: "[MountainGoatBioSubmission_HornMeasured] IN (10, 20)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Authorizations_BigGameHuntingLicence_Type_Enum",
                table: "Authorizations",
                sql: "[BigGameHuntingLicence_Type] IN (10, 20, 30, 40, 50, 60, 70, 80)");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CollaredActivity_ReportId",
                table: "Activities",
                column: "CollaredActivity_ReportId",
                unique: true,
                filter: "[CollaredActivity_ReportId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_HuntedActivity_IndividualHuntedMortalityReportId",
                table: "Activities",
                column: "HuntedActivity_IndividualHuntedMortalityReportId",
                unique: true,
                filter: "[HuntedActivity_IndividualHuntedMortalityReportId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ResearchActivity_ReportId",
                table: "Activities",
                column: "ResearchActivity_ReportId",
                unique: true,
                filter: "[ResearchActivity_ReportId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_GameManagementAreas_HuntedActivity_GameManagementAreaId",
                table: "Activities",
                column: "HuntedActivity_GameManagementAreaId",
                principalTable: "GameManagementAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Reports_CollaredActivity_ReportId",
                table: "Activities",
                column: "CollaredActivity_ReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Reports_HuntedActivity_IndividualHuntedMortalityReportId",
                table: "Activities",
                column: "HuntedActivity_IndividualHuntedMortalityReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Reports_HuntedActivity_OutfitterGuidedHuntReportId",
                table: "Activities",
                column: "HuntedActivity_OutfitterGuidedHuntReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Reports_HuntedActivity_SpecialGuidedHuntReportId",
                table: "Activities",
                column: "HuntedActivity_SpecialGuidedHuntReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Reports_TrappedActivity_TrappedMortalitiesReportId",
                table: "Activities",
                column: "TrappedActivity_TrappedMortalitiesReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_Activities_HuntingSeal_HuntedActivityId",
                table: "Authorizations",
                column: "HuntingSeal_HuntedActivityId",
                principalTable: "Activities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_People_SpecialGuideLicence_GuidedClientId",
                table: "Authorizations",
                column: "SpecialGuideLicence_GuidedClientId",
                principalTable: "People",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_RegisteredTrappingConcessions_TrappingLicence_RegisteredTrappingConcessionId",
                table: "Authorizations",
                column: "TrappingLicence_RegisteredTrappingConcessionId",
                principalTable: "RegisteredTrappingConcessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BioSubmissions_Mortalities_AmericanBlackBearBioSubmission_MortalityId",
                table: "BioSubmissions",
                column: "AmericanBlackBearBioSubmission_MortalityId",
                principalTable: "Mortalities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_OutfitterAreas_OutfitterGuidedHuntReport_OutfitterAreaId",
                table: "Reports",
                column: "OutfitterGuidedHuntReport_OutfitterAreaId",
                principalTable: "OutfitterAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_People_HumanWildlifeConflictMortalityReport_ConservationOfficerId",
                table: "Reports",
                column: "HumanWildlifeConflictMortalityReport_ConservationOfficerId",
                principalTable: "People",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_People_IndividualHuntedMortalityReport_PersonId",
                table: "Reports",
                column: "IndividualHuntedMortalityReport_PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_People_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports",
                column: "OutfitterGuidedHuntReport_ChiefGuideId",
                principalTable: "People",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_People_SpecialGuidedHuntReport_GuideId",
                table: "Reports",
                column: "SpecialGuidedHuntReport_GuideId",
                principalTable: "People",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_People_TrappedMortalitiesReport_ClientId",
                table: "Reports",
                column: "TrappedMortalitiesReport_ClientId",
                principalTable: "People",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_RegisteredTrappingConcessions_TrappedMortalitiesReport_RegisteredTrappingConcessionId",
                table: "Reports",
                column: "TrappedMortalitiesReport_RegisteredTrappingConcessionId",
                principalTable: "RegisteredTrappingConcessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_GameManagementAreas_HuntedActivity_GameManagementAreaId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Reports_CollaredActivity_ReportId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Reports_HuntedActivity_IndividualHuntedMortalityReportId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Reports_HuntedActivity_OutfitterGuidedHuntReportId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Reports_HuntedActivity_SpecialGuidedHuntReportId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Reports_TrappedActivity_TrappedMortalitiesReportId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Authorizations_Activities_HuntingSeal_HuntedActivityId",
                table: "Authorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Authorizations_People_SpecialGuideLicence_GuidedClientId",
                table: "Authorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Authorizations_RegisteredTrappingConcessions_TrappingLicence_RegisteredTrappingConcessionId",
                table: "Authorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_BioSubmissions_Mortalities_AmericanBlackBearBioSubmission_MortalityId",
                table: "BioSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_OutfitterAreas_OutfitterGuidedHuntReport_OutfitterAreaId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_People_HumanWildlifeConflictMortalityReport_ConservationOfficerId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_People_IndividualHuntedMortalityReport_PersonId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_People_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_People_SpecialGuidedHuntReport_GuideId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_People_TrappedMortalitiesReport_ClientId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_RegisteredTrappingConcessions_TrappedMortalitiesReport_RegisteredTrappingConcessionId",
                table: "Reports");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Reports_SpecialGuidedHuntReport_Result_Enum",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_People_ConservationOfficer_BadgeNumber",
                table: "People");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_CaribouMortality_Herd_Enum",
                table: "Mortalities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_ElkMortality_Herd_Enum",
                table: "Mortalities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_ThinhornSheepMortality_BodyColour_Enum",
                table: "Mortalities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_ThinhornSheepMortality_TailColour_Enum",
                table: "Mortalities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mortalities_WoodBisonMortality_PregnancyStatus_Enum",
                table: "Mortalities");

            migrationBuilder.DropIndex(
                name: "IX_BioSubmissions_AmericanBlackBearBioSubmission_MortalityId",
                table: "BioSubmissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BioSubmissions_AmericanBlackBearBioSubmission_SkullCondition_Enum",
                table: "BioSubmissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BioSubmissions_GreyWolfBioSubmission_PeltColour_Enum",
                table: "BioSubmissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BioSubmissions_MountainGoatBioSubmission_BroomedStatus_Enum",
                table: "BioSubmissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BioSubmissions_MountainGoatBioSubmission_HornMeasured_Enum",
                table: "BioSubmissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Authorizations_BigGameHuntingLicence_Type_Enum",
                table: "Authorizations");

            migrationBuilder.DropIndex(
                name: "IX_Activities_CollaredActivity_ReportId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_HuntedActivity_IndividualHuntedMortalityReportId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ResearchActivity_ReportId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Client_FirstName",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Client_LastName",
                table: "People");

            migrationBuilder.DropColumn(
                name: "AmericanBlackBearMortality_IsShotInConflict",
                table: "Mortalities");

            migrationBuilder.RenameColumn(
                name: "TrappedMortalitiesReport_RegisteredTrappingConcessionId",
                table: "Reports",
                newName: "RegisteredTrappingConcessionId");

            migrationBuilder.RenameColumn(
                name: "SpecialGuidedHuntReport_Result",
                table: "Reports",
                newName: "Result");

            migrationBuilder.RenameColumn(
                name: "SpecialGuidedHuntReport_HuntStartDate",
                table: "Reports",
                newName: "HuntStartDate");

            migrationBuilder.RenameColumn(
                name: "SpecialGuidedHuntReport_HuntEndDate",
                table: "Reports",
                newName: "HuntEndDate");

            migrationBuilder.RenameColumn(
                name: "SpecialGuidedHuntReport_GuideId",
                table: "Reports",
                newName: "GuideId");

            migrationBuilder.RenameColumn(
                name: "OutfitterGuidedHuntReport_OutfitterAreaId",
                table: "Reports",
                newName: "OutfitterAreaId");

            migrationBuilder.RenameColumn(
                name: "OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports",
                newName: "ChiefGuideId");

            migrationBuilder.RenameColumn(
                name: "IndividualHuntedMortalityReport_PersonId",
                table: "Reports",
                newName: "PersonId");

            migrationBuilder.RenameColumn(
                name: "HumanWildlifeConflictMortalityReport_HumanWildlifeConflictNumber",
                table: "Reports",
                newName: "HumanWildlifeConflictNumber");

            migrationBuilder.RenameColumn(
                name: "HumanWildlifeConflictMortalityReport_ConservationOfficerId",
                table: "Reports",
                newName: "ConservationOfficerId");

            migrationBuilder.RenameColumn(
                name: "CollaredMortalityReport_CollarNumber",
                table: "Reports",
                newName: "CollarNumber");

            migrationBuilder.RenameColumn(
                name: "TrappedMortalitiesReport_ClientId",
                table: "Reports",
                newName: "IndividualHuntedMortalityReport_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_TrappedMortalitiesReport_RegisteredTrappingConcessionId",
                table: "Reports",
                newName: "IX_Reports_RegisteredTrappingConcessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_TrappedMortalitiesReport_ClientId",
                table: "Reports",
                newName: "IX_Reports_IndividualHuntedMortalityReport_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_SpecialGuidedHuntReport_GuideId",
                table: "Reports",
                newName: "IX_Reports_GuideId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_OutfitterGuidedHuntReport_OutfitterAreaId",
                table: "Reports",
                newName: "IX_Reports_OutfitterAreaId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_OutfitterGuidedHuntReport_ChiefGuideId",
                table: "Reports",
                newName: "IX_Reports_ChiefGuideId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_IndividualHuntedMortalityReport_PersonId",
                table: "Reports",
                newName: "IX_Reports_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_HumanWildlifeConflictMortalityReport_ConservationOfficerId",
                table: "Reports",
                newName: "IX_Reports_ConservationOfficerId");

            migrationBuilder.RenameColumn(
                name: "ConservationOfficer_LastName",
                table: "People",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "ConservationOfficer_FirstName",
                table: "People",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "ConservationOfficer_BadgeNumber",
                table: "People",
                newName: "BadgeNumber");

            migrationBuilder.RenameColumn(
                name: "Client_BirthDate",
                table: "People",
                newName: "BirthDate");

            migrationBuilder.RenameColumn(
                name: "WoodBisonMortality_PregnancyStatus",
                table: "Mortalities",
                newName: "PregnancyStatus");

            migrationBuilder.RenameColumn(
                name: "WoodBisonMortality_IsWounded",
                table: "Mortalities",
                newName: "IsWounded");

            migrationBuilder.RenameColumn(
                name: "ThinhornSheepMortality_TailColour",
                table: "Mortalities",
                newName: "TailColour");

            migrationBuilder.RenameColumn(
                name: "ThinhornSheepMortality_BodyColour",
                table: "Mortalities",
                newName: "BodyColour");

            migrationBuilder.RenameColumn(
                name: "GrizzlyBearMortality_IsShotInConflict",
                table: "Mortalities",
                newName: "IsShotInConflict");

            migrationBuilder.RenameColumn(
                name: "ElkMortality_Herd",
                table: "Mortalities",
                newName: "ElkHerd");

            migrationBuilder.RenameColumn(
                name: "CaribouMortality_Herd",
                table: "Mortalities",
                newName: "CaribouHerd");

            migrationBuilder.RenameColumn(
                name: "ThinhornSheepBioSubmission_PlugNumber",
                table: "BioSubmissions",
                newName: "PlugNumber");

            migrationBuilder.RenameColumn(
                name: "ThinhornSheepBioSubmission_IsFullCurl",
                table: "BioSubmissions",
                newName: "IsFullCurl");

            migrationBuilder.RenameColumn(
                name: "ThinhornSheepBioSubmission_HornLengthToThirdAnnulusMillimetres",
                table: "BioSubmissions",
                newName: "HornLengthToThirdAnnulusMillimetres");

            migrationBuilder.RenameColumn(
                name: "MuleDeerBioSubmission_IsAntlersProvided",
                table: "BioSubmissions",
                newName: "IsAntlersProvided");

            migrationBuilder.RenameColumn(
                name: "MountainGoatBioSubmission_IsHornsProvided",
                table: "BioSubmissions",
                newName: "IsHornsProvided");

            migrationBuilder.RenameColumn(
                name: "MountainGoatBioSubmission_HornTotalLengthMillimetres",
                table: "BioSubmissions",
                newName: "HornTotalLengthMillimetres");

            migrationBuilder.RenameColumn(
                name: "MountainGoatBioSubmission_HornTipSpreadMillimetres",
                table: "BioSubmissions",
                newName: "HornTipSpreadMillimetres");

            migrationBuilder.RenameColumn(
                name: "MountainGoatBioSubmission_HornMeasured",
                table: "BioSubmissions",
                newName: "HornMeasured");

            migrationBuilder.RenameColumn(
                name: "MountainGoatBioSubmission_HornBaseCircumferenceMillimetres",
                table: "BioSubmissions",
                newName: "HornBaseCircumferenceMillimetres");

            migrationBuilder.RenameColumn(
                name: "MountainGoatBioSubmission_BroomedStatus",
                table: "BioSubmissions",
                newName: "BroomedStatus");

            migrationBuilder.RenameColumn(
                name: "GrizzlyBearBioSubmission_IsEvidenceOfSexAttached",
                table: "BioSubmissions",
                newName: "IsEvidenceOfSexAttached");

            migrationBuilder.RenameColumn(
                name: "GreyWolfBioSubmission_PeltColour",
                table: "BioSubmissions",
                newName: "PeltColour");

            migrationBuilder.RenameColumn(
                name: "ElkBioSubmission_IsHideProvided",
                table: "BioSubmissions",
                newName: "IsHideProvided");

            migrationBuilder.RenameColumn(
                name: "ElkBioSubmission_IsHeadProvided",
                table: "BioSubmissions",
                newName: "IsHeadProvided");

            migrationBuilder.RenameColumn(
                name: "CaribouBioSubmission_IsIncisorBarProvided",
                table: "BioSubmissions",
                newName: "IsIncisorBarProvided");

            migrationBuilder.RenameColumn(
                name: "CanadaLynxBioSubmission_PeltWidthMillimetres",
                table: "BioSubmissions",
                newName: "PeltWidthMillimetres");

            migrationBuilder.RenameColumn(
                name: "CanadaLynxBioSubmission_PeltLengthMillimetres",
                table: "BioSubmissions",
                newName: "PeltLengthMillimetres");

            migrationBuilder.RenameColumn(
                name: "CanadaLynxBioSubmission_IsPeltProvided",
                table: "BioSubmissions",
                newName: "IsPeltProvided");

            migrationBuilder.RenameColumn(
                name: "AmericanBlackBearBioSubmission_SkullWidthMillimetres",
                table: "BioSubmissions",
                newName: "SkullWidthMillimetres");

            migrationBuilder.RenameColumn(
                name: "AmericanBlackBearBioSubmission_SkullLengthMillimetres",
                table: "BioSubmissions",
                newName: "SkullLengthMillimetres");

            migrationBuilder.RenameColumn(
                name: "AmericanBlackBearBioSubmission_SkullCondition",
                table: "BioSubmissions",
                newName: "SkullCondition");

            migrationBuilder.RenameColumn(
                name: "AmericanBlackBearBioSubmission_MortalityId",
                table: "BioSubmissions",
                newName: "MortalityId");

            migrationBuilder.RenameColumn(
                name: "AmericanBlackBearBioSubmission_IsSkullProvided",
                table: "BioSubmissions",
                newName: "IsSkullProvided");

            migrationBuilder.RenameColumn(
                name: "TrappingLicence_RegisteredTrappingConcessionId",
                table: "Authorizations",
                newName: "RegisteredTrappingConcessionId");

            migrationBuilder.RenameColumn(
                name: "SpecialGuideLicence_GuidedClientId",
                table: "Authorizations",
                newName: "GuidedClientId");

            migrationBuilder.RenameColumn(
                name: "PhaHuntingPermit_HuntCode",
                table: "Authorizations",
                newName: "HuntCode");

            migrationBuilder.RenameColumn(
                name: "HuntingSeal_HuntedActivityId",
                table: "Authorizations",
                newName: "HuntedActivityId");

            migrationBuilder.RenameColumn(
                name: "BigGameHuntingLicence_Type",
                table: "Authorizations",
                newName: "Type");

            migrationBuilder.RenameIndex(
                name: "IX_Authorizations_TrappingLicence_RegisteredTrappingConcessionId",
                table: "Authorizations",
                newName: "IX_Authorizations_RegisteredTrappingConcessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Authorizations_SpecialGuideLicence_GuidedClientId",
                table: "Authorizations",
                newName: "IX_Authorizations_GuidedClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Authorizations_HuntingSeal_HuntedActivityId",
                table: "Authorizations",
                newName: "IX_Authorizations_HuntedActivityId");

            migrationBuilder.RenameColumn(
                name: "TrappedActivity_TrappedMortalitiesReportId",
                table: "Activities",
                newName: "TrappedMortalitiesReportId");

            migrationBuilder.RenameColumn(
                name: "HuntedActivity_SpecialGuidedHuntReportId",
                table: "Activities",
                newName: "SpecialGuidedHuntReportId");

            migrationBuilder.RenameColumn(
                name: "HuntedActivity_OutfitterGuidedHuntReportId",
                table: "Activities",
                newName: "OutfitterGuidedHuntReportId");

            migrationBuilder.RenameColumn(
                name: "HuntedActivity_Landmark",
                table: "Activities",
                newName: "Landmark");

            migrationBuilder.RenameColumn(
                name: "HuntedActivity_IndividualHuntedMortalityReportId",
                table: "Activities",
                newName: "IndividualHuntedMortalityReportId");

            migrationBuilder.RenameColumn(
                name: "HuntedActivity_GameManagementAreaId",
                table: "Activities",
                newName: "GameManagementAreaId");

            migrationBuilder.RenameColumn(
                name: "CollaredActivity_ReportId",
                table: "Activities",
                newName: "ReportId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_TrappedActivity_TrappedMortalitiesReportId",
                table: "Activities",
                newName: "IX_Activities_TrappedMortalitiesReportId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_HuntedActivity_SpecialGuidedHuntReportId",
                table: "Activities",
                newName: "IX_Activities_SpecialGuidedHuntReportId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_HuntedActivity_OutfitterGuidedHuntReportId",
                table: "Activities",
                newName: "IX_Activities_OutfitterGuidedHuntReportId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_HuntedActivity_GameManagementAreaId",
                table: "Activities",
                newName: "IX_Activities_GameManagementAreaId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Reports_Result_Enum",
                table: "Reports",
                sql: "[Result] IN (10, 20, 30)");

            migrationBuilder.CreateIndex(
                name: "IX_People_BadgeNumber",
                table: "People",
                column: "BadgeNumber",
                unique: true,
                filter: "[BadgeNumber] IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_BodyColour_Enum",
                table: "Mortalities",
                sql: "[BodyColour] IN (10, 20, 30)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_CaribouHerd_Enum",
                table: "Mortalities",
                sql: "[CaribouHerd] IN (10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 290, 300)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_ElkHerd_Enum",
                table: "Mortalities",
                sql: "[ElkHerd] IN (10, 20)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_PregnancyStatus_Enum",
                table: "Mortalities",
                sql: "[PregnancyStatus] IN (10, 20, 30)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mortalities_TailColour_Enum",
                table: "Mortalities",
                sql: "[TailColour] IN (10, 20)");

            migrationBuilder.CreateIndex(
                name: "IX_BioSubmissions_MortalityId",
                table: "BioSubmissions",
                column: "MortalityId",
                unique: true,
                filter: "[MortalityId] IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BioSubmissions_BroomedStatus_Enum",
                table: "BioSubmissions",
                sql: "[BroomedStatus] IN (10, 20, 30, 40)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BioSubmissions_HornMeasured_Enum",
                table: "BioSubmissions",
                sql: "[HornMeasured] IN (10, 20)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BioSubmissions_PeltColour_Enum",
                table: "BioSubmissions",
                sql: "[PeltColour] IN (10, 20, 30, 40)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BioSubmissions_SkullCondition_Enum",
                table: "BioSubmissions",
                sql: "[SkullCondition] IN (10, 20, 30, 40)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Authorizations_Type_Enum",
                table: "Authorizations",
                sql: "[Type] IN (10, 20, 30, 40, 50, 60, 70, 80)");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_IndividualHuntedMortalityReportId",
                table: "Activities",
                column: "IndividualHuntedMortalityReportId",
                unique: true,
                filter: "[IndividualHuntedMortalityReportId] IS NOT NULL");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_GameManagementAreas_GameManagementAreaId",
                table: "Activities",
                column: "GameManagementAreaId",
                principalTable: "GameManagementAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_Activities_HuntedActivityId",
                table: "Authorizations",
                column: "HuntedActivityId",
                principalTable: "Activities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_People_GuidedClientId",
                table: "Authorizations",
                column: "GuidedClientId",
                principalTable: "People",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_RegisteredTrappingConcessions_RegisteredTrappingConcessionId",
                table: "Authorizations",
                column: "RegisteredTrappingConcessionId",
                principalTable: "RegisteredTrappingConcessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BioSubmissions_Mortalities_MortalityId",
                table: "BioSubmissions",
                column: "MortalityId",
                principalTable: "Mortalities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_OutfitterAreas_OutfitterAreaId",
                table: "Reports",
                column: "OutfitterAreaId",
                principalTable: "OutfitterAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_People_ChiefGuideId",
                table: "Reports",
                column: "ChiefGuideId",
                principalTable: "People",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_People_ConservationOfficerId",
                table: "Reports",
                column: "ConservationOfficerId",
                principalTable: "People",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_People_GuideId",
                table: "Reports",
                column: "GuideId",
                principalTable: "People",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_People_IndividualHuntedMortalityReport_ClientId",
                table: "Reports",
                column: "IndividualHuntedMortalityReport_ClientId",
                principalTable: "People",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_People_PersonId",
                table: "Reports",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_RegisteredTrappingConcessions_RegisteredTrappingConcessionId",
                table: "Reports",
                column: "RegisteredTrappingConcessionId",
                principalTable: "RegisteredTrappingConcessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
