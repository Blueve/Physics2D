using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Collision
{
    public enum ContactType
    {
        CircleAndCircle,
        CircleAndEdge,
        CircleAndBox,
        EdgeAndEdge,
        EdgeAndBox,
        BoxAndBox
    }
}
