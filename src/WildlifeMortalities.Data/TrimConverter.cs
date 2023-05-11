using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

// ReSharper disable ReturnTypeCanBeEnumerable.Global

namespace WildlifeMortalities.Data;

internal class TrimConverter : ValueConverter<string, string>
{
    public TrimConverter()
        : base(v => v.Trim(), v => v) { }
}
