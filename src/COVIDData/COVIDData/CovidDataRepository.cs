using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVIDData
{
    public class CovidDataRepository : ICovidDataRepository
    {
        private ICovidDataSource _covidDataSource;
        private IList<CovidDataRow> _covidData;

        public CovidDataRepository(ICovidDataSource dataSource)
        {
            _covidDataSource = dataSource;
        }

        public async Task<CovidQueryResult> QueryByCounty(string county, DateRange range)
        {
            var data = await CovidData();

            var countyRow = data.First(d => string.Equals(d.County, county, StringComparison.OrdinalIgnoreCase));

            var countyCases = countyRow.ConfirmedCases.Where(kvp => range.Contains(kvp.Key));

            var minValue = countyCases.Min(kvp => kvp.Value);
            var minCases = countyCases.First(kvp => kvp.Value == minValue);

            var maxValue = countyCases.Max(kvp => kvp.Value);
            var maxCases = countyCases.First(kvp => kvp.Value == maxValue);

            var averageCases = countyCases.Average(kvp => kvp.Value);

            return new CovidQueryResult(county, countyRow.Latitude, countyRow.Longitude,
                averageCases, minCases.Value, minCases.Key, maxCases.Value, maxCases.Key);
        }

        public async Task<CovidQueryResult> QueryByState(string state, DateRange range)
        {
            var data = await CovidData();

            var stateRows = data.Where(d => string.Equals(d.ProvinceState, state, StringComparison.OrdinalIgnoreCase));

            var stateCases = stateRows.SelectMany(county => county.ConfirmedCases.Where(kvp => range.Contains(kvp.Key)));

            var minValue = stateCases.Min(kvp => kvp.Value);
            var minCases = stateCases.First(kvp => kvp.Value == minValue);

            var maxValue = stateCases.Max(kvp => kvp.Value);
            var maxCases = stateCases.First(kvp => kvp.Value == maxValue);

            var averageCases = stateCases.Average(kvp => kvp.Value);

            return new CovidQueryResult(state, string.Empty, string.Empty,
                averageCases, minCases.Value, minCases.Key, maxCases.Value, maxCases.Key);
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
