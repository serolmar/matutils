using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IMatrixRow<LineType, ColumnType, out T> : IEnumerable<IMatrixColumn<ColumnType, T>>
    {
        IMatrixColumn<ColumnType, T> this[ColumnType columnIndex] { get; }
        bool ContainsColumn(ColumnType index);
        LineType Line { get; }
    }
}
