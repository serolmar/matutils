namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define um item tabular defindo com um nome e um índice.
    /// </summary>
    public interface INamedTabularItem : ITabularItem
    {
        /// <summary>
        /// Obtém o número do item tabular no respectivo conjunto de itens tabulares.
        /// </summary>
        /// <value>O número do item tabular.</value>
        int TabularItemNumber { get; }

        /// <summary>
        /// Obtém o nome do item tabular no respectivo conjunto de itens tabulares.
        /// </summary>
        /// <value>O nome do item tabular.</value>
        string Name { get; }
    }
}
