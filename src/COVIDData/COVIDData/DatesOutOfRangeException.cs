using System;

namespace COVIDData
{
    public class DatesOutOfRangeException : ArgumentException
    {
        public DatesOutOfRangeException(string message) 
            : base(message, "Dates")
        {
        }
    }
}
