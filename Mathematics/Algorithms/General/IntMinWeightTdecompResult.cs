namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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

        internal IntMinWeightTdecompResult(List<int> medians, CostType cost)
        {
            this.medians = medians;
            this.cost = cost;
        }

        /// <summary>
        /// Obtém a lista de medianas.
        /// </summary>
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
        public CostType Cost
        {
            get
            {
                return this.cost;
            }
        }
    }
}
