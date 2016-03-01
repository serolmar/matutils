namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Matrix simétrica representada por um vector de bits.
    /// </summary>
    public class BitArraySymmetricMatrix : ISquareMathMatrix<bool>
    {
        /// <summary>
        /// O valor por defeito.
        /// </summary>
        private bool defaultValue;

        /// <summary>
        /// Os elementos.
        /// </summary>
        private BitArray[] elements;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BitArraySymmetricMatrix"/>.
        /// </summary>
        /// <param name="lines">O número de linhas da matriz.</param>
        /// <param name="columns">O número de colunas da matriz.</param>
        public BitArraySymmetricMatrix(int lines, int columns)
            : this(lines, columns, false)
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BitArraySymmetricMatrix"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public BitArraySymmetricMatrix(int lines, int columns, bool defaultValue)
        {
            if (lines < 0)
            {
                throw new ArgumentException("Parameter lines must be non-negative.");
            }
            else if (columns < 0)
            {
                throw new ArgumentException("Parameter columns must be non-negative.");
            }
            else
            {
                this.defaultValue = defaultValue;
                var lastLine = lines;
                this.elements = new BitArray[lines];
                for (int i = 0; i < lines; ++i)
                {
                    this.elements[i] = new BitArray(lastLine--, defaultValue);
                }
            }
        }

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for inferior a zero não for inferior ao tamanho da dimensão.
        /// </exception>
        public bool this[int line, int column]
        {
            get
            {
                if (line < 0 || column < 0)
                {
                    throw new IndexOutOfRangeException(
                        "Parameters line and column must be non-negative and less than the size of matrix.");
                }
                else if (this.elements.Length == 0)
                {
                    throw new IndexOutOfRangeException(
                        "Parameters line and column must be non-negative and less than the size of matrix.");
                }
                else
                {
                    var innerLine = line;
                    var innerColumn = column;
                    if (column < line)
                    {
                        innerLine = column;
                        innerColumn = line;
                    }

                    if (innerColumn >= this.elements.Length)
                    {
                        throw new IndexOutOfRangeException(
                        "Parameters line and column must be non-negative and less than the size of matrix.");
                    }

                    var columnIndex = innerColumn - innerLine;
                    return this.elements[innerLine][columnIndex];
                }
            }
            set
            {
                if (line < 0 || column < 0)
                {
                    throw new IndexOutOfRangeException(
                        "Parameters line and column must be non-negative and less than the size of matrix.");
                }
                else if (this.elements.Length == 0)
                {
                    throw new IndexOutOfRangeException(
                        "Parameters line and column must be non-negative and less than the size of matrix.");
                }
                else
                {
                    var innerLine = line;
                    var innerColumn = column;
                    if (column < line)
                    {
                        innerLine = column;
                        innerColumn = line;
                    }

                    if (innerColumn >= this.elements.Length)
                    {
                        throw new IndexOutOfRangeException(
                        "Parameters line and column must be non-negative and less than the size of matrix.");
                    }

                    var columnIndex = innerColumn - innerLine;
                    this.elements[innerLine][columnIndex] = value;
                }
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a matriz é simétrica.
        /// </summary>
        /// <remarks>
        /// A matriz é simétrica independentemente comparador que é passado como argumento.
        /// </remarks>
        /// <param name="equalityComparer">O comparador de elementos.</param>
        /// <returns>Verdadeiro caso a matriz seja simétrica e falso caso contrário.</returns>
        public bool IsSymmetric(IEqualityComparer<bool> equalityComparer)
        {
            return true;
        }

        /// <summary>
        /// Determina o número de linhas ou colunas conforme o valor da dimensão seja 0 ou 1.
        /// </summary>
        /// <param name="dimension">A dimensão.</param>
        /// <returns>O número de linhas ou colunas.</returns>
        public int GetLength(int dimension)
        {
            if (dimension < 0 || dimension > 1)
            {
                throw new IndexOutOfRangeException(
                    "Parameter dimension must be non-negative and less than the size of matrix.");
            }
            else
            {
                return this.elements.Length;
            }
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<bool> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<bool>(this, lines, columns);
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<bool> GetSubMatrix(IntegerSequence lines, IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<bool>(this, lines, columns);
        }

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <remarks>
        /// Esta função não é aplicável em matrizes simétricas.
        /// </remarks>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        /// <exception cref="MathematicsException">
        /// As matrizes simétricas não suportam a troca de linhas.
        /// </exception>
        public void SwapLines(int i, int j)
        {
            throw new MathematicsException("Can't swap the lines of a symmetric matrix.");
        }

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <remarks>
        /// Esta função não é aplicável em matrizes simétricas.
        /// </remarks>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        /// <exception cref="MathematicsException">
        /// As matrizes simétricas não suportam a troca de colunas.
        /// </exception>
        public void SwapColumns(int i, int j)
        {
            throw new MathematicsException("Can't swap the columns of a symmetric matrix.");
        }

        /// <summary>
        /// Multiplica os valores da linha pelo escalar definido.
        /// </summary>
        /// <param name="line">A linha a ser considerada.</param>
        /// <param name="scalar">O escalar a ser multiplicado.</param>
        /// <param name="ring">O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.</param>
        /// <exception cref="MathematicsException">
        /// A operação não é suportada pelas matrizes simétricas.
        /// </exception>
        public void ScalarLineMultiplication(int line, bool scalar, IRing<bool> ring)
        {
            throw new MathematicsException("Can't multiply the lines of a symmetric matrix.");
        }

        /// <summary>
        /// Substitui a linha especificada por uma combinação linear desta com uma outra. Por exemplo, li = a * li + b * lj, isto é,
        /// a linha i é substituída pela soma do produto de a pela linha i com o produto de b peloa linha j.
        /// </summary>
        /// <param name="i">A linha a ser substituída.</param>
        /// <param name="j">A linha a ser combinada.</param>
        /// <param name="a">O escalar a ser multiplicado pela primeira linha.</param>
        /// <param name="b">O escalar a ser multiplicado pela segunda linha.</param>
        /// <param name="ring">O objecto responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="MathematicsException">
        /// A operação não é suportada pelas matrizes simétricas.
        /// </exception>
        public void CombineLines(int i, int j, bool a, bool b, IRing<bool> ring)
        {
            throw new MathematicsException("Can't combine the lines of a symmetric matrix.");
        }

        /// <summary>
        /// Obtém um enumearador para todos os elementos da matriz.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<bool> GetEnumerator()
        {
            for (int i = 0; i < this.elements.Length; ++i)
            {
                for (int j = 0; j < i; ++j)
                {
                    var columnLine = this.elements[j];
                    yield return columnLine[i-j];
                }

                var currentLine = this.elements[i];
                for (int j = i; j < currentLine.Length; ++j)
                {
                    yield return currentLine[j];
                }
            }
        }

        /// <summary>
        /// Obtém o enumerador não genércio para a matriz.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
