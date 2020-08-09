using System.Collections.Generic;

namespace COVIDData.Models
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

        public IReadOnlyList<DailyRateOfChange> DailyRateOfChange { get; }
    }
}
