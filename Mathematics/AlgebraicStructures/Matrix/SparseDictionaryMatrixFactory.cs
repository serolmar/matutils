namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa funções que permitem criar objectos do tipo <see cref="SparseDictionaryMatrix{ObjectType}"/>.
    /// </summary>
    /// <typeparam name="ObjectType">
    /// O tipo de objectos que constituem os coeficientes das matrizes criadas.
    /// </typeparam>
    public class SparseDictionaryMatrixFactory<ObjectType> : IMatrixFactory<ObjectType>
    {
        /// <summary>
        /// Cria uma matriz esparsa baseada em dicionários.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <returns>A matriz esparsa.</returns>
        public IMatrix<ObjectType> CreateMatrix(int lines, int columns)
        {
            return new SparseDictionaryMatrix<ObjectType>(lines, columns);
        }

        /// <summary>
        /// Cria uma matriz esparsa baseada em dicionários.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>A matriz esparsa.</returns>
        public IMatrix<ObjectType> CreateMatrix(int lines, int columns, ObjectType defaultValue)
        {
            return new SparseDictionaryMatrix<ObjectType>(lines, columns, defaultValue);
        }
    }
}
