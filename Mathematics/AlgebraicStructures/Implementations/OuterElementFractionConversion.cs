namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite converter entre fracções e elementos externos.
    /// </summary>
    /// <typeparam name="OutElementType">O tipo do elemento externo.</typeparam>
    /// <typeparam name="FractionElementType">O tipo de elementos que constitui a fracção.</typeparam>
    public class OuterElementFractionConversion<OutElementType, FractionElementType>
        : IConversion<OutElementType, Fraction<FractionElementType>>
    {
        /// <summary>
        /// O domínio responsável pelas operações sobre os coeficientes das fracções.
        /// </summary>
        protected IEuclidenDomain<FractionElementType> domain;

        /// <summary>
        /// O conversor responsável pela conversão entre os coeficientes das fracções e os elementos externos.
        /// </summary>
        protected IConversion<OutElementType, FractionElementType> outTypeToFractionTypeConversion;

        public OuterElementFractionConversion(
            IConversion<OutElementType, FractionElementType> outTypeToFractionTypeConversion,
            IEuclidenDomain<FractionElementType> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else if (outTypeToFractionTypeConversion == null)
            {
                throw new ArgumentNullException("outTypeToFractionTypeConversion");
            }
            else
            {
                this.domain = domain;
                this.outTypeToFractionTypeConversion = outTypeToFractionTypeConversion;
            }
        }

        /// <summary>
        /// Indica se é possível converter o uma fracção para o tipo externo.
        /// </summary>
        /// <param name="objectToConvert">A fracção em análise.</param>
        /// <returns>Verdadeiro caso a conversão seja possível e falso caso contrário.</returns>
        public bool CanApplyDirectConversion(Fraction<FractionElementType> objectToConvert)
        {
            if (objectToConvert == null)
            {
                return false;
            }
            else
            {
                var fractionPartValue = objectToConvert.FractionalPart(this.domain).Numerator;
                if (this.domain.IsAdditiveUnity(fractionPartValue))
                {
                    return this.outTypeToFractionTypeConversion.CanApplyDirectConversion(
                        objectToConvert.IntegralPart(this.domain));
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Indica se é possível converter um valor externo para uma fracção.
        /// </summary>
        /// <remarks>
        /// Esta conversão é sempre possível se o valor externo puder ser convertido em coeficiente.
        /// </remarks>
        /// <param name="objectToConvert">O valor externo.</param>
        /// <returns>Verdadeiro se a conversão for possível e falso caso contrário.</returns>
        public bool CanApplyInverseConversion(OutElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                return true;
            }
            else
            {
                return this.outTypeToFractionTypeConversion.CanApplyInverseConversion(objectToConvert);
            }
        }

        /// <summary>
        /// Obtém o resultado da conversão de uma fracção para um valor externo.
        /// </summary>
        /// <param name="objectToConvert">A fracção.</param>
        /// <returns>O inteiro convertido.</returns>
        /// <exception cref="ArgumentNullException">Se a fracção for nula.</exception>
        /// <exception cref="MathematicsException">Caso o número de precisão dupla não represente um valor inteiro.</exception>
        public OutElementType DirectConversion(Fraction<FractionElementType> objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else
            {
                var fractionDecomposition = objectToConvert.FractionDecomposition(this.domain);
                if (this.domain.IsAdditiveUnity(fractionDecomposition.FractionalPart.Numerator))
                {
                    return this.outTypeToFractionTypeConversion.DirectConversion(fractionDecomposition.IntegralPart);
                }
                else
                {
                    throw new MathematicsException("Can't convert fraction to the matching element. Fraction has fractional part.");
                }
            }
        }

        /// <summary>
        /// Converte um valor inteiro externo para uma fracção.
        /// </summary>
        /// <param name="objectToConvert">O número externo a ser convertido.</param>
        /// <returns>A fracção.</returns>
        /// <exception cref="ArgumentNullException">Se o valor externo for nulo.</exception>
        public Fraction<FractionElementType> InverseConversion(OutElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else
            {
                return new Fraction<FractionElementType>(
                    this.outTypeToFractionTypeConversion.InverseConversion(objectToConvert), 
                    this.domain.MultiplicativeUnity, 
                    this.domain);
            }
        }
    }
}
