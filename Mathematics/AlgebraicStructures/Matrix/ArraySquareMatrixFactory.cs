namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite criar uma matriz quadrada baseada num vector de vectores.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de elementos contidos na matriz.</typeparam>
    public class ArraySquareMatrixFactory<ObjectType> : IMatrixFactory<ObjectType>
    {
        public IMatrix<ObjectType> CreateMatrix(int lines, int columns)
        {
            if (lines != columns)
            {
                throw new MathematicsException("The number of lines doesn't match the number of columns.");
            }
            else
            {
                return new ArraySquareMatrix<ObjectType>(lines);
            }
        }

        public IMatrix<ObjectType> CreateMatrix(int lines, int columns, ObjectType defaultValue)
        {
            if (lines != columns)
            {
                throw new MathematicsException("The number of lines doesn't match the number of columns.");
            }
            else
            {
                return new ArraySquareMatrix<ObjectType>(lines, defaultValue);
            }
        }
    }
}
