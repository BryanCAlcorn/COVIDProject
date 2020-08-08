using COVIDData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace COVIDTests
{
    [TestClass]
    public class DataRepositoryTests
    {
        [TestMethod]
        public void Test_GetData_VerifyDataParsing()
        {
            //Arrange
            var fetcher = new CovidDataRepository();

            //Act
            var data = fetcher.GetData().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(data, nameof(data));
            Assert.AreEqual(3340, data.Count, nameof(data.Count));
            foreach (var dataRow in data)
            {
                Assert.AreEqual(199, dataRow.ConfirmedCases.Count, $"{nameof(CovidDataRow.ConfirmedCases)}.Count");
            }
        }
    }
}
