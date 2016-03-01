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
                else if (dividendLength == divisorLength) // Mesmo comprimento
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
                        var firstDiff = default(int);
                        var comparision = this.FindSameLengthDifference(dividend, divisor, dividend.Length, out firstDiff);
                        if (comparision == 0)
                        {
                            // Ambos os vectores são iguais
                            return Tuple.Create<ulong[], ulong[]>(new ulong[] { 1UL }, null);
                        }
                        else if (comparision == 1)
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
                    else
                    {
                        var shift = dividendHigh - divisorHigh;
                        var firstDiff = default(int);
                        var comparision = this.FindSameLengthDifference(
                            dividend,
                            divisor,
                            dividendLength,
                            shift,
                            out firstDiff);
                        var quo = 1UL << shift;
                        if (comparision == 0)
                        {
                            return Tuple.Create<ulong[], ulong[]>(new[] { quo }, null);
                        }
                        else if (comparision == -1)
                        {
                            var remainderLength = firstDiff + 1;
                            var remainder = new ulong[Math.Max(divisor.Length, remainderLength)];
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
                                length = this.SubtractSameLength(
                                        divisor,
                                        remainder,
                                        divisor.Length,
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
                                return this.ProcessSameLengthValues(
                                    remainder,
                                    length,
                                    divisor,
                                    new[] { quo },
                                    false);
                            }
                        }
                        else
                        {
                            var remainderLength = firstDiff + 1;
                            var remainder = new ulong[remainderLength];
                            var length = this.SubtractSameLength(
                                    dividend,
                                    divisor,
                                    remainderLength,
                                    shift,
                                    remainder);
                            if (length < divisor.Length)
                            {
                                if (length < remainder.Length)
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
                else // Comprimento diferente
                {
                    var lengthDiff = dividendLength - divisorLength;
                    var dividendHigh = MathFunctions.GetHighestSettedBitIndex(
                        dividend[dividendLength - 1]);
                    var divisorHigh = MathFunctions.GetHighestSettedBitIndex(
                        divisor[divisorLength - 1]);

                    if (dividendHigh < divisorHigh)
                    {
                        // Estabelece a primeira aproximação para o quociente
                        var quo = new ulong[lengthDiff];
                        var shift = divisorHigh - dividendHigh;
                        quo[lengthDiff - 1] = 1UL << (64 - shift);

                        // Deslocamentos negativos
                        var foundIndex = default(int);
                        var comparision = this.FindOtherLengthDiffNegativeShift(
                            dividend,
                            dividendLength,
                            divisor,
                            divisorLength,
                            shift,
                            out foundIndex);
                        if (comparision == 0)
                        {
                            // Conta certa
                            return Tuple.Create<ulong[], ulong[]>(quo, null);
                        }
                        else if (comparision == 1)
                        {
                            var remainderLength = foundIndex + 1;
                            var remainder = new ulong[remainderLength];
                            Array.Copy(dividend, remainder, lengthDiff);

                            var length = this.SubtractDiffLengthNegativeShift(
                                dividend,
                                remainderLength,
                                divisor,
                                lengthDiff,
                                shift,
                                remainder);
                            if (length < divisor.Length)
                            {
                                if (length < remainder.Length)
                                {
                                    var remRes = new ulong[length];
                                    Array.Copy(remainder, remRes, length);
                                    return Tuple.Create<ulong[], ulong[]>(quo, remRes);
                                }
                                else
                                {
                                    return Tuple.Create<ulong[], ulong[]>(quo, remainder);
                                }
                            }
                            else if (length == divisorLength)
                            {
                                return this.ProcessSameLengthValues(
                                    remainder,
                                    remainderLength,
                                    divisor,
                                    quo,
                                    true);
                            }
                            else
                            {
                                return this.ProcessDiffLengthValues(
                                    remainder,
                                    length,
                                    divisor,
                                    quo,
                                    true);
                            }
                        }
                        else
                        {
                            var remainderLength = foundIndex + 1;
                            var remainder = new ulong[Math.Max(divisor.Length, remainderLength)];
                            var length = this.InvSubtractDiffLengthNegShift(
                                    divisor,
                                    lengthDiff,
                                    shift,
                                    dividend,
                                    remainderLength,
                                    remainder);
                            if (length < divisor.Length)
                            {
                                this.DecrementQuo(quo, 0);
                                length = this.GeneralSubtract(
                                    divisor,
                                    divisor.Length,
                                    remainder,
                                    length,
                                    remainder);
                                if (length < remainder.Length)
                                {
                                    var remRes = new ulong[length];
                                    Array.Copy(remainder, remRes, length);
                                    return Tuple.Create<ulong[], ulong[]>(quo, remRes);
                                }
                                else
                                {
                                    return Tuple.Create<ulong[], ulong[]>(quo, remainder);
                                }
                            }
                            else if (length == divisorLength)
                            {
                                return this.ProcessSameLengthValues(
                                    remainder,
                                    remainderLength,
                                    divisor,
                                    quo,
                                    false);
                            }
                            else
                            {
                                return this.ProcessDiffLengthValues(
                                    remainder,
                                    length,
                                    divisor,
                                    quo,
                                    false);
                            }
                        }
                    }
                    else if (dividendHigh == divisorHigh)
                    {
                        // Estabelece a primeira aproximação para o quociente
                        var quo = new ulong[lengthDiff + 1];
                        quo[lengthDiff] = 1UL;

                        // Não há deslocamentos
                        var foundIndex = default(int);
                        var comparision = this.FindOtherLengthDifference(
                            dividend,
                            dividendLength,
                            divisor,
                            divisorLength,
                            out foundIndex);
                        if (comparision == 0)
                        {
                            // Conta certa
                            return Tuple.Create<ulong[], ulong[]>(quo, null);
                        }
                        else if (comparision == 1)
                        {
                            var remainderLength = foundIndex + 1;
                            var remainder = new ulong[remainderLength];
                            Array.Copy(dividend, remainder, lengthDiff);

                            var length = this.SubtractDiffLength(
                                dividend,
                                remainderLength,
                                divisor,
                                lengthDiff,
                                remainder);
                            if (length < divisor.Length)
                            {
                                if (length < remainder.Length)
                                {
                                    var remRes = new ulong[length];
                                    Array.Copy(remainder, remRes, length);
                                    return Tuple.Create<ulong[], ulong[]>(quo, remRes);
                                }
                                else
                                {
                                    return Tuple.Create<ulong[], ulong[]>(quo, remainder);
                                }
                            }
                            else if (length == divisorLength)
                            {
                                return this.ProcessSameLengthValues(
                                    remainder,
                                    remainderLength,
                                    divisor,
                                    quo,
                                    true);
                            }
                            else
                            {
                                return this.ProcessDiffLengthValues(
                                    remainder,
                                    length,
                                    divisor,
                                    quo,
                                    true);
                            }
                        }
                        else
                        {
                            var remainderLength = foundIndex + 1;
                            var remainder = new ulong[Math.Max(divisor.Length, remainderLength)];
                            var length = this.InvSubtractDiffLength(
                                    divisor,
                                    lengthDiff,
                                    dividend,
                                    remainderLength,
                                    remainder);
                            if (length < divisor.Length)
                            {
                                this.DecrementQuo(quo, 0); // Sinal negativo
                                length = this.GeneralSubtract(
                                    divisor,
                                    divisor.Length,
                                    remainder,
                                    length,
                                    remainder);
                                if (length < remainder.Length)
                                {
                                    var remRes = new ulong[length];
                                    Array.Copy(remainder, remRes, length);
                                    return Tuple.Create<ulong[], ulong[]>(quo, remRes);
                                }
                                else
                                {
                                    return Tuple.Create<ulong[], ulong[]>(quo, remainder);
                                }
                            }
                            else if (length == divisorLength)
                            {
                                return this.ProcessSameLengthValues(
                                    remainder,
                                    remainderLength,
                                    divisor,
                                    quo,
                                    false);
                            }
                            else
                            {
                                return this.ProcessDiffLengthValues(
                                    remainder,
                                    length,
                                    divisor,
                                    quo,
                                    false);
                            }
                        }
                    }
                    else
                    {
                        // Estabelece a primeira aproximação para o quociente
                        var quo = new ulong[lengthDiff + 1];
                        var shift = dividendHigh - divisorHigh;
                        quo[lengthDiff] = 1UL << shift;

                        // Deslocamentos negativos
                        var foundIndex = default(int);
                        var comparision = this.FindOtherLengthDifference(
                            dividend,
                            dividendLength,
                            divisor,
                            divisorLength,
                            shift,
                            out foundIndex);
                        if (comparision == 0)
                        {
                            // Conta certa
                            return Tuple.Create<ulong[], ulong[]>(quo, null);
                        }
                        else if (comparision == 1)
                        {
                            var remainderLength = foundIndex + 1;
                            var remainder = new ulong[remainderLength];
                            Array.Copy(dividend, remainder, lengthDiff);
                            var length = this.SubtractDiffLength(
                                dividend,
                                remainderLength,
                                divisor,
                                lengthDiff,
                                shift,
                                remainder);
                            if (length < divisor.Length)
                            {
                                if (length < remainder.Length)
                                {
                                    var remRes = new ulong[length];
                                    Array.Copy(remainder, remRes, length);
                                    return Tuple.Create<ulong[], ulong[]>(quo, remRes);
                                }
                                else
                                {
                                    return Tuple.Create<ulong[], ulong[]>(quo, remainder);
                                }
                            }
                            else if (length == divisorLength)
                            {
                                return this.ProcessSameLengthValues(
                                    remainder,
                                    remainderLength,
                                    divisor,
                                    quo,
                                    true);
                            }
                            else
                            {
                                return this.ProcessDiffLengthValues(
                                    remainder,
                                    length,
                                    divisor,
                                    quo,
                                    true);
                            }
                        }
                        else
                        {
                            var remainderLength = foundIndex + 1;
                            var remainder = new ulong[Math.Max(divisor.Length, remainderLength)];
                            var length = this.InvSubtractDiffLength(
                                    divisor,
                                    lengthDiff,
                                    shift,
                                    dividend,
                                    remainderLength,
                                    remainder);
                            if (length < divisor.Length)
                            {
                                this.DecrementQuo(quo, 0);
                                length = this.GeneralSubtract(
                                    divisor,
                                    divisor.Length,
                                    remainder,
                                    length,
                                    remainder);
                                if (length < remainder.Length)
                                {
                                    var remRes = new ulong[length];
                                    Array.Copy(remainder, remRes, length);
                                    return Tuple.Create<ulong[], ulong[]>(quo, remRes);
                                }
                                else
                                {
                                    return Tuple.Create<ulong[], ulong[]>(quo, remainder);
                                }
                            }
                            else if (length == divisorLength)
                            {
                                return this.ProcessSameLengthValues(
                                    remainder,
                                    remainderLength,
                                    divisor,
                                    quo,
                                    false);
                            }
                            else
                            {
                                return this.ProcessDiffLengthValues(
                                    remainder,
                                    length,
                                    divisor,
                                    quo,
                                    false);
                            }
                        }
                    }
                }
            }
        }

        #region Funções internas

        /// <summary>
        /// Função interna para efeitos de teste.
        /// </summary>
        /// <param name="first">O primeiro vector.</param>
        /// <param name="second">O segundo vector.</param>
        /// <param name="index">O índice.</param>
        /// <param name="length">O número de entradas válidas em cada um dos vectores.</param>
        /// <returns>
        /// Os valores 1 caso o primeiro vector seja superior, 0 caso ambos os vectores sejam iguais
        /// e -1 caso o primeiro vector seja inferior ao segundo.
        /// </returns>
        internal int InternalFindSameLengthDifference(
            ulong[] first,
            ulong[] second,
            int length,
            out int index)
        {
            return this.FindSameLengthDifference(
                first,
                second,
                length,
                out index);
        }

        /// <summary>
        /// Função interna para efeitos de teste.
        /// </summary>
        /// <param name="first">O primeiro vector.</param>
        /// <param name="second">O segundo vector.</param>
        /// <param name="length">O número de entradas válidas em cada vector.</param>
        /// <param name="smallDivisorShift">O deslocamento em bits do segundo vector.</param>
        /// <param name="index">O índice onde foi encontrada a primeira diferença.</param>
        /// <returns>
        /// Os valores 1 caso o primeiro vector seja superior, 0 caso ambos os vectores sejam iguais
        /// e -1 caso o primeiro vector seja inferior ao segundo vector.
        /// </returns>
        internal int InternalFindSameLengthDifference(
            ulong[] first,
            ulong[] second,
            int length,
            int smallDivisorShift,
            out int index)
        {
            return this.FindSameLengthDifference(
                first,
                second,
                length,
                smallDivisorShift,
                out index);
        }

        /// <summary>
        /// Função interna para efeitos de testes.
        /// </summary>
        /// <param name="first">O primeiro vector a ser comparado.</param>
        /// <param name="firstLength">O número de entradas válidas do primeiro vector.</param>
        /// <param name="second">O segundo vector a ser comparado.</param>
        /// <param name="secondLength">O número de entradas válidas no segundo vector.</param>
        /// <param name="index">O índice onde é encontrada a primeira diferença.</param>
        /// <returns>
        /// Os valores 1 caso o primeiro vector seja superior, 0 caso ambos os vectores sejam iguais
        /// e -1 caso o primeiro vector seja inferior ao segundo vector.
        /// </returns>
        internal int InternalFindOtherLengthDifference(
            ulong[] first,
            int firstLength,
            ulong[] second,
            int secondLength,
            out int index)
        {
            return this.FindOtherLengthDifference(
                first,
                firstLength,
                second,
                secondLength,
                out index);
        }

        /// <summary>
        /// Função interna para efeitos de teste.
        /// </summary>
        /// <param name="first">O primeiro vector.</param>
        /// <param name="firstLength">O número de entradas válidas no primeiro vector.</param>
        /// <param name="second">O segundo vector.</param>
        /// <param name="secondLength">O número de entradas válidas no segundo vector.</param>
        /// <param name="smallShift">O deslocamento em bits.</param>
        /// <param name="index">O índice onde foi encontrada a primeira diferença.</param>
        /// <returns>
        /// Os valores 1 caso o primeiro vector seja superior, 0 caso ambos os vectores sejam iguais
        /// e -1 caso o primeiro vector seja inferior ao segundo.
        /// </returns>
        internal int InternalFindOtherLengthDifference(
            ulong[] first,
            int firstLength,
            ulong[] second,
            int secondLength,
            int smallShift,
            out int index)
        {
            return this.FindOtherLengthDifference(
                first,
                firstLength,
                second,
                secondLength,
                smallShift,
                out index);
        }

        /// <summary>
        /// Função interna para efeitos de teste.
        /// </summary>
        /// <param name="first">O primeiro vector.</param>
        /// <param name="firstLength">o número de entradas válidas no primeiro vector.</param>
        /// <param name="second">O segundo vector.</param>
        /// <param name="secondLength">O número de entradas válidas no segundo vector.</param>
        /// <param name="divShiftMagnitude">O deslocamento negativo em bits.</param>
        /// <param name="index">O índice onde foi encontrada a primeira diferença.</param>
        /// <returns>
        /// Os valores 1 caso o primeiro vector seja superior, 0 caso ambos os vectores sejam iguais
        /// e -1 caso o primeiro vector seja inferior ao segundo vector.
        /// </returns>
        internal int InternalFindOtherLengthDiffNegativeShift(
            ulong[] first,
            int firstLength,
            ulong[] second,
            int secondLength,
            int divShiftMagnitude,
            out int index)
        {
            return this.FindOtherLengthDiffNegativeShift(
                first,
                firstLength,
                second,
                secondLength,
                divShiftMagnitude,
                out index);
        }

        /// <summary>
        ///  Função interna para efeito de testes.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="length">O comprimento dos vectores.</param>
        /// <param name="outDifference">O vector que contém o resultado.</param>
        /// <returns>O tamanho do resultado no vector proporcionado.</returns>
        internal int InternalSubtractSameLength(
            ulong[] minuend,
            ulong[] subtrahend,
            int length,
            ulong[] outDifference)
        {
            return this.SubtractSameLength(
                minuend,
                subtrahend,
                length,
                outDifference);
        }

        /// <summary>
        ///  Função interna para efeito de testes.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="length">O número de itens válidos em cada vector.</param>
        /// <param name="subtrahendShift">O deslocamento aplicado ao subtraendo.</param>
        /// <param name="outDifference">O vector que contém o resultado.</param>
        /// <returns>O tamanho do resultado no vector proporcionado.</returns>
        internal int InternalSubtractSameLength(
            ulong[] minuend,
            ulong[] subtrahend,
            int length,
            int subtrahendShift,
            ulong[] outDifference)
        {
            return this.SubtractSameLength(
                minuend,
                subtrahend,
                length,
                subtrahendShift,
                outDifference);
        }

        /// <summary>
        ///  Função interna para efeito de testes.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="minuendShift">O deslocamento aplicado ao minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="length">O número de itens válidos em cada vector.</param>
        /// <param name="outDifference">O vector que contém o resultado.</param>
        /// <returns>O tamanho do resultado no vector proporcionado.</returns>
        internal int InternalInvSubtractSameLength(
            ulong[] minuend,
            int minuendShift,
            ulong[] subtrahend,
            int length,
            ulong[] outDifference)
        {
            return this.InvSubtractSameLength(
                minuend,
                minuendShift,
                subtrahend,
                length,
                outDifference);
        }

        /// <summary>
        /// Função interna para efeito de testes.
        /// </summary>
        /// <param name="minuend">O dividendo.</param>
        /// <param name="minuendLength">O número de itens do dividendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="subtrahendOffset">O deslocamento do divisor.</param>
        /// <param name="outDifference">O vector que contém o resultado.</param>
        /// <returns>O tamanho do resultado no vector proporcionado.</returns>
        internal int InternalSubtractDiffLength(
            ulong[] minuend,
            int minuendLength,
            ulong[] subtrahend,
            int subtrahendOffset,
            ulong[] outDifference)
        {
            return this.SubtractDiffLength(
                minuend,
                minuendLength,
                subtrahend,
                subtrahendOffset,
                outDifference);
        }

        /// <summary>
        /// Função interna para efeito de testes.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="minuendLength">O comprimento do minuendo.</param>
        /// <param name="subtraend">O subtraendo.</param>
        /// <param name="subtraendOffset">O deslocamento do subtraendo.</param>
        /// <param name="subtraendNegativeShift">A rotação do subtraendo.</param>
        /// <param name="outDifference">O vector que contém a diferença.</param>
        /// <returns>O tamanho do vector que contém resultados válidos.</returns>
        internal int InternalSubtractDiffLengthNegativeShift(
            ulong[] minuend,
            int minuendLength,
            ulong[] subtraend,
            int subtraendOffset,
            int subtraendNegativeShift,
            ulong[] outDifference)
        {
            return this.SubtractDiffLengthNegativeShift(
                minuend,
                minuendLength,
                subtraend,
                subtraendOffset,
                subtraendNegativeShift,
                outDifference);
        }

        /// <summary>
        /// Função interna para efeito de testes.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="minuendOffset">O deslocamento do minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="subtrahendLength">O tamanho do subtraendo.</param>
        /// <param name="outDifference">O vector que contém o resultado.</param>
        /// <returns>O tamanho do resultado no vector proporcionado.</returns>
        internal int InternalInvSubtractDiffLength(
            ulong[] minuend,
            int minuendOffset,
            ulong[] subtrahend,
            int subtrahendLength,
            ulong[] outDifference)
        {
            return this.InvSubtractDiffLength(
                minuend,
                minuendOffset,
                subtrahend,
                subtrahendLength,
                outDifference);
        }

        /// <summary>
        /// Função interna para efeito de testes.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="minuendLength">O tamanho do minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="subtrahendOffset">O deslocamento do subtraendo.</param>
        /// <param name="subtrahendShift">A rotação do subtraendo.</param>
        /// <param name="outDifference">O vector que contém o resultado.</param>
        /// <returns>O tamanho do resultado no vector proporcionado.</returns>
        internal int InternalSubtractDiffLength(
            ulong[] minuend,
            int minuendLength,
            ulong[] subtrahend,
            int subtrahendOffset,
            int subtrahendShift,
            ulong[] outDifference)
        {
            return this.SubtractDiffLength(
                minuend,
                minuendLength,
                subtrahend,
                subtrahendOffset,
                subtrahendShift,
                outDifference);
        }

        /// <summary>
        /// Função interna para efeito de testes.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="minuendOffset">O deslocamento do minuendo.</param>
        /// <param name="minuendShift">A rotação do minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="subtrahendLength">O tamanho do subtraendo.</param>
        /// <param name="outDifference">O vector que contém o resultado.</param>
        /// <returns>O tamanho do resultado no vector proporcionado.</returns>
        internal int InternalInvSubtractDiffLength(
            ulong[] minuend,
            int minuendOffset,
            int minuendShift,
            ulong[] subtrahend,
            int subtrahendLength,
            ulong[] outDifference)
        {
            return this.InvSubtractDiffLength(
                minuend,
                minuendOffset,
                minuendShift,
                subtrahend,
                subtrahendLength,
                outDifference);
        }

        /// <summary>
        /// Função interna para efeitos de testes.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="minuendOffset">O deslocamento vectorial do minuendo.</param>
        /// <param name="minuendShift">O deslocamento negativo menor do minuendo.</param>
        /// <param name="subtrahend">O sbutraendo.</param>
        /// <param name="subtrahendLength">O comprimento válido do subtraendo.</param>
        /// <param name="outDifference">O vector que irá conter a diferença.</param>
        /// <returns>O tamanho da diferença.</returns>
        internal int InternalInvSubtractDiffLengthNegShift(
            ulong[] minuend,
            int minuendOffset,
            int minuendShift,
            ulong[] subtrahend,
            int subtrahendLength,
            ulong[] outDifference)
        {
            return this.InvSubtractDiffLengthNegShift(
                minuend,
                minuendOffset,
                minuendShift,
                subtrahend,
                subtrahendLength,
                outDifference);
        }

        /// <summary>
        /// Função interna para efeitos de teste.
        /// </summary>
        /// <param name="quotient">O valor que será incrementado.</param>
        /// <param name="value">O valor a incrementar.</param>
        /// <param name="startIndex">O índice associado ao incremento.</param>
        internal void InternalDecrementQuo(
            ulong[] quotient,
            ulong value,
            int startIndex)
        {
            this.DecrementQuo(
                quotient,
                value,
                startIndex);
        }

        /// <summary>
        /// Função interna para efeitos de teste.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="minuendLength">O número de entradas válidas no minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="subtrahendLength">O número de entradas válidas no subtraendo.</param>
        /// <param name="outDifference">O vector que irá conter a diferença.</param>
        /// <returns>O número de entradas válidas do vector que contém a diferença.</returns>
        internal int InternalGeneralSubtract(
            ulong[] minuend,
            int minuendLength,
            ulong[] subtrahend,
            int subtrahendLength,
            ulong[] outDifference)
        {
            return this.GeneralSubtract(
                minuend,
                minuendLength,
                subtrahend,
                subtrahendLength,
                outDifference);
        }

        #endregion Funções internas

        #region Funções privadas

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
        /// <returns>O quociente.</returns>
        private Tuple<ulong[], ulong[]> ProcessSameLengthValues(
            ulong[] remainder,
            int length,
            ulong[] divisor,
            ulong[] currengQuotientValue,
            bool sign)
        {
            var innerSign = sign;
            var dividendHigh = MathFunctions.GetHighestSettedBitIndex(
                        remainder[length - 1]);
            var divisorHigh = MathFunctions.GetHighestSettedBitIndex(
                divisor[length - 1]);

            while (dividendHigh >= divisorHigh)
            {
                if (dividendHigh == divisorHigh)
                {
                    var firstDiff = default(int);
                    var comparision = this.FindSameLengthDifference(remainder, divisor, length, out firstDiff);
                    if (comparision == 0)
                    {
                        // Ambos os vectores são iguais
                        if (innerSign)
                        {
                            // Sinal positivo
                            this.IncrementQuo(
                                currengQuotientValue,
                                0);
                        }
                        else
                        {
                            // Sinal negativo
                            this.DecrementQuo(
                                currengQuotientValue,
                                0);
                        }

                        return Tuple.Create<ulong[], ulong[]>(
                                    this.GetTrimmedVector(currengQuotientValue),
                                    null);
                    }
                    else if (comparision == 1)
                    {
                        // É retornada a diferença
                        var remainderLength = firstDiff + 1;
                        var innerLength = this.SubtractSameLength(
                            remainder,
                            divisor,
                            remainderLength,
                            remainder);
                        if (remainder.Length == innerLength)
                        {
                            if (innerSign)
                            {
                                this.IncrementQuo(
                                    currengQuotientValue,
                                    0);
                                return Tuple.Create(
                                    this.GetTrimmedVector(currengQuotientValue),
                                    remainder);
                            }
                            else
                            {
                                // Nenhuma actualização é aplicada ao quociente
                                var resultLength = this.GeneralSubtract(
                                    divisor,
                                    divisor.Length,
                                    remainder,
                                    innerLength,
                                    remainder);
                                if (resultLength == remainder.Length)
                                {
                                    return Tuple.Create(
                                        this.GetTrimmedVector(currengQuotientValue),
                                        remainder);
                                }
                                else
                                {
                                    return Tuple.Create(
                                        this.GetTrimmedVector(currengQuotientValue),
                                        this.GetTrimmedVector(remainder, resultLength));
                                }
                            }
                        }
                        else
                        {
                            if (innerSign)
                            {
                                this.IncrementQuo(
                                    currengQuotientValue,
                                    0);
                                return Tuple.Create(
                                    this.GetTrimmedVector(currengQuotientValue),
                                    this.GetTrimmedVector(remainder, innerLength));
                            }
                            else
                            {
                                var resultLength = this.GeneralSubtract(
                                       divisor,
                                       divisor.Length,
                                       remainder,
                                       innerLength,
                                       remainder);
                                if (resultLength == remainder.Length)
                                {
                                    return Tuple.Create(
                                        this.GetTrimmedVector(currengQuotientValue),
                                        remainder);
                                }
                                else
                                {
                                    return Tuple.Create(
                                        this.GetTrimmedVector(currengQuotientValue),
                                        this.GetTrimmedVector(remainder, resultLength));
                                }
                            }
                        }
                    }
                    else
                    {
                        // O divisor é superior ao resto
                        if (innerSign)
                        {
                            return Tuple.Create(
                                this.GetTrimmedVector(currengQuotientValue),
                                remainder);
                        }
                        else
                        {
                            this.DecrementQuo(
                                currengQuotientValue,
                                0);
                            var resultLength = this.GeneralSubtract(
                                   divisor,
                                   divisor.Length,
                                   remainder,
                                   length,
                                   remainder);
                            if (resultLength == remainder.Length)
                            {
                                return Tuple.Create(
                                    this.GetTrimmedVector(currengQuotientValue),
                                    remainder);
                            }
                            else
                            {
                                return Tuple.Create(
                                    this.GetTrimmedVector(currengQuotientValue),
                                    this.GetTrimmedVector(remainder, resultLength));
                            }
                        }
                    }
                }
                else
                {
                    var shift = dividendHigh - divisorHigh;
                    var firstDiff = default(int);
                    var comparision = this.FindSameLengthDifference(
                        remainder,
                        divisor,
                        length,
                        shift,
                        out firstDiff);
                    if (comparision == 0)
                    {
                        if (innerSign)
                        {
                            // Sinal positivo
                            this.IncrementQuo(
                                currengQuotientValue,
                                1UL << shift,
                                0);
                        }
                        else
                        {
                            // Sinal negativo
                            this.DecrementQuo(
                                currengQuotientValue,
                                1UL << shift,
                                0);
                        }

                        return Tuple.Create<ulong[], ulong[]>(
                                    this.GetTrimmedVector(currengQuotientValue),
                                    null);
                    }
                    else if (comparision == 1)
                    {
                        var remainderLength = firstDiff + 1;
                        var innerLength = this.SubtractSameLength(
                            remainder,
                            divisor,
                            remainderLength,
                            shift,
                            remainder);
                        if (innerSign)
                        {
                            this.IncrementQuo(
                                currengQuotientValue,
                                1UL << shift,
                                0);

                            // Actualiza os dados para o próximo ciclo
                            if (innerLength < length)
                            {
                                return Tuple.Create(
                                    this.GetTrimmedVector(currengQuotientValue),
                                    this.GetTrimmedVector(remainder, innerLength));
                            }
                            else
                            {
                                dividendHigh = MathFunctions.GetHighestSettedBitIndex(
                                            remainder[length - 1]);
                                divisorHigh = MathFunctions.GetHighestSettedBitIndex(
                                    divisor[length - 1]);
                            }
                        }
                        else
                        {
                            this.DecrementQuo(
                                currengQuotientValue,
                                1UL << shift,
                                0);

                            // Actualiza os dados para o próximo ciclo
                            if (innerLength < length)
                            {
                                var resultLength = this.GeneralSubtract(
                                   divisor,
                                   divisor.Length,
                                   remainder,
                                   length,
                                   remainder);
                                if (resultLength == remainder.Length)
                                {
                                    return Tuple.Create(
                                        this.GetTrimmedVector(currengQuotientValue),
                                        remainder);
                                }
                                else
                                {
                                    return Tuple.Create(
                                        this.GetTrimmedVector(currengQuotientValue),
                                        this.GetTrimmedVector(remainder, resultLength));
                                }
                            }
                            else
                            {
                                dividendHigh = MathFunctions.GetHighestSettedBitIndex(
                                            remainder[length - 1]);
                            }
                        }
                    }
                    else
                    {
                        var remainderLength = firstDiff + 1;
                        var innerLength = this.InvSubtractSameLength(
                            divisor,
                            shift,
                            remainder,
                            remainderLength,
                            remainder);
                        if (innerSign)
                        {
                            this.IncrementQuo(
                                   currengQuotientValue,
                                   1UL << shift,
                                   0);

                            if (innerLength < length)
                            {
                                var resultLength = this.GeneralSubtract(
                                   divisor,
                                   divisor.Length,
                                   remainder,
                                   length,
                                   remainder);
                                if (resultLength == remainder.Length)
                                {
                                    return Tuple.Create(
                                        this.GetTrimmedVector(currengQuotientValue),
                                        remainder);
                                }
                                else
                                {
                                    return Tuple.Create(
                                        this.GetTrimmedVector(currengQuotientValue),
                                        this.GetTrimmedVector(remainder, resultLength));
                                }
                            }
                            else
                            {
                                dividendHigh = MathFunctions.GetHighestSettedBitIndex(
                                            remainder[length - 1]);
                            }
                        }
                        else
                        {
                            this.DecrementQuo(
                                   currengQuotientValue,
                                   1UL << shift,
                                   0);
                            if (innerLength < length)
                            {
                                return Tuple.Create(
                                    this.GetTrimmedVector(currengQuotientValue),
                                    this.GetTrimmedVector(remainder, innerLength));
                            }
                            else
                            {
                                dividendHigh = MathFunctions.GetHighestSettedBitIndex(
                                            remainder[length - 1]);
                            }
                        }

                        innerSign = !innerSign;
                    }
                }
            }

            if (innerSign)
            {
                if (remainder.Length == length)
                {
                    return Tuple.Create(
                        this.GetTrimmedVector(currengQuotientValue),
                        remainder);
                }
                else
                {
                    return Tuple.Create(
                        this.GetTrimmedVector(currengQuotientValue),
                        this.GetTrimmedVector(remainder, length));
                }
            }
            else
            {
                this.DecrementQuo(currengQuotientValue, 0);
                var resultLength = this.GeneralSubtract(
                                   divisor,
                                   divisor.Length,
                                   remainder,
                                   length,
                                   remainder);
                if (remainder.Length == length)
                {
                    return Tuple.Create(
                    this.GetTrimmedVector(currengQuotientValue),
                    remainder);
                }
                else
                {
                    return Tuple.Create(
                    this.GetTrimmedVector(currengQuotientValue),
                    this.GetTrimmedVector(remainder, resultLength));
                }
            }
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
        /// <returns>O quociente.</returns>
        private Tuple<ulong[], ulong[]> ProcessDiffLengthValues(
            ulong[] remainder,
            int length,
            ulong[] divisor,
            ulong[] currengQuotientValue,
            bool sign)
        {
            var innerSign = sign;
            var innerLength = length;
            var divisorLength = divisor.Length;
            var divisorHigh = MathFunctions.GetHighestSettedBitIndex(
                divisor[divisorLength - 1]);

            while (innerLength > divisorLength)
            {
                var displacement = innerLength - divisorLength;
                var dividendHigh = MathFunctions.GetHighestSettedBitIndex(
                        remainder[innerLength - 1]);
                if (divisorHigh < dividendHigh)
                {
                    var shift = dividendHigh - divisorHigh;
                    var firstDiff = default(int);
                    var comparision = this.FindOtherLengthDifference(
                        remainder,
                        innerLength,
                        divisor,
                        divisorLength,
                        shift,
                        out firstDiff);
                    if (comparision == 0)
                    {
                        // Ambos os vectores são iguais - o algoritmo termina
                        if (innerSign)
                        {
                            // Sinal positivo
                            this.IncrementQuo(
                                currengQuotientValue,
                                1UL << shift,
                                displacement);
                        }
                        else
                        {
                            // Sinal negativo
                            this.DecrementQuo(
                                currengQuotientValue,
                                1UL << shift,
                                displacement);
                        }

                        // O sinal do resto é irrelevante
                        return Tuple.Create<ulong[], ulong[]>(
                                        this.GetTrimmedVector(currengQuotientValue),
                                        null);
                    }
                    else if (comparision == 1)
                    {
                        var remainderLength = firstDiff + 1;
                        innerLength = this.SubtractDiffLength(
                            remainder,
                            remainderLength,
                            divisor,
                            displacement,
                            shift,
                            remainder);
                        if (innerSign)
                        {
                            // Sinal positivo
                            this.IncrementQuo(
                                currengQuotientValue,
                                1UL << shift,
                                displacement);
                        }
                        else
                        {
                            // Sinal negativo
                            this.DecrementQuo(
                                currengQuotientValue,
                                1UL << shift,
                                displacement);
                        }
                    }
                    else
                    {
                        var remainderLength = firstDiff + 1;
                        innerLength = InvSubtractDiffLength(
                            divisor,
                            displacement,
                            shift,
                            remainder,
                            remainderLength,
                            remainder);
                        if (innerSign)
                        {
                            // Sinal positivo
                            this.IncrementQuo(
                                currengQuotientValue,
                                1UL << shift,
                                displacement);
                        }
                        else
                        {
                            // Sinal negativo
                            this.DecrementQuo(
                                currengQuotientValue,
                                1UL << shift,
                                displacement);
                        }

                        innerSign = !innerSign;
                    }
                }
                else if (divisorHigh == dividendHigh)
                {
                    var firstDiff = default(int);
                    var comparision = this.FindOtherLengthDifference(
                        remainder,
                        innerLength,
                        divisor,
                        divisorLength,
                        out firstDiff);
                    if (comparision == 0)
                    {
                        // Ambos os vectores são iguais - o algoritmo termina
                        if (innerSign)
                        {
                            // Sinal positivo
                            this.IncrementQuo(
                                currengQuotientValue,
                                displacement);
                        }
                        else
                        {
                            // Sinal negativo
                            this.DecrementQuo(
                                currengQuotientValue,
                                displacement);
                        }

                        // O sinal do resto é irrelevante
                        return Tuple.Create<ulong[], ulong[]>(
                                        this.GetTrimmedVector(currengQuotientValue),
                                        null);
                    }
                    else if (comparision == 1)
                    {
                        var remainderLength = firstDiff + 1;
                        innerLength = this.SubtractDiffLength(
                            remainder,
                            remainderLength,
                            divisor,
                            displacement,
                            remainder);
                        if (innerSign)
                        {
                            // Sinal positivo
                            this.IncrementQuo(
                                currengQuotientValue,
                                displacement);
                        }
                        else
                        {
                            // Sinal negativo
                            this.DecrementQuo(
                                currengQuotientValue,
                                displacement);
                        }
                    }
                    else
                    {
                        var remainderLength = firstDiff + 1;
                        innerLength = InvSubtractDiffLength(
                            divisor,
                            displacement,
                            remainder,
                            remainderLength,
                            remainder);
                        if (innerSign)
                        {
                            // Sinal positivo
                            this.IncrementQuo(
                                currengQuotientValue,
                                displacement);
                        }
                        else
                        {
                            // Sinal negativo
                            this.DecrementQuo(
                                currengQuotientValue,
                                displacement);
                        }

                        innerSign = !innerSign;
                    }
                }
                else
                {
                    var negShift = divisorHigh - dividendHigh;
                    var firstDiff = default(int);
                    var comparision = this.FindOtherLengthDiffNegativeShift(
                        remainder,
                        innerLength,
                        divisor,
                        divisorLength,
                        negShift,
                        out firstDiff);
                    if (comparision == 0)
                    {
                        // Ambos os vectores são iguais - o algoritmo termina
                        if (innerSign)
                        {
                            // Sinal positivo
                            this.IncrementQuo(
                                currengQuotientValue,
                                1UL << (64 - negShift),
                                displacement - 1);
                        }
                        else
                        {
                            // Sinal negativo
                            this.DecrementQuo(
                                currengQuotientValue,
                                1UL << (64 - negShift),
                                displacement - 1);
                        }

                        // O sinal do resto é irrelevante
                        return Tuple.Create<ulong[], ulong[]>(
                                        this.GetTrimmedVector(currengQuotientValue),
                                        null);
                    }
                    else if (comparision == 1)
                    {
                        var remainderLength = firstDiff + 1;
                        innerLength = this.SubtractDiffLengthNegativeShift(
                            remainder,
                            remainderLength,
                            divisor,
                            displacement,
                            negShift,
                            remainder);
                        if (innerSign)
                        {
                            // Sinal positivo
                            this.IncrementQuo(
                                currengQuotientValue,
                                1UL << (64 - negShift),
                                displacement - 1);
                        }
                        else
                        {
                            // Sinal negativo
                            this.DecrementQuo(
                                currengQuotientValue,
                                1UL << (64 - negShift),
                                displacement - 1);
                        }
                    }
                    else
                    {
                        var remainderLength = firstDiff + 1;
                        innerLength = InvSubtractDiffLengthNegShift(
                            divisor,
                            displacement,
                            negShift,
                            remainder,
                            remainderLength,
                            remainder);
                        if (innerSign)
                        {
                            // Sinal positivo
                            this.IncrementQuo(
                                currengQuotientValue,
                                1UL << (64 - negShift),
                                displacement - 1);
                        }
                        else
                        {
                            // Sinal negativo
                            this.DecrementQuo(
                                currengQuotientValue,
                                1UL << (64 - negShift),
                                displacement - 1);
                        }

                        innerSign = !innerSign;
                    }
                }
            }

            // Processa os valores no caso dos vectores serem do mesmo tamanho
            return this.ProcessSameLengthValues(
                remainder,
                innerLength,
                divisor,
                currengQuotientValue,
                innerSign);
        }

        /// <summary>
        /// Determina o índice da diferença entre os vectores proporcionados,
        /// sendo estes do mesmo tamanho.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>O índice da primeira diferença.</returns>
        [Obsolete("Alterada", true)]
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
        /// Determina qual de dois vectores representa um número maior,
        /// indicando o índice onde foi encontrada a primeira diferença.
        /// </summary>
        /// <param name="first">O primeiro vector.</param>
        /// <param name="second">O segundo vector.</param>
        /// <param name="index">A variável que irá conter o valor do índice.</param>
        /// <returns>
        /// Os valores 1 caso o primeiro vector seja superior, 0 caso sejam iguais 
        /// e -1 caso o primeiro vector seja inferior ao segundo.
        /// </returns>
        [Obsolete("Alterada", true)]
        private int FindSameLengthDifference(
            ulong[] first,
            ulong[] second,
            out int index)
        {
            var result = 0;
            index = -1;
            for (int i = first.Length - 1; i > -1; --i)
            {
                if (first[i] < second[i])
                {
                    result = -1;
                    index = i;
                }
                else if (second[i] < first[i])
                {
                    result = 1;
                    index = i;
                }
            }

            return result;
        }

        /// <summary>
        /// Determina qual de dois vectores representa um número maior,
        /// indicando o índice onde foi encontrada a primeira diferença.
        /// </summary>
        /// <param name="first">O primeiro vector.</param>
        /// <param name="second">O segundo vector.</param>
        /// <param name="length">O número de entradas válidas em ambos os vectores.</param>
        /// <param name="index">A variável que irá conter o valor do índice.</param>
        /// <returns>
        /// Os valores 1 caso o primeiro vector seja superior, 0 caso sejam iguais 
        /// e -1 caso o primeiro vector seja inferior ao segundo.
        /// </returns>
        private int FindSameLengthDifference(
            ulong[] first,
            ulong[] second,
            int length,
            out int index)
        {
            index = -1;
            for (int i = length - 1; i > -1; --i)
            {
                if (first[i] < second[i])
                {
                    index = i;
                    return -1;
                }
                else if (second[i] < first[i])
                {
                    index = i;
                    return 1;
                }
            }

            return 0;
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
        [Obsolete("Alterada", true)]
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
        /// Determina qual de dois vectores representa um número maior, tendo em conta
        /// que o segundo vector é deslocado para a esquerda um número de bits inferior 
        /// ao tamanho da variável. É também determinado o índice, relativo ao primeiro
        /// vector onde foi encontrada a primeira diferença.
        /// </summary>
        /// <param name="first">O primeiro vector.</param>
        /// <param name="second">O segundo vector.</param>
        /// <param name="length">O comprimento dos vectores.</param>
        /// <param name="smallShift">O deslocamento a aplicar ao nívels dos bits.</param>
        /// <param name="index">O índice onde foi encontrada a primeira diferença.</param>
        /// <returns>
        /// Os valores 1 caso o primeiro vector seja superior, 0 caso sejam iguais
        /// e -1 caso o primeiro vector seja inferior ao segundo.
        /// </returns>
        private int FindSameLengthDifference(
            ulong[] first,
            ulong[] second,
            int length,
            int smallShift,
            out int index)
        {
            index = -1;
            var i = length - 1;
            var shiftedDiv = second[i] << smallShift;
            for (int j = i - 1; j > -1; --j, --i)
            {
                shiftedDiv |= (second[j] >> (64 - smallShift));
                if (shiftedDiv < first[i])
                {
                    index = i;
                    return 1;
                }
                else if (shiftedDiv > first[i])
                {
                    index = i;
                    return -1;
                }

                shiftedDiv = second[j] << smallShift;
            }

            if (shiftedDiv < first[0])
            {
                index = 0;
                return 1;
            }
            else if (shiftedDiv > first[0])
            {
                index = 0;
                return -1;
            }
            else
            {
                index = -1;
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
        [Obsolete("Alterada", true)]
        private int FindOtherLengthDifference(
            ulong[] dividend,
            int dividendLength,
            ulong[] divisor,
            int divisorLength)
        {
            var result = -1;
            var i = dividendLength - 1;
            var j = divisorLength - 1;
            for (; j > -1; --i, --j)
            {
                if (dividend[i] != divisor[j])
                {
                    return i;
                }
            }

            for (; i > -1; --i)
            {
                if (dividend[i] != 0)
                {
                    result = i;
                    i = -1;
                }
            }

            return result;
        }

        /// <summary>
        /// Determina qual dos vectores representa o maior número, tendo em conta
        /// que o segundo vector terá de ser alinhado com o primeiro.
        /// </summary>
        /// <param name="first">O primeiro vector.</param>
        /// <param name="firstLength">O número de entradas válidas no primeiro vector.</param>
        /// <param name="second">O segundo vector.</param>
        /// <param name="secondLength">O número de entradas válidas no segundo vector.</param>
        /// <param name="index">O índice onde foi encontrada a primeira diferença.</param>
        /// <returns>
        /// Os valores 1 caso o primeiro vector seja superior, 0 caso sejam iguais e -1 caso
        /// o primeiro vector seja inferior ao segundo.
        /// </returns>
        private int FindOtherLengthDifference(
            ulong[] first,
            int firstLength,
            ulong[] second,
            int secondLength,
            out int index)
        {
            index = -1;
            var i = firstLength - 1;
            var j = secondLength - 1;
            for (; j > -1; --i, --j)
            {
                if (first[i] < second[j])
                {
                    index = i;
                    return -1;
                }
                else if (first[i] > second[j])
                {
                    index = i;
                    return 1;
                }
            }

            for (; i > -1; --i)
            {
                if (first[i] > 0)
                {
                    index = i;
                    return 1;
                }
            }

            return 0;
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
        [Obsolete("Alterada", true)]
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
        /// Determina qual de dois vectores representa o maior número, tendo em conta que o segundo
        /// vector sofre um deslocamento na posição das entradas e de um determinado número de bits.
        /// </summary>
        /// <param name="first">O primeiro vector.</param>
        /// <param name="firstLength">O número de entradas válidas no primeiro vector.</param>
        /// <param name="second">O segundo vector.</param>
        /// <param name="secondLength">O número de entradas válidas no segundo vector.</param>
        /// <param name="smallDivisorShift">O deslocamento em bits.</param>
        /// <param name="index">O índice onde foi encontrada a primeira diferença.</param>
        /// <returns>
        /// Os valores 1 caso o primeiro vector seja superior, 0 caso ambos os vectores sejam iguais
        /// e -1 caso o primeiro vector seja inferior ao segundo.
        /// </returns>
        private int FindOtherLengthDifference(
            ulong[] first,
            int firstLength,
            ulong[] second,
            int secondLength,
            int smallDivisorShift,
            out int index)
        {
            index = -1;
            var i = firstLength - 1;
            var j = secondLength - 1;
            var shiftDiv = second[j] << smallDivisorShift;
            --j;
            for (; j > -1; --i, --j)
            {
                shiftDiv |= (second[j] >> (64 - smallDivisorShift));
                if (shiftDiv < first[i])
                {
                    index = i;
                    return 1;
                }
                else if (shiftDiv > first[i])
                {
                    index = i;
                    return -1;
                }

                shiftDiv = second[j] << smallDivisorShift;
            }

            if (shiftDiv < first[i])
            {
                index = i;
                return 1;
            }
            else if (shiftDiv > first[i])
            {
                index = i;
                return -1;
            }
            else
            {
                --i;
                for (; i > -1; --i)
                {
                    if (first[i] != 0)
                    {
                        index = i;
                        return 1;
                    }
                }

                index = i;
                return 0;
            }
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
        [Obsolete("Alterada", true)]
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
            if (shiftDiv != dividend[i])
            {
                return i;
            }

            shiftDiv = divisor[j] << (64 - divShiftMagnitude);
            --i;
            --j;
            for (; j > -1; --i, --j)
            {
                shiftDiv |= (divisor[j] >> divShiftMagnitude);
                if (shiftDiv != dividend[i])
                {
                    return i;
                }

                shiftDiv = divisor[j] << (64 - divShiftMagnitude);
            }

            if (shiftDiv == dividend[i])
            {
                --i;
            }

            return i;
        }

        private int FindOtherLengthDiffNegativeShift(
            ulong[] first,
            int firstLength,
            ulong[] second,
            int secondLength,
            int shiftMagnitude,
            out int index)
        {
            index = -1;
            var i = firstLength - 1;
            var j = secondLength - 1;
            var shiftDiv = second[j] >> shiftMagnitude;
            if (shiftDiv < first[i])
            {
                index = i;
                return 1;
            }
            else if (shiftDiv > first[i])
            {
                index = i;
                return -1;
            }

            shiftDiv = second[j] << (64 - shiftMagnitude);
            --i;
            --j;
            for (; j > -1; --i, --j)
            {
                shiftDiv |= (second[j] >> shiftMagnitude);
                if (shiftDiv < first[i])
                {
                    index = i;
                    return 1;
                }
                else if (shiftDiv > first[i])
                {
                    index = i;
                    return -1;
                }

                shiftDiv = second[j] << (64 - shiftMagnitude);
            }

            if (shiftDiv < first[i])
            {
                index = i;
                return 1;
            }
            else if (shiftDiv > first[i])
            {
                index = i;
                return -1;
            }

            return 0;
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
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            var result = length - 1;
            for (; result > -1; --result)
            {
                if (outDifference[result] != 0)
                {
                    return result + 1;
                }
            }

            return result + 1;
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
            for (int i = 1; i < length; ++i)
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
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            var result = length - 1;
            for (; result > -1; --result)
            {
                if (outDifference[result] != 0)
                {
                    return result + 1;
                }
            }

            return result + 1;
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
            var carry = false;
            var currentMinuend = minuend[0];
            var shiftCarry = currentMinuend >> (64 - minuendShift);
            currentMinuend <<= minuendShift;
            var complement = ~subtrahend[0];
            var sum = MathFunctions.Add(currentMinuend, complement);
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
                currentMinuend = minuend[i];
                var tempCarry = currentMinuend >> (64 - minuendShift);
                currentMinuend = (currentMinuend << minuendShift) | shiftCarry;
                shiftCarry = tempCarry;
                complement = ~subtrahend[i];
                sum = MathFunctions.Add(complement, currentMinuend);
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
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            var result = length - 1;
            for (; result > -1; --result)
            {
                if (outDifference[result] != 0)
                {
                    return result + 1;
                }
            }

            return result + 1;
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
                complement = ~subtrahend[j];
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
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            var result = minuendLength - 1;
            for (; result > -1; --result)
            {
                if (outDifference[result] != 0)
                {
                    return result + 1;
                }
            }

            return result + 1;
        }

        /// <summary>
        /// Determina a diferença entre dois números onde o subtraendo se encontra
        /// sujeito a um deslocamento para a esquerda e subsequente rotação para a direita.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="minuendLength">O comprimento do minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="subtrahendOffset">O deslocamento do subtraendo.</param>
        /// <param name="subtraendNegativeShift">A rotação do subtraendo.</param>
        /// <param name="outDifference">O vector que contém a diferença.</param>
        /// <returns>O tamanho do vector que contém resultados válidos.</returns>
        private int SubtractDiffLengthNegativeShift(
            ulong[] minuend,
            int minuendLength,
            ulong[] subtrahend,
            int subtrahendOffset,
            int subtraendNegativeShift,
            ulong[] outDifference)
        {
            var carry = true;
            var i = subtrahendOffset - 1;
            var j = 0;

            var aux = subtrahend[j];
            var complement = ~(aux << (64 - subtraendNegativeShift));
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
            var lastIndex = minuendLength - 1;
            if (i < lastIndex)
            {
                ++i;
                ++j;
                for (; i < lastIndex; ++i, ++j)
                {
                    complement = aux;
                    aux = subtrahend[j];
                    complement = (complement >> subtraendNegativeShift) | (aux << (64 - subtraendNegativeShift));
                    complement = ~complement;
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
                        }
                    }
                    else
                    {
                        outDifference[i] = sum.Item2;
                        carry = sum.Item1;
                    }
                }


                complement = aux >> subtraendNegativeShift;
                if (j < subtrahend.Length)
                {
                    complement |= (subtrahend[j] << (64 - subtraendNegativeShift));
                }

                complement = ~complement;
                sum = MathFunctions.Add(complement, minuend[i]);
                if (carry)
                {
                    outDifference[i] = sum.Item2 + 1;
                }
                else
                {
                    outDifference[i] = sum.Item2;
                }
            }

            var result = minuendLength - 1;
            for (; result > -1; --result)
            {
                if (outDifference[result] != 0)
                {
                    return result + 1;
                }
            }

            return result + 1;
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
            var carry = false;
            var i = 0;

            // Tratamento dos valores iniciais
            if (i < minuendOffset)
            {
                var currentSubtrahend = subtrahend[i];
                if (currentSubtrahend == 0)
                {
                    outDifference[i] = 0;
                    carry = true;

                    ++i;
                    while (carry && i < minuendOffset)
                    {
                        currentSubtrahend = subtrahend[i];
                        if (currentSubtrahend == 0)
                        {
                            outDifference[i] = 0;
                        }
                        else
                        {
                            outDifference[i] = ~currentSubtrahend + 1;
                            carry = false;
                        }

                        ++i;
                    }

                    // Não ocorre transporte
                    for (; i < minuendOffset; ++i)
                    {
                        currentSubtrahend = subtrahend[i];
                        outDifference[i] = ~currentSubtrahend;
                    }
                }
                else
                {
                    outDifference[i] = ~currentSubtrahend + 1;
                    ++i;

                    // Não ocorre transporte
                    for (; i < minuendOffset; ++i)
                    {
                        currentSubtrahend = subtrahend[i];
                        outDifference[i] = ~currentSubtrahend;
                    }
                }
            }

            var j = 0;
            for (; i < subtrahendLength; ++i, ++j)
            {
                var complement = ~subtrahend[i];
                var sum = MathFunctions.Add(complement, minuend[j]);
                if (carry)
                {
                    if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        outDifference[i] = 0;
                    }
                    else
                    {
                        outDifference[i] = sum.Item2 + 1;
                        carry = sum.Item1;
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            var result = subtrahendLength - 1;
            for (; result > -1; --result)
            {
                if (outDifference[result] != 0)
                {
                    return result + 1;
                }
            }

            return result + 1;
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
                var current = subtrahend[j];
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
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            var result = minuendLength - 1;
            for (; result > -1; --result)
            {
                if (outDifference[result] != 0)
                {
                    return result + 1;
                }
            }

            return result + 1;
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
            var carry = true;
            var i = 0;

            // Tratamento dos valores iniciais
            if (i < minuendOffset)
            {
                var currentSubtrahend = subtrahend[i];
                if (currentSubtrahend == 0)
                {
                    outDifference[i] = 0;
                    carry = true;

                    ++i;
                    while (carry && i < minuendOffset)
                    {
                        currentSubtrahend = subtrahend[i];
                        if (currentSubtrahend == 0)
                        {
                            outDifference[i] = 0;
                        }
                        else
                        {
                            outDifference[i] = ~currentSubtrahend + 1;
                            carry = false;
                        }

                        ++i;
                    }

                    // Não ocorre transporte
                    for (; i < minuendOffset; ++i)
                    {
                        currentSubtrahend = subtrahend[i];
                        outDifference[i] = ~currentSubtrahend;
                    }
                }
                else
                {
                    outDifference[i] = ~currentSubtrahend + 1;
                    ++i;

                    // Não ocorre transporte
                    for (; i < minuendOffset; ++i)
                    {
                        currentSubtrahend = subtrahend[i];
                        outDifference[i] = ~currentSubtrahend;
                    }

                    carry = false;
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
                }
            }
            else
            {
                outDifference[i] = sum.Item2;
                carry = sum.Item1;
            }

            ++i;
            ++j;
            for (; i < subtrahendLength; ++i, ++j)
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
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            var result = subtrahendLength - 1;
            for (; result > -1; --result)
            {
                if (outDifference[result] != 0)
                {
                    return result + 1;
                }
            }

            return result + 1;
        }

        /// <summary>
        /// Determina a diferença entre dois números na qual o minuendo se encontra
        /// deslocado e é submetido a um deslocamento negativo.
        /// </summary>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="minuendOffset">O deslocamento vectorial do minuendo.</param>
        /// <param name="minuendShift">O deslocamento negativo menor do minuendo.</param>
        /// <param name="subtrahend">O sbutraendo.</param>
        /// <param name="subtrahendLength">O comprimento válido do subtraendo.</param>
        /// <param name="outDifference">O vector que irá conter a diferença.</param>
        /// <returns>O tamanho da diferença.</returns>
        private int InvSubtractDiffLengthNegShift(
            ulong[] minuend,
            int minuendOffset,
            int minuendShift,
            ulong[] subtrahend,
            int subtrahendLength,
            ulong[] outDifference)
        {
            var carry = true;
            var i = 0;

            // Tratamento dos valores iniciais
            var lastOffset = minuendOffset - 1;
            if (i < lastOffset)
            {
                var currentSubtrahend = subtrahend[i];
                if (currentSubtrahend == 0)
                {
                    outDifference[i] = 0;
                    carry = true;

                    ++i;
                    while (carry && i < lastOffset)
                    {
                        currentSubtrahend = subtrahend[i];
                        if (currentSubtrahend == 0)
                        {
                            outDifference[i] = 0;
                        }
                        else
                        {
                            outDifference[i] = ~currentSubtrahend + 1;
                            carry = false;
                        }

                        ++i;
                    }

                    // Não ocorre transporte
                    for (; i < lastOffset; ++i)
                    {
                        currentSubtrahend = subtrahend[i];
                        outDifference[i] = ~currentSubtrahend;
                    }
                }
                else
                {
                    outDifference[i] = ~currentSubtrahend + 1;
                    ++i;

                    // Não ocorre transporte
                    for (; i < lastOffset; ++i)
                    {
                        currentSubtrahend = subtrahend[i];
                        outDifference[i] = ~currentSubtrahend;
                    }

                    carry = false;
                }
            }

            // Pré-execução anterior ao ciclo
            var j = 0;
            var aux = minuend[j];
            var current = aux << (64 - minuendShift);
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
                }
            }
            else
            {
                outDifference[i] = sum.Item2;
                carry = sum.Item1;
            }

            var lastLength = subtrahendLength - 1;
            if (i < lastLength)
            {
                ++i;
                ++j;
                for (; i < lastLength; ++i, ++j)
                {
                    var tempAux = minuend[j];
                    current = (aux >> minuendShift) | (tempAux << (64 - minuendShift));
                    aux = tempAux;
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
                        }
                    }
                    else
                    {
                        outDifference[i] = sum.Item2;
                        carry = sum.Item1;
                    }
                }

                // Última execução do ciclo
                current = aux >> minuendShift;
                if (j < minuend.Length)
                {
                    current |= (minuend[j] << (64 - minuendShift));
                }

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
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            var result = subtrahendLength - 1;
            for (; result > -1; --result)
            {
                if (outDifference[result] != 0)
                {
                    return result + 1;
                }
            }

            return result + 1;
        }

        /// <summary>
        /// Determina a diferença entre dois valores sem aplicar quaisquer
        /// deslocamentos.
        /// </summary>
        /// <remarks>
        /// O comprimento do minuendo deverá ser superior ou igual ao comprimento
        /// do subtraendo. Caso contrário, a função irá retornar valores errados.
        /// </remarks>
        /// <param name="minuend">O minuendo.</param>
        /// <param name="minuendLength">O número de entradas válidas no minuendo.</param>
        /// <param name="subtrahend">O subtraendo.</param>
        /// <param name="subtrahendLength">O número de entradas válidas no subtraendo.</param>
        /// <param name="outDifference">O vector que irá conter o resultado.</param>
        /// <returns>O número de entradas válidas no resultado.</returns>
        private int GeneralSubtract(
            ulong[] minuend,
            int minuendLength,
            ulong[] subtrahend,
            int subtrahendLength,
            ulong[] outDifference)
        {
            var carry = false;
            var i = 0;
            var complement = ~subtrahend[i];
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
            for (; i < minuendLength; ++i)
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
                    }
                }
                else
                {
                    outDifference[i] = sum.Item2;
                    carry = sum.Item1;
                }
            }

            if (carry)
            {
                for (; i < minuendLength; ++i)
                {
                    var current = minuend[i];
                    if (current == ulong.MaxValue)
                    {
                        outDifference[i] = 0;
                    }
                    else
                    {
                        outDifference[i] = minuend[i];
                        ++i;
                        for (; i < minuendLength; ++i)
                        {
                            outDifference[i] = minuend[i];
                        }
                    }
                }
            }
            else
            {
                for (; i < minuendLength; ++i)
                {
                    outDifference[i] = minuend[i];
                }
            }

            var result = minuendLength - 1;
            for (; result > -1; --result)
            {
                if (outDifference[result] != 0)
                {
                    return result + 1;
                }
            }

            return result + 1;
        }

        /// <summary>
        /// Incrementa o valor do quociente em um valor determinado.
        /// </summary>
        /// <param name="quo">O quociente a ser incrementado.</param>
        /// <param name="increment">O incremento.</param>
        /// <param name="startIndex">O índice inicial.</param>
        private void IncrementQuo(
            ulong[] quo,
            ulong increment,
            int startIndex)
        {
            var length = quo.Length;
            var i = startIndex;
            var sum = MathFunctions.Add(
                quo[i],
                increment);
            quo[i] = sum.Item2;
            if (sum.Item1)
            {
                ++i;
                for (; i < length; ++i)
                {
                    var currentQuo = quo[i];
                    if (currentQuo == ulong.MaxValue)
                    {
                        quo[i] = 0;
                    }
                    else
                    {
                        quo[i] = currentQuo + 1;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Incrementa o quociente em uma unidade.
        /// </summary>
        /// <param name="quo">O quociente.</param>
        /// <param name="startIndex">O índice inicial.</param>
        private void IncrementQuo(
            ulong[] quo,
            int startIndex)
        {
            var length = quo.Length;
            for (int i = startIndex; i < length; ++i)
            {
                var currentQuo = quo[i];
                if (currentQuo == ulong.MaxValue)
                {
                    quo[i] = 0;
                }
                else
                {
                    quo[i] = currentQuo + 1;
                    return;
                }
            }
        }

        /// <summary>
        /// Decrementa o quociente em um valor.
        /// </summary>
        /// <param name="quo">O quociente.</param>
        /// <param name="decrement">O decremento.</param>
        /// <param name="startIndex">O índice inicial.</param>
        private void DecrementQuo(
            ulong[] quo,
            ulong decrement,
            int startIndex)
        {
            var length = quo.Length;
            var i = startIndex;
            var complement = ~decrement + 1;
            var sum = MathFunctions.Add(
                quo[i],
                complement);
            quo[i] = sum.Item2;
            if (!sum.Item1)
            {
                ++i;
                for (; i < length; ++i)
                {
                    var currentQuo = quo[i];
                    if (currentQuo == 0)
                    {
                        quo[i] = ulong.MaxValue;
                    }
                    else
                    {
                        quo[i] = currentQuo - 1;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Decrementa o quociente em uma unidade.
        /// </summary>
        /// <param name="quo">O quociente.</param>
        /// <param name="startIndex">O índice inicial.</param>
        private void DecrementQuo(
            ulong[] quo,
            int startIndex)
        {
            var length = quo.Length;
            for (int i = startIndex; i < length; ++i)
            {
                var currentQuo = quo[i];
                if (currentQuo == 0)
                {
                    quo[i] = ulong.MaxValue;
                }
                else
                {
                    quo[i] = currentQuo - 1;
                    return;
                }
            }
        }

        /// <summary>
        /// Obtém um vector que resulta do vector proporcionado, eliminado os valores
        /// nulos de maior ordem.
        /// </summary>
        /// <param name="vector">O vector.</param>
        /// <returns>O vector resultante.</returns>
        private ulong[] GetTrimmedVector(ulong[] vector)
        {
            for (int i = vector.Length - 1; i > -1; --i)
            {
                if (vector[i] != 0)
                {
                    ++i;
                    var res = new ulong[i];
                    Array.Copy(
                        vector,
                        res,
                        i);
                    return res;
                }
            }

            return null;
        }

        /// <summary>
        /// Obtém o vector cortado segundo um determinado comprimento.
        /// </summary>
        /// <param name="vector">O vector a ser cortado.</param>
        /// <param name="length">O comprimento do vector resultante.</param>
        /// <returns>O vector cortado.</returns>
        private ulong[] GetTrimmedVector(
            ulong[] vector,
            int length)
        {
            var res = new ulong[length];
            Array.Copy(
                vector,
                res,
                length);
            return res;
        }

        #endregion Funções privadas
    }
}
