using System;

namespace COVIDData
{
    public class CovidQueryResult
    {
        public CovidQueryResult(string location, string latitute, string longitude,
            double avgDailyCases, int minCaseCount, DateTime minCaseDate, int maxCaseCount, DateTime maxCaseDate)
        {
            Location = location;
            Latitude = latitute;
            Longitude = longitude;
            AverageDailyCases = avgDailyCases;
            MinimumCaseCount = minCaseCount;
            MinimumCaseDate = minCaseDate;
            MaximumCaseCount = maxCaseCount;
            MaximumCaseDate = maxCaseDate;
        }

        public string Location { get; }

        public string Latitude { get; }

        public string Longitude { get; }

        public double AverageDailyCases { get; }

        public int MinimumCaseCount { get; }

        public DateTime MinimumCaseDate { get; }

        public int MaximumCaseCount { get; }

        public DateTime MaximumCaseDate { get; }
    }
}
