namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma conversão entre dois tipos de objectos.
    /// </summary>
    /// <remarks>
    /// Apesar de definir uma conversão enrte objectos do tipo <see cref="SecondType"/> em objectos do tipo
    /// <see cref="FirstType"/>, é ainda definida uma operação que permite definir a conversão inversa.
    /// </remarks>
    /// <typeparam name="FirstType">O tipo de objecto que resulta da conversão.</typeparam>
    /// <typeparam name="SecondType">O tipo de objecto a converter.</typeparam>
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
