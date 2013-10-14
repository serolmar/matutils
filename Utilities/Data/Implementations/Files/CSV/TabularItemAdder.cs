namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TabularItemAdder<ElementType>
        : IDataParseAdder<ITabularItem, ElementType>
    {
        /// <summary>
        /// Adiciona um conjunto de elementos a um item tabular.
        /// </summary>
        /// <param name="addObj">O item tabular.</param>
        /// <param name="elements">O conjunto de elementos.</param>
        public void Add(
            ITabularItem addObj, 
            IEnumerable<ElementType> elements)
        {
            addObj.Add(elements);
        }
    }
}
