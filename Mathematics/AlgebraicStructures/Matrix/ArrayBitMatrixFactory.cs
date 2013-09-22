namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ArrayBitMatrixFactory : IMatrixFactory<int>
    {
        public IMatrix<int> CreateMatrix(int lines, int columns)
        {
            return new ArrayBitMatrix(lines, columns);
        }

        public IMatrix<int> CreateMatrix(int lines, int columns, int defaultValue)
        {
            return new ArrayBitMatrix(lines, columns, defaultValue);
        }
    }
}
