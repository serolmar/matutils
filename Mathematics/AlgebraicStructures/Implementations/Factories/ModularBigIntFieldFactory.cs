namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    public class ModularBigIntFieldFactory : IModularFieldFactory<BigInteger>
    {
        /// <summary>
        /// Cria a instância de um corpo modular cujo módulo é passado como argumento.
        /// </summary>
        /// <param name="modulus">O módulo.</param>
        /// <returns>O corpo modular.</returns>
        public IModularField<BigInteger> CreateInstance(BigInteger modulus)
        {
            return new ModularBigIntegerField(modulus);
        }
    }
}
