using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Table.PivotTable;
using SkladBlazorApp.Server.Data;
using SkladBlazorApp.Server.Services.Report;

public class ReportService : IReportService
{
    private readonly SkladDbContext _context;

    public ReportService(SkladDbContext context)
    {
        _context = context;
    }

    public async Task<byte[]> GenerateBalanceExcel()
    {
        ExcelPackage.License.SetNonCommercialPersonal("SkladApp");

        var products = await _context.Products
            .Include(p => p.Category)
            .ToListAsync();

        using var package = new ExcelPackage();

        var dataSheet = package.Workbook.Worksheets.Add("Данные");

        dataSheet.Cells[1, 1].Value = "Товар";
        dataSheet.Cells[1, 2].Value = "Категория";
        dataSheet.Cells[1, 3].Value = "Количество";
        dataSheet.Cells[1, 4].Value = "Цена";
        dataSheet.Cells[1, 5].Value = "Стоимость";

        int row = 2;

        foreach (var p in products)
        {
            dataSheet.Cells[row, 1].Value = p.Name;
            dataSheet.Cells[row, 2].Value = p.Category?.Name;
            dataSheet.Cells[row, 3].Value = p.Quantity;
            dataSheet.Cells[row, 4].Value = p.Price;
            dataSheet.Cells[row, 5].Value = p.Quantity * p.Price;
            row++;
        }

        var dataRange = dataSheet.Cells[1, 1, row - 1, 5];

        var table = dataSheet.Tables.Add(dataRange, "ProductsTable");
        table.TableStyle = OfficeOpenXml.Table.TableStyles.Medium2;

        dataSheet.Cells.AutoFitColumns();

        // сводная
        var pivotSheet = package.Workbook.Worksheets.Add("Сводная");

        var pivot = pivotSheet.PivotTables.Add(
            pivotSheet.Cells["A3"],
            dataRange,
            "PivotTable"
        );

        pivot.RowFields.Add(pivot.Fields["Категория"]);
        pivot.DataFields.Add(pivot.Fields["Стоимость"]);
        pivot.DataFields[0].Function = DataFieldFunctions.Sum;

        pivotSheet.Cells["A1"].Value = "Сводная стоимость склада по категориям";
        pivotSheet.Cells["A1"].Style.Font.Bold = true;

        // график
        var chart = pivotSheet.Drawings.AddChart("CategoryChart", eChartType.Pie);

        chart.Title.Text = "Стоимость товаров по категориям";
        chart.SetPosition(5, 0, 0, 0);
        chart.SetSize(600, 400);

        chart.Series.Add(
            pivotSheet.Cells["B4:B20"],
            pivotSheet.Cells["A4:A20"]
        );

        return package.GetAsByteArray();
    }
}