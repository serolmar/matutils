namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite adicionar os valores lidos por coluna.
    /// </summary>
    /// <typeparam name="ElementsType">O tipo de elementos a considerar.</typeparam>
    public class ListTypeTransposedAdder<ElementsType>
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
            var pointer = 0;
            foreach (var element in elements)
            {
                if (pointer < addObj.Count)
                {
                    addObj[pointer].Add(element);
                }
                else
                {
                    addObj.Add(new List<ElementsType>() { element });
                }

                ++pointer;
            }
        }
    }
}
