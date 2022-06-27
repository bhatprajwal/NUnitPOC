using ExpenseTracker.Models;
using System;
using System.Collections.Generic;

namespace ExpenseTracker.Test.UnitTest;

public class ExpenseData
{
    public static List<Expense> GetAllExpense()
    {
        return new List<Expense>()
        {
            new Expense { Id = GetGuid("0f64f2cf-2d7b-40c3-be58-3e6fcb9f4639"), Name = "Grocery", Description = "Grocery purchase for Festival - Sankranti", Category = Enums.Category.Grocery, Amount = 345, Date = DateTime.Parse("2022-01-14"), Location = "Bengaluru", Note = "A1 Grocery Shop"},
            new Expense { Id = GetGuid("915ab029-f447-4489-8744-7aecaee3453d"), Name = "Pizza", Description = "Dominos", Category = Enums.Category.OutsideFood, Amount = 481, Date = DateTime.Parse("2022-01-30"), Location = "Bengaluru", Note = "Pizza"},
            new Expense { Id = GetGuid("b644c861-2011-4427-a595-54fb7a0b27f5"), Name = "Kitchen Tap", Description = "Steel water tap", Category = Enums.Category.Sanitory, Amount = 230, Date = DateTime.Parse("2022-02-15"), Location = "Bengaluru", Note = "Water Tap"},
            new Expense { Id = GetGuid("b09b32c2-9c8a-413f-aa4e-f885841b4b31"), Name = "Grocery", Description = "Monthly Grocery", Category = Enums.Category.Grocery, Amount = 1120, Date = DateTime.Parse("2022-01-31"), Location = "Bengaluru", Note = "Monthly Grocery"},
            new Expense { Id = GetGuid("91522870-334f-40b0-b835-f2a5fb2f3ed6"), Name = "Daily Wear", Description = "Daily Wear", Category = Enums.Category.Cloth, Amount = 889, Date = DateTime.Parse("2022-03-14"), Location = "Bengaluru", Note = "Daily Wear"},
            new Expense { Id = GetGuid("8cea82ce-9edf-4a0f-a585-aaf3b9faab09"), Name = "Light lamp", Description = "Light lamp", Category = Enums.Category.General, Amount = 349, Date = DateTime.Parse("2022-05-14"), Location = "Bengaluru", Note = "Light lamp"},
            new Expense { Id = GetGuid("73da9a12-163d-46b4-9e7c-1180349748ef"), Name = "Burger", Description = "Burger", Category = Enums.Category.OutsideFood, Amount = 189, Date = DateTime.Parse("2022-06-14"), Location = "Bengaluru", Note = "Burger"},
            new Expense { Id = GetGuid("4a38322a-44ea-45e4-9d79-3f75dc495f03"), Name = "Auto", Description = "Auto", Category = Enums.Category.Travel, Amount = 99, Date = DateTime.Parse("2022-04-14"), Location = "Bengaluru", Note = "Auto"},
            new Expense { Id = GetGuid("9ee05bb0-97b1-494d-9698-129361287bc8"), Name = "Dunzo", Description = "Dunzo", Category = Enums.Category.Transport, Amount = 30, Date = DateTime.Parse("2022-06-23"), Location = "Bengaluru", Note = "Dunzo"},
            new Expense { Id = GetGuid("bef867c2-5026-4a94-95f0-69155adeb3b5"), Name = "Stove", Description = "Glasstop Stove", Category = Enums.Category.Kitchen, Amount = 2490, Date = DateTime.Parse("2021-06-23"), Location = "Bengaluru", Note = "Glasstop Stove"},
        };
    }

    private static Guid GetGuid(string guid)
    {
        return !String.IsNullOrEmpty(guid) ? Guid.Parse(guid) : Guid.NewGuid();
    }
}