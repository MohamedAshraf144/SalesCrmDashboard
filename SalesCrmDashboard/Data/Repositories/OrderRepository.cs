using Dapper;
using SalesCrmDashboard.Data.Interfaces;
using SalesCrmDashboard.Models;

namespace SalesCrmDashboard.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperContext _context;

        public OrderRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalesOrderHeader>> GetAllOrdersAsync()
        {
            var query = @"
                SELECT 
                    soh.SalesOrderID, soh.RevisionNumber, soh.OrderDate, soh.DueDate, 
                    soh.ShipDate, soh.Status, soh.OnlineOrderFlag, soh.SalesOrderNumber,
                    soh.PurchaseOrderNumber, soh.AccountNumber, soh.CustomerID,
                    soh.ShipToAddressID, soh.BillToAddressID, soh.ShipMethod,
                    soh.CreditCardApprovalCode, soh.SubTotal, soh.TaxAmt, 
                    soh.Freight, soh.TotalDue, soh.Comment, soh.rowguid, soh.ModifiedDate,
                    CASE 
                        WHEN c.CompanyName IS NOT NULL THEN c.CompanyName
                        ELSE c.FirstName + ' ' + c.LastName 
                    END as CustomerName,
                    c.CompanyName as CustomerCompany
                FROM SalesLT.SalesOrderHeader soh
                INNER JOIN SalesLT.Customer c ON soh.CustomerID = c.CustomerID
                ORDER BY soh.OrderDate DESC";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<SalesOrderHeader>(query);
        }

        public async Task<SalesOrderHeader?> GetOrderByIdAsync(int orderId)
        {
            var query = @"
                SELECT 
                    soh.SalesOrderID, soh.RevisionNumber, soh.OrderDate, soh.DueDate, 
                    soh.ShipDate, soh.Status, soh.OnlineOrderFlag, soh.SalesOrderNumber,
                    soh.PurchaseOrderNumber, soh.AccountNumber, soh.CustomerID,
                    soh.ShipToAddressID, soh.BillToAddressID, soh.ShipMethod,
                    soh.CreditCardApprovalCode, soh.SubTotal, soh.TaxAmt, 
                    soh.Freight, soh.TotalDue, soh.Comment, soh.rowguid, soh.ModifiedDate,
                    CASE 
                        WHEN c.CompanyName IS NOT NULL THEN c.CompanyName
                        ELSE c.FirstName + ' ' + c.LastName 
                    END as CustomerName
                FROM SalesLT.SalesOrderHeader soh
                INNER JOIN SalesLT.Customer c ON soh.CustomerID = c.CustomerID
                WHERE soh.SalesOrderID = @OrderId";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<SalesOrderHeader>(query, new { OrderId = orderId });
        }

        public async Task<IEnumerable<SalesOrderDetail>> GetOrderDetailsAsync(int orderId)
        {
            var query = @"
                SELECT 
                    sod.SalesOrderID, sod.SalesOrderDetailID, sod.OrderQty, 
                    sod.ProductID, sod.UnitPrice, sod.UnitPriceDiscount, 
                    sod.LineTotal, sod.rowguid, sod.ModifiedDate,
                    p.Name as ProductName, p.ProductNumber
                FROM SalesLT.SalesOrderDetail sod
                INNER JOIN SalesLT.Product p ON sod.ProductID = p.ProductID
                WHERE sod.SalesOrderID = @OrderId
                ORDER BY sod.SalesOrderDetailID";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<SalesOrderDetail>(query, new { OrderId = orderId });
        }

        public async Task<IEnumerable<RecentOrder>> GetRecentOrdersAsync(int count = 10)
        {
            var query = @"
                SELECT TOP (@Count)
                    soh.SalesOrderID,
                    soh.SalesOrderNumber,
                    CASE 
                        WHEN c.CompanyName IS NOT NULL THEN c.CompanyName
                        ELSE c.FirstName + ' ' + c.LastName 
                    END as CustomerName,
                    soh.OrderDate,
                    soh.TotalDue,
                    CASE soh.Status
                        WHEN 1 THEN 'In Process'
                        WHEN 2 THEN 'Approved'
                        WHEN 3 THEN 'Backordered'
                        WHEN 4 THEN 'Rejected'
                        WHEN 5 THEN 'Shipped'
                        WHEN 6 THEN 'Cancelled'
                        ELSE 'Unknown'
                    END as Status
                FROM SalesLT.SalesOrderHeader soh
                INNER JOIN SalesLT.Customer c ON soh.CustomerID = c.CustomerID
                ORDER BY soh.OrderDate DESC";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<RecentOrder>(query, new { Count = count });
        }

        public async Task<int> GetTotalOrdersCountAsync()
        {
            var query = "SELECT COUNT(*) FROM SalesLT.SalesOrderHeader";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query);
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            var query = "SELECT ISNULL(SUM(TotalDue), 0) FROM SalesLT.SalesOrderHeader";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<decimal>(query);
        }
    }
}