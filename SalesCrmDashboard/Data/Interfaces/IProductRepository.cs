using SalesCrmDashboard.Models;

namespace SalesCrmDashboard.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int productId);
        Task<IEnumerable<TopSellingProduct>> GetTopSellingProductsAsync(int count = 5);
        Task<int> GetTotalProductsCountAsync();
    }
}
