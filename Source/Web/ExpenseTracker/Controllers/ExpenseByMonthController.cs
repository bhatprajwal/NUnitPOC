using ExpenseTracker.Data;
using ExpenseTracker.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ExpenseTracker.Controllers
{
    public class ExpenseByMonthController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ExpenseByMonthController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            var data = _applicationDbContext
                .Expenses
                .Select(k => new { k.Date.Year, k.Date.Month, k.Amount })
                .GroupBy(x => new { x.Year, x.Month }, (key, group) => new
                {
                    Year = key.Year,
                    Month = key.Month,
                    Total = group.Sum(k => k.Amount)
                })
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
                .ToList();
            
            var list = new List<ExpenseByMonth>();

            foreach (var item in data)
            {
                list.Add(new ExpenseByMonth() { Year = item.Year, Month = DateTimeFormatInfo.CurrentInfo.GetMonthName(item.Month), Total = item.Total });
            }

            return View(nameof(Index), list);
        }
    }
}