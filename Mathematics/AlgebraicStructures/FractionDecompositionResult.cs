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
        private T integralPart;

        private Fraction<T> fractionalPart;

        public FractionDecompositionResult(T integralPart, Fraction<T> fractionalPart)
        {
            this.integralPart = integralPart;
            this.fractionalPart = fractionalPart;
        }

        public T IntegralPart
        {
            get
            {
                return this.integralPart;
            }
        }

        public Fraction<T> FractionalPart
        {
            get
            {
                return this.fractionalPart;
            }
        }
    }
}
