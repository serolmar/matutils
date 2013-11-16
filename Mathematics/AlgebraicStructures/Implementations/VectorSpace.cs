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

        private IRing<CoeffType> ring;

        private IVectorFactory<CoeffType> vectorFactory;

        private IEqualityComparer<IEnumerable<CoeffType>> orderedColComparer;

        public VectorSpace(
            int dimension,
            IVectorFactory<CoeffType> vectorFactory,
            IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
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
                this.ring = ring;
                this.vectorFactory = vectorFactory;
                this.orderedColComparer = new OrderedColEqualityComparer<CoeffType>(ring);
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
                var result = this.vectorFactory.CreateVector(vectorSpaceElement.Length, this.ring.AdditiveUnity);
                for (int i = 0; i < vectorSpaceElement.Length; ++i)
                {
                    result[i] = this.ring.Multiply(coefficientElement, vectorSpaceElement[i]);
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
                var result = this.vectorFactory.CreateVector(number.Length, this.ring.AdditiveUnity);
                for (int i = 0; i < number.Length; ++i)
                {
                    result[i] = this.ring.AdditiveInverse(number[i]);
                }

                return result;
            }
        }

        public IVector<CoeffType> AdditiveUnity
        {
            get
            {
                return new ZeroVector<CoeffType>(this.dimension, this.ring);
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
                return value.IsNull(this.ring);
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
                var result = this.vectorFactory.CreateVector(this.dimension, this.ring.AdditiveUnity);
                for (int i = 0; i < this.dimension; ++i)
                {
                    result[i] = this.ring.Add(left[i], right[i]);
                }

                return result;
            }
        }
    }
}
