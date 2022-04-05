using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities
{
    public abstract class MortalityBase
    {
        public int Id { get; set; }
        public AllSpecies Species { get; set; }
        public BiologicalSubmission BiologicalSubmission { get; set; }
    }
}