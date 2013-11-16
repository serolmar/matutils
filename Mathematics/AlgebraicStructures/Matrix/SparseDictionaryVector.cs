namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    public class SparseDictionaryVector<CoeffType> : IVector<CoeffType>
    {
        private CoeffType defaultValue;

        private int length;

        private Dictionary<int, CoeffType> vectorEntries;

        public SparseDictionaryVector(int size, CoeffType defaultValue)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size");
            }
            else
            {
                this.vectorEntries = new Dictionary<int, CoeffType>();
                this.length = size;
                this.defaultValue = defaultValue;
            }
        }

        public SparseDictionaryVector(int size)
            : this(size, default(CoeffType))
        {
        }

        /// <summary>
        /// Otbém e atribui o valor da entrada do vector especificada pelo respectivo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor da entrada.</returns>
        public CoeffType this[int index]
        {
            get
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index must be non-negative and less than the size of the vector.");
                }
                else
                {
                    var value = default(CoeffType);
                    if (this.vectorEntries.TryGetValue(index, out value))
                    {
                        return value;
                    }
                    else
                    {
                        return this.defaultValue;
                    }
                }
            }
            set
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index must be non-negative and less than the size of the vector.");
                }
                else
                {
                    if (this.vectorEntries.ContainsKey(index))
                    {
                        if (EqualityComparer<CoeffType>.Default.Equals(value, this.defaultValue))
                        {
                            this.vectorEntries.Remove(index);
                        }
                        else
                        {
                            this.vectorEntries[index] = value;
                        }
                    }
                    else
                    {
                        this.vectorEntries.Add(index, value);
                    }
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
                return this.length;
            }
        }

        /// <summary>
        /// Obtém o valor por defeito associado ao vector.
        /// </summary>
        public CoeffType DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }

        /// <summary>
        /// Expande o vector, acrescentando um número específico de entradas.
        /// </summary>
        /// <param name="entriesNumber">O número de entradas a acrescentar.</param>
        private void Expand(int entriesNumber)
        {
            if (entriesNumber < 0)
            {
                throw new ArgumentException("Entries number must be non-negative.");
            }
            else
            {
                this.length += entriesNumber;
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
        /// Obtém o sub-vector especificado pela sequência de inteiros.
        /// </summary>
        /// <param name="indices">A sequência de inteiros.</param>
        /// <returns>O sub-vector.</returns>
        public IVector<CoeffType> GetSubVector(IntegerSequence indices)
        {
            return new IntegerSequenceSubVector<CoeffType>(this, indices);
        }

        /// <summary>
        /// Troca dois elementos do vector.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public void SwapElements(int first, int second)
        {
            if (first != second)
            {
                if (first < 0 || first >= this.length)
                {
                    throw new ArgumentOutOfRangeException("first");
                }
                else if (second < 0 || second >= this.length)
                {
                    throw new ArgumentOutOfRangeException("second");
                }
                else
                {
                    var firstValue = default(CoeffType);
                    if (this.vectorEntries.TryGetValue(first, out firstValue))
                    {
                        var secondValue = default(CoeffType);
                        if (this.vectorEntries.TryGetValue(second, out secondValue))
                        {
                            this.vectorEntries[first] = secondValue;
                            this.vectorEntries[second] = firstValue;
                        }
                        else
                        {
                            this.vectorEntries.Add(second, firstValue);
                            this.vectorEntries.Remove(first);
                        }
                    }
                    else
                    {
                        var secondValue = default(CoeffType);
                        if (this.vectorEntries.TryGetValue(second, out secondValue))
                        {
                            this.vectorEntries.Add(first, secondValue);
                            this.vectorEntries.Remove(second);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Verifica se a entrada especificada pelo índice é diferente do valor por defeito ou está incluída
        /// no vector.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>Verdadeiro caso a entrada esteja no vector e falso caso contrário.</returns>
        public bool ContainsEntry(int index)
        {
            return this.vectorEntries.ContainsKey(index);
        }

        public bool IsNull(IMonoid<CoeffType> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else
            {
                if (monoid.IsAdditiveUnity(this.defaultValue))
                {
                    if (this.vectorEntries.Count > 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (this.vectorEntries.Count < this.length)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (var itemKvp in this.vectorEntries)
                        {
                            if (!monoid.IsAdditiveUnity(itemKvp.Value))
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
        }

        public IVector<CoeffType> Clone()
        {
            var result = new SparseDictionaryVector<CoeffType>(this.length);
            result.defaultValue = this.defaultValue;
            result.vectorEntries = new Dictionary<int, CoeffType>();
            foreach (var kvp in this.vectorEntries)
            {
                result.vectorEntries.Add(kvp.Key, kvp.Value);
            }

            return result;
        }

        /// <summary>
        /// Remove a entrada especificada pelo índice, passando esta a adquirir o valor especificado por defeito.
        /// </summary>
        /// <param name="index">O índice a remover.</param>
        private void Remove(int index)
        {
            if (index >= 0)
            {
                this.Remove(index);
            }
        }

        /// <summary>
        /// Obtém todas as entradas diferentes do valor por defeito e respectivos índices.
        /// </summary>
        /// <returns>O conjunto das entradas e respectivos índices.</returns>
        IEnumerable<KeyValuePair<int, CoeffType>> GetEntries()
        {
            return this.vectorEntries;
        }

        /// <summary>
        /// Obtém o enumerador para todos os elementos do vector que são diferentes do valor por defeito.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<CoeffType> GetEnumerator()
        {
            foreach (var kvp in this.vectorEntries)
            {
                yield return kvp.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
