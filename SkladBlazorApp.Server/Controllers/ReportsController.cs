using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Table.PivotTable;
using SkladBlazorApp.Server.Data;
using SkladBlazorApp.Server.Services.Report;
using System.ComponentModel;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("balance-excel")]
    public async Task<IActionResult> ExportBalanceExcel()
    {
        var bytes = await _reportService.GenerateBalanceExcel();

        return File(
            bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Баланс_склада_{DateTime.Now:yyyy-MM-dd}.xlsx"
        );
    }
}