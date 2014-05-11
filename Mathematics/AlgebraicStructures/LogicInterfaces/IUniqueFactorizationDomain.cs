namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define as operações de domínio de factorização único sobre um objecto genérico.
    /// </summary>
    /// <typeparam name="T">
    /// O tipo de objectos sobre os quais estão defindas as operações de domínio de 
    /// facotrização única.
    /// </typeparam>
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
