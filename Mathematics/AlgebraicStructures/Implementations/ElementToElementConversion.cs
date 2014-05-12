namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Converte objectos em outros objectos do mesmo tipo.
    /// </summary>
    /// <remarks>
    /// Esta classe define uma espécie de identidade de conversões de tipos. As conversões
    /// poderão ser úteis em alguns algoritmos.
    /// </remarks>
    /// <typeparam name="ElementType">O tipo de objectos sob conversão.</typeparam>
    public class ElementToElementConversion<ElementType> : IConversion<ElementType, ElementType>
    {

        /// <summary>
        /// Determina se é possível converter o objecto.
        /// </summary>
        /// <param name="objectToConvert">O objecto a ser analisado.</param>
        /// <returns>Verdadeiro.</returns>
        public bool CanApplyDirectConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determina se é possível converter o objecto.
        /// </summary>
        /// <param name="objectToConvert">O objecto a ser analisado.</param>
        /// <returns>Verdadeiro.</returns>
        public bool CanApplyInverseConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// Converte um objecto num objecto do mesmo tipo.
        /// </summary>
        /// <param name="objectToConvert">O objecto a ser convertido.</param>
        /// <returns>O resultado da conversão.</returns>
        /// <exception cref="System.ArgumentNullException">Caso o objecto passado seja nulo.</exception>
        public ElementType DirectConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else
            {
                return objectToConvert;
            }
        }

        /// <summary>
        /// Converte um obejcto num objecto do mesmo tipo.
        /// </summary>
        /// <param name="objectToConvert">O objecto a ser convertido.</param>
        /// <returns>O resultado da conversão.</returns>
        /// <exception cref="System.ArgumentNullException">Caso o objecto passado seja nulo.</exception>
        public ElementType InverseConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else
            {
                return objectToConvert;
            }
        }
    }
}
