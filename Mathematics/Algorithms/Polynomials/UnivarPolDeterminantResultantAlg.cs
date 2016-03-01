namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Permite determinar o resultante entre dois polinómios recorrendo ao cálculo do determinante.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficientes.</typeparam>
    public class UnivarPolDeterminantResultantAlg<CoeffType> :
        IAlgorithm<
        UnivariatePolynomialNormalForm<CoeffType>,
        UnivariatePolynomialNormalForm<CoeffType>,
        CoeffType>
    {
        /// <summary>
        /// O anel reponsável pelas operações sobre os coeficientes.
        /// </summary>
        private IRing<CoeffType> ring;

        /// <summary>
        /// O algoritmo responsável pelo cáclculo de determinantes.
        /// </summary>
        IAlgorithm<ISquareMathMatrix<CoeffType>, UnivariatePolynomialNormalForm<CoeffType>> determinantAlg;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="UnivarPolDeterminantResultantAlg{CoeffType}"/>.
        /// </summary>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">Se o anel for nulo.</exception>
        public UnivarPolDeterminantResultantAlg(IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                this.ring = ring;
                this.determinantAlg = new FastDivisionFreeCharPolynomCalculator<CoeffType>(
                    "x",
                    this.ring);
            }
        }

        /// <summary>
        /// Determina o resultante entre dois polinómios.
        /// </summary>
        /// <param name="first">O primeiro polinómio.</param>
        /// <param name="second">O segundo polinómio.</param>
        /// <returns>O valor do resultante.</returns>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        public CoeffType Run(
            UnivariatePolynomialNormalForm<CoeffType> first,
            UnivariatePolynomialNormalForm<CoeffType> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            else if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            else if (first.IsZero || second.IsZero)
            {
                return this.ring.AdditiveUnity;
            }
            else if (first.IsValue && second.IsValue)
            {
                return this.ring.MultiplicativeUnity;
            }
            else
            {
                var n = first.Degree;
                var m = second.Degree;
                var squareMatrix = new ArraySquareMathMatrix<CoeffType>(m + n, this.ring.AdditiveUnity);
                for (int i = 0; i < m; ++i)
                {
                    for (int j = 0; j < first.Terms.Count; ++j)
                    {
                        var currentDeg = first.Terms.Keys[j];
                        var currentVal = first.Terms.Values[j];
                        squareMatrix[i, i + n - currentDeg] = currentVal;
                    }
                }

                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < second.Terms.Count; ++j)
                    {
                        var currentDeg = second.Terms.Keys[j];
                        var currentVal = second.Terms.Values[j];
                        squareMatrix[i + m, i + m - currentDeg] = currentVal;
                    }
                }

                var result = this.determinantAlg.Run(squareMatrix).Replace(this.ring.AdditiveUnity, this.ring);
                return result;
            }
        }
    }
}
