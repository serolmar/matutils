namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    /// <summary>
    /// Determina a solução da relaxação linear associada a uma matriz completa.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos coeficientes na matriz.</typeparam>
    public class LinearRelaxationAlgorithm<CoeffType>
        : IAlgorithm<SparseDictionaryMatrix<CoeffType>, SimplexOutput<CoeffType>>
    {
        /// <summary>
        /// Aplica o algoritmo sobre a matriz dos custos.
        /// </summary>
        /// <param name="data">A matriz dos custos.</param>
        /// <returns>O resultado da relaxação.</returns>
        public SimplexOutput<CoeffType> Run(SparseDictionaryMatrix<CoeffType> data)
        {
            throw new NotImplementedException();
        }
    }
}
