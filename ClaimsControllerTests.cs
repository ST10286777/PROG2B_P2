using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PROG_P1.Controllers;
using PROG_P1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG_P1_Tests
{
    [TestFixture]
    public class ClaimsControllerTests
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private ClaimsController _controller;

        [SetUp]
        public void Setup()
        {
            // Initialize the controller before each test
            _controller = new ClaimsController( _context, _userManager);

        }

        [Test]
        public void Index_Returns_ViewResult()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);  // Ensure that the result is not null
            Assert.True(result.ViewName == "" || result.ViewName == null);  // Check for implicit view name
        
        }

        [Test]
        public void Details_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = new ClaimsController(_context, _userManager);

            // Act
            var result = controller.Details(-1) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);  // Ensures that an invalid ID returns NotFound
        }


        [Test]
        public void Details_ValidId_ReturnsViewResult()
        {
            // Arrange
            var controller = new ClaimsController(_context, _userManager);

            // Act
            var result = controller.Details(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ViewName);  // Ensures it returns the correct view for valid ID
        }

    }
}
