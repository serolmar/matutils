// -----------------------------------------------------------------------
// <copyright file="FileTest.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define uma interface para um objecto que adiciona um conjunto de valores
    /// a um objecto.
    /// </summary>
    /// <typeparam name="ObjectCollectionType">
    /// O tipo de objecto que corresponde ao contentor de elementos.
    /// </typeparam>
    /// <typeparam name="ElementType">O tipo de elementos.</typeparam>
    public interface IDataParseAdder<in ObjectCollectionType, in ElementType>
    {
        /// <summary>
        /// Adiciona o conjunto de elementos ao objecto.
        /// </summary>
        /// <param name="addObj">O objecto.</param>
        /// <param name="elements">O conjunto de elementos a serem adicionados.</param>
        void Add(ObjectCollectionType addObj, IEnumerable<ElementType> elements);
    }
}
