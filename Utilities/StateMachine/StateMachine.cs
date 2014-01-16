using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class StateMachine<TSymbVal, TSymbType>
    {
        private IState<TSymbVal, TSymbType> start = null;
        private IState<TSymbVal, TSymbType> end = null;
        private IState<TSymbVal, TSymbType> currentState = null;

        public StateMachine(IState<TSymbVal, TSymbType> start, IState<TSymbVal, TSymbType> end)
        {
            this.start = start;
            this.end = end;
            this.Reset();
        }

        public void RunMachine(ISymbolReader<TSymbVal, TSymbType> reader)
        {
            this.RunMachine(reader, null);
        }

        public void RunMachine(
            ISymbolReader<TSymbVal, TSymbType> reader, 
            IEqualityComparer<IState<TSymbVal, TSymbType>> stateComparer)
        {
            // this.currentState = this.start;
            bool isEqual = stateComparer == null ? this.currentState.Equals(this.end) : stateComparer.Equals(this.currentState, this.end);
            while (!isEqual)
            {
                this.currentState = currentState.NextState(reader);
                isEqual = stateComparer == null ? currentState.Equals(this.end) : stateComparer.Equals(this.currentState, this.end);
            }
        }

        public bool NextState(ISymbolReader<TSymbVal, TSymbType> reader)
        {
            return this.NextState(reader, null);
        }

        public bool NextState(
            ISymbolReader<TSymbVal, TSymbType> reader, 
            IEqualityComparer<IState<TSymbVal, TSymbType>> stateComparer)
        {
            bool isEqual = stateComparer == null ? this.currentState.Equals(this.end) : stateComparer.Equals(this.currentState, this.end);
            if (isEqual)
            {
                return false;
            }

            this.currentState = currentState.NextState(reader);
            return true;
        }

        public void Reset()
        {
            this.currentState = this.start;
        }

        public void GotoState(IState<TSymbVal, TSymbType> state)
        {
            this.currentState = state;
        }
    }
}
