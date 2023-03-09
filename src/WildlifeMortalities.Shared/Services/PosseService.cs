using System.Net.Http.Json;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;

// ReSharper disable InconsistentNaming

namespace WildlifeMortalities.Shared.Services;

public class PosseService : IPosseService
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
        HuntingPermit_CaribouFortymileSummer = 100,
        HuntingPermit_CaribouFortymileWinter = 110,
        HuntingPermit_CaribouHartRiver = 120,
        HuntingPermit_CaribouNelchina = 130,
        HuntingPermit_ElkAdaptive = 140,
        HuntingPermit_ElkAdaptiveFirstNations = 150,
        HuntingPermit_ElkExclusion = 160,
        HuntingPermit_MooseThreshold = 170,
        HuntingPermit_WoodBisonThreshold = 180,
        HuntingSeal_AmericanBlackBear = 190,
        HuntingSeal_Caribou = 200,
        HuntingSeal_Deer = 210,
        HuntingSeal_Elk = 220,
        HuntingSeal_GrizzlyBear = 230,
        HuntingSeal_Moose = 240,
        HuntingSeal_MountainGoat = 250,
        HuntingSeal_ThinhornSheep = 260,
        HuntingSeal_WoodBison = 270,
        OutfitterAssistantGuideLicence = 280,
        OutfitterChiefGuideLicence = 290,
        PhaHuntingPermit_Caribou = 300,
        PhaHuntingPermit_Deer = 310,
        PhaHuntingPermit_Elk = 320,
        PhaHuntingPermit_Moose = 330,
        PhaHuntingPermit_MountainGoat = 340,
        PhaHuntingPermit_ThinhornSheep = 350,
        PhaHuntingPermit_ThinhornSheepKluane = 360,
        SmallGameHuntingLicence_CanadianResident = 370,
        SmallGameHuntingLicence_CanadianResidentFirstNationsOrInuit = 380,
        SmallGameHuntingLicence_NonResident = 390,
        SmallGameHuntingLicence_NonResidentFirstNationsOrInuit = 400,
        SmallGameHuntingLicence_YukonResident = 410,
        SmallGameHuntingLicence_YukonResidentSenior = 420,
        SmallGameHuntingLicence_YukonResidentFirstNationsOrInuit = 430,
        SmallGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior = 440,
        SpecialGuideLicence = 450,
        TrappingLicence_AssistantTrapper = 460,
        TrappingLicence_AssistantTrapperSenior = 470,
        TrappingLicence_ConcessionHolder = 480,
        TrappingLicence_ConcessionHolderSenior = 490,
        TrappingLicence_GroupConcessionAreaMember = 500,
        TrappingLicence_GroupConcessionAreaMemberSenior = 510,
        CustomWildlifeActPermit = 520
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
                AuthorizationType.BigGameHuntingLicence_CanadianResidentSpecialGuided,
                () =>
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
                AuthorizationType.BigGameHuntingLicence_YukonResidentSenior,
                () =>
                    new BigGameHuntingLicence(BigGameHuntingLicence.LicenceType.YukonResidentSenior)
            },
            {
                AuthorizationType.BigGameHuntingLicence_YukonResidentFirstNationsOrInuit,
                () =>
                    new BigGameHuntingLicence(
                        BigGameHuntingLicence.LicenceType.YukonResidentFirstNationsOrInuit
                    )
            },
            {
                AuthorizationType.BigGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior,
                () =>
                    new BigGameHuntingLicence(
                        BigGameHuntingLicence.LicenceType.YukonResidentFirstNationsOrInuitSenior
                    )
            },
            {
                AuthorizationType.BigGameHuntingLicence_YukonResidentTrapper,
                () =>
                    new BigGameHuntingLicence(
                        BigGameHuntingLicence.LicenceType.YukonResidentTrapper
                    )
            },
            {
                AuthorizationType.HuntingPermit_CaribouFortymileFall,
                () => new HuntingPermit(HuntingPermit.PermitType.CaribouFortymileFall)
            },
            {
                AuthorizationType.HuntingPermit_CaribouFortymileSummer,
                () => new HuntingPermit(HuntingPermit.PermitType.CaribouFortymileSummer)
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
            {
                AuthorizationType.HuntingSeal_Caribou,
                () => new HuntingSeal(HuntingSeal.SealType.Caribou)
            },
            {
                AuthorizationType.HuntingSeal_Deer,
                () => new HuntingSeal(HuntingSeal.SealType.Deer)
            },
            { AuthorizationType.HuntingSeal_Elk, () => new HuntingSeal(HuntingSeal.SealType.Elk) },
            {
                AuthorizationType.HuntingSeal_GrizzlyBear,
                () => new HuntingSeal(HuntingSeal.SealType.GrizzlyBear)
            },
            {
                AuthorizationType.HuntingSeal_Moose,
                () => new HuntingSeal(HuntingSeal.SealType.Moose)
            },
            {
                AuthorizationType.HuntingSeal_MountainGoat,
                () => new HuntingSeal(HuntingSeal.SealType.MountainGoat)
            },
            {
                AuthorizationType.HuntingSeal_ThinhornSheep,
                () => new HuntingSeal(HuntingSeal.SealType.ThinhornSheep)
            },
            {
                AuthorizationType.HuntingSeal_WoodBison,
                () => new HuntingSeal(HuntingSeal.SealType.WoodBison)
            },
            {
                AuthorizationType.OutfitterAssistantGuideLicence,
                () => new OutfitterAssistantGuideLicence()
            },
            {
                AuthorizationType.OutfitterChiefGuideLicence,
                () => new OutfitterChiefGuideLicence()
            },
            {
                AuthorizationType.PhaHuntingPermit_Caribou,
                () => new PhaHuntingPermit(PhaHuntingPermit.PermitType.Caribou)
            },
            {
                AuthorizationType.PhaHuntingPermit_Deer,
                () => new PhaHuntingPermit(PhaHuntingPermit.PermitType.Deer)
            },
            {
                AuthorizationType.PhaHuntingPermit_Elk,
                () => new PhaHuntingPermit(PhaHuntingPermit.PermitType.Elk)
            },
            {
                AuthorizationType.PhaHuntingPermit_Moose,
                () => new PhaHuntingPermit(PhaHuntingPermit.PermitType.Moose)
            },
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
                AuthorizationType.SmallGameHuntingLicence_CanadianResident,
                () =>
                    new SmallGameHuntingLicence(
                        SmallGameHuntingLicence.LicenceType.CanadianResident
                    )
            },
            {
                AuthorizationType.SmallGameHuntingLicence_CanadianResidentFirstNationsOrInuit,
                () =>
                    new SmallGameHuntingLicence(
                        SmallGameHuntingLicence.LicenceType.CanadianResidentFirstNationsOrInuit
                    )
            },
            {
                AuthorizationType.SmallGameHuntingLicence_NonResident,
                () => new SmallGameHuntingLicence(SmallGameHuntingLicence.LicenceType.NonResident)
            },
            {
                AuthorizationType.SmallGameHuntingLicence_NonResidentFirstNationsOrInuit,
                () =>
                    new SmallGameHuntingLicence(
                        SmallGameHuntingLicence.LicenceType.NonResidentFirstNationsOrInuit
                    )
            },
            {
                AuthorizationType.SmallGameHuntingLicence_YukonResident,
                () => new SmallGameHuntingLicence(SmallGameHuntingLicence.LicenceType.YukonResident)
            },
            {
                AuthorizationType.SmallGameHuntingLicence_YukonResidentSenior,
                () =>
                    new SmallGameHuntingLicence(
                        SmallGameHuntingLicence.LicenceType.YukonResidentSenior
                    )
            },
            {
                AuthorizationType.SmallGameHuntingLicence_YukonResidentFirstNationsOrInuit,
                () =>
                    new SmallGameHuntingLicence(
                        SmallGameHuntingLicence.LicenceType.YukonResidentFirstNationsOrInuit
                    )
            },
            {
                AuthorizationType.SmallGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior,
                () =>
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
                AuthorizationType.TrappingLicence_GroupConcessionAreaMemberSenior,
                () =>
                    new TrappingLicence(TrappingLicence.LicenceType.GroupConcessionAreaMemberSenior)
            },
            { AuthorizationType.CustomWildlifeActPermit, () => new CustomWildlifeActPermit() }
        };

    private readonly HttpClient _httpClient;

    public PosseService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<IEnumerable<(Client, IEnumerable<string>)>> RetrieveClientData(
        DateTimeOffset modifiedSinceDateTime
    )
    {
        var results = await _httpClient.GetFromJsonAsync<GetClientsResponse>(
            $"clients?modifiedSinceDateTime={modifiedSinceDateTime:yyyy-MM-ddThh:mm:ssK}"
        );

        var clients = new List<(Client, IEnumerable<string>)>();
        foreach (var recentlyModifiedClient in results.Clients)
        {
            var client = new Client
            {
                EnvClientId = recentlyModifiedClient.EnvClientId,
                FirstName = recentlyModifiedClient.FirstName,
                LastName = recentlyModifiedClient.LastName,
                BirthDate = recentlyModifiedClient.BirthDate.ToDateTime(new TimeOnly()),
                LastModifiedDateTime = recentlyModifiedClient.LastModifiedDateTime
            };

            clients.Add((client, recentlyModifiedClient.PreviousEnvClientIds));
        }

        clients = new List<(Client, IEnumerable<string>)>
        {
            (
                new Client
                {
                    EnvClientId = "217956",
                    FirstName = "John",
                    LastName = "Doe",
                    BirthDate = new DateTime(1974, 1, 1)
                },
                new[] { "691025" }
            )
        };

        return clients;
    }

    public async Task<
        IEnumerable<(
            string envClientId,
            Authorization authorization,
            string? specialGuidedHunterEnvClientId
        )>
    > RetrieveAuthorizationData(DateTimeOffset modifiedSinceDateTime)
    {
        var response = await _httpClient.GetAsync(
            $"authorizations?modifiedSinceDateTime={modifiedSinceDateTime:yyyy-MM-ddThh:mm:ssK}"
        );

        var test = await response.Content.ReadAsStringAsync();

        var results = await response.Content.ReadFromJsonAsync<GetAuthorizationsResponse>();

        //var results = await _httpClient.GetFromJsonAsync<GetAuthorizationsResponse>(
        //    $"authorizations?modifiedSinceDateTime={modifiedSinceDateTime:yyyy-MM-ddThh:mm:ssK}"
        //);

        //var results = await _httpClient.GetFromJsonAsync<GetAuthorizationsResponse>(
        //    $"authorizations?modifiedSinceDateTime={modifiedSinceDateTime}"
        //);

        var authorizations =
            new List<(string envClientId, Authorization, string? specialGuidedHunterEnvClientId)>();
        foreach (var posseAuthorization in results.Authorizations)
        {
            if (Enum.TryParse(posseAuthorization.Type, out AuthorizationType typeValue))
            {
                var authorization = s_authorizationMapper[typeValue]();
                authorization.Number = posseAuthorization.Number;
                authorization.ActiveFromDate = posseAuthorization.ValidFromDateTime;
                authorization.ActiveToDate = posseAuthorization.ValidToDateTime;
                authorization.LastModifiedDateTime = posseAuthorization.LastModifiedDateTime;

                authorizations.Add(
                    (
                        posseAuthorization.EnvClientId,
                        authorization,
                        posseAuthorization.SpecialGuideLicenceGuidedHunterEnvClientId
                    )
                );
            }
            else
            {
                Console.WriteLine($"Type {posseAuthorization.Type} not found.");
            }

            //if (authorization is CustomWildlifeActPermit wildlifeActPermit)
            //{
            //    if (string.IsNullOrEmpty(posseAuthorization.CustomWildlifeActPermitConditions))
            //    {
            //        // Todo throw exception?
            //        continue;
            //    }

            //    wildlifeActPermit.Conditions = posseAuthorization.CustomWildlifeActPermitConditions;
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(posseAuthorization.CustomWildlifeActPermitConditions))
            //    {
            //        // Todo throw exception?
            //    }
            //}

            //if (authorization is SpecialGuideLicence)
            //{
            //    if (
            //        string.IsNullOrEmpty(
            //            posseAuthorization.SpecialGuideLicenceGuidedHunterEnvClientId
            //        )
            //    )
            //    {
            //        // Todo throw exception?
            //        continue;
            //    }
            //}
            //else
            //{
            //    if (
            //        !string.IsNullOrEmpty(
            //            posseAuthorization.SpecialGuideLicenceGuidedHunterEnvClientId
            //        )
            //    )
            //    {
            //        // Todo throw exception?
            //        continue;
            //    }
            //}
        }

        return authorizations;
    }

    public class GetAuthorizationsResponse
    {
        public List<AuthorizationDto> Authorizations { get; set; }
    }

    public record AuthorizationDto(
        string Type,
        string EnvClientId,
        string Number,
        string? CustomWildlifeActPermitConditions,
        string? SpecialGuideLicenceGuidedHunterEnvClientId,
        string? PhaHuntingPermitHuntCode,
        string? RegisteredTrappingConcession,
        DateTimeOffset? ValidFromDateTime,
        DateTimeOffset? ValidToDateTime,
        DateTimeOffset LastModifiedDateTime,
        IEnumerable<string> OutfitterAreas
    );

    public class GetClientsResponse
    {
        public List<ClientDto> Clients { get; set; }
    }

    public record ClientDto(
        string EnvClientId,
        string FirstName,
        string LastName,
        DateOnly BirthDate,
        DateTimeOffset LastModifiedDateTime,
        IEnumerable<string> PreviousEnvClientIds
    );
}
