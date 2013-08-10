using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class ArrayMatrixFactory<ObjectType> : IMatrixFactory<ObjectType>
    {
        public IMatrix<ObjectType> CreateMatrix(int lines, int columns, ObjectType defaultValue)
        {
            return new ArrayMatrix<ObjectType>(lines, columns, defaultValue);
        }
    }
}
