using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Resultado da aplicação do algoritmo de Baché-Bezout a um domínio euclideano.
    /// </summary>
    public class BacheBezoutResult<T>
    {
        private T firstItem;

        private T secondItem;

        private T firstFactor;

        private T secondFactor;

        private T greatestCommonDivisor;

        private T leastCommonMultiple;

        public BacheBezoutResult(
            T firstItem,
            T secondItem,
            T firstFactor,
            T secondFactor,
            T greatestCommonDivisor,
            T leastCommonMultiple)
        {
            this.firstItem = firstItem;
            this.secondItem = secondItem;
            this.firstFactor = firstFactor;
            this.secondFactor = secondFactor;
            this.greatestCommonDivisor = greatestCommonDivisor;
            this.leastCommonMultiple = leastCommonMultiple;
        }

        public T FirstItem
        {
            get
            {
                return this.firstItem;
            }
        }

        public T SecondItem
        {
            get
            {
                return this.secondItem;
            }
        }

        public T FirstFactor
        {
            get
            {
                return this.firstFactor;
            }
        }

        public T SecondFactor
        {
            get
            {
                return this.secondFactor;
            }
        }

        public T GreatestCommonDivisor
        {
            get
            {
                return this.greatestCommonDivisor;
            }
        }

        public T LeastCommonMultiple
        {
            get
            {
                return this.leastCommonMultiple;
            }
        }
    }
}
