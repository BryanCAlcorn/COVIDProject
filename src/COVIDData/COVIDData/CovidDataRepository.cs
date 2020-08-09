﻿using System;
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

            var stateTotals = stateRows.Aggregate(new Dictionary<DateTime, int>(), (dict, countyRow) =>
            {
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
