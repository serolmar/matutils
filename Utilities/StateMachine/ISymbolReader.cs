namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
}
