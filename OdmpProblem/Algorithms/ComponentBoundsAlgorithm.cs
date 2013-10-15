namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    /// <summary>
    /// Implementa o algoritmo de limitação das escolhas das medianas
    /// em cada componente.
    /// </summary>
    /// <typeparam name="CostType">O tipo de dados associado ao custo.</typeparam>
    public class ComponentBoundsAlgorithm<CostType>
        : IAlgorithm<int, List<List<CostType>>, List<List<CostType>>, ComponentBoundsItemResult[]>,
        IAlgorithm<int, List<List<CostType>>, IntMinWeightTdecompResult<CostType>, ComponentBoundsItemResult[]>
    {
        /// <summary>
        /// O anel responsável pelas operações.
        /// </summary>
        private IRing<CostType> ring;

        /// <summary>
        /// O comparador de custos.
        /// </summary>
        private IComparer<CostType> comparer;

        /// <summary>
        /// O algoritmo de decomposição de um inteiro.
        /// </summary>
        IAlgorithm<int, List<List<CostType>>, IntMinWeightTdecompResult<CostType>> minWeightDecompAlg;

        public ComponentBoundsAlgorithm(
            IAlgorithm<int, List<List<CostType>>, IntMinWeightTdecompResult<CostType>> minWeightDecompAlg,
            IComparer<CostType> comparer,
            IRing<CostType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else if (minWeightDecompAlg == null)
            {
                throw new ArgumentNullException("minWeightDecompAlg");
            }
            else
            {
                this.ring = ring;
                this.comparer = comparer;
                this.minWeightDecompAlg = minWeightDecompAlg;
            }
        }

        /// <summary>
        /// Executa o algoritmo sobre as matrizes dos limites inferiores e superiores
        /// tendo em conta o número de medianas escolhido.
        /// </summary>
        /// <param name="medians">O número de medianas.</param>
        /// <param name="lowerBoundCosts">Os custos associados aos limites inferiores.</param>
        /// <param name="upperBoundCosts">Os custos associados aos limites superiores.</param>
        /// <returns>O vector que contém os limites por componente.</returns>
        public ComponentBoundsItemResult[] Run(
            int medians,
            List<List<CostType>> lowerBoundCosts,
            List<List<CostType>> upperBoundCosts)
        {
            if (lowerBoundCosts == null)
            {
                throw new ArgumentNullException("lowerBoundCosts");
            }
            else if (upperBoundCosts == null)
            {
                throw new ArgumentNullException("upperBoundCosts");
            }
            else if (lowerBoundCosts.Count != upperBoundCosts.Count)
            {
                throw new ArgumentException(
                    "The number of components in lower bounds must match the number of componentes in upper bounds.");
            }
            else
            {
                var minWeightDecomp = this.minWeightDecompAlg.Run(medians, upperBoundCosts);
                if (minWeightDecomp == null)
                {
                    return null;
                }
                else
                {
                    return this.InternalRun(medians, lowerBoundCosts, minWeightDecomp);
                }
            }
        }

        /// <summary>
        /// Executa o algoritmo sobre a matriz dos limites inferiores dado o custo
        /// associado aos custos superiores.
        /// </summary>
        /// <param name="medians">As medianas.</param>
        /// <param name="lowerBoundCosts">Os custos inferiores.</param>
        /// <param name="upperBoundCost">O custo efectivado dos limites superiores.</param>
        /// <returns>O vector que contém os limites por componente.</returns>
        public ComponentBoundsItemResult[] Run(
            int medians,
            List<List<CostType>> lowerBoundCosts,
            IntMinWeightTdecompResult<CostType> upperBoundCost)
        {
            if (lowerBoundCosts == null)
            {
                throw new ArgumentNullException("lowerBoundCosts");
            }
            else if (upperBoundCost == null)
            {
                throw new ArgumentNullException("upperBoundCost");
            }
            else if (lowerBoundCosts.Count != upperBoundCost.Medians.Count)
            {
                throw new ArgumentException(
                    "The number of components in lower bounds must match the number of medians in upper bound.");
            }
            else
            {
                if (medians < lowerBoundCosts.Count)
                {
                    return null;
                }
                else
                {
                    return this.InternalRun(medians, lowerBoundCosts, upperBoundCost);
                }
            }
        }

        /// <summary>
        /// Executa o algoritmo sobre a matriz dos limites inferiores dado o custo
        /// associado aos custos superiores sem efectuar verificações.
        /// </summary>
        /// <param name="medians">As medianas.</param>
        /// <param name="lowerBoundCosts">Os custos inferiores.</param>
        /// <param name="upperBoundCost">O custo efectivado dos limites superiores.</param>
        /// <returns>O vector que contém os limites por componente.</returns>
        public ComponentBoundsItemResult[] InternalRun(
            int medians,
            List<List<CostType>> lowerBoundCosts,
            IntMinWeightTdecompResult<CostType> upperBoundCost)
        {
            var result = new ComponentBoundsItemResult[lowerBoundCosts.Count];
            var numberOfComponents = lowerBoundCosts.Count;
            var firstLowerBoundEstimate = 1;
            var secondLowerBoundEstimate = medians;
            for (int i = 0; i < numberOfComponents; ++i)
            {
                var resultLowerBound = 0;
                var resultUpperBound = 0;

                // Tratamento dos limites inferiores
                var upBound = upperBoundCost.Medians[i];
                var downBound = firstLowerBoundEstimate;
                var exceptList = this.GetExcept(i, lowerBoundCosts);
                var lowerBounds = lowerBoundCosts[i];
                if (this.OutOfBounds(
                    medians, 
                    upperBoundCost.Cost,
                    lowerBounds[downBound - 1], 
                    downBound, 
                    exceptList))
                {
                    while (upBound != downBound)
                    {
                        // Verifica se os valores dos limites diferem em apenas uma unidade
                        if (downBound + 1 == upBound)
                        {
                            downBound = upBound;
                        }
                        else
                        {
                            var innerBound = (downBound + upBound) / 2;
                            var currentCost = lowerBounds[innerBound - 1];
                            if (this.OutOfBounds(
                                medians, 
                                upperBoundCost.Cost,
                                lowerBounds[innerBound - 1], 
                                innerBound, 
                                exceptList))
                            {
                                downBound = innerBound;
                            }
                            else
                            {
                                upBound = innerBound;
                            }
                        }
                    }

                    resultLowerBound = upBound;
                }
                else
                {
                    resultLowerBound = downBound;
                }

                // Tratamento dos limites superiores
                upBound = Math.Min(secondLowerBoundEstimate - numberOfComponents + i + 1, lowerBounds.Count);
                downBound = upperBoundCost.Medians[i];
                if (this.OutOfBounds(
                    medians, 
                    upperBoundCost.Cost,
                    lowerBounds[upBound - 1], 
                    upBound, 
                    exceptList))
                {
                    while (upBound != downBound)
                    {
                        // Verifica se os valores dos limites diferem em apenas uma unidade
                        if (downBound + 1 == upBound)
                        {
                            upBound = downBound;
                        }
                        else
                        {
                            var innerBound = (downBound + upBound) / 2;
                            var currentCost = lowerBounds[innerBound];
                            if (this.OutOfBounds(
                                medians, 
                                upperBoundCost.Cost,
                                lowerBounds[innerBound - 1], 
                                innerBound, 
                                exceptList))
                            {
                                upBound = innerBound;
                            }
                            else
                            {
                                downBound = innerBound;
                            }
                        }
                    }

                    resultUpperBound = downBound;
                }
                else
                {
                    resultUpperBound = upBound;
                }

                secondLowerBoundEstimate -= resultLowerBound;

                result[i] = new ComponentBoundsItemResult(resultLowerBound, resultUpperBound);
            }

            return result;
        }

        /// <summary>
        /// Verifica se um valor se encontra fora dos limites.
        /// </summary>
        /// <param name="medians">O número de medianas a serem processadas.</param>
        /// <param name="upperCost">O custo do limite superior.</param>
        /// <param name="currentCost">O custo do limite inferior associado à componente actual.</param>
        /// <param name="bound">O número de medianas a verificar.</param>
        /// <param name="exceptList">A lista das restantes componentes.</param>
        /// <returns>Verdadeiro caso se encontre fora dos limites e falso caso contrário.</returns>
        private bool OutOfBounds(
            int medians,
            CostType upperCost,
            CostType currentCost,
            int bound,
            List<List<CostType>> exceptList)
        {
            var minWeightTdecomp = this.minWeightDecompAlg.Run(medians - bound, exceptList);
            var sum = this.ring.Add(minWeightTdecomp.Cost, currentCost);
            return this.comparer.Compare(sum, upperCost) >= 0;
        }

        /// <summary>
        /// Obtém a lista dos custos exceptuando aqueles que correspondem à componente especificaada.
        /// </summary>
        /// <param name="except">A componente a ignorar.</param>
        /// <param name="costs">A matriz dos custos.</param>
        /// <returns>A lista procurada.</returns>
        private List<List<CostType>> GetExcept(int except, List<List<CostType>> costs)
        {
            var result = new List<List<CostType>>();
            for (int i = 0; i < costs.Count; ++i)
            {
                if (i != except)
                {
                    result.Add(costs[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Determina o máximo de dois custos de acordo com o comparador geral.
        /// </summary>
        /// <param name="firstCost">O primeiro custo.</param>
        /// <param name="secondCost">O segundo custo.</param>
        /// <returns>O maior de ambos os custos.</returns>
        private CostType Max(CostType firstCost, CostType secondCost)
        {
            if (this.comparer.Compare(firstCost, secondCost) > 0)
            {
                return firstCost;
            }
            else
            {
                return secondCost;
            }
        }

        /// <summary>
        /// Determina o mínimo de dois custos de acordo com o comparador geral.
        /// </summary>
        /// <param name="firstCost">O primeiro custo.</param>
        /// <param name="secondCost">O segundo custo.</param>
        /// <returns>O menor de ambos os custos.</returns>
        private CostType Min(CostType firstCost, CostType secondCost)
        {
            if (this.comparer.Compare(firstCost, secondCost) < 0)
            {
                return firstCost;
            }
            else
            {
                return secondCost;
            }
        }
    }
}
