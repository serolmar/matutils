namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma conversão.
    /// </summary>
    public interface IConversion<FirstType, SecondType>
    {
        /// <summary>
        /// Indica se o objecto é convertível num do primeiro tipo.
        /// </summary>
        /// <param name="objectToConvert">O objecto.</param>
        /// <returns>Verdadeiro caso seja convertível e falso no caso contrário.</returns>
        bool CanApplyDirectConversion(SecondType objectToConvert);

        /// <summary>
        /// Indica se o objecto é convertível num do segundo tipo.
        /// </summary>
        /// <param name="objectToConvert">O objecto.</param>
        /// <returns>Verdadeiro caso seja convertível e falso no caso contrário.</returns>
        bool CanApplyInverseConversion(FirstType objectToConvert);

        /// <summary>
        /// Converte o objecto do segundo tipo no primeiro.
        /// </summary>
        /// <param name="objectToConvert">O objecto a ser convertido.</param>
        /// <returns>O valor da conversão.</returns>
        FirstType DirectConversion(SecondType objectToConvert);

        /// <summary>
        /// Converte o objecto do primeiro tipo no segundo.
        /// </summary>
        /// <param name="objectToConvert">O objecto a ser convertido.</param>
        /// <returns>O valor da conversão.</returns>
        SecondType InverseConversion(FirstType objectToConvert);
    }
}
