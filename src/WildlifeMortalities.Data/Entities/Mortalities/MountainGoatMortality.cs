﻿using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class MountainGoatMortality : Mortality, IHasBioSubmission
{
    public MountainGoatBioSubmission? BioSubmission { get; set; }
    public override Species Species => Species.MountainGoat;

    public BioSubmission CreateDefaultBioSubmission() => new MountainGoatBioSubmission(this);
}
