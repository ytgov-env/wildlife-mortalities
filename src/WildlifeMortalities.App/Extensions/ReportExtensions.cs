using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.App.Features.Reports;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Shared.Extensions;
using WildlifeMortalities.Shared.Services.Reports.Single;

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

    public static async Task<int[]> GetActivityIds(this DbSet<Report> reports, int reportId)
    {
        var report = await reports.WithActivities().FirstAsync(x => x.Id == reportId);
        return report.GetActivities().Select(x => x.Id).ToArray();
    }
}
