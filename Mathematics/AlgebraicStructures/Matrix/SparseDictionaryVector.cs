namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Implementa um vector esparso baseado em dicionários.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem as entradas dos vectores.</typeparam>
    public class SparseDictionaryVector<CoeffType> : IVector<CoeffType>
    {
        /// <summary>
        /// O valor por defeito.
        /// </summary>
        private CoeffType defaultValue;

        /// <summary>
        /// O comprimento do vector.
        /// </summary>
        private int length;

        /// <summary>
        /// O contentor para os elementos do vector.
        /// </summary>
        private Dictionary<int, CoeffType> vectorEntries;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SparseDictionaryVector{CoeffType}"/>.
        /// </summary>
        /// <param name="size">O tamanho do vector.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Se o tamanho do vector for negativo.</exception>
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

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SparseDictionaryVector{CoeffType}"/>.
        /// </summary>
        /// <param name="size">O tamanho do vector.</param>
        public SparseDictionaryVector(int size)
            : this(size, default(CoeffType))
        {
        }

        /// <summary>
        /// Otbém e atribui o valor da entrada do vector especificada pelo respectivo índice.
        /// </summary>
        /// <value>
        /// O valor da entrada do vector especificada pelo índice.
        /// </value>
        /// <param name="index">O índice.</param>
        /// <returns>O valor da entrada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice for negativo ou não for inferior ao número de elementos no vector.</exception>
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
        /// <value>
        /// O tamanho do vector.
        /// </value>
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
        /// <value>O valor por defeeito.</value>
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
        /// <exception cref="ArgumentException">Se o argumento for um número negativo.</exception>
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
        /// <param name="first">O primeiro elemento a ser trocado.</param>
        /// <param name="second">O segundo elemento a ser trocado.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se algum dos argumentos for negativo ou não for inferior ao número de elementos no vector.
        /// </exception>
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

        /// <summary>
        /// Averigua se se trata de um vector nulo.
        /// </summary>
        /// <param name="monoid">O monóide responsável pela identificação do zero.</param>
        /// <returns>Veradeiro caso o vector seja nulo e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Se o argumento for nulo.</exception>
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

        /// <summary>
        /// Obtém uma cópia do vector corrente.
        /// </summary>
        /// <returns>A cópia.</returns>
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

        /// <summary>
        /// Obtém o enumerador não genérico para o vector.
        /// </summary>
        /// <returns>O enumerador não genérico para o vector.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
