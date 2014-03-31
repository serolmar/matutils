﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ISparseMatrix<ObjectType> : IMatrix<ObjectType>
    {
        /// <summary>
        /// Obtém a linha pelo respectivo valor.
        /// </summary>
        /// <param name="line">O índice.</param>
        /// <returns>A linha caso exista.</returns>
        /// <exception cref="MathematicsException">Se a linha não existir.</exception>
        ISparseMatrixLine<ObjectType> this[int line] { get; }

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
        IEnumerable<KeyValuePair<int, ISparseMatrixLine<ObjectType>>> GetLines();

        /// <summary>
        /// Remove a linha.
        /// </summary>
        /// <param name="lineNumber">O número da linha a ser removida.</param>
        void Remove(int lineNumber);

        /// <summary>
        /// Verifica se a matriz esparsa contém a linha especificada.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a matriz contenha a linha e falso caso contrário.</returns>
        bool ContainsLine(int line);

        /// <summary>
        /// Tenta obter a linha especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da linha.</param>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        bool TryGetLine(int index, out ISparseMatrixLine<ObjectType> line);

        /// <summary>
        /// Obtém as colunas atribuídas à linha especificada.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <returns>As colunas atribuídas.</returns>
        IEnumerable<KeyValuePair<int, ObjectType>> GetColumns(int line);
    }
}
