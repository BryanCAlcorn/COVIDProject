using COVIDData.Interfaces;
using COVIDData.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace COVIDData
{
    public class CovidDataSource : ICovidDataSource
    {
        private const string DataURL = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_US.csv";

        private readonly HttpClient _dataClient = new HttpClient();

        private IList<CovidDataRow> _cachedCovidData;
        private DateTime _fetchDate;

        public async Task<IList<CovidDataRow>> GetData()
        {
            if (_cachedCovidData == null ||
                //Re-Fetch cached data every day.
                (DateTime.Now - _fetchDate).TotalDays > 1)
            {
                _cachedCovidData = await GetDataFromSource();
                _fetchDate = DateTime.Now;
            }

            return _cachedCovidData;
        }

        private async Task<IList<CovidDataRow>> GetDataFromSource()
        {
            var parsedRows = new List<CovidDataRow>();

            using (var dataResponse = await _dataClient.GetAsync(DataURL))
            using (var dataStream = await dataResponse.Content.ReadAsStreamAsync())
            using (var parser = new TextFieldParser(dataStream))
            {
                //Manually Parse the CSV...
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                var header = parser.ReadFields();
                while (!parser.EndOfData)
                {
                    var line = parser.ReadFields();

                    var dataRow = new CovidDataRow(header, line);
                    parsedRows.Add(dataRow);
                }
            }

            return parsedRows;
        }
    }
}
