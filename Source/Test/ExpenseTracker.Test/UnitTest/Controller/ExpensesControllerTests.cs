using ExpenseTracker.Controllers;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExpenseTracker.Test.UnitTest;

public  class ExpensesControllerTests
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
    public async Task Index_WhenCalled_ReturnAllExpensesList()
    {
        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Index() as ViewResult;

        // assert
        List<Expense>? expenseList = result?.Model as List<Expense>;

        Assert.IsNotNull(result);
        Assert.That("Index", Is.EqualTo(result?.ViewName));
        Assert.That(10, Is.EqualTo(expenseList?.Count));
    }

    [Test]
    public async Task Index_WhenCalledWithEmptyExpense_ReturnProblemDetails()
    {
        // arrange
        _applicationDbContext.Expenses = null;

        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Index() as ObjectResult;

        // assert
        ProblemDetails? details = result?.Value as ProblemDetails;

        Assert.IsNotNull(result);
        Assert.That(500, Is.EqualTo(details?.Status));
        Assert.That("Entity set 'ApplicationDbContext.Expenses'  is null.", Is.EqualTo(details.Detail));
    }

    [Test]
    public async Task Details_WhenCalledWithNullExpense_ReturnNotFoundStatus()
    {
        // arrange
        _applicationDbContext.Expenses = null;

        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Details(null) as NotFoundResult;

        // assert
        Assert.That((int)HttpStatusCode.NotFound, Is.EqualTo(result.StatusCode));
    }

    [Test]
    public async Task Details_WhenCalledWithNullExpenseId_ReturnNotFoundStatus()
    {
        // arrange
        _applicationDbContext.Expenses = null;

        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Details(Guid.Parse("0f64f2cf-2d7b-40c3-be58-3e6fcb9f4639")) as NotFoundResult;

        // assert
        Assert.That((int)HttpStatusCode.NotFound, Is.EqualTo(result.StatusCode));
    }

    [Test]
    public async Task Details_WhenCalledWithInvalidExpenseId_ReturnNotFoundStatus()
    {
        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Details(Guid.Parse("be34db63-0db1-4b64-9e9e-f5f926bf0567")) as NotFoundResult;

        // assert
        Assert.That((int)HttpStatusCode.NotFound, Is.EqualTo(result.StatusCode));
    }

    [Test]
    public async Task Details_WhenCalledWithValidExpenseId_ReturnExpenseRecord()
    {
        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Details(Guid.Parse("0f64f2cf-2d7b-40c3-be58-3e6fcb9f4639")) as ViewResult;

        // assert
        var expense = result?.Model as Expense;

        Assert.That("Details", Is.EqualTo(result.ViewName));
        Assert.NotNull(result.Model);
        Assert.That(Guid.Parse("0f64f2cf-2d7b-40c3-be58-3e6fcb9f4639"), Is.EqualTo(expense?.Id));
        Assert.That(typeof(Expense), Is.EqualTo(result?.Model.GetType()));
    }

    [Test]
    public async Task Create_WhenCalled_ReturnCreateView()
    {
        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Create() as ViewResult;

        // assert
        Assert.That("Create", Is.EqualTo(result.ViewName));
    }

    [Test]
    public async Task Create_WhenExpenseProvided_SaveRecordAndRedirectToIndexView()
    {
        // arrange
        var id = Guid.NewGuid();

        Expense expense = new Expense()
        {
            Id = id,
            Name = "Clothing",
            Description = "Festival Cloth Shopping",
            Category = Enums.Category.Cloth,
            Amount = 2000,
            Date = DateTime.Now,
            Location = "Bengaluru",
            Note = "Shopping at Central JP Nagar"
        };

        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Create(expense) as RedirectToActionResult;

        // assert
        Assert.That("Index", Is.EqualTo(result.ActionName));
        Assert.That(11, Is.EqualTo(await _applicationDbContext.Expenses.CountAsync()));
        Assert.That(await _applicationDbContext.Expenses.AnyAsync(i => i.Id == id));
        Assert.That(2000, Is.EqualTo(_applicationDbContext.Expenses.FirstOrDefault(i => i.Id == id).Amount));
    }

    [Test]
    [Ignore(NUnitConstants.NotReady)]
    public async Task Create_WhenNoExpensesProvided_ReturnNullToCreateView()
    {
        throw new NotImplementedException();
    }

    [Test]
    public async Task Edit_WhenCalledWithNullExpense_ReturnNotFoundStatus()
    {
        // arrange
        _applicationDbContext.Expenses = null;

        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Edit(Guid.Parse("0f64f2cf-2d7b-40c3-be58-3e6fcb9f4639")) as NotFoundResult;

        // assert
        Assert.That((int)HttpStatusCode.NotFound, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task Edit_WhenCalledWithInvalidExpenseId_ReturnNotFoundStatus()
    {
        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Edit(Guid.Parse("be34db63-0db1-4b64-9e9e-f5f926bf0567")) as NotFoundResult;

        // assert
        Assert.That((int)HttpStatusCode.NotFound, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task Edit_WhenCalledWithValidExpenseId_ReturnExpenseDetailsWithEditView()
    {
        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Edit(Guid.Parse("0f64f2cf-2d7b-40c3-be58-3e6fcb9f4639")) as ViewResult;

        // assert
        var expense = result?.Model as Expense;

        Assert.That("Edit", Is.EqualTo(result.ViewName));
        Assert.NotNull(result.Model);
        Assert.That(Guid.Parse("0f64f2cf-2d7b-40c3-be58-3e6fcb9f4639"), Is.EqualTo(expense?.Id));
        Assert.That(typeof(Expense), Is.EqualTo(result?.Model.GetType()));
    }

    [Test]
    public async Task Edit_WhenInvalidExpenseIdPassedToEdit_ReturnNotFoundStatus()
    {
        // arrange
        var id = Guid.Parse("be34db63-0db1-4b64-9e9e-f5f926bf0567");

        var expense = _applicationDbContext.Expenses.Where(x => x.Id == Guid.Parse("0f64f2cf-2d7b-40c3-be58-3e6fcb9f4639")).FirstOrDefault();

        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Edit(id, expense) as NotFoundResult;

        // assert
        Assert.That((int)HttpStatusCode.NotFound, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task Edit_WhenValidExpenseRecord_UpdateTheExpenseAndRedirectToIndexView()
    {
        // arrange
        var id = Guid.Parse("0f64f2cf-2d7b-40c3-be58-3e6fcb9f4639");

        var expense = _applicationDbContext.Expenses.Where(x => x.Id == id).FirstOrDefault();
        expense.Amount = 2000;

        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Edit(id, expense) as RedirectToActionResult;

        // assert
        var exitedExpense = _applicationDbContext.Expenses.Where(x => x.Id == id).FirstOrDefault();
        
        Assert.That("Index", Is.EqualTo(result.ActionName));
        
        Assert.That(Guid.Parse("0f64f2cf-2d7b-40c3-be58-3e6fcb9f4639"), Is.EqualTo(exitedExpense?.Id));
        Assert.That(2000, Is.EqualTo(exitedExpense?.Amount));
    }

    [Test]
    public async Task Delete_WhenCalledWithNullExpenseId_ReturnNotFoundStatus()
    {
        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Delete(null) as NotFoundResult;

        // assert
        Assert.That((int)HttpStatusCode.NotFound, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task Delete_WhenCalledWithInvalidExpenseId_ReturnNotFoundStatus()
    {
        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Delete(Guid.Parse("be34db63-0db1-4b64-9e9e-f5f926bf0567")) as NotFoundResult;

        // assert
        Assert.That((int)HttpStatusCode.NotFound, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task Delete_WhenCalledWithValidExpenseId_ReturnExpenseDetailsWithDeleteView()
    {
        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.Delete(Guid.Parse("0f64f2cf-2d7b-40c3-be58-3e6fcb9f4639")) as ViewResult;

        // assert
        var expense = result?.Model as Expense;

        Assert.That("Delete", Is.EqualTo(result.ViewName));
        Assert.NotNull(result.Model);
        Assert.That(Guid.Parse("0f64f2cf-2d7b-40c3-be58-3e6fcb9f4639"), Is.EqualTo(expense?.Id));
        Assert.That(typeof(Expense), Is.EqualTo(result?.Model.GetType()));
    }



    [Test]
    public async Task Delete_WhenNoExpenseFoundToDelete_ReturnPoblemDetails()
    {
        // arrange
        _applicationDbContext.Expenses = null;

        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.DeleteConfirmed(Guid.Parse("0f64f2cf-2d7b-40c3-be58-3e6fcb9f4639")) as ObjectResult;

        // assert
        ProblemDetails? details = result?.Value as ProblemDetails;

        Assert.IsNotNull(result);
        Assert.That(500, Is.EqualTo(result?.StatusCode));
        Assert.That(500, Is.EqualTo(details?.Status));
        Assert.That("Entity set 'ApplicationDbContext.Expenses'  is null.", Is.EqualTo(details.Detail));
    }

    [Test]
    public async Task Delete_WhenValidExpenseRecord_RemoveTheExpenseRecordAndRedirectToIndexView()
    {
        // arrange
        var id = Guid.Parse("0f64f2cf-2d7b-40c3-be58-3e6fcb9f4639");

        var expense = _applicationDbContext.Expenses.Where(x => x.Id == id).FirstOrDefault();
        
        // act
        var controller = new ExpensesController(_applicationDbContext);

        var result = await controller.DeleteConfirmed(id) as RedirectToActionResult;

        // assert
        var exitedExpense = _applicationDbContext.Expenses.Where(x => x.Id == id).FirstOrDefault();

        Assert.That("Index", Is.EqualTo(result.ActionName));

        Assert.IsNull(exitedExpense);
        Assert.That(9, Is.EqualTo(_applicationDbContext.Expenses.Count()));
    }
}