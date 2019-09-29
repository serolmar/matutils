namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Cria vectores conceptuais baseados em dicionários.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de valores das entradas do vector.</typeparam>
    public class SparseDictionaryMathVectorFactory<CoeffType> : IMathVectorFactory<CoeffType>
    {
        /// <summary>
        /// O valor por defeito.
        /// </summary>
        private CoeffType defaultValue;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SparseDictionaryMathVectorFactory{CoeffType}"/>.
        /// </summary>
        public SparseDictionaryMathVectorFactory()
        {
            this.defaultValue = default(CoeffType);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SparseDictionaryMathVectorFactory{CoeffType}"/>.
        /// </summary>
        /// <param name="defaultValue">O valor por defeito.</param>
        public SparseDictionaryMathVectorFactory(
            CoeffType defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        /// <summary>
        /// Cria um vector com o tamanho especificado.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        /// <returns>O vector criado.</returns>
        public IMathVector<CoeffType> CreateVector(int length)
        {
            return new SparseDictionaryMathVector<CoeffType>(length, this.defaultValue);
        }

        /// <summary>
        /// Cria um vector com o tamanho especificado e um valor por defeito.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>O vector criado.</returns>
        public IMathVector<CoeffType> CreateVector(int length, CoeffType defaultValue)
        {
            return new SparseDictionaryMathVector<CoeffType>(length, defaultValue);
        }
    }
}
