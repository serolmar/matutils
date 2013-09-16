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
        : IAlgorithm<int, SparseDictionaryMatrix<CostType>, int[]>
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
        public int[] Run(
            int n,  
            SparseDictionaryMatrix<CostType> matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }
            else if (n < matrix.GetLength(0))
            {
                // Para existir solução, o número a ser decomposto terá de ser superior ao número de elementos
                // na decomposição.
                return null;
            }
            else
            {
                var p = matrix.GetLength(0);
                var columns = matrix.GetLength(1);
                var currentComponentAddition = 0;
                var linesEnumerator = matrix.GetLines().GetEnumerator();
                var state = linesEnumerator.MoveNext();
                var verticesInfo = new VertexCostPair[n];
                while (state)
                {
                    var currentLine = linesEnumerator.Current.Value;
                    var columnsEnumerator = currentLine.GetColumns().GetEnumerator();
                    var innerState = columnsEnumerator.MoveNext();
                    while (innerState)
                    {
                        if (columnsEnumerator.Current.Key < n - p + 1 - currentComponentAddition)
                        {

                        }
                        else
                        {
                            innerState = false;
                        }
                    }

                    ++currentComponentAddition;
                }

                if (verticesInfo[n - 1] == null)
                {
                    return null;
                }
                else
                {
                    var result = new int[p];
                }
            }

            throw new NotImplementedException();
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
        }
    }
}
