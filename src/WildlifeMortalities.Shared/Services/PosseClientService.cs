using System.Net.Http.Json;
using System.Text.Json;
using WildlifeMortalities.Data.Entities.Authorizations;

namespace WildlifeMortalities.Shared.Services;

public class PosseClientService : IPosseClientService
{
    public enum AuthorizationType
    {
        BigGameHuntingLicence_CanadianResident = 10,
        BigGameHuntingLicence_CanadianResidentSpecialGuided = 20,
        BigGameHuntingLicence_NonResident = 30,
        BigGameHuntingLicence_YukonResident = 40,
        BigGameHuntingLicence_YukonResidentSenior = 50,
        BigGameHuntingLicence_YukonResidentFirstNationsOrInuit = 60,
        BigGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior = 70,
        BigGameHuntingLicence_YukonResidentTrapper = 80,
        HuntingPermit_CaribouFortymileFall = 90,
        HuntingPermit_CaribouFortymileSummerPeriodOne = 100,
        HuntingPermit_CaribouFortymileSummerPeriodTwo = 110,
        HuntingPermit_CaribouFortymileSummerPeriodThree = 120,
        HuntingPermit_CaribouFortymileSummerPeriodFour = 130,
        HuntingPermit_CaribouFortymileSummerPeriodFive = 140,
        HuntingPermit_CaribouFortymileSummerPeriodSix = 150,
        HuntingPermit_CaribouFortymileSummerPeriodSeven = 160,
        HuntingPermit_CaribouFortymileSummerPeriodEight = 170,
        HuntingPermit_CaribouFortymileWinter = 180,
        HuntingPermit_CaribouHartRiver = 190,
        HuntingPermit_CaribouNelchina = 200,
        HuntingPermit_ElkAdaptive = 210,
        HuntingPermit_ElkAdaptiveFirstNations = 220,
        HuntingPermit_ElkExclusion = 230,
        HuntingPermit_MooseThreshold = 240,
        HuntingPermit_WoodBisonThreshold = 250,
        HuntingSeal_AmericanBlackBear = 260,
        HuntingSeal_Caribou = 270,
        HuntingSeal_Deer = 280,
        HuntingSeal_Elk = 290,
        HuntingSeal_GrizzlyBear = 300,
        HuntingSeal_Moose = 310,
        HuntingSeal_MountainGoat = 320,
        HuntingSeal_ThinhornSheep = 330,
        HuntingSeal_WoodBison = 340,
        OutfitterAssistantGuideLicence = 350,
        OutfitterChiefGuideLicence = 360,
        PhaHuntingPermit_Caribou = 370,
        PhaHuntingPermit_Deer = 380,
        PhaHuntingPermit_Elk = 390,
        PhaHuntingPermit_Moose = 400,
        PhaHuntingPermit_MountainGoat = 410,
        PhaHuntingPermit_ThinhornSheep = 420,
        PhaHuntingPermit_ThinhornSheepKluane = 430,
        SmallGameHuntingLicence_CanadianResident = 440,
        SmallGameHuntingLicence_CanadianResidentFirstNationsOrInuit = 450,
        SmallGameHuntingLicence_NonResident = 460,
        SmallGameHuntingLicence_NonResidentFirstNationsOrInuit = 470,
        SmallGameHuntingLicence_YukonResident = 480,
        SmallGameHuntingLicence_YukonResidentSenior = 490,
        SmallGameHuntingLicence_YukonResidentFirstNationsOrInuit = 500,
        SmallGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior = 510,
        SpecialGuideLicence = 520,
        TrappingLicence_AssistantTrapper = 530,
        TrappingLicence_AssistantTrapperSenior = 540,
        TrappingLicence_ConcessionHolder = 550,
        TrappingLicence_ConcessionHolderSenior = 560,
        TrappingLicence_GroupConcessionAreaMember = 570,
        TrappingLicence_GroupConcessionAreaMemberSenior = 580,
        WildlifeActPermit = 590
    }

