namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface INamedTabularItem : ITabularItem
    {
        /// <summary>
        /// Obtém o número do item tabular no respectivo conjunto de itens tabulares.
        /// </summary>
        int TabularItemNumber { get; }

        /// <summary>
        /// Obtém o nome do item tabular no respectivo conjunto de itens tabulares.
        /// </summary>
        string Name { get; }
    }
}
