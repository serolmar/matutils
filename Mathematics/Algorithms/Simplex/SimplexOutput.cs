﻿namespace Mathematics
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

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SimplexOutput{CoeffsType}"/>.
        /// </summary>
        /// <param name="solution">A solução.</param>
        /// <param name="cost">O custo.</param>
        /// <param name="hasSolution">Um valor que indica se a solução existe.</param>
        public SimplexOutput(
            CoeffsType[] solution, 
            CoeffsType cost,
            bool hasSolution)
        {
            this.solution = solution;
            this.cost = cost;
            this.hasSolution = hasSolution;
        }

        /// <summary>
        /// Obtém a solução.
        /// </summary>
        /// <value>A solução.</value>
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
