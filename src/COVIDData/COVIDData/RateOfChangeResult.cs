using System;
using System.Collections.Generic;
using System.Text;

namespace COVIDData
{
    public class RateOfChangeResult
    {
        public RateOfChangeResult(string location, string latitude, string longitude,
            IReadOnlyList<DailyRateOfChange> dailyRateOfChanges)
        {
            Location = location;
            Latitude = latitude;
            Longitude = longitude;
            DailyRateOfChange = dailyRateOfChanges;
        }

        public string Location { get; }

        public string Latitude { get; }

        public string Longitude { get; }

        IReadOnlyList<DailyRateOfChange> DailyRateOfChange { get; }
    }
}
