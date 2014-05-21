namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define uma acção e uma precedência para operadores.
    /// </summary>
    /// <typeparam name="TypeDelegate">O delegado responsável pela acção.</typeparam>
    public struct SOperatorPrecedence<TypeDelegate> : IComparer<SOperatorPrecedence<TypeDelegate>>
    {
        /// <summary>
        /// Obtém ou atribui o delegado responsável pela acção.
        /// </summary>
        /// <value>O delegado.</value>
        public TypeDelegate Op { get; set; }

        /// <summary>
        /// Obtém ou atribui uma precedência para o operador.
        /// </summary>
        /// <value>A precedência.</value>
        public int Precedence { get; set; }

        #region IComparer<OperatorPrecedence<TypeDelegate>> Members

        /// <summary>
        /// Permite comparar dois valores permitindo definir uma ordenação.
        /// </summary>
        /// <param name="x">O primeiro valor a ser comparado.</param>
        /// <param name="y">O segundo valor a ser comparado.</param>
        /// <returns>
        /// O valor 1 caso o primeiro valor seja superior ao segundo, 0 caso ambos sejam iguais e 1 caso o primeiro valor seja 
        /// inferior ao segundo.
        /// </returns>
        public int Compare(SOperatorPrecedence<TypeDelegate> x, SOperatorPrecedence<TypeDelegate> y)
        {
            return x.Precedence.CompareTo(y.Precedence);
        }

        #endregion
    }
}
