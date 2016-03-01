namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite desenvolver uma matriz triangular.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficientes que constituem as entradas das matrizes.</typeparam>
    public abstract class AArrayTriangularMathMatrix<CoeffType> : ArrayMathMatrix<CoeffType>, ISquareMathMatrix<CoeffType>
    {
        /// <summary>
        /// Mantém o valor por defeito.
        /// </summary>
        protected CoeffType defaultValue;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="AArrayTriangularMathMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        public AArrayTriangularMathMatrix(int dimension)
            : base(dimension, dimension)
        {
        }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="AArrayTriangularMathMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public AArrayTriangularMathMatrix(int dimension, CoeffType defaultValue)
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

            var last = 1;
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

                ++last;
            }

            return true;
        }

        /// <summary>
        /// Função sobrecarregada da classe orignal.
        /// </summary>
        /// <param name="i">O índice de linha a ser trocada.</param>
        /// <param name="j">O índice da linha a trocar.</param>
        /// <exception cref="MathematicsException">
        /// Não é possível trocar linhas de uma matriz triangular.
        /// </exception>
        public override void SwapLines(int i, int j)
        {
            throw new MathematicsException("Can't swap lines in a triangular matrix.");
        }

        /// <summary>
        /// Função sobrecarregada da classe orignal.
        /// </summary>
        /// <param name="i">O índice de coluna a ser trocada.</param>
        /// <param name="j">O índice da coluna a trocar.</param>
        /// <exception cref="MathematicsException">
        /// Não é possível trocar colunas de uma matriz triangular.
        /// </exception>
        public override void SwapColumns(int i, int j)
        {
            throw new MathematicsException("Can't swap columns in a triangular matrix.");
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
        public override void CombineLines(int i, int j, CoeffType a, CoeffType b, IRing<CoeffType> ring)
        {
            throw new MathematicsException("Can't combine lines in a triangular matrix.");
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
            var last = 1;
            for (int i = 0; i < line; ++i)
            {
                this.elements[i] = new CoeffType[last];
                ++last;
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
            var last = 1;
            for (int i = 0; i < line; ++i)
            {
                this.elements[i] = new CoeffType[last];
                for (int j = 0; j < last; ++j)
                {
                    this.elements[i][j] = defaultValue;
                }

                ++last;
            }

            this.numberOfLines = line;
            this.numberOfColumns = column;
        }
    }
}
