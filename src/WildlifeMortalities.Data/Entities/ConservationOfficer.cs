﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities;
public class ConservationOfficer : Reporter
{
    public string BadgeNumber { get; set; } = string.Empty;
}