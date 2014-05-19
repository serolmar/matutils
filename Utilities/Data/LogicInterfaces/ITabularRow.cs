namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Define uma linha de tabela.
    /// </summary>
    public interface ITabularRow
        : IIndexed<int, ITabularCell>, IEnumerable<ITabularCell>
    {
    }
}
