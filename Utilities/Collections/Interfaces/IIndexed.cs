using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    public interface IIndexed<in T, out P>
    {
        P this[T index] { get; }
        int Count { get; }
    }
}
