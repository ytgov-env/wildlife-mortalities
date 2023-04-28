using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Seasons;

// ReSharper disable InconsistentNaming

namespace WildlifeMortalities.Shared.Services.Posse;

public class PosseService : IPosseService
{
    private enum AuthorizationType
    {
        BigGameHuntingLicence_CanadianResident = 10,
        BigGameHuntingLicence_CanadianResidentSpecialGuided = 20,
        BigGameHuntingLicence_NonResident = 30,
        BigGameHuntingLicence_YukonResident = 40,
        BigGameHuntingLicence_YukonResidentSenior = 50,
        BigGameHuntingLicence_YukonResidentFirstNationsOrInuit = 60,
        BigGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior = 70,
        BigGameHuntingLicence_YukonResidentTrapper = 80,
        HuntingPermit_CaribouFortymileSummer = 90,
        HuntingPermit_CaribouFortymileWinter = 100,
        HuntingPermit_CaribouHartRiver = 110,
        HuntingPermit_CaribouNelchina = 120,
        HuntingPermit_ElkAdaptive = 130,
        HuntingPermit_ElkAdaptiveFirstNations = 140,
        HuntingPermit_ElkExclusion = 150,
        HuntingPermit_MooseThreshold = 160,
        HuntingPermit_WoodBisonThreshold = 170,
        HuntingSeal_AmericanBlackBear = 180,
        HuntingSeal_Caribou = 190,
        HuntingSeal_Deer = 200,
        HuntingSeal_Elk = 210,
        HuntingSeal_GrizzlyBear = 220,
        HuntingSeal_Moose = 230,
        HuntingSeal_MountainGoat = 240,
        HuntingSeal_ThinhornSheep = 250,
        HuntingSeal_WoodBison = 260,
        OutfitterAssistantGuideLicence = 270,
        OutfitterChiefGuideLicence = 280,
        PhaHuntingPermit_Caribou = 290,
        PhaHuntingPermit_Deer = 300,
        PhaHuntingPermit_Elk = 310,
        PhaHuntingPermit_Moose = 320,
        PhaHuntingPermit_MountainGoat = 330,
        PhaHuntingPermit_ThinhornSheep = 340,
        PhaHuntingPermit_ThinhornSheepKluane = 350,
        SmallGameHuntingLicence_CanadianResident = 360,
        SmallGameHuntingLicence_CanadianResidentFirstNationsOrInuit = 370,
        SmallGameHuntingLicence_NonResident = 380,
        SmallGameHuntingLicence_NonResidentFirstNationsOrInuit = 390,
        SmallGameHuntingLicence_YukonResident = 400,
        SmallGameHuntingLicence_YukonResidentSenior = 410,
        SmallGameHuntingLicence_YukonResidentFirstNationsOrInuit = 420,
        SmallGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior = 430,
        SpecialGuideLicence = 440,
        TrappingLicence_AssistantTrapper = 450,
        TrappingLicence_AssistantTrapperSenior = 460,
        TrappingLicence_ConcessionHolder = 470,
        TrappingLicence_ConcessionHolderSenior = 480,
        TrappingLicence_GroupConcessionAreaMember = 490,
        TrappingLicence_GroupConcessionAreaMemberSenior = 500,
        CustomWildlifeActPermit = 510
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
                () => new HuntingPermit(HuntingPermit.PermitType.WoodBison)
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
                () => new HuntingSeal(HuntingSeal.SealType.MuleDeer)
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

