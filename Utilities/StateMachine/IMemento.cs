namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IMemento : IDisposable
    {
        /// <summary>
        /// Checks if memento consumes a lot of resources.
        /// </summary>
        bool IsHeavyMemento { get; }
    }
}
