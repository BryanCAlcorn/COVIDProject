using COVIDData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace COVIDTests
{
    [TestClass]
    public class DateRangeTests
    {
        [TestMethod]
        public void Test_DateInRange()
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
        public void Test_DateInRange_MinValue()
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
        public void Test_DateInRange_MaxValue()
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
        public void Test_DateOutOfRange_MaxValue()
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
        public void Test_DateOutOfRange_MinValue()
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
