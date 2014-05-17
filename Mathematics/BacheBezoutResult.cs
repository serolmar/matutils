using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Resultado da aplicação do algoritmo de Baché-Bezout a um domínio euclideano.
    /// </summary>
    /// <typeparam name="T">O tipo dos coeficientes.</typeparam>
    public class BacheBezoutResult<T>
    {
        /// <summary>
        /// O primeiro valor.
        /// </summary>
        private T firstItem;

        /// <summary>
        /// O segundo valor.
        /// </summary>
        private T secondItem;

        /// <summary>
        /// O primeiro coeficiente da identidade.
        /// </summary>
        private T firstFactor;

        /// <summary>
        /// O segundo coeficiente da identidade.
        /// </summary>
        private T secondFactor;

        /// <summary>
        /// O máximo divisor comum.
        /// </summary>
        private T greatestCommonDivisor;

        /// <summary>
        /// O cofactor do primeiro valor relativamente ao máximo divisor comum.
        /// </summary>
        private T firstCofactor;

        /// <summary>
        /// O cofactor do segundo valor relativamente ao máximo divisor comum.
        /// </summary>
        private T secondCofactor;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BacheBezoutResult{T}"/>.
        /// </summary>
        /// <param name="firstItem">O primeiro valor.</param>
        /// <param name="secondItem">O segundo valor.</param>
        /// <param name="firstFactor">O coeficiente do primeiro valor na identidade.</param>
        /// <param name="secondFactor">O coeficiente do segundo valor na identidade..</param>
        /// <param name="greatestCommonDivisor">O máximo divisor comum.</param>
        /// <param name="firstCofactor">O cofactor do primeiro valor relativamente ao máximo divisor comum.</param>
        /// <param name="secondCofactor">O cofactor do segundo valor relativamente ao máximo divisor comum.</param>
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
        /// <value>O primeiro número.</value>
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
        /// <value>O segundo valor</value>
        public T SecondItem
        {
            get
            {
                return this.secondItem;
            }
        }

        /// <summary>
        /// Obtém o factor do primeiro coeficiente na identidade.
        /// </summary>
        /// <value>O factor.</value>
        public T FirstFactor
        {
            get
            {
                return this.firstFactor;
            }
        }

        /// <summary>
        /// Obtém o factor do segundo coeficiente na identidade.
        /// </summary>
        /// <value>O factor.</value>
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
        /// <value>O máximo divisor comum.</value>
        public T GreatestCommonDivisor
        {
            get
            {
                return this.greatestCommonDivisor;
            }
        }

        /// <summary>
        /// Obtém o cofactor do primeiro número associado ao máximo divisor comum.
        /// </summary>
        /// <value>O cofactor.</value>
        public T FirstCofactor
        {
            get
            {
                return this.firstCofactor;
            }
        }

        /// <summary>
        /// Obtém o cofactor do segundo número associado ao máximo divisor comum.
        /// </summary>
        /// <value>O cofactor.</value>
        public T SecondCofactor
        {
            get
            {
                return this.secondCofactor;
            }
        }
    }
}
