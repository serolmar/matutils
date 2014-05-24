namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Compõe do delimitador com a operação que representa.
    /// </summary>
    /// <typeparam name="ObjType">O tipo de objecto a ser lido.</typeparam>
    /// <typeparam name="SymbType">O tipo dos objectos que constituem os tipos de símbolos.</typeparam>
    public class ExpressionCompoundDelimiter<ObjType, SymbType>
    {
        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ExpressionCompoundDelimiter{ObjType, SymbType}"/>.
        /// </summary>
        public ExpressionCompoundDelimiter()
        {
            this.DelimiterType = default(SymbType);
        }

        /// <summary>
        /// Obtém ou atribui o tipo do delimitador.
        /// </summary>
        /// <value>
        /// O tipo do delimitador.
        /// </value>
        public SymbType DelimiterType { get; set; }

        /// <summary>
        /// Obtém ou atribui o operador associado ao delimitador.
        /// </summary>
        /// <value>
        /// O operador.
        /// </value>
        public UnaryOperator<ObjType> DelimiterOperator { get; set; }

        /// <summary>
        /// Determina se o objecto é igual à instância corrente.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>
        /// Verdadeiro caso o objecto seja igual e falso caso contrário.
        /// </returns>
        public override bool Equals(object obj)
        {
            var delim = obj as ExpressionCompoundDelimiter<ObjType, SymbType>;
            if (delim == null)
            {
                return base.Equals(obj);
            }
            else
            {
                return this.DelimiterType.Equals(delim.DelimiterType);
            }
        }

        /// <summary>
        /// Retorna o código confuso para a instância actual.
        /// </summary>
        /// <returns>
        /// O código confuso para a instância utilizado em alguns algoritmos.
        /// </returns>
        public override int GetHashCode()
        {
            if (this.DelimiterType == null)
            {
                return base.GetHashCode();
            }
            else
            {
                return this.DelimiterType.GetHashCode();
            }
        }
    }
}
