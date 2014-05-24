namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um matriz triangular superior.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem as entradas das matrizes.</typeparam>
    public class ArrayTriangUpperMatrix<CoeffType> : AArrayTriangularMatrix<CoeffType>
    {
        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayTriangUpperMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        public ArrayTriangUpperMatrix(int dimension)
            : base(dimension)
        {
        }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayTriangUpperMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public ArrayTriangUpperMatrix(int dimension, CoeffType defaultValue)
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
        /// Se o índice da coluna for inferior ao índice da linha.
        /// </exception>
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
                if (column < line)
                {
                    throw new MathematicsException("Can't set the lower terms in an upper triangular matrix.");
                }
                else
                {
                    base[line, column] = value;
                }
            }
        }
    }
}
