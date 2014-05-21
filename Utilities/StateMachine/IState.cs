using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// Define uma enumeração de tipos de estados.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Estado admissível.
        /// </summary>
        ST_OK,

        /// <summary>
        /// Estado de falha.
        /// </summary>
        ST_FAIL
    }

    /// <summary>
    /// Define um estado aceite pela máquina de estados <see cref="StateMachine{TSymbVal, TSymbType}"/>.
    /// </summary>
    /// <typeparam name="TSymbVal">O tipo dos objectos que constituem os valores dos símbolos.</typeparam>
    /// <typeparam name="TSymbType">O tipo dos objectos que constituem os tipos dos símbolos.</typeparam>
    public interface IState<TSymbVal, TSymbType>
    {
        /// <summary>
        /// Obtém o próximo estado com base no símbolo lido de um leitor de valores.
        /// </summary>
        /// <param name="reader">O leitor de valores.</param>
        /// <returns>O próximo estado.</returns>
        IState<TSymbVal, TSymbType> NextState(ISymbolReader<TSymbVal, TSymbType> reader);
    }
}
