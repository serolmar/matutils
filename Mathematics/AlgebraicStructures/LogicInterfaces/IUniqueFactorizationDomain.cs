namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IUniqueFactorizationDomain<T> : IRing<T>
    {
        /// <summary>
        /// Permite obter o número de unidades.
        /// </summary>
        int UnitsCount { get; }

        /// <summary>
        /// Permite enumerar todas as unidades.
        /// </summary>
        IEnumerable<T> Units { get; }
    }
}
