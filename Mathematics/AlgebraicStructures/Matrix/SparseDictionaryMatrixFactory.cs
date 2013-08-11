using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class SparseDictionaryMatrixFactory<ObjectType> : IMatrixFactory<ObjectType>
    {
        public IMatrix<ObjectType> CreateMatrix(int lines, int columns)
        {
            return new SparseDictionaryMatrix<ObjectType>(lines, columns);
        }

        public IMatrix<ObjectType> CreateMatrix(int lines, int columns, ObjectType defaultValue)
        {
            return new SparseDictionaryMatrix<ObjectType>(lines, columns, defaultValue);
        }
    }
}
