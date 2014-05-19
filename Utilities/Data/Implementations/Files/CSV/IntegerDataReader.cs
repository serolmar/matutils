namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um leitor de inteiros.
    /// </summary>
    public class IntegerDataReader : IDataReader<string>
    {
        /// <summary>
        /// O tipo de obejecto a ser lido.
        /// </summary>
        /// <value>O tipo <see cref="System.int"/>.</value>
        public Type ObjectType
        {
            get
            {
                return typeof(int);
            }
        }

        /// <summary>
        /// Tenta fazer a leitura de um número inteiro.
        /// </summary>
        /// <param name="text">O texto.</param>
        /// <param name="value">O valor que conterá a leitura.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryRead(string text, out object value)
        {
            var innerValue = 0;
            if (int.TryParse(text, out innerValue))
            {
                value = innerValue;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}
