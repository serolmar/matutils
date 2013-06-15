using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public class DelegateStateImplementation<InputReader, SymbolValue, SymbolType> : IState<InputReader, SymbolValue, SymbolType>
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
        protected NextStateDelegate<InputReader, SymbolValue, SymbolType> transitionDelegate;

        public DelegateStateImplementation(
            int stateId,
            string description,
            NextStateDelegate<InputReader, SymbolValue, SymbolType> nextStateDelegate)
        {
            if (nextStateDelegate == null)
            {
                throw new ArgumentNullException("nextStateDelegate");
            }

            this.stateId = stateId;
            this.description = description;
            this.transitionDelegate = nextStateDelegate;
        }

        public IState<InputReader, SymbolValue, SymbolType> NextState(SymbolReader<InputReader, SymbolValue, SymbolType> reader)
        {
            return this.transitionDelegate.Invoke(reader);
        }

        public override bool Equals(object obj)
        {
            var innerObj = obj as DelegateStateImplementation<InputReader, SymbolValue, SymbolType>;
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
