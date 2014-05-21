using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// Implementa um leitor de símbolos com capacidade de memorização.
    /// </summary>
    /// <typeparam name="InputReader">O leitor de valores.</typeparam>
    /// <typeparam name="TSymbVal">O tipo de objectos que constituem os valores dos símbolos.</typeparam>
    /// <typeparam name="TSymbType">O tipo de objectos que constituem os tipos dos símbolos.</typeparam>
    public abstract class MementoSymbolReader<InputReader, TSymbVal, TSymbType> 
        : SymbolReader<InputReader, TSymbVal, TSymbType>, IMementoOriginator
    {
        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="MementoSymbolReader"/>.
        /// </summary>
        /// <param name="inputTextStream">O leitor de dados.</param>
        public MementoSymbolReader(InputReader inputTextStream)
            : base(inputTextStream)
        {
        }

        /// <summary>
        /// Guarda o estado da instância corrente no memorizador.
        /// </summary>
        /// <returns>O memorizador.</returns>
        public IMemento SaveToMemento()
        {
            return new Memento(this.bufferPointer);
        }

        /// <summary>
        /// Retorna a instância corrente ao estado especificado pelo memorizador.
        /// </summary>
        /// <param name="memento">O memorizador.</param>
        public void RestoreToMemento(IMemento memento)
        {
            if (memento == null)
            {
                throw new ArgumentNullException("Memento can't be null.");
            }

            var innerMemento = memento as Memento;
            if (innerMemento == null)
            {
                throw new InvalidCastException("Invalid memento.");
            }

            this.bufferPointer = innerMemento.State;
        }

        /// <summary>
        /// O memorizador do estado relacionado com o leitor.
        /// </summary>
        protected class Memento : IMemento
        {
            /// <summary>
            /// The memento state.
            /// </summary>
            private int mementoState = 0;

            /// <summary>
            /// Valor que indica se o memorizador foi descartado.
            /// </summary>
            private bool disposed = false;

            /// <summary>
            /// Instantiates a new instance of the <see cref="Memento"/> class.
            /// </summary>
            /// <param name="mementoState">The memento state.</param>
            public Memento(int mementoState)
            {
                this.mementoState = mementoState;
            }

            /// <summary>
            /// Obtém o estado do mermorizador.
            /// </summary>
            /// <value>O estado do mermorizador.</value>
            /// <exception cref="InvalidOperationException">Se o memorizador foi descartado.</exception>
            public int State
            {
                get
                {
                    if (this.disposed)
                    {
                        throw new InvalidOperationException("Memento has already been disposed.");
                    }

                    return this.mementoState;
                }
            }

            /// <summary>
            /// Obtém um valor que indica se o memorizador consome recursos.
            /// </summary>
            /// <value>Verdadeiro se o memorizador consumir muitos recursos e falso caso contrário.</value>
            /// <exception cref="InvalidOperationException">Se o memorizador foi descartado.</exception>
            public bool IsHeavyMemento
            {
                get
                {
                    if (this.disposed)
                    {
                        throw new InvalidOperationException("Memento has already been disposed.");
                    }

                    return false;
                }
            }

            /// <summary>
            /// Descarta o memorizador.
            /// </summary>
            public void Dispose()
            {
                this.disposed = true;
            }
        }
    }
}
