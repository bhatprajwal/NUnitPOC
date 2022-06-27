using ExpenseTracker.Controllers;
using ExpenseTracker.Data;
using ExpenseTracker.Dto;
using ExpenseTracker.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseTracker.Test.UnitTest;

public class ExpenseByCategoryControllerTests
{
    private ApplicationDbContext? _applicationDbContext;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ExpenseTracker")
            .Options;

        _applicationDbContext = new ApplicationDbContext(options);

        /*
         * Clean and Seed data
         */
        DbHelper.CleanUpDb(_applicationDbContext);
        _applicationDbContext.Expenses.AddRange(ExpenseData.GetAllExpense());
        _applicationDbContext.SaveChanges();
    }

    [Test]
    public void Index_WhenCalled_ReturnIndexViewWithExpensesByCategory()
    {
        // act
        using var controller = new ExpenseByCategoryController(_applicationDbContext);
        var result = controller.Index() as ViewResult;

        // assert
        var modelResult = result?.Model as List<ExpenseByCategory>;

        Assert.That("Index", Is.EqualTo(result.ViewName));
        Assert.IsNotNull(result.Model);
        Assert.That(8, Is.EqualTo(modelResult.Count));

        Assert.That(1465, Is.EqualTo(modelResult.Where(i => i.CategoryId == Category.Grocery).Sum(s => s.Total)));
        Assert.That(670, Is.EqualTo(modelResult.Where(i => i.CategoryId == Category.OutsideFood).Sum(s => s.Total)));

        Assert.That(230, Is.EqualTo(modelResult.Where(i => i.CategoryId == Category.Sanitory).Sum(s => s.Total)));
        Assert.That(889, Is.EqualTo(modelResult.Where(i => i.CategoryId == Category.Cloth).Sum(s => s.Total)));
        Assert.That(349, Is.EqualTo(modelResult.Where(i => i.CategoryId == Category.General).Sum(s => s.Total)));
        Assert.That(99, Is.EqualTo(modelResult.Where(i => i.CategoryId == Category.Travel).Sum(s => s.Total)));
        Assert.That(30, Is.EqualTo(modelResult.Where(i => i.CategoryId == Category.Transport).Sum(s => s.Total)));
        Assert.That(2490, Is.EqualTo(modelResult.Where(i => i.CategoryId == Category.Kitchen).Sum(s => s.Total)));
        
        Assert.That(6222, Is.EqualTo(modelResult.Sum(s => s.Total)));
    }
}