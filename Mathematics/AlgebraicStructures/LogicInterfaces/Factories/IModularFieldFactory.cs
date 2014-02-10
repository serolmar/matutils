namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite criar uma instância de um corpo modular.
    /// </summary>
    /// <typeparam name="T">O tipo dos valores sobre os quais actual o corpo.</typeparam>
    public interface IModularFieldFactory<T>
    {
        /// <summary>
        /// Cria a instância de um corpo modular cujo módulo é passado como argumento.
        /// </summary>
        /// <param name="modulus">O módulo.</param>
        /// <returns>O corpo modular.</returns>
        IModularField<T> CreateInstance(T modulus);
    }
}
