using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Representa a decomposição de uma fracção nas suas partes inteira e fraccionária.
    /// </summary>
    /// <typeparam name="T">O tipo de elementos na fracção.</typeparam>
    public class FractionDecompositionResult<T>
    {
        /// <summary>
        /// A parte inteira.
        /// </summary>
        private T integralPart;

        /// <summary>
        /// A parte fraccionária.
        /// </summary>
        private Fraction<T> fractionalPart;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="FractionDecompositionResult{T}"/>.
        /// </summary>
        /// <param name="integralPart">A parte inteira.</param>
        /// <param name="fractionalPart">A parte fraccionária.</param>
        public FractionDecompositionResult(T integralPart, Fraction<T> fractionalPart)
        {
            this.integralPart = integralPart;
            this.fractionalPart = fractionalPart;
        }

        /// <summary>
        /// Obtém a parte inteira.
        /// </summary>
        /// <value>
        /// A parte inteira.
        /// </value>
        public T IntegralPart
        {
            get
            {
                return this.integralPart;
            }
        }

        /// <summary>
        /// Obtém a parte fraccionária.
        /// </summary>
        /// <value>
        /// A parte fraccionária.
        /// </value>
        public Fraction<T> FractionalPart
        {
            get
            {
                return this.fractionalPart;
            }
        }
    }
}
