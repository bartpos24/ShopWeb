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
                        page.Size(PageSizes.A4.Landscape());
                        page.Margin(20);
                        page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                        page.Header().Element(Header);
                        page.Content().Element(content => Content(content, inventorySummaryVm));
                        page.Footer().Element(Footer);
                    });
                });

                return document.GeneratePdf();
            });
        }

        private void Header(IContainer container)
        {
            var headerColor = "#FF9999"; // Pink/red color from template

            container.Column(column =>
            {
                // Title section
                column.Item().Background(headerColor).Padding(10).Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("ARKUSZ SPISU Z NATURY")
                            .FontSize(14).Bold();
                        col.Item().Text("(uniwersalny)")
                            .FontSize(10);
                    });

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignRight().Text("Rodzaj inwentaryzacji").FontSize(8);
                        col.Item().AlignRight().Text("Strona nr").FontSize(8);
                        col.Item().AlignRight().Text("Sposób przeprowadzenia").FontSize(8);
                    });
                });

                // Info section
                column.Item().Background("#FFE0E0").Padding(5).Text(text =>
                {
                    text.Span("Firma (nazwa i siedziba): ").FontSize(8);
                    text.Span("Miejło materiałów odpowiedzialnej").FontSize(8);
                });

                column.Item().Background("#FFE0E0").Padding(5).Row(row =>
                {
                    row.RelativeItem().Text("Nazwę i adres:").FontSize(8);
                    row.RelativeItem().Text("Jednostki inwentaryzacyjnej:").FontSize(8);
                });

                // Additional info row
                column.Item().Background("#FFE0E0").Padding(5).Row(row =>
                {
                    row.RelativeItem().Text("SKŁAD KOMISJI INWENTARYZACYJNEJ (imię, nazwisko i stanowisko służbowe):")
                        .FontSize(7);
                    row.RelativeItem().Text("IMIĘ I OSÓBY OBECNE PRZY SPISIE (imię, nazwisko i stanowisko służbowe):")
                        .FontSize(7);
                });

                column.Item().PaddingVertical(5);
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
                    row.RelativeItem().Text($"Data rozpoczęcia dnia: {model.Inventory.StartDate:dd.MM.yyyy}");
                    row.RelativeItem().Text($"Data zakończenia dnia: {model.Inventory.EndDate?.ToString("dd.MM.yyyy") ?? ""}");
                });

                column.Item().PaddingVertical(5);

                // Main table
                column.Item().Table(table =>
                {
                    // Define columns
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(30);  // Lp.
                        columns.RelativeColumn(3);   // Nazwa (opis/artykuł)
                        columns.ConstantColumn(60);  // Cecha, symbol, numer, gatunek
                        columns.ConstantColumn(40);  // Jednostka miary
                        columns.ConstantColumn(50);  // Ilość księgowa
                        columns.ConstantColumn(50);  // Ilość faktyczna
                        columns.ConstantColumn(60);  // Cena jednostkowa
                        columns.ConstantColumn(70);  // Wartość (zł)
                        columns.ConstantColumn(60);  // Uwagi
                    });

                    // Header row
                    table.Header(header =>
                    {
                        header.Cell().Background(headerColor).Border(0.5f).Padding(3)
                            .Text("Lp.").FontSize(7).Bold();

                        header.Cell().Background(headerColor).Border(0.5f).Padding(3)
                            .Text("PRZEDMIOT SPISYWANY\nNazwa (opis/artykuł)").FontSize(7).Bold();

                        header.Cell().Background(headerColor).Border(0.5f).Padding(3)
                            .Text("Cecha, symbol, numer, gatunek").FontSize(7).Bold();

                        header.Cell().Background(headerColor).Border(0.5f).Padding(3)
                            .Text("Jednostka miary").FontSize(7).Bold();

                        header.Cell().Background(headerColor).Border(0.5f).Padding(3)
                            .Text("Ilość księgowa").FontSize(7).Bold();

                        header.Cell().Background(headerColor).Border(0.5f).Padding(3)
                            .Text("Ilość faktyczna\nz przymierzenia").FontSize(7).Bold();

                        header.Cell().Background(headerColor).Border(0.5f).Padding(3)
                            .Text("Cena jednostkowa").FontSize(7).Bold();

                        header.Cell().Background(headerColor).Border(0.5f).Padding(3)
                            .Text("Wartość\n(zł)").FontSize(7).Bold();

                        header.Cell().Background(lightPink).Border(0.5f).Padding(3)
                            .Text("Uwagi").FontSize(7).Bold();
                    });

                    // Data rows
                    int index = 1;
                    foreach (var position in model.Positions)
                    {
                        var rowColor = index % 2 == 0 ? Colors.White : alternateRowColor;

                        table.Cell().Background(rowColor).Border(0.5f).Padding(3)
                            .AlignCenter().AlignMiddle().Text(index.ToString()).FontSize(8);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(3)
                            .AlignLeft().AlignMiddle().Text(position.ProductName).FontSize(8);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(3)
                            .AlignCenter().AlignMiddle().Text("").FontSize(8);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(3)
                            .AlignCenter().AlignMiddle().Text(position.Unit).FontSize(8);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(3)
                            .AlignRight().AlignMiddle().Text("").FontSize(8);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(3)
                            .AlignRight().AlignMiddle().Text(position.Quantity.ToString("N2")).FontSize(8);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(3)
                            .AlignRight().AlignMiddle().Text(position.Price.ToString("N2")).FontSize(8);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(3)
                            .AlignRight().AlignMiddle().Text((position.Quantity * position.Price).ToString("N2")).FontSize(8);

                        table.Cell().Background(lightPink).Border(0.5f).Padding(3)
                            .AlignLeft().AlignMiddle().Text("").FontSize(8);

                        index++;
                    }

                    // Add empty rows to fill the page (approximately 20 more rows)
                    for (int i = index; i <= index + 15; i++)
                    {
                        var rowColor = i % 2 == 0 ? Colors.White : alternateRowColor;

                        for (int col = 0; col < 9; col++)
                        {
                            var bgColor = col == 8 ? Color.FromHex(lightPink) : alternateRowColor;
                            table.Cell().Background(bgColor).Border(0.5f).Padding(3)
                                .Text("").FontSize(8);
                        }
                    }

                    // Summary row
                    table.Cell().ColumnSpan(7).Background(headerColor).Border(0.5f).Padding(3)
                        .AlignCenter().Text("RAZEM").FontSize(8).Bold();

                    var totalValue = model.Positions.Sum(p => p.Quantity * p.Price);
                    table.Cell().Background(headerColor).Border(0.5f).Padding(3)
                        .AlignRight().Text(totalValue.ToString("N2")).FontSize(8).Bold();

                    table.Cell().Background(lightPink).Border(0.5f).Padding(3)
                        .Text("").FontSize(8);
                });
            });
        }

        private void Footer(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Wykonał:").FontSize(8);
                        col.Item().PaddingTop(20).BorderBottom(0.5f).Text("");
                        col.Item().PaddingTop(2).Text("(czytelny podpis)").FontSize(7);
                    });

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Wycenił:").FontSize(8);
                        col.Item().PaddingTop(20).BorderBottom(0.5f).Text("");
                        col.Item().PaddingTop(2).Text("(czytelny podpis)").FontSize(7);
                    });

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Sprawdził:").FontSize(8);
                        col.Item().PaddingTop(20).BorderBottom(0.5f).Text("");
                        col.Item().PaddingTop(2).Text("(czytelny podpis)").FontSize(7);
                    });
                });

                column.Item().PaddingTop(10).Text(text =>
                {
                    text.Span("Podpisy: osoby odpowiedzialnej materialnie i człenkow zespołu   Sprawdzil: ").FontSize(7);
                    text.Span("(czytelny podpis)").FontSize(6).Italic();
                });
            });
        }
    }
}
