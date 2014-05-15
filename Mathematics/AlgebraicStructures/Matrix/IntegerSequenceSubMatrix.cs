namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Representa uma matriz especificada por sequências de inteiros ao invés de vectores.
    /// </summary>
    /// <remarks>
    /// A utilização de sequências permite optimizar determinados processos em termos de tempo
    /// e memória utilizada.
    /// </remarks>
    /// <typeparam name="ObjectType">O tipo dos objectos que constituem as entradas da matriz.</typeparam>
    internal class IntegerSequenceSubMatrix<ObjectType> : IMatrix<ObjectType>
    {
        /// <summary>
        /// A matriz.
        /// </summary>
        private IMatrix<ObjectType> matrix;

        /// <summary>
        /// O número de linhas.
        /// </summary>
        private IntegerSequence lines;

        /// <summary>
        /// O número de colunas.
        /// </summary>
        private IntegerSequence columns;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="IntegerSequenceSubMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="matrix">A matriz principal.</param>
        /// <param name="lines">As linhas da matriz que pertencem à sub-matriz.</param>
        /// <param name="columns">As colunas da matriz que pertencem à sub-matriz.</param>
        public IntegerSequenceSubMatrix(IMatrix<ObjectType> matrix, IntegerSequence lines, IntegerSequence columns)
        {
            if (lines == null || columns == null)
            {
                throw new ArgumentException("Parameters lines and columns must be non null.");
            }
            else
            {
                this.matrix = matrix;
                this.lines = lines.Clone();
                this.columns = columns.Clone();
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
        public ObjectType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.lines.Count)
                {
                    throw new IndexOutOfRangeException("Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.columns.Count)
                {
                    throw new IndexOutOfRangeException("Index column must be non-negative and lesser than the number of columns in matrix.");
                }
                else
                {
                    return this.matrix[this.lines[line], this.columns[column]];
                }
            }
            set
            {
                if (line < 0 || line >= this.lines.Count)
                {
                    throw new IndexOutOfRangeException("Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.columns.Count)
                {
                    throw new IndexOutOfRangeException("Index column must be non-negative and lesser than the number of columns in matrix.");
                }
                else
                {
                    this.matrix[this.lines[line], this.columns[column]] = value;
                }
            }
        }

        /// <summary>
        /// Obtém o número de linhas ou colunas da matriz.
        /// </summary>
        /// <param name="dimension">Zero caso seja pretendido o número de linhas e um caso seja pretendido
        /// o número de colunas.
        /// </param>
        /// <returns>O número de entradas na respectiva dimensão.</returns>
        /// <exception cref="ArgumentException">Se o valor da dimensão diferir de zero ou um.</exception>
        public int GetLength(int dimension)
        {
            if (dimension == 0)
            {
                return this.lines.Count;
            }
            else if (dimension == 1)
            {
                return this.columns.Count;
            }
            else
            {
                throw new ArgumentException("Parameter dimension can only take the values 0 or 1.");
            }
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<ObjectType> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<ObjectType>(this, lines, columns);
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<ObjectType> GetSubMatrix(IntegerSequence lines, IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<ObjectType>(this, lines, columns);
        }

        /// <summary>
        /// Não suportado por uma sub-matriz baseada em sequências de inteiros uma vez que estas não
        /// conservam qualquer ordem.
        /// </summary>
        /// <param name="i">A linha a trocar.</param>
        /// <param name="j">A linha a ser trocada.</param>
        /// <exception cref="MathematicsException">Sempre.</exception>
        public void SwapLines(int i, int j)
        {
            // Não é suportado por uma submatriz definida por uma sequência ordenada
            throw new MathematicsException("Can't swap integer sequence submatrix lines.");
        }

        /// <summary>
        /// Não suportado por uma sub-matriz baseada em sequências de inteiros uma vez que estas não
        /// conservam qualquer ordem.
        /// </summary>
        /// <param name="i">A coluna a trocar.</param>
        /// <param name="j">A coluna a ser trocada.</param>
        /// <exception cref="MathematicsException">Sempre.</exception>
        public void SwapColumns(int i, int j)
        {
            // Não é suportado por uma submatriz definida por uma sequência ordenada
            throw new MathematicsException("Can't swap integer sequence submatrix columns.");
        }

        /// <summary>
        /// Obtém o enumerador genérico para a matriz.
        /// </summary>
        /// <returns>O enumerador genérico.</returns>
        public IEnumerator<ObjectType> GetEnumerator()
        {
            for (int i = 0; i < this.lines.Count; ++i)
            {
                for (int j = 0; j < this.columns.Count; ++j)
                {
                    yield return this.matrix[this.lines[i], this.columns[j]];
                }
            }
        }

        /// <summary>
        /// Constrói uma representação textual da matriz.
        /// </summary>
        /// <returns>A representação textual da matriz.</returns>
        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("[");
            if (0 < this.lines.Count)
            {
                resultBuilder.Append("[");
                if (0 < this.columns.Count)
                {
                    resultBuilder.Append(this.matrix[this.lines[0], this.columns[0]]);
                    for (int i = 1; i < this.columns.Count; ++i)
                    {
                        resultBuilder.AppendFormat(", {0}", this.matrix[this.lines[0], this.columns[i]]);
                    }
                }
                resultBuilder.Append("]");

                for (int i = 1; i < this.lines.Count; ++i)
                {
                    resultBuilder.Append(", [");
                    if (0 < this.columns.Count)
                    {
                        resultBuilder.Append(this.matrix[this.lines[i], this.columns[0]]);
                        for (int j = 1; j < this.columns.Count; ++j)
                        {
                            resultBuilder.AppendFormat(", {0}", this.matrix[this.lines[i], this.columns[j]]);
                        }
                    }

                    resultBuilder.Append("]");
                }

            }

            resultBuilder.Append("]");
            return resultBuilder.ToString();
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
