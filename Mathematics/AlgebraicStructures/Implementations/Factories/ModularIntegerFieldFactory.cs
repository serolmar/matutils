namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ModularIntegerFieldFactory : IModularFieldFactory<int>
    {
        /// <summary>
        /// Cria a instância de um corpo modular cujo módulo é passado como argumento.
        /// </summary>
        /// <param name="modulus">O módulo.</param>
        /// <returns>O corpo modular.</returns>
        public IModularField<int> CreateInstance(int modulus)
        {
            return new ModularIntegerField(modulus);
        }
    }
}
