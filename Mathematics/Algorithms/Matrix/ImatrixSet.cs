using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface ImatrixSet<ComponentType, LineType, ColumnType, out T> : 
        IEnumerable<IMatrix<ComponentType, LineType, ColumnType, T>>
    {
        IMatrix<ComponentType, LineType, ColumnType, T> this[ComponentType componentIndex]{get;}

        void Unset(ComponentType componentIndex);

        void Clear();
    }
}
