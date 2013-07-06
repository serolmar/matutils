using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface ImatrixSet<in ComponentIndex, in Line, in Column, out T> : 
        IEnumerable<IMatrix<Line, Column, T>>
    {
        IMatrix<Line, Column, T> this[ComponentIndex componentIndex]{get;}

        void Unset(ComponentIndex componentIndex);

        void Clear();
    }
}
