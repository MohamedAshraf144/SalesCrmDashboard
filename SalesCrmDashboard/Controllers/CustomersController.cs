using Microsoft.AspNetCore.Mvc;
using SalesCrmDashboard.Data.Interfaces;
using SalesCrmDashboard.Models.ViewModels;

namespace SalesCrmDashboard.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerRepository customerRepository, ILogger<CustomersController> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var customers = await _customerRepository.GetAllCustomersAsync();
                return View(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading customers");
                ViewBag.Error = "Unable to load customers data.";
                return View(new List<Models.Customer>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                var orders = await _customerRepository.GetCustomerOrdersAsync(id);
                var viewModel = new CustomerDetailsViewModel
                {
                    Customer = customer,
                    Orders = orders.ToList(),
                    TotalOrderValue = orders.Sum(o => o.TotalDue),
                    TotalOrders = orders.Count(),
                    LastOrderDate = orders.Any() ? orders.Max(o => o.OrderDate) : null
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading customer details for ID: {CustomerId}", id);
                return NotFound();
            }
        }
    }
}