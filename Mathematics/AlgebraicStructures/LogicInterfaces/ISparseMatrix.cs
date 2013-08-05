namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ISparseMatrix<ObjectType> : IMatrix<ObjectType>
    {
        /// <summary>
        /// Obtém o valor por defeito.
        /// </summary>
        ObjectType DefaultValue { get; }

        /// <summary>
        /// Obtém o número de linhas não nulas.
        /// </summary>
        int NumberOfLines { get; }

        /// <summary>
        /// Obtém um enumerador para todas as linhas não nulas da matriz.
        /// </summary>
        /// <returns>As linhas não nulas da matriz.</returns>
        IEnumerator<KeyValuePair<int, ISparseMatrixLine<ObjectType>>> GetLines();

        /// <summary>
        /// Remove a linha.
        /// </summary>
        /// <param name="lineNumber">O número da linha a ser removida.</param>
        void Remove(int lineNumber);
    }
}
