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

        /// <summary>
        /// As máscaras.
        /// </summary>
        private static uint[] masks;

        /// <summary>
        /// O tamanho de uma variável inteira.
        /// </summary>
        private static int integerSize;

        /// <summary>
        /// Metade do tamanho de uma variável inteira.
        /// </summary>
        private static int semiIntegerSize;

        /// <summary>
        /// O comprimento do vector lógico.
        /// </summary>
        private int length;

        /// <summary>
        /// O contentor do vector lógico.
        /// </summary>
        private uint[] values;

        /// <summary>
        /// Inicializa a classe <see cref="LogicCombinationBitArray"/>.
        /// </summary>
        static LogicCombinationBitArray()
        {
            integerSize = sizeof(int) << 3;
            semiIntegerSize = integerSize >> 1;
            InitializeMasks(integerSize);
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LogicCombinationBitArray"/>.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o tamanho for negativo.</exception>
        public LogicCombinationBitArray(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.length = length;
                var requiredLength = length << 1;
                var requiredSpace = requiredLength / integerSize;
                if (this.length % integerSize != 0)
                {
                    ++requiredSpace;
                }

                this.values = new uint[requiredSpace];
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LogicCombinationBitArray"/>.
        /// </summary>
        /// <param name="length">O tamanho.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o tamanho for negativo.</exception>
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
                semiIntegerSize = integerSize >> 1;
                var requiredLength = length << 1;
                var requiredSpace = requiredLength / integerSize;
                if (this.length % integerSize != 0)
                {
                    ++requiredSpace;
                }

                this.values = new uint[requiredSpace];
                InitializeMasks(integerSize);
                if (defaultValue != EBooleanMinimalFormOutStatus.ERROR)
                {
                    this.SetAll(defaultValue);
                }
            }
        }

        /// <summary>
        /// Construtor que permite realizar cópias internamente.
        /// </summary>
        private LogicCombinationBitArray()
        {
        }


        /// <summary>
        /// Obtém e atribui o valor lógico na entrada especificada pelo índice.
        /// </summary>
        /// <value>O valor lógico.</value>
        /// <param name="index">O índice.</param>
        /// <returns>O valor lógico.</returns>
        /// <exception cref="IndexOutOfRangeException">Se o índice não se encontrar nos limites do vector.</exception>
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
                    var elementIndex = index / semiIntegerSize;
                    var innerIndex = index % semiIntegerSize;
                    var element = this.values[elementIndex];
                    var mask = masks[innerIndex];
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
                    var elementIndex = index / semiIntegerSize;
                    var innerIndex = index % semiIntegerSize;
                    var element = this.values[elementIndex];
                    var mask = masks[innerIndex];
                    this.values[elementIndex] = (~mask & element) | ((uint)value << (innerIndex << 1));
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho da combinação lógica.
        /// </summary>
        /// <value>O tamanho.</value>
        public int Length
        {
            get
            {
                return this.length;
            }
        }

        /// <summary>
        /// Atribui o valor especificado a todas as entradas da combinação.
        /// </summary>
        /// <param name="status">O valor a ser atribuído.</param>
        /// <exception cref="MathematicsException">Se o argumento contiver um valor não suportado.</exception>
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
            cover = default(LogicCombinationBitArray);
            if (combination == null)
            {
                return false;
            }
            if (this.length != combination.length)
            {
                return false;
            }
            else
            {
                var xoredValues = this.XorValues(this.values, combination.values);

                // Se não existirem valores de indiferentes que se sobreponham a valores ligado / desligado...
                if (this.HasNoBitValues(xoredValues))
                {
                    bool foundIndex = false;
                    var currentGeneralPointer = 0;
                    var currentValuesPointer = 0;
                    while (currentGeneralPointer < this.length)
                    {
                        var currentInnerIndex = 0;
                        while (currentInnerIndex < semiIntegerSize &&
                            currentGeneralPointer < this.length)
                        {
                            var valuesElement = this.values[currentValuesPointer];
                            var xoredElement = xoredValues[currentValuesPointer];
                            var mask = masks[currentInnerIndex];

                            var statusValue = (valuesElement & mask) >> (currentInnerIndex << 1);
                            if (statusValue == 3U)
                            {
                                xoredElement = (~mask & xoredElement) |
                                    (3U << (currentInnerIndex << 1));
                                xoredValues[currentValuesPointer] = xoredElement;
                            }
                            else
                            {
                                statusValue = (xoredElement & mask) >> (currentInnerIndex << 1);
                                if (statusValue == 3)
                                {
                                    if (foundIndex)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        xoredElement = (~mask & xoredElement) |
                                            (3U << (currentInnerIndex << 1));
                                        xoredValues[currentValuesPointer] = xoredElement;
                                        foundIndex = true;
                                    }
                                }
                            }

                            ++currentInnerIndex;
                            ++currentGeneralPointer;
                        }

                        ++currentValuesPointer;
                    }

                    xoredValues = this.OrValues(this.values, xoredValues);
                    cover = new LogicCombinationBitArray();
                    cover.length = this.length;
                    cover.values = xoredValues;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Conta o número de elementos com o valor especificado.
        /// </summary>
        /// <param name="status">O valor a ser contado.</param>
        /// <returns>O número de elementos com o valor esepcificado.</returns>
        /// <exception cref="MathematicsException">Se o argumento contiver um valor não suportado.</exception>
        public int CountElementsWithValue(EBooleanMinimalFormOutStatus status)
        {
            if (status == EBooleanMinimalFormOutStatus.DONT_CARE)
            {
                return this.CountDontCares(this.values);
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

        /// <summary>
        /// Permite enumerar a lista de valores.
        /// </summary>
        /// <returns>O enumerador para a lista de valores.</returns>
        public IEnumerator<EBooleanMinimalFormOutStatus> GetEnumerator()
        {
            var currentGeneralPointer = 0;
            var currentValuesPointer = 0;
            while (currentGeneralPointer < this.length)
            {
                var currentInnerIndex = 0;
                while (currentInnerIndex < semiIntegerSize &&
                    currentGeneralPointer < this.length)
                {
                    var element = this.values[currentValuesPointer];
                    var mask = masks[currentInnerIndex];
                    yield return (EBooleanMinimalFormOutStatus)((element & mask) >> (currentInnerIndex << 1));
                }

                ++currentValuesPointer;
            }
        }

        /// <summary>
        /// Obtém a representação textual da combinação lógica.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            var resultBuilder = new StringBuilder("[");
            var separator = string.Empty;
            var currentGeneralPointer = 0;
            var currentValuesPointer = 0;
            while (currentGeneralPointer < this.length)
            {
                var currentInnerIndex = 0;
                while (currentInnerIndex < semiIntegerSize &&
                    currentGeneralPointer < this.length)
                {
                    var element = this.values[currentValuesPointer];
                    var mask = masks[currentInnerIndex];
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

        /// <summary>
        /// Verifica se a combinação lógica corrente é igual a outra.
        /// </summary>
        /// <param name="obj">A combinação lógica a ser verificada.</param>
        /// <returns>Verdadeiro caso ambas as combinações sejam iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else
            {
                var innerObj = obj as LogicCombinationBitArray;
                if (innerObj == null)
                {
                    return false;
                }
                else if (this.length != innerObj.length)
                {
                    return false;
                }
                else
                {
                    var lastIndex = this.values.Length - 1;
                    for (int i = 0; i < lastIndex; ++i)
                    {
                        if (this.values[i] != innerObj.values[i])
                        {
                            return false;
                        }
                    }

                    var innerIndex = (this.length << 1) % integerSize;
                    var mask = uint.MaxValue >> (integerSize - innerIndex);
                    var currentValue = this.values[lastIndex] & mask;
                    var otherValue = innerObj.values[lastIndex] & mask;
                    return currentValue == otherValue;
                }
            }
        }

        /// <summary>
        /// Obtém o código confuso da combinações lógica.
        /// </summary>
        /// <returns>O valor do código confuso.</returns>
        public override int GetHashCode()
        {
            var result = 17;
            var lastIndex = this.values.Length - 1;
            for (int i = 0; i < lastIndex; ++i)
            {
                result ^= this.values[i].GetHashCode();
            }

            var innerIndex = (this.length << 1) % integerSize;
            var mask = uint.MaxValue >> (integerSize - innerIndex);
            var value = this.values[lastIndex] & mask;
            result ^= value.GetHashCode();

            return result;
        }

        /// <summary>
        /// Determina o ou exclusivo entre dois valores.
        /// </summary>
        /// <param name="first">O primeiro valor.</param>
        /// <param name="second">O segundo valor.</param>
        /// <returns>O resultado do ou exclusivo.</returns>
        private uint[] XorValues(uint[] first, uint[] second)
        {
            var result = new uint[first.Length];
            for (int i = 0; i < first.Length; ++i)
            {
                result[i] = first[i] ^ second[i];
            }

            return result;
        }

        /// <summary>
        /// Determina o ou entre dois valores.
        /// </summary>
        /// <param name="first">O primeiro valor.</param>
        /// <param name="second">O segundo valor.</param>
        /// <returns>O resultado do ou.</returns>
        private uint[] OrValues(uint[] first, uint[] second)
        {
            var result = new uint[first.Length];
            for (int i = 0; i < first.Length; ++i)
            {
                result[i] = first[i] | second[i];
            }

            return result;
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

        /// <summary>
        /// Conta o número de indiferentes num conjunto de valores.
        /// </summary>
        /// <param name="valueSet">O conjunto de valores.</param>
        /// <returns>O número de indiferentes.</returns>
        private int CountDontCares(uint[] valueSet)
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

        /// <summary>
        /// Verifica se existem valores nos estados ligado / desligado.
        /// </summary>
        /// <param name="valueSet">Os valores a serem analisados.</param>
        /// <returns>Verdadeiro se não existirem valores nos estado ligad / desligado e falso caso contrário.</returns>
        private bool HasNoBitValues(uint[] valueSet)
        {
            var last = valueSet.Length - 1;
            for (int i = 0; i < this.values.Length - 1; ++i)
            {
                var currentValue = valueSet[i];
                var first = (currentValue >> 1) & 0x55555555;
                var second = ((currentValue << 1) & 0xAAAAAAAA) >> 1;
                if ((first ^ second) == 0)
                {
                    return false;
                }
            }

            var lastCount = (this.length % 16);
            var lastValue = valueSet[last] & ((1U << (lastCount << 1)) - 1);
            var firstTemp = (lastValue >> 1) & 0x55555555;
            var secondTemp = ((lastValue << 1) & 0xAAAAAAAA) >> 1;
            return (firstTemp ^ secondTemp) == 0;
        }

        /// <summary>
        /// Inicializa as máscaras de extracção.
        /// </summary>
        /// <param name="integerSize">O valor do tamanho de um inteiro.</param>
        private static void InitializeMasks(int integerSize)
        {
            masks = new uint[integerSize >> 1];
            masks[0] = 3;
            var previousMask = masks[0];
            for (int i = 1; i < masks.Length; ++i)
            {
                masks[i] = previousMask << 2;
                previousMask = masks[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
