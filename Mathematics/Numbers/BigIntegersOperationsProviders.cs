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
        private int SubtractShiftedQuoFromRema(int shiftOffset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Subtrai o resto do quociente rodado.
        /// </summary>
        /// <param name="shiftOffset">O deslocamento do quociente rodado.</param>
        /// <returns>O tamanho do novo resto.</returns>
        private int SubtractRemFromShiftedQuo(int remLength, int shiftOffset)
        {
            throw new NotImplementedException();
        }
    }
}
