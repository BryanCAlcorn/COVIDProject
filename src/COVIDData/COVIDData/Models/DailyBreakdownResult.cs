using System;
using System.Collections.Generic;
using System.Text;

namespace COVIDData.Models
{
    public class DailyBreakdownResult
    {
        public DailyBreakdownResult(string location, string latitude, string longitude, 
            IReadOnlyList<DailyChange> dailyChanges)
        {
            Location = location;
            Latitude = latitude;
            Longitude = longitude;
            DailyChanges = dailyChanges;
        }

        public string Location { get; }

        public string Latitude { get; }

        public string Longitude { get; }

        public IReadOnlyList<DailyChange> DailyChanges { get; }
    }
}