    public PosseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<
        IEnumerable<(Client client, IEnumerable<string> previousEnvClientIds)>
    > GetClients(DateTimeOffset modifiedSinceDateTime)
    {
        var clients = new List<(Client, IEnumerable<string>)>();

        //var results = await _httpClient.GetFromJsonAsync<GetClientsResponse>(
        //    $"clients?modifiedSinceDateTime={modifiedSinceDateTime:O}"
        //);

        var jsonDoc = await File.ReadAllTextAsync("C:\\Users\\jhodgins\\SND_clients.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var results = JsonSerializer.Deserialize<GetClientsResponse>(jsonDoc, options);

        var rand = new Random();
        foreach (var recentlyModifiedClient in results.Clients)
        {
            var client = new Client
            {
                EnvPersonId = recentlyModifiedClient.EnvClientId,
                //FirstName = recentlyModifiedClient.FirstName,
                FirstName = FakeClients.FirstNames[rand.Next(FakeClients.FirstNames.Length)],
                //LastName = recentlyModifiedClient.LastName,
                LastName = FakeClients.LastNames[rand.Next(FakeClients.LastNames.Length)],
                //BirthDate = recentlyModifiedClient.BirthDate.ToDateTime(new TimeOnly()),
                BirthDate = new DateTime(rand.Next(1930, 2010), rand.Next(1, 12), rand.Next(1, 28)),
                LastModifiedDateTime = recentlyModifiedClient.LastModifiedDateTime
            };

            clients.Add((client, recentlyModifiedClient.PreviousEnvClientIds));
        }

        return clients;
    }

    public async Task<IEnumerable<AuthorizationDto>> GetAuthorizationsByEnvClientId(
        string envClientId,
        DateTimeOffset modifiedSinceDateTime
    )
    {
        return await Task.FromResult(Array.Empty<AuthorizationDto>());
        //return (
        //    await _httpClient.GetFromJsonAsync<GetAuthorizationsResponse>(
        //        $"authorizations/{envClientId}?modifiedSinceDateTime={modifiedSinceDateTime:O}"
        //    )
        //)!.Authorizations;
    }

    public async Task<IEnumerable<Authorization>> GetAuthorizations(
        DateTimeOffset modifiedSinceDateTime,
        Dictionary<string, PersonWithAuthorizations> personMapper,
        AppDbContext context
    )
    {
        //var results = await _httpClient.GetFromJsonAsync<GetAuthorizationsResponse>(
        //    $"authorizations?modifiedSinceDateTime={modifiedSinceDateTime:O}"
        //);

        var jsonDoc = await File.ReadAllTextAsync(
            "C:\\Users\\jhodgins\\OneDrive - Government of Yukon\\Desktop\\SND_authorizations-subset.json"
        );
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var results = JsonSerializer.Deserialize<GetAuthorizationsResponse>(jsonDoc, options);

        var authorizations = new List<Authorization>();
        if (results == null)
        {
            return authorizations;
        }
        var outfitterAreas = await context.OutfitterAreas.ToArrayAsync();
        var registeredTrappingConcessions =
            await context.RegisteredTrappingConcessions.ToArrayAsync();

        var existingInvalidAuthorizations = await context.InvalidAuthorizations.ToDictionaryAsync(
            x => x.GetIdentifier(),
            x => x
        );
        foreach (
            var posseAuthorization in results.Authorizations
                .OrderBy(a => IsBigGameHuntingLicence(a) ? 0 : 1)
                .OrderBy(a => a.Type)
                .ThenBy(a => a.ValidFromDateTime)
        )
        {
            if (Enum.TryParse(posseAuthorization.Type, out AuthorizationType typeValue))
            {
                var authorization = s_authorizationMapper[typeValue]();
                authorization.Number = posseAuthorization.Number;
                authorization.ValidFromDateTime = posseAuthorization.ValidFromDateTime;
                authorization.ValidToDateTime = posseAuthorization.ValidToDateTime;
                authorization.LastModifiedDateTime = posseAuthorization.LastModifiedDateTime;

                authorization.Season = (
                    authorization is TrappingLicence
                        ? await TrappingSeason.TryGetSeason(authorization, context)
                        : await HuntingSeason.TryGetSeason(authorization, context)
                )!;

                var checks = new Func<(bool, string)>[]
                {
                    () => HasValidEnvPersonId(posseAuthorization, authorization, personMapper),
                    () => HasValidFromDateTimeBeforeValidToDateTime(posseAuthorization),
                    () => HasValidSeason(authorization, posseAuthorization),
                    () => HasSeasonAfter2019_2020(authorization),
                    () => HasValidityPeriodOfAtLeastOneDay(posseAuthorization),
                    () =>
                        TryProcessAuthorizationWithOutfitterAreas(
                            posseAuthorization,
                            authorization,
                            outfitterAreas
                        ),
                    () =>
                        TryProcessAuthorization<CustomWildlifeActPermit>(
                            authorization,
                            posseAuthorization,
                            () =>
                                string.IsNullOrWhiteSpace(
                                    posseAuthorization.CustomWildlifeActPermitConditions
                                ),
                            (customWildlifeActPermit) =>
                            {
                                customWildlifeActPermit.Conditions =
                                    posseAuthorization.CustomWildlifeActPermitConditions!;
                                return (true, string.Empty);
                            },
                            nameof(posseAuthorization.CustomWildlifeActPermitConditions)
                        ),
                    () =>
                        TryProcessAuthorization<SpecialGuideLicence>(
                            authorization,
                            posseAuthorization,
                            () =>
                                string.IsNullOrWhiteSpace(
                                    posseAuthorization.SpecialGuideLicenceGuidedHunterEnvClientId
                                ),
                            (specialGuideLicence) =>
                            {
                                personMapper.TryGetValue(
                                    posseAuthorization.SpecialGuideLicenceGuidedHunterEnvClientId!,
                                    out var person
                                );
                                if (person == null)
                                {
                                    Log.Error(
                                        "A {type} has an unrecognized guidedHunterEnvClientId {guidedHunterEnvClientId}: {@authorization}",
                                        posseAuthorization.Type,
                                        posseAuthorization.SpecialGuideLicenceGuidedHunterEnvClientId,
                                        authorization
                                    );
                                    return (
                                        false,
                                        $"This authorization has an unrecognized guided hunter with EnvClientId: {posseAuthorization.SpecialGuideLicenceGuidedHunterEnvClientId}. Verify in POSSE."
                                    );
                                    ;
                                }
                                specialGuideLicence.GuidedClient = (Client)person;
                                return (true, string.Empty);
                            },
                            nameof(posseAuthorization.SpecialGuideLicenceGuidedHunterEnvClientId)
                        ),
                    () =>
                        TryProcessAuthorization<TrappingLicence>(
                            authorization,
                            posseAuthorization,
                            () =>
                                string.IsNullOrWhiteSpace(
                                    posseAuthorization.RegisteredTrappingConcession
                                ),
                            (trappingLicence) =>
                            {
                                var item = Array.Find(
                                    registeredTrappingConcessions,
                                    r => r.Area == posseAuthorization.RegisteredTrappingConcession
                                );
                                if (item == null)
                                {
                                    Log.Error(
                                        "An authorization of type {type} has an unrecognized {property} value of {unrecognizedValue}: {@authorization}",
                                        posseAuthorization.Type,
                                        nameof(trappingLicence.RegisteredTrappingConcession),
                                        posseAuthorization.RegisteredTrappingConcession,
                                        authorization
                                    );
                                    return (
                                        false,
                                        $"This authorization has an unrecognized registered trapping concession: {posseAuthorization.RegisteredTrappingConcession}. Verify in POSSE."
                                    );
                                }
                                trappingLicence.RegisteredTrappingConcession = item;
                                return (true, string.Empty);
                            },
                            nameof(posseAuthorization.RegisteredTrappingConcession)
                        ),
                    () =>
                        TryProcessAuthorization<PhaHuntingPermit>(
                            authorization,
                            posseAuthorization,
                            () =>
                                string.IsNullOrWhiteSpace(
                                    posseAuthorization.PhaHuntingPermitHuntCode
                                ),
                            (phaHuntingPermit) =>
                            {
                                phaHuntingPermit.HuntCode =
                                    posseAuthorization.PhaHuntingPermitHuntCode!;
                                return (true, string.Empty);
                            },
                            nameof(posseAuthorization.PhaHuntingPermitHuntCode)
                        )
                };

                var isValid = true;
                foreach (var item in checks)
                {
                    var (isCheckValid, errMsg) = item();
                    if (isCheckValid == false)
                    {
                        isValid = false;
                        if (string.IsNullOrEmpty(errMsg) == false)
                        {
                            var invalidAuthorization = new InvalidAuthorization(
                                authorization,
                                errMsg
                            );
                            if (
                                existingInvalidAuthorizations.ContainsKey(
                                    invalidAuthorization.GetIdentifier()
                                ) == false
                            )
                            {
                                context.InvalidAuthorizations.Add(invalidAuthorization);
                            }
                        }

                        break;
                    }
                }
                if (isValid)
                {
                    if (
                        existingInvalidAuthorizations.TryGetValue(
                            new InvalidAuthorization(authorization, string.Empty).GetIdentifier(),
                            out var invalidAuthorization
                        )
                    )
                    {
                        context.InvalidAuthorizations.Remove(invalidAuthorization);
                    }

                    authorizations.Add(authorization);
                }
            }
            else
            {
                Log.Warning(
                    "The posse api returned an unrecognized type, which was rejected: {@authorization}",
                    posseAuthorization
                );
            }
        }

        return authorizations;
    }

    private static (bool, string) HasValidityPeriodOfAtLeastOneDay(
        AuthorizationDto posseAuthorization
    )
    {
        if (posseAuthorization.ValidFromDateTime.AddDays(1) > posseAuthorization.ValidToDateTime)
        {
            Log.Information(
                "An authorization of type {type} is valid for less than one day: {@authorization}",
                posseAuthorization.Type,
                posseAuthorization
            );
            return (false, "This authorization is valid for less than one day.");
        }
        return (true, string.Empty);
    }

    private static (bool, string) HasSeasonAfter2019_2020(Authorization authorization)
    {
        if (authorization.Season.StartDate.Year < 2020)
        {
            return (false, string.Empty);
        }
        return (true, string.Empty);
    }

    private static (bool, string) HasValidSeason(
        Authorization authorization,
        AuthorizationDto posseAuthorization
    )
    {
        if (authorization.Season == null)
        {
            Log.Error(
                "An authorization of type {type} spans multiple harvest seasons, or does not have a season between 2000-2100: {@authorization}",
                posseAuthorization.Type,
                posseAuthorization
            );
            return (
                false,
                "This authorization spans multiple harvest seasons, or does not have a season between 2000-2100. Verify in POSSE."
            );
        }
        return (true, string.Empty);
    }

    private static bool IsBigGameHuntingLicence(AuthorizationDto x) =>
        x.Type.StartsWith(nameof(BigGameHuntingLicence));

    private static (bool, string) HasValidFromDateTimeBeforeValidToDateTime(
        AuthorizationDto posseAuthorization
    )
    {
        if (posseAuthorization.ValidFromDateTime > posseAuthorization.ValidToDateTime)
        {
            Log.Error(
                "An authorization of type {type} has a validFromDateTime that occurs after validToDateTime: {@authorization}",
                posseAuthorization.Type,
                posseAuthorization
            );
            return (
                false,
                "This authorization has a validFromDateTime that occurs after validToDateTime."
            );
        }
        return (true, string.Empty);
    }

    private static (bool, string) HasValidEnvPersonId(
        AuthorizationDto posseAuthorization,
        Authorization authorization,
        Dictionary<string, PersonWithAuthorizations> personMapper
    )
    {
        // Todo: Handle EnvOrganizationId after organizations are added to the posse api
        if (posseAuthorization.EnvClientId == null)
        {
            LogErrorRequiredPropertyIsMissing(
                posseAuthorization,
                nameof(posseAuthorization.EnvClientId)
            );
            return (false, string.Empty);
        }
        if (personMapper.TryGetValue(posseAuthorization.EnvClientId, out var client))
        {
            authorization.Person = client;
        }
        else
        {
            Log.Error(
                "An authorization is associated with EnvClientId {EnvClientId}, which doesn't exist. The authorization was rejected: {@authorization}",
                posseAuthorization.EnvClientId,
                authorization
            );
            return (false, string.Empty);
        }
        return (true, string.Empty);
    }

    private static (bool, string) TryProcessAuthorization<T>(
        Authorization authorization,
        AuthorizationDto posseAuthorization,
        Func<bool> condition,
        Func<T, (bool, string)> updater,
        string propertyName
    )
    {
        var conditionResult = condition();
        if (authorization is T castedAuthorization)
        {
            if (conditionResult)
            {
                LogErrorRequiredPropertyIsMissing(posseAuthorization, propertyName);

                return (false, $"This authorization is missing {propertyName}. Verify in POSSE.");
            }
            return updater.Invoke(castedAuthorization);
        }
        else if (!conditionResult)
        {
            LogWarningInvalidPropertyIsSet(posseAuthorization, propertyName);
        }
        return (true, string.Empty);
    }

    private static (bool, string) TryProcessAuthorizationWithOutfitterAreas(
        AuthorizationDto posseAuthorization,
        Authorization authorization,
        IEnumerable<OutfitterArea> outfitterAreas
    )
    {
        var condition = !posseAuthorization.OutfitterAreas.Any();
        IHasOutfitterAreas? auth = authorization switch
        {
            BigGameHuntingLicence
            and {
                Type: BigGameHuntingLicence.LicenceType.CanadianResident
                    or BigGameHuntingLicence.LicenceType.NonResident
            }
                => (BigGameHuntingLicence)authorization,
            // Todo: does small game hunting licence actually require outfitter areas?
            SmallGameHuntingLicence
            and { Type: SmallGameHuntingLicence.LicenceType.NonResident }
                => (SmallGameHuntingLicence)authorization,
            OutfitterChiefGuideLicence licence => licence,
            OutfitterAssistantGuideLicence licence => licence,
            _ => null
        };
        if (auth != null)
        {
            if (condition)
            {
                LogErrorRequiredPropertyIsMissing(
                    posseAuthorization,
                    nameof(posseAuthorization.OutfitterAreas)
                );
                return (false, "This authorization is missing outfitter areas. Verify in POSSE.");
            }
            auth.OutfitterAreas = new();
            foreach (var area in posseAuthorization.OutfitterAreas)
            {
                var item = outfitterAreas.FirstOrDefault(o => o.Area == area.TrimStart('0'));
                if (item != null)
                {
                    auth.OutfitterAreas.Add(item);
                }
                else
                {
                    LogWarningPropertyContainsInvalidValue(
                        posseAuthorization,
                        nameof(posseAuthorization.OutfitterAreas),
                        area
                    );
                    return (
                        false,
                        $"This authorization has an unrecognized outfitter area: {area}. Verify in POSSE."
                    );
                }
            }
        }
        else if (!condition)
        {
            LogWarningInvalidPropertyIsSet(
                posseAuthorization,
                nameof(posseAuthorization.OutfitterAreas)
            );
        }
        return (true, string.Empty);
    }

    private static void LogErrorRequiredPropertyIsMissing(
        AuthorizationDto authorization,
        string nameOfRequiredProperty
    )
    {
        if (authorization is null)
            throw new ArgumentNullException(nameof(authorization));
        if (string.IsNullOrWhiteSpace(nameOfRequiredProperty))
        {
            throw new ArgumentException(
                $"{nameof(nameOfRequiredProperty)} cannot be null, empty, or whitespace.",
                nameof(nameOfRequiredProperty)
            );
        }
        Log.Error(
            "An authorization of type {type} is missing required property {requiredPropertyForType}: {@authorization}",
            authorization.Type,
            nameOfRequiredProperty,
            authorization
        );
    }

    private static void LogWarningInvalidPropertyIsSet(
        AuthorizationDto authorization,
        string nameOfInvalidProperty
    )
    {
        if (nameOfInvalidProperty is null)
            throw new ArgumentNullException(nameof(nameOfInvalidProperty));
        if (authorization is null)
            throw new ArgumentNullException(nameof(authorization));
        Log.Warning(
            "An authorization of type {type} has property {invalidPropertyForType} set, which isn't applicable to this type. The property was ignored. {@authorization}",
            authorization.Type,
            nameOfInvalidProperty,
            authorization
        );
    }

    private static void LogWarningPropertyContainsInvalidValue(
        AuthorizationDto authorization,
        string nameOfProperty,
        string value
    )
    {
        if (authorization is null)
            throw new ArgumentNullException(nameof(authorization));
        if (string.IsNullOrWhiteSpace(nameOfProperty))
        {
            throw new ArgumentException(
                $"'{nameof(nameOfProperty)}' cannot be null, empty, or whitespace.",
                nameof(nameOfProperty)
            );
        }
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                $"'{nameof(value)}' cannot be null, empty, or whitespace.",
                nameof(value)
            );
        }

        Log.Warning(
            "An authorization of type {type} has property {property} with an invalid value: {value}. The value was ignored. {@authorization}",
            authorization.Type,
            nameOfProperty,
            value,
            authorization
        );
    }
}
