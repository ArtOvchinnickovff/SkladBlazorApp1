namespace SkladBlazorApp.Server.Services.Report
{
    public interface IReportService
    {
        Task<byte[]> GenerateBalanceExcel();
    }
}
