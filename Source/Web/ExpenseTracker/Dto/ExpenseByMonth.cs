namespace ExpenseTracker.Dto;

public class ExpenseByMonth
{
    public int Year { get; set; }
    public string? Month { get; set; }
    public decimal? Total { get; set; }
}