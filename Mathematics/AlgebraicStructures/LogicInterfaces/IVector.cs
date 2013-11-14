namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    public interface IVector<CoeffType> : IEnumerable<CoeffType>
    {
        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="index">O índice da entrada.</param>
        /// <returns>O valor da entrada.</returns>
        CoeffType this[int index] { get; set; }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Obtém o sub-vector especificado pelos índices.
        /// </summary>
        /// <param name="indices">Os índices.</param>
        /// <returns>O sub-vector.</returns>
        IVector<CoeffType> GetSubVector(int[] indices);

        /// <summary>
        /// Otbém o sub-vector especificado pela sequência de índices.
        /// </summary>
        /// <param name="indices">A sequência de índices.</param>
        /// <returns>O sub-vector.</returns>
        IVector<CoeffType> GetSubVector(IntegerSequence indices);

        /// <summary>
        /// Troca dois elementos do vector.
        /// </summary>
        /// <param name="first">O primeiro elemento a ser trocado.</param>
        /// <param name="second">O segundo elemento a ser trocado.</param>
        void SwapElements(int first, int second);
    }
}
