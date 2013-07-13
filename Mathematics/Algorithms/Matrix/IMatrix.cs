using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IMatrix<ComponentType, LineType, ColumnType, out T> : IEnumerable<IMatrixRow<LineType, ColumnType, T>>
    {
        IMatrixRow<LineType, ColumnType, T> this[LineType line] { get; }

        T this[LineType line, ColumnType column] { get; }

        int Count { get; }

        ComponentType Component { get; }

        bool ContainsLine(LineType line);

        bool ContainsColumn(LineType line, ColumnType column);
    }
}
