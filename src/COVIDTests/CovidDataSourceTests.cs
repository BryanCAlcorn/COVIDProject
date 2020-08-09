using COVIDData;
using COVIDData.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace COVIDTests
{
    [TestClass]
    public class CovidDataSourceTests
    {
        [TestMethod]
        public void Test_GetData_VerifyDataParsing()
        {
            //Arrange
            var source = new CovidDataSource();

            //Act
            var data = source.GetData().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(data, nameof(data));
            Assert.AreEqual(3340, data.Count, nameof(data.Count));
            //This data appears to change over time, since we're using the real data source here!
            //foreach (var dataRow in data)
            //{
            //    Assert.AreEqual(199, dataRow.ConfirmedCases.Count, $"{nameof(CovidDataRow.ConfirmedCases)}.Count");
            //}
        }
    }
}
