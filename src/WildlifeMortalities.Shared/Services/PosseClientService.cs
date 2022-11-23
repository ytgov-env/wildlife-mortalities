using System.Net.Http.Json;
using System.Text.Json.Serialization;
using WildlifeMortalities.Data.Entities.Authorizations;

namespace WildlifeMortalities.Shared.Services;

public class PosseClientService : IPosseClientService
{
    private readonly HttpClient _httpClient;

    private static readonly Dictionary<AuthorizationType, Func<Authorization>> s_authorizationMapper = new()
    {
        {
            AuthorizationType.BigGameHuntingLicence_CanadianResident,
            () => new BigGameHuntingLicence(BigGameHuntingLicence.LicenceType.CanadianResident)
        },
        { AuthorizationType.BigGameHuntingLicence_CanadianResidentSpecialGuided, () => new BigGameHuntingLicence { } },
        { AuthorizationType.BigGameHuntingLicence_NonResident, () => new BigGameHuntingLicence { } },
        { AuthorizationType.BigGameHuntingLicence_YukonResident, () => new BigGameHuntingLicence { } },
        { AuthorizationType.BigGameHuntingLicence_YukonResidentSenior, () => new BigGameHuntingLicence { } },
        {
            AuthorizationType.BigGameHuntingLicence_YukonResidentFirstNationsOrInuit,
            () => new BigGameHuntingLicence { }
        },
        {
            AuthorizationType.BigGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior,
            () => new BigGameHuntingLicence { }
        },
        { AuthorizationType.BigGameHuntingLicence_YukonResidentTrapper, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_CaribouFortymileFall, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodOne, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodTwo, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodThree, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodFour, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodFive, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodSix, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodSeven, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_CaribouFortymileSummerPeriodEight, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_CaribouFortymileWinter, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_CaribouHartRiver, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_CaribouNelchina, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_ElkAdaptive, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_ElkAdaptiveFirstNations, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_ElkExclusion, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_MooseThreshold, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingPermit_WoodBisonThreshold, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingSeal_AmericanBlackBear, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingSeal_Caribou, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingSeal_Deer, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingSeal_Elk, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingSeal_GrizzlyBear, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingSeal_Moose, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingSeal_MountainGoat, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingSeal_ThinhornSheep, () => new BigGameHuntingLicence { } },
        { AuthorizationType.HuntingSeal_WoodBison, () => new BigGameHuntingLicence { } },
        { AuthorizationType.OutfitterAssistantGuideLicence, () => new BigGameHuntingLicence { } },
        { AuthorizationType.OutfitterChiefGuideLicence, () => new BigGameHuntingLicence { } },
        { AuthorizationType.PhaHuntingPermit_Caribou, () => new BigGameHuntingLicence { } },
        { AuthorizationType.PhaHuntingPermit_Deer, () => new BigGameHuntingLicence { } },
        { AuthorizationType.PhaHuntingPermit_Elk, () => new BigGameHuntingLicence { } },
        { AuthorizationType.PhaHuntingPermit_Moose, () => new BigGameHuntingLicence { } },
        { AuthorizationType.PhaHuntingPermit_MountainGoat, () => new BigGameHuntingLicence { } },
        { AuthorizationType.PhaHuntingPermit_ThinhornSheep, () => new BigGameHuntingLicence { } },
        { AuthorizationType.PhaHuntingPermit_ThinhornSheepKluane, () => new BigGameHuntingLicence { } },
        { AuthorizationType.SmallGameHuntingLicence_CanadianResident, () => new BigGameHuntingLicence { } },
        {
            AuthorizationType.SmallGameHuntingLicence_CanadianResidentFirstNationsOrInuit,
            () => new BigGameHuntingLicence { }
        },
        { AuthorizationType.SmallGameHuntingLicence_NonResident, () => new BigGameHuntingLicence { } },
        {
            AuthorizationType.SmallGameHuntingLicence_NonResidentFirstNationsOrInuit,
            () => new BigGameHuntingLicence { }
        },
        { AuthorizationType.SmallGameHuntingLicence_YukonResident, () => new BigGameHuntingLicence { } },
        { AuthorizationType.SmallGameHuntingLicence_YukonResidentSenior, () => new BigGameHuntingLicence { } },
        {
            AuthorizationType.SmallGameHuntingLicence_YukonResidentFirstNationsOrInuit,
            () => new BigGameHuntingLicence { }
        },
        {
            AuthorizationType.SmallGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior,
            () => new BigGameHuntingLicence { }
        },
        { AuthorizationType.SpecialGuideLicence, () => new BigGameHuntingLicence { } },
        { AuthorizationType.TrappingLicence_AssistantTrapper, () => new BigGameHuntingLicence { } },
        { AuthorizationType.TrappingLicence_AssistantTrapperSenior, () => new BigGameHuntingLicence { } },
        { AuthorizationType.TrappingLicence_ConcessionHolder, () => new BigGameHuntingLicence { } },
        { AuthorizationType.TrappingLicence_ConcessionHolderSenior, () => new BigGameHuntingLicence { } },
        { AuthorizationType.TrappingLicence_GroupConcessionAreaMember, () => new BigGameHuntingLicence { } },
        { AuthorizationType.TrappingLicence_GroupConcessionAreaMemberSenior, () => new TrappingLicence() { } },
        { AuthorizationType.WildlifeActPermit, () => new WildlifeActPermit { } }
    };

