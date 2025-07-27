using Microsoft.AspNetCore.Mvc;
using SalesCrmDashboard.Data.Interfaces;
using SalesCrmDashboard.Models;
using System.Diagnostics;

namespace SalesCrmDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IDashboardRepository dashboardRepository, ILogger<HomeController> logger)
        {
            _dashboardRepository = dashboardRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dashboardData = await _dashboardRepository.GetDashboardDataAsync();
                return View(dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");
                var emptyDashboard = new DashboardViewModel();
                ViewBag.Error = "Unable to load dashboard data. Please check your database connection.";
                return View(emptyDashboard);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
