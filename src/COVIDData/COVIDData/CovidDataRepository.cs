using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace COVIDData
{
    public class CovidDataRepository
    {
        private CovidDataSource _covidDataSource;
        private IList<CovidDataRow> _covidData;

        public CovidDataRepository()
        {
            _covidDataSource = new CovidDataSource();
        }

        public async Task<CovidQueryResult> QueryByCounty(string county, DateRange range)
        {
            var data = await CovidData();

            var countyRow = data.First(d => string.Equals(d.County, county, StringComparison.OrdinalIgnoreCase));

            var countyCases = countyRow.ConfirmedCases.Where(kvp => range.Contains(kvp.Key));

            var minCases = countyCases.Min(kvp => kvp.Value);
            var maxCases = countyCases.Max(kvp => kvp.Value);
            double averageCases = countyCases.Average(kvp => kvp.Value);

            
        }

        private async Task<IList<CovidDataRow>> CovidData()
        {
            if (_covidData == null)
            {
                _covidData = await _covidDataSource.GetData();
            }
            return _covidData;
        }

    }
}
