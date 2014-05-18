namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um número com parte finita e parte infinita.
    /// </summary>
    /// <typeparam name="CostType">O tipo de objectos que constituem o custo.</typeparam>
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

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SimplexMaximumNumberField{CostType}"/>.
        /// </summary>
        public SimplexMaximumNumberField()
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SimplexMaximumNumberField{CostType}"/>.
        /// </summary>
        /// <param name="finitePart">A parte finita.</param>
        /// <param name="bigPart">A parte infinita.</param>
        public SimplexMaximumNumberField(CostType finitePart, CostType bigPart)
        {
            this.finitePart = finitePart;
            this.bigPart = bigPart;
        }

        /// <summary>
        /// Obtém e atribui a parte finita do número.
        /// </summary>
        /// <value>A parte finita.</value>
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
        /// <value>A parte infinita.</value>
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
