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

            IList<CovidDataRow> covidRows = new List<CovidDataRow>()
            {
                new CovidDataRow("Alameda", "California", new Dictionary<DateTime, int>()
                {
                    { DateTime.Now.AddDays(-2).Date, 0 },
                    { DateTime.Now.AddDays(-1).Date, 0 },
                    { DateTime.Now.Date, 1 },
                    { DateTime.Now.AddDays(1).Date, 2 },
                    { DateTime.Now.AddDays(2).Date, 4 },
                }),
                new CovidDataRow("Contra Costa", "California", new Dictionary<DateTime, int>()
                {
                    { DateTime.Now.AddDays(-2).Date, 0 },
                    { DateTime.Now.AddDays(-1).Date, 1 },
                    { DateTime.Now.Date, 2 },
                    { DateTime.Now.AddDays(1).Date, 3 },
                    { DateTime.Now.AddDays(2).Date, 5 },
                }),
            };

            mockSource.Setup(s => s.GetData()).Returns(() => Task.FromResult(covidRows));

            var repository = new CovidDataRepository(mockSource.Object);

            var range = new DateRange(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

            //Act
            var result = repository.QueryByCounty("Alameda", range).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual("Alameda", result.Location);
        }

    }
}
