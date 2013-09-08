using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdmpProblem
{
    public interface IOdmpMatrixRow<LineType, ColumnType, out T> : IEnumerable<IOdmpMatrixColumn<ColumnType, T>>
    {
        IOdmpMatrixColumn<ColumnType, T> this[ColumnType columnIndex] { get; }

        LineType Line { get; }

        int Count { get; }

        bool ContainsColumn(ColumnType index);
    }
}
