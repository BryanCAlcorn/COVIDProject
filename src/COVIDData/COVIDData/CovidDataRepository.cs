using COVIDData.Exceptions;
using COVIDData.Extensions;
using COVIDData.Interfaces;
using COVIDData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVIDData
{
    public class CovidDataRepository : ICovidDataRepository
    {
        private ICovidDataSource _covidDataSource;
        private IList<CovidDataRow> _cachedCovidData;
        private DateTime _fetchDate;

        public CovidDataRepository(ICovidDataSource dataSource)
        {
            _covidDataSource = dataSource;
        }

        public async Task<CovidQueryResult> QueryByCounty(string county, DateRange range)
        {
            var countyRow = await GetDataForCounty(county);

            var minCases = countyRow.ConfirmedCases.GetValueOrMin(range.StartDate, out var minDate);
            var maxCases = countyRow.ConfirmedCases.GetValueOrMax(range.EndDate, out var maxDate);
            
            var totalDays = (maxDate - minDate).TotalDays;
            var averageCases = GetAverage(minCases, maxCases, totalDays);

            return new CovidQueryResult(county, countyRow.Latitude, countyRow.Longitude,
                averageCases, minCases, minDate, maxCases, maxDate);
        }

        public async Task<CovidQueryResult> QueryByState(string state, DateRange range)
        {
            var stateRows = await GetDataForState(state);

            var stateTotals = AggregateCaseTotalsOverRange(stateRows, range);

            var minDate = stateTotals.Keys.Min();
            var maxDate = stateTotals.Keys.Max();
            var minCases = stateTotals[minDate];
            var maxCases = stateTotals[maxDate];
            var totalDays = (maxDate - minDate).TotalDays;
            var averageCases = GetAverage(minCases, maxCases, totalDays);

            return new CovidQueryResult(state, string.Empty, string.Empty,
                averageCases, minCases, minDate, maxCases, maxDate);
        }

        public async Task<DailyBreakdownResult> GetDailyBreakdownByCounty(string county, DateRange range)
        {
            var countyRow = await GetDataForCounty(county);

            return GetDailyBreakdown(county, countyRow.Latitude, countyRow.Longitude, countyRow.ConfirmedCases, range);
        }

        public async Task<DailyBreakdownResult> GetDailyBreakdownByState(string state, DateRange range)
        {
            var stateRows = await GetDataForState(state);

            var stateTotalsInRange = AggregateCaseTotalsOverRange(stateRows, range);

            return GetDailyBreakdown(state, string.Empty, string.Empty, stateTotalsInRange, range);
        }

        public async Task<RateOfChangeResult> GetRateOfChangeByCounty(string county, DateRange range)
        {
            var countyRow = await GetDataForCounty(county);

            return GetRateOfChange(county, countyRow.Latitude, countyRow.Longitude, countyRow.ConfirmedCases, range);
        }

        public async Task<RateOfChangeResult> GetRateOfChangeByState(string state, DateRange range)
        {
            var stateRows = await GetDataForState(state);

            var stateTotalsInRange = AggregateCaseTotalsOverRange(stateRows, range);

            return GetRateOfChange(state, string.Empty, string.Empty, stateTotalsInRange, range);
        }

        private DailyBreakdownResult GetDailyBreakdown(string location, string latitude, string longitude, 
            IReadOnlyDictionary<DateTime, int> locationData, DateRange range)
        {
            var orderedCasesInRange = locationData.Where(kvp => range.Contains(kvp.Key)).OrderBy(kvp => kvp.Key);

            var dailyChanges = new List<DailyChange>();

            var prevCases = 0;
            foreach (var dailyCases in orderedCasesInRange)
            {
                var change = new DailyChange(dailyCases.Key, dailyCases.Value, dailyCases.Value - prevCases);
                prevCases = dailyCases.Value;
                dailyChanges.Add(change);
            }

            return new DailyBreakdownResult(location, latitude, longitude, dailyChanges);
        }

        private RateOfChangeResult GetRateOfChange(string location, string latitude, string longitude,
            IReadOnlyDictionary<DateTime, int> locationData, DateRange range)
        {
            var orderedCasesInRange = locationData.Where(kvp => range.Contains(kvp.Key)).OrderBy(kvp => kvp.Key);

            var dailyRateOfChange = new List<DailyRateOfChange>();

            //TODO: Requirement is unclear on what the percentage should be against:
            //e.g. Could be percentage of total population in the region, etc.
            //Using the data we have, going for rate of change against the highest number of cases in the range.
            var maxCasesInRange = orderedCasesInRange.Max(kvp => kvp.Value);
            var normalizationFactor = 100.0 / maxCasesInRange;

            var prevCases = 0;
            foreach (var dailyCases in orderedCasesInRange)
            {
                var change = GetDailyRateOfChange(dailyCases, prevCases, normalizationFactor);
                prevCases = dailyCases.Value;
                dailyRateOfChange.Add(change);
            }

            return new RateOfChangeResult(location, latitude, longitude, dailyRateOfChange);
        }

        private DailyRateOfChange GetDailyRateOfChange(KeyValuePair<DateTime, int> dailyCases, int prevCases, double normalizationFactor)
        {
            var totalChange = dailyCases.Value - prevCases;
            var percentChange = Math.Round(totalChange * normalizationFactor, 1);
            return new DailyRateOfChange(dailyCases.Key, totalChange, percentChange);
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

        private async Task<CovidDataRow> GetDataForCounty(string county)
        {
            var data = await CovidData();

            var countyRow = data.FirstOrDefault(d => string.Equals(d.County, county, StringComparison.OrdinalIgnoreCase));
            if (countyRow == null) throw new DataNotFoundException($"County {county} not available in data set.", nameof(county));

            return countyRow;
        }

        private async Task<IEnumerable<CovidDataRow>> GetDataForState(string state)
        {
            var data = await CovidData();

            var stateRows = data.Where(d => string.Equals(d.ProvinceState, state, StringComparison.OrdinalIgnoreCase));
            if (!stateRows.Any()) throw new DataNotFoundException($"State {state} not available in data set", nameof(state));

            return stateRows;
        }

        private IReadOnlyDictionary<DateTime, int> AggregateCaseTotalsOverRange(IEnumerable<CovidDataRow> rowsToAgg, DateRange range)
        {
            return rowsToAgg.Aggregate(new Dictionary<DateTime, int>(), (dict, countyRow) =>
            {
                var casesInRange = countyRow.ConfirmedCases.Where(kvp => range.Contains(kvp.Key));

                foreach (var cases in casesInRange)
                {
                    dict.AddOrUpdateValue(cases.Key, cases.Value);
                }

                return dict;
            });
        }

        private async Task<IList<CovidDataRow>> CovidData()
        {
            if (_cachedCovidData == null ||
                //Re-Fetch cached data every day.
                (DateTime.Now - _fetchDate).TotalDays > 1)
            {
                _cachedCovidData = await _covidDataSource.GetData();
                _fetchDate = DateTime.Now;
            }

            return _cachedCovidData;
        }
    }
}
