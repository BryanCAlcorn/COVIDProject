using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace COVIDData
{
    public class CovidDataFetcher
    {
        private const string DataURL = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_US.csv";

        private readonly HttpClient _dataClient = new HttpClient();

        public CovidDataFetcher()
        {
        }

        

        public async Task<IList<CovidDataRow>> GetData()
        {
            var parsedRows = new List<CovidDataRow>();

            var dataResponse = await _dataClient.GetAsync(DataURL);
            var dataStream = await dataResponse.Content.ReadAsStreamAsync();

            //Manually Parsing the CSV...
            using (var parser = new TextFieldParser(dataStream))
            {
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
