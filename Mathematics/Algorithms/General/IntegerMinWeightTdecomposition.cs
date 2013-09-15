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
        : IAlgorithm<int, int, SparseDictionaryMatrix<CostType>, int[]>
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
        /// <param name="p">O número de elementos na decomposição.</param>
        /// <param name="matrix">a matriz dos custos.</param>
        /// <returns>A decomposição caso exista e nulo caso contrário.</returns>
        public int[] Run(
            int n, 
            int p, 
            SparseDictionaryMatrix<CostType> matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }
            else if (matrix.GetLength(1) != p)
            {
                throw new ArgumentException("The number of columns in cost matrix must match the value of p.");
            }
            else if (n < p)
            {
                // Para existir solução, o número a ser decomposto terá de ser superior ao número de elementos
                // na decomposição.
                return null;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
