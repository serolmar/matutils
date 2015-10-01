namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um leitor de símbolos sobre um leitor de valores.
    /// </summary>
    /// <typeparam name="InputReader">O leitor de valores.</typeparam>
    /// <typeparam name="TSymbVal">O tipo de objectos que constituem os valores dos símbolos.</typeparam>
    /// <typeparam name="TSymbType">O tipo de objectos que constituem os tipos dos símbolos.</typeparam>
    public abstract class SymbolReader<InputReader, TSymbVal, TSymbType> : ISymbolReader<TSymbVal, TSymbType>
    {
        /// <summary>
        /// O leitor de valores.
        /// </summary>
        protected InputReader inputStream;

        /// <summary>
        /// Um contentor de símbolos lidos.
        /// </summary>
        protected List<ISymbol<TSymbVal, TSymbType>> symbolBuffer = new List<ISymbol<TSymbVal, TSymbType>>();

        /// <summary>
        /// O apontador para o contentor.
        /// </summary>
        protected int bufferPointer = 0;

        /// <summary>
        /// Um valor que indica se o leitor foi iniciado.
        /// </summary>
        protected bool started = false;

        /// <summary>
        /// Instancia um novo objecto do tipos <see cref="SymbolReader{InputReader, TSymbVal, TSymbType}"/>.
        /// </summary>
        /// <param name="inputTextStream">O leitor de valores.</param>
        /// <exception cref="ArgumentNullException">Se o leitor de valores for nulo.</exception>
        public SymbolReader(InputReader inputTextStream)
        {
            if (inputTextStream == null)
            {
                throw new ArgumentNullException("An input reader must be provided.");
            }

            this.inputStream = inputTextStream;
        }

        /// <summary>
        /// Obtém ou atribui um valor que indica se o leitor foi iniciado.
        /// </summary>
        /// <value>Verdadeiro caso o leitor tenha sido iniciado e falso caso contrário.</value>
        public bool Started
        {
            get
            {
                return this.started;
            }
        }

        /// <summary>
        /// Lê o próximo símbolo sem avançar o cursor.
        /// </summary>
        /// <returns>O símbolo.</returns>
        public abstract ISymbol<TSymbVal, TSymbType> Peek();

        /// <summary>
        /// Lê o próximo símbolo avançando o cursor.
        /// </summary>
        /// <returns>O símbolo.</returns>
        public abstract ISymbol<TSymbVal, TSymbType> Get();

        /// <summary>
        /// Retrocede o cursor.
        /// </summary>
        public abstract void UnGet();

        /// <summary>
        /// Indica se o leitor encontrou o final de ficheiro.
        /// </summary>
        /// <returns>Verdadeiro caso o leitor tenha encontrado o final de ficheiro e falso caso contrário.</returns>
        public abstract bool IsAtEOF();

        /// <summary>
        /// Indica se o símbolo constitui um final de ficheiro.
        /// </summary>
        /// <param name="symbol">O símbolo.</param>
        /// <returns>Verdadeiro caso o símbolo seja um final de ficheiro e falso caso contrário.</returns>
        public abstract bool IsAtEOFSymbol(ISymbol<TSymbVal, TSymbType> symbol);
    }

    /// <summary>
    /// Implementa um leitor de símbolos com capacidade de memorização.
    /// </summary>
    /// <typeparam name="InputReader">O leitor de valores.</typeparam>
    /// <typeparam name="TSymbVal">O tipo de objectos que constituem os valores dos símbolos.</typeparam>
    /// <typeparam name="TSymbType">O tipo de objectos que constituem os tipos dos símbolos.</typeparam>
    public abstract class MementoSymbolReader<InputReader, TSymbVal, TSymbType>
        : SymbolReader<InputReader, TSymbVal, TSymbType>, IMementoSymbolReader<TSymbVal, TSymbType>
    {
        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="MementoSymbolReader{InputReader, TSymbVal, TSymbType}"/>.
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
