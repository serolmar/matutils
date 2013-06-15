using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IMatrix<in Line, in Column, out RowNumber, out T> : IEnumerable<IMatrixRow<Column, RowNumber, T>>
        where Line : RowNumber
    {
        IMatrixRow<Column, RowNumber, T> this[Line line] { get; }

        T this[Line line, Column column] { get; }

        int GetLength(int dimension);

        bool ContainsLine(Line line);

        bool ContainsColumn(Line line, Column column);
    }
}
