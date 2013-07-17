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
    internal class IntegerFactorialFraction
    {
        /// <summary>
        /// O conjunto dos factores e respectiva potência no numerador.
        /// </summary>
        Dictionary<int, int> numeratorFactors;

        /// <summary>
        /// O conjunto dos factores e respectiva potência no denominador.
        /// </summary>
        Dictionary<int, int> denomimatorFactors;

        public IntegerFactorialFraction()
        {
            this.numeratorFactors = new Dictionary<int,int>();
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
        /// Multiplica o denominador pelo factorial correspondente.
        /// </summary>
        /// <param name="factorialValue">O factorial.</param>
        public void MultiplyNumerator(int factorialValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Multiplica o denominador pelo factorial correspondente.
        /// </summary>
        /// <param name="factorialValue">O factorial.</param>
        public void MultiplyDenominator(int factorialValue)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calcula o produto de um dicionário com os factores e respectivas potências.
        /// </summary>
        /// <param name="factors">O dicionário com os factores e respectivas potências.</param>
        /// <returns>O valor do produto.</returns>
        private int ComputeProduct(Dictionary<int, int> factors)
        {
            var result = 1;
            throw new NotImplementedException();
        }
    }
}
