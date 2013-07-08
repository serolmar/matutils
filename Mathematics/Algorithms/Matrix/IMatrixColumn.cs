using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IMatrixColumn<ColumnType, out T>
    {
        ColumnType Column { get; }

        T Value { get; }
    }
}
