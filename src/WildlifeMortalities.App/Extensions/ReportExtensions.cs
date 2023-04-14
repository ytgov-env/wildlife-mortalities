using System.Reflection;
using WildlifeMortalities.App.Features.Reports;
using WildlifeMortalities.Shared.Extensions;

namespace WildlifeMortalities.App.Extensions;

public static class ReportExtensions
{
    public static T? GetEnumValueCustomAttribute<T>(this Enum enumValue)
        where T : Attribute =>
        enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttributes<T>()
            .FirstOrDefault();

    public static string GetReportType(this ReportType enumValue) =>
        enumValue.GetEnumValueCustomAttribute<ReportTypeAttribute>()?.ReportType.Name
        ?? $"Error: enum {enumValue} is missing a report type attribute";

    public static string GetReportTypeDisplayName(this string input)
    {
        var types = Enum.GetValues<ReportType>();
        var reportType = Array.Find(types, x => x.GetReportType() == input);
        return reportType.GetDisplayName();
    }

    public static bool IsCreatable(this ReportType enumValue) =>
        enumValue.GetEnumValueCustomAttribute<IsCreatable>() != null;
}
