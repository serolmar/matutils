namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Algoritmo que visa a construção de um tuplo ordenado de p números cuja soma
    /// valha n de modo a minimizar o custo. Este custo é calculado de acordo com a matriz
    /// que atribui, à escolha de um determinado valor, o seu custo na respectiva posição.
    /// </summary>
    /// <typeparam name="CostType">O tipo de dados associado ao custo.</typeparam>
    public class IntegerMinWeightTdecomposition<CostType>
        : IAlgorithm<int, List<List<CostType>>, List<int>>
    {
        /// <summary>
        /// O anel responsável pelas operações.
        /// </summary>
        IRing<CostType> ring;

        /// <summary>
        /// O comparador de custos.
        /// </summary>
        IComparer<CostType> comparer;

        public IntegerMinWeightTdecomposition(IComparer<CostType> comparer, IRing<CostType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.comparer = comparer;
                this.ring = ring;
            }
        }

        /// <summary>
        /// Constrói o tuplo ordenado que corresponde à t-decomposição de n de acordo com a matriz
        /// de custos especificada.
        /// </summary>
        /// <param name="n">O número a ser decomposto.</param>
        /// <param name="matrix">a matriz dos custos.</param>
        /// <returns>A decomposição caso exista e nulo caso contrário.</returns>
        public List<int> Run(
            int n,
            List<List<CostType>> matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }
            else if (n < matrix.Count)
            {
                // Para existir solução, o número a ser decomposto terá de ser superior ao número de elementos
                // na decomposição.
                return null;
            }
            else
            {
                var countVertices = this.CountVertices(matrix);
                if (countVertices < 0 || countVertices < n)
                {
                    // Não é possível encontrar solução quando n é superior ao número de elementos na matriz
                    // nem quando existe alguma linha vazia.
                    return null;
                }
                else
                {
                    var p = matrix.Count;
                    var verticesInfoTable = new List<List<VertexCostPair>>();
                    if (0 < p)
                    {
                        var verticesInfoLine = new List<VertexCostPair>();
                        var precomputedNumber = n - p + 1;
                        var currentCosts = matrix[0];
                        for (int j = 0; j < currentCosts.Count && j < precomputedNumber; ++j)
                        {
                            var currentCost = currentCosts[j];
                            verticesInfoLine.Add(new VertexCostPair()
                            {
                                Cost = currentCosts[j],
                                Vertex = 0
                            });
                        }

                        verticesInfoTable.Add(verticesInfoLine);

                        for (int i = 1; i < p - 1; ++i)
                        {
                            var previousInfoLine = verticesInfoLine;
                            verticesInfoLine = new List<VertexCostPair>();
                            currentCosts = matrix[i];

                            var count = 0;
                            var previousLineValue = previousInfoLine[0];
                            for (int k = 0; k < currentCosts.Count && count < precomputedNumber; ++k)
                            {
                                verticesInfoLine.Add(new VertexCostPair()
                                {
                                    Cost = this.ring.Add(previousLineValue.Cost, currentCosts[k]),
                                    Vertex = 0
                                });

                                ++count;
                            }

                            var lastIndex = count - 1;
                            var lastCost = currentCosts[lastIndex];
                            for (int j = 1; j < previousInfoLine.Count && count < precomputedNumber; ++j)
                            {
                                verticesInfoLine.Add(new VertexCostPair()
                                {
                                    Cost = this.ring.Add(lastCost, previousInfoLine[j].Cost),
                                    Vertex = j
                                });

                                ++count;
                            }

                            for (int j = 1; j < previousInfoLine.Count && j < precomputedNumber; ++j)
                            {
                                var maxValue = precomputedNumber - j;
                                for (int k = 0; k < currentCosts.Count - 1 && k < maxValue; ++k)
                                {
                                    var currentPreviousValue = previousInfoLine[j].Cost;
                                    var tempCost = this.ring.Add(currentPreviousValue, currentCosts[k]);
                                    var currentVertexLine = verticesInfoLine[k + j];
                                    if (this.comparer.Compare(tempCost, currentVertexLine.Cost) < 0)
                                    {
                                        currentVertexLine.Cost = tempCost;
                                        currentVertexLine.Vertex = j;
                                    }
                                }
                            }

                            verticesInfoTable.Add(verticesInfoLine);
                        }

                        // p iguala o número total de componentes do custo
                        var vertex = verticesInfoLine.Count;
                        var diff = precomputedNumber - vertex;
                        currentCosts = matrix[p - 1];
                        --vertex;
                        var lastValue = new VertexCostPair()
                        {
                            Cost = this.ring.Add(verticesInfoLine[vertex].Cost, currentCosts[diff]),
                            Vertex = vertex
                        };

                        --vertex;
                        for (int i = diff + 1; i < currentCosts.Count; ++i)
                        {
                            var currentValue = this.ring.Add(verticesInfoLine[vertex].Cost, currentCosts[i]);
                            if (this.comparer.Compare(currentValue, lastValue.Cost) < 0)
                            {
                                lastValue.Cost = currentValue;
                                lastValue.Vertex = vertex;
                            }

                            --vertex;
                        }

                        return this.GetSolution(n, p, lastValue, verticesInfoTable);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the solution from computed values.
        /// </summary>
        /// <param name="n">O número a ser decomposto.</param>
        /// <param name="p">The number of components.</param>
        /// <param name="lastValue">The last computed value.</param>
        /// <param name="table">The table.</param>
        /// <returns>The solution.</returns>
        private List<int> GetSolution(
            int n,
            int p,
            VertexCostPair lastValue, 
            List<List<VertexCostPair>> table)
        {
            var temporarySolution = new int[p + 1];
            var i = p;
            temporarySolution[i] = n;
            --i;
            temporarySolution[i] = lastValue.Vertex + i;
            --i;
            for (; i > 0; --i)
            {
                lastValue = table[i][lastValue.Vertex];
                temporarySolution[i] = lastValue.Vertex + i;
            }

            var result = new List<int>();
            for (i = 0; i < p; ++i)
            {
                result.Add( temporarySolution[i + 1] - temporarySolution[i]);
            }

            return result;
        }

        /// <summary>
        /// Determina o número de elementos na matriz.
        /// </summary>
        /// <param name="matrix">A matriz.</param>
        /// <returns>O número de elementos na matriz e -1 caso exista alguma linha vazia.</returns>
        private int CountVertices(List<List<CostType>> matrix)
        {
            var result = 0;
            foreach (var line in matrix)
            {
                if (line.Count == 0)
                {
                    return -1;
                }
                else
                {
                    result += line.Count;
                }
            }

            return result;
        }

        private class VertexCostPair
        {
            private int vertex;

            private CostType cost;

            public int Vertex
            {
                get
                {
                    return this.vertex;
                }
                set
                {
                    this.vertex = value;
                }
            }

            public CostType Cost
            {
                get
                {
                    return this.cost;
                }
                set
                {
                    this.cost = value;
                }
            }

            public override string ToString()
            {
                return string.Format("{0}:{1}", this.vertex, this.cost);
            }
        }
    }
}
