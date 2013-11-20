namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um matriz triangular superior.
    /// </summary>
    public class ArrayTriangUpperMatrix<CoeffType> : AArrayTriangularMatrix<CoeffType>
    {
        public ArrayTriangUpperMatrix(int dimension)
            : base(dimension)
        {
        }

        public ArrayTriangUpperMatrix(int dimension, CoeffType defaultValue)
            : base(dimension, defaultValue)
        {
        }

        public override CoeffType this[int line, int column]
        {
            get
            {
                if (line < column)
                {
                    return this.defaultValue;
                }
                else
                {
                    return base[line, column];
                }
            }
            set
            {
                if (line < column)
                {
                    throw new MathematicsException("Can't set the lower terms in an umpper triangular matrix.");
                }
                else
                {
                    base[line, column] = value;
                }
            }
        }
    }
}