    public PosseClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<(string,Authorization)>> RetrieveData(DateTimeOffset modifiedSinceDateTime)
    {
        var datetimeAsString = System.Text.Json.JsonSerializer.Serialize(modifiedSinceDateTime);
        var results = await _httpClient.GetFromJsonAsync<GetAuthorizationsResponse>($"/authorizations?ModifiedSinceDateTime={datetimeAsString}");

        var returnObject = new List<(string, Authorization)>();
        foreach (var posseAuthorization in results.Authorizations)
        {
            var authorization = s_authorizationMapper[posseAuthorization.Type]();
            authorization.Number = posseAuthorization.Number;
            authorization.ValidFromDate = posseAuthorization.ActiveFromDateTime;
            authorization.ValidToDate = posseAuthorization.ActiveToDateTime;

            if (authorization is WildlifeActPermit wildlifeActPermit)
            {
                if (string.IsNullOrEmpty((posseAuthorization.WildlifeActPermitConditions)) == true)
                {
                    continue;
                    // Todo throw exception?
                }

                wildlifeActPermit.Conditions = posseAuthorization.WildlifeActPermitConditions;
            }

            if (authorization is SpecialGuideLicence specialGuideLicence)
            {

            }

            returnObject.Add((posseAuthorization.EnvClientId,authorization));
        }

        return returnObject;
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
        DateTimeOffset ActiveFromDateTime,
        DateTimeOffset ActiveToDateTime,
        DateTimeOffset LastModifiedDateTime
    );

    public enum AuthorizationType
    {
        BigGameHuntingLicence_CanadianResident = 10,
        BigGameHuntingLicence_CanadianResidentSpecialGuided,
        BigGameHuntingLicence_NonResident,
        BigGameHuntingLicence_YukonResident,
        BigGameHuntingLicence_YukonResidentSenior,
        BigGameHuntingLicence_YukonResidentFirstNationsOrInuit,
        BigGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior,
        BigGameHuntingLicence_YukonResidentTrapper,
        HuntingPermit_CaribouFortymileFall,
        HuntingPermit_CaribouFortymileSummerPeriodOne,
        HuntingPermit_CaribouFortymileSummerPeriodTwo,
        HuntingPermit_CaribouFortymileSummerPeriodThree,
        HuntingPermit_CaribouFortymileSummerPeriodFour,
        HuntingPermit_CaribouFortymileSummerPeriodFive,
        HuntingPermit_CaribouFortymileSummerPeriodSix,
        HuntingPermit_CaribouFortymileSummerPeriodSeven,
        HuntingPermit_CaribouFortymileSummerPeriodEight,
        HuntingPermit_CaribouFortymileWinter,
        HuntingPermit_CaribouHartRiver,
        HuntingPermit_CaribouNelchina,
        HuntingPermit_ElkAdaptive,
        HuntingPermit_ElkAdaptiveFirstNations,
        HuntingPermit_ElkExclusion,
        HuntingPermit_MooseThreshold,
        HuntingPermit_WoodBisonThreshold,
        HuntingSeal_AmericanBlackBear,
        HuntingSeal_Caribou,
        HuntingSeal_Deer,
        HuntingSeal_Elk,
        HuntingSeal_GrizzlyBear,
        HuntingSeal_Moose,
        HuntingSeal_MountainGoat,
        HuntingSeal_ThinhornSheep,
        HuntingSeal_WoodBison,
        OutfitterAssistantGuideLicence,
        OutfitterChiefGuideLicence,
        PhaHuntingPermit_Caribou,
        PhaHuntingPermit_Deer,
        PhaHuntingPermit_Elk,
        PhaHuntingPermit_Moose,
        PhaHuntingPermit_MountainGoat,
        PhaHuntingPermit_ThinhornSheep,
        PhaHuntingPermit_ThinhornSheepKluane,
        SmallGameHuntingLicence_CanadianResident,
        SmallGameHuntingLicence_CanadianResidentFirstNationsOrInuit,
        SmallGameHuntingLicence_NonResident,
        SmallGameHuntingLicence_NonResidentFirstNationsOrInuit,
        SmallGameHuntingLicence_YukonResident,
        SmallGameHuntingLicence_YukonResidentSenior,
        SmallGameHuntingLicence_YukonResidentFirstNationsOrInuit,
        SmallGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior,
        SpecialGuideLicence,
        TrappingLicence_AssistantTrapper,
        TrappingLicence_AssistantTrapperSenior,
        TrappingLicence_ConcessionHolder,
        TrappingLicence_ConcessionHolderSenior,
        TrappingLicence_GroupConcessionAreaMember,
        TrappingLicence_GroupConcessionAreaMemberSenior,
        WildlifeActPermit
    }
}
