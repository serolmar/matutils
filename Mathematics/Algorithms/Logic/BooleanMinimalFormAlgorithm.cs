namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo habitual para simplificação de expressões lógicas colocadas na forma
    /// canónica. É útil em simplificação de esquemas electrónicos.
    /// </summary>
    public class BooleanMinimalFormAlgorithm : IAlgorithm<BooleanMinimalFormInOut, BooleanMinimalFormInOut>
    {
        /// <summary>
        /// Obtém a forma mínima para a lista de combinações lógicas fornecidas.
        /// </summary>
        /// <param name="data">A lista de combinações lógicas de entrada.</param>
        /// <returns>O resultado minimal.</returns>
        public BooleanMinimalFormInOut Run(BooleanMinimalFormInOut data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Organiza as combinações de acordo com o número de bits ligados.
        /// </summary>
        /// <param name="data">O conjunto das combinações.</param>
        /// <returns>A lista de combinações organizadas de acordo com o número de bits ligados.</returns>
        private List<BooleanCombination>[] GetSortedCombinations(BooleanMinimalFormInOut data)
        {
            var maxNumberOfEntries = data.Max(d => d.LogicInput.Length);
            var result = new List<BooleanCombination>[maxNumberOfEntries];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = new List<BooleanCombination>();
            }

            foreach (var combination in data)
            {
                if (combination.LogicOutput == EBooleanMinimalFormOutStatus.ON)
                {
                    var onBitsNumer = MathFunctions.CountSettedBits(combination.LogicInput);
                    result[onBitsNumer].Add(combination);
                }
            }

            return result;
        }

        /// <summary>
        /// Representa uma linha da tabela que auxilia a obtenção de implicantes primos.
        /// </summary>
        private class ProcessTableLine
        {
            private List<int> tableTuples;

            private BitArray tableCombinations;

            private bool isPrimeImplicant;

            public ProcessTableLine()
            {
                this.tableTuples = new List<int>();
            }

            public List<int> TableTuples
            {
                get
                {
                    return this.tableTuples;
                }
            }

            public BitArray TableCombinations
            {
                get
                {
                    return this.tableCombinations;
                }
                set
                {
                    this.tableCombinations = value;
                }
            }

            public bool IsPrimeImplicant
            {
                get
                {
                    return this.isPrimeImplicant;
                }
                set
                {
                    this.isPrimeImplicant = value;
                }
            }
        }
    }
}
