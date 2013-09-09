namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;
    using Utilities.Collections;

    /// <summary>
    /// Permite definir o algoritmo guloso básico.
    /// </summary>
    /// <typeparam name="ElementType">O tipo associado às entradas da matriz.</typeparam>
    public abstract class AGreedyAlg<ElementType>
        : IAlgorithm<List<SparseDictionaryMatrix<ElementType>>, int, List<GreedyAlgSolution<ElementType>>>
    {
        /// <summary>
        /// O objecto utilizado para bloquear os processos.
        /// </summary>
        protected object lockObject = new object();

        /// <summary>
        /// O anel responsável pelas operações sobre os elementos.
        /// </summary>
        protected IRing<ElementType> ring;

        /// <summary>
        /// Mantém o algoritmo responsável pela obtenção da próxima referência.
        /// </summary>
        protected IAlgorithm<
                IntegerSequence,
                SparseDictionaryMatrix<ElementType>,
                ElementType[],
                Tuple<int, ElementType>> referencesGetAlgorithm;

        public AGreedyAlg(
            IRing<ElementType> ring,
            IAlgorithm<
                IntegerSequence,
                SparseDictionaryMatrix<ElementType>,
                ElementType[],
                Tuple<int, ElementType>> referencesGetAlgorithm)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                this.referencesGetAlgorithm = referencesGetAlgorithm;
            }
        }

        public List<GreedyAlgSolution<ElementType>> Run(
            List<SparseDictionaryMatrix<ElementType>> matrices, 
            int referencesNumber)
        {
            throw new NotImplementedException();
        }
    }
}
