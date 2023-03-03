using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

internal static class SimpleExtension
{
    private static IContainer Cell(this IContainer container, bool dark)
    {
        return container
            .Border(1)
            .Background(dark ? Colors.Grey.Lighten2 : Colors.White)
            .Padding(10);
    }

    // displays only text label
    public static void LabelCell(this IContainer container, string text)
    {
        container.Cell(true).Text(text).Medium();
    }

    // allows to inject any type of content, e.g. image
    public static IContainer ValueCell(this IContainer container)
    {
        return container.Cell(false);
    }
}
