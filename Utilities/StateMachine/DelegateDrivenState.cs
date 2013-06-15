using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public delegate IState<InputReader, TSymbVal, TSymbType> NextStateDelegate<InputReader, TSymbVal, TSymbType>(SymbolReader<InputReader, TSymbVal, TSymbType> reader);

    public class DelegateDrivenState<InputReader, TSymbVal, TSymbType> : IState<InputReader, TSymbVal, TSymbType>
    {
        private int stateID;
        private string description = string.Empty;
        private NextStateDelegate<InputReader, TSymbVal, TSymbType> nextStateDelegate;

        public DelegateDrivenState(int stateID, string description, NextStateDelegate<InputReader, TSymbVal, TSymbType> nextStateDelegate)
        {
            if (nextStateDelegate == null)
            {
                throw new ArgumentNullException("nextStateDelegate");
            }

            this.stateID = stateID;
            this.description = description;
            this.nextStateDelegate = nextStateDelegate;
        }

        public IState<InputReader, TSymbVal, TSymbType> NextState(SymbolReader<InputReader, TSymbVal, TSymbType> reader)
        {
            return this.nextStateDelegate.Invoke(reader);
        }

        public override bool Equals(object obj)
        {
            var left = obj as DelegateDrivenState<InputReader, TSymbVal, TSymbType>;
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
