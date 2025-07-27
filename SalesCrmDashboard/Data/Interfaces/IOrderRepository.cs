using SalesCrmDashboard.Models;

namespace SalesCrmDashboard.Data.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<SalesOrderHeader>> GetAllOrdersAsync();
        Task<SalesOrderHeader?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<SalesOrderDetail>> GetOrderDetailsAsync(int orderId);
        Task<IEnumerable<RecentOrder>> GetRecentOrdersAsync(int count = 10);
        Task<int> GetTotalOrdersCountAsync();
        Task<decimal> GetTotalRevenueAsync();
    }
}
