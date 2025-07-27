using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SalesCrmDashboard.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            try
            {
                _logger.LogInformation("Login attempt for username: {Username}", username);

                // Trim whitespace and validate input
                username = username?.Trim() ?? "";
                password = password?.Trim() ?? "";

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ViewBag.Error = "Please enter both username and password";
                    ViewBag.ReturnUrl = returnUrl;
                    return View();
                }

                // Simple hardcoded authentication - replace with your authentication logic
                // Check multiple variations to be more flexible
                bool isValidUser = (username.Equals("admin", StringComparison.OrdinalIgnoreCase) &&
                                   password == "admin123") ||
                                  (username.Equals("administrator", StringComparison.OrdinalIgnoreCase) &&
                                   password == "admin123") ||
                                  (username.Equals("demo", StringComparison.OrdinalIgnoreCase) &&
                                   password == "demo123");

                if (isValidUser)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim("DisplayName", username)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                        AllowRefresh = true
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    _logger.LogInformation("Successful login for user: {Username}", username);

                    // Redirect logic
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogWarning("Failed login attempt for username: {Username}", username);
                    ViewBag.Error = "Invalid username or password. Please try: admin / admin123";
                    ViewBag.ReturnUrl = returnUrl;
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login process");
                ViewBag.Error = "An error occurred during login. Please try again.";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                _logger.LogInformation("User logged out successfully");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}