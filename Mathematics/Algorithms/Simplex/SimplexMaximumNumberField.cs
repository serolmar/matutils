namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SimplexMaximumNumberField<CostType>
    {
        /// <summary>
        /// A parte finita do número.
        /// </summary>
        private CostType finitePart;

        /// <summary>
        /// A parte enorme do número.
        /// </summary>
        private CostType bigPart;

        public SimplexMaximumNumberField()
        {
        }

        public SimplexMaximumNumberField(CostType finitePart, CostType bigPart)
        {
            this.finitePart = finitePart;
            this.bigPart = bigPart;
        }

        /// <summary>
        /// Obtém e atribui a parte finita do número.
        /// </summary>
        public CostType FinitePart
        {
            get
            {
                return this.finitePart;
            }
            set
            {
                this.finitePart = value;
            }
        }

        /// <summary>
        /// Obtém e atribui a parte infinita do número.
        /// </summary>
        public CostType BigPart
        {
            get
            {
                return this.bigPart;
            }
            set
            {
                this.bigPart = value;
            }
        }
    }
}
