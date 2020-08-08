using System;
using System.Collections.Generic;

namespace COVIDData
{
    public class CovidDataRow
    {
        public CovidDataRow(string[] header, string[] data)
        {
            if (header.Length != data.Length) throw new ArgumentException("Header and data not same length");

            ConfirmedCases = new Dictionary<DateTime, int>();
            for (int i = 0; i < header.Length; i++)
            {
                try
                {
                    //Non-Date Headers:
                    //UID,iso2,iso3,code3,FIPS,Admin2,Province_State,Country_Region,Lat,Long_,Combined_Key,
                    if (string.Equals(header[i], nameof(UID), StringComparison.OrdinalIgnoreCase))
                    {
                        UID = Convert.ToInt64(data[i]);
                    }
                    else if (string.Equals(header[i], nameof(ISO2), StringComparison.OrdinalIgnoreCase))
                    {
                        ISO2 = data[i];
                    }
                    else if (string.Equals(header[i], nameof(ISO3), StringComparison.OrdinalIgnoreCase))
                    {
                        ISO3 = data[i];
                    }
                    else if (string.Equals(header[i], nameof(Code3), StringComparison.OrdinalIgnoreCase))
                    {
                        if (int.TryParse(data[i], out var code3))
                        {
                            Code3 = code3;
                        }
                    }
                    else if (string.Equals(header[i], nameof(FIPS), StringComparison.OrdinalIgnoreCase))
                    {
                        if (double.TryParse(data[i], out var fips))
                        {
                            FIPS = fips;
                        }
                    }
                    else if (string.Equals(header[i], nameof(Admin2), StringComparison.OrdinalIgnoreCase))
                    {
                        Admin2 = data[i];
                    }
                    else if (string.Equals(header[i], "Province_State", StringComparison.OrdinalIgnoreCase))
                    {
                        ProvinceState = data[i];
                    }
                    else if (string.Equals(header[i], "Country_Region", StringComparison.OrdinalIgnoreCase))
                    {
                        CountryRegion = data[i];
                    }
                    else if (string.Equals(header[i], "Lat", StringComparison.OrdinalIgnoreCase))
                    {
                        Latitude = data[i];
                    }
                    else if (string.Equals(header[i], "Long_", StringComparison.OrdinalIgnoreCase))
                    {
                        Longitude = data[i];
                    }
                    else if (string.Equals(header[i], "Combined_Key", StringComparison.OrdinalIgnoreCase))
                    {
                        CombinedKey = data[i];
                    }
                    else if (DateTime.TryParse(header[i], out var dt))
                    {
                        ConfirmedCases[dt] = Convert.ToInt32(data[i]);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error parsing data: {header[i]} at index {i}, data: {data[i]}.");
                    Console.WriteLine($"Exception: {ex}");
                }
            }
        }

        public long UID { get; set; }

        public string ISO2 { get; set; }

        public string ISO3 { get; set; }

        public int? Code3 { get; set; }

        public double? FIPS { get; set; }

        public string Admin2 { get; set; }

        public string ProvinceState { get; set; }

        public string CountryRegion { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string CombinedKey { get; set; }

        public IDictionary<DateTime, int> ConfirmedCases { get; set; }
    }
}
