using Dapper;
using SalesCrmDashboard.Data.Interfaces;
using SalesCrmDashboard.Models;

namespace SalesCrmDashboard.Data.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DapperContext _context;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public DashboardRepository(
            DapperContext context,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository)
        {
            _context = context;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var dashboard = new DashboardViewModel
            {
                TotalCustomers = await _customerRepository.GetTotalCustomersCountAsync(),
                TotalProducts = await _productRepository.GetTotalProductsCountAsync(),
                TotalOrders = await _orderRepository.GetTotalOrdersCountAsync(),
                TotalRevenue = await _orderRepository.GetTotalRevenueAsync(),
                TopProducts = (await _productRepository.GetTopSellingProductsAsync(5)).ToList(),
                RecentOrders = (await _orderRepository.GetRecentOrdersAsync(10)).ToList(),
                DailyOrderSummary = (await GetDailyOrderSummaryAsync(30)).ToList()
            };

            return dashboard;
        }

        public async Task<IEnumerable<DailyOrderSummary>> GetDailyOrderSummaryAsync(int days = 30)
        {
            var query = @"
                SELECT 
                    CAST(OrderDate AS DATE) as OrderDate,
                    COUNT(*) as OrderCount,
                    SUM(TotalDue) as TotalAmount
                FROM SalesLT.SalesOrderHeader
                WHERE OrderDate >= DATEADD(DAY, -@Days, GETDATE())
                GROUP BY CAST(OrderDate AS DATE)
                ORDER BY OrderDate DESC";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<DailyOrderSummary>(query, new { Days = days });
        }
    }
}