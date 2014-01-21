namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    public class GreedyAlgSolution<CostType>
    {
        private IntegerSequence chosen;

        private CostType cost;

        public GreedyAlgSolution()
        {
            this.chosen = new IntegerSequence();
        }

        internal GreedyAlgSolution(IntegerSequence chosen)
        {
            if (chosen == null)
            {
                throw new ArgumentNullException("chosen");
            }
            else
            {
                this.chosen = chosen;
            }
        }

        public IntegerSequence Chosen
        {
            get
            {
                return this.chosen;
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

        public virtual GreedyAlgSolution<CostType> Clone()
        {
            var result = new GreedyAlgSolution<CostType>();
            result.chosen = this.chosen.Clone();
            result.cost = this.cost;
            return result;
        }
    }
}
