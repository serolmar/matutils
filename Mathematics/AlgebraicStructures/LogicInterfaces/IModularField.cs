namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um corpo molular.
    /// </summary>
    public interface IModularField<ModuleType> : IField<ModuleType>
    {
        /// <summary>
        /// Obtém o módulo.
        /// </summary>
        ModuleType Module { get; }
    }
}
