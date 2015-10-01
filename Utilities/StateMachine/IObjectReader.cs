namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define um leitor de objectos.
    /// </summary>
    /// <typeparam name="Object">O tipos dos objectos a serem lidos.</typeparam>
    public interface IObjectReader<out Object>
    {
        /// <summary>
        /// Obtém o objecto sem avançar o cursor.
        /// </summary>
        /// <returns>O objecto.</returns>
        Object Peek();

        /// <summary>
        /// Obtém o objecto e avança o cursor.
        /// </summary>
        /// <returns>O objecto.</returns>
        Object Get();

        /// <summary>
        /// Retrocede o cursor.
        /// </summary>
        void UnGet();

        /// <summary>
        /// Determina um valor que indica se o final do ficheiro foi atingido.
        /// </summary>
        /// <returns>Verdadeiro caso o final de ficheiro tenha sido atingido e falso caso contrário.</returns>
        bool IsAtEOF();
    }

    /// <summary>
    /// Define um leitor de símbolos.
    /// </summary>
    /// <typeparam name="TSymbVal">O tipo dos objectos que constituem os valores dos símbolos.</typeparam>
    /// <typeparam name="TSymbType">O tipo dos objectos que constituem os tipos dos símbolos.</typeparam>
    public interface ISymbolReader<TSymbVal, TSymbType> : IObjectReader<ISymbol<TSymbVal, TSymbType>>
    {
        /// <summary>
        /// Determina se o símbolo lido corresponde ao final do ficheiro.
        /// </summary>
        /// <param name="symbol">O símbolo.</param>
        /// <returns>Verdadeiro caso o símbolo corresponda ao final do ficheiro e falso caso contrário.</returns>
        bool IsAtEOFSymbol(ISymbol<TSymbVal, TSymbType> symbol);
    }

    /// <summary>
    /// Define um leitor de símbolos com memória.
    /// </summary>
    /// <typeparam name="TSymbVal">O tipo dos objectos que constituem os valores dos símbolos.</typeparam>
    /// <typeparam name="TSymbType">O tipo dos objectos que constituem os tipos dos símbolos.</typeparam>
    public interface IMementoSymbolReader<TSymbVal, TSymbType>
        : ISymbolReader<TSymbVal, TSymbType>, IMementoOriginator
    {
    }
}
