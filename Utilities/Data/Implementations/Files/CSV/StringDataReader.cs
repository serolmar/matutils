namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um leitor de texto.
    /// </summary>
    public class StringDataReader : IDataReader<string>
    {
        /// <summary>
        /// O tipo dos objectos lidos.
        /// </summary>
        /// <value>O tipo dos objectos.</value>
        public Type ObjectType
        {
            get
            {
                return typeof(string);
            }
        }

        /// <summary>
        /// Tenta fazer a leitura de texto.
        /// </summary>
        /// <param name="conversionObject">O texto.</param>
        /// <param name="value">O valor que conterá a leitura.</param>
        /// <returns>Verdadeiro.</returns>
        public bool TryRead(string conversionObject, out object value)
        {
            value = conversionObject;
            return true;
        }
    }
}
