using System;

namespace COVIDData.Models
{
    public class DailyChange
    {
        public DailyChange(DateTime date, int totalCases, int newCases)
        {
            Date = date;
            TotalCases = totalCases;
            NewCases = newCases;
        }

        public DateTime Date { get; }

        public int TotalCases { get; }

        public int NewCases { get; }
    }
}
