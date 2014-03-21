namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GeneralMatrixGroup<CoeffType> : IGroup<IMatrix<CoeffType>>
    {
        protected IGroup<CoeffType> coeffsGroup;

        protected IAdditionOperation<IMatrix<CoeffType>> matrixAdditionOperation;

        IMatrixFactory<CoeffType> matrixFactory;

        protected int lines;

        protected int columns;

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
                throw new ArgumentNullException("The number of columns must be non negative.");
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

        public IMatrix<CoeffType> AdditiveUnity
        {
            get
            {
                return new ZeroMatrix<CoeffType>(this.lines, this.columns, this.coeffsGroup);
            }
        }

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
