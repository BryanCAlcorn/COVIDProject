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
        private const string DataURL = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_US.csv";

        private readonly HttpClient _dataClient = new HttpClient();

        private IList<CovidDataRow> _covidData;

        public CovidDataRepository()
        {
        }

        public async Task<IList<CovidDataRow>> QueryByCounty(string county, DateRange range)
        {
            var data = await CovidData();
            var filtered = data.Where(d => string.Equals(d.County, county, StringComparison.OrdinalIgnoreCase));

            var countyCases = filtered.SelectMany(d => d.ConfirmedCases.Where(kvp => range.Contains(kvp.Key)));


        }

        private async Task<IList<CovidDataRow>> CovidData()
        {
            if (_covidData == null)
            {
                _covidData = await GetData();
            }
            return _covidData;
        }

        private async Task<IList<CovidDataRow>> GetData()
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
