using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace COVIDData
{
    public class CovidDataRow
    {
        public CovidDataRow(string[] header, string[] data)
        {
            if (header.Length != data.Length) throw new ArgumentException("Header and data not same length");

            var caseTotals = new Dictionary<DateTime, int>();

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
                    else if (string.Equals(header[i], "FIPS", StringComparison.OrdinalIgnoreCase))
                    {
                        if (double.TryParse(data[i], out var fips))
                        {
                            FIPS = fips;
                        }
                    }
                    else if (string.Equals(header[i], "Admin2", StringComparison.OrdinalIgnoreCase))
                    {
                        County = data[i];
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
                        caseTotals[dt] = Convert.ToInt32(data[i]);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error parsing data: {header[i]} at index {i}, data: {data[i]}.");
                    Console.WriteLine($"Exception: {ex}");
                    throw;
                }
            }

            ConfirmedCases = caseTotals;
        }

        public long UID { get; }

        public string ISO2 { get; }

        public string ISO3 { get; }

        public int? Code3 { get; }

        public double? FIPS { get; }

        public string County { get; }

        public string ProvinceState { get; }

        public string CountryRegion { get; }

        public string Latitude { get; }

        public string Longitude { get; }

        public string CombinedKey { get; }

        public IReadOnlyDictionary<DateTime, int> ConfirmedCases { get; }
    }
}
