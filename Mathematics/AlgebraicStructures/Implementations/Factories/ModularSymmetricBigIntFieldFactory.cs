namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Permite criar instâncias de corpos modulares simétricos para inteiros de precisão arbitrária.
    /// </summary>
    public class ModularSymmetricBigIntFieldFactory : IModularFieldFactory<BigInteger>
    {
        /// <summary>
        /// Obtém a instância de um corpo modular simétrico.
        /// </summary>
        /// <remarks>
        /// Apesar de ser possível criar instâncias dos vários tipos de objectos que permitem realizar
        /// aritmética modular, alguns algoritmos poderão necessitar criá-los internamente.
        /// </remarks>
        /// <param name="modulus">O módulo associado ao corpo modular.</param>
        /// <returns>O corpo modular simétrico.</returns>
        public IModularField<BigInteger> CreateInstance(BigInteger modulus)
        {
            return new ModularSymmetricBigIntField(modulus);
        }
    }
}
