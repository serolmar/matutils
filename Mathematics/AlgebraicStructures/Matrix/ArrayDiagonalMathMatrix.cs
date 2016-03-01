namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Matriz cujos elementos na diagonal são suportados por um vector.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem as entradas da matriz.</typeparam>
    class ArrayDiagonalMathMatrix<CoeffType> : ISquareMathMatrix<CoeffType>
    {
        /// <summary>
        /// O valor por defeito.
        /// </summary>
        private CoeffType defaultValue;

        /// <summary>
        /// O conjunto dos elementos na diagonal.
        /// </summary>
        private CoeffType[] diagonalElements;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayDiagonalMathMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        public ArrayDiagonalMathMatrix(int dimension)
        {
            if (dimension < 0)
            {
                throw new ArgumentException("Parameter dimension must be non-negative.");
            }
            else
            {
                this.diagonalElements = new CoeffType[dimension];
            }
        }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayDiagonalMathMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public ArrayDiagonalMathMatrix(int dimension, CoeffType defaultValue)
        {
            if (dimension < 0)
            {
                throw new ArgumentException("Parameter dimension must be non-negative.");
            }
            else
            {
                this.diagonalElements = new CoeffType[dimension];
                for (int i = 0; i < dimension; ++i)
                {
                    this.diagonalElements[i] = defaultValue;
                }

                this.defaultValue = defaultValue;
            }
        }

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for negativo ou não for inferior ao tamanho da dimensão.
        /// </exception>
        public CoeffType this[int line, int column]
        {
            get
            {
                var dimension = this.diagonalElements.Length;
                if (line < 0 || line >= dimension)
                {
                    throw new ArgumentOutOfRangeException("line");
                }
                else if (column < 0 || column >= dimension)
                {
                    throw new ArgumentOutOfRangeException("column");
                }
                else
                {
                    if (line == column)
                    {
                        return this.diagonalElements[line];
                    }
                    else
                    {
                        return this.defaultValue;
                    }
                }
            }
            set
            {
                var dimension = this.diagonalElements.Length;
                if (line < 0 || line >= dimension)
                {
                    throw new ArgumentOutOfRangeException("line");
                }
                else if (column < 0 || column >= dimension)
                {
                    throw new ArgumentOutOfRangeException("column");
                }
                else
                {
                    if (line == column)
                    {
                        this.diagonalElements[line] = value;
                    }
                    else
                    {
                        throw new MathematicsException("Can't set non-diagonal values in a diagonal matrix.");
                    }
                }
            }
        }

        /// <summary>
        /// Verifica se se trata de uma matriz simétrica.
        /// </summary>
        /// <param name="equalityComparer">O comparador das entradas.</param>
        /// <returns>Verdadeiro caso se trate de uma matriz simétrica e falso no caso contrário.</returns>
        public bool IsSymmetric(IEqualityComparer<CoeffType> equalityComparer)
        {
            return true;
        }

        /// <summary>
        /// Obtém o número de linhas ou colunas da matriz.
        /// </summary>
        /// <param name="dimension">Zero caso seja pretendido o número de linhas e um caso seja pretendido
        /// o número de colunas.
        /// </param>
        /// <returns>O número de entradas na respectiva dimensão.</returns>
        public int GetLength(int dimension)
        {
            return this.diagonalElements.Length;
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<CoeffType> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<CoeffType>(this, lines, columns);
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<CoeffType> GetSubMatrix(IntegerSequence lines, IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<CoeffType>(this, lines, columns);
        }

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        public void SwapLines(int i, int j)
        {
            throw new MathematicsException("Can't swap lines in a diagonal matrix.");
        }

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocaada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        public void SwapColumns(int i, int j)
        {
            throw new MathematicsException("Can't swap columns in a diagonal matrix.");
        }

        /// <summary>
        /// Multiplica os valores da linha pelo escalar definido.
        /// </summary>
        /// <param name="line">A linha a ser considerada.</param>
        /// <param name="scalar">O escalar a ser multiplicado.</param>
        /// <param name="ring">O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.</param>
        public void ScalarLineMultiplication(int line, CoeffType scalar, IRing<CoeffType> ring)
        {
            var dimension = this.diagonalElements.Length;
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (line < 0 || line >= dimension)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else
            {
                this.diagonalElements[line] = ring.Multiply(
                    this.diagonalElements[line],
                    scalar);
            }
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
        public void CombineLines(int i, int j, CoeffType a, CoeffType b, IRing<CoeffType> ring)
        {
            throw new MathematicsException("Can't combine lines in a diagonal matrix.");
        }

        /// <summary>
        /// Obtém o enumerador genérico para a matriz.
        /// </summary>
        /// <returns>O enumerador genérico.</returns>
        public IEnumerator<CoeffType> GetEnumerator()
        {
            var length = this.diagonalElements.Length;
            for (int i = 0; i < length; ++i)
            {
                yield return this.diagonalElements[i];
            }
        }

        /// <summary>
        /// Obtém o enumerador não genércio para a matriz.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
