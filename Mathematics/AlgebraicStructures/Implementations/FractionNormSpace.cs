namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class FractionNormSpace<CoeffType, DomainType>
        : INormSpace<Fraction<CoeffType, DomainType>, Fraction<CoeffType, DomainType>>
        where DomainType : IEuclidenDomain<CoeffType>
    {
        /// <summary>
        /// O objeto responsável pela comparação de fracções.
        /// </summary>
        FractionComparer<CoeffType, DomainType> fractionComparer;

        /// <summary>
        /// O espaço das normas associado aos coeficientes.
        /// </summary>
        private INormSpace<CoeffType, CoeffType> coeffNormSpace;

        public FractionNormSpace(
            INormSpace<CoeffType, CoeffType> coeffNormSpace,
            FractionComparer<CoeffType, DomainType> fractionComparer)
        {
            if (fractionComparer == null)
            {
                throw new ArgumentNullException("fractionComparer");
            }
            if (coeffNormSpace == null)
            {
                throw new ArgumentNullException("coeffNormSpace");
            }
            else
            {
                this.fractionComparer = fractionComparer;
                this.coeffNormSpace = coeffNormSpace;
            }
        }

        /// <summary>
        /// Obtém o objeto responsável pela comparação de fracções.
        /// </summary>
        FractionComparer<CoeffType, DomainType> FractionComparer
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
        public Fraction<CoeffType, DomainType> GetNorm(Fraction<CoeffType, DomainType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return new Fraction<CoeffType, DomainType>(
                    this.coeffNormSpace.GetNorm(value.Numerator),
                    this.coeffNormSpace.GetNorm(value.Denominator),
                    value.Domain);
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
        public int Compare(Fraction<CoeffType, DomainType> x, Fraction<CoeffType, DomainType> y)
        {
            return this.fractionComparer.Compare(x, y);
        }
    }
}
