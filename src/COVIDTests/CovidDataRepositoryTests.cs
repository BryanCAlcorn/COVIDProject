using COVIDData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COVIDTests
{
    [TestClass]
    public class CovidDataRepositoryTests
    {
        [TestMethod]
        public void Test_Repository_QueryByCounty()
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

        [TestMethod]
        public void Test_Repository_QueryByState()
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
                new CovidDataRow("Contra Costa", "Utah", "9.999", "9.999", new Dictionary<DateTime, int>()
                {
                    { DateTime.Now.AddDays(-2).Date, 0 },
                    { DateTime.Now.AddDays(-1).Date, 99 },
                    { DateTime.Now.Date, 999 },
                    { DateTime.Now.AddDays(1).Date, 9999 },
                    { DateTime.Now.AddDays(2).Date, 99999 },
                }),
            };
        }

    }
}
