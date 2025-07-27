# 📊 Sales CRM Dashboard

> A comprehensive Sales Customer Relationship Management (CRM) dashboard built with ASP.NET Core MVC and Dapper.NET, connected to Microsoft's AdventureWorksLT2022 sample database.

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![SQL Server](https://img.shields.io/badge/SQL%20Server-AdventureWorksLT2022-red)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.0-purple)
![Dapper](https://img.shields.io/badge/Dapper.NET-2.1.35-orange)
![License](https://img.shields.io/badge/License-MIT-green)

## 🌟 Features

### 📈 Dashboard Analytics
- **Key Performance Metrics**: Total customers, revenue, orders, and products
- **Top 5 Best-Selling Products** with sales data
- **Recent Orders Summary** with customer information
- **Daily Sales Overview** for the last 30 days

### 👥 Customer Management
- **Complete Customer Directory** with search and filtering
- **Customer Details View** with order history
- **Customer Statistics**: Total orders, revenue, and last order date
- **Contact Information Management**

### 📦 Product Catalog
- **Comprehensive Product Listing** with search and sorting
- **Category-based Filtering** and sales performance data
- **Product Details** with pricing and inventory information
- **Sales Analytics** per product

### 🛒 Sales Order Management
- **Order Processing Dashboard** with status tracking
- **Detailed Order Views** with line items
- **Date Range Filtering** and customer-based search
- **Order Status Management** (In Process, Shipped, etc.)

### 🔐 Authentication System
- **Cookie-based Authentication** with session management
- **Admin Dashboard Access** control
- **Secure Login/Logout** functionality

## 🏗️ Architecture

### Clean Layered Architecture
```
📁 Project Structure
├── 🎮 Controllers/          # MVC Controllers
├── 📊 Models/              # Data Models & ViewModels
├── 🗄️ Data/                # Data Access Layer
│   ├── Interfaces/         # Repository Interfaces
│   ├── Repositories/       # Repository Implementations
│   └── DapperContext.cs    # Database Connection Context
├── 🎨 Views/               # Razor Views (Bootstrap 5)
└── 🌐 wwwroot/             # Static Files (CSS, JS)
```

### 🔧 Technology Stack
- **Backend**: ASP.NET Core 8.0 MVC
- **Data Access**: Dapper.NET (Micro-ORM)
- **Database**: SQL Server with AdventureWorksLT2022
- **Frontend**: Bootstrap 5, HTML5, CSS3, JavaScript
- **Authentication**: ASP.NET Core Identity (Cookie-based)
- **Icons**: Bootstrap Icons

### 🎯 Design Patterns
- **Repository Pattern** for data access abstraction
- **Dependency Injection** for loose coupling
- **MVC Pattern** for separation of concerns
- **SOLID Principles** implementation

## 🚀 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (LocalDB, Express, or Full)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server Management Studio](https://docs.microsoft.com/sql/ssms/) (recommended)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/sales-crm-dashboard.git
   cd sales-crm-dashboard
   ```

2. **Download and restore AdventureWorksLT2022 database**
   ```bash
   # Download from Microsoft's GitHub
   # https://github.com/Microsoft/sql-server-samples/releases/tag/adventureworks
   
   # Restore using SQL Server Management Studio or T-SQL:
   RESTORE DATABASE AdventureWorksLT2022
   FROM DISK = 'C:\YourPath\AdventureWorksLT2022.bak'
   WITH REPLACE;
   ```

3. **Install NuGet packages**
   ```bash
   dotnet restore
   ```

4. **Update connection string**
   ```json
   // appsettings.json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AdventureWorksLT2022;Integrated Security=true;TrustServerCertificate=true;"
     }
   }
   ```

5. **Build and run**
   ```bash
   dotnet build
   dotnet run
   ```

6. **Access the application**
   - Navigate to `https://localhost:5001`
   - Login with: **admin** / **admin123**

## 📸 Screenshots

### Dashboard Overview
![Dashboard](screenshots/dashboard.png)

### Customer Management
![Customers](screenshots/customers.png)

### Product Catalog
![Products](screenshots/products.png)

### Order Management
![Orders](screenshots/orders.png)

## 🎯 Key Dapper.NET Implementation Highlights

### Advanced Query Examples
```csharp
// Complex Join with Aggregation
var query = @"
    SELECT p.*, pc.Name as CategoryName, 
           SUM(sod.OrderQty) as QuantitySold,
           SUM(sod.LineTotal) as TotalRevenue
    FROM SalesLT.Product p
    LEFT JOIN SalesLT.ProductCategory pc ON p.ProductCategoryID = pc.ProductCategoryID
    LEFT JOIN SalesLT.SalesOrderDetail sod ON p.ProductID = sod.ProductID
    GROUP BY p.ProductID, p.Name, pc.Name
    ORDER BY TotalRevenue DESC";

var products = await connection.QueryAsync<Product>(query);
```

### Transaction Management
```csharp
using var transaction = connection.BeginTransaction();
try 
{
    await connection.ExecuteAsync(insertQuery, data, transaction);
    await connection.ExecuteAsync(updateQuery, updateData, transaction);
    transaction.Commit();
}
catch 
{
    transaction.Rollback();
    throw;
}
```

### Parameterized Queries
```csharp
var customers = await connection.QueryAsync<Customer>(
    "SELECT * FROM SalesLT.Customer WHERE CustomerID = @Id", 
    new { Id = customerId });
```

## 🔧 Configuration

### Database Connection Strings
```json
// SQL Server LocalDB
"Server=(localdb)\\MSSQLLocalDB;Database=AdventureWorksLT2022;Integrated Security=true;TrustServerCertificate=true;"

// SQL Server Express
"Server=.\\SQLEXPRESS;Database=AdventureWorksLT2022;Integrated Security=true;TrustServerCertificate=true;"

// SQL Server with Authentication
"Server=YourServer;Database=AdventureWorksLT2022;User Id=username;Password=password;TrustServerCertificate=true;"
```

### Authentication Settings
```csharp
// Cookie Configuration
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });
```

## 📊 Database Schema

### Key Tables Used
- **SalesLT.Customer**: Customer information and contact details
- **SalesLT.Product**: Product catalog with pricing and categories
- **SalesLT.ProductCategory**: Product categorization
- **SalesLT.SalesOrderHeader**: Sales order headers with totals
- **SalesLT.SalesOrderDetail**: Order line items and products

## 🎨 UI/UX Features

### Responsive Design
- **Mobile-First Approach** with Bootstrap 5
- **Clean, Professional Interface** with consistent styling
- **Interactive Dashboard** with real-time data
- **Intuitive Navigation** with breadcrumbs and search

### Modern UI Components
- **Data Tables** with sorting and filtering
- **Status Badges** for order management
- **Progress Indicators** and loading states
- **Alert Messages** for user feedback

## 🔐 Security Features

- **Cookie-based Authentication** with secure session management
- **Anti-forgery Token** protection for forms
- **SQL Injection Prevention** through parameterized queries
- **Input Validation** and error handling
- **Secure Connection Strings** with encryption support

## 🚀 Performance Optimizations

- **Dapper.NET** for high-performance data access
- **Asynchronous Operations** throughout the application
- **Efficient Query Design** with proper indexing
- **Connection Pooling** for database connections
- **Minimal Dependencies** for faster load times

## 🧪 Testing

### Sample Data Verification
```sql
-- Verify database setup
SELECT 'Customers' as TableName, COUNT(*) as RecordCount FROM SalesLT.Customer
UNION ALL
SELECT 'Products', COUNT(*) FROM SalesLT.Product
UNION ALL
SELECT 'Orders', COUNT(*) FROM SalesLT.SalesOrderHeader;
```

## 📈 Future Enhancements

- [ ] **Advanced Reporting** with charts and graphs
- [ ] **Email Integration** for customer communications
- [ ] **Inventory Management** module
- [ ] **Sales Forecasting** with AI/ML
- [ ] **API Development** for mobile applications
- [ ] **Real-time Notifications** with SignalR
- [ ] **Advanced Search** with Elasticsearch
- [ ] **Export Capabilities** (PDF, Excel)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Mohamed** - [GitHub Profile](https://github.com/MohamedAshraf144)

## 🙏 Acknowledgments

- Microsoft for the AdventureWorksLT2022 sample database
- Dapper.NET team for the excellent micro-ORM
- Bootstrap team for the responsive CSS framework
- .NET team for the powerful web framework

## 📞 Support

If you have any questions or need help getting started:

- 📧 Email: mohamed.dev321@gmail.com
---

⭐ **Star this repository if you found it helpful!**

Made with ❤️ using ASP.NET Core and Dapper.NET
