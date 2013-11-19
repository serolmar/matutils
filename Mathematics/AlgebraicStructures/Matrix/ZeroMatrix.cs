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
    /// <typeparam name="MonoidType">O tipo do monóide.</typeparam>
    public class ZeroMatrix<ElementType, MonoidType> : IMatrix<ElementType>
        where MonoidType : IMonoid<ElementType>
    {
        /// <summary>
        /// O monóide responsável por determinar o elemento nulo.
        /// </summary>
        private MonoidType monoid;

        /// <summary>
        /// O número de linhas.
        /// </summary>
        private int linesNumber;

        /// <summary>
        /// O número de colunas.
        /// </summary>
        private int columnsNumber;

        public ZeroMatrix(int lines, int columns, MonoidType monoid)
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

        public IMatrix<ElementType> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<ElementType>(this, lines, columns);
        }

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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
