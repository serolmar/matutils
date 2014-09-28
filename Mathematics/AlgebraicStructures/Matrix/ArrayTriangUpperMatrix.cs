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
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException("Paramter line is out of range.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException("Paramter column is out of range.");
                }
                else if (column < line)
                {
                    return this.defaultValue;
                }
                else
                {
                    return this.elements[column][line];
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
                else if (column < line)
                {
                    throw new MathematicsException("Can't set the lower terms in an upper triangular matrix.");
                }
                else
                {
                    this.elements[column][line] = value;
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
                var length = this.elements.Length ;
                for (int i = line; i < length; ++i)
                {
                    var currentLine = this.elements[i];
                    currentLine[line] = ring.Multiply(currentLine[line], scalar);
                }
            }
        }
    }
}
