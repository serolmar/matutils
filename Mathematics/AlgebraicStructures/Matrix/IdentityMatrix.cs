namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    public class IdentityMatrix<ElementType, RingType> : IMatrix<ElementType>
        where RingType : IRing<ElementType>
    {
        /// <summary>
        /// O anel responsável pela indicação das unidades aditiva e multiplicativa.
        /// </summary>
        private RingType ring;

        /// <summary>
        /// As dimensões da matriz quadrada.
        /// </summary>
        private int dimensions;

        public IdentityMatrix(int dimensions, RingType ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (dimensions < 0)
            {
                throw new ArgumentOutOfRangeException("dimensions");
            }
            else {
                this.ring = ring;
                this.dimensions = dimensions;
            }
        }

        public ElementType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.dimensions)
                {
                    throw new IndexOutOfRangeException("Parameter line must be non-negative and less than the size of matrix.");
                }
                else if (column < 0 || column >= this.dimensions)
                {
                    throw new IndexOutOfRangeException("Parameter line must be non-negative and less than the size of matrix.");
                }
                else
                {
                    if (line == column)
                    {
                        return this.ring.MultiplicativeUnity;
                    }
                    else
                    {
                        return this.ring.AdditiveUnity;
                    }
                }
            }
            set
            {
                throw new MathematicsException("Identity matrix is constant.");
            }
        }

        public bool IsSymmetric(IEqualityComparer<ElementType> equalityComparer)
        {
            return true;
        }

        public int GetLength(int dimension)
        {
            if (dimension == 0 || dimension == 1)
            {
                return this.dimensions;
            }
            else
            {
                throw new ArgumentOutOfRangeException("dimension");
            }
        }

        public IMatrix<ElementType> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<ElementType>(this, lines, columns);
        }

        public IMatrix<ElementType> GetSubMatrix(IntegerSequence lines, IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<ElementType>(this, lines, columns);
        }

        public void SwapLines(int i, int j)
        {
            throw new MathematicsException("Identity matrix is constant.");
        }

        public void SwapColumns(int i, int j)
        {
            throw new MathematicsException("Identity matrix is constant.");
        }

        public IEnumerator<ElementType> GetEnumerator()
        {
            for (int i = 0; i < this.dimensions; ++i)
            {
                for (int j = 0; j < this.dimensions; ++j)
                {
                    if (i == j)
                    {
                        yield return this.ring.MultiplicativeUnity;
                    }
                    else
                    {
                        yield return this.ring.AdditiveUnity;
                    }
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
