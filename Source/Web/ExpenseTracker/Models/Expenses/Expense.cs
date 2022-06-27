using ExpenseTracker.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models;

public class Expense
{
    public Expense()
    {
        Id = Guid.NewGuid();
    }

    [Key]
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Category? Category { get; set; }
    public DateTime Date { get; set; }
    public string? Location { get; set; }
    public decimal? Amount { get; set; }
    public string? Note { get; set; }
    public string? CreatedBy { get; set; }
}