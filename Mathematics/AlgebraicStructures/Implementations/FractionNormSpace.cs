namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class FractionNormSpace<CoeffType>
        : INormSpace<Fraction<CoeffType>, Fraction<CoeffType>>
    {
        /// <summary>
        /// O objeto responsável pela comparação de fracções.
        /// </summary>
        private FractionComparer<CoeffType> fractionComparer;

        private IEuclidenDomain<CoeffType> domain;

        /// <summary>
        /// O espaço das normas associado aos coeficientes.
        /// </summary>
        private INormSpace<CoeffType, CoeffType> coeffNormSpace;

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
        FractionComparer<CoeffType> FractionComparer
        {
            get
            {
                return this.fractionComparer;
            }
        }

        /// <summary>
        /// O espaço das normas associado aos coeficientes.
        /// </summary>
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
