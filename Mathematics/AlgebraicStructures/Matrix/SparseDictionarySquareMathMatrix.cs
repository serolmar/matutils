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
    public class SparseDictionarySquareMathMatrix<ObjectType> : SparseDictionaryMathMatrix<ObjectType>, ISquareMathMatrix<ObjectType>
    {
        /// <summary>
        /// Cria instâncias de obejctos do tipo <see cref="SparseDictionarySquareMathMatrix{CoeffType}"/>
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        public SparseDictionarySquareMathMatrix(int dimension) : base(dimension, dimension) { }

        /// <summary>
        /// Cria instâncias de obejctos do tipo <see cref="SparseDictionarySquareMathMatrix{CoeffType}"/>
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public SparseDictionarySquareMathMatrix(int dimension, ObjectType defaultValue) 
            : base(dimension, dimension, defaultValue) { }
    }
}
