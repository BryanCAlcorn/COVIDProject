﻿using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace COVIDData
{
    public class CovidDataRow
    {
        public CovidDataRow(string[] header, string[] data)
        {
            if (header.Length != data.Length) throw new ArgumentException("Header and data not same length");

            ConfirmedCases = new Dictionary<DateTime, int>();
            /*
             * UID,iso2,iso3,code3,FIPS,Admin2,Province_State,Country_Region,Lat,Long_,Combined_Key,
             * 1/22/20,1/23/20,1/24/20,1/25/20,1/26/20,1/27/20,1/28/20,1/29/20,1/30/20,1/31/20,2/1/20,2/2/20,2/3/20,2/4/20,2/5/20,2/6/20,
             * 2/7/20,2/8/20,2/9/20,2/10/20,2/11/20,2/12/20,2/13/20,2/14/20,2/15/20,2/16/20,2/17/20,2/18/20,2/19/20,2/20/20,2/21/20,2/22/20,
             * 2/23/20,2/24/20,2/25/20,2/26/20,2/27/20,2/28/20,2/29/20,3/1/20,3/2/20,3/3/20,3/4/20,3/5/20,3/6/20,3/7/20,3/8/20,3/9/20,3/10/20,
             * 3/11/20,3/12/20,3/13/20,3/14/20,3/15/20,3/16/20,3/17/20,3/18/20,3/19/20,3/20/20,3/21/20,3/22/20,3/23/20,3/24/20,3/25/20,3/26/20,
             * 3/27/20,3/28/20,3/29/20,3/30/20,3/31/20,4/1/20,4/2/20,4/3/20,4/4/20,4/5/20,4/6/20,4/7/20,4/8/20,4/9/20,4/10/20,4/11/20,4/12/20,
             * 4/13/20,4/14/20,4/15/20,4/16/20,4/17/20,4/18/20,4/19/20,4/20/20,4/21/20,4/22/20,4/23/20,4/24/20,4/25/20,4/26/20,4/27/20,4/28/20,
             * 4/29/20,4/30/20,5/1/20,5/2/20,5/3/20,5/4/20,5/5/20,5/6/20,5/7/20,5/8/20,5/9/20,5/10/20,5/11/20,5/12/20,5/13/20,5/14/20,5/15/20,
             * 5/16/20,5/17/20,5/18/20,5/19/20,5/20/20,5/21/20,5/22/20,5/23/20,5/24/20,5/25/20,5/26/20,5/27/20,5/28/20,5/29/20,5/30/20,5/31/20,
             * 6/1/20,6/2/20,6/3/20,6/4/20,6/5/20,6/6/20,6/7/20,6/8/20,6/9/20,6/10/20,6/11/20,6/12/20,6/13/20,6/14/20,6/15/20,6/16/20,6/17/20,
             * 6/18/20,6/19/20,6/20/20,6/21/20,6/22/20,6/23/20,6/24/20,6/25/20,6/26/20,6/27/20,6/28/20,6/29/20,6/30/20,7/1/20,7/2/20,7/3/20,
             * 7/4/20,7/5/20,7/6/20,7/7/20,7/8/20,7/9/20,7/10/20,7/11/20,7/12/20,7/13/20,7/14/20,7/15/20,7/16/20,7/17/20,7/18/20,7/19/20,7/20/20,
             * 7/21/20,7/22/20,7/23/20,7/24/20,7/25/20,7/26/20,7/27/20,7/28/20,7/29/20,7/30/20,7/31/20,8/1/20,8/2/20,8/3/20,8/4/20,8/5/20,8/6/20,
             * 8/7/20
             */
            for (int i = 0; i < header.Length; i++)
            {
                //*UID,iso2,iso3,code3,FIPS,Admin2,Province_State,Country_Region,Lat,Long_,Combined_Key,
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
                    Code3 = Convert.ToInt32(data[i]);
                }
                else if (string.Equals(header[i], nameof(FIPS), StringComparison.OrdinalIgnoreCase))
                {
                    FIPS = Convert.ToDouble(data[i]);
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
                else if(DateTime.TryParse(header[i], out var dt))
                {
                    ConfirmedCases[dt] = Convert.ToInt32(data[i]);
                }
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
