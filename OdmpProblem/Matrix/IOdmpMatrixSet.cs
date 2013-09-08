using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdmpProblem
{
    public interface IOdmpMatrixSet<ComponentType, LineType, ColumnType, out T> : 
        IEnumerable<IOdmpMatrix<ComponentType, LineType, ColumnType, T>>
    {
        IOdmpMatrix<ComponentType, LineType, ColumnType, T> this[ComponentType componentIndex]{get;}

        int Count { get; }

        void Unset(ComponentType componentIndex);

        void Clear();
    }
}
