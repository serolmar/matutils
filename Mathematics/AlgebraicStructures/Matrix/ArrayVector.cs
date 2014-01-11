namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    public class ArrayVector<CoeffType> : IVector<CoeffType>
    {
        /// <summary>
        /// Mantém o conjunto de entradas do vector.
        /// </summary>
        private CoeffType[] vectorEntries;

        public ArrayVector(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.vectorEntries = new CoeffType[length];
            }
        }

        public ArrayVector(int length, CoeffType defaultValue)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.vectorEntries = new CoeffType[length];
                if (!EqualityComparer<object>.Default.Equals(defaultValue, default(CoeffType)))
                {
                    for (int i = 0; i < length; ++i)
                    {
                        this.vectorEntries[i] = defaultValue;
                    }
                }
            }
        }

        /// <summary>
        /// Permite criar um vector com base num conjunto de valores pré-definidos.
        /// </summary>
        /// <param name="elements">O conjunto de valores pré-definidos.</param>
        public ArrayVector(CoeffType[] elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }
            else
            {
                this.vectorEntries = new CoeffType[elements.Length];
                Array.Copy(elements, this.vectorEntries, elements.Length);
            }
        }

        private ArrayVector()
        {
        }

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada pelo índice respectivo.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CoeffType this[int index]
        {
            get
            {
                if (index < 0 || index >= this.vectorEntries.Length)
                {
                    throw new IndexOutOfRangeException("Index must be non-negative and less than the size of the vector.");
                }
                else
                {
                    return this.vectorEntries[index];
                }
            }
            set
            {
                if (index < 0 || index >= this.vectorEntries.Length)
                {
                    throw new IndexOutOfRangeException("Index must be non-negative and less than the size of the vector.");
                }
                else
                {
                    this.vectorEntries[index] = value;
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public int Length
        {
            get
            {
                return this.vectorEntries.Length;
            }
        }

        /// <summary>
        /// Obtém o sub-vector especificado pelos índices.
        /// </summary>
        /// <param name="indices">Os índices.</param>
        /// <returns>O sub-vector.</returns>
        public IVector<CoeffType> GetSubVector(int[] indices)
        {
            return new SubVector<CoeffType>(this, indices);
        }

        /// <summary>
        /// Obtém o sub-vector especificado pela sequência especificada.
        /// </summary>
        /// <param name="indices">A sequência.</param>
        /// <returns>O sub-vector.</returns>
        public IVector<CoeffType> GetSubVector(IntegerSequence indices)
        {
            return new IntegerSequenceSubVector<CoeffType>(this, indices);
        }

        /// <summary>
        /// Troca dois elementos do vector.
        /// </summary>
        /// <param name="first">A posição do primeiro elmeneto.</param>
        /// <param name="second">A posição do segundo elemento.</param>
        public void SwapElements(int first, int second)
        {
            if (first != second)
            {
                if (first < 0 || first >= this.vectorEntries.Length)
                {
                    throw new ArgumentOutOfRangeException("first");
                }
                else if (second < 0 || second >= this.vectorEntries.Length)
                {
                    throw new ArgumentOutOfRangeException("second");
                }
                else
                {
                    var swap = this.vectorEntries[first];
                    this.vectorEntries[first] = this.vectorEntries[second];
                    this.vectorEntries[second] = swap;
                }
            }
        }

        public IEnumerator<CoeffType> GetEnumerator()
        {
            for (int i = 0; i < this.vectorEntries.Length; ++i)
            {
                yield return this.vectorEntries[i];
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
                for (int i = 0; i < this.vectorEntries.Length; ++i)
                {
                    if (!monoid.IsAdditiveUnity(this.vectorEntries[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public IVector<CoeffType> Clone()
        {
            var result = new ArrayVector<CoeffType>();
            result.vectorEntries = new CoeffType[this.vectorEntries.Length];
            Array.Copy(this.vectorEntries, result.vectorEntries, this.vectorEntries.Length);
            return result;
        }

        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("[");
            if (this.vectorEntries.Length > 0)
            {
                resultBuilder.Append(this.vectorEntries[0]);
                for (int i = 1; i < this.vectorEntries.Length; ++i)
                {
                    resultBuilder.Append(",");
                    resultBuilder.Append(this.vectorEntries[i]);
                }
            }

            resultBuilder.Append("]");
            return resultBuilder.ToString();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
