namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Define operações de anel sobre matrizes genéricas.
    /// </summary>
    /// <remarks>
    /// As matrizes sobre as quais é possível operar terão de ser quadradas. Caso contrário, resultarão em erro.
    /// </remarks>
    /// <typeparam name="CoeffType">O tipo de coeficientes da matriz.</typeparam>
    public class GeneralMatrixRing<CoeffType> : GeneralMatrixGroup<CoeffType>, IRing<IMatrix<CoeffType>>
    {
        /// <summary>
        /// O anel responsável pelas operações sobre os coeficientes.
        /// </summary>
        protected IRing<CoeffType> coeffsRing;

        /// <summary>
        /// O objecto responsável pela multiplicação de matrizes.
        /// </summary>
        protected IMultiplicationOperation<IMatrix<CoeffType>, IMatrix<CoeffType>, IMatrix<CoeffType>> matrixMult;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="GeneralMatrixRing{CoeffType}"/>.
        /// </summary>
        /// <param name="dimension">A dimensão das matrizes que poderão ser operadas.</param>
        /// <param name="matrixFactory">A fábrica responsável pela instância das matrizes resultantes.</param>
        /// <param name="coeffsRing">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">Caso algum dos argumentos seja nulo.</exception>
        public GeneralMatrixRing(
            int dimension,
            IMatrixFactory<CoeffType> matrixFactory,
            IRing<CoeffType> coeffsRing)
            : base(dimension, dimension, matrixFactory, coeffsRing)
        {
            if (matrixFactory == null)
            {
                throw new ArgumentNullException("matrixFactory");
            }
            else
            {
                this.coeffsRing = coeffsRing;
                this.matrixMult = new MatrixMultiplicationOperation<CoeffType>(
                    matrixFactory,
                    this.coeffsRing,
                    this.coeffsRing);
            }
        }

        /// <summary>
        /// Obtém a unidade multiplicativa.
        /// </summary>
        /// <value>
        /// A unidade multiplicativa.
        /// </value>
        public IMatrix<CoeffType> MultiplicativeUnity
        {
            get
            {
                return new IdentityMathMatrix<CoeffType>(this.lines, this.coeffsRing);
            }
        }

        /// <summary>
        /// Permite determinar a adição repetida de uma matriz.
        /// </summary>
        /// <param name="element">A matriz.</param>
        /// <param name="times">O número de vezes que a matriz é adicionada.</param>
        /// <returns>O resultado da adição repetida.</returns>
        public IMatrix<CoeffType> AddRepeated(IMatrix<CoeffType> element, int times)
        {
            return MathFunctions.AddPower(element, times, this);
        }

        /// <summary>
        /// Determina se a matriz proporcionada é uma unidade multiplicativa.
        /// </summary>
        /// <param name="value">A matriz a ser analisada.</param>
        /// <returns>Verdadeiro caso a matriz seja uma unidade aditiva e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Caso o argumento seja nulo.</exception>
        /// <exception cref="MathematicsException">
        /// Se a matriz proporcionada não contiver as dimensões definidas para o anel corrente.
        /// </exception>
        public bool IsMultiplicativeUnity(IMatrix<CoeffType> value)
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
                    var diagonal = value[i, i];
                    if (!this.coeffsRing.IsMultiplicativeUnity(diagonal))
                    {
                        return false;
                    }
                }

                for (int i = 0; i < this.lines; ++i)
                {
                    for (int j = 0; j < i; ++j)
                    {
                        var current = value[i, j];
                        if (!this.coeffsRing.IsAdditiveUnity(current))
                        {
                            return false;
                        }
                    }

                    for (int j = i + 1; j < i; ++j)
                    {
                        var current = value[i, j];
                        if (!this.coeffsRing.IsAdditiveUnity(current))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Calcula o produto de duas matrizes.
        /// </summary>
        /// <remarks>
        /// O produto de duas matrizes é calculado com base nas fórmulas habituais. Convém lembrar que existe
        /// um algoritmo capaz de efectuar esta multiplicação de forma mais rápida.
        /// </remarks>
        /// <param name="left">A primeira matriz a ser multiplicada.</param>
        /// <param name="right">A segunda matriz a ser multiplicada.</param>
        /// <returns>O resultado do produto das matrizes.</returns>
        /// <exception cref="ArgumentNullException">
        /// Se cada uma das matrizes for nula.
        /// </exception>
        /// <exception cref="MathematicsException">
        /// Se a matriz proporcionada não contiver as dimensões definidas para o anel corrente.
        /// </exception>
        public IMatrix<CoeffType> Multiply(IMatrix<CoeffType> left, IMatrix<CoeffType> right)
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
                return this.matrixMult.Multiply(left, right);
            }
        }
    }
}
