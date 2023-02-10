using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class HornMeasurementEntry
{
    public int Annulus { get; set; }
    public int LengthMillimetres { get; set; }
    public int CircumferenceMillimetres { get; set; }
    public bool IsBroomed { get; set; }
}
