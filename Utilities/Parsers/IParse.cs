namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Enumeração dos níveis de erro possíveis numa leitura.
    /// </summary>
    public enum EParseErrorLevel
    {
        /// <summary>
        /// Erro de leitura.
        /// </summary>
        ERROR = 0
    }

    /// <summary>
    /// Define um leitor de símbolos num objecto.
    /// </summary>
    /// <typeparam name="T">O tipo de objecto a ser lido.</typeparam>
    /// <typeparam name="SymbValue">O tipo de objecto que constitui o valor dos símbolos lidos.</typeparam>
    /// <typeparam name="SymbType">O tipo de objecto que constitui o tipo dos símbolos lidos.</typeparam>
    public interface IParse<out T, SymbValue, SymbType>
    {
        /// <summary>
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        T Parse(
            ISymbol<SymbValue, SymbType>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs);
    }
}
