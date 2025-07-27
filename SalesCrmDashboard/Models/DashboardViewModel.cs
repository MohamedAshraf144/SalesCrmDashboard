namespace SalesCrmDashboard.Models
{
    public class DashboardViewModel
    {
        public int TotalCustomers { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public List<TopSellingProduct> TopProducts { get; set; } = new();
        public List<DailyOrderSummary> DailyOrderSummary { get; set; } = new();
        public List<RecentOrder> RecentOrders { get; set; } = new();
    }
    public class TopSellingProduct
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class DailyOrderSummary
    {
        public DateTime OrderDate { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class RecentOrder
    {
        public int SalesOrderID { get; set; }
        public string SalesOrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalDue { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
