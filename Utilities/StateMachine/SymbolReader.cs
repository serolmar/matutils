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
}
