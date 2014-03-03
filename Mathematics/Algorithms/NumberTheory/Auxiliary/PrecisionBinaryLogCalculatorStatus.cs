namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Resultado de saída correspondente ao cálculo do logaritmo binário de um número incluindo potências inteiras de 1/2
    /// até uma determinada precisão.
    /// </summary>
    public class PrecisionBinaryLogCalculatorStatus<T>
    {
        /// <summary>
        /// Valor do qual se pretende obter o logaritmo.
        /// </summary>
        private T value;

        /// <summary>
        /// A parte inteira do logaritmo.
        /// </summary>
        private T integerPart;

        /// <summary>
        /// O numerador do resto multiplicativo do logaritmo.
        /// </summary>
        private T numeratorRemainder;

        /// <summary>
        /// O denominador do resto multiplicativo do logaritmo.
        /// </summary>
        private T denominatorRemainder;

        /// <summary>
        /// A potência de 1/2 até à qual os cálculos foram efectuados.
        /// </summary>
        private T currentPrecision;

        /// <summary>
        /// As potências inteiras de 1/2 que contribuem para a precisão pretendida.
        /// </summary>
        private List<T> computedPrecisions;

        /// <summary>
        /// Valor que indica se o resultado foi inicializado ou não.
        /// </summary>
        private bool notInitialized;

        /// <summary>
        /// Indica se o valor encontrado do logaritmo é exacto.
        /// </summary>
        private bool exact = false;

        /// <summary>
        /// Permite instanciar os parâmetros de entrada para o cálculo do logaritmo com uma determinada precisão.
        /// </summary>
        /// <param name="value">O valor do qual se pretende obter o logaritmo.</param>
        public PrecisionBinaryLogCalculatorStatus(T value)
        {
            this.value = value;
            this.notInitialized = true;
            this.computedPrecisions = new List<T>();
        }

        /// <summary>
        /// Obtém o valor do qual se pretende obter o logaritmo.
        /// </summary>
        public T Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Obtém a parte inteira do logaritmo do valor especificado.
        /// </summary>
        public T IntegerPart
        {
            get
            {
                if (this.notInitialized)
                {
                    throw new MathematicsException("The precision status was not initialized.");
                }
                else
                {
                    return this.integerPart;
                }
            }
            internal set
            {
                this.integerPart = value;
            }
        }

        /// <summary>
        /// Obtém os valores que contriuem para a precisão calculada (números que correspondem às potências inteiras de 1/2).
        /// </summary>
        public IList<T> ComputedPrecisions
        {
            get
            {
                if (this.notInitialized)
                {
                    throw new MathematicsException("The precision status was not initialized.");
                }
                else
                {
                    return this.computedPrecisions.AsReadOnly();
                }
            }
        }

        internal T NumeratorRemainder
        {
            get
            {
                return this.numeratorRemainder;
            }
            set
            {
                this.numeratorRemainder = value;
            }
        }

        internal T DenominatorRemainder
        {
            get
            {
                return this.denominatorRemainder;
            }
            set
            {
                this.denominatorRemainder = value;
            }
        }

        internal T CurrentPrecision
        {
            get
            {
                return this.currentPrecision;
            }
            set
            {
                this.currentPrecision = value;
            }
        }

        internal List<T> InternalComputedPrecisions
        {
            get
            {
                return this.computedPrecisions;
            }
        }

        internal bool NotInitialized
        {
            get
            {
                return this.notInitialized;
            }
            set
            {
                this.notInitialized = value;
            }
        }

        internal bool Exact
        {
            get
            {
                return this.exact;
            }
            set
            {
                this.exact = value;
            }
        }
    }
}
