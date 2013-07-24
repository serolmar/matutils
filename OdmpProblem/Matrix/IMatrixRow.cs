using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdmpProblem
{
    public interface IMatrixRow<LineType, ColumnType, out T> : IEnumerable<IMatrixColumn<ColumnType, T>>
    {
        IMatrixColumn<ColumnType, T> this[ColumnType columnIndex] { get; }

        LineType Line { get; }

        int Count { get; }

        bool ContainsColumn(ColumnType index);
    }
}
