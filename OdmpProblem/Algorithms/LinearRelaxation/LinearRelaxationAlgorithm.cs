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
        /// <param name="costs">A matriz dos custos.</param>
        /// <returns>O resultado da relaxação.</returns>
        public SimplexOutput<CoeffType> Run(SparseDictionaryMatrix<CoeffType> costs)
        {
            var objectiveFunction = new List<CoeffType>();
            var numberOfVertices = costs.GetLength(1);
            foreach (var line in costs.GetLines())
            {
                foreach (var column in line.Value.GetColumns())
                {
                    if (column.Key != line.Key)
                    {
                        objectiveFunction.Add(column.Value);
                    }
                }
            }

            throw new NotImplementedException();
        }
    }
}
