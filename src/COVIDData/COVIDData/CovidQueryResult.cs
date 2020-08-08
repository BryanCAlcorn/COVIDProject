using System;

namespace COVIDData
{
    public class CovidQueryResult
    {
        public string County { get; }

        public string State { get; }

        public string Latitude { get; }

        public string Longitude { get; }

        public double AverageDailyCases { get; }

        public int MinimumCaseCount { get; }

        public DateTime MinimumCaseDate { get; }

        public int MaximumCaseCount { get; }

        public DateTime MaximumCaseDate { get; }
    }
}
