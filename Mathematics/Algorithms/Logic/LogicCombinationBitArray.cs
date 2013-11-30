namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Representa uma combinação de valores lógicos, incluindo os que não são considerados.
    /// </summary>
    public class LogicCombinationBitArray : IEnumerable<EBooleanMinimalFormOutStatus>
    {
        /// <summary>
        /// Mantém o valor associado ao número de bits contidos num "byte".
        /// </summary>
        public const int ByteSize = 8;

        private int semiIntegerSize;

        private uint[] masks;

        private int length;

        private uint[] values;

        public LogicCombinationBitArray(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.length = length;
                var integerSize = sizeof(int) << 3;
                this.semiIntegerSize = integerSize >> 1;
                var requiredLength = length << 1;
                var requiredSpace = requiredLength / integerSize;
                if (this.length % integerSize != 0)
                {
                    ++requiredSpace;
                }

                this.values = new uint[requiredSpace];
                this.InitializeMasks(integerSize);
            }
        }

        public LogicCombinationBitArray(int length, EBooleanMinimalFormOutStatus defaultValue)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.length = length;
                var integerSize = sizeof(int) << 3;
                this.semiIntegerSize = integerSize >> 1;
                var requiredLength = length << 1;
                var requiredSpace = requiredLength / integerSize;
                if (this.length % integerSize != 0)
                {
                    ++requiredSpace;
                }

                this.values = new uint[requiredSpace];
                this.InitializeMasks(integerSize);
                if (defaultValue != EBooleanMinimalFormOutStatus.ERROR)
                {
                    this.SetAll(defaultValue);
                }
            }
        }

        /// <summary>
        /// Obtém o valor lógico na entrada especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor lógico.</returns>
        public EBooleanMinimalFormOutStatus this[int index]
        {
            get
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    var elementIndex = index / this.semiIntegerSize;
                    var innerIndex = index % this.semiIntegerSize;
                    var element = this.values[elementIndex];
                    var mask = this.masks[innerIndex];
                    return (EBooleanMinimalFormOutStatus)((element & mask) >> (innerIndex << 1));
                }
            }
            set
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    var elementIndex = index / this.semiIntegerSize;
                    var innerIndex = index % this.semiIntegerSize;
                    var element = this.values[elementIndex];
                    var mask = this.masks[innerIndex];
                    this.values[elementIndex] = (~mask & element) | ((uint)value << (innerIndex << 1));
                }
            }
        }

        /// <summary>
        /// Atribui o valor especificado a todas as entradas da combinação.
        /// </summary>
        /// <param name="status">O valor a ser atribuído.</param>
        public void SetAll(EBooleanMinimalFormOutStatus status)
        {
            if (status == EBooleanMinimalFormOutStatus.DONT_CARE)
            {
                for (int i = 0; i < this.values.Length; ++i)
                {
                    this.values[i] = int.MaxValue;
                }
            }
            else if (status == EBooleanMinimalFormOutStatus.ERROR)
            {
                for (int i = 0; i < this.values.Length; ++i)
                {
                    this.values[i] = 0;
                }
            }
            else if (status == EBooleanMinimalFormOutStatus.OFF)
            {
                for (int i = 0; i < this.values.Length; ++i)
                {
                    this.values[i] = 0xAAAAAAAA;
                }
            }
            else if (status == EBooleanMinimalFormOutStatus.ON)
            {
                for (int i = 0; i < this.values.Length; ++i)
                {
                    this.values[i] = 0x55555555;
                }
            }
            else
            {
                throw new MathematicsException(string.Format(
                    "Status value {0} isn't supported.",
                    status));
            }
        }

        /// <summary>
        /// Tenta obter uma redução lógica como resultado da combinação com outra expressão.
        /// </summary>
        /// <param name="combination">A combinação lógica a cobrir.</param>
        /// <param name="cover">A função lógica simplificada.</param>
        /// <returns>Verdadeiro caso a simplificação seja possível e falso caso contrário.</returns>
        public bool TryToGetReduced(LogicCombinationBitArray combination, out LogicCombinationBitArray cover)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Conta o número de elementos com o valor especificado.
        /// </summary>
        /// <param name="status">O valor a ser contado.</param>
        /// <returns>O número de elementos com o valor esepcificado.</returns>
        public int CountElementsWithValue(EBooleanMinimalFormOutStatus status)
        {
            if (status == EBooleanMinimalFormOutStatus.DONT_CARE)
            {
                var count = 0;
                var last = this.values.Length - 1;
                for (int i = 0; i < this.values.Length - 1; ++i)
                {
                    count += this.CountDontCares(this.values[i]);
                    
                }

                var lastCount = (this.length % 16);
                var lastValue = this.values[last] & ((1U << (lastCount << 1)) - 1);
                count += this.CountDontCares(lastValue);
                return count;
            }
            else if (status == EBooleanMinimalFormOutStatus.ERROR)
            {
                var count = 0;
                var last = this.values.Length - 1;
                for (int i = 0; i < this.values.Length - 1; ++i)
                {
                    count += this.CountDontCares(~this.values[i]);
                }

                var lastCount = (this.length % 16);
                var lastValue = ~this.values[last] & ((1U << (lastCount << 1)) - 1);
                count += this.CountDontCares(lastValue);
                return count;
            }
            else if (status == EBooleanMinimalFormOutStatus.OFF)
            {
                var count = 0;
                var last = this.values.Length - 1;
                for (int i = 0; i < this.values.Length - 1; ++i)
                {
                    var currentValue = (this.values[i] >> 1) & 0x55555555;
                    currentValue ^= this.values[i];
                    count += this.CountDontCares(currentValue);
                }

                var lastCount = (this.length % 16);
                var lastValue = this.values[last] & ((1U << (lastCount << 1)) - 1);
                var temp = (lastValue >> 1) & 0x55555555;
                lastValue ^= temp;
                count += this.CountDontCares(lastValue);
                return count;
            }
            else if (status == EBooleanMinimalFormOutStatus.ON)
            {
                var count = 0;
                var last = this.values.Length - 1;
                for (int i = 0; i < this.values.Length - 1; ++i)
                {
                    var currentValue = (this.values[i] << 1) & 0xAAAAAAAA;
                    currentValue ^= this.values[i];
                    count += this.CountDontCares(currentValue);
                }

                var lastCount = (this.length % 16);
                var lastValue = this.values[last] & ((1U << (lastCount << 1)) - 1);
                var temp = (lastValue << 1) & 0xAAAAAAAA;
                lastValue ^= temp;
                count += this.CountDontCares(lastValue);
                return count;
            }
            else
            {
                throw new MathematicsException(string.Format(
                    "Status value {0} isn't supported.",
                    status));
            }
        }

        public IEnumerator<EBooleanMinimalFormOutStatus> GetEnumerator()
        {
            var currentGeneralPointer = 0;
            var currentValuesPointer = 0;
            while (currentGeneralPointer < this.length)
            {
                var currentInnerIndex = 0;
                while (currentInnerIndex < this.semiIntegerSize &&
                    currentGeneralPointer < this.length)
                {
                    var element = this.values[currentValuesPointer];
                    var mask = this.masks[currentInnerIndex];
                    yield return (EBooleanMinimalFormOutStatus)((element & mask) >> (currentInnerIndex << 1));
                }

                ++currentValuesPointer;
            }
        }

        /// <summary>
        /// Conta o número de indiferentes num inteiro.
        /// </summary>
        /// <param name="value">O valor que contém os bits.</param>
        /// <returns>O número de indiferentes.</returns>
        private int CountDontCares(uint value)
        {
            var first = (value >> 1) & 0x55555555;
            var second = ((value << 1) & 0xAAAAAAAA) >> 1;
            return MathFunctions.PopCount(first & second);
        }

        private void InitializeMasks(int integerSize)
        {
            this.masks = new uint[integerSize >> 1];
            this.masks[0] = 3;
            var previousMask = this.masks[0];
            for (int i = 1; i < this.masks.Length; ++i)
            {
                this.masks[i] = previousMask << 2;
                previousMask = this.masks[i];
            }
        }

        public override string ToString()
        {
            var resultBuilder = new StringBuilder("[");
            var separator = string.Empty;
            var currentGeneralPointer = 0;
            var currentValuesPointer = 0;
            while (currentGeneralPointer < this.length)
            {
                var currentInnerIndex = 0;
                while (currentInnerIndex < this.semiIntegerSize &&
                    currentGeneralPointer < this.length)
                {
                    var element = this.values[currentValuesPointer];
                    var mask = this.masks[currentInnerIndex];
                    resultBuilder.AppendFormat(
                        "{1}{0}",
                        (EBooleanMinimalFormOutStatus)((element & mask) >> (currentInnerIndex << 1)),
                        separator);
                    separator = ", ";

                    ++currentInnerIndex;
                    ++currentGeneralPointer;
                }

                ++currentValuesPointer;
            }

            resultBuilder.Append("]");
            return resultBuilder.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
