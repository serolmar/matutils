namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma matriz triangular inferior.
    /// </summary>
    public class ArrayTriangLowerMatrix<CoeffType> : AArrayTriangularMatrix<CoeffType>
    {
        public ArrayTriangLowerMatrix(int dimension)
            : base(dimension)
        {
        }

        public ArrayTriangLowerMatrix(int dimension, CoeffType defaultValue)
            : base(dimension, defaultValue)
        {
        }

        public override CoeffType this[int line, int column]
        {
            get
            {
                if (column < line)
                {
                    return this.defaultValue;
                }
                else
                {
                    return base[column, line];
                }
            }
            set
            {
                if (column < line)
                {
                    throw new MathematicsException("Can't set the lower terms in an umpper triangular matrix.");
                }
                else
                {
                    base[column, line] = value;
                }
            }
        }
    }
}
