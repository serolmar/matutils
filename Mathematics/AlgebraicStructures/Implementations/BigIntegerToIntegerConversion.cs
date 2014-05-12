namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Permite converter entre um inteiro e um inteiro grande.
    /// </summary>
    public class BigIntegerToIntegerConversion : IConversion<int, BigInteger>
    {
        /// <summary>
        /// Indica se é possível converter o inteiro grande num inteiro normal.
        /// </summary>
        /// <param name="objectToConvert">O inteiro grande a ser verificado.</param>
        /// <returns>
        /// Verdadeiro caso o inteiro grande se encontre no intervalo dos inteiros e falso caso contrário.
        /// </returns>
        public bool CanApplyDirectConversion(BigInteger objectToConvert)
        {
            if (objectToConvert > int.MaxValue || objectToConvert < int.MinValue)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Indica se é possível converter um inteiro num inteiro grande.
        /// </summary>
        /// <param name="objectToConvert">O valor a verificar.</param>
        /// <returns>Verdadeiro.</returns>
        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return true;
        }

        /// <summary>
        /// Sempre que possível converte um inteiro grande num inteiro.
        /// </summary>
        /// <param name="objectToConvert">O inteiro grande a ser convertido.</param>
        /// <returns>O resultado da conversão.</returns>
        /// <exception cref="MathematicsException">Caso a conversão não seja possível devido ao valor do inteiro de precisão arbitrária.</exception>
        public int DirectConversion(BigInteger objectToConvert)
        {
            if (objectToConvert > int.MaxValue || objectToConvert < int.MinValue)
            {
                throw new MathematicsException("Big integer is too big to be converted to an integer.");
            }
            else
            {
                return (int)objectToConvert;
            }
        }

        /// <summary>
        /// Converte um inteiro num inteiro grande.
        /// </summary>
        /// <param name="objectToConvert">O inteiro a ser convertido.</param>
        /// <returns>O inteiro grande que resulta da convcersão.</returns>
        public BigInteger InverseConversion(int objectToConvert)
        {
            return (BigInteger)objectToConvert;
        }
    }
}
