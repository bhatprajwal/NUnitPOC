using ExpenseTracker.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ExpenseTracker.Test.UnitTest
{
    public class HomeControllerTest
    {
        private ILogger<HomeController>? _logger;

        [SetUp]
        public void Setup() 
        {
            // arrange
            _logger = Mock.Of<ILogger<HomeController>>();
        }

        [Test]
        public void Index_WhenCalled_ThenReturnIndexView()
        {
            // act
            var controller = new HomeController(_logger);

            var result = controller.Index() as ViewResult;

            // assert
            Assert.That("Index", Is.EqualTo(result?.ViewName));
        }

        [Test]
        public void Expense_WhenCalled_ThenRedirectToIndexViewOfExpensesController()
        {
            // act
            var controller = new HomeController(_logger);

            var result = controller.Expense() as RedirectToActionResult;

            // assert
            Assert.That("Index", Is.EqualTo(result?.ActionName));
            Assert.That("Expenses", Is.EqualTo(result?.ControllerName));
        }

        [Test]
        public void ExpenseByMonth_WhenCalled_ThenRedirectToIndexViewOfExpenseByMonthController()
        {            
            // act
            var controller = new HomeController(_logger);

            var result = controller.ExpenseByMonth() as RedirectToActionResult;

            // assert
            Assert.That("Index", Is.EqualTo(result?.ActionName));
            Assert.That("ExpenseByMonth", Is.EqualTo(result?.ControllerName));
        }

        [Test]
        public void ExpenseByCategory_WhenCalled_ThenRedirectToIndexViewOfExpenseByCategoryController()
        {
            // act
            var controller = new HomeController(_logger);

            var result = controller.ExpenseByCategory() as RedirectToActionResult;

            // assert
            Assert.That("Index", Is.EqualTo(result?.ActionName));
            Assert.That("ExpenseByCategory", Is.EqualTo(result?.ControllerName));
        }
    }
}