namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa uma matriz esparsa com base em dicionários.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem os argumentos.</typeparam>
    public class SparseDictionarySquareMatrix<ObjectType> : SparseDictionaryMatrix<ObjectType>, ISquareMatrix<ObjectType>
    {
        /// <summary>
        /// Cria instâncias de obejctos do tipo <see cref="SparseDictionarySquareMatrix{CoeffType}"/>
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        public SparseDictionarySquareMatrix(int dimension) : base(dimension, dimension) { }

        /// <summary>
        /// Cria instâncias de obejctos do tipo <see cref="SparseDictionarySquareMatrix{CoeffType}"/>
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public SparseDictionarySquareMatrix(int dimension, ObjectType defaultValue) 
            : base(dimension, dimension, defaultValue) { }
    }
}
