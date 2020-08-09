using System;
using System.Collections.Generic;
using System.Text;

namespace COVIDData
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
