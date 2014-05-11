namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite criar corpos modulares simétricos.
    /// </summary>
    public class ModularSymmetricIntFieldFactory : IModularFieldFactory<int>
    {
        /// <summary>
        /// Obtém instâncias de corpos modulares simétricos.
        /// </summary>
        /// <remarks>
        /// Apesar de ser possível criar instâncias dos vários tipos de objectos que permitem realizar
        /// aritmética modular, alguns algoritmos poderão necessitar criá-los internamente.
        /// </remarks>
        /// <param name="modulus">O módulo associado ao corpo modular.</param>
        /// <returns>O corpo modular simétrico.</returns>
        public IModularField<int> CreateInstance(int modulus)
        {
            return new ModularSymmetricIntField(modulus);
        }
    }
}
