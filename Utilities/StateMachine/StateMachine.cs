using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public class StateMachine<InputReader, TSymbVal, TSymbType>
    {
        private IState<InputReader, TSymbVal, TSymbType> start = null;
        private IState<InputReader, TSymbVal, TSymbType> end = null;
        private IState<InputReader, TSymbVal, TSymbType> currentState = null;

        public StateMachine(IState<InputReader, TSymbVal, TSymbType> start, IState<InputReader, TSymbVal, TSymbType> end)
        {
            this.start = start;
            this.end = end;
            this.Reset();
        }

        public void RunMachine(SymbolReader<InputReader, TSymbVal, TSymbType> reader)
        {
            this.RunMachine(reader, null);
        }

        public void RunMachine(SymbolReader<InputReader, TSymbVal, TSymbType> reader, IEqualityComparer<IState<InputReader, TSymbVal, TSymbType>> stateComparer)
        {
            // this.currentState = this.start;
            bool isEqual = stateComparer == null ? this.currentState.Equals(this.end) : stateComparer.Equals(this.currentState, this.end);
            while (!isEqual)
            {
                this.currentState = currentState.NextState(reader);
                isEqual = stateComparer == null ? currentState.Equals(this.end) : stateComparer.Equals(this.currentState, this.end);
            }
        }

        public bool NextState(SymbolReader<InputReader, TSymbVal, TSymbType> reader)
        {
            return this.NextState(reader, null);
        }

        public bool NextState(SymbolReader<InputReader, TSymbVal, TSymbType> reader, IEqualityComparer<IState<InputReader, TSymbVal, TSymbType>> stateComparer)
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

        public void GotoState(IState<InputReader, TSymbVal, TSymbType> state)
        {
            this.currentState = state;
        }
    }
}
