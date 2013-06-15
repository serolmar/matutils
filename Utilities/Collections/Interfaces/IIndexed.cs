using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    public interface IIndexed<in T, out P>
    {
        int Count { get; }
        P this[T index] { get; }
    }
}
