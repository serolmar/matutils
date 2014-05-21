namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define um memorizador.
    /// </summary>
    public interface IMemento : IDisposable
    {
        /// <summary>
        /// Obtém um valor que determina se o memorizador consome muitos recursos.
        /// </summary>
        /// <value>Verdadeiro caso o memorizador consuma muitos recursos e falso caso contrário.</value>
        bool IsHeavyMemento { get; }
    }
}
