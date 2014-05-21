namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define um leitor de símbolos num objecto.
    /// </summary>
    /// <typeparam name="T">O tipo de objecto a ser lido.</typeparam>
    /// <typeparam name="SymbValue">O tipo de objecto que constitui o valor dos símbolos lidos.</typeparam>
    /// <typeparam name="SymbType">O tipo de objecto que constitui o tipo dos símbolos lidos.</typeparam>
    public interface IParse<T, SymbValue, SymbType>
    {
        /// <summary>
        /// Tenta fazer a leitura.
        /// </summary>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="value">O valor que contém a leitura.</param>
        /// <returns>Verdadeiro se a leitura for bem-sucedida e falso caso contrário.</returns>
        bool TryParse(ISymbol<SymbValue, SymbType>[] symbolListToParse, out T value);
    }
}
