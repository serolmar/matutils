namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SubsetSumLLLReductionAlgorithm<CoeffType> 
        : IAlgorithm<CoeffType[], CoeffType, IMonoid<CoeffType>, CoeffType[]>
    {
        /// <summary>
        /// Permite encontrar uma solução do problema da soma dos subconjunto.
        /// </summary>
        /// <remarks>
        /// Dado um conjunto de valores A={a[1], a[2], a[3], ...}, encontrar um subconjunto B contido em A,
        /// B={a[b1], a[b2], ...} de tal forma que a sua soma a[b1]+a[b2]+... seja igual a <see cref="sum"/>.
        /// O algoritmo implementado recorre à redução LLL.
        /// </remarks>
        /// <param name="coefficientValues">O conjunto de valores.</param>
        /// <param name="sum">A soma a ser encontrada.</param>
        /// <param name="monoid">O objecto responsável pelas adições.</param>
        /// <returns>O subconjunto do conjunto inicial cuja soma iguala o valor fornecido.</returns>
        public CoeffType[] Run(
            CoeffType[] coefficientValues, 
            CoeffType sum, 
            IMonoid<CoeffType> monoid)
        {
            throw new NotImplementedException();
        }
    }
}
