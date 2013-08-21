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

        private T firstCofactor;

        private T secondCofactor;

        internal BacheBezoutResult(
            T firstItem,
            T secondItem,
            T firstFactor,
            T secondFactor,
            T greatestCommonDivisor,
            T firstCofactor,
            T secondCofactor)
        {
            this.firstItem = firstItem;
            this.secondItem = secondItem;
            this.firstFactor = firstFactor;
            this.secondFactor = secondFactor;
            this.greatestCommonDivisor = greatestCommonDivisor;
            this.firstCofactor = firstCofactor;
            this.secondCofactor = secondCofactor;
        }

        /// <summary>
        /// Obtém o primeiro número sobre o qual se processa a identidade.
        /// </summary>
        public T FirstItem
        {
            get
            {
                return this.firstItem;
            }
        }

        /// <summary>
        /// Obtém o segundo número sobre o qual se processa a identidade.
        /// </summary>
        public T SecondItem
        {
            get
            {
                return this.secondItem;
            }
        }

        /// <summary>
        /// Obtém o primeiro factor da identidade.
        /// </summary>
        public T FirstFactor
        {
            get
            {
                return this.firstFactor;
            }
        }

        /// <summary>
        /// Obtém o segundo factor da identidade.
        /// </summary>
        public T SecondFactor
        {
            get
            {
                return this.secondFactor;
            }
        }

        /// <summary>
        /// Obtém o máximo divisor comum.
        /// </summary>
        public T GreatestCommonDivisor
        {
            get
            {
                return this.greatestCommonDivisor;
            }
        }

        /// <summary>
        /// Obtém o co-factor do primeiro número associado ao máximo divisor comum.
        /// </summary>
        public T FirstCofactor
        {
            get
            {
                return this.firstCofactor;
            }
        }

        /// <summary>
        /// Obtém o co-factor do segundo número associado ao máximo divisor comum.
        /// </summary>
        public T SecondCofactor
        {
            get
            {
                return this.secondCofactor;
            }
        }
    }
}
