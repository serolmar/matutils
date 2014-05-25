namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite calcular a parte inteira do logaritmo numa base inteira arbitrária de um número.
    /// </summary>
    /// <typeparam name="NumberType">O tipo de objectos que constituem os números.</typeparam>
    public class BaseLogIntegerPart<NumberType> : IAlgorithm<NumberType, NumberType, NumberType>
    {
        /// <summary>
        /// O domínio resposnável pelas operações sobre inteiros.
        /// </summary>
        IIntegerNumber<NumberType> integerDomain;

        /// <summary>
        /// Instancia o objecto responsável pelo cálculo da parte inteira do logaritmo binário de um número.
        /// </summary>
        /// <param name="integerDomain">O objecto responsável pelas operações sobre inteiros.</param>
        /// <exception cref="ArgumentNullException">Se o domínio for nulo.</exception>
        public BaseLogIntegerPart(IIntegerNumber<NumberType> integerDomain)
        {
            if (integerDomain == null)
            {
                throw new ArgumentNullException("integerDomain");
            }
            else
            {
                this.integerDomain = integerDomain;
            }
        }

        /// <summary>
        /// Determina a parte inteira do logaritmo numa base arbitrária do número especificado.
        /// </summary>
        /// <param name="baseNumber">A base do logaritmo.</param>
        /// <param name="data">O número.</param>
        /// <returns>A parte inteira do logaritmo binário.</returns>
        /// <exception cref="ArgumentException">Se p+elo menos um dos argumentos for nulo.</exception>
        public NumberType Run(NumberType baseNumber, NumberType data)
        {
            if (this.integerDomain.Compare(baseNumber, this.integerDomain.MultiplicativeUnity) < 0)
            {
                throw new ArgumentException("The base of logarithm must be greater than unity.");
            }
            else if (this.integerDomain.Compare(data, this.integerDomain.AdditiveUnity) <= 0)
            {
                throw new ArgumentException("Can only compute logarithms of positive numbers.");
            }
            else if (this.integerDomain.Equals(data, this.integerDomain.MultiplicativeUnity))
            {
                return this.integerDomain.AdditiveUnity;
            }
            else
            {
                var b = baseNumber;
                var lb = this.integerDomain.MultiplicativeUnity;
                var r = this.integerDomain.AdditiveUnity;
                var pr = this.integerDomain.MultiplicativeUnity;
                while (this.integerDomain.Compare(lb, this.integerDomain.AdditiveUnity) > 0)
                {
                    var sr = r;
                    var spr = pr;

                    r = this.integerDomain.Add(r, lb);
                    pr = this.integerDomain.Multiply(pr, b);
                    if (this.integerDomain.Compare(pr, data) > 0)
                    {
                        r = sr;
                        pr = spr;
                        b = baseNumber;
                        lb = this.integerDomain.MultiplicativeUnity;

                        r = this.integerDomain.Add(r, lb);
                        pr = this.integerDomain.Multiply(pr, b);
                        if (this.integerDomain.Compare(pr, data) > 0)
                        {
                            r = sr;
                            lb = this.integerDomain.AdditiveUnity;
                        }
                    }
                    else
                    {
                        b = this.integerDomain.Multiply(b, b);
                        lb = this.integerDomain.Add(lb, lb);
                    }
                }

                return r;
            }
        }
    }
}
