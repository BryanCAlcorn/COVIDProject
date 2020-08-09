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

            var minCases = countyRow.ConfirmedCases[range.StartDate];
            var maxCases = countyRow.ConfirmedCases[range.EndDate];
            
            var averageCases = GetAverage(minCases, maxCases, range.TotalDays);

            return new CovidQueryResult(county, countyRow.Latitude, countyRow.Longitude,
                averageCases, minCases, range.StartDate, maxCases, range.EndDate);
        }

        public async Task<CovidQueryResult> QueryByState(string state, DateRange range)
        {
            var data = await CovidData();

            var stateRows = data.Where(d => string.Equals(d.ProvinceState, state, StringComparison.OrdinalIgnoreCase));

            var stateTotals = stateRows.Aggregate(new Dictionary<DateTime, int>(), (dict, county) =>
            {
                var countyStartCases = county.ConfirmedCases[range.StartDate];
                var countyEndCases = county.ConfirmedCases[range.EndDate];
                
                if (dict.ContainsKey(range.StartDate))
                {
                    dict[range.StartDate] += countyStartCases;
                }
                else
                {
                    dict[range.StartDate] = countyStartCases;
                }

                if (dict.ContainsKey(range.EndDate))
                {
                    dict[range.EndDate] += countyEndCases;
                }
                else
                {
                    dict[range.EndDate] = countyEndCases;
                }

                return dict;
            });

            var minCases = stateTotals[range.StartDate];
            var maxCases = stateTotals[range.EndDate];

            var averageCases = GetAverage(minCases, maxCases, range.TotalDays);

            return new CovidQueryResult(state, string.Empty, string.Empty,
                averageCases, minCases, range.StartDate, maxCases, range.EndDate);
        }

        private double GetAverage(int minCases, int maxCases, double days)
        {
            if (days > 0)
            {
                //Average Case Change over time
                var average = (maxCases - minCases) / days;
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
