using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IMatrix<CoordType, ObjectType>
    {
        ObjectType this[CoordType line, CoordType column] { get; set; }
    }
}
