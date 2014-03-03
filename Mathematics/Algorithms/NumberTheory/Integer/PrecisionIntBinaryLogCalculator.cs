namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite calcular as várias potências de 2 negativas envolvidas nas várias aproximações do logaritmo.
    /// </summary>
    public class PrecisionIntBinaryLogCalculator
        : IAlgorithm<PrecisionBinaryLogCalculatorStatus<int>, bool>, IAlgorithm<PrecisionBinaryLogCalculatorStatus<int>, int, bool>
    {
        /// <summary>
        /// Responsável pela determinação da parte inteira do logaritmo.
        /// </summary>
        private IAlgorithm<int, int> integerPartLogAlg;

        /// <summary>
        /// Permite obter a instância de um calculador de logaritmos binários com uma precisão pré-definida.
        /// </summary>
        public PrecisionIntBinaryLogCalculator()
        {
            this.integerPartLogAlg = new FastBinaryLogIntegerPartAlg();
        }

        /// <summary>
        /// Determina a próxima potência de 1/2 que contenha um valor não nulo como aproximação do logaritmo.
        /// </summary>
        /// <param name="valueStatus">O estado da computação para o valor pretendido.</param>
        /// <returns>Verdadeiro caso seja encontrada uma nova precisão e falso caso o valor existente seja exacto.</returns>
        public bool Run(PrecisionBinaryLogCalculatorStatus<int> valueStatus)
        {
            if (valueStatus == null)
            {
                throw new ArgumentNullException("valueStatus");
            }
            else if (valueStatus.Value <= 0)
            {
                throw new ArgumentException("Can only compute the logarithm of strictly positive integers.");
            }
            else
            {
                var state = this.Initialize(valueStatus);
                if (state)
                {
                    this.GetNextPrecisionStatus(valueStatus);
                }

                return state;
            }
        }

        /// <summary>
        /// Permite evoluir o estado dos cálculos para valor até uma determinada precisão.
        /// </summary>
        /// <param name="valueStatus">O estado da computação para o valor pretendido.</param>
        /// <param name="precision">A precisão com a qual se pretende o valor - última potência de 1/2 incluída na aproximação.</param>
        /// <returns>Verdadeiro caso seja possível aumentar a precisão e falso caso contrário.</returns>
        public bool Run(PrecisionBinaryLogCalculatorStatus<int> valueStatus, int precision)
        {
            if (valueStatus == null)
            {
                throw new ArgumentNullException("valueStatus");
            }
            else if (valueStatus.Value <= 0)
            {
                throw new ArgumentException("Can only compute the logarithm of strictly positive integers.");
            }
            else
            {
                var state = this.Initialize(valueStatus);
                if (state)
                {
                    while (valueStatus.CurrentPrecision < precision)
                    {
                        this.GetNextPrecisionStatus(valueStatus);
                    }
                }

                return state;
            }
        }

        /// <summary>
        /// Permite inicializar o estado da computação caso ainda não o tenha sido.
        /// </summary>
        /// <param name="status">O estado da computação.</param>
        /// <returns>Verdadeiro caso o valor do logaritmo possa ser aproximado e falso caso o valor seja exacto.</returns>
        private bool Initialize(PrecisionBinaryLogCalculatorStatus<int> status)
        {
            if (status.NotInitialized)
            {
                if (status.Value == 1)
                {
                    status.IntegerPart = 0;
                    status.NumeratorRemainder = 1;
                    status.DenominatorRemainder = 1;
                }
                else
                {
                    status.IntegerPart = this.integerPartLogAlg.Run(status.Value);
                    status.NumeratorRemainder = status.Value;
                    status.DenominatorRemainder = 1 << status.IntegerPart;
                    status.CurrentPrecision = 0;
                    status.NotInitialized = true;

                    if (status.NumeratorRemainder == status.DenominatorRemainder)
                    {
                        status.Exact = true;
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Obtém o estado associado à próxima precisão.
        /// </summary>
        /// <param name="status">O estado.</param>
        private void GetNextPrecisionStatus(PrecisionBinaryLogCalculatorStatus<int> status)
        {
            throw new NotImplementedException();
        }
    }
}
