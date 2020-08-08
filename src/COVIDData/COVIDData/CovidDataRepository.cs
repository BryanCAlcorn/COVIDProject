using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

            var averageCases = GetAverage(minCases, maxCases, range.TotalDays);

            return new CovidQueryResult(county, countyRow.Latitude, countyRow.Longitude,
                averageCases, minCases.Value, minCases.Key, maxCases.Value, maxCases.Key);
        }

        public async Task<CovidQueryResult> QueryByState(string state, DateRange range)
        {
            var data = await CovidData();

            var stateRows = data.Where(d => string.Equals(d.ProvinceState, state, StringComparison.OrdinalIgnoreCase));

            var stateCases = stateRows.SelectMany(county => county.ConfirmedCases.Where(kvp => range.Contains(kvp.Key)));

            var stateTotals = stateRows.Aggregate(new Dictionary<DateTime, int>(), (dict, county) =>
            {
                var countyCases = county.ConfirmedCases.Where(kvp => range.Contains(kvp.Key));

                foreach(var cases in countyCases)
                {
                    if (dict.ContainsKey(cases.Key))
                    {
                        dict[cases.Key] += cases.Value;
                    }
                    else
                    {
                        dict[cases.Key] = cases.Value;
                    }
                }

                return dict;
            });

            var minValue = stateTotals.Min(kvp => kvp.Value);
            var minCases = stateTotals.First(kvp => kvp.Value == minValue);

            var maxValue = stateTotals.Max(kvp => kvp.Value);
            var maxCases = stateTotals.First(kvp => kvp.Value == maxValue);

            var averageCases = GetAverage(minCases, maxCases, range.TotalDays);

            return new CovidQueryResult(state, string.Empty, string.Empty,
                averageCases, minCases.Value, minCases.Key, maxCases.Value, maxCases.Key);
        }

        private double GetAverage(KeyValuePair<DateTime, int> minCases, KeyValuePair<DateTime, int> maxCases, double days)
        {
            if (days > 0)
            {
                //Average Case Change over time
                var average = (maxCases.Value - minCases.Value) / days;
                //Rounded to the nearest tenth
                return Math.Round(average, 1);
            }
            else
            {
                return 0;
            }
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
