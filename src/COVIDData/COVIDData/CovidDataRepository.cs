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

            var countyRow = data.FirstOrDefault(d => string.Equals(d.County, county, StringComparison.OrdinalIgnoreCase));
            if (countyRow == null) throw new DataNotFoundException($"County {county} not available in data set.", nameof(county));

            var minDate = range.StartDate;
            if(!countyRow.ConfirmedCases.TryGetValue(minDate, out var minCases))
            {
                minDate = countyRow.ConfirmedCases.Keys.Min();
                minCases = countyRow.ConfirmedCases[minDate];
            }

            var maxDate = range.EndDate;
            if(!countyRow.ConfirmedCases.TryGetValue(maxDate, out var maxCases))
            {
                maxDate = countyRow.ConfirmedCases.Keys.Max();
                maxCases = countyRow.ConfirmedCases[maxDate];
            }

            var totalDays = (maxDate - minDate).TotalDays;
            var averageCases = GetAverage(minCases, maxCases, totalDays);

            return new CovidQueryResult(county, countyRow.Latitude, countyRow.Longitude,
                averageCases, minCases, minDate, maxCases, maxDate);
        }

        public async Task<CovidQueryResult> QueryByState(string state, DateRange range)
        {
            var data = await CovidData();

            var stateRows = data.Where(d => string.Equals(d.ProvinceState, state, StringComparison.OrdinalIgnoreCase));
            if (!stateRows.Any()) throw new DataNotFoundException($"State {state} not available in data set", nameof(state));

            var stateTotals = stateRows.Aggregate(new Dictionary<DateTime, int>(), (dict, countyRow) =>
            {
                //TODO: Factor this into a separate method.
                var minDate = range.StartDate;
                if (!countyRow.ConfirmedCases.TryGetValue(minDate, out var minCases))
                {
                    minDate = countyRow.ConfirmedCases.Keys.Min();
                    minCases = countyRow.ConfirmedCases[minDate];
                }

                var maxDate = range.EndDate;
                if (!countyRow.ConfirmedCases.TryGetValue(maxDate, out var maxCases))
                {
                    maxDate = countyRow.ConfirmedCases.Keys.Max();
                    maxCases = countyRow.ConfirmedCases[maxDate];
                }

                if (dict.ContainsKey(minDate))
                {
                    dict[minDate] += minCases;
                }
                else
                {
                    dict[minDate] = minCases;
                }

                if (dict.ContainsKey(maxDate))
                {
                    dict[maxDate] += maxCases;
                }
                else
                {
                    dict[maxDate] = maxCases;
                }

                return dict;
            });

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
            var data = await CovidData();

            var countyRow = data.FirstOrDefault(d => string.Equals(d.County, county, StringComparison.OrdinalIgnoreCase));
            if (countyRow == null) throw new DataNotFoundException($"County {county} not available in data set.", nameof(county));

            var orderedCasesInRange = countyRow.ConfirmedCases.Where(kvp => range.Contains(kvp.Key)).OrderBy(kvp => kvp.Key);

            var dailyChanges = new List<DailyChange>();

            var prevCases = 0;
            foreach(var dailyCases in orderedCasesInRange)
            {
                var change = new DailyChange(dailyCases.Key, dailyCases.Value, dailyCases.Value - prevCases);
                prevCases = dailyCases.Value;
                dailyChanges.Add(change);
            }

            return new DailyBreakdownResult(county, countyRow.Latitude, countyRow.Longitude, dailyChanges);
        }

        public async Task<DailyBreakdownResult> GetDailyBreakdownByState(string state, DateRange range)
        {
            var data = await CovidData();

            var stateRows = data.Where(d => string.Equals(d.ProvinceState, state, StringComparison.OrdinalIgnoreCase));
            if (!stateRows.Any()) throw new DataNotFoundException($"State {state} not available in data set", nameof(state));

            var stateTotalsInRange = stateRows.Aggregate(new Dictionary<DateTime, int>(), (dict, countyRow) =>
            {
                var casesInRange = countyRow.ConfirmedCases.Where(kvp => range.Contains(kvp.Key));

                foreach(var cases in casesInRange)
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

            var orderedCasesInRange = stateTotalsInRange.OrderBy(kvp => kvp.Key);

            var dailyChanges = new List<DailyChange>();

            var prevCases = 0;
            foreach (var dailyCases in orderedCasesInRange)
            {
                var change = new DailyChange(dailyCases.Key, dailyCases.Value, dailyCases.Value - prevCases);
                prevCases = dailyCases.Value;
                dailyChanges.Add(change);
            }

            return new DailyBreakdownResult(state, string.Empty, string.Empty, dailyChanges);
        }

        public async Task<RateOfChangeResult> GetRateOfChangeByCounty(string county, DateRange range)
        {
            var data = await CovidData();

            var countyRow = data.FirstOrDefault(d => string.Equals(d.County, county, StringComparison.OrdinalIgnoreCase));
            if (countyRow == null) throw new DataNotFoundException($"County {county} not available in data set.", nameof(county));

            var orderedCasesInRange = countyRow.ConfirmedCases.Where(kvp => range.Contains(kvp.Key)).OrderBy(kvp => kvp.Key);

            var dailyRateOfChange = new List<DailyRateOfChange>();

            //TODO: Requirement is unclear on what the percentage should be against:
            //e.g. Could be percentage of total population in the region, etc.
            //Using the data we have, going for rate of change against the highest number of cases in the range.
            var maxCasesInRange = orderedCasesInRange.Max(kvp => kvp.Value);
            var normalizationFactor = 100.0 / maxCasesInRange;

            var prevCases = 0;
            foreach (var dailyCases in orderedCasesInRange)
            {
                var totalChange = dailyCases.Value - prevCases;
                var percentChange = Math.Round(totalChange * normalizationFactor, 1);
                var change = new DailyRateOfChange(dailyCases.Key, totalChange, percentChange);
                prevCases = dailyCases.Value;
                dailyRateOfChange.Add(change);
            }

            return new RateOfChangeResult(county, countyRow.Latitude, countyRow.Longitude, dailyRateOfChange);
        }

        public async Task<RateOfChangeResult> GetRateOfChangeByState(string state, DateRange range)
        {
            var data = await CovidData();

            var stateRows = data.Where(d => string.Equals(d.ProvinceState, state, StringComparison.OrdinalIgnoreCase));
            if (!stateRows.Any()) throw new DataNotFoundException($"State {state} not available in data set", nameof(state));

            var stateTotalsInRange = stateRows.Aggregate(new Dictionary<DateTime, int>(), (dict, countyRow) =>
            {
                var casesInRange = countyRow.ConfirmedCases.Where(kvp => range.Contains(kvp.Key));

                foreach (var cases in casesInRange)
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

            var orderedCasesInRange = stateTotalsInRange.OrderBy(kvp => kvp.Key);

            var dailyRateOfChange = new List<DailyRateOfChange>();

            //TODO: Requirement is unclear on what the percentage should be against:
            //e.g. Could be percentage of total population in the region, etc.
            //Using the data we have, going for rate of change against the highest number of cases in the range.
            var maxCasesInRange = orderedCasesInRange.Max(kvp => kvp.Value);
            var normalizationFactor = 100.0 / maxCasesInRange;

            var prevCases = 0;
            foreach (var dailyCases in orderedCasesInRange)
            {
                var totalChange = dailyCases.Value - prevCases;
                var percentChange = Math.Round(totalChange * normalizationFactor, 1);
                var change = new DailyRateOfChange(dailyCases.Key, totalChange, percentChange);
                prevCases = dailyCases.Value;
                dailyRateOfChange.Add(change);
            }

            return new RateOfChangeResult(state, string.Empty, string.Empty, dailyRateOfChange);
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
