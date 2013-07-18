using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mathematics.Algorithms;

namespace Mathematics.AlgebraicStructures.Polynomial
{
    /// <summary>
    /// Classe útil na determinação do valor dos coeficientes
    /// do produto de monómios simétricos.
    /// </summary>
    public class IntegerFactorialFraction
    {
        /// <summary>
        /// O domínio responsável pelas multiplicações.
        /// </summary>
        private IntegerDomain integerEuclideanDomain;

        /// <summary>
        /// O conjunto dos factores e respectiva potência no numerador.
        /// </summary>
        private Dictionary<int, int> numeratorFactors;

        /// <summary>
        /// O conjunto dos factores e respectiva potência no denominador.
        /// </summary>
        private Dictionary<int, int> denomimatorFactors;

        public IntegerFactorialFraction()
        {
            this.integerEuclideanDomain = new IntegerDomain();
            this.numeratorFactors = new Dictionary<int, int>();
            this.denomimatorFactors = new Dictionary<int, int>();
        }

        /// <summary>
        /// Obtém o valor calculado do numerador.
        /// </summary>
        public int Numerator
        {
            get
            {
                return this.ComputeProduct(this.numeratorFactors);
            }
        }

        /// <summary>
        /// Obtém o valor calculado do denominador.
        /// </summary>
        public int Denominator
        {
            get
            {
                return this.ComputeProduct(this.denomimatorFactors);
            }
        }

        /// <summary>
        /// Inverte a fracção.
        /// </summary>
        public void Invert()
        {
            var temp = this.numeratorFactors;
            this.numeratorFactors = this.denomimatorFactors;
            this.denomimatorFactors = temp;
        }

        /// <summary>
        /// Multiplica o denominador pelo factorial correspondente.
        /// </summary>
        /// <param name="factorialValue">O factorial.</param>
        public void MultiplyNumerator(int factorialValue)
        {
            this.Multiply(this.numeratorFactors, this.denomimatorFactors, factorialValue);
        }

        /// <summary>
        /// Multiplica o denominador pelo factorial correspondente.
        /// </summary>
        /// <param name="factorialValue">O factorial.</param>
        public void MultiplyDenominator(int factorialValue)
        {
            this.Multiply(this.denomimatorFactors, this.numeratorFactors, factorialValue);
        }

        /// <summary>
        /// Multiplica o denominador pelo valor correspondente.
        /// </summary>
        /// <param name="factorialValue">O valor.</param>
        public void NumeratorNumberMultiply(int number)
        {
            if (number == 0)
            {
                throw new ArgumentException("Can't multiply by zero.");
            }
            else
            {
                this.MultiplyNumber(this.numeratorFactors, this.denomimatorFactors, number);
            }
        }

        /// <summary>
        /// Multiplica o denominador pelo valor correspondente.
        /// </summary>
        /// <param name="factorialValue">O valor.</param>
        public void DenominatorNumberMultiply(int number)
        {
            if (number == 0)
            {
                throw new ArgumentException("Can't multiply by zero.");
            }
            else
            {
                return this.MultiplyNumber(this.denomimatorFactors, this.numeratorFactors, number);
            }
        }

        /// <summary>
        /// Obtém a representação textual do objecto.
        /// </summary>
        /// <returns>A representação textual do objecto.</returns>
        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("Numerator: ");
            var termsEnumerator = this.numeratorFactors.GetEnumerator();
            if (termsEnumerator.MoveNext())
            {
                var current = termsEnumerator.Current;
                resultBuilder.Append(current.Key);
                if (current.Value != 1)
                {
                    resultBuilder.AppendFormat("^{0}", current.Value);
                }

                while (termsEnumerator.MoveNext())
                {
                    current = termsEnumerator.Current;
                    resultBuilder.AppendFormat("*{0}", current.Key);
                    if (current.Value != 1)
                    {
                        resultBuilder.AppendFormat("^{0}", current.Value);
                    }
                }
            }
            else
            {
                resultBuilder.Append("1");
            }

            resultBuilder.Append("; Denominator: ");
            termsEnumerator = this.denomimatorFactors.GetEnumerator();
            if (termsEnumerator.MoveNext())
            {
                var current = termsEnumerator.Current;
                resultBuilder.Append(current.Key);
                if (current.Value != 1)
                {
                    resultBuilder.AppendFormat("^{0}", current.Value);
                }

                while (termsEnumerator.MoveNext())
                {
                    current = termsEnumerator.Current;
                    resultBuilder.AppendFormat("*{0}", current.Key);
                    if (current.Value != 1)
                    {
                        resultBuilder.AppendFormat("^{0}", current.Value);
                    }
                }
            }
            else
            {
                resultBuilder.Append("1");
            }

