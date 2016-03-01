namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma matriz triangular inferior.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem as entradas das matrizes.</typeparam>
    public class ArrayTriangLowerMathMatrix<CoeffType> : AArrayTriangularMathMatrix<CoeffType>
    {
        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayTriangLowerMathMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        public ArrayTriangLowerMathMatrix(int dimension)
            : base(dimension)
        {
        }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayTriangLowerMathMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public ArrayTriangLowerMathMatrix(int dimension, CoeffType defaultValue)
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
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException("Paramter line is out of range.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException("Paramter column is out of range.");
                }
                else if (line < column)
                {
                    return this.defaultValue;
                }
                else
                {
                    return this.elements[line][column];
                }
            }
            set
            {
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException("Paramter line is out of range.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException("Paramter column is out of range.");
                }
                else if (line < column)
                {
                    throw new MathematicsException("Can't set the upper terms in an lower triangular matrix.");
                }
                else
                {
                    this.elements[line][column] = value;
                }
            }
        }

        /// <summary>
        /// Multiplica os valores da linha pelo escalar definido.
        /// </summary>
        /// <param name="line">A linha a ser considerada.</param>
        /// <param name="scalar">O escalar a ser multiplicado.</param>
        /// <param name="ring">O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.</param>
        public override void ScalarLineMultiplication(int line, CoeffType scalar, IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (line < 0 || line >= this.numberOfLines)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else
            {
                var currentLine = this.elements[line];
                for (int i = 0; i < this.numberOfColumns; ++i)
                {
                    currentLine[i] = ring.Multiply(currentLine[i], scalar);
                }
            }
        }

        /// <summary>
        /// Obtém o enumerador genérico para a matriz.
        /// </summary>
        /// <returns>O enumerador genérico.</returns>
        public override IEnumerator<CoeffType> GetEnumerator()
        {
            for (int i = 0; i < this.numberOfLines; ++i)
            {
                for (int j = 0; j <=i; ++j)
                {
                    yield return this.elements[i][j];
                }
            }
        }
    }
}
