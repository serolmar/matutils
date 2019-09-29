namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa funções que permitem criar objectos do tipo <see cref="SparseDictionaryMathMatrix{ObjectType}"/>.
    /// </summary>
    /// <typeparam name="ObjectType">
    /// O tipo de objectos que constituem os coeficientes das matrizes criadas.
    /// </typeparam>
    public class SparseDictionaryMathMatrixFactory<ObjectType> : IMathMatrixFactory<ObjectType>
    {
        /// <summary>
        /// O objecto considerado por defeito.
        /// </summary>
        private ObjectType defaultValue;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SparseDictionaryMathMatrixFactory{ObjectType}"/>.
        /// </summary>
        public SparseDictionaryMathMatrixFactory()
        {
            this.defaultValue = default(ObjectType);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SparseDictionaryMathMatrixFactory{ObjectType}"/>.
        /// </summary>
        /// <param name="defaultValue">O valor por defeito.</param>
        public SparseDictionaryMathMatrixFactory(
            ObjectType defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        /// <summary>
        /// Cria uma matriz esparsa baseada em dicionários.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <returns>A matriz esparsa.</returns>
        public IMathMatrix<ObjectType> CreateMatrix(int lines, int columns)
        {
            return new SparseDictionaryMathMatrix<ObjectType>(lines, columns, this.defaultValue);
        }

        /// <summary>
        /// Cria uma matriz esparsa baseada em dicionários.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>A matriz esparsa.</returns>
        public IMathMatrix<ObjectType> CreateMatrix(int lines, int columns, ObjectType defaultValue)
        {
            return new SparseDictionaryMathMatrix<ObjectType>(lines, columns, defaultValue);
        }
    }
}
