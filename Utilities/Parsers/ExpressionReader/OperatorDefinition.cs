namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa uma definição de operador.
    /// </summary>
    /// <typeparam name="SymbType">O tipo de objecto que define o tipo de símbolo.</typeparam>
    class OperatorDefinition<SymbType>
    {
        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="OperatorDefinition{SymbType}"/>.
        /// </summary>
        public OperatorDefinition() { }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="OperatorDefinition{SymbType}"/>.
        /// </summary>
        /// <param name="symbol">O símbolo.</param>
        /// <param name="operatorType">O tipo de operador.( ver <see cref="EOperatorType"/>).</param>
        public OperatorDefinition(SymbType symbol, EOperatorType operatorType)
        {
            this.Symbol = symbol;
            this.OperatorType = operatorType;
        }

        /// <summary>
        /// Obtém ou atribui o tipo de símbolo.
        /// </summary>
        /// <value>O tipo do símbolo.</value>
        public SymbType Symbol { get; set; }

        /// <summary>
        /// Obtém ou atribui o tipo de operador.
        /// </summary>
        /// <value>O tipo de operador.</value>
        public EOperatorType OperatorType { get; set; }
    }
}
