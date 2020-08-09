using System;

namespace COVIDData.Exceptions
{
    public class DataNotFoundException : ArgumentException
    {
        public DataNotFoundException(string message, string argument): base(message, argument)
        {
        }
    }
}
