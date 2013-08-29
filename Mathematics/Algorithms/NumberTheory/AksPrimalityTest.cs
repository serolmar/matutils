namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo AKS cujo propósito consiste em averiguar
    /// se um número é primo.
    /// </summary>
    public class AksPrimalityTest : IAlgorithm<int, bool>
    {
        /// <summary>
        /// Algoritmo que permite determinar se um núemro constitui uma potência perfeita.
        /// </summary>
        private IAlgorithm<int, bool> perfectPowerTest;

        private IAlgorithm<int, int> totientFunctionAlg;

        /// <summary>
        /// O domínio que permite efectuar operações sobre inteiros.
        /// </summary>
        private IntegerDomain integerDomain;

        /// <summary>
        /// Instancia uma nova instância da classe que implementa o algoritmo AKS.
        /// </summary>
        public AksPrimalityTest()
        {
            this.perfectPowerTest = new PerfectPowerTestAlgorithm();
            this.totientFunctionAlg = new EulerTotFuncAlg();
            this.integerDomain = new IntegerDomain();
        }

        /// <summary>
        /// Averigua se o número especificado é primo.
        /// </summary>
        /// <param name="data">O número.</param>
        /// <returns>Verdadeiro caso o número seja primo e falso caso este seja composto.</returns>
        public bool Run(int data)
        {
            if (data == 0)
            {
                return false;
            }
            else if (data == 1 || data == -1)
            {
                return false;
            }
            else
            {
                var innerData = Math.Abs(data);
                if (innerData == 2)
                {
                    return true;
                }
                else if (this.perfectPowerTest.Run(data))
                {
                    return false;
                }
                else
                {
                    var log = Math.Log(innerData);
                    var r = this.EvaluateLimitValue(innerData, log);
                    for (int i = 2; i <= r; ++i)
                    {
                        var gcd = MathFunctions.GreatCommonDivisor(innerData, i, this.integerDomain);
                        if (gcd > 1 && gcd < innerData)
                        {
                            return false;
                        }
                    }

                    if (innerData <= r)
                    {
                        return true;
                    }
                    else
                    {
                        var limit = (int)Math.Floor(Math.Sqrt(innerData) * log);
                        var modularField = new ModularIntegerField(innerData);
                        var terms = new Dictionary<int, int>();

                        var lagrangeAlgorithm = new LagrangeAlgorithm<
                            UnivariatePolynomialNormalForm<int, ModularIntegerField>,
                            UnivarPolynomEuclideanDomain<int, ModularIntegerField>>(
                            new UnivarPolynomEuclideanDomain<int, ModularIntegerField>("x", modularField));
                        terms.Clear();
                        terms.Add(r, 1);
                        terms.Add(0, -1);
                        var modularPolynomial = new UnivariatePolynomialNormalForm<int, ModularIntegerField>(
                                terms,
                                "x",
                                modularField);
                        for (int i = 1; i <= limit; ++i)
                        {
                            terms.Clear();
                            terms.Add(0, i);
                            terms.Add(1, 1);
                            var polynomial = new UnivariatePolynomialNormalForm<int, ModularIntegerField>(
                            terms,
                            "x",
                            modularField);

                            terms.Clear();
                            terms.Add(0, i);
                            terms.Add(innerData, 1);
                            var comparisionPol = new UnivariatePolynomialNormalForm<int, ModularIntegerField>(
                            terms,
                            "x",
                            modularField);

                            var modularAritmeticField = new ModularBachetBezoutField<
                                                            UnivariatePolynomialNormalForm<int, ModularIntegerField>,
                                                            UnivarPolynomEuclideanDomain<int, ModularIntegerField>,
                                                            LagrangeAlgorithm<
                                                                UnivariatePolynomialNormalForm<int, ModularIntegerField>,
                                                                UnivarPolynomEuclideanDomain<int, ModularIntegerField>>>(
                                                                                            modularPolynomial,
                                                                                            lagrangeAlgorithm);
                            var poweredPol = MathFunctions.Power(polynomial, innerData, modularAritmeticField);
                            if (!poweredPol.Equals(modularAritmeticField.GetReduced(comparisionPol)))
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }
            }
        }

        /// <summary>
        /// Avalia o limite do valor sobre o qual se vão deteterminar os vários máximos divisores comuns.
        /// Na literatura surge frequentemente representado por r.
        /// </summary>
        /// <param name="data">O número a ser avaliado.</param>
        /// <returns>O valor de r.</returns>
        private int EvaluateLimitValue(int data, double log)
        {
            var squaredLog = log * log;
            for (int i = 2; i < data; ++i)
            {

                var order = 2;
                if (order > squaredLog)
                {
                    return i;
                }
                else
                {
                    ++order;
                    var power = (i * i) % data;
                    while (power != 1)
                    {
                        if (order > squaredLog)
                        {
                            return i;
                        }
                        else
                        {
                            power = (power * i) % data;
                            ++order;
                        }
                    }
                }
            }

            return -1;
        }
    }
}
