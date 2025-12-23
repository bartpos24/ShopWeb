using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ShopWeb.Application.TransferObjects.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Extensions
{
    public class SheetGenerator
    {
        static SheetGenerator()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }
        public async Task<byte[]> GenerateSheet(InventorySummaryVm inventorySummaryVm)
        {
            return await Task.Run(() =>
            {
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(15);
                        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

                        page.Header().Element(content => Header(content, inventorySummaryVm));
                        page.Content().Element(content => Content(content, inventorySummaryVm));
                        page.Footer().Element(Footer);
                    });
                });

                return document.GeneratePdf();
            });
        }

        private void Header(IContainer container, InventorySummaryVm model)
        {
            var headerColor = "#FF9999"; // Pink/red color from template
            var lightPink = "#FFE0E0";

            container.Column(column =>
            {
                // Title section
                column.Item().Background(headerColor).Padding(8).Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("ARKUSZ SPISU Z NATURY")
                            .FontSize(12).Bold();
                        col.Item().Text("(uniwersalny)")
                            .FontSize(8);
                    });

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignRight().Text($"Rodzaj inwentaryzacji: {model.Inventory.Type ?? ""}").FontSize(7);
                        col.Item().AlignRight().Text("Strona nr: 1").FontSize(7);
                        col.Item().AlignRight().Text($"Sposób przeprowadzenia: {model.Inventory.ExecuteWay ?? ""}").FontSize(7);
                    });
                });

                // Company info section
                column.Item().Background(lightPink).Padding(4).Text(text =>
                {
                    text.Span("Firma (nazwa i siedziba): ").FontSize(7).Bold();
                    text.Span(model.Inventory.CompanyInformation ?? "").FontSize(7);
                });

                column.Item().Background(lightPink).Padding(4).Row(row =>
                {
                    row.RelativeItem().Text(text =>
                    {
                        text.Span("Nazwa: ").FontSize(7).Bold();
                        text.Span(model.Inventory.Name ?? "").FontSize(7);
                    });
                    row.RelativeItem().Text(text =>
                    {
                        text.Span("Osoba odpowiedzialna: ").FontSize(7).Bold();
                        text.Span(model.Inventory.ResponsiblePerson ?? "").FontSize(7);
                    });
                });

                // Commission team section
                column.Item().Background(lightPink).Padding(4).Column(col =>
                {
                    col.Item().Text("SKŁAD KOMISJI INWENTARYZACYJNEJ (imię, nazwisko i stanowisko służbowe):")
                        .FontSize(7).Bold();

                    if (model.Inventory.CommissionTeam != null && model.Inventory.CommissionTeam.Any())
                    {
                        col.Item().Text(string.Join(", ", model.Inventory.CommissionTeam))
                            .FontSize(6);
                    }
                });

                // Verification persons section
                column.Item().Background(lightPink).Padding(4).Row(row =>
                {
                    row.RelativeItem().Text(text =>
                    {
                        text.Span("Wycenił: ").FontSize(7).Bold();
                        text.Span(model.Inventory.PersonToValue ?? "").FontSize(7);
                    });
                    row.RelativeItem().Text(text =>
                    {
                        text.Span("Sprawdził: ").FontSize(7).Bold();
                        text.Span(model.Inventory.PersonToCheck ?? "").FontSize(7);
                    });
                });

                column.Item().PaddingVertical(3);
            });
        }

        private void Content(IContainer container, InventorySummaryVm model)
        {
            var headerColor = "#FF9999";
            var lightPink = "#FFE0E0";
            var alternateRowColor = Color.FromHex("#FFFAFA");

            container.Column(column =>
            {
                // Date information
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text($"Data rozpoczęcia: {model.Inventory.StartDate:dd.MM.yyyy}").FontSize(7);
                    row.RelativeItem().Text($"Data zakończenia: {model.Inventory.EndDate?.ToString("dd.MM.yyyy") ?? ""}").FontSize(7);
                });

                column.Item().PaddingVertical(3);

                // Main table
                column.Item().Table(table =>
                {
                    // Define columns - removed "Ilość księgowa"
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(25);  // Lp.
                        columns.RelativeColumn(4);   // Nazwa (opis/artykuł) - increased
                        columns.ConstantColumn(50);  // Cecha, symbol, numer, gatunek
                        columns.ConstantColumn(35);  // Jednostka miary
                        columns.ConstantColumn(50);  // Ilość faktyczna
                        columns.ConstantColumn(50);  // Cena jednostkowa
                        columns.ConstantColumn(60);  // Wartość (zł)
                        columns.ConstantColumn(50);  // Uwagi
                    });

                    // Header row
                    table.Header(header =>
                    {
                        header.Cell().Background(headerColor).Border(0.5f).Padding(2)
                            .AlignCenter().Text("Lp.").FontSize(6).Bold();

                        header.Cell().Background(headerColor).Border(0.5f).Padding(2)
                            .AlignCenter().Text("PRZEDMIOT SPISYWANY\nNazwa (opis/artykuł)").FontSize(6).Bold();

                        header.Cell().Background(headerColor).Border(0.5f).Padding(2)
                            .Text("Cecha, symbol, numer, gatunek").FontSize(6).Bold();

                        header.Cell().Background(headerColor).Border(0.5f).Padding(2)
                            .Text("Jednostka miary").FontSize(6).Bold();

                        header.Cell().Background(headerColor).Border(0.5f).Padding(2)
                            .Text("Ilość stwierdzona").FontSize(6).Bold();

                        header.Cell().Background(headerColor).Border(0.5f).Padding(2)
                            .Text("Cena jednostkowa").FontSize(6).Bold();

                        header.Cell().Background(headerColor).Border(0.5f).Padding(2)
                            .Text("Wartość").FontSize(6).Bold();

                        header.Cell().Background(lightPink).Border(0.5f).Padding(2)
                            .Text("Uwagi").FontSize(6).Bold();
                    });

                    // Data rows
                    int index = 1;
                    foreach (var position in model.Positions)
                    {
                        var rowColor = index % 2 == 0 ? Colors.White : alternateRowColor;

                        table.Cell().Background(rowColor).Border(0.5f).Padding(2)
                            .AlignCenter().AlignMiddle().Text(index.ToString()).FontSize(7);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(2)
                            .AlignLeft().AlignMiddle().Text(position.ProductName).FontSize(7);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(2)
                            .AlignCenter().AlignMiddle().Text("").FontSize(7);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(2)
                            .AlignCenter().AlignMiddle().Text(position.Unit).FontSize(7);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(2)
                            .AlignRight().AlignMiddle().Text(position.Quantity.ToString("N2")).FontSize(7);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(2)
                            .AlignRight().AlignMiddle().Text(position.Price.ToString("N2")).FontSize(7);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(2)
                            .AlignRight().AlignMiddle().Text((position.Quantity * position.Price).ToString("N2")).FontSize(7);

                        table.Cell().Background(lightPink).Border(0.5f).Padding(2)
                            .AlignLeft().AlignMiddle().Text("").FontSize(7);

                        index++;
                    }

                    // Add empty rows to fill the page (more rows for portrait)
                    for (int i = index; i <= index + 25; i++)
                    {
                        var rowColor = i % 2 == 0 ? Colors.White : alternateRowColor;

                        for (int col = 0; col < 8; col++) // Changed from 9 to 8
                        {
                            var bgColor = col == 7 ? Color.FromHex(lightPink) : rowColor;
                            table.Cell().Background(bgColor).Border(0.5f).Padding(2)
                                .Text("").FontSize(7);
                        }
                    }

                    // Summary row
                    table.Cell().ColumnSpan(6).Background(headerColor).Border(0.5f).Padding(2)
                        .AlignCenter().Text("RAZEM").FontSize(7).Bold();

                    var totalValue = model.Positions.Sum(p => p.Quantity * p.Price);
                    table.Cell().Background(headerColor).Border(0.5f).Padding(2)
                        .AlignRight().Text(totalValue.ToString("N2")).FontSize(7).Bold();

                    table.Cell().Background(lightPink).Border(0.5f).Padding(2)
                        .Text("").FontSize(7);
                });
            });
        }

        private void Footer(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().PaddingTop(8).Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Wykonał:").FontSize(7);
                        col.Item().PaddingTop(15).BorderBottom(0.5f).Text("");
                        col.Item().PaddingTop(2).Text("(czytelny podpis)").FontSize(6);
                    });

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Wycenił:").FontSize(7);
                        col.Item().PaddingTop(15).BorderBottom(0.5f).Text("");
                        col.Item().PaddingTop(2).Text("(czytelny podpis)").FontSize(6);
                    });

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Sprawdził:").FontSize(7);
                        col.Item().PaddingTop(15).BorderBottom(0.5f).Text("");
                        col.Item().PaddingTop(2).Text("(czytelny podpis)").FontSize(6);
                    });
                });

                column.Item().PaddingTop(8).Text(text =>
                {
                    text.Span("Podpisy: osoby odpowiedzialnej materialnie i członków zespołu   Sprawdził: ").FontSize(6);
                    text.Span("(czytelny podpis)").FontSize(5).Italic();
                });
            });
        }
    }
}
