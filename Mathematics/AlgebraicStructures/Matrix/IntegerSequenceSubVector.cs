namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    internal class IntegerSequenceSubVector<CoeffType> : IVector<CoeffType>
    {
        private IVector<CoeffType> vector;

        private IntegerSequence indicesSequence;

        public IntegerSequenceSubVector(IVector<CoeffType> vector, IntegerSequence indicesSequence)
        {
            if (vector == null)
            {
                throw new ArgumentNullException("vector");
            }
            else
            {
                this.vector = vector;
                this.indicesSequence = indicesSequence.Clone();
            }
        }

        public CoeffType this[int index]
        {
            get
            {
                if (index < 0 || index >= this.indicesSequence.Count)
                {
                    throw new IndexOutOfRangeException("The index must be non-negative and less than the size of the vector.");
                }
                else
                {
                    return this.vector[this.indicesSequence[index]];
                }
            }
            set
            {
                if (index < 0 || index >= this.indicesSequence.Count)
                {
                    throw new IndexOutOfRangeException("The index must be non-negative and less than the size of the vector.");
                }
                else
                {
                    this.vector[this.indicesSequence[index]] = value;
                }
            }
        }

        public int Length
        {
            get
            {
                return this.indicesSequence.Count;
            }
        }

        public IVector<CoeffType> GetSubVector(int[] indices)
        {
            return new SubVector<CoeffType>(this, indices);
        }

        public IVector<CoeffType> GetSubVector(IntegerSequence indices)
        {
            return new IntegerSequenceSubVector<CoeffType>(this, indices);
        }

        public void SwapElements(int first, int second)
        {
            throw new MathematicsException("Can't swap integer sequence sub-vector entries.");
        }

        public IEnumerator<CoeffType> GetEnumerator()
        {
            foreach (var index in this.indicesSequence)
            {
                yield return this.vector[index];
            }
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
