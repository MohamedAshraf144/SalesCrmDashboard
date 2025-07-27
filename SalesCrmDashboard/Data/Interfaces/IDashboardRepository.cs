using SalesCrmDashboard.Models;

namespace SalesCrmDashboard.Data.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
        Task<IEnumerable<DailyOrderSummary>> GetDailyOrderSummaryAsync(int days = 30);
    }
}
