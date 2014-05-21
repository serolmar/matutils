namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa uma máquina de estados.
    /// </summary>
    /// <typeparam name="TSymbVal">O tipo de objectos que constituem os valores dos símbolos.</typeparam>
    /// <typeparam name="TSymbType">O tipo de objectos que constituem os tipos dos símbolos.</typeparam>
    public class StateMachine<TSymbVal, TSymbType>
    {
        /// <summary>
        /// O estado inicial.
        /// </summary>
        private IState<TSymbVal, TSymbType> start = null;

        /// <summary>
        /// O estado final.
        /// </summary>
        private IState<TSymbVal, TSymbType> end = null;

        /// <summary>
        /// O estado actual.
        /// </summary>
        private IState<TSymbVal, TSymbType> currentState = null;

        /// <summary>
        /// Intancia um novo objecto do tipo <see cref="StateMachine{TSymbVal, TSymbType}"/>.
        /// </summary>
        /// <param name="start">O estado incial.</param>
        /// <param name="end">O estado final.</param>
        public StateMachine(IState<TSymbVal, TSymbType> start, IState<TSymbVal, TSymbType> end)
        {
            this.start = start;
            this.end = end;
            this.Reset();
        }

        /// <summary>
        /// Executa a máquina de estados.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        public void RunMachine(ISymbolReader<TSymbVal, TSymbType> reader)
        {
            this.RunMachine(reader, null);
        }

        /// <summary>
        /// Executa a máquina de estados.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="stateComparer">O comparador de estados.</param>
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

        /// <summary>
        /// Obtém o próximo estado.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>Verdadeiro caso não tenha sido atingido o estado final e falso caso contrário.</returns>
        public bool NextState(ISymbolReader<TSymbVal, TSymbType> reader)
        {
            return this.NextState(reader, null);
        }

        /// <summary>
        /// Obtém o próximo estado.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>Verdadeiro caso não tenha sido atingido o estado final e falso caso contrário.</returns>
        /// <param name="stateComparer">O comparador de estados.</param>
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

        /// <summary>
        /// Inicia a máquina de estados.
        /// </summary>
        public void Reset()
        {
            this.currentState = this.start;
        }

        /// <summary>
        /// Coloca a máquina no estado especificado.
        /// </summary>
        /// <param name="state">O estado.</param>
        public void GotoState(IState<TSymbVal, TSymbType> state)
        {
            this.currentState = state;
        }
    }
}
