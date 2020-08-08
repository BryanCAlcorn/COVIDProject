using System;
using System.Collections.Generic;
using System.Text;

namespace COVIDData
{
    public class DateRange
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public DateRange(DateTime? startDate, DateTime? endDate)
        {
            _startDate = (startDate ?? DateTime.MinValue).Date;
            _endDate = (endDate ?? DateTime.MaxValue).Date;
        }

        public double TotalDays
        {
            get
            {
                return (_endDate - _startDate).TotalDays;
            }
        }

        /// <summary>
        /// Inclusive Includes
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <returns></returns>
        public bool Contains(DateTime dateToCheck)
        {
            return dateToCheck.Date >= _startDate && dateToCheck.Date <= _endDate;
        }

    }
}
