namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define operações de grupo sobre matrizes.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem as entradas das matrizes.</typeparam>
    public class GeneralMatrixGroup<CoeffType> : IMatrixGroup<CoeffType>
    {
        /// <summary>
        /// O grupo responsável pelas operações sobre os coeficientes.
        /// </summary>
        protected IGroup<CoeffType> coeffsGroup;

        /// <summary>
        /// O objecto responsável pela adição de matrizes.
        /// </summary>
        protected IAdditionOperation<IMatrix<CoeffType>, IMatrix<CoeffType>, IMatrix<CoeffType>> matrixAdditionOperation;

        /// <summary>
        /// A fábrica responsável pela criação de instâncias de matrizes.
        /// </summary>
        protected IMatrixFactory<CoeffType> matrixFactory;

        /// <summary>
        /// O número de linhas das matrizes que podem ser processadas.
        /// </summary>
        protected int lines;

        /// <summary>
        /// O número de colunas das matrizes que podem ser processadas.
        /// </summary>
        protected int columns;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="GeneralMatrixGroup{CoeffType}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="matrixFactory">A fábrica responsável pela criação de instâncias de matrizes.</param>
        /// <param name="coeffsGroup">O grupo responsável pelas operações sobre as entradas das matrizes.</param>
        /// <exception cref="ArgumentException">Se o número de linhas ou de colunas for negativo.</exception>
        /// <exception cref="ArgumentNullException">
        /// Se forem nulos os argumentos "matrixFactory" e "coeffsGroup".
        /// </exception>
        public GeneralMatrixGroup(
            int lines,
            int columns,
            IMatrixFactory<CoeffType> matrixFactory,
            IGroup<CoeffType> coeffsGroup)
        {
            if (lines < 0)
            {
                throw new ArgumentException("The number of lines must be non negative.");
            }
            else if (columns < 0)
            {
                throw new ArgumentException("The number of columns must be non negative.");
            }
            else if (matrixFactory == null)
            {
                throw new ArgumentNullException("matrixFactory");
            }
            else if (coeffsGroup == null)
            {
                throw new ArgumentNullException("coeffsGroup");
            }
            else
            {
                this.lines = lines;
                this.columns = columns;
                this.coeffsGroup = coeffsGroup;
                this.matrixFactory = matrixFactory;
                this.matrixAdditionOperation = new MatrixAdditionOperation<CoeffType>(matrixFactory, this.coeffsGroup);
            }
        }

        /// <summary>
        /// Obtém o número de linhas das matrizes que fazem parte do grupo.
        /// </summary>
        /// <value>
        /// O número de linhas das matrizes que fazem parte do grupo.
        /// </value>
        public int Lines
        {
            get
            {
                return this.lines;
            }
        }

        /// <summary>
        /// Obtém o número de colunas das matrizes que fazem parte do grupo.
        /// </summary>
        /// <value>
        /// O número de colunas que fazem parte do grupo.
        /// </value>
        public int Columns
        {
            get
            {
                return this.columns;
            }
        }

        /// <summary>
        /// Obtém a fábrica responsável pela criação de instâncias de matrizes.
        /// </summary>
        /// <value>
        /// A fábrica responsável pela criação de instâncias de matrizes.
        /// </value>
        public IMatrixFactory<CoeffType> Factory
        {
            get
            {
                return this.matrixFactory;
            }
        }

        /// <summary>
        /// Obtém a unidade aditiva.
        /// </summary>
        /// <value>
        /// A unidade aditiva.
        /// </value>
        public IMatrix<CoeffType> AdditiveUnity
        {
            get
            {
                return new ZeroMatrix<CoeffType>(this.lines, this.columns, this.coeffsGroup);
            }
        }

        /// <summary>
        /// Determina a inversa aditiva de uma matriz.
        /// </summary>
        /// <param name="number">A matriz.</param>
        /// <returns>A inversa aditiva.</returns>
        /// <exception cref="ArgumentNullException">Caso o argumento seja nulo.</exception>
        /// <exception cref="MathematicsException">
        /// Se o número de linhas ou colunas da matriz proporcionada não estiver de acordo com o número de linhas
        /// e colunas aceites pelo grupo actual, respectivamente.
        /// </exception>
        public IMatrix<CoeffType> AdditiveInverse(IMatrix<CoeffType> number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else if (number.GetLength(0) != this.lines)
            {
                throw new MathematicsException(
                    string.Format("Expecting a matrix with {0} lines.", this.lines));
            }
            else if (number.GetLength(1) != this.columns)
            {
                throw new MathematicsException(
                    string.Format("Expecting a matrix with {0} columns.", this.columns));
            }
            else
            {
                var resultMatrix = this.matrixFactory.CreateMatrix(this.lines, this.columns, this.coeffsGroup.AdditiveUnity);
                for (int i = 0; i < this.lines; ++i)
                {
                    for (int j = 0; j < this.columns; ++j)
                    {
                        var current = number[i, j];
                        if (!this.coeffsGroup.IsAdditiveUnity(current))
                        {
                            resultMatrix[i, j] = this.coeffsGroup.AdditiveInverse(current);
                        }
                    }
                }

                return resultMatrix;
            }
        }

        /// <summary>
        /// Determina se o valor especificado é uma unidade aditiva.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Caso o valor seja nulo.</exception>
        /// <exception cref="MathematicsException">
        /// Se o número de linhas ou colunas da matriz proporcionada não estiver de acordo com o número de linhas
        /// e colunas aceites pelo grupo actual, respectivamente.
        /// </exception>
        public bool IsAdditiveUnity(IMatrix<CoeffType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else if (this.lines != value.GetLength(0))
            {
                throw new MathematicsException(
                    string.Format("Expecting a matrix with {0} lines.", this.lines));
            }
            else if (this.columns != value.GetLength(1))
            {
                throw new MathematicsException(
                    string.Format("Expecting a matrix with {0} columns.", this.columns));
            }
            else
            {
                for (int i = 0; i < this.lines; ++i)
                {
                    for (int j = 0; j < this.columns; ++j)
                    {
                        var current = value[i, j];
                        if (!this.coeffsGroup.IsAdditiveUnity(current))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }


        /// <summary>
        /// Adiciona duas matrizes.
        /// </summary>
        /// <param name="left">A primeira matriz a ser adicionada.</param>
        /// <param name="right">A segunda matriz a ser adicionada.</param>
        /// <returns>A soma das matrizes.</returns>
        /// <exception cref="ArgumentNullException">
        /// Se algum dos argumentos for nulo.
        /// </exception>
        /// <exception cref="MathematicsException">
        /// Se o número de linhas ou colunas da matriz proporcionada não estiver de acordo com o número de linhas
        /// e colunas aceites pelo grupo actual, respectivamente.
        /// </exception>
        public IMatrix<CoeffType> Add(IMatrix<CoeffType> left, IMatrix<CoeffType> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (this.lines != left.GetLength(0) || this.lines != right.GetLength(0))
            {
                throw new MathematicsException(
                    string.Format("Expecting a matrix with {0} lines.", this.lines));
            }
            else if (this.columns != left.GetLength(1) || this.columns != right.GetLength(1))
            {
                throw new MathematicsException(
                    string.Format("Expecting a matrix with {0} columns.", this.columns));
            }
            else
            {
                return this.matrixAdditionOperation.Add(left, right);
            }
        }

        /// <summary>
        /// Permite determinar se duas matrizes são iguais.
        /// </summary>
        /// <remarks>
        /// Apenas as matrizes com o número especificado de linhas e colunas são comparáveis. No rol de matrizes que não se podem
        /// comparar encontra-se a referência nula.
        /// </remarks>
        /// <param name="x">A primeira matriz.</param>
        /// <param name="y">A segunda matriz.</param>
        /// <returns>Verdadeiro caso as matrizes sejam iguais e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Caso um dos argumentos seja nulo.</exception>
        /// <exception cref="MathematicsException">
        /// Se o número de linhas ou colunas da matriz proporcionada não estiver de acordo com o número de linhas
        /// e colunas aceites pelo grupo actual, respectivamente.
        /// </exception>
        public bool Equals(IMatrix<CoeffType> x, IMatrix<CoeffType> y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            else if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            if (this.lines != x.GetLength(0) || this.lines != y.GetLength(0))
            {
                throw new MathematicsException(
                string.Format("Can only compare matices with {0} lines.", this.lines));
            }
            else if (this.columns != x.GetLength(1) || this.columns != y.GetLength(1))
            {
                throw new MathematicsException(
                string.Format("Can only compare matices with {0} columns.", this.columns));
            }
            else if (x == null)
            {
                return false;
            }
            else if (y == null)
            {
                return false;
            }
            else if (this.lines != x.GetLength(0) || this.lines != y.GetLength(0))
            {
                throw new MathematicsException(
                    string.Format("Can only compare matices with {0} lines.", this.lines));
            }
            else if (this.columns != x.GetLength(1) || this.columns != y.GetLength(1))
            {
                throw new MathematicsException(
                    string.Format("Can only compare matices with {0} columns.", this.columns));
            }
            else
            {
                for (int i = 0; i < this.lines; ++i)
                {
                    for (int j = 0; j < this.columns; ++j)
                    {
                        var leftValue = x[i, j];
                        var rightValue = y[i, j];
                        if (!this.coeffsGroup.Equals(leftValue, rightValue))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Permite obter o código misturado associado à matriz.
        /// </summary>
        /// <param name="obj">A matriz.</param>
        /// <returns>O código misturado.</returns>
        /// <exception cref="ArgumentNullException">Caso a matriz introduzida seja a referência nula.</exception>
        /// <exception cref="MathematicsException">
        /// Se o número de linhas ou colunas da matriz proporcionada não estiver de acordo com o número de linhas
        /// e colunas aceites pelo grupo actual, respectivamente.
        /// </exception>
        public int GetHashCode(IMatrix<CoeffType> obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            else if (this.lines != obj.GetLength(0))
            {
                throw new MathematicsException(
                    string.Format("Can only compare matices with {0} lines.", this.lines));
            }
            else if (this.columns != obj.GetLength(1))
            {
                throw new MathematicsException(
                    string.Format("Can only compare matices with {0} columns.", this.columns));
            }
            else
            {
                var result = 17;
                for (int i = 0; i < this.lines; ++i)
                {
                    for (int j = 0; j < this.columns; ++j)
                    {
                        var current = obj[i, j];
                        result ^= result + 19 * this.coeffsGroup.GetHashCode(current);
                    }
                }

                return result;
            }
        }
    }
}
