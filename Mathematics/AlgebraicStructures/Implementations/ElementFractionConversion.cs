// -----------------------------------------------------------------------
// <copyright file="ElementFractionConversion.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Converte entre coeficientes e fracções sobre esses coeficientes.
    /// </summary>
    /// <typeparam name="ElementType">O tipo de elemento.</typeparam>
    public class ElementFractionConversion<ElementType>
        : IConversion<ElementType, Fraction<ElementType>>
    {
        /// <summary>
        /// O domínio responsável pelas operações sobre os coeficientes.
        /// </summary>
        protected IEuclidenDomain<ElementType> domain;

        /// <summary>
        /// Cria uma instância de objectos do tipo <see cref="ElementFractionConversion{ElementType}"/>.
        /// </summary>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">Caso o domínio passado seja nulo.</exception>
        public ElementFractionConversion(IEuclidenDomain<ElementType> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                this.domain = domain;
            }
        }

        /// <summary>
        /// Obtém o domínio responsável pelas operações sobre os coeficientes.
        /// </summary>
        public IEuclidenDomain<ElementType> Domain
        {
            get
            {
                return this.domain;
            }
        }

        /// <summary>
        /// Indica se é possível converter uma fracção de coeficientes num coeficiente.
        /// </summary>
        /// <remarks>
        /// Uma fracção de coeficientes é convertível num coeficiente caso o seu denominador seja uma unidade
        /// aditiva ou o seu inverso aditivo.
        /// </remarks>
        /// <param name="objectToConvert">O coeficiente em análise.</param>
        /// <returns>Verdadeiro caso a conversão seja possível e falso caso contrário.</returns>
        public bool CanApplyDirectConversion(Fraction<ElementType> objectToConvert)
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
                    return true;
                }
                else if (this.domain.IsAdditiveUnity(this.domain.AdditiveInverse(fractionPartValue)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Indica se é possível converter um coeficiente numa fracção.
        /// </summary>
        /// <remarks>
        /// Esta conversão é sempre possível.
        /// </remarks>
        /// <param name="objectToConvert">O coeficiente.</param>
        /// <returns>Verdadeiro.</returns>
        public bool CanApplyInverseConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                return false;
            }
            else
            {
                return true; ;
            }
        }

        /// <summary>
        /// Obtém o resultado da conversão de uma fracção de coeficiente num coeficiente.
        /// </summary>
        /// <remarks>
        /// Uma fracção de coeficientes é convertível num coeficiente caso o seu denominador seja uma unidade
        /// aditiva ou o seu inverso aditivo.
        /// </remarks>
        /// <param name="objectToConvert">A fracção de coeficientes.</param>
        /// <returns>O coeficiente convertido.</returns>
        /// <exception cref="MathematicsException">Caso a fracção não seja convertível para coeficiente.</exception>
        public ElementType DirectConversion(Fraction<ElementType> objectToConvert)
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
                    return fractionDecomposition.IntegralPart;
                }
                else if (this.domain.IsAdditiveUnity(this.domain.AdditiveInverse(fractionDecomposition.FractionalPart.Numerator)))
                {
                    return this.domain.AdditiveInverse(fractionDecomposition.IntegralPart);
                }
                else
                {
                    throw new MathematicsException("Can't convert fraction to the matching element. Fraction has fractional part.");
                }
            }
        }

        /// <summary>
        /// Efectua a conversão inversa de um coeficiente para uma fracção de coeficientes.
        /// </summary>
        /// <param name="objectToConvert">O coeficiente a ser convertido.</param>
        /// <returns>A fracção que permite representar o coeficiente.</returns>
        /// <exception cref="System.ArgumentNullException">Caso o objecto passado seja nulo.</exception>
        public Fraction<ElementType> InverseConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else
            {
                return new Fraction<ElementType>(objectToConvert, this.domain.MultiplicativeUnity, this.domain);
            }
        }
    }
}