    private static readonly Dictionary<
        AuthorizationType,
        Func<Authorization>
    > s_authorizationMapper =
        new()
        {
            {
                AuthorizationType.BigGameHuntingLicence_CanadianResident,
                () => new BigGameHuntingLicence(BigGameHuntingLicence.LicenceType.CanadianResident)
            },
            {
                AuthorizationType.BigGameHuntingLicence_CanadianResidentSpecialGuided, () =>
                    new BigGameHuntingLicence(
                        BigGameHuntingLicence.LicenceType.CanadianResidentSpecialGuided
                    )
            },
            {
                AuthorizationType.BigGameHuntingLicence_NonResident,
                () => new BigGameHuntingLicence(BigGameHuntingLicence.LicenceType.NonResident)
            },
            {
                AuthorizationType.BigGameHuntingLicence_YukonResident,
                () => new BigGameHuntingLicence(BigGameHuntingLicence.LicenceType.YukonResident)
            },
            {
                AuthorizationType.BigGameHuntingLicence_YukonResidentSenior, () =>
                    new BigGameHuntingLicence(BigGameHuntingLicence.LicenceType.YukonResidentSenior)
            },
            {
                AuthorizationType.BigGameHuntingLicence_YukonResidentFirstNationsOrInuit, () =>
                    new BigGameHuntingLicence(
                        BigGameHuntingLicence.LicenceType.YukonResidentFirstNationsOrInuit
                    )
            },
            {
                AuthorizationType.BigGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior, () =>
                    new BigGameHuntingLicence(
                        BigGameHuntingLicence.LicenceType.YukonResidentFirstNationsOrInuitSenior
                    )
            },
            {
                AuthorizationType.BigGameHuntingLicence_YukonResidentTrapper, () =>
                    new BigGameHuntingLicence(
                        BigGameHuntingLicence.LicenceType.YukonResidentTrapper
                    )
            },
            {
                AuthorizationType.HuntingPermit_CaribouFortymileFall,
                () => new HuntingPermit(HuntingPermit.PermitType.CaribouFortymileFall)
            },
            {
                AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodOne,
                () => new HuntingPermit(HuntingPermit.PermitType.CaribouFortymileSummerPeriodOne)
            },
            {
                AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodTwo,
                () => new HuntingPermit(HuntingPermit.PermitType.CaribouFortymileSummerPeriodTwo)
            },
            {
                AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodThree,
                () => new HuntingPermit(HuntingPermit.PermitType.CaribouFortymileSummerPeriodThree)
            },
            {
                AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodFour,
                () => new HuntingPermit(HuntingPermit.PermitType.CaribouFortymileSummerPeriodFour)
            },
            {
                AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodFive,
                () => new HuntingPermit(HuntingPermit.PermitType.CaribouFortymileSummerPeriodFive)
            },
            {
                AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodSix,
                () => new HuntingPermit(HuntingPermit.PermitType.CaribouFortymileSummerPeriodSix)
            },
            {
                AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodSeven,
                () => new HuntingPermit(HuntingPermit.PermitType.CaribouFortymileSummerPeriodSeven)
            },
            {
                AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodEight,
                () => new HuntingPermit(HuntingPermit.PermitType.CaribouFortymileSummerPeriodEight)
            },
            {
                AuthorizationType.HuntingPermit_CaribouFortymileWinter,
                () => new HuntingPermit(HuntingPermit.PermitType.CaribouFortymileWinter)
            },
            {
                AuthorizationType.HuntingPermit_CaribouHartRiver,
                () => new HuntingPermit(HuntingPermit.PermitType.CaribouHartRiver)
            },
            {
                AuthorizationType.HuntingPermit_CaribouNelchina,
                () => new HuntingPermit(HuntingPermit.PermitType.CaribouNelchina)
            },
            {
                AuthorizationType.HuntingPermit_ElkAdaptive,
                () => new HuntingPermit(HuntingPermit.PermitType.ElkAdaptive)
            },
            {
                AuthorizationType.HuntingPermit_ElkAdaptiveFirstNations,
                () => new HuntingPermit(HuntingPermit.PermitType.ElkAdaptiveFirstNations)
            },
            {
                AuthorizationType.HuntingPermit_ElkExclusion,
                () => new HuntingPermit(HuntingPermit.PermitType.ElkExclusion)
            },
            {
                AuthorizationType.HuntingPermit_MooseThreshold,
                () => new HuntingPermit(HuntingPermit.PermitType.MooseThreshold)
            },
            {
                AuthorizationType.HuntingPermit_WoodBisonThreshold,
                () => new HuntingPermit(HuntingPermit.PermitType.WoodBisonThreshold)
            },
            {
                AuthorizationType.HuntingSeal_AmericanBlackBear,
                () => new HuntingSeal(HuntingSeal.SealType.AmericanBlackBear)
            },
            { AuthorizationType.HuntingSeal_Caribou, () => new HuntingSeal(HuntingSeal.SealType.Caribou) },
            { AuthorizationType.HuntingSeal_Deer, () => new HuntingSeal(HuntingSeal.SealType.Deer) },
            { AuthorizationType.HuntingSeal_Elk, () => new HuntingSeal(HuntingSeal.SealType.Elk) },
            { AuthorizationType.HuntingSeal_GrizzlyBear, () => new HuntingSeal(HuntingSeal.SealType.GrizzlyBear) },
            { AuthorizationType.HuntingSeal_Moose, () => new HuntingSeal(HuntingSeal.SealType.Moose) },
            { AuthorizationType.HuntingSeal_MountainGoat, () => new HuntingSeal(HuntingSeal.SealType.MountainGoat) },
            { AuthorizationType.HuntingSeal_ThinhornSheep, () => new HuntingSeal(HuntingSeal.SealType.ThinhornSheep) },
            { AuthorizationType.HuntingSeal_WoodBison, () => new HuntingSeal(HuntingSeal.SealType.WoodBison) },
            { AuthorizationType.OutfitterAssistantGuideLicence, () => new OutfitterAssistantGuideLicence() },
            { AuthorizationType.OutfitterChiefGuideLicence, () => new OutfitterChiefGuideLicence() },
            {
                AuthorizationType.PhaHuntingPermit_Caribou,
                () => new PhaHuntingPermit(PhaHuntingPermit.PermitType.Caribou)
            },
            { AuthorizationType.PhaHuntingPermit_Deer, () => new PhaHuntingPermit(PhaHuntingPermit.PermitType.Deer) },
            { AuthorizationType.PhaHuntingPermit_Elk, () => new PhaHuntingPermit(PhaHuntingPermit.PermitType.Elk) },
            { AuthorizationType.PhaHuntingPermit_Moose, () => new PhaHuntingPermit(PhaHuntingPermit.PermitType.Moose) },
            {
                AuthorizationType.PhaHuntingPermit_MountainGoat,
                () => new PhaHuntingPermit(PhaHuntingPermit.PermitType.MountainGoat)
            },
            {
                AuthorizationType.PhaHuntingPermit_ThinhornSheep,
                () => new PhaHuntingPermit(PhaHuntingPermit.PermitType.ThinhornSheep)
            },
            {
                AuthorizationType.PhaHuntingPermit_ThinhornSheepKluane,
                () => new PhaHuntingPermit(PhaHuntingPermit.PermitType.ThinhornSheepKluane)
            },
            {
                AuthorizationType.SmallGameHuntingLicence_CanadianResident, () =>
                    new SmallGameHuntingLicence(
                        SmallGameHuntingLicence.LicenceType.CanadianResident
                    )
            },
            {
                AuthorizationType.SmallGameHuntingLicence_CanadianResidentFirstNationsOrInuit, () =>
                    new SmallGameHuntingLicence(
                        SmallGameHuntingLicence.LicenceType.CanadianResidentFirstNationsOrInuit
                    )
            },
            {
                AuthorizationType.SmallGameHuntingLicence_NonResident,
                () => new SmallGameHuntingLicence(SmallGameHuntingLicence.LicenceType.NonResident)
            },
            {
                AuthorizationType.SmallGameHuntingLicence_NonResidentFirstNationsOrInuit, () =>
                    new SmallGameHuntingLicence(
                        SmallGameHuntingLicence.LicenceType.NonResidentFirstNationsOrInuit
                    )
            },
            {
                AuthorizationType.SmallGameHuntingLicence_YukonResident,
                () => new SmallGameHuntingLicence(SmallGameHuntingLicence.LicenceType.YukonResident)
            },
            {
                AuthorizationType.SmallGameHuntingLicence_YukonResidentSenior, () =>
                    new SmallGameHuntingLicence(
                        SmallGameHuntingLicence.LicenceType.YukonResidentSenior
                    )
            },
            {
                AuthorizationType.SmallGameHuntingLicence_YukonResidentFirstNationsOrInuit, () =>
                    new SmallGameHuntingLicence(
                        SmallGameHuntingLicence.LicenceType.YukonResidentFirstNationsOrInuit
                    )
            },
            {
                AuthorizationType.SmallGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior, () =>
                    new SmallGameHuntingLicence(
                        SmallGameHuntingLicence.LicenceType.YukonResidentFirstNationsOrInuitSenior
                    )
            },
            { AuthorizationType.SpecialGuideLicence, () => new SpecialGuideLicence() },
            {
                AuthorizationType.TrappingLicence_AssistantTrapper,
                () => new TrappingLicence(TrappingLicence.LicenceType.AssistantTrapper)
            },
            {
                AuthorizationType.TrappingLicence_AssistantTrapperSenior,
                () => new TrappingLicence(TrappingLicence.LicenceType.AssistantTrapperSenior)
            },
            {
                AuthorizationType.TrappingLicence_ConcessionHolder,
                () => new TrappingLicence(TrappingLicence.LicenceType.ConcessionHolder)
            },
            {
                AuthorizationType.TrappingLicence_ConcessionHolderSenior,
                () => new TrappingLicence(TrappingLicence.LicenceType.ConcessionHolderSenior)
            },
            {
                AuthorizationType.TrappingLicence_GroupConcessionAreaMember,
                () => new TrappingLicence(TrappingLicence.LicenceType.GroupConcessionAreaMember)
            },
            {
                AuthorizationType.TrappingLicence_GroupConcessionAreaMemberSenior, () =>
                    new TrappingLicence(TrappingLicence.LicenceType.GroupConcessionAreaMemberSenior)
            },
            { AuthorizationType.WildlifeActPermit, () => new WildlifeActPermit() }
        };

