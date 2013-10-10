namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IDataReader<in ConversionType>
    {
        /// <summary>
        /// Obtém o tipo do objecto proporcionado pelo leitor.
        /// </summary>
        Type ObjectType { get; }

        /// <summary>
        /// Tenta obter a leitura de um objecto a partir de um outro objecto.
        /// </summary>
        /// <param name="conversionObject">O objecto a ser convertido.</param>
        /// <param name="value">O valor.</param>
        /// <returns>Verdadeiro caso a leitura seja bem sucedida e falso caso contrário.</returns>
        bool TryRead(ConversionType conversionObject, out object value);
    }
}
