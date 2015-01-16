using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Common.Exceptions
{
    public class PointNotFoundException : Exception
    {
        public PointNotFoundException(string message)
            : base(message)
        { }
    }
}
