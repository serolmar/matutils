namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um leitor de números de precisão dupla.
    /// </summary>
    public class DoubleDataReader : IDataReader<string>
    {
        /// <summary>
        /// Obtém o tipo dos objetos lidos.
        /// </summary>
        /// <value>O tipo de dados.</value>
        public Type ObjectType
        {
            get
            {
                return typeof(double);
            }
        }

        /// <summary>
        /// Tenta fazer a leitura do objecto.
        /// </summary>
        /// <param name="text">O texto.</param>
        /// <param name="value">O valor que irá conter a leitura.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryRead(string text, out object value)
        {
            var innerValue = 0.0;
            if (double.TryParse(text, out innerValue))
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
