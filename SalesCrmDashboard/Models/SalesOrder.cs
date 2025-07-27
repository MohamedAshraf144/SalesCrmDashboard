namespace SalesCrmDashboard.Models
{
    public class SalesOrderHeader
    {
        public int SalesOrderID { get; set; }
        public int RevisionNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public byte Status { get; set; }
        public bool OnlineOrderFlag { get; set; }
        public string SalesOrderNumber { get; set; } = string.Empty;
        public string? PurchaseOrderNumber { get; set; }
        public string? AccountNumber { get; set; }
        public int CustomerID { get; set; }
        public int? ShipToAddressID { get; set; }
        public int? BillToAddressID { get; set; }
        public string ShipMethod { get; set; } = string.Empty;
        public string? CreditCardApprovalCode { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal Freight { get; set; }
        public decimal TotalDue { get; set; }
        public string? Comment { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public string? CustomerName { get; set; }
        public string? CustomerCompany { get; set; }
        public List<SalesOrderDetail> OrderDetails { get; set; } = new();
    }

    public class SalesOrderDetail
    {
        public int SalesOrderID { get; set; }
        public int SalesOrderDetailID { get; set; }
        public short OrderQty { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceDiscount { get; set; }
        public decimal LineTotal { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public string? ProductName { get; set; }
        public string? ProductNumber { get; set; }
    }
}
