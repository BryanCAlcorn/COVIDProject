using System;
using System.Collections.Generic;
using System.Text;

namespace COVIDData
{
    public class DataNotFoundException : ArgumentException
    {
        public DataNotFoundException(string message, string argument): base(message, argument)
        {
        }
    }
}
