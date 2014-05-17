namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um resultado do algoritmo <see cref="BooleanMinimalFormAlgorithm"/>.
    /// </summary>
    public class BooleanMinimalFormInOut : IEnumerable<BooleanCombination>
    {
        /// <summary>
        /// Mantém a lista de combinações lógicas.
        /// </summary>
        private List<BooleanCombination> combinationElements;

        /// <summary>
        /// O máximo dos vários números de combinações por elemento.
        /// </summary>
        private int maxCombinationsLength;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BooleanMinimalFormInOut"/>.
        /// </summary>
        public BooleanMinimalFormInOut()
        {
            this.combinationElements = new List<BooleanCombination>();
            this.maxCombinationsLength = 0;
        }

        /// <summary>
        /// Obtém a combinação que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor que se encontra na posição especificada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice for negativo ou não for inferior ao número de elementos no conjunto de combinações.
        /// </exception>
        public BooleanCombination this[int index]
        {
            get
            {
                if (index < 0 || index >= this.combinationElements.Count)
                {
                    throw new IndexOutOfRangeException("The index is out of range.");
                }
                else
                {
                    return this.combinationElements[index];
                }
            }
        }

        /// <summary>
        /// Obtém o número de combinações.
        /// </summary>
        /// <value>O número de combinações.</value>
        public int Count
        {
            get
            {
                return this.combinationElements.Count;
            }
        }

        /// <summary>
        /// Obtém o número máximo de combinações por elemento.
        /// </summary>
        /// <value>O número máximo de combinações por elemento.</value>
        public int MaxCombinationsLength
        {
            get
            {
                return this.maxCombinationsLength;
            }
        }

        /// <summary>
        /// Adiciona uma combinação à lista de entrada/saída.
        /// </summary>
        /// <param name="array">O vector de bits que representa uma entrada.</param>
        /// <param name="outputStatus">O estado de saída associado à entrada.</param>
        /// <returns>A representação interna.</returns>
        /// <exception cref="ArgumentNullException">Se o vector de bits for nulo.</exception>
        public BooleanCombination Add(
            LogicCombinationBitArray array,
            EBooleanMinimalFormOutStatus outputStatus)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else
            {
                var result = new BooleanCombination(
                    array,
                    outputStatus);
                this.combinationElements.Add(result);
                this.maxCombinationsLength = Math.Max(
                    this.maxCombinationsLength,
                    array.Length);

                return result;
            }
        }

        /// <summary>
        /// Remove a combinação especificada da lista de combinaçõse.
        /// </summary>
        /// <param name="combination">A combinação a ser removida.</param>
        public void Remove(BooleanCombination combination)
        {
            if (combination != null)
            {
                if (this.combinationElements.Remove(combination))
                {
                    if (combination.LogicInput.Length == this.maxCombinationsLength)
                    {
                        this.maxCombinationsLength = this.combinationElements.Max(ce => ce.LogicInput.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Obtém um enumerador para as combinações lógicas.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<BooleanCombination> GetEnumerator()
        {
            return this.combinationElements.GetEnumerator();
        }

        /// <summary>
        /// Obtém o enumerador não-genérico para as combinações lógicas.
        /// </summary>
        /// <returns>O enumerador.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
