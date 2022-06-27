using ExpenseTracker.Data;
using System.Linq;

namespace ExpenseTracker.Test.UnitTest;

public class DbHelper
{
    public static void CleanUpDb(ApplicationDbContext _context)
    {
		var expense = _context.Expenses?.ToList();
		if (expense.Any())
		{
			_context.RemoveRange(expense);
			_context.SaveChanges();
		}
	}
}