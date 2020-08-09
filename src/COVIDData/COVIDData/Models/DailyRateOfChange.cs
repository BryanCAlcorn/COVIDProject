using System;

namespace COVIDData.Models
{
    public class DailyRateOfChange
    {
        public DailyRateOfChange(DateTime date, int newCases, double percentChange)
        {
            Date = date;
            NewCases = newCases;
            PercentChange = percentChange;
        }

        public DateTime Date { get; }

        public int NewCases { get; }

        public double PercentChange { get; }
    }
}
