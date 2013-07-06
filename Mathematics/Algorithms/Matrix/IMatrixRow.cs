using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IMatrixRow<in Column, out T> : IEnumerable<T>
    {
        T this[Column columnIndex] { get; }
        bool ContainsColumn(Column index);
    }
}
