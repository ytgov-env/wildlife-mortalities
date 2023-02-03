namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public interface IHasHornMeasurementEntries
{
    public HornMeasured? HornMeasured { get; set; }
    public BroomedStatus? BroomedStatus { get; set; }

    public int? HornTotalLengthMillimetres { get; set; }
    public int? HornBaseCircumferenceMillimetres { get; set; }
    public int? HornTipSpreadMillimetres { get; set; }

    public List<HornMeasurementEntry> HornMeasurementEntries { get; set; }
}
