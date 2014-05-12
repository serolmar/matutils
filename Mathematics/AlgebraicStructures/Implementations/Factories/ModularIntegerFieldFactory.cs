namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite criar instâncias de corpos modulares sobre inteiros.
    /// </summary>
    public class ModularIntegerFieldFactory : IModularFieldFactory<int>
    {
        /// <summary>
        /// Cria a instância de um corpo modular cujo módulo é passado como argumento.
        /// </summary>
        /// <remarks>
        /// Apesar de ser possível criar instâncias dos vários tipos de objectos que permitem realizar
        /// aritmética modular, alguns algoritmos poderão necessitar criá-los internamente.</remarks>
        /// <param name="modulus">O módulo.</param>
        /// <returns>O corpo modular.</returns>
        public IModularField<int> CreateInstance(int modulus)
        {
            return new ModularIntegerField(modulus);
        }
    }
}
