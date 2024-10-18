using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PROG_P1.Models;


namespace PROG_P1_Tests
{
    [TestFixture]
    public class ClaimTests
    {
        [Test]
        public void Claims_InputData_ReturnsCorrectData()
        {

            //Arrange
            var claims = new Claims();
            var hourlyWage = 39;
            var name = "Jack";
            var module = "PROG6222";
            var hoursWorked = 60;


            //Act
            claims.HourlyWage = hourlyWage;
            claims.Name = name;
            claims.Module = module;
            claims.HoursWorked = hoursWorked;

            //Assert 
            Assert.Equals(hourlyWage, claims.HourlyWage);
            Assert.Equals(name, claims.Name);
            Assert.Equals(module, claims.Module);
            Assert.Equals(hoursWorked, claims.HoursWorked);
           

        }

        [Test]
        public void Claims_CalculateTotalEarnings_ReturnsTotalEarnings()
        {
            // Arrange
            var claim = new Claims();
            claim.HourlyWage = 250;
            claim.HoursWorked = 20;


            // Act
            var totalEarnings = claim.HoursWorked * claim.HourlyWage;

            // Assert
            Assert.Equals(5000, totalEarnings);
        }
    }
}
