using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using WildlifeMortalities.Data;

using var context = new AppDbContext();

var document = Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.A4);
        page.Margin(2, Unit.Centimetre);
        page.PageColor(Colors.White);
        page.DefaultTextStyle(x => x.FontFamily(Fonts.Arial).FontSize(20));

        page.Header()
            .Column(column =>
            {
                column
                    .Item()
                    .Row(row =>
                    {
                        row.RelativeItem()
                            .Image(
                                "..\\WildlifeMortalities.Shared\\Resources\\GYWordmark_2018_RGB.png",
                                ImageScaling.FitHeight
                            );
                        row.RelativeItem()
                            .Text("Harvest report")
                            .SemiBold()
                            .FontSize(24)
                            .FontColor(Colors.Black);
                    });
            });

        page.Content()
            .PaddingVertical(1, Unit.Centimetre)
            .Column(column =>
            {
                column
                    .Item()
                    .Row(row =>
                    {
                        row.RelativeItem().LabelCell("Label 1");

                        row.RelativeItem(3)
                            .Grid(grid =>
                            {
                                grid.Columns(3);

                                grid.Item(2).LabelCell("Label 2");
                                grid.Item().LabelCell("Label 3");

                                grid.Item(2).ValueCell().Text("Value 2");
                                grid.Item().ValueCell().Text("Value 3");
                            });
                    });

                column
                    .Item()
                    .Row(row =>
                    {
                        row.RelativeItem().ValueCell().Text("Value 1");

                        row.RelativeItem(3)
                            .Grid(grid =>
                            {
                                grid.Columns(3);

                                grid.Item().LabelCell("Label 4");
                                grid.Item(2).LabelCell("Label 5");

                                grid.Item().ValueCell().Text("Value 4");
                                grid.Item(2).ValueCell().Text("Value 5");
                            });
                    });

                column
                    .Item()
                    .Row(row =>
                    {
                        row.RelativeItem().LabelCell("Label 6");
                        row.RelativeItem()
                            .Image(
                                "..\\WildlifeMortalities.Shared\\Resources\\YG_Environment_Lichen_RGB.png"
                            );
                    });
            });

        page.Footer()
            .AlignCenter()
            .Text(x =>
            {
                x.Span("Page ");
                x.CurrentPageNumber();
            });
    });
});

document.ShowInPreviewer();
