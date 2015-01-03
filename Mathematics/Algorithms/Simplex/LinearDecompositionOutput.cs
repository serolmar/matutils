namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa o resultado final da aplicação do algoritmo da decomposição linear.
    /// </summary>
    /// <typeparam name="CoeffsType">O tipo dos objectos que constituem os coeficientes.</typeparam>
    public class LinearDecompositionOutput<CoeffsType>
    {
        /// <summary>
        /// A solução.
        /// </summary>
        private CoeffsType[][] problemSolution;

        /// <summary>
        /// A solução que se encontra associada ao problema principal.
        /// </summary>
        private CoeffsType[] masterExclusiveSolution;

        /// <summary>
        /// O custo.
        /// </summary>
        private CoeffsType cost;

        /// <summary>
        /// Valor que indica se a solução é válida.
        /// </summary>
        private bool hasSolution;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SimplexOutput{CoeffsType}"/>.
        /// </summary>
        /// <param name="problemSolution">A solução.</param>
        /// <param name="cost">O custo.</param>
        /// <param name="hasSolution">Um valor que indica se a solução existe.</param>
        public LinearDecompositionOutput(
            CoeffsType[][] problemSolution,
            CoeffsType cost,
            bool hasSolution)
        {
            this.problemSolution = problemSolution;
            this.cost = cost;
            this.hasSolution = hasSolution;
        }

        /// <summary>
        /// Obtém a solução.
        /// </summary>
        /// <value>A solução.</value>
        public CoeffsType[][] ProblemSolution
        {
            get
            {
                return this.problemSolution;
            }
        }

        /// <summary>
        /// Obtém o custo.
        /// </summary>
        /// <value>O custo.</value>
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
        /// <value>Verdadeiro caso exista solução e falso caso contrário.</value>
        public bool HasSolution
        {
            get
            {
                return this.hasSolution;
            }
        }
    }
}
