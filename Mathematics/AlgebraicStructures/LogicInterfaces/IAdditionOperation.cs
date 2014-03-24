using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IAdditionOperation<in P, in Q, out T>
    {
        T Add(P left, Q right);
    }
}
