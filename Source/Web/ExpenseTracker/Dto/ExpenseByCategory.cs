using ExpenseTracker.Enums;

namespace ExpenseTracker.Dto;

public class ExpenseByCategory
{
    public int Year { get; set; }
    public Category? CategoryId { get; set; }
    public string? Category { get; set; }
    public decimal? Total { get; set; }
}