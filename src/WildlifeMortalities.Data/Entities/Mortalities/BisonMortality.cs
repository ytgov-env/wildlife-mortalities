using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities.Mortalities;
public class BisonMortality : Mortality
{
    public string? TemporarySealNumber { get; set ; }
    public Sex Sex { get; set; }
    public int? GameManagementAreaId { get; set; }
    public GameManagementArea? GameManagementArea { get; set; }
    public string? Landmark { get; set; }
    public Point? Coordinates { get; set; }
}
