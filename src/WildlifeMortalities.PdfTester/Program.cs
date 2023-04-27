using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.People;

using var context = new AppDbContext();

var client = await context.People.OfType<Client>().FirstOrDefaultAsync();

var document = Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.A4);
        page.Margin(15, Unit.Millimetre);
        page.PageColor(Colors.White);
        page.DefaultTextStyle(x => x.FontFamily("Nunito Sans").FontSize(12));

        page.Header()
            .Column(column =>
            {
                column
                    .Item()
                    .Row(row =>
                    {
                        row.RelativeItem()
                            .Image("Resources\\GYWordmark_2018_RGB.png", ImageScaling.FitHeight);
                        row.RelativeItem()
                            .Column(column =>
                            {
                                column
                                    .Item()
                                    .AlignRight()
                                    .Text("4X9H")
                                    .FontSize(16)
                                    .FontColor(Colors.Red.Medium)
                                    .FontFamily("Montserrat");
                                column
                                    .Item()
                                    .AlignRight()
                                    .Text("Individual hunt report")
                                    .SemiBold()
                                    .FontSize(18)
                                    .FontColor(Colors.Black)
                                    .FontFamily("Montserrat");
                            });
                    });
            });

        page.Content()
            .PaddingVertical(1, Unit.Centimetre)
            .Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Cell().Row(1).Column(1).Component(new ClientComponent(client));
                table.Cell().Row(1).Column(3).Element(Block).Text("A");
                table.Cell().Row(2).Column(2).Element(Block).Text("B");
                table.Cell().Row(3).Column(3).Element(Block).Text("C");

                table.Cell().ColumnSpan(3).Image("Resources\\YG_Wildlife_Lichen_RGB.png");

                static IContainer Block(IContainer container)
                {
                    return container
                        .Border(1)
                        .Background(Colors.Grey.Lighten3)
                        .ShowOnce()
                        .MinWidth(50)
                        .MinHeight(50)
                        .AlignCenter()
                        .AlignMiddle();
                }
            });

        page.Footer()
            .AlignCenter()
            .Text(x =>
            {
                x.CurrentPageNumber();
                x.Span(" / ");
                x.TotalPages();
            });
    });
});

document.ShowInPreviewer();

public class ClientComponent : IComponent
{
    private readonly Client _client;

    public ClientComponent(Client client)
    {
        _client = client;
    }

    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text($"Client Id: {_client.EnvPersonId}");
            column.Item().Text($"Name: {_client.FirstName} {_client.LastName}");
            column.Item().Text($"Date of birth: {_client.BirthDate.ToShortDateString()}");
        });
    }
}
