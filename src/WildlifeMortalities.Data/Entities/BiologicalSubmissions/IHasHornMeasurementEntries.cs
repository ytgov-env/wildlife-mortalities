namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public interface IHasHornMeasurementEntries
{
    HornMeasured? HornMeasured { get; set; }
    BroomedStatus? BroomedStatus { get; set; }

    int? HornTotalLengthMillimetres { get; set; }
    int? HornBaseCircumferenceMillimetres { get; set; }
    int? HornTipSpreadMillimetres { get; set; }

    List<HornMeasurementEntry> HornMeasurementEntries { get; set; }
}
