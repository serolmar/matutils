namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public delegate IState<TSymbVal, TSymbType> NextStateDelegate<TSymbVal, TSymbType>(ISymbolReader<TSymbVal, TSymbType> reader);

    public class DelegateDrivenState<TSymbVal, TSymbType> : IState<TSymbVal, TSymbType>
    {
        private int stateID;
        private string description = string.Empty;
        private NextStateDelegate<TSymbVal, TSymbType> nextStateDelegate;

        public DelegateDrivenState(
            int stateID, 
            string description, 
            NextStateDelegate<TSymbVal, TSymbType> nextStateDelegate)
        {
            if (nextStateDelegate == null)
            {
                throw new ArgumentNullException("nextStateDelegate");
            }

            this.stateID = stateID;
            this.description = description;
            this.nextStateDelegate = nextStateDelegate;
        }

        public IState<TSymbVal, TSymbType> NextState(ISymbolReader<TSymbVal, TSymbType> reader)
        {
            return this.nextStateDelegate.Invoke(reader);
        }

        public override bool Equals(object obj)
        {
            var left = obj as DelegateDrivenState<TSymbVal, TSymbType>;
            if (left == null)
            {
                return false;
            }

            return this.stateID.Equals(left.stateID);
        }

        public override int GetHashCode()
        {
            return this.stateID.GetHashCode();
        }
    }
}
