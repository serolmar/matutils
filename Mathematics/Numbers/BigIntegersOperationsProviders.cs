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
        : IAlgorithm<ulong[],ulong[],Tuple<ulong[],ulong[]>>
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
        /// Determina a representação vectorial do resto e do quociente, recebendo as representações vectoriais
        /// do dividendo e do divisor.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>O par quociente/resto da divisão.</returns>
        public Tuple<ulong[], ulong[]> Run(ulong[] dividend, ulong[] divisor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Subtrai o quociente roadado do resto.
        /// </summary>
        /// <param name="shiftOffset">O deslocamento do quociente rodado.</param>
        /// <returns>O tamanho do novo resto.</returns>
        private int SubtractShiftedQuoFromRem(int shiftOffset)
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
                    if (sum.Item2 != 0)
                    {
                        result = i;
                    }
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
                    if (sum.Item2 != 0)
                    {
                        result = i;
                    }
                }
            }

            ++result;
            return result;
        }

        /// <summary>
        /// Subtrai o resto do quociente rodado.
        /// </summary>
        /// <remarks>O resultado é colocado no resto.</remarks>
        /// <param name="shiftOffset">O deslocamento do quociente rodado.</param>
        /// <returns>O tamanho do novo resto.</returns>
        private int SubtractRemFromShiftedQuo(int remLength, int shiftOffset)
        {
            var result = -1;
            var subtraendIndex = 0;
            var carry = false;
            var complement = ~this.currentRemainder[subtraendIndex];
            ++complement;
            if (subtraendIndex == shiftOffset)
            {
                var addValues = MathFunctions.Add(this.currentShiftQuotient[subtraendIndex], complement);
                carry = addValues.Item1;
                this.currentRemainder[subtraendIndex] = addValues.Item2;
                if (addValues.Item2 != 0)
                {
                    result = 0;
                }
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

                var minuendIndex = 0;
                while (subtraendIndex < remLength)
                {

                }
            }
            throw new NotImplementedException();
        }
    }
}
