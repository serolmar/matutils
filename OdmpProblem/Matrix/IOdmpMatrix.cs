using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdmpProblem
{
    public interface IOdmpMatrix<ComponentType, LineType, ColumnType, out T> : IEnumerable<IOdmpMatrixRow<LineType, ColumnType, T>>
    {
        IOdmpMatrixRow<LineType, ColumnType, T> this[LineType line] { get; }

        T this[LineType line, ColumnType column] { get; }

        int Count { get; }

        ComponentType Component { get; }

        bool ContainsLine(LineType line);

        bool ContainsColumn(LineType line, ColumnType column);
    }
}
