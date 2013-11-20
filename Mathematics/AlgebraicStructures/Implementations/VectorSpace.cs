namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    public class VectorSpace<CoeffType> : IVectorSpace<CoeffType, IVector<CoeffType>>
    {
        private int dimension;

        private IField<CoeffType> field;

        private IVectorFactory<CoeffType> vectorFactory;

        private IEqualityComparer<IEnumerable<CoeffType>> orderedColComparer;

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
                this.orderedColComparer = new OrderedColEqualityComparer<CoeffType>(field);
            }
        }

        /// <summary>
        /// Obtém o corpo associado ao espaço vectorial.
        /// </summary>
        public IField<CoeffType> Field
        {
            get
            {
                return this.field;
            }
        }

        /// <summary>
        /// Multiplica o vector por um escalar.
        /// </summary>
        /// <param name="coefficientElement">O escalar.</param>
        /// <param name="vectorSpaceElement">O vector.</param>
        /// <returns>O resultado da multiplicação.</returns>
        public IVector<CoeffType> MultiplyScalar(CoeffType coefficientElement, IVector<CoeffType> vectorSpaceElement)
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

        public IVector<CoeffType> AdditiveInverse(IVector<CoeffType> number)
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

        public IVector<CoeffType> AdditiveUnity
        {
            get
            {
                return new ZeroVector<CoeffType>(this.dimension, this.field);
            }
        }

        public bool IsAdditiveUnity(IVector<CoeffType> value)
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

        public bool Equals(IVector<CoeffType> x, IVector<CoeffType> y)
        {
            return this.orderedColComparer.Equals(x, y);
        }

        public int GetHashCode(IVector<CoeffType> obj)
        {
            return this.orderedColComparer.GetHashCode(obj);
        }

        public IVector<CoeffType> Add(IVector<CoeffType> left, IVector<CoeffType> right)
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
