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
                var dividendLength = dividend.Length;
                var divisorLength = divisor.Length;
                if (dividendLength < divisorLength)
                {
                    return Tuple.Create<ulong[], ulong[]>(null, dividend);
                }
                else if (dividendLength == divisorLength)
                {
                    var dividendHigh = MathFunctions.GetHighestSettedBitIndex(
                        dividend[dividendLength - 1]);
                    var divisorHigh = MathFunctions.GetHighestSettedBitIndex(
                        divisor[divisorLength - 1]);
                    if (dividendHigh < divisorHigh)
                    {
                        return Tuple.Create<ulong[], ulong[]>(null, dividend);
                    }
                    else if (dividendHigh == divisorHigh)
                    {
                        var firstDiff = this.FindSameLengthDifference(dividend, divisor);
                        if (firstDiff == -1)
                        {
                            // Ambos os vectores são iguais
                            return Tuple.Create<ulong[], ulong[]>(new ulong[] { 1UL }, null);
                        }
                        else
                        {
                            var currentDiv = dividend[firstDiff];
                            var currentQuo = divisor[firstDiff];
                            if (currentDiv > currentQuo)
                            {
                                // É retornada a diferença
                                var remainderLength = firstDiff + 1;
                                var remainder = new ulong[remainderLength];
                                var length = this.SubtractSameLength(
                                    dividend,
                                    divisor,
                                    remainderLength,
                                    remainder);
                                if (remainderLength == length)
                                {
                                    return Tuple.Create<ulong[], ulong[]>(
                                        new ulong[] { 1UL },
                                        remainder);
                                }
                                else
                                {
                                    var remRes = new ulong[length];
                                    Array.Copy(remainder, remRes, length);
                                    return Tuple.Create<ulong[], ulong[]>(
                                        new ulong[] { 1UL },
                                        remRes);
                                }
                            }
                            else
                            {
                                // O quociente é superior ao divisor
                                return Tuple.Create<ulong[], ulong[]>(null, dividend);
                            }
                        }
                    }
                    else
                    {
                        var shift = dividendHigh - divisorHigh;
                        var firstDiff = this.FindSameLengthDifference(
                            dividend,
                            divisor,
                            dividendLength,
                            shift);
                        var quo = 1UL << shift;
                        if (firstDiff == -1)
                        {
                            return Tuple.Create<ulong[], ulong[]>(new[] { quo }, null);
                        }
                        else
                        {
                            var currentDiv = dividend[firstDiff];
                            var currentQuo = divisor[firstDiff] << shift;
                            if (firstDiff > 0)
                            {
                                currentQuo |= (divisor[firstDiff - 1] >> (64 - shift));
                            }

                            var remainderLength = firstDiff + 1;
                            var remainder = new ulong[dividend.Length];
                            if (currentDiv < currentQuo)
                            {
                                var length = this.InvSubtractSameLength(
                                    divisor,
                                    shift,
                                    dividend,
                                    remainderLength,
                                    remainder);
                                if (length < divisor.Length)
                                {
                                    // O resultado pode ser determinado, sendo negativo o sinal do resto
                                    --quo;
                                    if (shift == 1)
                                    {
                                        length = this.SubtractSameLength(
                                            dividend,
                                            divisor,
                                            dividend.Length,
                                            remainder);
                                        if (length < dividendLength)
                                        {
                                            var remRes = new ulong[length];
                                            Array.Copy(remainder, remRes, length);
                                            return Tuple.Create<ulong[], ulong[]>(new[] { quo }, remRes);
                                        }
                                        else
                                        {
                                            return Tuple.Create<ulong[], ulong[]>(new[] { quo }, remainder);
                                        }
                                    }
                                    else
                                    {
                                        length = this.SubtractSameLength(
                                            dividend,
                                            divisor,
                                            dividendLength,
                                            shift - 1,
                                            remainder);
                                        if (length < dividendLength)
                                        {
                                            var remRes = new ulong[length];
                                            Array.Copy(remainder, remRes, length);
                                            return Tuple.Create<ulong[], ulong[]>(new[] { quo }, remRes);
                                        }
                                        else
                                        {
                                            return Tuple.Create<ulong[], ulong[]>(new[] { quo }, remainder);
                                        }
                                    }
                                }
                                else
                                {
                                    return this.ProcessSameLengthValues(
                                        remainder,
                                        length,
                                        divisor,
                                        new[] { quo },
                                        false);
                                }
                            }
                            else // O dividendo é superior ao divisor
                            {
                                var length = this.SubtractSameLength(
                                    dividend,
                                    divisor,
                                    remainderLength,
                                    shift,
                                    remainder);
                                if (length < divisor.Length)
                                {
                                    if (length < remainderLength)
                                    {
                                        var remRes = new ulong[length];
                                        Array.Copy(remainder, remRes, length);
                                        return Tuple.Create<ulong[], ulong[]>(new[] { quo }, remRes);
                                    }
                                    else
                                    {
                                        return Tuple.Create<ulong[], ulong[]>(new[] { quo }, remainder);
                                    }
                                }
                                else
                                {
                                    return this.ProcessSameLengthValues(
                                        remainder,
                                        length,
                                        divisor,
                                        new[] { quo },
                                        true);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var dividendHigh = MathFunctions.GetHighestSettedBitIndex(
                        dividend[dividendLength - 1]);
                    var divisorHigh = MathFunctions.GetHighestSettedBitIndex(
                        divisor[divisorLength - 1]);
                    if (dividendHigh < divisorHigh)
                    {
                    }
                    else if (dividendHigh == divisorHigh)
                    {
                    }
                    else
                    {
                    }
                }

                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Executa o processo da divisão sobre valores representados por vectores
        /// com o mesmo comprimento.
        /// </summary>
        /// <remarks>
        /// A função altera o vector que contém os valores do resto.
        /// </remarks>
        /// <param name="remainder">O vector que contém o resto actual.</param>
        /// <param name="length">O tamanho dos vectores.</param>
        /// <param name="divisor">O vector que contém o divisor.</param>
        /// <param name="currengQuotientValue">O valor do quociente actual.</param>
        /// <param name="sign">O sinal actual.</param>
        /// <param name="divisorShift">O deslocamento menor a ser aplicado ao divisor.</param>
        /// <returns>O quociente.</returns>
        private Tuple<ulong[], ulong[]> ProcessSameLengthValues(
            ulong[] remainder,
            int length,
            ulong[] divisor,
            ulong[] currengQuotientValue,
            bool sign)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executa o processo da divisão sobre valores representados por vectores
        /// com comprimentos diferentes.
        /// </summary>
        /// <remarks>
        /// A função altera o vector que contém os valores do resto.
        /// </remarks>
        /// <param name="remainder">O vector que contém o resto actual.</param>
        /// <param name="length">O tamanho dos vectores.</param>
        /// <param name="divisor">O vector que contém o divisor.</param>
        /// <param name="currengQuotientValue">O valor do quociente actual.</param>
        /// <param name="sign">O sinal actual.</param>
        /// <param name="divisorShift">O deslocamento menor a ser aplicado ao divisor.</param>
        /// <returns>O quociente.</returns>
        private Tuple<ulong[], ulong[]> ProcessDiffLengthValues(
            ulong[] remainder,
            int length,
            ulong[] divisor,
            ulong[] currengQuotientValue,
            bool sign)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determina o índice da diferença entre os vectores proporcionados,
        /// sendo estes do mesmo tamanho.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>O índice da primeira diferença.</returns>
        private int FindSameLengthDifference(
            ulong[] dividend,
            ulong[] divisor)
        {
            var result = -1;
            for (int i = dividend.Length - 1; i > result; --i)
            {
                if (dividend[i] != divisor[i])
                {
                    result = i;
                }
            }

            return result;
        }

        /// <summary>
        /// Determina o índice da primeira diferença, sabendo que os
        /// "bits" são deslocados em um valor inferior ao tamanho da variável.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <param name="length">O número de itens válidos em cada vector.</param>
        /// <param name="smallDivisorShift">O deslocamento.</param>
        /// <returns>O índice do vector do dividendo onde ocorre a diferença.</returns>
        private int FindSameLengthDifference(
            ulong[] dividend,
            ulong[] divisor,
            int length,
            int smallDivisorShift)
        {
            var i = length - 1;
            var shiftedDiv = divisor[i] << smallDivisorShift;
            for (int j = i - 1; j > -1; --j, --i)
            {
                shiftedDiv |= (divisor[j] >> (64 - smallDivisorShift));
                if (shiftedDiv != dividend[i])
                {
                    return i;
                }

                shiftedDiv = divisor[j] << smallDivisorShift;
            }

            if (shiftedDiv == dividend[0])
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Determina o índice da primeira diferença relativa ao dividendo,
        /// assumindo que o tamanho do divisor é inferior ou igual ao do dividendo.
        /// </summary>
        /// <remarks>
        /// É aqui assumido que o dígito mais significativo do divisor
        /// é alinhado com o dígito mais significativo do dividendo.
        /// </remarks>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="dividendLength">O número de itens válidos no dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <param name="divisorLength">O número de itens válidos no divisor.</param>
        /// <returns>O índice da diferença.</returns>
        private int FindOtherLengthDifference(
            ulong[] dividend,
            int dividendLength,
            ulong[] divisor,
            int divisorLength)
        {
            var result = -1;
            for (int i = dividendLength - 1, j = divisorLength - 1; j > -1; --i, --j)
            {
                if (dividend[i] != divisor[j])
                {
                    result = i;
                    j = -1;
                }
            }

            return result;
        }

        /// <summary>
        /// Determina o índice da primeira diferença, sabendo que os
        /// "bits" são deslocados em um valor inferior ao tamanho da variável e que o tamanho
        /// do divisor é inferior ou igual ao do dividendo.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="dividendLength">O número de itens válidos no dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <param name="divisorLength">O número de itens válidos no divisor.</param>
        /// <param name="smallDivisorShift">O deslocamento.</param>
        /// <returns>O índice do vector do dividendo onde ocorre a diferença.</returns>
        private int FindOtherLengthDifference(
            ulong[] dividend,
            int dividendLength,
            ulong[] divisor,
            int divisorLength,
            int smallDivisorShift)
        {
            var i = dividendLength - 1;
            var j = divisorLength - 1;
            var shiftDiv = divisor[j] << smallDivisorShift;
            --j;
            for (; j > -1; --i, --j)
            {
                shiftDiv |= (divisor[j] >> (64 - smallDivisorShift));
                if (shiftDiv != dividend[i])
                {
                    return i;
                }

                shiftDiv = divisor[j] << smallDivisorShift;
            }

            if (shiftDiv == dividend[i])
            {
                --i;
            }

            return i;
        }

        /// <summary>
        /// Determina o índice da primeira diferença, sabendo que os "bits" do divisor
        /// terão de ser deslocados para a direita.
        /// </summary>
        /// <remarks>
        /// A função é apenas válida quando o tamanho do vector do divisor for inferior ao
        /// tamanho do vector do dividendo.
        /// </remarks>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="dividendLength">O número de itens válidos no dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <param name="divisorLength">O número de itens válidos no divisor.</param>
        /// <param name="divShiftMagnitude">O deslocamento para a direita.</param>
        /// <returns>O índice do vector do dividendo onde ocorre a diferença.</returns>
        private int FindOtherLengthDiffNegativeShift(
            ulong[] dividend,
            int dividendLength,
            ulong[] divisor,
            int divisorLength,
            int divShiftMagnitude)
        {
            var i = dividendLength - 1;
            var j = divisorLength - 1;
            var shiftDiv = divisor[j] >> divShiftMagnitude;
            --j;
            for (; j > -1; --i, --j)
            {
                shiftDiv |= (divisor[j] << (64 - divShiftMagnitude));
                if (shiftDiv != dividend[i])
                {
                    return i;
                }

                shiftDiv = divisor[j] >> divShiftMagnitude;
            }

            if (shiftDiv == dividend[i])
            {
                --i;
            }

            return i;
        }

        /// <summary>
        ///  Determina a diferença entre dois números e coloca-a no vector dado para o resultado.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="length">O comprimento dos vectores.</param>
        /// <param name="outDifference">O vector que contém o resultado.</param>
        /// <returns>O tamanho do resultado no vector proporcionado.</returns>
        private int SubtractSameLength(
            ulong[] minuend,
            ulong[] subtrahend,
            int length,
            ulong[] outDifference)
        {
            var result = 0;
            var carry = false;
            var complement = ~subtrahend[0];
            var sum = MathFunctions.Add(minuend[0], complement);
            complement = sum.Item2 + 1;
            if (complement == 0)
            {
                carry = true;
            }
            else
            {
                carry = sum.Item1;
            }

            outDifference[0] = complement;
            for (int i = 1; i < length; ++i)
            {
                complement = ~subtrahend[i];
                sum = MathFunctions.Add(complement, minuend[i]);
                if (carry)
                {
                    if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        outDifference[i] = 0;
                        carry = true;
                    }
                    else
                    {
                        outDifference[i] = sum.Item2 + 1;
                        carry = sum.Item1;
                        result = i;
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            ++result;
            return result;
        }

        /// <summary>
        ///  Determina a diferença entre dois números e coloca-a no vector dado para o resultado.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="length">O número de itens válidos em cada vector.</param>
        /// <param name="subtrahendShift">O deslocamento aplicado ao subtraendo.</param>
        /// <param name="outDifference">O vector que contém o resultado.</param>
        /// <returns>O tamanho do resultado no vector proporcionado.</returns>
        private int SubtractSameLength(
            ulong[] minuend,
            ulong[] subtrahend,
            int length,
            int subtrahendShift,
            ulong[] outDifference)
        {
            var result = 0;
            var subtrahendLength = subtrahend.Length;
            var carry = false;
            var complement = subtrahend[0];
            var shiftCarry = complement >> (64 - subtrahendShift);
            complement <<= subtrahendShift;
            complement = ~complement;
            var sum = MathFunctions.Add(minuend[0], complement);
            complement = sum.Item2 + 1;
            if (complement == 0)
            {
                carry = true;
            }
            else
            {
                carry = sum.Item1;
            }

            outDifference[0] = complement;
            for (int i = 1; i < subtrahendLength; ++i)
            {
                var currentSubtrahend = subtrahend[i];
                complement = (currentSubtrahend << subtrahendShift) | shiftCarry;
                complement = ~complement;
                shiftCarry = currentSubtrahend >> (64 - subtrahendShift);
                sum = MathFunctions.Add(complement, minuend[i]);
                if (carry)
                {
                    if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        outDifference[i] = 0;
                        carry = true;
                    }
                    else
                    {
                        outDifference[i] = sum.Item2 + 1;
                        carry = sum.Item1;
                        result = i;
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            ++result;
            return result;
        }

        /// <summary>
        ///  Determina a diferença entre dois números e coloca-a no vector dado para o resultado.
        /// </summary>
        /// <remarks>
        /// Função aplicável ao caso em que o quociente, após deslocado, é superior ao dividendo
        /// acutal.
        /// </remarks>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="minuendShift">O deslocamento aplicado ao minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="length">O número de itens válidos em cada vector.</param>
        /// <param name="outDifference">O vector que contém o resultado.</param>
        /// <returns>O tamanho do resultado no vector proporcionado.</returns>
        private int InvSubtractSameLength(
            ulong[] minuend,
            int minuendShift,
            ulong[] subtrahend,
            int length,
            ulong[] outDifference)
        {
            var result = 0;
            var subtrahendLength = subtrahend.Length;
            var carry = false;
            var currentMinuend = minuend[0];
            var shiftCarry = currentMinuend >> (64 - minuendShift);
            currentMinuend <<= minuendShift;
            var complement = ~subtrahend[0];
            var sum = MathFunctions.Add(minuend[0], complement);
            complement = sum.Item2 + 1;
            if (complement == 0)
            {
                carry = true;
            }
            else
            {
                carry = sum.Item1;
            }

            outDifference[0] = complement;
            for (int i = 1; i < subtrahendLength; ++i)
            {
                currentMinuend = minuend[i];
                var tempCarry = currentMinuend >> (64 - minuendShift);
                currentMinuend = (currentMinuend << minuendShift) | shiftCarry;
                shiftCarry = tempCarry;
                complement = ~subtrahend[i];
                sum = MathFunctions.Add(complement, minuend[i]);
                if (carry)
                {
                    if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        outDifference[i] = 0;
                        carry = true;
                    }
                    else
                    {
                        outDifference[i] = sum.Item2 + 1;
                        carry = sum.Item1;
                        result = i;
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            ++result;
            return result;
        }

        /// <summary>
        /// Determina a diferença entre dois números na qual o subtraendo se encontra
        /// deslocado.
        /// </summary>
        /// <remarks>
        /// A escrita no vector de saída é iniciada no final do deslocamento.
        /// </remarks>
        /// <param name="minuend">O dividendo.</param>
        /// <param name="minuendLength">O número de itens do dividendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="subtrahendOffset">O deslocamento do divisor.</param>
        /// <param name="outDifference">O vector que contém o resultado.</param>
        /// <returns>O tamanho do resultado no vector proporcionado.</returns>
        private int SubtractDiffLength(
            ulong[] minuend,
            int minuendLength,
            ulong[] subtrahend,
            int subtrahendOffset,
            ulong[] outDifference)
        {
            var result = 0;
            var carry = false;
            var i = subtrahendOffset;
            var j = 0;
            var complement = ~subtrahend[j];
            var sum = MathFunctions.Add(minuend[i], complement);
            complement = sum.Item2 + 1;
            if (complement == 0)
            {
                carry = true;
            }
            else
            {
                carry = sum.Item1;
            }

            outDifference[i] = complement;
            ++i;
            ++j;
            for (; i < minuendLength; ++i, ++j)
            {
                complement = ~subtrahend[i];
                sum = MathFunctions.Add(complement, minuend[i]);
                if (carry)
                {
                    if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        outDifference[i] = 0;
                        carry = true;
                    }
                    else
                    {
                        outDifference[i] = sum.Item2 + 1;
                        carry = sum.Item1;
                        result = i;
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            ++result;
            return result;
        }

        /// <summary>
        /// Determina a diferença entre dois números na qual o minuendo se encontra
        /// deslocado.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="minuendOffset">O deslocamento do minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="subtrahendLength">O tamanho do subtraendo.</param>
        /// <param name="outDifference">O vector que contém o resultado.</param>
        /// <returns>O tamanho do resultado no vector proporcionado.</returns>
        private int InvSubtractDiffLength(
            ulong[] minuend,
            int minuendOffset,
            ulong[] subtrahend,
            int subtrahendLength,
            ulong[] outDifference)
        {
            var result = 0;
            var carry = false;
            var i = 0;

            // Tratamento dos valores iniciais
            if (i < minuendOffset)
            {
                var currentSubtrahend = subtrahend[i];
                if (currentSubtrahend == 0)
                {
                    outDifference[i] = 0;
                }
                else
                {
                    carry = true;
                    outDifference[i] = ~currentSubtrahend + 1;
                    result = i;
                }

                ++i;
                for (; i < minuendOffset; ++i)
                {
                    currentSubtrahend = subtrahend[i];
                    if (carry)
                    {
                        carry = true;
                        currentSubtrahend = subtrahend[i];
                        if (currentSubtrahend == 0xFFFFFFFFFFFFFFFF)
                        {
                            outDifference[i] = 0;
                        }
                        else
                        {
                            outDifference[i] = currentSubtrahend + 1;
                            result = i;
                        }
                    }
                    else
                    {
                        if (currentSubtrahend == 0)
                        {
                            outDifference[i] = 0;
                            carry = false;
                        }
                        else
                        {
                            carry = true;
                            outDifference[i] = ~currentSubtrahend + 1;
                            result = i;
                        }
                    }
                }
            }

            var j = 0;
            for (; i < subtrahendLength; ++i)
            {
                var complement = ~subtrahend[i];
                var sum = MathFunctions.Add(complement, minuend[j]);
                if (carry)
                {
                    if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        outDifference[i] = 0;
                        carry = true;
                    }
                    else
                    {
                        outDifference[i] = sum.Item2 + 1;
                        carry = sum.Item1;
                        result = i;
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            return result;
        }

        /// <summary>
        /// Determina a diferença entre dois números na qual o subtraendo se encontra
        /// deslocado e é submetido a uma rotação.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="minuendLength">O tamanho do minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="subtrahendOffset">O deslocamento do subtraendo.</param>
        /// <param name="subtrahendShift">A rotação do subtraendo.</param>
        /// <param name="outDifference">O vector que contém o resultado.</param>
        /// <returns>O tamanho do resultado no vector proporcionado.</returns>
        private int SubtractDiffLength(
            ulong[] minuend,
            int minuendLength,
            ulong[] subtrahend,
            int subtrahendOffset,
            int subtrahendShift,
            ulong[] outDifference)
        {
            var result = 0;
            var carry = false;
            var i = subtrahendOffset;
            var j = 0;
            var rotationCarry = subtrahend[j];
            var complement = ~(rotationCarry << subtrahendShift);
            rotationCarry >>= 64 - subtrahendShift;
            var sum = MathFunctions.Add(minuend[i], complement);
            complement = sum.Item2 + 1;
            if (complement == 0)
            {
                carry = true;
            }
            else
            {
                carry = sum.Item1;
            }

            outDifference[i] = complement;
            ++i;
            ++j;
            for (; i < minuendLength; ++i, ++j)
            {
                var current = subtrahend[i];
                complement = ~((current << subtrahendShift) | rotationCarry);
                rotationCarry = current >> (64 - subtrahendShift);
                sum = MathFunctions.Add(complement, minuend[i]);
                if (carry)
                {
                    if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        outDifference[i] = 0;
                        carry = true;
                    }
                    else
                    {
                        outDifference[i] = sum.Item2 + 1;
                        carry = sum.Item1;
                        result = i;
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            ++result;
            return result;
        }

        /// <summary>
        /// Determina a diferença enter dois números na qual o minuendo se encontra
        /// deslocado e é submetido a uma rotação.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="minuendOffset">O deslocamento do minuendo.</param>
        /// <param name="minuendShift">A rotação do minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="subtrahendLength">O tamanho do subtraendo.</param>
        /// <param name="outDifference">O vector que contém o resultado.</param>
        /// <returns>O tamanho do resultado no vector proporcionado.</returns>
        private int InvSubtractDiffLength(
            ulong[] minuend,
            int minuendOffset,
            int minuendShift,
            ulong[] subtrahend,
            int subtrahendLength,
            ulong[] outDifference)
        {
            var result = 0;
            var carry = false;
            var i = 0;

            // Tratamento dos valores iniciais
            if (i < minuendOffset)
            {
                var currentSubtrahend = subtrahend[i];
                if (currentSubtrahend == 0)
                {
                    outDifference[i] = 0;
                }
                else
                {
                    carry = true;
                    outDifference[i] = ~currentSubtrahend + 1;
                    result = i;
                }

                ++i;
                for (; i < minuendOffset; ++i)
                {
                    currentSubtrahend = subtrahend[i];
                    if (carry)
                    {
                        carry = true;
                        currentSubtrahend = subtrahend[i];
                        if (currentSubtrahend == 0xFFFFFFFFFFFFFFFF)
                        {
                            outDifference[i] = 0;
                        }
                        else
                        {
                            outDifference[i] = currentSubtrahend + 1;
                            result = i;
                        }
                    }
                    else
                    {
                        if (currentSubtrahend == 0)
                        {
                            outDifference[i] = 0;
                            carry = false;
                        }
                        else
                        {
                            carry = true;
                            outDifference[i] = ~currentSubtrahend + 1;
                            result = i;
                        }
                    }
                }
            }

            // Execução do primeiro processo do ciclo
            var j = 0;
            var current = minuend[j];
            var rotationCarry = current >> (64 - minuendShift);
            current <<= minuendShift;
            var complement = ~subtrahend[i];
            var sum = MathFunctions.Add(complement, current);
            if (carry)
            {
                if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                {
                    outDifference[i] = 0;
                    carry = true;
                }
                else
                {
                    outDifference[i] = sum.Item2 + 1;
                    carry = sum.Item1;
                    result = i;
                }
            }
            else
            {
                outDifference[i] = sum.Item2;
                carry = sum.Item1;
            }

            ++i;
            ++j;
            for (; i < subtrahendLength; ++i)
            {
                var aux = minuend[j];
                current = (aux << minuendShift) | rotationCarry;
                rotationCarry = aux >> (64 - minuendShift);

                complement = ~subtrahend[i];
                sum = MathFunctions.Add(complement, current);
                if (carry)
                {
                    if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        outDifference[i] = 0;
                        carry = true;
                    }
                    else
                    {
                        outDifference[i] = sum.Item2 + 1;
                        carry = sum.Item1;
                        result = i;
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            return result;
        }
    }
}
