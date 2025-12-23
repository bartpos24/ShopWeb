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
                // Top section with m.p. and title
                column.Item().Row(row =>
                {
                    // Left: m.p. box
                    row.ConstantItem(170).BorderTop(0.5f).BorderLeft(0.5f).BorderRight(0.5f).Padding(4).AlignTop()
                        .AlignLeft().Text("m.p.").FontSize(7);

                    // Center: Title
                    row.RelativeItem().Background(headerColor).Border(0.5f).Padding(6).Column(col =>
                    {
                        col.Item().AlignCenter().Text("ARKUSZ SPISU Z NATURY")
                            .FontSize(11).Bold();
                        col.Item().AlignCenter().Text("(uniwersalny) Strona nr ___________")
                            .FontSize(7);
                    });

                    // Right: Inventory details
                    row.RelativeItem().Border(0.5f).Padding(4).Column(col =>
                    {
                        // First row: Rodzaj inwentaryzacji with value
                        col.Item().Row(r =>
                        {
                            r.AutoItem().Text("Rodzaj inwentaryzacji ").FontSize(7);
                            r.RelativeItem().BorderBottom(0.5f).Padding(1).AlignCenter()
                                .Text(model.Inventory.Type ?? "").FontSize(9);
                        });

                        // Second row: Sposób przeprowadzenia with value
                        col.Item().PaddingTop(3).Row(r =>
                        {
                            r.AutoItem().Text("Sposób przeprowadzenia ").FontSize(7);
                            r.RelativeItem().BorderBottom(0.5f).Padding(1).AlignCenter()
                                .Text(model.Inventory.ExecuteWay ?? "").FontSize(9);
                        });
                    });
                });

                // Company name and address section
                column.Item().Row(row =>
                {
                    row.ConstantItem(170).BorderBottom(0.5f).BorderLeft(0.5f).BorderRight(0.5f).Padding(4).AlignBottom().AlignCenter()
                        .Text("Firma (nazwisko i imię), adres").FontSize(6);

                    row.RelativeItem().Background(lightPink).Border(0.5f).Padding(4).Column(col =>
                    {
                        col.Item().Row(r =>
                        {
                            r.AutoItem().Text("Imię i nazwisko\nosoby materialnie odpowiedzialnej").FontSize(7);
                            r.RelativeItem().BorderBottom(0.5f).Padding(1).AlignCenter()
                                .Text(model.Inventory.ResponsiblePerson ?? "").FontSize(9);
                        });
                    });
                });

                // Unit name and address
                column.Item().Background(lightPink).Border(0.5f).Padding(4).Column(col =>
                {
                    col.Item().Row(r =>
                    {
                        r.AutoItem().Text("Nazwa i adres\njednostki inwentaryzowanej").FontSize(7);
                        r.RelativeItem().BorderBottom(0.5f).Padding(1).AlignCenter()
                            .Text(model.Inventory.CompanyInformation ?? "").FontSize(9);
                    });
                });

                // Commission team and other persons section
                column.Item().Row(row =>
                {
                    // Left: Commission team
                    row.RelativeItem().Padding(4).Column(col =>
                    {
                        col.Item().Background(lightPink).Text("SKŁAD KOMISJI INWENTARYZACYJNEJ (Imię, nazwisko i stanowisko służbowe)")
                            .FontSize(6).AlignCenter();

                        if (model.Inventory.CommissionTeam != null && model.Inventory.CommissionTeam.Any())
                        {
                            for (int i = 0; i < Math.Min(model.Inventory.CommissionTeam.Count, 3); i++)
                            {
                                col.Item().Row(r =>
                                {
                                    r.ConstantItem(15).Text($"{i + 1}.").FontSize(6);
                                    r.RelativeItem().BorderBottom(0.5f).Padding(1)
                                        .Text(model.Inventory.CommissionTeam[i]).FontSize(6);
                                });
                            }

                            // Add empty lines if less than 3
                            for (int i = model.Inventory.CommissionTeam.Count; i < 3; i++)
                            {
                                col.Item().Row(r =>
                                {
                                    r.ConstantItem(15).Text($"{i + 1}.").FontSize(6);
                                    r.RelativeItem().BorderBottom(0.5f).Padding(1).Text("").FontSize(6);
                                });
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                col.Item().Row(r =>
                                {
                                    r.ConstantItem(15).Text($"{i + 1}.").FontSize(6);
                                    r.RelativeItem().BorderBottom(0.5f).Padding(1).Text("").FontSize(6);
                                });
                            }
                        }
                    });

                    // Right: Other persons present
                    row.RelativeItem().Padding(4).Column(col =>
                    {
                        col.Item().Background(lightPink).Text("INNE OSOBY OBECNE PRZY SPISIE (Imię, nazwisko i stanowisko służbowe)")
                            .FontSize(6).AlignCenter();

                        for (int i = 0; i < 3; i++)
                        {
                            col.Item().Row(r =>
                            {
                                r.ConstantItem(15).Text($"{i + 1}.").FontSize(6);
                                r.RelativeItem().BorderBottom(0.5f).Padding(1).Text("").FontSize(6);
                            });
                        }
                    });
                });

                // Date section at the bottom of header
                column.Item().Padding(4).Row(row =>
                {
                    row.RelativeItem().Row(r =>
                    {
                        r.AutoItem().Text("Spis rozpoczęto dnia: ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f)
                            .Text($"  {model.Inventory.StartDate.Day:D2}  ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f)
                            .Text($"  {model.Inventory.StartDate.Month:D2}  ").FontSize(7);
                        r.AutoItem().Text(" 20 ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f)
                            .Text($"  {model.Inventory.StartDate.Year.ToString().Substring(2)}  ").FontSize(7);
                        r.AutoItem().Text(" r. o godz. ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f)
                            .Text($"  {model.Inventory.StartDate.Hour:D2}  ").FontSize(7);
                        r.AutoItem().Text(" : ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f)
                            .Text($"  {model.Inventory.StartDate.Minute:D2}  ").FontSize(7);
                    });

                    row.RelativeItem().Row(r =>
                    {
                        r.AutoItem().Text("Spis zakończono dnia: ").FontSize(7);
                        var endDate = model.Inventory.EndDate ?? DateTime.Now;
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f)
                            .Text($"  {endDate.Day:D2}  ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f)
                            .Text($"  {endDate.Month:D2}  ").FontSize(7);
                        r.AutoItem().Text(" 20 ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f)
                            .Text($"  {endDate.Year.ToString().Substring(2)}  ").FontSize(7);
                        r.AutoItem().Text(" r. o godz. ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f)
                            .Text($"  {endDate.Hour:D2}  ").FontSize(7);
                        r.AutoItem().Text(" : ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f)
                            .Text($"  {endDate.Minute:D2}  ").FontSize(7);
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
