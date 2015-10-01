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

    /// <summary>
    /// Define uma entidade capaz de memorizar o seu estado num memorizador.
    /// </summary>
    public interface IMementoOriginator
    {
        /// <summary>
        /// Saves the originator to a memento object.
        /// </summary>
        /// <returns>The memento.</returns>
        IMemento SaveToMemento();

        /// <summary>
        /// Restores the originator to the specified memento.
        /// </summary>
        /// <param name="memento">The restoring memento.</param>
        void RestoreToMemento(IMemento memento);
    }
}
