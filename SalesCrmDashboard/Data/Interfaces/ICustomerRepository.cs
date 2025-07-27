using SalesCrmDashboard.Models;

namespace SalesCrmDashboard.Data.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task<IEnumerable<SalesOrderHeader>> GetCustomerOrdersAsync(int customerId);
        Task<int> GetTotalCustomersCountAsync();
    }
}
