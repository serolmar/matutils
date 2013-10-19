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
    /// <typeparam name="D">O tipo de domínio sobre a qual esta é simplificada.</typeparam>
    public class FractionDecompositionResult<T, D>
        where D : IEuclidenDomain<T>
    {
        private T integralPart;

        private Fraction<T, D> fractionalPart;

        public FractionDecompositionResult(T integralPart, Fraction<T, D> fractionalPart)
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

        public Fraction<T, D> FractionalPart
        {
            get
            {
                return this.fractionalPart;
            }
        }
    }
}
