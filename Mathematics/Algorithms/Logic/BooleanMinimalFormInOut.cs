namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BooleanMinimalFormInOut : IEnumerable<BooleanCombination>
    {
        /// <summary>
        /// Mantém a lista de combinações lógicas.
        /// </summary>
        private List<BooleanCombination> combinationElements;

        public BooleanMinimalFormInOut()
        {
            this.combinationElements = new List<BooleanCombination>();
        }

        /// <summary>
        /// Adiciona uma combinação à lista de entrada/saída.
        /// </summary>
        /// <param name="array">O vector de bits que representa uma entrada.</param>
        /// <param name="outputStatus">O estado de saída associado à entrada.</param>
        /// <returns>A representação interna.</returns>
        public BooleanCombination Add(
            BitArray array, 
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
                this.combinationElements.Remove(combination);
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
