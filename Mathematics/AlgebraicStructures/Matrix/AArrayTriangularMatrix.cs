namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite desenvolver uma matriz triangular.
    /// </summary>
    public abstract class AArrayTriangularMatrix<CoeffType> : ArrayMatrix<CoeffType>, ISquareMatrix<CoeffType>
    {
        /// <summary>
        /// Mantém o valor por defeito.
        /// </summary>
        protected CoeffType defaultValue;

        public AArrayTriangularMatrix(int dimension)
            : base(dimension, dimension)
        {
        }

        public AArrayTriangularMatrix(int dimension, CoeffType defaultValue)
            : base(dimension, dimension, defaultValue)
        {
        }

        /// <summary>
        /// Verifica se se trata de uma matriz simétrica.
        /// </summary>
        /// <param name="equalityComparer">O comparador das entradas.</param>
        /// <returns>Verdadeiro caso se trate de uma matriz simétrica e falso no caso contrário.</returns>
        public bool IsSymmetric(IEqualityComparer<CoeffType> equalityComparer)
        {
            var innerEqualityComparer = equalityComparer;
            if (innerEqualityComparer == null)
            {
                innerEqualityComparer = EqualityComparer<CoeffType>.Default;
            }

            var last = this.numberOfColumns;
            for (int i = 0; i < this.numberOfLines; ++i)
            {
                for (int j = 0; j < last; ++j)
                {
                    var currentEntry = this.elements[i][j];
                    if (!innerEqualityComparer.Equals(currentEntry, this.defaultValue))
                    {
                        return false;
                    }
                }

                --last;
            }

            return true;
        }

        public override void SwapLines(int i, int j)
        {
            throw new MathematicsException("Can't swap lines in a triangular matrix.");
        }

        public override void SwapColumns(int i, int j)
        {
            throw new MathematicsException("Can't swap columns in a triangular matrix.");
        }

        /// <summary>
        /// Sobrecarrega a função que permite inicializar a matriz.
        /// </summary>
        /// <param name="line">O número de linhas da matriz.</param>
        /// <param name="column">O número de colunas da matriz.</param>
        protected override void InitializeMatrix(int line, int column)
        {
            this.defaultValue = default(CoeffType);
            this.elements = new CoeffType[line][];
            var last = column;
            for (int i = 0; i < line; ++i)
            {
                this.elements[i] = new CoeffType[last];
                --last;
            }

            this.numberOfLines = line;
            this.numberOfColumns = column;
        }

        /// <summary>
        /// Sobrecarrega a função que permite inicializar a matriz sobre a qual é atribuído um
        /// valor por defeito.
        /// </summary>
        /// <param name="line">O número de linhas.</param>
        /// <param name="column">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        protected override void InitializeMatrix(int line, int column, CoeffType defaultValue)
        {
            this.defaultValue = defaultValue;
            this.elements = new CoeffType[line][];
            var last = column;
            if (EqualityComparer<object>.Default.Equals(defaultValue, default(CoeffType)))
            {
                for (int i = 0; i < line; ++i)
                {
                    this.elements[i] = new CoeffType[last];
                    --last;
                }
            }
            else
            {
                for (int i = 0; i < line; ++i)
                {
                    this.elements[i] = new CoeffType[last];
                    for (int j = 0; j < last; ++j)
                    {
                        this.elements[i][j] = defaultValue;
                    }
                }
            }

            this.numberOfLines = line;
            this.numberOfColumns = column;
        }
    }
}
