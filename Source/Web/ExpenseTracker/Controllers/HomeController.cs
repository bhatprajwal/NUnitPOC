using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpenseTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(nameof(Index));
        }

        public IActionResult Expense()
        {
            return RedirectToAction(nameof(Index), "Expenses");
        }
        
        public IActionResult ExpenseByMonth()
        {
            return RedirectToAction(nameof(Index), "ExpenseByMonth");
        }

        public IActionResult ExpenseByCategory()
        {
            return RedirectToAction(nameof(Index), "ExpenseByCategory");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}