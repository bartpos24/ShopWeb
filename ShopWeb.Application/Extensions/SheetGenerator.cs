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
                            r.ConstantItem(5);
                            r.AutoItem().AlignCenter()
                                .Text("Strona nr:  ").FontSize(9);
                            r.RelativeItem().BorderBottom(0.5f).AlignCenter().Text(text =>
                            {
                                text.CurrentPageNumber().FontSize(11);
                            });
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
            const int itemsPerPage = 35;

            // Split positions into pages of 40 items
            var totalPages = (int)Math.Ceiling((double)model.Positions.Count / itemsPerPage);
            if (totalPages == 0) totalPages = 1; // At least one page even if no items

            container.Column(column =>
            {
                for (int pageIndex = 0; pageIndex < totalPages; pageIndex++)
                {
                    var pagePositions = model.Positions.Skip(pageIndex * itemsPerPage).Take(itemsPerPage).ToList();
            
            // Calculate total from ALL previous pages (pages before current one)
            var previousPageTotal = model.Positions
                .Take(pageIndex * itemsPerPage)
                .Sum(p => p.Quantity * p.Price);
            
            // Calculate cumulative total including current page (all pages up to and including current)
            var cumulativeTotal = model.Positions
                .Take((pageIndex + 1) * itemsPerPage)
                .Sum(p => p.Quantity * p.Price);

                    // Add page break before each page except the first
                    if (pageIndex > 0)
                    {
                        column.Item().PageBreak();
                    }

                    column.Item().Table(table =>
                    {
                        // Define columns
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(25);  // Lp.
                            columns.RelativeColumn(4);   // Nazwa (opis/artykuł)
                            columns.ConstantColumn(50);  // Cecha, symbol, numer, gatunek
                            columns.ConstantColumn(25);  // Jednostka miary
                            columns.ConstantColumn(40);  // Ilość faktyczna
                            columns.ConstantColumn(50);  // Cena jednostkowa
                            columns.ConstantColumn(55);  // Wartość (zł)
                            columns.ConstantColumn(40);  // Uwagi
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
                            header.Cell().Background(headerColor).Border(0.5f).Padding(2)
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

                            // Previous page total (Z przeniesienia)
                            header.Cell().Background(Colors.White).Border(0.5f).Padding(2)
                                .AlignRight().AlignMiddle().Text(previousPageTotal.ToString("N2")).FontSize(9);
                        });

                        // Data rows
                        int startIndex = pageIndex * itemsPerPage + 1;
                        for (int i = 0; i < itemsPerPage; i++)
                        {
                            var rowColor = Colors.White;
                            
                            if (i < pagePositions.Count)
                            {
                                var position = pagePositions[i];
                                var itemIndex = startIndex + i;

                                table.Cell().Background(rowColor).Border(0.5f).Padding(2)
                                    .AlignCenter().AlignMiddle().Text(itemIndex.ToString()).FontSize(9);

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
                            }
                            else
                            {
                                // Empty rows
                                for (int col = 0; col < 8; col++)
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
                        }

                        // Empty cells before summary
                        table.Cell().Padding(2).Text("").FontSize(7);
                        table.Cell().Padding(2).Text("").FontSize(7);

                        // Summary row
                        table.Cell().ColumnSpan(4).Background(headerColor).Border(0.5f).Padding(2)
                            .AlignCenter().Text("RAZEM").FontSize(9).Bold();

                        table.Cell().Background(headerColor).Border(0.5f).Padding(2)
                            .AlignRight().Text(cumulativeTotal.ToString("N2")).FontSize(9).Bold();
                    });
                }
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
