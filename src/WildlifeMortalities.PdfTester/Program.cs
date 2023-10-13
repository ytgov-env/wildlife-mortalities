using Microsoft.EntityFrameworkCore;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using SkiaSharp;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;

using var context = new AppDbContext();

var client = await context.People.OfType<Client>().FirstOrDefaultAsync();

var qrGenerator = new QRCodeGenerator();
var qrCodeData = qrGenerator.CreateQrCode(
    "The text which should be encoded.",
    QRCodeGenerator.ECCLevel.Q
);
var qrCode = new PngByteQRCode(qrCodeData);
var qrCodeImage = qrCode.GetGraphic(20);

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
                        row.RelativeItem().Image("Resources\\GYWordmark_2018_RGB.png").FitHeight();
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

                table
                    .Cell()
                    .ColumnSpan(3)
                    .Layers(layers =>
                    {
                        layers
                            .PrimaryLayer()
                            .Canvas(
                                (canvas, size) =>
                                {
                                    DrawRoundedRectangle("#e7e8e8", false);

                                    void DrawRoundedRectangle(string color, bool isStroke)
                                    {
                                        using var paint = new SKPaint
                                        {
                                            Color = SKColor.Parse(color),
                                            IsStroke = isStroke,
                                            StrokeWidth = 2,
                                            IsAntialias = true
                                        };

                                        canvas.DrawRoundRect(
                                            0,
                                            0,
                                            size.Width,
                                            size.Height / 3,
                                            20,
                                            20,
                                            paint
                                        );
                                    }
                                }
                            );

                        table
                            .Cell()
                            .Row(1)
                            .Column(3)
                            .PaddingLeft(8)
                            .Component(new ClientComponent(client));

                        layers
                            .Layer()
                            .Canvas(
                                (canvas, size) =>
                                {
                                    using var paint = new SKPaint
                                    {
                                        Color = SKColor.Parse("#636466"),
                                        IsAntialias = true
                                    };

                                    canvas.DrawRect(
                                        -43,
                                        30,
                                        (float)(size.Width / 2.1),
                                        size.Height / 8,
                                        paint
                                    );
                                }
                            );

                        layers
                            .Layer()
                            .Canvas(
                                (canvas, size) =>
                                {
                                    using var paint = new SKPaint
                                    {
                                        Color = SKColor.Parse("#ffffff"),
                                        IsAntialias = true
                                    };

                                    canvas.DrawRect(
                                        200,
                                        30,
                                        (float)(size.Width / 6),
                                        size.Height / 8,
                                        paint
                                    );
                                }
                            );
                    });

                //table.Cell().Row(1).Column(3).Element(Block).Text("A");
                //table.Cell().Row(2).Column(2).Element(Block).Text("B");
                //table.Cell().Row(3).Column(3).Element(Block).Text("C");

                table.Cell().ColumnSpan(3).Image("Resources\\YG_Wildlife_Lichen_RGB.png");
                table.Cell().Column(1).Image(qrCodeImage);

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
            column.Item().Text("Client information").SemiBold().FontSize(16).FontColor("#3d0237");
            column.Item().Text("Client id").Bold();
            column.Item().Text(_client.EnvPersonId);
            column.Item().Text("First name").Bold();
            column.Item().Text(_client.FirstName);
            column.Item().Text("Last name").Bold();
            column.Item().Text(_client.LastName);
            column.Item().Text("Date of birth").Bold();
            column
                .Item()
                .Text(
                    _client.BirthDate.ToString(
                        WildlifeMortalities.Shared.Constants.FormatStrings.StandardDateFormat
                    )
                );
        });
    }
}

public class ReportBaseComponent : IComponent
{
    private readonly Report _report;

    public ReportBaseComponent(Report report)
    {
        _report = report;
    }

    public void Compose(IContainer container)
    {
        throw new NotImplementedException();
    }
}
