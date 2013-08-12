using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
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
