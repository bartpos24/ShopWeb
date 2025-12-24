using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ShopWeb.Application.TransferObjects.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                        page.Footer().Element(content => Footer(content, inventorySummaryVm));
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
                            .FontSize(13).Bold();
                        col.Item().Row(r =>
                        {
                            r.AutoItem().Text("(uniwersalny)").FontSize(11);
                            r.RelativeItem().AlignCenter()
                                .Text("Strona nr ___________").FontSize(9);
                        });
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
                            r.RelativeItem().BorderBottom(0.5f).AlignCenter().AlignMiddle()
                                .Text(model.Inventory.ResponsiblePerson ?? "").FontSize(9);
                        });
                    });
                });

                // Unit name and address
                column.Item().Row(row =>//.Background(lightPink).Border(0.5f).Column(col =>
                {
                    row.RelativeItem().Background(lightPink).Border(0.5f).Padding(4).Column(col =>
                    {
                        col.Item().Row(r =>
                        {
                            r.AutoItem().Text("Nazwa i adres\njednostki inwentaryzowanej").FontSize(7);
                            r.RelativeItem().BorderBottom(0.5f).AlignCenter().AlignMiddle()
                                .Text(model.Inventory.CompanyInformation ?? "").FontSize(9);
                        });
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
                                    r.ConstantItem(15).Text($"{i + 1}.").FontSize(8);
                                    r.RelativeItem().BorderBottom(0.5f)
                                        .Text(model.Inventory.CommissionTeam[i]).FontSize(8);
                                });
                            }

                            // Add empty lines if less than 3
                            for (int i = model.Inventory.CommissionTeam.Count; i < 3; i++)
                            {
                                col.Item().Row(r =>
                                {
                                    r.ConstantItem(15).Text($"{i + 1}.").FontSize(8);
                                    r.RelativeItem().BorderBottom(0.5f).Text("").FontSize(8);
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
                                r.ConstantItem(15).Text($"{i + 1}.").FontSize(8);
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
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f).AlignCenter()
                            .Text($"  {model.Inventory.StartDate.Day:D2}  ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f).AlignCenter()
                            .Text($"  {model.Inventory.StartDate.Month:D2}  ").FontSize(7);
                        r.AutoItem().Text(" 20 ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f).AlignCenter()
                            .Text($"  {model.Inventory.StartDate.Year.ToString().Substring(2)}  ").FontSize(7);
                        r.AutoItem().Text(" r. o godz. ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f).AlignCenter()
                            .Text($"  {model.Inventory.StartDate.Hour:D2}  ").FontSize(7);
                        r.AutoItem().Text(" : ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f).AlignCenter()
                            .Text($"  {model.Inventory.StartDate.Minute:D2}  ").FontSize(7);
                    });

                    row.RelativeItem().Row(r =>
                    {
                        r.AutoItem().Text("Spis zakończono dnia: ").FontSize(7);
                        var endDate = model.Inventory.EndDate ?? DateTime.Now;
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f).AlignCenter()
                            .Text($"  {endDate.Day:D2}  ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f).AlignCenter()
                            .Text($"  {endDate.Month:D2}  ").FontSize(7);
                        r.AutoItem().Text(" 20 ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f).AlignCenter()
                            .Text($"  {endDate.Year.ToString().Substring(2)}  ").FontSize(7);
                        r.AutoItem().Text(" r. o godz. ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f).AlignCenter()
                            .Text($"  {endDate.Hour:D2}  ").FontSize(7);
                        r.AutoItem().Text(" : ").FontSize(7);
                        r.AutoItem().PaddingHorizontal(2).BorderBottom(0.5f).AlignCenter()
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
                        columns.ConstantColumn(20);  // Lp.
                        columns.RelativeColumn(4);   // Nazwa (opis/artykuł) - increased
                        columns.ConstantColumn(50);  // Cecha, symbol, numer, gatunek
                        columns.ConstantColumn(25);  // Jednostka miary
                        columns.ConstantColumn(40);  // Ilość faktyczna
                        columns.ConstantColumn(50);  // Cena jednostkowa
                        columns.ConstantColumn(50);  // Wartość (zł)
                        columns.ConstantColumn(50);  // Uwagi
                    });

                    table.Header(header =>
                    {
                        // === FIRST HEADER ROW ===

                        // Lp. - spans 2 rows
                        header.Cell().RowSpan(2).Background(headerColor).Border(0.5f).Padding(2)
                            .AlignMiddle().AlignCenter().Text("Lp.").FontSize(7).Bold();

                        // PRZEDMIOT SPISYWANY - spans 2 columns
                        header.Cell().ColumnSpan(2).Background(headerColor).BorderTop(0.5f).BorderLeft(0.5f).BorderRight(0.5f).Padding(2)
                            .AlignMiddle().AlignCenter().Text("PRZEDMIOT SPISYWANY").FontSize(7).Bold();

                        // J.M. - spans 2 rows
                        header.Cell().RowSpan(2).Background(headerColor).Border(0.5f).Padding(2)
                            .AlignMiddle().AlignCenter().Text("J.M.").FontSize(7).Bold();

                        // Ilość stwierdzona - spans 2 rows
                        header.Cell().RowSpan(2).Background(headerColor).Border(0.5f).Padding(2)
                            .AlignMiddle().AlignCenter().Text("Ilość\nstwierdzona").FontSize(7).Bold();

                        // Cena jednostkowa - spans 2 rows
                        header.Cell().RowSpan(2).Background(headerColor).Border(0.5f).Padding(2)
                            .AlignMiddle().AlignCenter().Text("Cena\njednostkowa").FontSize(7).Bold();

                        // Wartość - spans 2 rows
                        header.Cell().RowSpan(2).Background(headerColor).Border(0.5f).Padding(2)
                            .AlignMiddle().AlignCenter().Text("Wartość").FontSize(7).Bold();

                        // Uwagi - spans 2 rows
                        header.Cell().RowSpan(2).Background(headerColor).Border(0.5f).Padding(2)
                            .AlignMiddle().AlignCenter().Text("Uwagi").FontSize(7).Bold();

                        // === SECOND HEADER ROW ===

                        // Nazwa (określenie)
                        header.Cell().Background(headerColor).Border(0.5f).Padding(2)
                            .AlignMiddle().AlignCenter().Text("Nazwa (określenie)").FontSize(6).Bold();

                        // Cecha, symbol, numer, gatunek
                        header.Cell().Background(headerColor).Border(0.5f).Padding(2)
                            .AlignMiddle().AlignCenter().Text("Cecha, symbol,\nnumer, gatunek").FontSize(6).Bold();
                    });

                    // Data rows
                    int index = 1;
                    foreach (var position in model.Positions)
                    {
                        var rowColor = Colors.White;//index % 2 == 0 ? Colors.White : alternateRowColor;

                        table.Cell().Background(rowColor).Border(0.5f).Padding(2)
                            .AlignCenter().AlignMiddle().Text(index.ToString()).FontSize(9);

                        table.Cell().Background(rowColor).BorderLeft(0.5f).BorderHorizontal(0.5f).Padding(2)
                            .AlignLeft().AlignMiddle().Text(position.ProductName).FontSize(9);

                        table.Cell().Background(lightPink).BorderRight(0.5f).BorderHorizontal(0.5f).Padding(2)
                            .AlignCenter().AlignMiddle().Text("").FontSize(9);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(2)
                            .AlignCenter().AlignMiddle().Text(position.Unit).FontSize(9);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(2)
                            .AlignRight().AlignMiddle().Text(position.Quantity.ToString("N2")).FontSize(9);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(2)
                            .AlignRight().AlignMiddle().Text(position.Price.ToString("N2")).FontSize(9);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(2)
                            .AlignRight().AlignMiddle().Text((position.Quantity * position.Price).ToString("N2")).FontSize(9);

                        table.Cell().Background(rowColor).Border(0.5f).Padding(2)
                            .AlignLeft().AlignMiddle().Text("").FontSize(9);

                        index++;
                    }

                    // Add empty rows to fill the page (more rows for portrait)
                    for (int i = index; i <= 45; i++)
                    {
                        var rowColor = Colors.White;//i % 2 == 0 ? Colors.White : alternateRowColor;

                        for (int col = 0; col < 8; col++) // Changed from 9 to 8
                        {
                            var bgColor = col == 2 ? Color.FromHex(lightPink) : rowColor;
                            if (col == 1)
                            {
                                table.Cell().Background(bgColor).BorderLeft(0.5f).BorderHorizontal(0.5f).Padding(2)
                                    .Text("").FontSize(7);
                            }
                            else if (col == 2)
                            {
                                table.Cell().Background(bgColor).BorderRight(0.5f).BorderHorizontal(0.5f).Padding(2)
                                    .Text("").FontSize(7);
                            }
                            else
                            {
                                table.Cell().Background(bgColor).Border(0.5f).Padding(2)
                                    .Text("").FontSize(7);
                            }
                        }
                    }

                    table.Cell().Padding(2)
                        .Text("").FontSize(7);
                    table.Cell().Padding(2)
                        .Text("").FontSize(7);
                    // Summary row
                    table.Cell().ColumnSpan(4).Background(headerColor).Border(0.5f).Padding(2)
                        .AlignCenter().Text("RAZEM").FontSize(9).Bold();

                    var totalValue = model.Positions.Sum(p => p.Quantity * p.Price);
                    table.Cell().Background(headerColor).Border(0.5f).Padding(2)
                        .AlignRight().Text(totalValue.ToString("N2")).FontSize(9).Bold();
                });
            });
        }

        private void Footer(IContainer container, InventorySummaryVm inventorySummaryVm)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    // Left: m.p. box
                    row.ConstantItem(230).Padding(4).AlignLeft().Text("").FontSize(7);

                    // Right: Inventory details
                    row.RelativeItem().Padding(4).Column(col =>
                    {
                        // First row: Rodzaj inwentaryzacji with value
                        col.Item().Row(r =>
                        {
                            r.AutoItem().PaddingRight(10).Text("Wycenił: ").FontSize(7);
                            r.RelativeItem(200).BorderBottom(0.5f).AlignCenter()
                                .Text($"{inventorySummaryVm.Inventory.PersonToValue}").FontSize(9);
                            r.ConstantItem(5);
                            r.ConstantItem(110).BorderBottom(0.5f).AlignCenter().AlignBottom().PaddingBottom(-10)
                                .Text("(podpis)").FontSize(5);
                        });
                    });
                });

                column.Item().Row(row =>
                {
                    // Left: m.p. box
                    row.ConstantItem(230).Padding(4).AlignLeft().Text("Podpisy: osoby odpowiedzialnej materialnie i członków zespołu").FontSize(7);

                    // Right: Inventory details
                    row.RelativeItem().Padding(4).Column(col =>
                    {
                        // First row: Rodzaj inwentaryzacji with value
                        col.Item().Row(r =>
                        {
                            r.AutoItem().PaddingRight(10).Text("Sprawdził: ").FontSize(7);
                            r.RelativeItem(200).BorderBottom(0.5f).AlignCenter()
                                .Text($"{inventorySummaryVm.Inventory.PersonToCheck}").FontSize(9);
                            r.ConstantItem(5);
                            r.ConstantItem(110).BorderBottom(0.5f).AlignCenter().AlignBottom().PaddingBottom(-10)
                                .Text("(podpis)").FontSize(5);
                        });
                    });
                });
            });
        }
    }
}
