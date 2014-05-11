namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Indica que não é possível converter entre os dois tipos de dados.
    /// </summary>
    /// <typeparam name="FirstType">O tipo de objecto que resulta da conversão.</typeparam>
    /// <typeparam name="SecondType">O tipo de objecto a converter.</typeparam>
    public class CantConvertConversion<FirstType, SecondType> : IConversion<FirstType, SecondType>
    {
        /// <summary>
        /// Indica se o objecto é convertível num do primeiro tipo.
        /// </summary>
        /// <param name="objectToConvert">O objecto.</param>
        /// <returns>Verdadeiro caso seja convertível e falso no caso contrário.</returns>
        public bool CanApplyDirectConversion(SecondType objectToConvert)
        {
            // Nuca é possível converter.
            return false;
        }

        /// <summary>
        /// Indica se o objecto é convertível num do segundo tipo.
        /// </summary>
        /// <param name="objectToConvert">O objecto.</param>
        /// <returns>Verdadeiro caso seja convertível e falso no caso contrário.</returns>
        public bool CanApplyInverseConversion(FirstType objectToConvert)
        {
            // Nunca é possível converter.
            return false;
        }

        /// <summary>
        /// Converte o objecto do segundo tipo no primeiro.
        /// </summary>
        /// <param name="objectToConvert">O objecto a ser convertido.</param>
        /// <returns>O valor da conversão.</returns>
        /// <exception cref="MathematicsException">Sempre.</exception>
        public FirstType DirectConversion(SecondType objectToConvert)
        {
            throw new MathematicsException(
                string.Format("Can't convert from type {0} to type {1}.",
                typeof(SecondType).ToString(),
                typeof(FirstType).ToString()));
        }

        /// <summary>
        /// Converte o objecto do primeiro tipo no segundo.
        /// </summary>
        /// <param name="objectToConvert">O objecto a ser convertido.</param>
        /// <returns>O valor da conversão.</returns>
        /// <exception cref="MathematicsException">Sempre.</exception>
        public SecondType InverseConversion(FirstType objectToConvert)
        {
            throw new MathematicsException(
                string.Format("Can't convert from type {0} to type {1}.",
                typeof(SecondType).ToString(),
                typeof(FirstType).ToString()));
        }
    }
}
