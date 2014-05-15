using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Collections;

namespace Mathematics
{
    /// <summary>
    /// Representa a submatriz de uma matriz sem ter a necessidade de copiá-la
    /// na íntegra.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de elementos na matriz.</typeparam>
    internal class SubMatrix<ObjectType> : IMatrix<ObjectType>
    {
        /// <summary>
        /// A matriz original.
        /// </summary>
        private IMatrix<ObjectType> matrix;

        /// <summary>
        /// As linhas que constituem a sub-matriz.
        /// </summary>
        private int[] lines;

        /// <summary>
        /// As colunas que constituem a sub-matriz.
        /// </summary>
        private int[] columns;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SubMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="matrix">A matriz original.</param>
        /// <param name="lines">As linhas que constituem a sub-matriz.</param>
        /// <param name="columns">As colunas que constituem a sub-matriz.</param>
        /// <exception cref="System.ArgumentException">
        /// Se as linhas ou as colunas forem vectores nulos.
        /// </exception>
        public SubMatrix(IMatrix<ObjectType> matrix, int[] lines, int[] columns)
        {
            if (lines == null || columns == null)
            {
                throw new ArgumentException("Parameters lines and columns must be non null.");
            }
            else
            {
                this.matrix = matrix;
                this.lines = new int[lines.Length];
                Array.Copy(lines, this.lines, lines.Length);
                this.columns = new int[columns.Length];
                Array.Copy(columns, this.columns, columns.Length);
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificaod pelos índices.
        /// </summary>
        /// <remarks>
        /// A atribuição de um valor implica a actualização da entrada respectiva na matriz original.
        /// </remarks>
        /// <value>
        /// O valor.
        /// </value>
        /// <param name="line">O índice correspondente à linha.</param>
        /// <param name="column">O índice correspondente à coluna.</param>
        /// <returns></returns>
        /// <exception cref="System.IndexOutOfRangeException">
        /// Se o número da linha ou o número da coluna forem negativos ou não forem inferiores ao tamanho da
        /// respectiva dimensão.
        /// </exception>
        public ObjectType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.lines.Length)
                {
                    throw new IndexOutOfRangeException("Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.columns.Length)
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
                if (line < 0 || line >= this.lines.Length)
                {
                    throw new IndexOutOfRangeException("Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.columns.Length)
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
                return this.lines.Length;
            }
            else if (dimension == 1)
            {
                return this.columns.Length;
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
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for negativo ou não for inferior ao tamanho da dimensão respectiva.
        /// </exception>
        public void SwapLines(int i, int j)
        {
            if (i < 0 || i > this.lines.Length)
            {
                throw new IndexOutOfRangeException("i");
            }
            else if (j < 0 || j > this.lines.Length)
            {
                throw new IndexOutOfRangeException("j");
            }
            else if (i != j)
            {
                var swapLine = this.lines[i];
                this.lines[i] = this.lines[j];
                this.lines[j] = swapLine;
            }
        }

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocaada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for negativo ou não for inferior ao tamanho da dimensão respectiva.
        /// </exception>
        public void SwapColumns(int i, int j)
        {
            if (i < 0 || i > this.columns.Length)
            {
                throw new IndexOutOfRangeException("i");
            }
            else if (j < 0 || j > this.columns.Length)
            {
                throw new IndexOutOfRangeException("j");
            }
            else if (i != j)
            {
                var swapColumn = this.columns[i];
                this.columns[i] = this.columns[j];
                this.columns[j] = swapColumn;
            }
        }

        /// <summary>
        /// Obtém o enumerador genérico para a matriz.
        /// </summary>
        /// <returns>O enumerador genérico.</returns>
        public IEnumerator<ObjectType> GetEnumerator()
        {
            for (int i = 0; i < this.lines.Length; ++i)
            {
                for (int j = 0; j < this.columns.Length; ++j)
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
            if (0 < this.lines.Length)
            {
                resultBuilder.Append("[");
                if (0 < this.columns.Length)
                {
                    resultBuilder.Append(this.matrix[this.lines[0], this.columns[0]]);
                    for (int i = 1; i < this.columns.Length; ++i)
                    {
                        resultBuilder.AppendFormat(", {0}", this.matrix[this.lines[0], this.columns[i]]);
                    }
                }
                resultBuilder.Append("]");

                for (int i = 1; i < this.lines.Length; ++i)
                {
                    resultBuilder.Append(", [");
                    if (0 < this.columns.Length)
                    {
                        resultBuilder.Append(this.matrix[this.lines[i], this.columns[0]]);
                        for (int j = 1; j < this.columns.Length; ++j)
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
