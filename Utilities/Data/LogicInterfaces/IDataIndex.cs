using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public interface IDataIndex<out C, T>
    {
        /// <summary>
        /// Obtém os números da linha associados ao índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>Os números da linha.</returns>
        int[] this[T[] index] { get;}

        /// <summary>
        /// Adiciona um valor ao índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="value">O valor.</param>
        void Add(T[] index, int value);

        /// <summary>
        /// Obtém as colunas assoicadas ao índice.
        /// </summary>
        C[] Columns { get; }

        /// <summary>
        /// Verifica se o índice está contido no indexador.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>Verdadeiro caso o índice esteja contido e falso caso contrário.</returns>
        bool ContainsIndex(T[] index);

        /// <summary>
        /// Tenta obter os valores associados ao índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="value">Os valores.</param>
        /// <returns>Verdadeiro caso seja possível obter o índice e falso caso contrário.</returns>
        bool TryGetValue(T[] index, out int[] value);

        /// <summary>
        /// Remove o índice que está a indexar o valor especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="value">O valor.</param>
        void Remove(T[] index, int value);

        /// <summary>
        /// Elimina a indexação do índice.  
        /// </summary>
        void Clear();
    }
}
