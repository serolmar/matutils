namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite calcular o símbolo de Legendre e Jacobi.
    /// </summary>
    /// <typeparam name="NumberType">O tipo de objectos que constituem números.</typeparam>
    public class LegendreJacobiSymbolAlgorithm<NumberType> : IAlgorithm<NumberType, NumberType, NumberType>
    {
        /// <summary>
        /// Mantém o objecto responsável pelas operações sobre inteiros.
        /// </summary>
        private IIntegerNumber<NumberType> integerNumber;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LegendreJacobiSymbolAlgorithm{NumberType}"/>.
        /// </summary>
        /// <param name="integerNumber">O objecto responsável pelas operações sobre os números inteiros.</param>
        /// <exception cref="ArgumentNullException">
        /// Se o objecto responsável pelas operações sobre os números inteiros for nulo.
        /// </exception>
        public LegendreJacobiSymbolAlgorithm(IIntegerNumber<NumberType> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else
            {
                this.integerNumber = integerNumber;
            }
        }

        /// <summary>
        /// Calcula o valor do símbolo de Jacobi para inteiros.
        /// </summary>
        /// <param name="topSymbolValue">O valor superior do símbolo.</param>
        /// <param name="bottomSymbolValue">O valor inferior do símbolo.</param>
        /// <returns>O valor numérico do símbolo.</returns>
        /// <exception cref="ArgumentException">Se o valor inferior do símbolo for nulo.</exception>
        public NumberType Run(NumberType topSymbolValue, NumberType bottomSymbolValue)
        {
            if (this.integerNumber.IsAdditiveUnity(bottomSymbolValue))
            {
                throw new ArgumentException("The bottom symbol value mustn't be zero.");
            }
            else if (this.integerNumber.IsAdditiveUnity(topSymbolValue))
            {
                return this.integerNumber.AdditiveUnity;
            }
            else if (this.integerNumber.IsAdditiveUnity(
                this.integerNumber.Rem(topSymbolValue, this.integerNumber.MapFrom(2))) &&
                this.integerNumber.IsAdditiveUnity(
                this.integerNumber.Rem(bottomSymbolValue, this.integerNumber.MapFrom(2))))
            {
                return this.integerNumber.AdditiveUnity;
            }
            else
            {
                var result = this.integerNumber.MultiplicativeUnity;
                var innerBottomSymbolValue = bottomSymbolValue;

                // Uma vez que J(p,2) = J(2,p)
                var two = this.integerNumber.MapFrom(2);
                var power = this.integerNumber.AdditiveUnity;
                var remQuoResult = this.integerNumber.GetQuotientAndRemainder(innerBottomSymbolValue, two);
                while (this.integerNumber.IsAdditiveUnity(remQuoResult.Remainder))
                {
                    power = this.integerNumber.Successor(power);
                    innerBottomSymbolValue = remQuoResult.Quotient;
                    remQuoResult = this.integerNumber.GetQuotientAndRemainder(innerBottomSymbolValue, two);
                }

                var topRemainder = this.integerNumber.AdditiveUnity;
                var eight = this.integerNumber.MapFrom(8);
                var three = this.integerNumber.MapFrom(3);
                var five = this.integerNumber.MapFrom(5);
                if (!this.integerNumber.IsAdditiveUnity(
                    this.integerNumber.Rem(power, two)))
                {
                    topRemainder = this.integerNumber.Rem(topSymbolValue, eight);
                    if (this.integerNumber.Equals(topRemainder, three) ||
                        this.integerNumber.Equals(topRemainder, five))
                    {
                        result = this.integerNumber.AdditiveInverse(result);
                    }
                }

                if (!this.integerNumber.IsMultiplicativeUnity(innerBottomSymbolValue))
                {
                    var innerTopSymbolValue = this.integerNumber.Rem(topSymbolValue, bottomSymbolValue);
                    var state = 0;
                    while (state != -1)
                    {
                        if (this.integerNumber.IsAdditiveUnity(innerTopSymbolValue))
                        {
                            result = this.integerNumber.AdditiveUnity;
                            state = -1;
                        }
                        else if (this.integerNumber.IsMultiplicativeUnity(innerTopSymbolValue))
                        {
                            state = -1;
                        }
                        else
                        {
                            power = this.integerNumber.AdditiveUnity;
                            remQuoResult = this.integerNumber.GetQuotientAndRemainder(innerTopSymbolValue, two);
                            while (this.integerNumber.IsAdditiveUnity(remQuoResult.Remainder))
                            {
                                power = this.integerNumber.Successor(power);
                                innerTopSymbolValue = this.integerNumber.Quo(innerTopSymbolValue, two);
                                remQuoResult = this.integerNumber.GetQuotientAndRemainder(innerTopSymbolValue, two);
                            }

                            var bottomRemainder = this.integerNumber.AdditiveUnity;
                            if (!this.integerNumber.IsAdditiveUnity(
                                this.integerNumber.Rem(power, two)))
                            {
                                bottomRemainder = this.integerNumber.Rem(innerBottomSymbolValue, eight);
                                if (this.integerNumber.Equals(bottomRemainder, three) || 
                                    this.integerNumber.Equals(bottomRemainder, five))
                                {
                                    result = this.integerNumber.AdditiveInverse(result);
                                }
                            }

                            var four = this.integerNumber.MapFrom(4);
                            if (!this.integerNumber.IsMultiplicativeUnity(innerTopSymbolValue))
                            {
                                bottomRemainder = this.integerNumber.Rem(innerBottomSymbolValue, four);
                                topRemainder = this.integerNumber.Rem(innerTopSymbolValue, four);
                                if (this.integerNumber.Equals(bottomRemainder, three) && 
                                    this.integerNumber.Equals(topRemainder, three))
                                {
                                    result = this.integerNumber.AdditiveInverse(result);
                                }

                                var temporaryBottomSymbolValue = innerBottomSymbolValue;
                                innerBottomSymbolValue = innerTopSymbolValue;
                                innerTopSymbolValue = this.integerNumber.Rem(
                                    temporaryBottomSymbolValue,
                                    innerTopSymbolValue);
                            }
                        }
                    }
                }

                return result;
            }
        }
    }
}
