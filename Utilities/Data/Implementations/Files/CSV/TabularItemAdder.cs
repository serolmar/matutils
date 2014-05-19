namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite adicionar linhas a uma tabela.
    /// </summary>
    /// <typeparam name="ElementType">O tipo de objetos que constituem as entradas da tabela.</typeparam>
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
