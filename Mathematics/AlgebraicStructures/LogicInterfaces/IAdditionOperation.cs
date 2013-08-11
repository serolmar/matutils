using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IAdditionOperation<T>
    {
        T Add(T left, T right);
    }
}
