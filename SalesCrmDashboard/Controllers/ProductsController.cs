using Microsoft.AspNetCore.Mvc;
using SalesCrmDashboard.Data.Interfaces;

namespace SalesCrmDashboard.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync();

                // Apply search filter
                if (!string.IsNullOrEmpty(searchString))
                {
                    products = products.Where(p =>
                        p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        p.ProductNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        (p.CategoryName != null && p.CategoryName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    );
                }

                // Apply sorting
                products = sortOrder switch
                {
                    "name_desc" => products.OrderByDescending(p => p.Name),
                    "price" => products.OrderBy(p => p.ListPrice),
                    "price_desc" => products.OrderByDescending(p => p.ListPrice),
                    "category" => products.OrderBy(p => p.CategoryName),
                    "category_desc" => products.OrderByDescending(p => p.CategoryName),
                    "sales" => products.OrderBy(p => p.QuantitySold ?? 0),
                    "sales_desc" => products.OrderByDescending(p => p.QuantitySold ?? 0),
                    _ => products.OrderBy(p => p.Name)
                };

                ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";
                ViewBag.CategorySortParm = sortOrder == "category" ? "category_desc" : "category";
                ViewBag.SalesSortParm = sortOrder == "sales" ? "sales_desc" : "sales";
                ViewBag.CurrentFilter = searchString;

                return View(products.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading products");
                ViewBag.Error = "Unable to load products data.";
                return View(new List<Models.Product>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product details for ID: {ProductId}", id);
                return NotFound();
            }
        }
    }
}