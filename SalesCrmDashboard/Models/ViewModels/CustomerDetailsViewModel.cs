namespace SalesCrmDashboard.Models.ViewModels
{
    public class CustomerDetailsViewModel
    {
        public Customer Customer { get; set; } = new();
        public List<SalesOrderHeader> Orders { get; set; } = new();
        public decimal TotalOrderValue { get; set; }
        public int TotalOrders { get; set; }
        public DateTime? LastOrderDate { get; set; }
    }
}
