namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Provedor que permite determinar a representação vectorial do resto e do quociente
    /// a partir da representação vectorial do dividendo e do divisor, tratando-se de representações
    /// de inteiros enormes.
    /// </summary>
    internal class UlongBigIntegerSequentialQuotAndRemAlg
        : IAlgorithm<ulong[], ulong[], Tuple<ulong[], ulong[]>>
    {
        /// <summary>
        /// Mantém o valor actual do quociente.
        /// </summary>
        private ulong[] currentQuotient;

        /// <summary>
        /// Mantém o valor actual do resto.
        /// </summary>
        private ulong[] currentRemainder;

        /// <summary>
        /// O tamanho válido do resto.
        /// </summary>
        private int currentRemainderLength;

        /// <summary>
        /// O quociente rodado actual.
        /// </summary>
        private ulong[] currentShiftQuotient;

        /// <summary>
        /// O valor corrente do dividendo.
        /// </summary>
        private ulong currentDivisorValue;

        /// <summary>
        /// O valor corrente do divisor.
        /// </summary>
        private ulong currentDividendValue;

        /// <summary>
        /// Determina a representação vectorial do resto e do quociente, recebendo as representações vectoriais
        /// do dividendo e do divisor.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>O par quociente/resto da divisão.</returns>
        public Tuple<ulong[], ulong[]> Run(ulong[] dividend, ulong[] divisor)
        {
            if (divisor == null)
            {
                throw new DivideByZeroException();
            }
            else if (dividend == null)
            {
                return Tuple.Create<ulong[], ulong[]>(null, null);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Subtrai o quociente roadado do resto.
        /// </summary>
        /// <param name="shiftOffset">O deslocamento do quociente rodado.</param>
        private void SubtractShiftedQuoFromRem(int shiftOffset)
        {
            var result = 0;
            var subtraendLength = this.currentShiftQuotient.Length;
            var minuendIndex = shiftOffset;

            var carry = false;
            var complement = ~this.currentShiftQuotient[0];
            var sum = MathFunctions.Add(this.currentRemainder[minuendIndex], complement);
            complement = sum.Item2 + 1;
            if (complement == 0)
            {
                carry = true;
            }
            else
            {
                carry = sum.Item1;
            }

            this.currentRemainder[minuendIndex] = complement;
            for (int i = 1; i < subtraendLength; ++i)
            {
                ++minuendIndex;
                complement = ~this.currentShiftQuotient[i];
                sum = MathFunctions.Add(this.currentRemainder[i], complement);
                if (carry)
                {
                    if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        this.currentRemainder[minuendIndex] = 0;
                        carry = true;
                    }
                    else
                    {
                        this.currentRemainder[minuendIndex] = sum.Item2 + 1;
                        carry = sum.Item1;
                        result = minuendIndex;
                    }
                }
                else
                {
                    this.currentRemainder[minuendIndex] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            ++minuendIndex;
            for (int i = minuendIndex; i < this.currentRemainderLength; ++i)
            {
                sum = MathFunctions.Add(this.currentRemainder[i], 0xFFFFFFFFFFFFFFFF);
                if (carry)
                {
                    if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        this.currentRemainder[i] = 0;
                        carry = true;
                    }
                    else
                    {
                        this.currentRemainder[i] = sum.Item2 + 1;
                        carry = sum.Item1;
                        result = i;
                    }
                }
                else
                {
                    this.currentRemainder[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            for (int i = this.currentRemainderLength - 1; i > result; --i)
            {
                if (this.currentRemainder[i] != 0ul)
                {
                    result = i;
                }
            }

            ++result;
            this.currentRemainderLength = result;
        }

        /// <summary>
        /// Subtrai o resto do quociente rodado.
        /// </summary>
        /// <remarks>O resultado é colocado no resto.</remarks>
        /// <param name="shiftOffset">O deslocamento do quociente rodado.</param>
        private void SubtractRemFromShiftedQuo(int shiftOffset)
        {
            var remLength = this.currentRemainder.Length;
            var result = -1;
            var subtraendIndex = 0;
            var minuendIndex = 0;
            var carry = false;
            var complement = ~this.currentRemainder[subtraendIndex];
            ++complement;
            if (subtraendIndex == shiftOffset)
            {
                var addValues = MathFunctions.Add(this.currentShiftQuotient[minuendIndex], complement);
                carry = addValues.Item1;
                this.currentRemainder[subtraendIndex] = addValues.Item2;
                ++subtraendIndex;
                ++minuendIndex;
            }
            else
            {
                this.currentRemainder[subtraendIndex] = complement;
                if (complement != 0)
                {
                    result = subtraendIndex;
                }

                ++subtraendIndex;
                while (subtraendIndex < shiftOffset)
                {
                    complement = ~this.currentRemainder[subtraendIndex];
                    this.currentRemainder[subtraendIndex] = complement;
                    if (complement != 0)
                    {
                        result = subtraendIndex;
                    }

                    ++subtraendIndex;
                }
            }

            while (subtraendIndex < remLength)
            {
                complement = ~this.currentRemainder[subtraendIndex];
                var sum = MathFunctions.Add(this.currentRemainder[minuendIndex], complement);
                if (carry)
                {
                    if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        this.currentRemainder[minuendIndex] = 0;
                        carry = true;
                    }
                    else
                    {
                        this.currentRemainder[minuendIndex] = sum.Item2 + 1;
                        carry = sum.Item1;
                        result = minuendIndex;
                    }
                }
                else
                {
                    this.currentRemainder[minuendIndex] = sum.Item2;
                    carry = sum.Item1;
                }

                ++minuendIndex;
                ++subtraendIndex;
            }

            var shiftLength = this.currentShiftQuotient.Length;
            while (minuendIndex < shiftLength)
            {
                var sum = MathFunctions.Add(this.currentRemainder[minuendIndex], 0xFFFFFFFFFFFFFFFF);
                if (carry)
                {
                    if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        this.currentRemainder[minuendIndex] = 0;
                        carry = true;
                    }
                    else
                    {
                        this.currentRemainder[minuendIndex] = sum.Item2 + 1;
                        carry = sum.Item1;
                        result = minuendIndex;
                    }
                }
                else
                {
                    this.currentRemainder[minuendIndex] = sum.Item2;
                    carry = sum.Item1;
                }

                ++minuendIndex;
            }

            for (int i = shiftLength - 1; i > result; --i)
            {
                if (this.currentRemainder[i] != 0ul)
                {
                    result = i;
                }
            }

            ++result;
            this.currentRemainderLength = result;
        }

        /// <summary>
        /// Função auxiliar que permite rodar à equerda um vector de longos sem sinal em um número de bits
        /// inferior ao tamanho da variável.
        /// </summary>
        /// <remarks>
        /// Função definida para auxiliar o processo de divisão. Neste caso, o vector passado no argumento
        /// terá de conter pelo menos um valor.
        /// </remarks>
        /// <param name="value">O valor a ser rodado.</param>
        /// <param name="places">O número bits a ser aplicado na rotação, inferior a 64.</param>
        private void RotateLeftToShiftQuotient(ulong[] value, int places)
        {
            var output = this.currentShiftQuotient;
            if (places == 0)
            {
                Array.Copy(value, output, value.Length);
            }
            else
            {
                var counterRotate = 64 - places;
                var index = value.Length - 1;
                output[index] = value[index] << places;
                for (int i = index - 1; i > -1; --i, --index)
                {
                    var current = value[i];
                    output[index] |= (current >> counterRotate);
                    output[i] = current << places;
                }
            }
        }

        /// <summary>
        /// Função auxiliar que permite rodar à direita um vector de longos sem sinal em um número de bits
        /// inferior ao tamanho da variável.
        /// </summary>
        /// <remarks>
        /// Função definida para auxiliar o processo de divisão.
        /// </remarks>
        /// <param name="value">O valor a ser rodado.</param>
        /// <param name="places">O número de posições a rodar, inferior a 64.</param>
        private void RotateRightToShiftQuotient(ulong[] value, int places)
        {
            var output = this.currentShiftQuotient;
            var valueLength = value.Length;
            if (places == 0)
            {
                Array.Copy(value, 0, output, 1, valueLength);
            }
            else
            {
                var couterRotate = 64 - places;
                var index = 0;
                var i = 0;
                var current = value[i];
                output[index] = current << couterRotate;
                ++index;
                if (index < valueLength)
                {
                    output[index] = current >> places;
                    ++i;
                    for (; i < valueLength; ++i)
                    {
                        output[index] |= (current << couterRotate);
                        current = value[i];
                        ++index;
                        output[index] = current >> places;
                    }
                }
            }
        }

        /// <summary>
        /// Permite determinar as diferenças entre o dividendo e o divisor actual,
        /// retornando o índice onde se encontra a primeira diferença.
        /// </summary>
        /// <remarks>
        /// O índice retornado é sempre relativo ao resto actual. A função actualiza os
        /// campos correntes do dividendo e do divisor.
        /// </remarks>
        /// <returns>O valor do índice.</returns>
        private int FindDifference()
        {
            var remainderIndex = this.currentRemainderLength - 1;
            var shiftedIndex = this.currentShiftQuotient.Length - 1;
            var currentDividend = this.currentRemainder[remainderIndex];
            var currentDivisor = this.currentRemainder[shiftedIndex];
            if (currentDividend == currentDivisor)
            {
                --remainderIndex;
                --shiftedIndex;
                while (shiftedIndex > -1)
                {
                    currentDividend = this.currentRemainder[remainderIndex];
                    currentDivisor = this.currentRemainder[shiftedIndex];
                    if (currentDividend == currentDivisor)
                    {
                        --remainderIndex;
                        --shiftedIndex;
                    }
                    else
                    {
                        this.currentDividendValue = currentDividend;
                        this.currentDivisorValue = currentDivisor;
                        return remainderIndex;
                    }
                }

                while (remainderIndex > -1)
                {
                    currentDividend = this.currentRemainder[remainderIndex];
                    if (currentDividend == 0ul)
                    {
                        --remainderIndex;
                    }
                    else
                    {
                        this.currentDividendValue = currentDividend;
                        this.currentDivisorValue = 0ul;
                        return remainderIndex;
                    }
                }
            }
            else
            {
                this.currentDividendValue = currentDividend;
                this.currentDivisorValue = currentDivisor;
                return remainderIndex;
            }

            // Ambos os valores são iguais
            return -1;
        }
    }
}
