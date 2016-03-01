namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa as operações de espaço vectorial.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem as entradas dos vectores.</typeparam>
    public class VectorSpace<CoeffType> : IVectorSpace<CoeffType, IMathVector<CoeffType>>
    {
        /// <summary>
        /// A dimensão dos vectores tratados pelo espaço actual.
        /// </summary>
        private int dimension;

        /// <summary>
        /// O corpo responsável pelas operações sobre os coeficientes.
        /// </summary>
        private IField<CoeffType> field;

        /// <summary>
        /// A fábrica responsável pela criação de instâncias de vectores.
        /// </summary>
        private IVectorFactory<CoeffType> vectorFactory;

        /// <summary>
        /// O comparador de colecções independente da ordem.
        /// </summary>
        private IEqualityComparer<IEnumerable<CoeffType>> orderedColComparer;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="VectorSpace{CoeffType}"/>.
        /// </summary>
        /// <param name="dimension">A dimensão dos vectoes tratados pelo espaço vectorial.</param>
        /// <param name="vectorFactory">A fábrica responsável pela criação de vectores..</param>
        /// <param name="field">O corpo responsável pelas operações sobre vectores.</param>
        /// <exception cref="ArgumentNullException">
        /// Se os argumentos "vectorFactory" ou "field" forem nulos.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">Se a dimensão for um número negativo.</exception>
        public VectorSpace(
            int dimension,
            IVectorFactory<CoeffType> vectorFactory,
            IField<CoeffType> field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            else if (vectorFactory == null)
            {
                throw new ArgumentNullException("vectorFactory");
            }
            else if (dimension < 0)
            {
                throw new ArgumentOutOfRangeException("dimension");
            }
            else
            {
                this.dimension = dimension;
                this.field = field;
                this.vectorFactory = vectorFactory;
                this.orderedColComparer = new OrderedEqualityColComparer<CoeffType>(field);
            }
        }

        /// <summary>
        /// Obtém o corpo associado ao espaço vectorial.
        /// </summary>
        /// <value>
        /// O corpo associado ao espaço vectorial.
        /// </value>
        public IField<CoeffType> Field
        {
            get
            {
                return this.field;
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pela ciração de vectores durante as operações.
        /// </summary>
        /// <value>
        /// O objecto responsável pela ciração de vectores durante as operações.
        /// </value>
        public IVectorFactory<CoeffType> VectorFactory
        {
            get
            {
                return this.vectorFactory;
            }
        }

        /// <summary>
        /// Obtém a unidade aditiva.
        /// </summary>
        /// <value>
        /// A unidade aditiva.
        /// </value>
        public IMathVector<CoeffType> AdditiveUnity
        {
            get
            {
                return new ZeroVector<CoeffType>(this.dimension, this.field);
            }
        }

        /// <summary>
        /// Multiplica o vector por um escalar.
        /// </summary>
        /// <param name="coefficientElement">O escalar.</param>
        /// <param name="vectorSpaceElement">O vector.</param>
        /// <returns>O resultado da multiplicação.</returns>
        /// <exception cref="ArgumentNullException">Se um dos argumentos for nulo.</exception>
        /// <exception cref="MathematicsException">
        /// Se a dimensão do vector não coincidir com a dimensão
        /// definda para o espaço vectorial corrente.</exception>
        public IMathVector<CoeffType> MultiplyScalar(
            CoeffType coefficientElement, 
            IMathVector<CoeffType> vectorSpaceElement)
        {
            if (coefficientElement == null)
            {
                throw new ArgumentNullException("coefficientElement");
            }
            else if (vectorSpaceElement == null)
            {
                throw new ArgumentNullException("vectorSpaceElement");
            }
            else if (vectorSpaceElement.Length != this.dimension)
            {
                throw new MathematicsException("Vector's dimension doesn't match space dimension");
            }
            else
            {
                var result = this.vectorFactory.CreateVector(vectorSpaceElement.Length, this.field.AdditiveUnity);
                for (int i = 0; i < vectorSpaceElement.Length; ++i)
                {
                    result[i] = this.field.Multiply(coefficientElement, vectorSpaceElement[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// Determina a inversa aditiva de um vector.
        /// </summary>
        /// <param name="number">O vector.</param>
        /// <returns>A inversa aditiva.</returns>
        /// <exception cref="ArgumentNullException">Se o vector for nulo.</exception>
        /// <exception cref="MathematicsException">
        /// Se a dimensão do vector não coincidir com a dimensão definida para o espaço
        /// vectorial.
        /// </exception>
        public IMathVector<CoeffType> AdditiveInverse(IMathVector<CoeffType> number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else if (number.Length != this.dimension)
            {
                throw new MathematicsException("Vector's dimension doesn't match space dimension");
            }
            else
            {
                var result = this.vectorFactory.CreateVector(number.Length, this.field.AdditiveUnity);
                for (int i = 0; i < number.Length; ++i)
                {
                    result[i] = this.field.AdditiveInverse(number[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// Determina se o vector proporcionado é uma unidade aditiva.
        /// </summary>
        /// <param name="value">O vector.</param>
        /// <returns>Verdadeiro caso o vector seja uma unidade aditiva e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Se o vector for nulo.</exception>
        /// <exception cref="MathematicsException">
        /// Se a dimensão do vector não coincidir com a dimensão definida para o espaço
        /// vectorial.
        /// </exception>
        public bool IsAdditiveUnity(IMathVector<CoeffType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else if (value.Length != this.dimension)
            {
                throw new MathematicsException("Vector's dimension doesn't match space dimension");
            }
            else
            {
                return value.IsNull(this.field);
            }
        }

        /// <summary>
        /// Determina se ambos os vectores são iguais.
        /// </summary>
        /// <param name="x">O primeiro vector a ser comparado.</param>
        /// <param name="y">O segundo vector a ser comparado.</param>
        /// <returns>
        /// Verdadeiro se ambos os vectores forem iguais e falso caso contrário.
        /// </returns>
        public bool Equals(IMathVector<CoeffType> x, IMathVector<CoeffType> y)
        {
            return this.orderedColComparer.Equals(x, y);
        }

        /// <summary>
        /// Obtém o código confuso de um vector.
        /// </summary>
        /// <param name="obj">O vector.</param>
        /// <returns>
        /// O código confuso do vector adequado à utilização em alguns algoritmos habituais.
        /// </returns>
        public int GetHashCode(IMathVector<CoeffType> obj)
        {
            return this.orderedColComparer.GetHashCode(obj);
        }

        /// <summary>
        /// Calcula a soma de dois vectores.
        /// </summary>
        /// <param name="left">O primeiro vector a ser adicionado.</param>
        /// <param name="right">O segundo vector a ser adicionado.</param>
        /// <returns>O resultado da adição.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        /// <exception cref="MathematicsException">
        /// Se a dimensão do vector não coincidir com a dimensão definida para o espaço
        /// vectorial.
        /// </exception>
        public IMathVector<CoeffType> Add(IMathVector<CoeffType> left, IMathVector<CoeffType> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (left.Length != this.dimension)
            {
                throw new MathematicsException("Vector's dimension doesn't match space dimension");
            }
            else if (right.Length != this.dimension)
            {
                throw new MathematicsException("Vector's dimension doesn't match space dimension");
            }
            else
            {
                var result = this.vectorFactory.CreateVector(this.dimension, this.field.AdditiveUnity);
                for (int i = 0; i < this.dimension; ++i)
                {
                    result[i] = this.field.Add(left[i], right[i]);
                }

                return result;
            }
        }
    }
}
