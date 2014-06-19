namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um corpo molular.
    /// </summary>
    /// <typeparam name="ModuleType">O tipo de objectos que constituem o módulo.</typeparam>
    public interface IModularField<ModuleType> : IField<ModuleType>
    {
        /// <summary>
        /// Obtém e atribui o módulo.
        /// </summary>
        ModuleType Module { get; set; }

        /// <summary>
        /// Obtém a forma reduzida do elemento especificado.
        /// </summary>
        /// <param name="element">O elemento.</param>
        /// <returns>A forma reduzida.</returns>
        ModuleType GetReduced(ModuleType element);
    }
}
