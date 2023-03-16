using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;

// ReSharper disable InconsistentNaming

namespace WildlifeMortalities.Shared.Services;

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

    public PosseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<
        IEnumerable<(Client client, IEnumerable<string> previousEnvClientIds)>
    > GetClients(DateTimeOffset modifiedSinceDateTime)
    {
        var clients = new List<(Client, IEnumerable<string>)>();

        var results = await _httpClient.GetFromJsonAsync<GetClientsResponse>(
            $"clients?modifiedSinceDateTime={modifiedSinceDateTime:yyyy-MM-ddTHH:mm:ssK}"
        );

        var rand = new Random();
        foreach (var recentlyModifiedClient in results.Clients)
        {
            var client = new Client
            {
                EnvClientId = recentlyModifiedClient.EnvClientId,
                FirstName = recentlyModifiedClient.FirstName,
                LastName = recentlyModifiedClient.LastName,
                //BirthDate = recentlyModifiedClient.BirthDate.ToDateTime(new TimeOnly()),
                BirthDate = new DateTime(rand.Next(1930, 2010), rand.Next(1, 12), rand.Next(1, 28)),
                LastModifiedDateTime = recentlyModifiedClient.LastModifiedDateTime
            };

            clients.Add((client, recentlyModifiedClient.PreviousEnvClientIds));
        }

        //clients = new List<(Client, IEnumerable<string>)>
        //{
        //    (new Client { EnvClientId = "217956", }, new[] { "169422" })
        //};

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
        //        $"authorizations/{envClientId}?modifiedSinceDateTime={modifiedSinceDateTime:yyyy-MM-ddTHH:mm:ssK}"
        //    )
        //)!.Authorizations;
    }

    public bool IsBigGameHuntingLicence(AuthorizationDto x) =>
        x.Type.StartsWith(nameof(BigGameHuntingLicence));

    public async Task<IEnumerable<(Authorization, string)>> GetAuthorizations(
        DateTimeOffset modifiedSinceDateTime,
        Dictionary<string, Client> clientMapper,
        AppDbContext context
    )
    {
        var jsonDoc = await File.ReadAllTextAsync(
            "C:\\Users\\jhodgins\\OneDrive - Government of Yukon\\Desktop\\SND_authorizations.json"
        );
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var results = JsonSerializer.Deserialize<GetAuthorizationsResponse>(jsonDoc, options);

        //var results = await _httpClient.GetFromJsonAsync<GetAuthorizationsResponse>(
        //    $"authorizations?modifiedSinceDateTime={modifiedSinceDateTime:yyyy-MM-ddThh:mm:ssK}"
        //);

        var authorizations = new List<(Authorization authorization, string envClientId)>();
        if (results == null)
        {
            return authorizations;
        }
        var outfitterAreas = await context.OutfitterAreas.ToArrayAsync();
        var registeredTrappingConcessions =
            await context.RegisteredTrappingConcessions.ToArrayAsync();

        // Since we ignore (don't save) authorizations that
        List<AuthorizationDto> authorizationToBeProcessed = new();
        foreach (var item in results.Authorizations.Where(IsBigGameHuntingLicence))
        {
            var childList = await GetAuthorizationsByEnvClientId(
                item.EnvClientId,
                item.ValidFromDateTime
            );
            authorizationToBeProcessed.AddRange(childList);
        }

        // Big game hunting licences must be processed first because it is a required parent of other authorization types
        foreach (
            var posseAuthorization in results.Authorizations
                .Union(authorizationToBeProcessed)
                .OrderBy(a => IsBigGameHuntingLicence(a) ? 0 : 1)
                .DistinctBy(x => $"{x.EnvClientId}-{GetSeason(x)}-{x.Type}")
        )
        {
            if (Enum.TryParse(posseAuthorization.Type, out AuthorizationType typeValue))
            {
                var authorization = s_authorizationMapper[typeValue]();
                authorization.Number = posseAuthorization.Number;
                authorization.ValidFromDateTime = posseAuthorization.ValidFromDateTime;
                authorization.ValidToDateTime = posseAuthorization.ValidToDateTime;
                authorization.Season = GetSeason(authorization);
                authorization.LastModifiedDateTime = posseAuthorization.LastModifiedDateTime;

                if (HasInvalidEnvClientId(posseAuthorization))
                    continue;

                if (HasInvalidDateTimes(posseAuthorization))
                    continue;

                if (HasInvalidSeason(authorization, posseAuthorization))
                    continue;

                // Ignore authorizations before the 20/21 season
                if (HasSeasonBefore2020_2021(authorization))
                    continue;

                if (
                    !TryProcessAuthorizationWithOutfitterAreas(
                        posseAuthorization,
                        authorization,
                        outfitterAreas
                    )
                )
                {
                    continue;
                }

                if (
                    !TryProcessAuthorization<CustomWildlifeActPermit>(
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
                        },
                        nameof(posseAuthorization.CustomWildlifeActPermitConditions)
                    )
                )
                {
                    continue;
                }

                if (
                    !TryProcessAuthorization<SpecialGuideLicence>(
                        authorization,
                        posseAuthorization,
                        () =>
                            string.IsNullOrWhiteSpace(
                                posseAuthorization.SpecialGuideLicenceGuidedHunterEnvClientId
                            ),
                        (specialGuideLicence) =>
                        {
                            clientMapper.TryGetValue(
                                posseAuthorization.SpecialGuideLicenceGuidedHunterEnvClientId!,
                                out var client
                            );
                            if (client != null)
                            {
                                specialGuideLicence.GuidedClient = client;
                            }
                        },
                        nameof(posseAuthorization.SpecialGuideLicenceGuidedHunterEnvClientId)
                    )
                )
                {
                    continue;
                }

                if (
                    !TryProcessAuthorization<TrappingLicence>(
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
                            if (item != null)
                            {
                                trappingLicence.RegisteredTrappingConcession = item;
                            }
                            //else
                            //{
                            //    Log.Error(
                            //        "An authorization of type {type} has an unrecognized {property} value of {unrecognizedValue}, and was ignored: {@authorization}",
                            //        posseAuthorization.Type,
                            //        nameof(trappingLicence.RegisteredTrappingConcession),
                            //        posseAuthorization.RegisteredTrappingConcession,
                            //        authorization
                            //    );
                            //}
                        },
                        nameof(posseAuthorization.RegisteredTrappingConcession)
                    )
                )
                {
                    continue;
                }

                if (
                    !TryProcessAuthorization<PhaHuntingPermit>(
                        authorization,
                        posseAuthorization,
                        () =>
                            string.IsNullOrWhiteSpace(posseAuthorization.PhaHuntingPermitHuntCode),
                        (phaHuntingpermit) =>
                        {
                            phaHuntingpermit.HuntCode =
                                posseAuthorization.PhaHuntingPermitHuntCode!;
                        },
                        nameof(posseAuthorization.PhaHuntingPermitHuntCode)
                    )
                )
                {
                    continue;
                }

                if (
                    HasInvalidBigGameHuntingLicenceForDependantAuthorization(
                        posseAuthorization,
                        authorization,
                        authorizations
                    )
                )
                {
                    continue;
                }

                authorizations.Add((authorization, posseAuthorization.EnvClientId));
            }
            else
            {
                Log.Warning(
                    "The posse api returned an unrecognized type, which was ignored: {@authorization}",
                    posseAuthorization
                );
            }
        }

        return authorizations;
    }

    private static bool HasSeasonBefore2020_2021(Authorization authorization)
    {
        if (authorization.Season == null)
        {
            return true;
        }
        return int.Parse(authorization.Season.Substring(3, 2)) < 21;
    }

    private static bool HasInvalidSeason(
        Authorization authorization,
        AuthorizationDto posseAuthorization
    )
    {
        if (authorization.Season == null)
        {
            Log.Error(
                "An authorization of type {type} spans multiple harvest seasons, and was ignored: {@authorization}",
                posseAuthorization.Type,
                posseAuthorization
            );
            return true;
        }
        return false;
    }

    private static bool HasInvalidBigGameHuntingLicenceForDependantAuthorization(
        AuthorizationDto posseAuthorization,
        Authorization authorization,
        List<(Authorization authorization, string envClientId)> authorizations
    )
    {
        if (authorization is IHasBigGameHuntingLicence auth)
        {
            var bigGameHuntingLicencesInSameSeason = authorizations.Where(
                (a) =>
                    a.envClientId == posseAuthorization.EnvClientId
                    && a.authorization is BigGameHuntingLicence
                    && a.authorization.Season == authorization.Season
            );
            var applicableBigGameHuntingLicences = bigGameHuntingLicencesInSameSeason.Where(
                (a) =>
                    a.authorization.ValidFromDateTime <= authorization.ValidFromDateTime
                    // Pad by 5 minutes to handle authorization "suspensions" in POSSE, which can suspend the dependant authorizations after suspending the parent
                    && a.authorization.ValidToDateTime.AddMinutes(5)
                        >= authorization.ValidToDateTime
            );
            var parent = (BigGameHuntingLicence?)
                applicableBigGameHuntingLicences.SingleOrDefault().authorization;
            if (parent == null)
            {
                if (bigGameHuntingLicencesInSameSeason.Any())
                {
                    Log.Error(
                        "EnvClientId {envClientId} has at least one Big Game Hunting Licence for season {season}, but a dependant authorization of type {type} in season {season} is not within its validity period: {@authorization}. Licences in season: {@licences}",
                        posseAuthorization.EnvClientId,
                        authorization.Season,
                        posseAuthorization.Type,
                        authorization.Season,
                        posseAuthorization,
                        bigGameHuntingLicencesInSameSeason
                            .Select(x => x.authorization)
                            .Select(
                                x =>
                                    new
                                    {
                                        Type = nameof(BigGameHuntingLicence),
                                        x.Number,
                                        x.ValidFromDateTime,
                                        x.ValidToDateTime,
                                        x.LastModifiedDateTime
                                    }
                            )
                            .ToArray()
                    );
                }
                else
                {
                    Log.Error(
                        "EnvClientId {envClientId} does not have a Big Game Hunting Licence for season {season}, so a dependant authorization of type {type} in season {season} was ignored: {@authorization}",
                        posseAuthorization.EnvClientId,
                        authorization.Season,
                        posseAuthorization.Type,
                        authorization.Season,
                        posseAuthorization
                    );
                }
                return true;
            }
            auth.BigGameHuntingLicence = parent;
        }
        return false;
    }

    private static bool HasInvalidDateTimes(AuthorizationDto posseAuthorization)
    {
        if (posseAuthorization.ValidFromDateTime > posseAuthorization.ValidToDateTime)
        {
            Log.Error(
                "An authorization of type {type} has a validFromDateTime that occurs after validToDateTime, and was ignored: {@authorization}",
                posseAuthorization.Type,
                posseAuthorization
            );
            return true;
        }
        return false;
    }

    private static bool HasInvalidEnvClientId(AuthorizationDto posseAuthorization)
    {
        if (posseAuthorization.EnvClientId == null)
        {
            LogRequiredPropertyIsMissing(
                posseAuthorization,
                nameof(posseAuthorization.EnvClientId)
            );
            return true;
        }
        return false;
    }

    private static string? GetSeason(AuthorizationDto authorization) =>
        GetSeason(
            authorization.ValidFromDateTime,
            authorization.ValidToDateTime,
            Enum.TryParse(authorization.Type, out AuthorizationType typeValue)
                && s_authorizationMapper[typeValue]() is TrappingLicence
        );

    private static string? GetSeason(Authorization authorization) =>
        GetSeason(
            authorization.ValidFromDateTime,
            authorization.ValidToDateTime,
            authorization is TrappingLicence
        );

    private static string? GetSeason(
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        bool isTrrapping
    )
    {
        // Hunting April-March, Trapping July-June
        var seasonEndMonth = isTrrapping ? 6 : 3;

        int startYear;
        if (startDate.Month <= seasonEndMonth)
        {
            startYear = startDate.Year - 1;
        }
        else
        {
            startYear = startDate.Year;
        }

        int endYear;
        if (endDate.Month <= seasonEndMonth)
        {
            endYear = endDate.Year;
        }
        else
        {
            endYear = endDate.Year + 1;
        }

        if (endYear - startYear != 1)
        {
            return null;
        }

        return $"{startYear % 100:00}/{endYear % 100:00}";
    }

    private static bool TryProcessAuthorization<T>(
        Authorization authorization,
        AuthorizationDto posseAuthorization,
        Func<bool> condition,
        Action<T> updater,
        string propertyName
    )
    {
        var conditionResult = condition();
        if (authorization is T castedAuthorization)
        {
            if (conditionResult)
            {
                LogRequiredPropertyIsMissing(posseAuthorization, propertyName);

                return false;
            }
            updater.Invoke(castedAuthorization);
        }
        else if (!conditionResult)
        {
            LogInvalidPropertyIsSet(posseAuthorization, propertyName);
        }
        return true;
    }

    private static bool TryProcessAuthorizationWithOutfitterAreas(
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
            OutfitterChiefGuideLicence licence => licence,
            OutfitterAssistantGuideLicence licence => licence,
            _ => null
        };
        if (auth != null)
        {
            if (condition)
            {
                LogRequiredPropertyIsMissing(
                    posseAuthorization,
                    nameof(posseAuthorization.OutfitterAreas)
                );
                return false;
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
                    LogPropertyContainsInvalidValue(
                        posseAuthorization,
                        nameof(posseAuthorization.OutfitterAreas),
                        area
                    );
                }
            }
        }
        else if (!condition)
        {
            LogInvalidPropertyIsSet(posseAuthorization, nameof(posseAuthorization.OutfitterAreas));
        }
        return true;
    }

    private static void LogRequiredPropertyIsMissing(
        AuthorizationDto authorization,
        string nameOfRequiredProperty
    )
    {
        if (authorization is null)
            throw new ArgumentNullException(nameof(authorization));
        if (string.IsNullOrEmpty(nameOfRequiredProperty))
        {
            throw new ArgumentException(
                $"'{nameof(nameOfRequiredProperty)}' cannot be null or empty.",
                nameof(nameOfRequiredProperty)
            );
        }
        Log.Error(
            "An authorization of type {type} is missing required property {requiredPropertyForType}, and was ignored: {@authorization}",
            authorization.Type,
            nameOfRequiredProperty,
            authorization
        );
    }

    private static void LogInvalidPropertyIsSet(
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

    private static void LogPropertyContainsInvalidValue(
        AuthorizationDto authorization,
        string nameOfProperty,
        string value
    )
    {
        if (authorization is null)
            throw new ArgumentNullException(nameof(authorization));
        if (string.IsNullOrEmpty(nameOfProperty))
        {
            throw new ArgumentException(
                $"'{nameof(nameOfProperty)}' cannot be null or empty.",
                nameof(nameOfProperty)
            );
        }
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException(
                $"'{nameof(value)}' cannot be null or empty.",
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

    public class GetAuthorizationsResponse
    {
        public IEnumerable<AuthorizationDto> Authorizations { get; set; } = null!;
    }

    public record AuthorizationDto(
        string Type,
        string EnvClientId,
        string Number,
        string? CustomWildlifeActPermitConditions,
        string? SpecialGuideLicenceGuidedHunterEnvClientId,
        string? PhaHuntingPermitHuntCode,
        string? RegisteredTrappingConcession,
        DateTimeOffset ValidFromDateTime,
        DateTimeOffset ValidToDateTime,
        DateTimeOffset LastModifiedDateTime,
        IEnumerable<string> OutfitterAreas
    );

    public class GetClientsResponse
    {
        public IEnumerable<ClientDto> Clients { get; set; } = null!;
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
