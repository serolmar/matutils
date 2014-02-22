namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Permite criar instâncias de corpos modulares simétricos.
    /// </summary>
    public class ModularSymmetricBigIntFieldFactory : IModularFieldFactory<BigInteger>
    {
        /// <summary>
        /// Obtém a instância de um corpo modular simétrico.
        /// </summary>
        /// <param name="modulus">O módulo associado ao corpo modular.</param>
        /// <returns>O corpo modular simétrico.</returns>
        public IModularField<BigInteger> CreateInstance(BigInteger modulus)
        {
            return new ModularSymmetricBigIntField(modulus);
        }
    }
}
