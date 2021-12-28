using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Indica que não é possível converter entre os dois tipos de dados.
    /// </summary>
    /// <typeparam name="FirstType">O tipo de objecto que resulta da conversão.</typeparam>
    /// <typeparam name="SecondType">O tipo de objecto a converter.</typeparam>
    public class CantConvertConversion<FirstType, SecondType> : IConversion<FirstType, SecondType>
    {
        /// <summary>
        /// Indica se o objecto é convertível num do primeiro tipo.
        /// </summary>
        /// <param name="objectToConvert">O objecto.</param>
        /// <returns>Verdadeiro caso seja convertível e falso no caso contrário.</returns>
        public bool CanApplyDirectConversion(SecondType objectToConvert)
        {
            // Nuca é possível converter.
            return false;
        }

        /// <summary>
        /// Indica se o objecto é convertível num do segundo tipo.
        /// </summary>
        /// <param name="objectToConvert">O objecto.</param>
        /// <returns>Verdadeiro caso seja convertível e falso no caso contrário.</returns>
        public bool CanApplyInverseConversion(FirstType objectToConvert)
        {
            // Nunca é possível converter.
            return false;
        }

        /// <summary>
        /// Converte o objecto do segundo tipo no primeiro.
        /// </summary>
        /// <param name="objectToConvert">O objecto a ser convertido.</param>
        /// <returns>O valor da conversão.</returns>
        /// <exception cref="MathematicsException">Sempre.</exception>
        public FirstType DirectConversion(SecondType objectToConvert)
        {
            throw new MathematicsException(
                string.Format("Can't convert from type {0} to type {1}.",
                typeof(SecondType).ToString(),
                typeof(FirstType).ToString()));
        }

        /// <summary>
        /// Converte o objecto do primeiro tipo no segundo.
        /// </summary>
        /// <param name="objectToConvert">O objecto a ser convertido.</param>
        /// <returns>O valor da conversão.</returns>
        /// <exception cref="MathematicsException">Sempre.</exception>
        public SecondType InverseConversion(FirstType objectToConvert)
        {
            throw new MathematicsException(
                string.Format("Can't convert from type {0} to type {1}.",
                typeof(SecondType).ToString(),
                typeof(FirstType).ToString()));
        }
    }

    /// <summary>
    /// Converte objectos em outros objectos do mesmo tipo.
    /// </summary>
    /// <remarks>
    /// Esta classe define uma espécie de identidade de conversões de tipos. As conversões
    /// poderão ser úteis em alguns algoritmos.
    /// </remarks>
    /// <typeparam name="ElementType">O tipo de objectos sob conversão.</typeparam>
    public class ElementToElementConversion<ElementType> : IConversion<ElementType, ElementType>
    {

        /// <summary>
        /// Determina se é possível converter o objecto.
        /// </summary>
        /// <param name="objectToConvert">O objecto a ser analisado.</param>
        /// <returns>Verdadeiro.</returns>
        public bool CanApplyDirectConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determina se é possível converter o objecto.
        /// </summary>
        /// <param name="objectToConvert">O objecto a ser analisado.</param>
        /// <returns>Verdadeiro.</returns>
        public bool CanApplyInverseConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// Converte um objecto num objecto do mesmo tipo.
        /// </summary>
        /// <param name="objectToConvert">O objecto a ser convertido.</param>
        /// <returns>O resultado da conversão.</returns>
        /// <exception cref="ArgumentNullException">Caso o objecto passado seja nulo.</exception>
        public ElementType DirectConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else
            {
                return objectToConvert;
            }
        }

        /// <summary>
        /// Converte um obejcto num objecto do mesmo tipo.
        /// </summary>
        /// <param name="objectToConvert">O objecto a ser convertido.</param>
        /// <returns>O resultado da conversão.</returns>
        /// <exception cref="ArgumentNullException">Caso o objecto passado seja nulo.</exception>
        public ElementType InverseConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else
            {
                return objectToConvert;
            }
        }
    }

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
        /// <value>
        /// O domínio responsável pelas operações sobre os coeficientes.
        /// </value>
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
        /// <exception cref="ArgumentNullException">Caso o objecto passado seja nulo.</exception>
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

    /// <summary>
    /// Permite converter de um número de precisão dupla para um inteiro 
    /// <see cref="int"/>.
    /// </summary>
    public class DoubleToIntegerConversion : IConversion<int, double>
    {
        /// <summary>
        /// A precisão que se pretende considerar.
        /// </summary>
        private double precision;

        /// <summary>
        /// Permite instanciar um conversor de ponto flutuante para inteiro.
        /// </summary>
        /// <remarks>
        /// Um número será considerado inteiro caso o seu valor diferir de um valor inteiro em um valor inferior
        /// à precisão estabelecida.
        /// </remarks>
        /// <param name="precision">
        /// A precisão a ter em conta na comparação de valores. Será considerado o módulo do valor fornecido.
        /// </param>
        public DoubleToIntegerConversion(double precision = 0.0)
        {
            this.precision = Math.Abs(precision);
        }

        /// <summary>
        /// Indica se é possível converter o número de precisão dupla para inteiro.
        /// </summary>
        /// <param name="objectToConvert">O número em análise.</param>
        /// <returns>Verdadeiro caso a conversão seja possível e falso caso contrário.</returns>
        public bool CanApplyDirectConversion(double objectToConvert)
        {
            var value = Math.Round(objectToConvert);
            if (this.precision == 0 && value == objectToConvert)
            {
                return true;
            }
            else if (value < objectToConvert + this.precision && value > objectToConvert - this.precision)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Indica se é possível converter um número inteiro para um número de precisão dupla.
        /// </summary>
        /// <remarks>
        /// Esta conversão é sempre possível.
        /// </remarks>
        /// <param name="objectToConvert">O número inteiro.</param>
        /// <returns>Verdadeiro.</returns>
        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return true;
        }

        /// <summary>
        /// Obtém o resultado da conversão de um número de precisão dupla num inteiro.
        /// </summary>
        /// <param name="objectToConvert">O número de precisão dupla.</param>
        /// <returns>O inteiro convertido.</returns>
        /// <exception cref="MathematicsException">Caso o número de precisão dupla não represente um valor inteiro.</exception>
        public int DirectConversion(double objectToConvert)
        {
            var value = Math.Round(objectToConvert);
            if (this.precision == 0 && value == objectToConvert)
            {
                return (int)value;
            }
            else if (value < objectToConvert + this.precision && value > objectToConvert - this.precision)
            {

                return (int)value;
            }
            else
            {
                throw new MathematicsException(string.Format("Can't convert value {0} to integer.", objectToConvert));
            }
        }

        /// <summary>
        /// Converte um número inteiro num número de precisão dupla.
        /// </summary>
        /// <param name="objectToConvert">O número inteiro a ser convertido.</param>
        /// <returns>O número de precisão dupla.</returns>
        public double InverseConversion(int objectToConvert)
        {
            return objectToConvert;
        }
    }

    /// <summary>
    /// Permite realizar conversões entre inteiros e longos.
    /// </summary>
    public class LongToIntegerConversion : IConversion<int, long>
    {
        /// <summary>
        /// Determina se é possível converter um número longo num número inteiro.
        /// </summary>
        /// <param name="objectToConvert">O número longo a ser analisado.</param>
        /// <returns>Veradeiro caso a conversão seja possível e falso caso contrário.</returns>
        public bool CanApplyDirectConversion(long objectToConvert)
        {
            return objectToConvert <= int.MaxValue && objectToConvert >= int.MinValue;
        }

        /// <summary>
        /// Determina se é possível converter um número inteiro num número longo.
        /// </summary>
        /// <param name="objectToConvert">O número inteiro.</param>
        /// <returns>Veradeiro caso a conversão seja possível e falso caso contrário.</returns>
        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return true;
        }

        /// <summary>
        /// Converte um número longo num número inteiro.
        /// </summary>
        /// <param name="objectToConvert">O número longo.</param>
        /// <returns>O número inteiro.</returns>
        public int DirectConversion(long objectToConvert)
        {
            return (int)objectToConvert;
        }

        /// <summary>
        /// Converte um número inteiro num número longo.
        /// </summary>
        /// <param name="objectToConvert">O número inteiro.</param>
        /// <returns>O número longo.</returns>
        public long InverseConversion(int objectToConvert)
        {
            return objectToConvert;
        }
    }

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

        /// <summary>
        /// Instancia um novo objecto do tipo
        /// <see cref="OuterElementFractionConversion{OutElementType, FractionElementType}"/>.
        /// </summary>
        /// <param name="outTypeToFractionTypeConversion">
        /// O objecto do tipo correspondente aos elementos externos.
        /// </param>
        /// <param name="domain">O domínio.</param>
        /// <exception cref="ArgumentNullException">
        /// Se algum dos argumentos for nulo.
        /// </exception>
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

    /// <summary>
    /// Permite converter um polinómio para um inteiro.
    /// </summary>
    /// <typeparam name="CoeffsType">O tipo dos objectos que constituem os coeficientes.</typeparam>
    public class UnivarPolynomNormalFormToIntegerConversion<CoeffsType>
        : IConversion<int, UnivariatePolynomialNormalForm<CoeffsType>>
    {
        /// <summary>
        /// O anel responsável pela determinaçao do zero e da unidade dos coeficientes.
        /// </summary>
        private IRing<CoeffsType> coeffsRing;

        /// <summary>
        /// O conversor responsável por tentar converter um coeficiente para um valor inteiro.
        /// </summary>
        private IConversion<int, CoeffsType> coeffsConversion;

        /// <summary>
        /// A variável associada ao polinómio convertido.
        /// </summary>
        private string variableName;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="UnivarPolynomNormalFormToIntegerConversion{CoeffsType}"/>.
        /// </summary>
        /// <param name="variableName">O nome da variável dos polinómios sobre os quais se efectuam as operações.</param>
        /// <param name="coeffsConversion">O conversor que permite converter inteiros em coeficientes.</param>
        /// <param name="coeffsRing">O anel responsável pelas operações sobre os coeficientes.</param>
        public UnivarPolynomNormalFormToIntegerConversion(
            string variableName,
            IConversion<int, CoeffsType> coeffsConversion,
            IRing<CoeffsType> coeffsRing)
        {
            if (coeffsRing == null)
            {
                throw new ArgumentNullException("coeffsRing");
            }
            if (coeffsConversion == null)
            {
                throw new ArgumentNullException("coeffsConversion");
            }
            else if (string.IsNullOrWhiteSpace(variableName))
            {
                throw new MathematicsException("Variable name can't be empty.");
            }
            else
            {
                this.coeffsConversion = coeffsConversion;
                this.coeffsRing = coeffsRing;
                this.variableName = variableName;
            }
        }

        /// <summary>
        /// Indica se é possível converter um polinómio para inteiro.
        /// </summary>
        /// <param name="objectToConvert">O polinómio em análise.</param>
        /// <returns>Verdadeiro caso a conversão seja possível e falso caso contrário.</returns>
        public bool CanApplyDirectConversion(UnivariatePolynomialNormalForm<CoeffsType> objectToConvert)
        {
            if (objectToConvert == null)
            {
                return false;
            }
            else if (objectToConvert.IsValue)
            {
                var value = objectToConvert.GetAsValue(this.coeffsRing);
                return this.coeffsConversion.CanApplyDirectConversion(value);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Indica se é possível converter um número inteiro para um polinómio.
        /// </summary>
        /// <remarks>
        /// Esta conversão é sempre possível se for possível converter o inteiro num coeficiente.
        /// </remarks>
        /// <param name="objectToConvert">O número inteiro.</param>
        /// <returns>Verdadeiro caso a conversão seja possível e falso caso contrário.</returns>
        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return this.coeffsConversion.CanApplyInverseConversion(objectToConvert);
        }

        /// <summary>
        /// Obtém o resultado da conversão de um polinómio num inteiro.
        /// </summary>
        /// <param name="objectToConvert">O polinómio.</param>
        /// <returns>O inteiro convertido.</returns>
        /// <exception cref="ArgumentNullException">Se o polinómio proporcionado for nulo.</exception>
        /// <exception cref="MathematicsException">Caso o polinómio não represente um valor inteiro.</exception>
        public int DirectConversion(UnivariatePolynomialNormalForm<CoeffsType> objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else if (objectToConvert.IsValue)
            {
                var value = objectToConvert.GetAsValue(this.coeffsRing);
                return this.coeffsConversion.DirectConversion(value);
            }
            else
            {
                throw new MathematicsException(string.Format(
                    "Can't convert {0} to integer.",
                    objectToConvert));
            }
        }

        /// <summary>
        /// Converte um número inteiro num polinómio.
        /// </summary>
        /// <param name="objectToConvert">O número inteiro.</param>
        /// <returns>O polinómio.</returns>
        public UnivariatePolynomialNormalForm<CoeffsType> InverseConversion(int objectToConvert)
        {
            var value = this.coeffsConversion.InverseConversion(objectToConvert);
            return new UnivariatePolynomialNormalForm<CoeffsType>(
                value,
                0,
                variableName,
                this.coeffsRing);
        }
    }
}
