using Dapper;
using SalesCrmDashboard.Data.Interfaces;
using SalesCrmDashboard.Models;

namespace SalesCrmDashboard.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _context;

        public ProductRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var query = @"
                SELECT 
                    p.ProductID, p.Name, p.ProductNumber, p.Color, p.StandardCost, 
                    p.ListPrice, p.Size, p.Weight, p.ProductCategoryID, p.ProductModelID,
                    p.SellStartDate, p.SellEndDate, p.DiscontinuedDate, 
                    p.ThumbnailPhotoFileName, p.rowguid, p.ModifiedDate,
                    pc.Name as CategoryName,
                    ISNULL(SUM(sod.OrderQty), 0) as QuantitySold,
                    ISNULL(SUM(sod.LineTotal), 0) as TotalRevenue
                FROM SalesLT.Product p
                LEFT JOIN SalesLT.ProductCategory pc ON p.ProductCategoryID = pc.ProductCategoryID
                LEFT JOIN SalesLT.SalesOrderDetail sod ON p.ProductID = sod.ProductID
                GROUP BY 
                    p.ProductID, p.Name, p.ProductNumber, p.Color, p.StandardCost, 
                    p.ListPrice, p.Size, p.Weight, p.ProductCategoryID, p.ProductModelID,
                    p.SellStartDate, p.SellEndDate, p.DiscontinuedDate, 
                    p.ThumbnailPhotoFileName, p.rowguid, p.ModifiedDate, pc.Name
                ORDER BY p.Name";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product>(query);
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var query = @"
                SELECT 
                    p.ProductID, p.Name, p.ProductNumber, p.Color, p.StandardCost, 
                    p.ListPrice, p.Size, p.Weight, p.ProductCategoryID, p.ProductModelID,
                    p.SellStartDate, p.SellEndDate, p.DiscontinuedDate, 
                    p.ThumbnailPhotoFileName, p.rowguid, p.ModifiedDate,
                    pc.Name as CategoryName
                FROM SalesLT.Product p
                LEFT JOIN SalesLT.ProductCategory pc ON p.ProductCategoryID = pc.ProductCategoryID
                WHERE p.ProductID = @ProductId";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Product>(query, new { ProductId = productId });
        }

        public async Task<IEnumerable<TopSellingProduct>> GetTopSellingProductsAsync(int count = 5)
        {
            var query = @"
                SELECT TOP (@Count)
                    p.ProductID,
                    p.Name as ProductName,
                    SUM(sod.OrderQty) as TotalQuantitySold,
                    SUM(sod.LineTotal) as TotalRevenue
                FROM SalesLT.Product p
                INNER JOIN SalesLT.SalesOrderDetail sod ON p.ProductID = sod.ProductID
                GROUP BY p.ProductID, p.Name
                ORDER BY SUM(sod.LineTotal) DESC";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<TopSellingProduct>(query, new { Count = count });
        }

        public async Task<int> GetTotalProductsCountAsync()
        {
            var query = "SELECT COUNT(*) FROM SalesLT.Product";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query);
        }
    }
}
