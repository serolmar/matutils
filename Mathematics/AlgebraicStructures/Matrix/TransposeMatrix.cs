namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Representa a transposta de uma matriz.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo dos objectos que constituem as entradas das matrizes.</typeparam>
    public class TransposeMatrix<ObjectType> : IMatrix<ObjectType>
    {
        /// <summary>
        /// A matriz original.
        /// </summary>
        private IMatrix<ObjectType> matrix;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="TransposeMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="matrix">A matriz.</param>
        /// <exception cref="ArgumentNullException">Se a matriz for nula.</exception>
        public TransposeMatrix(IMatrix<ObjectType> matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }
            else
            {
                this.matrix = matrix;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor na linha e coluna espcificados pelos índices.
        /// </summary>
        /// <value>
        /// O valor.
        /// </value>
        /// <param name="line">O número da linha.</param>
        /// <param name="column">O número da coluna.</param>
        /// <returns>O valor.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número da linha ou o número da coluna for negativo ou não for inferior ao tamanho
        /// da respectiva dimensão.
        /// </exception>
        public ObjectType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.matrix.GetLength(1))
                {
                    throw new ArgumentOutOfRangeException("line");
                }
                else if (column < 0 || column >= this.matrix.GetLength(0))
                {
                    throw new ArgumentOutOfRangeException("column");
                }
                else
                {
                    return this.matrix[column, line];
                }
            }
            set
            {
                if (line < 0 || line >= this.matrix.GetLength(1))
                {
                    throw new ArgumentOutOfRangeException("line");
                }
                else if (column < 0 || column >= this.matrix.GetLength(0))
                {
                    throw new ArgumentOutOfRangeException("column");
                }
                else
                {
                    this.matrix[column, line] = value;
                }
            }
        }

        /// <summary>
        /// Obtém a matriz original.
        /// </summary>
        /// <value>
        /// A matriz original.
        /// </value>
        public IMatrix<ObjectType> Matrix
        {
            get
            {
                return this.matrix;
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
                return this.matrix.GetLength(1);
            }
            else if (dimension == 1)
            {
                return this.matrix.GetLength(0);
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
        public void SwapLines(int i, int j)
        {
            this.matrix.SwapColumns(i, j);
        }

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocaada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        public void SwapColumns(int i, int j)
        {
            this.SwapLines(i, j);
        }

        /// <summary>
        /// Obtém o enumerador genérico para a matriz.
        /// </summary>
        /// <returns>O enumerador genérico.</returns>
        public IEnumerator<ObjectType> GetEnumerator()
        {
            for (int i = 0; i < this.matrix.GetLength(1); ++i)
            {
                for (int j = 0; j < this.matrix.GetLength(0); ++j)
                {
                    yield return this.matrix[j, i];
                }
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
