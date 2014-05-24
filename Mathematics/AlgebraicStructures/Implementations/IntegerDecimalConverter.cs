namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define uma conversão entre um número inteiro e um número 
    /// decimal <see cref="double"/>.
    /// </summary>
    public class IntegerDecimalConverter : IConversion<int, decimal>
    {
        /// <summary>
        /// Determina se é possível converter um número decimal num número inteiro.
        /// </summary>
        /// <remarks>
        /// Um número decimal é convertível num número inteiro caso a sua parte decimal seja nula e o número
        /// especificado se encontre dentro dos limites.
        /// </remarks>
        /// <param name="objectToConvert">O número decimal a ser analisado.</param>
        /// <returns>
        /// Verdadeiro caso o número decimal seja convertível num número inteiro e falso caso contrário.
        /// </returns>
        public bool CanApplyDirectConversion(decimal objectToConvert)
        {
            return Math.Round(objectToConvert) == objectToConvert &&
                (objectToConvert <= int.MaxValue && objectToConvert >= int.MinValue);
        }

        /// <summary>
        /// A função retorna sempre verdadeiro uma vez que é sempre possível converter um inteiro num decimal.
        /// </summary>
        /// <param name="objectToConvert">O valor inteiro a ser analisado.</param>
        /// <returns>Verdadeiro.</returns>
        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return true;
        }

        /// <summary>
        /// Obtém a conversão de um número decimal num número inteiro quando esta é possível.
        /// </summary>
        /// <param name="objectToConvert">O número decimal a ser convertido.</param>
        /// <returns>O número inteiro representado pelo número decimal.</returns>
        /// <exception cref="MathematicsException">Se o número decimal não for convertível.</exception>
        public int DirectConversion(decimal objectToConvert)
        {
            var value = Math.Round(objectToConvert);
            if (value != objectToConvert || objectToConvert > int.MaxValue || objectToConvert < int.MaxValue)
            {
                throw new MathematicsException(string.Format("Can't convert value {0} to integer.", objectToConvert));
            }
            else
            {
                return (int)value;
            }
        }

        /// <summary>
        /// Obtém a representação decimal de um número inteiro.
        /// </summary>
        /// <param name="objectToConvert">O número inteiro.</param>
        /// <returns>A representação decimal</returns>
        public decimal InverseConversion(int objectToConvert)
        {
            return objectToConvert;
        }
    }
}
