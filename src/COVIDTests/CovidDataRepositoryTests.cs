using COVIDData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COVIDTests
{
    [TestClass]
    public class CovidDataRepositoryTests
    {
        [TestMethod]
        public void Test_Repository_QueryByCounty_InRange()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

            //Act
            var result = repository.QueryByCounty("Alameda", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual("Alameda", result.Location, nameof(result.Location));
            Assert.AreEqual("1.234", result.Latitude, nameof(result.Latitude));
            Assert.AreEqual("5.678", result.Longitude, nameof(result.Longitude));
            Assert.AreEqual(1, result.MinimumCaseCount, nameof(result.MinimumCaseCount));
            Assert.AreEqual(DateTime.Now.AddDays(-1).Date, result.MinimumCaseDate, nameof(result.MinimumCaseDate));
            Assert.AreEqual(3, result.MaximumCaseCount, nameof(result.MaximumCaseCount));
            Assert.AreEqual(DateTime.Now.AddDays(1).Date, result.MaximumCaseDate, nameof(result.MaximumCaseDate));
            Assert.AreEqual(1.0, result.AverageDailyCases, nameof(result.AverageDailyCases));
        }

        [TestMethod, ExpectedException(typeof(DataNotFoundException))]
        public void Test_Repository_QueryByCounty_CountyDoesntExist()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

            //Act
            var result = repository.QueryByCounty("Not A County", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            //Expect Exception
        }

        [TestMethod]
        public void Test_Repository_QueryByCounty_OutOfRange()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(DateTime.Now.AddDays(-3), DateTime.Now.AddDays(3));

            //Act
            var result = repository.QueryByCounty("Alameda", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual("Alameda", result.Location, nameof(result.Location));
            Assert.AreEqual("1.234", result.Latitude, nameof(result.Latitude));
            Assert.AreEqual("5.678", result.Longitude, nameof(result.Longitude));
            Assert.AreEqual(0, result.MinimumCaseCount, nameof(result.MinimumCaseCount));
            Assert.AreEqual(DateTime.Now.AddDays(-2).Date, result.MinimumCaseDate, nameof(result.MinimumCaseDate));
            Assert.AreEqual(4, result.MaximumCaseCount, nameof(result.MaximumCaseCount));
            Assert.AreEqual(DateTime.Now.AddDays(2).Date, result.MaximumCaseDate, nameof(result.MaximumCaseDate));
            Assert.AreEqual(1.0, result.AverageDailyCases, nameof(result.AverageDailyCases));
        }

        [TestMethod]
        public void Test_Repository_QueryByCounty_NoRange()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(null, null);

            //Act
            var result = repository.QueryByCounty("Alameda", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual("Alameda", result.Location, nameof(result.Location));
            Assert.AreEqual("1.234", result.Latitude, nameof(result.Latitude));
            Assert.AreEqual("5.678", result.Longitude, nameof(result.Longitude));
            Assert.AreEqual(0, result.MinimumCaseCount, nameof(result.MinimumCaseCount));
            Assert.AreEqual(DateTime.Now.AddDays(-2).Date, result.MinimumCaseDate, nameof(result.MinimumCaseDate));
            Assert.AreEqual(4, result.MaximumCaseCount, nameof(result.MaximumCaseCount));
            Assert.AreEqual(DateTime.Now.AddDays(2).Date, result.MaximumCaseDate, nameof(result.MaximumCaseDate));
            Assert.AreEqual(1.0, result.AverageDailyCases, nameof(result.AverageDailyCases));
        }

        [TestMethod]
        public void Test_Repository_QueryByState_InRange()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

            //Act
            var result = repository.QueryByState("California", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual("California", result.Location, nameof(result.Location));
            Assert.AreEqual(string.Empty, result.Latitude, nameof(result.Latitude));
            Assert.AreEqual(string.Empty, result.Longitude, nameof(result.Longitude));
            Assert.AreEqual(3, result.MinimumCaseCount, nameof(result.MinimumCaseCount));
            Assert.AreEqual(DateTime.Now.AddDays(-1).Date, result.MinimumCaseDate, nameof(result.MinimumCaseDate));
            Assert.AreEqual(9, result.MaximumCaseCount, nameof(result.MaximumCaseCount));
            Assert.AreEqual(DateTime.Now.AddDays(1).Date, result.MaximumCaseDate, nameof(result.MaximumCaseDate));
            Assert.AreEqual(3.0, result.AverageDailyCases, nameof(result.AverageDailyCases));
        }

        [TestMethod, ExpectedException(typeof(DataNotFoundException))]
        public void Test_Repository_QueryByState_StateDoesntExist()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

            //Act
            var result = repository.QueryByState("Not A State", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            //Expect Exception
        }

        [TestMethod]
        public void Test_Repository_QueryByState_OutOfRange()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(DateTime.Now.AddDays(-3), DateTime.Now.AddDays(3));

            //Act
            var result = repository.QueryByState("California", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual("California", result.Location, nameof(result.Location));
            Assert.AreEqual(string.Empty, result.Latitude, nameof(result.Latitude));
            Assert.AreEqual(string.Empty, result.Longitude, nameof(result.Longitude));
            Assert.AreEqual(0, result.MinimumCaseCount, nameof(result.MinimumCaseCount));
            Assert.AreEqual(DateTime.Now.AddDays(-2).Date, result.MinimumCaseDate, nameof(result.MinimumCaseDate));
            Assert.AreEqual(12, result.MaximumCaseCount, nameof(result.MaximumCaseCount));
            Assert.AreEqual(DateTime.Now.AddDays(2).Date, result.MaximumCaseDate, nameof(result.MaximumCaseDate));
            Assert.AreEqual(3.0, result.AverageDailyCases, nameof(result.AverageDailyCases));
        }

        [TestMethod]
        public void Test_Repository_QueryByState_NoRange()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(null, null);

            //Act
            var result = repository.QueryByState("California", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual("California", result.Location, nameof(result.Location));
            Assert.AreEqual(string.Empty, result.Latitude, nameof(result.Latitude));
            Assert.AreEqual(string.Empty, result.Longitude, nameof(result.Longitude));
            Assert.AreEqual(0, result.MinimumCaseCount, nameof(result.MinimumCaseCount));
            Assert.AreEqual(DateTime.Now.AddDays(-2).Date, result.MinimumCaseDate, nameof(result.MinimumCaseDate));
            Assert.AreEqual(12, result.MaximumCaseCount, nameof(result.MaximumCaseCount));
            Assert.AreEqual(DateTime.Now.AddDays(2).Date, result.MaximumCaseDate, nameof(result.MaximumCaseDate));
            Assert.AreEqual(3.0, result.AverageDailyCases, nameof(result.AverageDailyCases));
        }

        [TestMethod]
        public void Test_Repository_DailyBreakdownByCounty_InRange()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

            //Act
            var result = repository.GetDailyBreakdownByCounty("Alameda", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual("Alameda", result.Location, nameof(result.Location));
            Assert.AreEqual("1.234", result.Latitude, nameof(result.Latitude));
            Assert.AreEqual("5.678", result.Longitude, nameof(result.Longitude));
            Assert.AreEqual(3, result.DailyChanges.Count, $"{nameof(result.DailyChanges)}.Count");

            var dailyChange0 = result.DailyChanges[0];
            Assert.AreEqual(DateTime.Now.AddDays(-1).Date, dailyChange0.Date, $"{nameof(dailyChange0)}.{nameof(dailyChange0.Date)}");
            Assert.AreEqual(1, dailyChange0.TotalCases, $"{nameof(dailyChange0)}.{nameof(dailyChange0.TotalCases)}");
            Assert.AreEqual(1, dailyChange0.NewCases, $"{nameof(dailyChange0)}.{nameof(dailyChange0.NewCases)}");

            var dailyChange1 = result.DailyChanges[1];
            Assert.AreEqual(DateTime.Now.Date, dailyChange1.Date, $"{nameof(dailyChange1)}.{nameof(dailyChange1.Date)}");
            Assert.AreEqual(2, dailyChange1.TotalCases, $"{nameof(dailyChange1)}.{nameof(dailyChange1.TotalCases)}");
            Assert.AreEqual(1, dailyChange1.NewCases, $"{nameof(dailyChange1)}.{nameof(dailyChange1.NewCases)}");

            var dailyChange2 = result.DailyChanges[2];
            Assert.AreEqual(DateTime.Now.AddDays(1).Date, dailyChange2.Date, $"{nameof(dailyChange2)}.{nameof(dailyChange2.Date)}");
            Assert.AreEqual(3, dailyChange2.TotalCases, $"{nameof(dailyChange2)}.{nameof(dailyChange2.TotalCases)}");
            Assert.AreEqual(1, dailyChange2.NewCases, $"{nameof(dailyChange2)}.{nameof(dailyChange2.NewCases)}");
        }

        [TestMethod, ExpectedException(typeof(DataNotFoundException))]
        public void Test_Repository_DailyBreakdownByCounty_CountyDoesntExist()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

            //Act
            var result = repository.GetDailyBreakdownByCounty("Not A County", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            //Expect Exception
        }

        [TestMethod]
        public void Test_Repository_DailyBreakdownByCounty_OutOfRange()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(DateTime.Now.AddDays(-3), DateTime.Now.AddDays(3));

            //Act
            var result = repository.GetDailyBreakdownByCounty("Alameda", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual("Alameda", result.Location, nameof(result.Location));
            Assert.AreEqual("1.234", result.Latitude, nameof(result.Latitude));
            Assert.AreEqual("5.678", result.Longitude, nameof(result.Longitude));
            Assert.AreEqual(5, result.DailyChanges.Count, $"{nameof(result.DailyChanges)}.Count");

            var dailyChange0 = result.DailyChanges[0];
            Assert.AreEqual(DateTime.Now.AddDays(-2).Date, dailyChange0.Date, $"{nameof(dailyChange0)}.{nameof(dailyChange0.Date)}");
            Assert.AreEqual(0, dailyChange0.TotalCases, $"{nameof(dailyChange0)}.{nameof(dailyChange0.TotalCases)}");
            Assert.AreEqual(0, dailyChange0.NewCases, $"{nameof(dailyChange0)}.{nameof(dailyChange0.NewCases)}");

            var dailyChange1 = result.DailyChanges[1];
            Assert.AreEqual(DateTime.Now.Date.AddDays(-1).Date, dailyChange1.Date, $"{nameof(dailyChange1)}.{nameof(dailyChange1.Date)}");
            Assert.AreEqual(1, dailyChange1.TotalCases, $"{nameof(dailyChange1)}.{nameof(dailyChange1.TotalCases)}");
            Assert.AreEqual(1, dailyChange1.NewCases, $"{nameof(dailyChange1)}.{nameof(dailyChange1.NewCases)}");

            var dailyChange2 = result.DailyChanges[2];
            Assert.AreEqual(DateTime.Now.AddDays(0).Date, dailyChange2.Date, $"{nameof(dailyChange2)}.{nameof(dailyChange2.Date)}");
            Assert.AreEqual(2, dailyChange2.TotalCases, $"{nameof(dailyChange2)}.{nameof(dailyChange2.TotalCases)}");
            Assert.AreEqual(1, dailyChange2.NewCases, $"{nameof(dailyChange2)}.{nameof(dailyChange2.NewCases)}");

            var dailyChange3 = result.DailyChanges[3];
            Assert.AreEqual(DateTime.Now.Date.AddDays(1).Date, dailyChange3.Date, $"{nameof(dailyChange3)}.{nameof(dailyChange3.Date)}");
            Assert.AreEqual(3, dailyChange3.TotalCases, $"{nameof(dailyChange3)}.{nameof(dailyChange3.TotalCases)}");
            Assert.AreEqual(1, dailyChange3.NewCases, $"{nameof(dailyChange3)}.{nameof(dailyChange3.NewCases)}");

            var dailyChange4 = result.DailyChanges[4];
            Assert.AreEqual(DateTime.Now.AddDays(2).Date, dailyChange4.Date, $"{nameof(dailyChange4)}.{nameof(dailyChange4.Date)}");
            Assert.AreEqual(4, dailyChange4.TotalCases, $"{nameof(dailyChange4)}.{nameof(dailyChange4.TotalCases)}");
            Assert.AreEqual(1, dailyChange4.NewCases, $"{nameof(dailyChange4)}.{nameof(dailyChange4.NewCases)}");
        }

        [TestMethod]
        public void Test_Repository_DailyBreakdownByCounty_NoRange()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(null, null);

            //Act
            var result = repository.GetDailyBreakdownByCounty("Alameda", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual("Alameda", result.Location, nameof(result.Location));
            Assert.AreEqual("1.234", result.Latitude, nameof(result.Latitude));
            Assert.AreEqual("5.678", result.Longitude, nameof(result.Longitude));
            Assert.AreEqual(5, result.DailyChanges.Count, $"{nameof(result.DailyChanges)}.Count");

            var dailyChange0 = result.DailyChanges[0];
            Assert.AreEqual(DateTime.Now.AddDays(-2).Date, dailyChange0.Date, $"{nameof(dailyChange0)}.{nameof(dailyChange0.Date)}");
            Assert.AreEqual(0, dailyChange0.TotalCases, $"{nameof(dailyChange0)}.{nameof(dailyChange0.TotalCases)}");
            Assert.AreEqual(0, dailyChange0.NewCases, $"{nameof(dailyChange0)}.{nameof(dailyChange0.NewCases)}");

            var dailyChange1 = result.DailyChanges[1];
            Assert.AreEqual(DateTime.Now.Date.AddDays(-1).Date, dailyChange1.Date, $"{nameof(dailyChange1)}.{nameof(dailyChange1.Date)}");
            Assert.AreEqual(1, dailyChange1.TotalCases, $"{nameof(dailyChange1)}.{nameof(dailyChange1.TotalCases)}");
            Assert.AreEqual(1, dailyChange1.NewCases, $"{nameof(dailyChange1)}.{nameof(dailyChange1.NewCases)}");

            var dailyChange2 = result.DailyChanges[2];
            Assert.AreEqual(DateTime.Now.AddDays(0).Date, dailyChange2.Date, $"{nameof(dailyChange2)}.{nameof(dailyChange2.Date)}");
            Assert.AreEqual(2, dailyChange2.TotalCases, $"{nameof(dailyChange2)}.{nameof(dailyChange2.TotalCases)}");
            Assert.AreEqual(1, dailyChange2.NewCases, $"{nameof(dailyChange2)}.{nameof(dailyChange2.NewCases)}");

            var dailyChange3 = result.DailyChanges[3];
            Assert.AreEqual(DateTime.Now.Date.AddDays(1).Date, dailyChange3.Date, $"{nameof(dailyChange3)}.{nameof(dailyChange3.Date)}");
            Assert.AreEqual(3, dailyChange3.TotalCases, $"{nameof(dailyChange3)}.{nameof(dailyChange3.TotalCases)}");
            Assert.AreEqual(1, dailyChange3.NewCases, $"{nameof(dailyChange3)}.{nameof(dailyChange3.NewCases)}");

            var dailyChange4 = result.DailyChanges[4];
            Assert.AreEqual(DateTime.Now.AddDays(2).Date, dailyChange4.Date, $"{nameof(dailyChange4)}.{nameof(dailyChange4.Date)}");
            Assert.AreEqual(4, dailyChange4.TotalCases, $"{nameof(dailyChange4)}.{nameof(dailyChange4.TotalCases)}");
            Assert.AreEqual(1, dailyChange4.NewCases, $"{nameof(dailyChange4)}.{nameof(dailyChange4.NewCases)}");
        }

        [TestMethod]
        public void Test_Repository_DailyBreakdownByState_InRange()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

            //Act
            var result = repository.GetDailyBreakdownByState("California", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual("California", result.Location, nameof(result.Location));
            Assert.AreEqual(string.Empty, result.Latitude, nameof(result.Latitude));
            Assert.AreEqual(string.Empty, result.Longitude, nameof(result.Longitude));
            Assert.AreEqual(3, result.DailyChanges.Count, $"{nameof(result.DailyChanges)}.Count");

            var dailyChange0 = result.DailyChanges[0];
            Assert.AreEqual(DateTime.Now.AddDays(-1).Date, dailyChange0.Date, $"{nameof(dailyChange0)}.{nameof(dailyChange0.Date)}");
            Assert.AreEqual(3, dailyChange0.TotalCases, $"{nameof(dailyChange0)}.{nameof(dailyChange0.TotalCases)}");
            Assert.AreEqual(3, dailyChange0.NewCases, $"{nameof(dailyChange0)}.{nameof(dailyChange0.NewCases)}");

            var dailyChange1 = result.DailyChanges[1];
            Assert.AreEqual(DateTime.Now.Date, dailyChange1.Date, $"{nameof(dailyChange1)}.{nameof(dailyChange1.Date)}");
            Assert.AreEqual(6, dailyChange1.TotalCases, $"{nameof(dailyChange1)}.{nameof(dailyChange1.TotalCases)}");
            Assert.AreEqual(3, dailyChange1.NewCases, $"{nameof(dailyChange1)}.{nameof(dailyChange1.NewCases)}");

            var dailyChange2 = result.DailyChanges[2];
            Assert.AreEqual(DateTime.Now.AddDays(1).Date, dailyChange2.Date, $"{nameof(dailyChange2)}.{nameof(dailyChange2.Date)}");
            Assert.AreEqual(9, dailyChange2.TotalCases, $"{nameof(dailyChange2)}.{nameof(dailyChange2.TotalCases)}");
            Assert.AreEqual(3, dailyChange2.NewCases, $"{nameof(dailyChange2)}.{nameof(dailyChange2.NewCases)}");
        }

        [TestMethod, ExpectedException(typeof(DataNotFoundException))]
        public void Test_Repository_DailyBreakdownByState_StateDoesntExist()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

            //Act
            var result = repository.GetDailyBreakdownByState("Not A State", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            //Expect Exception
        }

        [TestMethod]
        public void Test_Repository_DailyBreakdownByState_OutOfRange()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(DateTime.Now.AddDays(-3), DateTime.Now.AddDays(3));

            //Act
            var result = repository.GetDailyBreakdownByState("California", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual("California", result.Location, nameof(result.Location));
            Assert.AreEqual(string.Empty, result.Latitude, nameof(result.Latitude));
            Assert.AreEqual(string.Empty, result.Longitude, nameof(result.Longitude));
            Assert.AreEqual(5, result.DailyChanges.Count, $"{nameof(result.DailyChanges)}.Count");

            var dailyChange0 = result.DailyChanges[0];
            Assert.AreEqual(DateTime.Now.AddDays(-2).Date, dailyChange0.Date, $"{nameof(dailyChange0)}.{nameof(dailyChange0.Date)}");
            Assert.AreEqual(0, dailyChange0.TotalCases, $"{nameof(dailyChange0)}.{nameof(dailyChange0.TotalCases)}");
            Assert.AreEqual(0, dailyChange0.NewCases, $"{nameof(dailyChange0)}.{nameof(dailyChange0.NewCases)}");

            var dailyChange1 = result.DailyChanges[1];
            Assert.AreEqual(DateTime.Now.Date.AddDays(-1).Date, dailyChange1.Date, $"{nameof(dailyChange1)}.{nameof(dailyChange1.Date)}");
            Assert.AreEqual(3, dailyChange1.TotalCases, $"{nameof(dailyChange1)}.{nameof(dailyChange1.TotalCases)}");
            Assert.AreEqual(3, dailyChange1.NewCases, $"{nameof(dailyChange1)}.{nameof(dailyChange1.NewCases)}");

            var dailyChange2 = result.DailyChanges[2];
            Assert.AreEqual(DateTime.Now.AddDays(0).Date, dailyChange2.Date, $"{nameof(dailyChange2)}.{nameof(dailyChange2.Date)}");
            Assert.AreEqual(6, dailyChange2.TotalCases, $"{nameof(dailyChange2)}.{nameof(dailyChange2.TotalCases)}");
            Assert.AreEqual(3, dailyChange2.NewCases, $"{nameof(dailyChange2)}.{nameof(dailyChange2.NewCases)}");

            var dailyChange3 = result.DailyChanges[3];
            Assert.AreEqual(DateTime.Now.Date.AddDays(1).Date, dailyChange3.Date, $"{nameof(dailyChange3)}.{nameof(dailyChange3.Date)}");
            Assert.AreEqual(9, dailyChange3.TotalCases, $"{nameof(dailyChange3)}.{nameof(dailyChange3.TotalCases)}");
            Assert.AreEqual(3, dailyChange3.NewCases, $"{nameof(dailyChange3)}.{nameof(dailyChange3.NewCases)}");

            var dailyChange4 = result.DailyChanges[4];
            Assert.AreEqual(DateTime.Now.AddDays(2).Date, dailyChange4.Date, $"{nameof(dailyChange4)}.{nameof(dailyChange4.Date)}");
            Assert.AreEqual(12, dailyChange4.TotalCases, $"{nameof(dailyChange4)}.{nameof(dailyChange4.TotalCases)}");
            Assert.AreEqual(3, dailyChange4.NewCases, $"{nameof(dailyChange4)}.{nameof(dailyChange4.NewCases)}");
        }

        [TestMethod]
        public void Test_Repository_DailyBreakdownByState_NoRange()
        {
            //Arrange
            var mockSource = new Mock<ICovidDataSource>();

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(GenerateData()));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(null, null);

            //Act
            var result = repository.GetDailyBreakdownByState("California", range)
                .ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual("California", result.Location, nameof(result.Location));
            Assert.AreEqual(string.Empty, result.Latitude, nameof(result.Latitude));
            Assert.AreEqual(string.Empty, result.Longitude, nameof(result.Longitude));
            Assert.AreEqual(5, result.DailyChanges.Count, $"{nameof(result.DailyChanges)}.Count");

            var dailyChange0 = result.DailyChanges[0];
            Assert.AreEqual(DateTime.Now.AddDays(-2).Date, dailyChange0.Date, $"{nameof(dailyChange0)}.{nameof(dailyChange0.Date)}");
            Assert.AreEqual(0, dailyChange0.TotalCases, $"{nameof(dailyChange0)}.{nameof(dailyChange0.TotalCases)}");
            Assert.AreEqual(0, dailyChange0.NewCases, $"{nameof(dailyChange0)}.{nameof(dailyChange0.NewCases)}");

            var dailyChange1 = result.DailyChanges[1];
            Assert.AreEqual(DateTime.Now.Date.AddDays(-1).Date, dailyChange1.Date, $"{nameof(dailyChange1)}.{nameof(dailyChange1.Date)}");
            Assert.AreEqual(3, dailyChange1.TotalCases, $"{nameof(dailyChange1)}.{nameof(dailyChange1.TotalCases)}");
            Assert.AreEqual(3, dailyChange1.NewCases, $"{nameof(dailyChange1)}.{nameof(dailyChange1.NewCases)}");

            var dailyChange2 = result.DailyChanges[2];
            Assert.AreEqual(DateTime.Now.AddDays(0).Date, dailyChange2.Date, $"{nameof(dailyChange2)}.{nameof(dailyChange2.Date)}");
            Assert.AreEqual(6, dailyChange2.TotalCases, $"{nameof(dailyChange2)}.{nameof(dailyChange2.TotalCases)}");
            Assert.AreEqual(3, dailyChange2.NewCases, $"{nameof(dailyChange2)}.{nameof(dailyChange2.NewCases)}");

            var dailyChange3 = result.DailyChanges[3];
            Assert.AreEqual(DateTime.Now.Date.AddDays(1).Date, dailyChange3.Date, $"{nameof(dailyChange3)}.{nameof(dailyChange3.Date)}");
            Assert.AreEqual(9, dailyChange3.TotalCases, $"{nameof(dailyChange3)}.{nameof(dailyChange3.TotalCases)}");
            Assert.AreEqual(3, dailyChange3.NewCases, $"{nameof(dailyChange3)}.{nameof(dailyChange3.NewCases)}");

            var dailyChange4 = result.DailyChanges[4];
            Assert.AreEqual(DateTime.Now.AddDays(2).Date, dailyChange4.Date, $"{nameof(dailyChange4)}.{nameof(dailyChange4.Date)}");
            Assert.AreEqual(12, dailyChange4.TotalCases, $"{nameof(dailyChange4)}.{nameof(dailyChange4.TotalCases)}");
            Assert.AreEqual(3, dailyChange4.NewCases, $"{nameof(dailyChange4)}.{nameof(dailyChange4.NewCases)}");
        }

        private IList<CovidDataRow> GenerateData()
        {
            return new List<CovidDataRow>()
            {
                new CovidDataRow("Alameda", "California", "1.234", "5.678", new Dictionary<DateTime, int>()
                {
                    { DateTime.Now.AddDays(-2).Date, 0 },
                    { DateTime.Now.AddDays(-1).Date, 1 },
                    { DateTime.Now.Date, 2 },
                    { DateTime.Now.AddDays(1).Date, 3 },
                    { DateTime.Now.AddDays(2).Date, 4 },
                }),
                new CovidDataRow("Contra Costa", "California", "5.678", "1.234", new Dictionary<DateTime, int>()
                {
                    { DateTime.Now.AddDays(-2).Date, 0 },
                    { DateTime.Now.AddDays(-1).Date, 2 },
                    { DateTime.Now.Date, 4 },
                    { DateTime.Now.AddDays(1).Date, 6 },
                    { DateTime.Now.AddDays(2).Date, 8 },
                }),
                //Negative case, this one is not chosen by any tests.
                new CovidDataRow("Contra Costa", "Utah", "9.999", "9.999", new Dictionary<DateTime, int>()
                {
                    { DateTime.Now.AddDays(-2).Date, 9 },
                    { DateTime.Now.AddDays(-1).Date, 99 },
                    { DateTime.Now.Date, 999 },
                    { DateTime.Now.AddDays(1).Date, 9999 },
                    { DateTime.Now.AddDays(2).Date, 99999 },
                }),
            };
        }

    }
}
