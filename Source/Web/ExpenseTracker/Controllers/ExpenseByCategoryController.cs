using ExpenseTracker.Data;
using ExpenseTracker.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class ExpenseByCategoryController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ExpenseByCategoryController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            var data = _applicationDbContext
                .Expenses
                .Select(k => new { k.Date.Year, k.Category, k.Amount })
                .GroupBy(x => new { x.Year, x.Category }, (key, group) => new
                {
                    Year = key.Year,
                    Category = key.Category,
                    Total = group.Sum(k => k.Amount)
                })
                .OrderByDescending(x => x.Year)
                .ToList();

            var list = new List<ExpenseByCategory>();

            foreach (var item in data)
            {
                list.Add(new ExpenseByCategory() { Year = item.Year, CategoryId =item.Category, Category = item.Category.Value.ToString(), Total = item.Total });
            }

            return View(nameof(Index), list);
        }
    }
}