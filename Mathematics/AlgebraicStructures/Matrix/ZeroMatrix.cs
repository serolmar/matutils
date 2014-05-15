namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Representa uma matriz constante cujas entradas são os elementos nulos de um monóide.
    /// </summary>
    /// <typeparam name="ElementType">O tipo de dados contidos na matriz.</typeparam>
    public class ZeroMatrix<ElementType> : IMatrix<ElementType>
    {
        /// <summary>
        /// O monóide responsável por determinar o elemento nulo.
        /// </summary>
        private IMonoid<ElementType> monoid;

        /// <summary>
        /// O número de linhas.
        /// </summary>
        private int linesNumber;

        /// <summary>
        /// O número de colunas.
        /// </summary>
        private int columnsNumber;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ZeroMatrix{ElementType}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas da matriz.</param>
        /// <param name="columns">O número de colunas da matriz.</param>
        /// <param name="monoid">O monóide responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="System.ArgumentNullException">Se o monóide for nulo.</exception>
        /// <exception cref="System.ArgumentException">
        /// Se o número de linhas ou o número de colunas for negativo.
        /// </exception>
        public ZeroMatrix(int lines, int columns, IMonoid<ElementType> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else if (lines < 0)
            {
                throw new ArgumentException("Parameter lines must be non-negative.");
            }
            else if (columns < 0)
            {
                throw new ArgumentException("Parameter columns must be non-negative.");
            }
            else
            {
                this.monoid = monoid;
                this.linesNumber = lines;
                this.columnsNumber = columns;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor na posição da matriz especificada pelos índices.
        /// </summary>
        /// <remarks>
        /// Apenas o zero associado ao monóide pode ser introduzido. Todos os outros valores irão resultar
        /// numa excepção.
        /// </remarks>
        /// <value>
        /// O valor nulo.
        /// </value>
        /// <param name="line">O número da linha.</param>
        /// <param name="column">O número da coluna.</param>
        /// <returns></returns>
        /// <exception cref="System.IndexOutOfRangeException">
        /// Se o número da linha ou o número da coluna for negativo ou não for inferior ao tamanho da dimensão
        /// respectiva.
        /// </exception>
        /// <exception cref="MathematicsException">Se algum valor diferente de zero for atribuído.</exception>
        public ElementType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.linesNumber)
                {
                    throw new IndexOutOfRangeException("Parameter line must be non-negative and less than the size of matrix.");
                }
                else if (column < 0 || column >= this.columnsNumber)
                {
                    throw new IndexOutOfRangeException("Parameter line must be non-negative and less than the size of matrix.");
                }
                else
                {
                    return this.monoid.AdditiveUnity;
                }
            }
            set
            {
                if (!this.monoid.IsAdditiveUnity(value))
                {
                    throw new MathematicsException("All entries of zero matrix must be zero.");
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
                return this.linesNumber;
            }
            else if (dimension == 1)
            {
                return this.columnsNumber;
            }
            else
            {
                throw new ArgumentOutOfRangeException("dimension");
            }
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<ElementType> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<ElementType>(this, lines, columns);
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<ElementType> GetSubMatrix(IntegerSequence lines, IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<ElementType>(this, lines, columns);
        }

        /// <summary>
        /// A troca de linhas deixa a matriz nula invariante.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        public void SwapLines(int i, int j)
        {
        }

        /// <summary>
        /// A troca de colunas deixa a matriz nula invariante.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        public void SwapColumns(int i, int j)
        {
        }

        /// <summary>
        /// Obtém o enumerador genérico para a matriz.
        /// </summary>
        /// <returns>O enumerador genérico.</returns>
        public IEnumerator<ElementType> GetEnumerator()
        {
            for (int i = 0; i < this.linesNumber; ++i)
            {
                for (int j = 0; j < this.columnsNumber; ++j)
                {
                    yield return this.monoid.AdditiveUnity;
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
