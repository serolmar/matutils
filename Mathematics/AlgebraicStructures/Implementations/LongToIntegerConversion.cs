namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite realizar conversões entre inteiros e longos.
    /// </summary>
    public class LongToIntegerConversion : IConversion<int,long>
    {
        /// <summary>
        /// Determina se é possível converter um número longo num número inteiro.
        /// </summary>
        /// <param name="objectToConvert">O número longo a ser analisado.</param>
        /// <returns>Veradeiro caso a conversão seja possível e falso caso contrário.</returns>
        public bool CanApplyDirectConversion(long objectToConvert)
        {
            return objectToConvert <= int.MaxValue && objectToConvert >= int.MinValue;
        }

        /// <summary>
        /// Determina se é possível converter um número inteiro num número longo.
        /// </summary>
        /// <param name="objectToConvert">O número inteiro.</param>
        /// <returns>Veradeiro caso a conversão seja possível e falso caso contrário.</returns>
        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return true;
        }

        /// <summary>
        /// Converte um número longo num número inteiro.
        /// </summary>
        /// <param name="objectToConvert">O número longo.</param>
        /// <returns>O número inteiro.</returns>
        public int DirectConversion(long objectToConvert)
        {
            return (int)objectToConvert;
        }

        /// <summary>
        /// Converte um número inteiro num número longo.
        /// </summary>
        /// <param name="objectToConvert">O número inteiro.</param>
        /// <returns>O número longo.</returns>
        public long InverseConversion(int objectToConvert)
        {
            return objectToConvert;
        }
    }
}
