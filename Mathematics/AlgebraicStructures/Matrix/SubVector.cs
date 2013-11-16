namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Representa um sub-vector.
    /// </summary>
    internal class SubVector<CoeffType> : IVector<CoeffType>
    {
        private IVector<CoeffType> vector;

        private int[] subVectorIndices;

        public SubVector(IVector<CoeffType> vector, int[] subVectorIndices)
        {
            if (subVectorIndices == null)
            {
                throw new ArgumentNullException("subVectorIndices");
            }
            else
            {
                this.subVectorIndices = new int[subVectorIndices.Length];
                Array.Copy(subVectorIndices, this.subVectorIndices, subVectorIndices.Length);
                this.vector = vector;
            }
        }

        private SubVector()
        {
        }

        /// <summary>
        /// Obtém e atribui a entrada do vector especificada pelo respectivo índice.
        /// </summary>
        /// <param name="index">O índice que identifica a entrada do vector.</param>
        /// <returns>A entrada do vector.</returns>
        public CoeffType this[int index]
        {
            get
            {
                if (index < 0 || index >= this.subVectorIndices.Length)
                {
                    throw new IndexOutOfRangeException("Index must be non-negative and less than the size of vector.");
                }
                else
                {
                    return this.vector[this.subVectorIndices[index]];
                }
            }
            set
            {
                if (index < 0 || index >= this.subVectorIndices.Length)
                {
                    throw new IndexOutOfRangeException("Index must be non-negative and less than the size of vector.");
                }
                else
                {
                    this.vector[this.subVectorIndices[index]] = value;
                }
            }
        }

        /// <summary>
        /// Otbém o tamanho do vector.
        /// </summary>
        public int Length
        {
            get
            {
                return this.subVectorIndices.Length;
            }
        }

        /// <summary>
        /// Obtém um sub-vector do vector corrente.
        /// </summary>
        /// <param name="indices">O conjunto de índices que identificam o sub-vector.</param>
        /// <returns>O sub-vector.</returns>
        public IVector<CoeffType> GetSubVector(int[] indices)
        {
            return new SubVector<CoeffType>(this, indices);
        }

        /// <summary>
        /// Obtém o sub-vector especificado por uma sequência de inteiros.
        /// </summary>
        /// <param name="indices">A sequência de inteiros.</param>
        /// <returns>O sub-vector.</returns>
        public IVector<CoeffType> GetSubVector(IntegerSequence indices)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Permite trocar dois elementos do sub-vector sem influenciar o vector original.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public void SwapElements(int first, int second)
        {
            if (first != second)
            {
                if (first < 0 || first >= this.subVectorIndices.Length)
                {
                    throw new ArgumentOutOfRangeException("first");
                }
                else if (second < 0 || second >= this.subVectorIndices.Length)
                {
                    throw new ArgumentOutOfRangeException("second");
                }
                else
                {
                    var swapValue = this.subVectorIndices[first];
                    this.subVectorIndices[first] = this.subVectorIndices[second];
                    this.subVectorIndices[second] = swapValue;
                }
            }
        }

        public bool IsNull(IMonoid<CoeffType> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else
            {
                for (int i = 0; i < this.subVectorIndices.Length; ++i)
                {
                    if (!monoid.IsAdditiveUnity(this.vector[this.subVectorIndices[i]]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public IVector<CoeffType> Clone()
        {
            var result = new SubVector<CoeffType>();
            result.vector = this.vector;
            result.subVectorIndices = new int[this.subVectorIndices.Length];
            Array.Copy(this.subVectorIndices, result.subVectorIndices, this.subVectorIndices.Length);
            return result;
        }

        public IEnumerator<CoeffType> GetEnumerator()
        {
            for (int i = 0; i < this.subVectorIndices.Length; ++i)
            {
                yield return this.vector[this.subVectorIndices[i]];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