            return resultBuilder.ToString();
        }

        /// <summary>
        /// Multiplica um dos termos, dividindo o outro.
        /// </summary>
        /// <param name="main">O termo a ser multiplicado.</param>
        /// <param name="subsidiary">O termo a ser dividido.</param>
        /// <param name="factorialValue">O valor do factorial a ser multiplicado.</param>
        private void Multiply(Dictionary<int, int> main,
            Dictionary<int, int> subsidiary,
            int factorialValue)
        {
            if (factorialValue < 0)
            {
                throw new ArgumentOutOfRangeException("Factorial value must be non-negative.");
            }
            else
            {
                var factorsList = new List<int>();
                var auxiliary = new Dictionary<int, int>();
                for (int i = 2; i <= factorialValue; ++i)
                {
                    var currentValue = i;
                    var subsidiaryEnumerator = subsidiary.GetEnumerator();
                    var state = subsidiaryEnumerator.MoveNext();
                    var currentKvp = subsidiaryEnumerator.Current.Key;
                    var currentDegree = subsidiaryEnumerator.Current.Value;
                    while (state)
                    {
                        if (currentKvp == 0)
                        {
                            currentKvp = subsidiaryEnumerator.Current.Key;
                            currentDegree = subsidiaryEnumerator.Current.Value;
                        }

                        if (currentValue == 1)
                        {
                            if (currentDegree != 0)
                            {
                                var degree = 0;
                                if (auxiliary.TryGetValue(currentKvp, out degree))
                                {
                                    auxiliary[currentKvp] = degree + currentDegree;
                                }
                                else
                                {
                                    auxiliary.Add(currentKvp, currentDegree);
                                }
                            }

                            state = subsidiaryEnumerator.MoveNext();
                            currentKvp = 0;
                        }
                        else
                        {
                            var gcd = MathFunctions.GreatCommonDivisor(currentValue, currentKvp, this.integerEuclideanDomain);
                            if (gcd == 1)
                            {
                                var degree = 0;
                                if (auxiliary.TryGetValue(currentKvp, out degree))
                                {
                                    auxiliary[currentKvp] = degree + currentDegree;
                                }
                                else
                                {
                                    auxiliary.Add(currentKvp, currentDegree);
                                }

                                state = subsidiaryEnumerator.MoveNext();
                                currentKvp = 0;
                            }
                            else
                            {
                                var currentKvpAux = currentKvp / gcd;
                                currentValue = currentValue / gcd;
                                currentDegree = currentDegree - 1;
                                if (currentDegree == 0)
                                {
                                    if (currentKvpAux != 1)
                                    {
                                        var degree = 0;
                                        if (auxiliary.TryGetValue(currentKvp, out degree))
                                        {
                                            auxiliary[currentKvp] = degree + 1;
                                        }
                                        else
                                        {
                                            auxiliary.Add(currentKvp, 1);
                                        }

                                        currentKvp = currentKvpAux;
                                    }
                                    else
                                    {
                                        state = subsidiaryEnumerator.MoveNext();
                                        currentKvp = 0;
                                    }
                                }
                                else
                                {
                                    if (currentValue == 1)
                                    {
                                        var degree = 0;
                                        if (auxiliary.TryGetValue(currentKvp, out degree))
                                        {
                                            auxiliary[currentKvp] = degree + currentDegree;
                                        }
                                        else
                                        {
                                            auxiliary.Add(currentKvp, currentDegree);
                                        }

                                        subsidiaryEnumerator.MoveNext();
                                        currentKvp = 0;
                                    }
                                }
                            }
                        }
                    }

                    if (currentValue != 1)
                    {
                        factorsList.Add(currentValue);
                    }

                    subsidiary.Clear();
                    foreach (var kvp in auxiliary)
                    {
                        subsidiary.Add(kvp.Key, kvp.Value);
                    }

                    auxiliary.Clear();
                }

                foreach (var factor in factorsList)
                {
                    var degree = 0;
                    if (main.TryGetValue(factor, out degree))
                    {
                        main[factor] = degree + 1;
                    }
                    else
                    {
                        main.Add(factor, 1);
                    }
                }
            }
        }

