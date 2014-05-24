namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define uma norma sobre o espaço de fracções.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos coeficientes na fracção.</typeparam>
    public class FractionNormSpace<CoeffType>
        : INormSpace<Fraction<CoeffType>, Fraction<CoeffType>>
    {
        /// <summary>
        /// O objeto responsável pela comparação de fracções.
        /// </summary>
        private FractionComparer<CoeffType> fractionComparer;

        /// <summary>
        /// O domínio responsável pelas operações sobre os coeficientes.
        /// </summary>
        private IEuclidenDomain<CoeffType> domain;

        /// <summary>
        /// O espaço das normas associado aos coeficientes.
        /// </summary>
        private INormSpace<CoeffType, CoeffType> coeffNormSpace;

        /// <summary>
        /// Cria uma instância de objectos do tipo <see cref="FractionNormSpace{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffNormSpace">O objecto que define uma norma sobre os coeficientes.</param>
        /// <param name="coeffsComparer">O comprador de coeficientes.</param>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">
        /// Caso algum dos argumentos seja nulo.
        /// </exception>
        public FractionNormSpace(
            INormSpace<CoeffType, CoeffType> coeffNormSpace,
            IComparer<CoeffType> coeffsComparer,
            IEuclidenDomain<CoeffType> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else if (coeffsComparer == null)
            {
                throw new ArgumentNullException("coeffsComparer");
            }
            if (coeffNormSpace == null)
            {
                throw new ArgumentNullException("coeffNormSpace");
            }
            else
            {
                this.fractionComparer = new FractionComparer<CoeffType>(
                    coeffsComparer, 
                    domain);
                this.coeffNormSpace = coeffNormSpace;
                this.domain = domain;
            }
        }

        /// <summary>
        /// Obtém o objeto responsável pela comparação de fracções.
        /// </summary>
        /// <value>
        /// O objecto responsável pela compração de fracções.
        /// </value>
        FractionComparer<CoeffType> FractionComparer
        {
            get
            {
                return this.fractionComparer;
            }
        }

        /// <summary>
        /// Obtém o espaço das normas associado aos coeficientes.
        /// </summary>
        /// <value>
        /// O espaço das normas associado aos coeficientes.
        /// </value>
        public INormSpace<CoeffType, CoeffType> CoeffNormSpace
        {
            get
            {
                return this.coeffNormSpace;
            }
        }

        /// <summary>
        /// Obtém o móudlo de uma fracção.
        /// </summary>
        /// <param name="value">A fracção.</param>
        /// <returns>O módulo da fracção.</returns>
        /// <exception cref="ArgumentNullException">Caso o argumento seja nulo.</exception>
        public Fraction<CoeffType> GetNorm(Fraction<CoeffType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return new Fraction<CoeffType>(
                    this.coeffNormSpace.GetNorm(value.Numerator),
                    this.coeffNormSpace.GetNorm(value.Denominator),
                    this.domain);
            }

        }

        /// <summary>
        /// Compara duas fracções.
        /// </summary>
        /// <param name="x">A primeira fracção a ser comparada.</param>
        /// <param name="y">A segunda fracção a ser comparada.</param>
        /// <returns>
        /// O valor 1 caso a primeira seja superior à segunda, 0 caso sejam iguais e -1 caso contrário.
        /// </returns>
        public int Compare(Fraction<CoeffType> x, Fraction<CoeffType> y)
        {
            return this.fractionComparer.Compare(x, y);
        }
    }
}
