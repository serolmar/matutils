﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ISparseMatrixLine<ObjectType> : IEnumerable<KeyValuePair<int, ObjectType>>
    {
        /// <summary>
        /// Obtém e atribui o valor da entrada no índice especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O objecto.</returns>
        ObjectType this[int index] { get; set; }

        /// <summary>
        /// Obtém o número de colunas não nulas.
        /// </summary>
        int NumberOfColumns { get; }

        /// <summary>
        /// Obtém um enumerador para todas as colunas não nulas.
        /// </summary>
        /// <returns>O enumerador.</returns>
        IEnumerator<KeyValuePair<int, ObjectType>> GetColumns();

        /// <summary>
        /// Remove a entrada espeficada pelo índice.
        /// </summary>
        /// <param name="columnIndex">O índice da entrada a ser removido.</param>
        void Remove(int columnIndex);
    }
}
