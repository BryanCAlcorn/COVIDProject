using FileHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace COVIDData
{
    public class CovidDataRow
    {
        public CovidDataRow(string[] header, string[] data)
        {
            if (header.Length != data.Length) throw new ArgumentException("Header and data not same length");

            for(int i = 0; i < header.Length; i++)
            {

            }
        }

        public long UID { get; set; }

        public string ISO2 { get; set; }

        public string ISO3 { get; set; }

        public int Code3 { get; set; }

        public double FIPS { get; set; }

        public string Admin2 { get; set; }

        public string ProvinceState { get; set; }

        public string CountryRegion { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string CombinedKey { get; set; }

        //[FieldConverter()]
        public IDictionary<DateTime, int> ConfirmedCases { get; set; }
    }
}
