namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DelegateStateImplementation<SymbolValue, SymbolType> : IState<SymbolValue, SymbolType>
    {
        /// <summary>
        /// The state id.
        /// </summary>
        protected int stateId;

        /// <summary>
        /// The state description.
        /// </summary>
        protected string description;

        /// <summary>
        /// The transition delegate.
        /// </summary>
        protected NextStateDelegate<SymbolValue, SymbolType> transitionDelegate;

        public DelegateStateImplementation(
            int stateId,
            string description,
            NextStateDelegate<SymbolValue, SymbolType> nextStateDelegate)
        {
            if (nextStateDelegate == null)
            {
                throw new ArgumentNullException("nextStateDelegate");
            }

            this.stateId = stateId;
            this.description = description;
            this.transitionDelegate = nextStateDelegate;
        }

        public IState<SymbolValue, SymbolType> NextState(ISymbolReader<SymbolValue, SymbolType> reader)
        {
            return this.transitionDelegate.Invoke(reader);
        }

        public override bool Equals(object obj)
        {
            var innerObj = obj as DelegateStateImplementation<SymbolValue, SymbolType>;
            if (innerObj == null)
            {
                return false;
            }

            return this.stateId == innerObj.stateId;
        }

        public override int GetHashCode()
        {
            return this.stateId.GetHashCode();
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.description))
            {
                return "No description.";
            }
            else
            {
                return this.description;
            }
        }
    }
}
