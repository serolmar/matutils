namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma matriz triangular inferior.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas das matrizes.</typeparam>
    public class ArrayTriangLowerMatrix<CoeffType> : AArrayTriangularMatrix<CoeffType>
    {
        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayTriangLowerMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        public ArrayTriangLowerMatrix(int dimension)
            : base(dimension)
        {
        }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayTriangLowerMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public ArrayTriangLowerMatrix(int dimension, CoeffType defaultValue)
            : base(dimension, defaultValue)
        {
        }

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        /// <exception cref="MathematicsException">
        /// Se o índice da coluna for inferior ao índice da linha na atribuição do valor.
        /// </exception>
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
                if (line < column)
                {
                    throw new MathematicsException("Can't set the upper terms in an lower triangular matrix.");
                }
                else
                {
                    base[column, line] = value;
                }
            }
        }
    }
}