        /// <summary>
        /// Multiplica um dos termos, dividindo o outro.
        /// </summary>
        /// <param name="main">O termo a ser multiplicado.</param>
        /// <param name="subsidiary">O termo a ser dividido.</param>
        /// <param name="factorValue">O valor a ser multiplicado.</param>
        private void MultiplyNumber(
            Dictionary<int, int> main,
            Dictionary<int, int> subsidiary,
            int factorValue)
        {
            var currentValue = factorValue;
            var auxiliary = new Dictionary<int, int>();
            var subsidiaryEnumerator = subsidiary.GetEnumerator();
            var state = subsidiaryEnumerator.MoveNext();
            var currentKvp = subsidiaryEnumerator.Current.Key;
            var currentDegree = subsidiaryEnumerator.Current.Value;
            while (state)
            {
                if (currentKvp == 0)
                {
                    currentKvp = subsidiaryEnumerator.Current.Key;
                    currentDegree = subsidiaryEnumerator.Current.Value;
                }

                if (currentValue == 1)
                {
                    if (currentDegree != 0)
                    {
                        var degree = 0;
                        if (auxiliary.TryGetValue(currentKvp, out degree))
                        {
                            auxiliary[currentKvp] = degree + currentDegree;
                        }
                        else
                        {
                            auxiliary.Add(currentKvp, currentDegree);
                        }
                    }

                    state = subsidiaryEnumerator.MoveNext();
                    currentKvp = 0;
                }
                else
                {
                    var gcd = MathFunctions.GreatCommonDivisor(currentValue, currentKvp, this.integerEuclideanDomain);
                    if (gcd == 1)
                    {
                        var degree = 0;
                        if (auxiliary.TryGetValue(currentKvp, out degree))
                        {
                            auxiliary[currentKvp] = degree + currentDegree;
                        }
                        else
                        {
                            auxiliary.Add(currentKvp, currentDegree);
                        }

                        state = subsidiaryEnumerator.MoveNext();
                        currentKvp = 0;
                    }
                    else
                    {
                        var currentKvpAux = currentKvp / gcd;
                        currentValue = currentValue / gcd;
                        currentDegree = currentDegree - 1;
                        if (currentDegree == 0)
                        {
                            if (currentKvpAux != 1)
                            {
                                var degree = 0;
                                if (auxiliary.TryGetValue(currentKvp, out degree))
                                {
                                    auxiliary[currentKvp] = degree + 1;
                                }
                                else
                                {
                                    auxiliary.Add(currentKvp, 1);
                                }

                                currentKvp = currentKvpAux;
                            }
                            else
                            {
                                state = subsidiaryEnumerator.MoveNext();
                                currentKvp = 0;
                            }
                        }
                        else
                        {
                            if (currentValue == 1)
                            {
                                var degree = 0;
                                if (auxiliary.TryGetValue(currentKvp, out degree))
                                {
                                    auxiliary[currentKvp] = degree + currentDegree;
                                }
                                else
                                {
                                    auxiliary.Add(currentKvp, currentDegree);
                                }

                                subsidiaryEnumerator.MoveNext();
                                currentKvp = 0;
                            }
                        }
                    }
                }
            }

            subsidiary.Clear();
            foreach (var kvp in auxiliary)
            {
                subsidiary.Add(kvp.Key, kvp.Value);
            }

            if (currentValue != 1)
            {
                var mainDegree = 0;
                if (main.TryGetValue(currentValue, out mainDegree))
                {
                    main[currentValue] = mainDegree + 1;
                }
                else
                {
                    main.Add(currentValue, 1);
                }
            }
        }

        /// <summary>
        /// Calcula o produto de um dicionário com os factores e respectivas potências.
        /// </summary>
        /// <param name="factors">O dicionário com os factores e respectivas potências.</param>
        /// <returns>O valor do produto.</returns>
        private int ComputeProduct(Dictionary<int, int> factors)
        {
            var result = 1;
            foreach (var kvp in factors)
            {
                if (kvp.Value == 1)
                {
                    result *= kvp.Key;
                }
                else if (kvp.Value == 2)
                {
                    result *= kvp.Key * kvp.Key;
                }
                else
                {
                    result *= MathFunctions.Power(kvp.Key, kvp.Value, this.integerEuclideanDomain);
                }
            }

            return result;
        }
    }
}
