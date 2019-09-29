namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa funções que permitem criar objectos do tipo <see cref="SparseDictionarySquareMatrixFactory{ObjectType}"/>.
    /// </summary>
    /// <typeparam name="ObjectType">
    /// O tipo de objectos que constituem os coeficientes das matrizes criadas.
    /// </typeparam>
    public class SparseDictionarySquareMatrixFactory<ObjectType> : IMathMatrixFactory<ObjectType>
    {
        /// <summary>
        /// Cria uma matriz quadrada esparsa baseada em dicionários.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <returns>A matriz esparsa.</returns>
        /// <exception cref="ArgumentException">If the number of lines doesn't match the number of columns.</exception>
        public IMathMatrix<ObjectType> CreateMatrix(int lines, int columns)
        {
            if (lines == columns)
            {
                return new SparseDictionarySquareMathMatrix<ObjectType>(lines);
            }
            else
            {
                throw new ArgumentException("The number of lines doesn't match the number of columns.");
            }
        }

        /// <summary>
        /// Cria uma matriz quadrada esparsa baseada em dicionários.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>A matriz esparsa.</returns>
        /// <exception cref="ArgumentException">If the number of lines doesn't match the number of columns.</exception>
        public IMathMatrix<ObjectType> CreateMatrix(int lines, int columns, ObjectType defaultValue)
        {
            if (lines == columns)
            {
                return new SparseDictionarySquareMathMatrix<ObjectType>(lines, defaultValue);
            }
            else
            {
                throw new ArgumentException("The number of lines doesn't match the number of columns.");
            }
        }
    }
}
