using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IMatrix<in Line, in Column, out T> : IEnumerable<IMatrixRow<Column, T>>
    {
        IMatrixRow<Column, T> this[Line line] { get; }

        T this[Line line, Column column] { get; }

        bool ContainsLine(Line line);

        bool ContainsColumn(Line line, Column column);
    }
}
