using Dapper;
using SalesCrmDashboard.Data.Interfaces;
using SalesCrmDashboard.Models;

namespace SalesCrmDashboard.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DapperContext _context;

        public CustomerRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var query = @"
                SELECT 
                    CustomerID, NameStyle, Title, FirstName, MiddleName, LastName, 
                    Suffix, CompanyName, SalesPerson, EmailAddress, Phone, 
                    PasswordHash, PasswordSalt, rowguid, ModifiedDate
                FROM SalesLT.Customer 
                ORDER BY LastName, FirstName";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Customer>(query);
        }

        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            var query = @"
                SELECT 
                    CustomerID, NameStyle, Title, FirstName, MiddleName, LastName, 
                    Suffix, CompanyName, SalesPerson, EmailAddress, Phone, 
                    PasswordHash, PasswordSalt, rowguid, ModifiedDate
                FROM SalesLT.Customer 
                WHERE CustomerID = @CustomerId";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { CustomerId = customerId });
        }

        public async Task<IEnumerable<SalesOrderHeader>> GetCustomerOrdersAsync(int customerId)
        {
            var query = @"
                SELECT 
                    soh.SalesOrderID, soh.RevisionNumber, soh.OrderDate, soh.DueDate, 
                    soh.ShipDate, soh.Status, soh.OnlineOrderFlag, soh.SalesOrderNumber,
                    soh.PurchaseOrderNumber, soh.AccountNumber, soh.CustomerID,
                    soh.ShipToAddressID, soh.BillToAddressID, soh.ShipMethod,
                    soh.CreditCardApprovalCode, soh.SubTotal, soh.TaxAmt, 
                    soh.Freight, soh.TotalDue, soh.Comment, soh.rowguid, soh.ModifiedDate
                FROM SalesLT.SalesOrderHeader soh
                WHERE soh.CustomerID = @CustomerId
                ORDER BY soh.OrderDate DESC";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<SalesOrderHeader>(query, new { CustomerId = customerId });
        }

        public async Task<int> GetTotalCustomersCountAsync()
        {
            var query = "SELECT COUNT(*) FROM SalesLT.Customer";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query);
        }
    }
}
