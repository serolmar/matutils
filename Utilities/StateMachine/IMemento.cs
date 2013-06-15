using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public interface IMemento : IDisposable
    {
        /// <summary>
        /// Checks if memento consumes a lot of resources.
        /// </summary>
        bool IsHeavyMement { get; }
    }
}
