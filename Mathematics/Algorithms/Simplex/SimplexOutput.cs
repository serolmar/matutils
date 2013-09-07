namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa o resultado da aplicação do algoritmo do simplex.
    /// </summary>
    /// <typeparam name="CoeffsType">O tipo dos coeficientes.</typeparam>
    public class SimplexOutput<CoeffsType>
    {
        /// <summary>
        /// A solução.
        /// </summary>
        private CoeffsType[] solution;

        /// <summary>
        /// O custo.
        /// </summary>
        private CoeffsType cost;

        /// <summary>
        /// Valor que indica se a solução é válida.
        /// </summary>
        private bool hasSolution;

        public SimplexOutput(
            CoeffsType[] solution, 
            CoeffsType cost,
            bool hasSolution)
        {
            this.solution = solution;
            this.cost = cost;
        }

        /// <summary>
        /// Obtém a solução.
        /// </summary>
        public CoeffsType[] Solution
        {
            get
            {
                return this.solution;
            }
        }

        /// <summary>
        /// Obtém o custo.
        /// </summary>
        public CoeffsType Cost
        {
            get
            {
                return this.cost;
            }
        }

        /// <summary>
        /// Obtém o valor que indica se a solução é válida.
        /// </summary>
        public bool HasSolution
        {
            get
            {
                return this.hasSolution;
            }
        }
    }
}
