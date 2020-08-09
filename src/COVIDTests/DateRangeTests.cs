using COVIDData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace COVIDTests
{
    [TestClass]
    public class DateRangeTests
    {
        [TestMethod, ExpectedException(typeof(DatesOutOfRangeException))]
        public void Test_Constructor_StartDateHigherThanEndDate_ExpectException()
        {
            //Arrange
            var startDt = DateTime.Now.AddDays(5);
            var endDt = DateTime.Now.AddDays(-5);

            //Act
            new DateRange(startDt, endDt);

            //Assert
            //Expected Exception
        }

        [TestMethod]
        public void Test_TotalDays()
        {
            //Arrange
            var startDt = DateTime.Now.AddDays(-5);
            var endDt = DateTime.Now.AddDays(5);

            var range = new DateRange(startDt, endDt);

            //Act
            var totalDays = range.TotalDays;

            //Assert
            Assert.AreEqual(10, totalDays);
        }

        [TestMethod]
        public void Test_Contains_DateInRange()
        {
            //Arrange
            var startDt = DateTime.Now.AddDays(-5);
            var endDt = DateTime.Now.AddDays(5);

            var range = new DateRange(startDt, endDt);

            //Act
            var contains = range.Contains(DateTime.Now);

            //Assert
            Assert.IsTrue(contains);
            Assert.AreEqual(10.0, range.TotalDays);
        }

        [TestMethod]
        public void Test_Contains_DateInRange_MinValue()
        {
            //Arrange
            var startDt = DateTime.Now.AddDays(-5);
            var endDt = DateTime.Now.AddDays(5);

            var range = new DateRange(startDt, endDt);

            //Act
            var contains = range.Contains(startDt);

            //Assert
            Assert.IsTrue(contains);
        }

        [TestMethod]
        public void Test_Contains_DateInRange_MaxValue()
        {
            //Arrange
            var startDt = DateTime.Now.AddDays(-5);
            var endDt = DateTime.Now.AddDays(5);

            var range = new DateRange(startDt, endDt);

            //Act
            var contains = range.Contains(endDt);

            //Assert
            Assert.IsTrue(contains);
        }

        [TestMethod]
        public void Test_Contains_DateOutOfRange_MaxValue()
        {
            //Arrange
            var startDt = DateTime.Now.AddDays(-5);
            var endDt = DateTime.Now.AddDays(5);

            var range = new DateRange(startDt, endDt);

            //Act
            var contains = range.Contains(endDt.AddDays(1));

            //Assert
            Assert.IsFalse(contains);
        }

        [TestMethod]
        public void Test_Contains_DateOutOfRange_MinValue()
        {
            //Arrange
            var startDt = DateTime.Now.AddDays(-5);
            var endDt = DateTime.Now.AddDays(5);

            var range = new DateRange(startDt, endDt);

            //Act
            var contains = range.Contains(startDt.AddDays(-1));

            //Assert
            Assert.IsFalse(contains);
        }
    }
}
