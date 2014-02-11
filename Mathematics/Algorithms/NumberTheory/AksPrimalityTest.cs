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

        /// <summary>
        /// O objecto responsável pelo cálculo da função totient.
        /// </summary>
        private IAlgorithm<int, int> totientFunctionAlg;

        /// <summary>
        /// O objecto responsável pela instanciação de um corpo modular.
        /// </summary>
        private IModularFieldFactory<int> modularFactory;

        /// <summary>
        /// O domínio que permite efectuar operações sobre inteiros.
        /// </summary>
        private IIntegerNumber<int> integerNumber;

        /// <summary>
        /// Instancia uma nova instância da classe que implementa o algoritmo AKS.
        /// </summary>
        public AksPrimalityTest()
        {
            this.perfectPowerTest = new IntPerfectPowerTestAlg(
                new PrimeNumbersIteratorFactory());
            this.integerNumber = new IntegerDomain();
            this.totientFunctionAlg = new EulerTotFuncAlg<int>(
                new IntegerSquareRootAlgorithm(),
                new PrimeNumbersIteratorFactory(),
                this.integerNumber);
            this.modularFactory = new ModularIntegerFieldFactory();
        }

        /// <summary>
        /// Averigua se o número especificado é primo.
        /// </summary>
        /// <param name="data">O número.</param>
        /// <returns>Verdadeiro caso o número seja primo e falso caso este seja composto.</returns>
        public bool Run(int data)
        {
            if (this.integerNumber.IsAdditiveUnity(data))
            {
                return false;
            }
            else if (
                this.integerNumber.IsMultiplicativeUnity(data) ||
                this.integerNumber.IsMultiplicativeUnity(this.integerNumber.AdditiveInverse(data)))
            {
                return false;
            }
            else
            {
                var innerData = this.integerNumber.GetNorm(data);
                var two = this.integerNumber.MapFrom(2);
                if (this.integerNumber.Equals(innerData, two))
                {
                    return true;
                }
                else if (this.perfectPowerTest.Run(data))
                {
                    return false;
                }
                else
                {
                    var log = Math.Log(innerData)/Math.Log(2);
                    var r = this.EvaluateLimitValue(innerData, log);
                    r = this.totientFunctionAlg.Run(r);
                    var i = two;
                    for (;this.integerNumber.Compare(i ,r)<=0; i = this.integerNumber.Successor(i))
                    {
                        var gcd = MathFunctions.GreatCommonDivisor(innerData, i, this.integerNumber);
                        if (this.integerNumber.Compare(gcd, this.integerNumber.MultiplicativeUnity) > 0 &&
                            this.integerNumber.Compare(gcd, innerData) < 0)
                        {
                            return false;
                        }
                    }

                    var limit = (int)Math.Floor(Math.Sqrt(r) * log);
                    var modularField = this.modularFactory.CreateInstance(innerData);
                    var terms = new Dictionary<int, int>();

                    var modularPolynomialRing = new AuxAksModArithmRing<int>(
                        r,
                        "x",
                        modularField);
                    for (int j = 1; j <= limit; ++j)
                    {
                        terms.Clear();
                        terms.Add(0, j);
                        terms.Add(1, 1);
                        var polynomial = new UnivariatePolynomialNormalForm<int>(
                        terms,
                        "x",
                        modularField);

                        terms.Clear();
                        terms.Add(0, j);
                        terms.Add(innerData, 1);
                        var comparisionPol = new UnivariatePolynomialNormalForm<int>(
                        terms,
                        "x",
                        modularField);

                        var poweredPol = MathFunctions.Power(polynomial, innerData, modularPolynomialRing);
                        if (!poweredPol.Equals(modularPolynomialRing.GetReduced(comparisionPol)))
                        {
                            return false;
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
                var order = 1;
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

            return 2;
        }
    }
}
