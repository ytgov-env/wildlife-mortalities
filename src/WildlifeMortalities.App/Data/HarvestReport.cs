﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.App.Data
{
    public class HarvestReport
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int ClientId { get; set; }
        public string Species { get; set; }
        public string Status { get; set; }
    }
}
