using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Common.Exceptions
{
    public class InvalidArgumentException : ArgumentException
    {
        public InvalidArgumentException(string message, string paramName)
            :base(message, paramName)
        { }
    }
}
