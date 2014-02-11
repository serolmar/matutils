namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o cálculo da raiz de índice arbitrário de um número.
    /// </summary>
    /// <typeparam name="NumberType">O tipo de número inteiro.</typeparam>
    public class GenericIntegerNthRootAlgorithm<NumberType> : IAlgorithm<NumberType, NumberType, NumberType>
    {
        IIntegerNumber<NumberType> integerNumber;

        public GenericIntegerNthRootAlgorithm(IIntegerNumber<NumberType> integerNumber)
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
        /// Calcula a parte inteira da raiz de ordem arbitrária de um número.
        /// </summary>
        /// <param name="index">O índice de raiz.</param>
        /// <param name="number">O número.</param>
        /// <returns>A raiz de ordem especificada de um número.</returns>
        public NumberType Run(NumberType index, NumberType number)
        {
            if (this.integerNumber.Compare(index, this.integerNumber.AdditiveUnity) <= 0)
            {
                throw new ArgumentException("Root index must be a positive number.");
            }
            else if (this.integerNumber.IsAdditiveUnity(index))
            {
                return number;
            }
            else if (this.integerNumber.IsAdditiveUnity(number))
            {
                return number;
            }
            else
            {
                var two = this.integerNumber.Successor(this.integerNumber.MultiplicativeUnity);
                var rem = this.integerNumber.Rem(index, two);
                if (this.integerNumber.IsAdditiveUnity(rem) &&
                    this.integerNumber.Compare(number, this.integerNumber.AdditiveUnity) < 0)
                {
                    throw new MathematicsException("When root index is even then the number must be non-negative.");
                }
                else
                {
                    var innerNumber = this.integerNumber.GetNorm(number);

                    // Cálculo da aproximação inicial da matriz
                    var r = two;
                    var pr = MathFunctions.Power(two, index, this.integerNumber, this.integerNumber);
                    var sr = this.integerNumber.MultiplicativeUnity;
                    var spr = this.integerNumber.MultiplicativeUnity;

                    while (this.integerNumber.Compare(pr, innerNumber) <= 0)
                    {
                        sr = r;
                        spr = pr;
                        r = this.integerNumber.Multiply(r, r);
                        pr = this.integerNumber.Multiply(pr, pr);
                    }

                    r = sr;
                    pr = spr;
                    var b = this.integerNumber.Quo(r, two);
                    if (this.integerNumber.Compare(b, this.integerNumber.AdditiveUnity) > 0)
                    {
                        var combinationCoeffs = this.ComputeCombinatorialNumbers(index);
                        var xCoeffs = this.ComputeQuietCoeffs(r, combinationCoeffs);

                        r = this.integerNumber.Add(r, b);
                        var delta = this.ComputeDelta(xCoeffs, b);
                        pr = this.integerNumber.Add(pr, delta);
                        if (this.integerNumber.Compare(pr, innerNumber) > 0)
                        {
                            r = sr;
                            pr = spr;
                        }
                        else
                        {
                            xCoeffs = this.ComputeQuietCoeffs(r, combinationCoeffs);
                        }

                        b = this.integerNumber.Quo(b, two);
                        while (this.integerNumber.Compare(b, this.integerNumber.AdditiveUnity) > 0)
                        {
                            sr = r;
                            spr = pr;

                            r = this.integerNumber.Add(r, b);
                            delta = this.ComputeDelta(xCoeffs, b);
                            pr = this.integerNumber.Add(pr, delta);
                            if (this.integerNumber.Compare(pr, innerNumber) > 0)
                            {
                                r = sr;
                                pr = spr;
                                b = this.integerNumber.Quo(b, two);
                            }
                            else
                            {
                                xCoeffs = this.ComputeQuietCoeffs(r, combinationCoeffs);
                            }
                        }
                    }

                    if (this.integerNumber.Compare(number, this.integerNumber.AdditiveUnity) < 0)
                    {
                        r = this.integerNumber.AdditiveInverse(r);
                    }

                    return r;
                }
            }
        }

        /// <summary>
        /// Calcula os coeficientes binomiais da linha do triângulo aritmético.
        /// </summary>
        /// <param name="number">O número da linha do triângulo aritmético.</param>
        /// <returns>A lista com os coeficientes binomiais.</returns>
        private List<NumberType> ComputeCombinatorialNumbers(NumberType number)
        {
            var result = new List<NumberType>();
            result.Add(number);
            var p = this.integerNumber.Predecessor(number);
            var n = number;
            var pred = this.integerNumber.Predecessor(number);
            var count = this.integerNumber.MultiplicativeUnity;
            while (this.integerNumber.Compare(count, pred) < 0)
            {
                var num = this.integerNumber.Multiply(n, p);
                var den = this.integerNumber.Successor(
                    this.integerNumber.Add(number, this.integerNumber.AdditiveInverse(p)));
                n = this.integerNumber.Quo(num, den);
                p = this.integerNumber.Predecessor(p);
                result.Add(n);
                count = this.integerNumber.Successor(count);
            }

            return result;
        }

        /// <summary>
        /// Permite calcular o valor dos coeficientes de (x+k)^n-x^k como coeficientes em k.
        /// </summary>
        /// <param name="x">O valor de x.</param>
        /// <param name="combinatorialCoeffs">O valor dos coeficientes combinatórios.</param>
        /// <returns>Os coeficientes do polinómio em k.</returns>
        private NumberType[] ComputeQuietCoeffs(NumberType x, List<NumberType> combinatorialCoeffs)
        {
            var result = new NumberType[combinatorialCoeffs.Count];
            var temp = x;
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = this.integerNumber.Multiply(combinatorialCoeffs[i], temp);
                temp = this.integerNumber.Multiply(temp, x);
            }

            return result;
        }

        /// <summary>
        /// Permite calcular o valor de (x+k)^n-x^k a partir dos coeficientes em k.
        /// </summary>
        /// <param name="coeffs">Os coeficientes em k.</param>
        /// <param name="k">O valor de k.</param>
        /// <returns>O valor de delta.</returns>
        private NumberType ComputeDelta(NumberType[] coeffs, NumberType k)
        {
            var result = k;
            for (int i = 0; i < coeffs.Length; ++i)
            {
                result = this.integerNumber.Add(result, coeffs[i]);
                result = this.integerNumber.Multiply(result, k);
            }

            return result;
        }
    }
}
