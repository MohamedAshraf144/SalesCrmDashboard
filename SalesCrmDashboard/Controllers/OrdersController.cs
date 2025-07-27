using Microsoft.AspNetCore.Mvc;
using SalesCrmDashboard.Data.Interfaces;

namespace SalesCrmDashboard.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderRepository orderRepository, ILogger<OrdersController> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var orders = await _orderRepository.GetAllOrdersAsync();

                // Apply search filter
                if (!string.IsNullOrEmpty(searchString))
                {
                    orders = orders.Where(o =>
                        o.SalesOrderNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        (o.CustomerName != null && o.CustomerName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    );
                }

                // Apply date filter
                if (fromDate.HasValue)
                {
                    orders = orders.Where(o => o.OrderDate >= fromDate.Value);
                }
                if (toDate.HasValue)
                {
                    orders = orders.Where(o => o.OrderDate <= toDate.Value);
                }

                // Apply sorting
                orders = sortOrder switch
                {
                    "order_number" => orders.OrderBy(o => o.SalesOrderNumber),
                    "order_number_desc" => orders.OrderByDescending(o => o.SalesOrderNumber),
                    "customer" => orders.OrderBy(o => o.CustomerName),
                    "customer_desc" => orders.OrderByDescending(o => o.CustomerName),
                    "date" => orders.OrderBy(o => o.OrderDate),
                    "date_desc" => orders.OrderByDescending(o => o.OrderDate),
                    "total" => orders.OrderBy(o => o.TotalDue),
                    "total_desc" => orders.OrderByDescending(o => o.TotalDue),
                    _ => orders.OrderByDescending(o => o.OrderDate)
                };

                ViewBag.OrderNumberSortParm = sortOrder == "order_number" ? "order_number_desc" : "order_number";
                ViewBag.CustomerSortParm = sortOrder == "customer" ? "customer_desc" : "customer";
                ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
                ViewBag.TotalSortParm = sortOrder == "total" ? "total_desc" : "total";
                ViewBag.CurrentFilter = searchString;
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;

                return View(orders.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading orders");
                ViewBag.Error = "Unable to load orders data.";
                return View(new List<Models.SalesOrderHeader>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                var orderDetails = await _orderRepository.GetOrderDetailsAsync(id);
                order.OrderDetails = orderDetails.ToList();

                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order details for ID: {OrderId}", id);
                return NotFound();
            }
        }
    }
}