namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa um vector esparso baseado em dicionários.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem as entradas dos vectores.</typeparam>
    public class SparseDictionaryMathVector<CoeffType> : ISparseVector<CoeffType>, IMathVector<CoeffType>
    {
        /// <summary>
        /// Objecto responsável pela sicronização de processos de execução.
        /// </summary>
        private object lockObject = new object();

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
        private SortedDictionary<int, CoeffType> vectorEntries;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SparseDictionaryMathVector{CoeffType}"/>.
        /// </summary>
        /// <param name="size">O tamanho do vector.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o tamanho do vector for negativo.</exception>
        public SparseDictionaryMathVector(int size, CoeffType defaultValue)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size");
            }
            else
            {
                this.vectorEntries = new SortedDictionary<int, CoeffType>(Comparer<int>.Default);
                this.length = size;
                this.defaultValue = defaultValue;
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SparseDictionaryMathVector{CoeffType}"/>.
        /// </summary>
        /// <remarks>
        /// Como se trata de um construtor interno à livraria, nenhuma verificação é realizada no que concerne
        /// à integridade dos argumentos.
        /// </remarks>
        /// <param name="size">O tamanho do vector.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <param name="vectorEntries">As entradas do vector.</param>
        internal SparseDictionaryMathVector(
            int size,
            CoeffType defaultValue,
            SortedDictionary<int, CoeffType> vectorEntries)
        {
            this.length = size;
            this.vectorEntries = vectorEntries;
            this.defaultValue = defaultValue;
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SparseDictionaryMathVector{CoeffType}"/>.
        /// </summary>
        /// <param name="size">O tamanho do vector.</param>
        public SparseDictionaryMathVector(int size)
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
                    lock (this.lockObject)
                    {
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
            }
            set
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index must be non-negative and less than the size of the vector.");
                }
                else
                {
                    lock (this.lockObject)
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
        public CoeffType this[long index]
        {
            get
            {
                if (index < 0L || index >= this.LongLength)
                {
                    throw new IndexOutOfRangeException("Index must be non-negative and less than the size of the vector.");
                }
                else
                {
                    var value = default(CoeffType);
                    lock (this.lockObject)
                    {
                        if (this.vectorEntries.TryGetValue((int)index, out value))
                        {
                            return value;
                        }
                        else
                        {
                            return this.defaultValue;
                        }
                    }
                }
            }
            set
            {
                if (index < 0L || index >= this.LongLength)
                {
                    throw new IndexOutOfRangeException("Index must be non-negative and less than the size of the vector.");
                }
                else
                {
                    lock (this.lockObject)
                    {
                        if (this.vectorEntries.ContainsKey((int)index))
                        {
                            if (EqualityComparer<CoeffType>.Default.Equals(value, this.defaultValue))
                            {
                                this.vectorEntries.Remove((int)index);
                            }
                            else
                            {
                                this.vectorEntries[(int)index] = value;
                            }
                        }
                        else
                        {
                            this.vectorEntries.Add((int)index, value);
                        }
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
        /// Obtém o tamanho do vector.
        /// </summary>
        /// <value>
        /// O tamanho do vector.
        /// </value>
        public long LongLength
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
        /// Obtém o número de entradas não nulas.
        /// </summary>
        public int NumberOfEntries
        {
            get
            {
                return this.vectorEntries.Count;
            }
        }

        /// <summary>
        /// Obtém o contentor para as entradas do vector.
        /// </summary>
        internal SortedDictionary<int, CoeffType> VectorEntries
        {
            get
            {
                return this.vectorEntries;
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
                    lock (this.lockObject)
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
        }

        /// <summary>
        /// Verifica se a entrada especificada pelo índice é diferente do valor por defeito ou está incluída
        /// no vector.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>Verdadeiro caso a entrada esteja no vector e falso caso contrário.</returns>
        public bool ContainsEntry(int index)
        {
            lock (this.lockObject)
            {
                return this.vectorEntries.ContainsKey(index);
            }
        }

        /// <summary>
        /// Obtém um enumerador para todas as entradas não nulas do vector.
        /// </summary>
        /// <returns>As entradas não nulas do vector.</returns>
        IEnumerable<KeyValuePair<int, CoeffType>> ISparseVector<CoeffType>.GetEntries()
        {
            return this.vectorEntries;
        }

        /// <summary>
        /// Remove a entrada.
        /// </summary>
        /// <param name="entryNumber">O número da entrada a ser removida.</param>
        void ISparseVector<CoeffType>.Remove(int entryNumber)
        {
            lock (this.lockObject)
            {
                this.vectorEntries.Remove(entryNumber);
            }
        }

        /// <summary>
        /// Tenta obter a entrada especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da entrada.</param>
        /// <param name="entry">A entrada.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryGetLine(int index, out CoeffType entry)
        {
            entry = default(CoeffType);
            if (index >= 0)
            {
                lock (this.lockObject)
                {
                    return this.vectorEntries.TryGetValue(index, out entry);
                }
            }
            else
            {
                return false;
            }
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
            var result = new SparseDictionaryMathVector<CoeffType>(this.length);
            result.defaultValue = this.defaultValue;
            result.vectorEntries = new SortedDictionary<int, CoeffType>(Comparer<int>.Default);
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
                lock (this.lockObject)
                {
                    this.Remove(index);
                }
            }
        }

        /// <summary>
        /// Copia o conteúdo do vector para um alcance.
        /// </summary>
        /// <param name="array">O alcance.</param>
        /// <param name="index">O índice a partir do qual se inicia a cópia.</param>
        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (index < 0 || index > this.length)
            {
                throw new ArgumentOutOfRangeException(
                    "index",
                    "Index was out of range. Must be non-negative and less than the size of collection.");
            }
            else
            {
                var arrayDimension = array.Rank;
                if (arrayDimension == 1)
                {
                    var length = this.length;
                    if (index + length <= array.LongLength)
                    {
                        var pointer = index;
                        var i = 0L;
                        var keyValuePairEnum = this.vectorEntries.GetEnumerator();
                        while (keyValuePairEnum.MoveNext())
                        {
                            var currKvp = keyValuePairEnum.Current;
                            var entryKey = currKvp.Key;
                            var entryVal = currKvp.Value;
                            while (i < entryKey)
                            {
                                array.SetValue(this.defaultValue, pointer++);
                                ++i;
                            }

                            array.SetValue(entryVal, pointer++);
                            ++i;
                        }
                    }
                    else
                    {
                        throw new ArgumentException(
                            "The number of elements in the source array is greater than the available number of elements from index to the end of the destination array");
                    }
                }
                else
                {
                    throw new ArgumentException(
                        "The provided array is multidimensional");
                }
            }
        }

        /// <summary>
        /// Copia o conteúdo do vector para um alcance.
        /// </summary>
        /// <param name="array">O alcance.</param>
        /// <param name="index">O índice a partir do qual se inicia a cópia.</param>
        public void CopyTo(Array array, long index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (index < 0 || index > this.length)
            {
                throw new ArgumentOutOfRangeException(
                    "index",
                    "Index was out of range. Must be non-negative and less than the size of collection.");
            }
            else
            {
                var arrayDimension = array.Rank;
                if (arrayDimension == 1)
                {
                    var length = this.length;
                    if (index + length <= array.LongLength)
                    {
                        var pointer = index;
                        var i = 0L;
                        var keyValuePairEnum = this.vectorEntries.GetEnumerator();
                        while (keyValuePairEnum.MoveNext())
                        {
                            var currKvp = keyValuePairEnum.Current;
                            var entryKey = currKvp.Key;
                            var entryVal = currKvp.Value;
                            while (i < entryKey)
                            {
                                array.SetValue(this.defaultValue, pointer++);
                                ++i;
                            }

                            array.SetValue(entryVal, pointer++);
                            ++i;
                        }
                    }
                    else
                    {
                        throw new ArgumentException(
                            "The number of elements in the source array is greater than the available number of elements from index to the end of the destination array");
                    }
                }
                else
                {
                    throw new ArgumentException(
                        "The provided array is multidimensional");
                }
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
