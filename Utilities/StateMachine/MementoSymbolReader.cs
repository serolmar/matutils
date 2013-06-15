using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public abstract class MementoSymbolReader<InputReader, TSymbVal, TSymbType> : SymbolReader<InputReader, TSymbVal, TSymbType>, IMementoOriginator
    {
        public MementoSymbolReader(InputReader inputTextStream)
            : base(inputTextStream)
        {
        }

        /// <summary>
        /// Saves the originator to a memento object.
        /// </summary>
        /// <returns>The memento.</returns>
        public IMemento SaveToMemento()
        {
            return new Memento(this.bufferPointer);
        }

        /// <summary>
        /// Restores the originator to the specified memento.
        /// </summary>
        /// <param name="memento">The restoring memento.</param>
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
        /// The char symbol reader mement.0.
        /// </summary>
        protected class Memento : IMemento
        {
            /// <summary>
            /// The memento state.
            /// </summary>
            private int mementoState = 0;

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
            /// Gets the memento state.
            /// </summary>
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

            public bool IsHeavyMement
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

            public void Dispose()
            {
                this.disposed = true;
            }
        }
    }
}
