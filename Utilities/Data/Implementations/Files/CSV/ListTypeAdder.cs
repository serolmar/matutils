namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa uma interaface capaz de adicionar um item a uma lista de listas.
    /// </summary>
    /// <typeparam name="ElementsType">O tipo de objectos que constituem as entradas das listas.</typeparam>
    public class ListTypeAdder<ElementsType> 
        : IDataParseAdder<List<List<ElementsType>>, ElementsType>
    {
        /// <summary>
        /// Adiciona um conjunto de elementos a uma lista de listas.
        /// </summary>
        /// <param name="addObj">A lista de listas.</param>
        /// <param name="elements">Os elementos a serem adicionados.</param>
        public void Add(
            List<List<ElementsType>> addObj, 
            IEnumerable<ElementsType> elements)
        {
            var listToAdd = new List<ElementsType>();
            listToAdd.AddRange(elements);
            addObj.Add(listToAdd);
        }
    }
}