    private readonly HttpClient _httpClient;

    public PosseClientService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<
        IEnumerable<(string envClientId, Authorization, string? specialGuidedHunterEnvClientId)>
    > RetrieveData(DateTimeOffset modifiedSinceDateTime)
    {
        var datetimeAsString = JsonSerializer.Serialize(modifiedSinceDateTime);
        var results = await _httpClient.GetFromJsonAsync<GetAuthorizationsResponse>(
            $"/authorizations?ModifiedSinceDateTime={datetimeAsString}"
        );

        var authorizations =
            new List<(string envClientId, Authorization, string? specialGuidedHunterEnvClientId)>();
        foreach (var posseAuthorization in results.Authorizations)
        {
            var authorization = s_authorizationMapper[posseAuthorization.Type]();
            authorization.Number = posseAuthorization.Number;
            authorization.ActiveFromDate = posseAuthorization.ActiveFromDateTime;
            authorization.ActiveToDate = posseAuthorization.ActiveToDateTime;

            if (authorization is WildlifeActPermit wildlifeActPermit)
            {
                if (string.IsNullOrEmpty(posseAuthorization.WildlifeActPermitConditions))
                {
                    // Todo throw exception?
                    continue;
                }

                wildlifeActPermit.Conditions = posseAuthorization.WildlifeActPermitConditions;
            }
            else
            {
                if (!string.IsNullOrEmpty(posseAuthorization.WildlifeActPermitConditions))
                {
                    // Todo throw exception?
                }
            }

            if (authorization is SpecialGuideLicence specialGuideLicence)
            {
                if (
                    string.IsNullOrEmpty(
                        posseAuthorization.SpecialGuideLicenceGuidedHunterEnvClientId
                    )
                )
                {
                    // Todo throw exception?
                    continue;
                }
            }
            else
            {
                if (
                    !string.IsNullOrEmpty(
                        posseAuthorization.SpecialGuideLicenceGuidedHunterEnvClientId
                    )
                )
                {
                    // Todo throw exception?
                    continue;
                }
            }

            authorizations.Add(
                (
                    posseAuthorization.EnvClientId,
                    authorization,
                    posseAuthorization.SpecialGuideLicenceGuidedHunterEnvClientId
                )
            );
        }

        return authorizations;
    }

    public class GetAuthorizationsResponse
    {
        public List<AuthorizationDto> Authorizations { get; set; }
    }

    public record AuthorizationDto(
        AuthorizationType Type,
        string EnvClientId,
        string Number,
        string? WildlifeActPermitConditions,
        string? SpecialGuideLicenceGuidedHunterEnvClientId,
        DateTimeOffset? ActiveFromDateTime,
        DateTimeOffset? ActiveToDateTime,
        DateTimeOffset LastModifiedDateTime
    );
}
