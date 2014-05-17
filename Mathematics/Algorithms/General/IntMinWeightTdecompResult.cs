namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa o resultado de uma decomposição.
    /// </summary>
    /// <typeparam name="CostType">O tipo de objectos que constituem os custos.</typeparam>
    public class IntMinWeightTdecompResult<CostType>
    {
        /// <summary>
        /// As medianas escolhidas.
        /// </summary>
        private List<int> medians;

        /// <summary>
        /// O custo.
        /// </summary>
        private CostType cost;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="IntMinWeightTdecompResult{CostType}"/>.
        /// </summary>
        /// <param name="medians">As medianas.</param>
        /// <param name="cost">O custo.</param>
        internal IntMinWeightTdecompResult(List<int> medians, CostType cost)
        {
            this.medians = medians;
            this.cost = cost;
        }

        /// <summary>
        /// Obtém a lista de medianas.
        /// </summary>
        /// <value>A lista de medianas.</value>
        public IList<int> Medians
        {
            get
            {
                return this.medians.AsReadOnly();
            }
        }

        /// <summary>
        /// Obtém o custo associado.
        /// </summary>
        /// <value>O custo.</value>
        public CostType Cost
        {
            get
            {
                return this.cost;
            }
        }
    }
}
