using ExpenseTracker.Controllers;
using ExpenseTracker.Data;
using ExpenseTracker.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseTracker.Test.UnitTest;

public class ExpenseByMonthControllerTests
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
    public void Index_WhenCalled_ReturnIndexViewWithMonthlyExpenceRecord()
    {
        // act
        using var controller = new ExpenseByMonthController(_applicationDbContext);
        var result = controller.Index() as ViewResult;

        // assert
        var modelResult = result?.Model as List<ExpenseByMonth>;

        Assert.That("Index", Is.EqualTo(result.ViewName));
        Assert.IsNotNull(result.Model);
        Assert.That(7, Is.EqualTo(modelResult.Count));

        Assert.That(1946, Is.EqualTo(modelResult.Where(i => i.Year == 2022 && i.Month == "January").Sum(s => s.Total)));
        Assert.That(219, Is.EqualTo(modelResult.Where(i => i.Year == 2022 && i.Month == "June").Sum(s => s.Total)));

        Assert.That(230, Is.EqualTo(modelResult.Where(i => i.Year == 2022 && i.Month == "February").Sum(s => s.Total)));
        Assert.That(889, Is.EqualTo(modelResult.Where(i => i.Year == 2022 && i.Month == "March").Sum(s => s.Total)));
        Assert.That(99, Is.EqualTo(modelResult.Where(i => i.Year == 2022 && i.Month == "April").Sum(s => s.Total)));
        Assert.That(349, Is.EqualTo(modelResult.Where(i => i.Year == 2022 && i.Month == "May").Sum(s => s.Total)));
        Assert.That(2490, Is.EqualTo(modelResult.Where(i => i.Year == 2021 && i.Month == "June").Sum(s => s.Total)));
        
        Assert.That(6222, Is.EqualTo(modelResult.Sum(s => s.Total)));
    }
}