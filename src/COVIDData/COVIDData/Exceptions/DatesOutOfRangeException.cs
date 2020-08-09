using System;

namespace COVIDData.Exceptions
{
    public class DatesOutOfRangeException : ArgumentException
    {
        public DatesOutOfRangeException(string message) 
            : base(message, "Dates")
        {
        }
    }
}
