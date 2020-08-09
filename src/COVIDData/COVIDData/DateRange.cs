using System;
using System.Collections.Generic;
using System.Text;

namespace COVIDData
{
    public class DateRange
    {
        
        public DateRange(DateTime? startDate, DateTime? endDate)
        {
            StartDate = (startDate ?? DateTime.MinValue).Date;
            EndDate = (endDate ?? DateTime.MaxValue).Date;

            if (TotalDays <= 0) throw new DatesOutOfRangeException($"{nameof(startDate)} must be earlier than {nameof(endDate)}");
        }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public double TotalDays
        {
            get
            {
                return (EndDate - StartDate).TotalDays;
            }
        }

        /// <summary>
        /// Inclusive Includes
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <returns></returns>
        public bool Contains(DateTime dateToCheck)
        {
            return dateToCheck.Date >= StartDate && dateToCheck.Date <= EndDate;
        }

    }
}
