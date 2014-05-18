namespace Utilities.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Implementa uma lista de bits.
    /// </summary>
    public class BitList : IList<int>, IEquatable<BitList>, IComparable<BitList>
    {
        #region fields

        /// <summary>
        /// Número de bits contidos num byte.
        /// </summary>
        public static readonly int byteNumber = 8;

        /// <summary>
        /// Número de bits numa variável do vector.
        /// </summary>
        public static readonly int bitNumber = byteNumber * sizeof(ulong);

        /// <summary>
        /// O vector.
        /// </summary>
        private List<ulong> elements;

        /// <summary>
        /// O número de bits atribuídos.
        /// </summary>
        private int countBits = 0;

        /// <summary>
        /// Máscara que identifica os bits atribuídos em cada entrada do vector.
        /// </summary>
        private ulong[] maskPositionBits;

        #endregion

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BitList"/>.
        /// </summary>
        public BitList()
        {
            this.elements = new List<ulong>();
            InitMask();
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BitList"/>.
        /// </summary>
        /// <param name="capacity">A capcidade.</param>
        public BitList(int capacity)
        {
            this.Reserve(capacity);
            this.InitMask();
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BitList"/>.
        /// </summary>
        /// <param name="numberOfBits">O número de bits.</param>
        /// <param name="set">Valor por defeito.</param>
        public BitList(int numberOfBits, bool set)
        {
            Reserve(numberOfBits);
            this.countBits = numberOfBits;
            InitMask();
            SetAllBits(set);
        }

        #region IList<int> Members

        /// <summary>
        /// Determina o índice do primeiro valor igual ao argumento.
        /// </summary>
        /// <param name="item">O valor a ser procurado.</param>
        /// <returns>
        /// O índice do valor.
        /// </returns>
        public int IndexOf(int item)
        {
            int result = -1;
            int pointer = 0;
            while (result == -1 && pointer < this.elements.Count)
            {
                result = FindFirstBitInVariable(item, this.elements[pointer]);
            }
            return result;
        }

        /// <summary>
        /// Inserte um valor no índice especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O valor..</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Se o índice não se encontrar nos limites da colecção.
        /// </exception>
        public void Insert(int index, int item)
        {
            if (index < 0 || index > countBits)
            {
                throw new ArgumentOutOfRangeException("Index must be within the bounds of the List." +
                Environment.NewLine + "Parameter name: index");
            }
            int minPointer = index / bitNumber;
            int rem = index % bitNumber;
            ++countBits;
            if (countBits / bitNumber >= elements.Count)
            {
                elements.Add(0L);
            }
            int pointer = elements.Count - 1;
            while (minPointer < pointer)
            {
                ulong lastBitInPrevious = maskPositionBits[bitNumber - 1] & elements[pointer - 1];
                elements[pointer] = elements[pointer] << 1;
                if (lastBitInPrevious == 0)
                {
                    elements[pointer] = elements[pointer] & ~(1UL);
                }
                else
                {
                    elements[pointer] = elements[pointer] | 1UL;
                }
                --pointer;
            }
            elements[pointer] = RotateVariableBetweenIndices(rem, bitNumber - 1, elements[pointer], 1, false);
            if (item == 0)
            {
                elements[pointer] = elements[pointer] & ~maskPositionBits[rem];
            }
            else
            {
                elements[pointer] = elements[pointer] | maskPositionBits[rem];
            }
        }

        /// <summary>
        /// Remove o objecto que se encontra no índice especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        ///<exception cref="System.ArgumentOutOfRangeException">
        /// Se o índice não se encontrar nos limites da colecção.
        /// </exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= countBits)
            {
                throw new ArgumentOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the collection." +
                Environment.NewLine + "Parameter name: index");
            }

            int pointer = index / bitNumber;
            int rem = index % bitNumber;
            elements[pointer] = this.RotateVariableBetweenIndices(rem, bitNumber - 1, this.elements[pointer], 1, true);
            for (int i = pointer + 1; i < elements.Count; ++i)
            {
                ulong firstBitInCurrent = this.maskPositionBits[0] & this.elements[pointer];
                if (firstBitInCurrent == 0)
                {
                    this.elements[i - 1] = this.elements[i - 1] & (maskPositionBits[bitNumber - 1] - 1);
                }
                else
                {
                    this.elements[i - 1] = this.elements[i - 1] | maskPositionBits[bitNumber - 1];
                }

                elements[i] = elements[i] << 1;
            }

            --countBits;
            this.UpdateList();
        }

        /// <summary>
        /// Obtém o bit da lista na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da posição na lista.</param>
        /// <returns>O bit que se encontra na posição.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Se o índice não se encontrar nos limites da colecção.
        /// </exception>
        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= countBits)
                {
                    throw new ArgumentOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the collection." +
                    Environment.NewLine + "Parameter name: index");
                }
                int pointer = index / bitNumber;
                int rem = index % bitNumber;
                return GetBitFromVariable(this.elements[pointer], rem);
            }
            set
            {
                if (index < 0 || index >= countBits)
                {
                    throw new ArgumentOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the collection." +
                    Environment.NewLine + "Parameter name: index");
                }
                int pointer = index / bitNumber;
                int rem = index % bitNumber;
                this.elements[pointer] = SetBitInVariable(this.elements[pointer], value, rem);
            }
        }

        #endregion

        #region ICollection<int> Members

        /// <summary>
        /// Adiciona um bit à lista de bits. Se o valor for zero, adiciona o bit zero. Caso contrário, adiciona o
        /// bit 1.
        /// </summary>
        /// <param name="item">O bit a ser adicionado.</param>
        public void Add(int item)
        {
            long elementCounter = countBits % bitNumber;
            if (elementCounter == 0)
            {
                this.elements.Add(0);
            }
            if (item != 0)
            {
                this.elements[this.elements.Count - 1] |= maskPositionBits[elementCounter];
            }
            ++countBits;
        }

        /// <summary>
        /// Limpa todos os valores da lista.
        /// </summary>
        public void Clear()
        {
            this.elements.Clear();
            this.countBits = 0;
        }

        /// <summary>
        /// Determina se a colecção contém o valor especificado.
        /// </summary>
        /// <param name="item">O objecto a ser localizado na colecção.</param>
        /// <returns>
        /// Verdadeiro se a colecção contiver o valor especificado e falso caso contrário.
        /// </returns>
        public bool Contains(int item)
        {
            if (item == 0)
            {
                foreach (ulong val in this.elements)
                {
                    if (val != ulong.MaxValue)
                    {
                        return true;
                    }
                }

                return false;
            }
            else if (item == 1)
            {
                foreach (ulong val in this.elements)
                {
                    if (val != 0)
                    {
                        return true;
                    }
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// Copia o conteúdo da lista para um vector de inteiros.
        /// </summary>
        /// <param name="array">O vector de inteiros.</param>
        /// <param name="arrayIndex">O índice do vector de destino a partir do qual é iniciada a cópia.</param>
        /// <exception cref="ArgumentNullException">Se o vector for nulo.</exception>
        /// <exception cref="ArgumentException">Se o tamanho do vector não for suficiente.</exception>
        public void CopyTo(int[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(
                    "Value cannot be null." + Environment.NewLine + "Parameter name: dest");
            }

            int numberOfOperations = countBits + arrayIndex;
            if (numberOfOperations > array.Length)
            {
                throw new ArgumentException(
                    "Destination array was not long enough. Check destIndex and length, and the array's lower bounds.");
            }

            int bitPointer = 0;
            int variablePointer = 0;
            int arrayPointer = arrayIndex;
            while (variablePointer < elements.Count && arrayPointer < countBits)
            {
                array[arrayPointer] = (this.elements[variablePointer] & maskPositionBits[bitPointer]) == 0UL ? 0 : 1;
                ++arrayPointer;
                ++bitPointer;
                if (bitPointer >= bitNumber)
                {
                    bitPointer = 0;
                    ++variablePointer;
                }
            }
        }

        /// <summary>
        /// Copia o conteúdo da lista para um vector de bits.
        /// </summary>
        /// <param name="array">O vector de bits.</param>
        /// <param name="arrayIndex">O índice do vector de destino a partir do qual é iniciada a cópia.</param>
        /// <exception cref="ArgumentNullException">Se o vector for nulo.</exception>
        /// <exception cref="ArgumentException">Se o tamanho do vector não for suficiente.</exception>
        public void CopyTo(BitArray array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else
            {
                var count = this.countBits + arrayIndex;
                if (count > array.Count)
                {
                    throw new ArgumentException("The length of array is no long enough.");
                }
                else
                {
                    int bitPointer = 0;
                    int variablePointer = 0;
                    int arrayPointer = arrayIndex;
                    while (variablePointer < this.elements.Count && arrayPointer < countBits)
                    {
                        array[arrayPointer] = (this.elements[variablePointer] & maskPositionBits[bitPointer]) == 0UL ?
                            false : true;
                        ++arrayPointer;
                        ++bitPointer;
                        if (bitPointer >= bitNumber)
                        {
                            bitPointer = 0;
                            ++variablePointer;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o número de elementos contidos na colecção.
        /// </summary>
        /// <returns>O número de elementos contidos na colecção..</returns>
        public int Count
        {
            get
            {
                return this.countBits;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a colecção é apenas de leitura.
        /// </summary>
        /// <returns>Sempre falso.</returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Remove a primeira ocorreência do item especificado.
        /// </summary>
        /// <param name="item">O objecto a ser removido.</param>
        /// <returns>
        /// Verdadeiro caso o objecto seja removido com sucesso e falso caso contrário.
        /// </returns>
        public bool Remove(int item)
        {
            if (countBits <= 0)
            {
                return false;
            }

            ulong comparer = item != 0 ? 0L : ulong.MaxValue;
            int pointer = 0;
            int bitPointer = 0;
            while (pointer < this.elements.Count - 1 && this.elements[pointer] == comparer)
            {
                ++pointer;
                bitPointer += bitNumber;
            }

            if (pointer == this.elements.Count)
            {
                return false;
            }

            int index = FindFirstBitInVariable(item, this.elements[pointer]);
            bitPointer += index;
            if (bitPointer >= countBits)
            {
                return false;
            }

            elements[pointer] = RotateVariableBetweenIndices(index, bitNumber - 1, this.elements[pointer], 1, true);
            for (int i = pointer + 1; i < elements.Count; ++i)
            {
                ulong firstBitInCurrent = this.maskPositionBits[0] & this.elements[pointer];
                if (firstBitInCurrent == 0)
                {
                    this.elements[pointer - 1] = this.elements[pointer - 1] & (maskPositionBits[bitNumber - 1] - 1);
                }
                else
                {
                    this.elements[pointer - 1] = this.elements[pointer - 1] | maskPositionBits[bitNumber - 1];
                }

                elements[pointer] = elements[pointer] << 1;
            }

            --countBits;
            this.UpdateList();
            return true;
        }

        #endregion

        #region IEnumerable<int> Members

        /// <summary>
        /// Retorna um numerador que intera ao longo da colecção.
        /// </summary>
        /// <returns>
        /// O enumerador.
        /// </returns>
        public IEnumerator<int> GetEnumerator()
        {
            return new Enumerator(ref this.elements, ref this.countBits, bitNumber, maskPositionBits);
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Obtém o enumerador não genérico.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Override members

        /// <summary>
        /// Constrói uma respresentação textual da instância corrente.
        /// </summary>
        /// <returns>
        /// A representação textual.
        /// </returns>
        public override string ToString()
        {
            return "[" + GetBitsFromLongArray(elements.ToArray(), countBits) + "]";
        }

        /// <summary>
        /// Determina se o objecto especificado é igual à instância corrente.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>
        /// Verdadeiro caso o objecto seja igual e falso caso contrário.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj == null)
            {
                return false;
            }
            else
            {
                var innerObj = obj as BitList;
                return this.Equals(innerObj);
            }
        }

        /// <summary>
        /// Retorna um código confuso para a instância.
        /// </summary>
        /// <returns>
        /// O código confuso útil em alguns algoritmos.
        /// </returns>
        public override int GetHashCode()
        {
            var result = 19UL;
            if (this.countBits > 0)
            {
                var length = this.elements.Count - 1;
                for (int i = 0; i < length; ++i)
                {
                    result ^= this.elements[i] * 17;
                }

                var lastElement = this.elements[length];
                var rem = this.countBits % bitNumber;
                if (rem > 0)
                {
                    lastElement = lastElement & (ulong.MaxValue >> (bitNumber - rem));
                }

                result ^= lastElement * 17;
            }

            return result.GetHashCode();
        }

        #endregion

        /// <summary>
        /// Adiciona um conjunto de bits à lista.
        /// </summary>
        /// <param name="range">O conjunto de bits a ser adicionado.</param>
        /// <exception cref="ArgumentNullException">Se o conjunto de bits for nulo.</exception>
        public void AddRange(BitList range)
        {
            if (range == null)
            {
                throw new ArgumentNullException("range");
            }
            else
            {
                int rem = this.countBits % BitList.bitNumber;
                int quo = this.countBits / BitList.bitNumber;
                ++quo;

                this.elements.AddRange(range.elements);
                if (rem != 0)
                {
                    for (int i = quo; i < this.elements.Count; ++i)
                    {
                        ulong tempMask = this.elements[i] << rem;
                        this.elements[i - 1] = (this.elements[i - 1] & (maskPositionBits[rem] - 1)) | tempMask;
                        this.elements[i] = this.elements[i] >> (bitNumber - rem);
                    }
                }

                this.countBits += range.countBits;
                this.UpdateList();
            }
        }

        /// <summary>
        /// Adiciona um conjunto de bits a partir de um vector de longos.
        /// </summary>
        /// <param name="bitsArray">O vector de longos.</param>
        /// <param name="bitsNumber">O número de bits a serem adicionados.</param>
        /// <exception cref="ArgumentNullException">Se o conjunto de bits for nulo.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número de bits for negativo ou não for inferior ao número de bits no conjunto.
        /// </exception>
        public void AddRange(ulong[] bitsArray, int bitsNumber)
        {
            if (bitsArray == null)
            {
                throw new ArgumentNullException("bitsArray");
            }
            else if (bitsNumber < 0 || bitsNumber >= bitsArray.Length * (sizeof(ulong) * BitList.byteNumber))
            {
                throw new ArgumentOutOfRangeException("bitsNumber");
            }
            else
            {
                int rem = this.countBits % BitList.bitNumber;
                int quo = this.countBits / BitList.bitNumber;
                ++quo;

                this.elements.AddRange(bitsArray);
                if (rem != 0)
                {
                    for (int i = quo; i < this.elements.Count; ++i)
                    {
                        ulong tempMask = this.elements[i] << rem;
                        this.elements[i - 1] = (this.elements[i - 1] & (maskPositionBits[rem] - 1)) | tempMask;
                        this.elements[i] = this.elements[i] >> (bitNumber - rem);
                    }
                }

                this.countBits += bitsNumber;
                this.UpdateList();
            }
        }

        /// <summary>
        /// Insere uma lista de bits a partir de uma determinada posição.
        /// </summary>
        /// <param name="index">O índice de inserção.</param>
        /// <param name="range">O conjunto de bits a ser inserido.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o índice for negativo ou não for inferior ao número de bits no conjunto.
        /// </exception>
        public void InsertRange(int index, BitList range)
        {
            if (index < 0 || index > countBits)
            {
                throw new ArgumentOutOfRangeException("Index must be within the bounds of the List." +
                Environment.NewLine + "Parameter name: index");
            }
            BitList tempBitList = this.GetSubBitList(0, index);
            tempBitList.AddRange(range);
            tempBitList.AddRange(this.GetSubBitList(index, this.countBits - index));
            this.elements = tempBitList.elements;
            this.countBits = tempBitList.countBits;
        }

        /// <summary>
        /// Insere uma lista de bits a partir de uma determinada posição - obsoleta.
        /// </summary>
        /// <param name="index">O índice de inserção.</param>
        /// <param name="range">O conjunto de bits a ser inserido.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o índice for negativo ou não for inferior ao número de bits no conjunto.
        /// </exception>
        [Obsolete("Versão não funcional", true)]
        public void InsertRangeOtherVersion(int index, BitList range)
        {
            if (index < 0 || index > countBits)
            {
                throw new ArgumentOutOfRangeException("Index must be within the bounds of the List." +
                Environment.NewLine + "Parameter name: index");
            }
            if (range.countBits == 0)
            {
                return;
            }
            int indexQuo = index / countBits;
            int indexRem = index % countBits;
            int indexPointer = 0;
            int variablePointer = 0;
            if (indexRem == 0)
            {
                while (indexPointer < range.countBits)
                {
                    this.elements.Insert(indexQuo++, range.elements[variablePointer]);
                    ++variablePointer;
                    indexRem += bitNumber;
                    indexPointer += bitNumber;
                }
            }
            else
            {
                ulong indexVariableResidue = (~(maskPositionBits[indexRem] - 1) & this.elements[indexQuo]) >> indexRem;
                ulong temp = (~(maskPositionBits[bitNumber - indexRem] - 1) & range.elements[0]) << indexRem;
                this.elements[indexQuo] = temp | (this.elements[indexQuo] & (maskPositionBits[indexRem] - 1));

                while (indexPointer < range.countBits)
                {
                    this.elements.Insert(indexQuo++, range.elements[variablePointer]);

                    ++variablePointer;
                    indexRem += bitNumber;
                    indexPointer += bitNumber;
                }
            }
            this.countBits += range.countBits;
        }

        /// <summary>
        /// Retorna uma sub-lista de bits.
        /// </summary>
        /// <param name="startIndex">O índice de início.</param>
        /// <param name="numberOfBits">O número de bits da sub-lista.</param>
        /// <returns>A sub-lista.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o índice de início não se encontrar nos limites da lista.
        /// </exception>
        public BitList GetSubBitList(int startIndex, int numberOfBits)
        {
            if (startIndex + numberOfBits > this.countBits)
            {
                numberOfBits = this.countBits - startIndex;
            }
            if (numberOfBits < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "Start index must be inside the size of bit list and number of bits must be non negative.");
            }

            return GetUnckeckedSubBitList(startIndex, numberOfBits);
        }

        /// <summary>
        /// Verifica se todos os bits da lista estão a zero.
        /// </summary>
        /// <returns>Verdadeiro caso os bits da lista estejam a zero e falso caso contrário.</returns>
        public bool IsAllZero()
        {
            var lastIndex = this.elements.Count - 1;
            var count = this.countBits;
            for (int i = 0; i < lastIndex; ++i)
            {
                if (this.elements[i] != 0)
                {
                    return false;
                }

                count -= bitNumber;
            }

            var last = this.elements[lastIndex];
            if (count < bitNumber)
            {
                var mask = this.maskPositionBits[count];
                return (last & mask) == 0;
            }
            else
            {
                return last == 0;
            }
        }

        /// <summary>
        /// Verifica se todos os bits da lista estão a um.
        /// </summary>
        /// <returns>Verdadeiro caso os bits da lista estejam a um e falso caso contrário.</returns>
        public bool IsAllOne()
        {
            var lastIndex = this.elements.Count - 1;
            var count = this.countBits;
            for (int i = 0; i < lastIndex; ++i)
            {
                if (this.elements[i] != ulong.MaxValue)
                {
                    return false;
                }

                count -= bitNumber;
            }

            var last = this.elements[lastIndex];
            if (count < bitNumber)
            {
                var mask = this.maskPositionBits[count];
                return (last & mask) == mask;
            }
            else
            {
                return last == ulong.MaxValue;
            }
        }

        /// <summary>
        /// Permite verificar se duas listas são iguais.
        /// </summary>
        /// <param name="other">A outra lista.</param>
        /// <returns>Verdadeiro se a lista for igual e falso caso contrário.</returns>
        public bool Equals(BitList other)
        {
            if (other == null)
            {
                return false;
            }
            else if (this.countBits == other.countBits)
            {
                if (this.countBits == 0)
                {
                    return true;
                }
                else
                {
                    var length = this.elements.Count - 1;
                    for (int i = 0; i < length; ++i)
                    {
                        if (this.elements[i] != other.elements[i])
                        {
                            return false;
                        }
                    }

                    // Último elemento
                    var currentLastElement = this.elements[length];
                    var otherLastElement = other.elements[length];
                    var rem = this.countBits % bitNumber;
                    if (rem > 0)
                    {
                        currentLastElement = currentLastElement & (ulong.MaxValue >> (bitNumber - rem));
                        otherLastElement = otherLastElement & (ulong.MaxValue >> (bitNumber - rem));
                    }

                    return currentLastElement == otherLastElement;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Permite comparar duas listas para verificar qual é a maior quando encarada como sendo um número
        /// binário no qual o primeiro elemento corresopnde à menor potência.
        /// </summary>
        /// <example>
        /// A lista "1001" representa o número 9 que é menor do que "1010" que representa o número 10.
        /// </example>
        /// <param name="other">A lista a ser comparada.</param>
        /// <returns>Os valores 1, 0, -1 caso a lista actual seja maior, igual ou menor do que a 
        /// lista especificada, respectivamente.
        /// </returns>
        /// <exception cref="ArgumentNullException">Se a lista for nula.</exception>
        public int CompareTo(BitList other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.countBits == 0)
            {
                if (other.countBits == 0)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else if (other.countBits == 0)
            {
                return 1;
            }
            else
            {
                var comparer = Comparer<ulong>.Default;
                var i = 0;
                var j = 0;
                var currentLastIndex = this.elements.Count - 1;
                var otherLastIndex = other.elements.Count - 1;
                while (i < currentLastIndex && j < otherLastIndex)
                {
                    var compareValue = comparer.Compare(this.elements[i], other.elements[j]);
                    if (compareValue != 0)
                    {
                        return compareValue;
                    }

                    ++i;
                    ++j;
                }

                if (currentLastIndex == otherLastIndex)
                {
                    var firstRemainder = this.countBits % bitNumber;
                    var secondRemainder = other.countBits % bitNumber;

                    var currentLastValue = this.elements[currentLastIndex];
                    if (firstRemainder != 0)
                    {
                        currentLastValue = currentLastValue & (ulong.MaxValue >> (bitNumber - firstRemainder));
                    }

                    var otherLastValue = other.elements[currentLastIndex];
                    if (secondRemainder != 0)
                    {
                        otherLastValue = otherLastValue & (ulong.MaxValue >> (bitNumber - firstRemainder));
                    }

                    return comparer.Compare(currentLastValue, otherLastValue);
                }
                else if (currentLastIndex < otherLastIndex)
                {
                    j = otherLastIndex - 1;
                    while (j > currentLastIndex)
                    {
                        if (other.elements[j] != 0)
                        {
                            return -1;
                        }

                        --j;
                    }

                    var firstRemainder = this.countBits % bitNumber;
                    var currentLastValue = this.elements[currentLastIndex];
                    if (firstRemainder != 0)
                    {
                        currentLastValue = currentLastValue & (ulong.MaxValue >> (bitNumber - firstRemainder));
                    }

                    var result = comparer.Compare(currentLastValue, other.elements[j]);
                    if (result == 0)
                    {
                        var secondRemainder = other.countBits & bitNumber;
                        var otherLastValue = other.elements[otherLastIndex];
                        if (secondRemainder != 0)
                        {
                            otherLastValue = otherLastValue & (ulong.MaxValue >> (bitNumber - secondRemainder));
                        }

                        if (otherLastValue == 0)
                        {
                            return 0;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
                else
                {
                    i = currentLastIndex - 1;
                    while (i > currentLastIndex)
                    {
                        if (this.elements[i] != 0)
                        {
                            return 1;
                        }

                        --i;
                    }

                    var secondRemainder = other.countBits % bitNumber;
                    var otherLastValue = other.elements[otherLastIndex];
                    if (secondRemainder != 0)
                    {
                        otherLastValue = otherLastValue & (ulong.MaxValue >> (bitNumber - secondRemainder));
                    }

                    var result = comparer.Compare(otherLastValue, this.elements[i]);
                    if (result == 0)
                    {
                        var firstRemainder = this.countBits & bitNumber;
                        var currentLastValue = this.elements[currentLastIndex];
                        if (secondRemainder != 0)
                        {
                            currentLastValue = currentLastValue & (ulong.MaxValue >> (bitNumber - firstRemainder));
                        }

                        if (currentLastValue == 0)
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
            }
        }

        #region Operações Lógicas

        /// <summary>
        /// Aplica o operador "ou" entre os "bits" da lista actual e os da lista especificada.
        /// </summary>
        /// <param name="other">A lista de entrada.</param>
        /// <returns>O resultado da operação.</returns>
        /// <exception cref="ArgumentNullException">Se a lista for nula.</exception>
        public BitList BitListOr(BitList other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else
            {
                var result = new BitList();
                result.countBits = this.countBits < other.countBits ? other.countBits : this.countBits;
                var i = 0;
                var j = 0;
                while (i < this.elements.Count && j < other.elements.Count)
                {
                    result.elements.Add(this.elements[i] | other.elements[j]);
                    ++i;
                    ++j;
                }

                while (i < this.elements.Count)
                {
                    result.elements.Add(this.elements[i] | 0);
                    ++i;
                }

                while (j < other.elements.Count)
                {
                    result.elements.Add(0 | other.elements[j]);
                    ++j;
                }

                return result;
            }
        }

        /// <summary>
        /// Aplica o operador "e" entre os "bits" da lista actual e os da lista especificada.
        /// </summary>
        /// <param name="other">A lista de entrada.</param>
        /// <returns>O resultado da operação.</returns>
        /// <exception cref="ArgumentNullException">Se a lista for nula.</exception>
        public BitList BitListAnd(BitList other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else
            {
                var result = new BitList();
                result.countBits = this.countBits < other.countBits ? other.countBits : this.countBits;
                var i = 0;
                var j = 0;
                while (i < this.elements.Count && j < other.elements.Count)
                {
                    result.elements.Add(this.elements[i] & other.elements[j]);
                    ++i;
                    ++j;
                }

                while (i < this.elements.Count)
                {
                    result.elements.Add(this.elements[i] & 0);
                    ++i;
                }

                while (j < other.elements.Count)
                {
                    result.elements.Add(0 & other.elements[j]);
                    ++j;
                }

                return result;
            }
        }

        /// <summary>
        /// Aplica o operador "ou exclusivo" entre os "bits" da lista actual e os da lista especificada.
        /// </summary>
        /// <param name="other">A lista de entrada.</param>
        /// <returns>O resultado da operação.</returns>
        /// <exception cref="ArgumentNullException">Se a lista for nula.</exception>
        public BitList BitListXor(BitList other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else
            {
                var result = new BitList();
                result.countBits = this.countBits < other.countBits ? other.countBits : this.countBits;
                var i = 0;
                var j = 0;
                while (i < this.elements.Count && j < other.elements.Count)
                {
                    result.elements.Add(this.elements[i] ^ other.elements[j]);
                    ++i;
                    ++j;
                }

                while (i < this.elements.Count)
                {
                    result.elements.Add(this.elements[i] ^ 0);
                    ++i;
                }

                while (j < other.elements.Count)
                {
                    result.elements.Add(0 ^ other.elements[j]);
                    ++j;
                }

                return result;
            }
        }

        /// <summary>
        /// Aplica o operador "negação" aos "bits" da lista actual.
        /// </summary>
        /// <returns>O resultado da operação.</returns>
        /// <exception cref="ArgumentNullException">Se a lista for nula.</exception>
        public BitList BitListNot()
        {
            var result = new BitList();
            result.countBits = this.countBits;
            if (result.countBits > 0)
            {
                var lastIndex = this.elements.Count - 1;
                for (int i = 0; i < lastIndex; ++i)
                {
                    result.elements.Add(~this.elements[i]);
                }

                var lastElement = this.elements[lastIndex];
                var rem = this.countBits % bitNumber;
                if (rem == 0)
                {
                    result.elements.Add(lastElement);
                }
                else
                {
                    lastElement = ~lastElement;
                    result.elements.Add(lastElement);
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Permite ler uma lista de bits a partir de uma sequência de 0 e 1.
        /// </summary>
        /// <param name="binaryText">A sequência de 0 e 1.</param>
        /// <returns>A lista de bits.</returns>
        /// <exception cref="UtilitiesException">Se a sequência contiver texto inválido.</exception>
        public static BitList ReadBinary(string binaryText)
        {
            var result = new BitList();
            if (!string.IsNullOrWhiteSpace(binaryText))
            {
                var innerText = binaryText.Trim();
                for (int i = 0; i < innerText.Length; ++i)
                {
                    var currentChar = innerText[i];
                    if (currentChar == '0')
                    {
                        result.Add(0);
                    }
                    else if (currentChar == '1')
                    {
                        result.Add(1);
                    }
                    else
                    {
                        throw new UtilitiesException(string.Format("Unexpected char: '{0}'.", currentChar));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Permite fazer a leitura da lista de bits a partir de texto que representa
        /// um número inteiro.
        /// </summary>
        /// <param name="numericText">O texto que representa um número inteiro positivo.</param>
        /// <returns>A lista.</returns>
        /// <exception cref="UtilitiesException">Se o texto for inválido.</exception>
        public static BitList ReadNumeric(string numericText)
        {
            var result = new BitList();
            if (!string.IsNullOrWhiteSpace(numericText))
            {
                var innerText = numericText.Trim().TrimStart('0');
                while (innerText != string.Empty)
                {
                    var auxText = string.Empty;
                    var value = 0;
                    for (int i = 0; i < innerText.Length; ++i)
                    {
                        var currentChar = innerText[i];
                        var currentValue = (int)char.GetNumericValue(currentChar);
                        if (currentValue < 0)
                        {
                            throw new UtilitiesException(string.Format(
                                "Char '{0}' seems not to be a digit.",
                                currentChar));
                        }
                        else
                        {
                            value = 10 * value + currentValue;
                            auxText += (value >> 1);
                            value = (value & 1);
                        }
                    }

                    result.Add(value);
                    innerText = auxText.TrimStart('0');
                }

                if (result.countBits == 0)
                {
                    result.Add(0);
                }
            }

            return result;
        }

        #region Funções Privadas

        /// <summary>
        /// Obtém uma sublista de bits sem efectuar a verificação das restrições.
        /// </summary>
        /// <param name="startIndex">O índice de partida.</param>
        /// <param name="numberOfBits">O número de bits.</param>
        /// <returns>A sublista procurada.</returns>
        private BitList GetUnckeckedSubBitList(int startIndex, int numberOfBits)
        {
            BitList bitlist = new BitList();
            List<ulong> temporaryConstructionList = new List<ulong>();
            int startRemainder = startIndex % bitNumber;
            int startQuo = startIndex / bitNumber;
            int numberOfBitsQuo = (startRemainder + numberOfBits) / bitNumber + 1;
            if (startRemainder == 0)
            {
                for (int i = startQuo; i < numberOfBitsQuo; ++i)
                {
                    bitlist.elements.Add(this.elements[i]);
                }
            }
            else
            {
                BitList tempBitList = new BitList();
                tempBitList.countBits = 0;
                ulong valueToAdd = (~(maskPositionBits[startRemainder] - 1) & this.elements[startQuo]) >> startRemainder;
                tempBitList.elements.Add(valueToAdd);
                tempBitList.countBits = bitNumber - startRemainder;
                bitlist.AddRange(tempBitList);
                for (int i = startQuo + 1; i < numberOfBitsQuo; ++i)
                {
                    tempBitList = new BitList();
                    tempBitList.elements.Add(this.elements[i]);
                    tempBitList.countBits = bitNumber;
                    bitlist.AddRange(tempBitList);
                }
            }
            bitlist.countBits = numberOfBits;
            bitlist.UpdateList();
            return bitlist;
        }

        /// <summary>
        /// Inicializa as máscaras.
        /// </summary>
        private void InitMask()
        {
            ulong powerOfTwo = 1;
            maskPositionBits = new ulong[bitNumber];
            maskPositionBits[0] = powerOfTwo;
            for (int i = 1; i < bitNumber; ++i)
            {
                powerOfTwo = powerOfTwo << 1;
                maskPositionBits[i] = powerOfTwo;
            }
        }

        /// <summary>
        /// Actualiza a lista após algum conjunto de operações, eliminando os termos remanescentes.
        /// </summary>
        private void UpdateList()
        {
            if (countBits / bitNumber < elements.Count - 1)
            {
                int numberOfVariablesNeeded = countBits / bitNumber + 1;
                while (this.elements.Count > numberOfVariablesNeeded)
                {
                    this.elements.RemoveAt(this.elements.Count - 1);
                }
            }
        }

        /// <summary>
        /// Roda os bits compreendidos entre os dois índices inclusivé.
        /// </summary>
        /// <param name="index1">O índice menor.</param>
        /// <param name="index2">O índice maior.</param>
        /// <param name="variable">A variável a ser rodada.</param>
        /// <param name="rotationNumber">A magnitude da rotação.</param>
        /// <param name="direction">Verdadeiro caso seja para rodar à esquerda e falso para rodar à direita.</param>
        /// <returns>A variável com os respectivos bits rodados.</returns>
        private ulong RotateVariableBetweenIndices(int index1, int index2, ulong variable, int rotationNumber, bool direction)
        {
            if (index1 > index2)
            {
                return variable;
            }

            ulong mask = ~(maskPositionBits[index1] - 1);
            if (index2 < bitNumber - 1)
            {
                mask &= (maskPositionBits[index2] - 1);
            }
            else
            {
                mask &= ulong.MaxValue;
            }

            ulong result = variable & mask;
            if (direction)
            {
                result = result >> rotationNumber;
            }
            else
            {
                result = result << rotationNumber;
            }

            return (variable & ~mask) | (result & mask);
        }

        /// <summary>
        /// Procura pelo primeiro bit na variável.
        /// </summary>
        /// <param name="bitToFind">O valor do bit pretendido.</param>
        /// <param name="variable">A variável sobre a qual se efectua a pesquisa.</param>
        /// <returns>O índice da primeira ocorrência e -1 caso contrário.</returns>
        private int FindFirstBitInVariable(int bitToFind, ulong variable)
        {
            for (int i = 0; i < bitNumber; ++i)
            {
                ulong temporary = variable & maskPositionBits[i];
                if (bitToFind == 0 && temporary == 0)
                {
                    return i;
                }
                if (bitToFind != 0 && temporary != 0)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Estabelece o valor do "bit" na variável dada pelo respectivo número.
        /// </summary>
        /// <param name="variable">A variável.</param>
        /// <param name="bitToSet">O valor do "bit".</param>
        /// <param name="index">A posição a atribuir.</param>
        /// <returns>A variável que resulta aquando da atribuição do valor do "bit" na respectiva posição.</returns>
        private ulong SetBitInVariable(ulong variable, int bitToSet, int index)
        {
            if (index < 0 || index > bitNumber)
            {
                return variable;
            }
            ulong result = variable;
            if (bitToSet == 0)
            {
                result = result & ~maskPositionBits[index];
            }
            else
            {
                result = result | maskPositionBits[index];
            }
            return result;
        }

        /// <summary>
        /// Obtém o bit de uma variável sem efectuar verificação de limites.
        /// </summary>
        /// <param name="variable">A variável.</param>
        /// <param name="index">O índice da posição do bit.</param>
        /// <returns>O bit.</returns>
        private int GetBitFromVariable(ulong variable, int index)
        {
            return (maskPositionBits[index] & variable) == 0 ? 0 : 1;
        }

        /// <summary>
        /// Obtém uma representação textual de todos os bits no vector.
        /// </summary>
        /// <param name="array">O vector.</param>
        /// <param name="countBits">O número de bits válidos contidos no vector.</param>
        /// <returns>A representação textual do vector.</returns>
        private string GetBitsFromLongArray(ulong[] array, long countBits)
        {
            if (countBits / bitNumber > array.Length)
            {
                return "Bit overflow";
            }

            StringBuilder resultBuilder = new StringBuilder();
            for (long i = 0; i < countBits; ++i)
            {
                long quo = i / bitNumber;
                long rem = i % bitNumber;
                resultBuilder.Append((array[quo] & maskPositionBits[rem]) != 0 ? 1 : 0);
            }
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Atribui o valor especificado a todos os "bits" da lista.
        /// </summary>
        /// <param name="set">O valor do "bit" a ser atribuído.</param>
        private void SetAllBits(bool set)
        {
            if (countBits == 0)
            {
                return;
            }
            int variableIndex = countBits / bitNumber;
            int indexInLastVariable = countBits % bitNumber;
            if (indexInLastVariable == 0)
            {
                variableIndex--;
            }

            if (set)
            {
                for (int i = 0; i < variableIndex + 1 && i < this.elements.Count; ++i)
                {
                    this.elements[i] = ulong.MaxValue;
                }
                for (int i = this.elements.Count; i < variableIndex + 1; ++i)
                {
                    this.elements.Add(ulong.MaxValue);
                }
            }
            else
            {
                for (int i = 0; i < variableIndex + 1 && i < this.elements.Count; ++i)
                {
                    this.elements[i] = 0;
                }
                for (int i = this.elements.Count; i < variableIndex + 1; ++i)
                {
                    this.elements.Add(0);
                }
            }
        }

        /// <summary>
        /// Reserva o espaço especificado.
        /// </summary>
        /// <param name="capacity">O espaço a ser reservado.</param>
        private void Reserve(int capacity)
        {
            int listCapacity = capacity / bitNumber;
            if (capacity % bitNumber != 0)
            {
                ++listCapacity;
            }
            elements = new List<ulong>(listCapacity);
        }

        /// <summary>
        /// Descarrega todos os bits do vector.
        /// </summary>
        /// <param name="variable">A variável que contém os bits.</param>
        /// <returns>A representação textual de todos os bits.</returns>
        private string DumpVariable(ulong variable)
        {
            StringBuilder resultBuilder = new StringBuilder();
            for (int i = 0; i < bitNumber; ++i)
            {
                resultBuilder.Append((variable & maskPositionBits[i]) != 0 ? 1 : 0);
            }
            return resultBuilder.ToString();
        }

        #endregion Funções Privadas

        /// <summary>
        /// Implementa um enumerador para a lista.
        /// </summary>
        private class Enumerator : IEnumerator<int>
        {
            #region fields
            /// <summary>
            /// O número de bits na variável.
            /// </summary>
            int bitNumber;

            /// <summary>
            /// A máscara para cada posição de um inteiro.
            /// </summary>
            ulong[] maskPositionBits;

            /// <summary>
            /// Os elementos da lista.
            /// </summary>
            List<ulong> elements;

            /// <summary>
            /// O número de bits na lista.
            /// </summary>
            int countBits;

            /// <summary>
            /// Apontador para cada um dos elementos.
            /// </summary>
            int variablePointer;

            /// <summary>
            /// Apontador para cada bit na variável.
            /// </summary>
            int bitPointer;

            /// <summary>
            /// Apontador geral.
            /// </summary>
            int elementPointer;

            #endregion

            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="Enumerator"/>.
            /// </summary>
            /// <param name="elements">O contentor da lista.</param>
            /// <param name="countBits">O número de bits na lista.</param>
            /// <param name="bitNumber">O número de bits na variável.</param>
            /// <param name="maskPositionBits">A máscara para cad posição num inteiro.</param>
            public Enumerator(ref List<ulong> elements, ref int countBits, int bitNumber, ulong[] maskPositionBits)
            {
                this.elements = elements;
                this.countBits = countBits;
                this.bitNumber = bitNumber;
                this.maskPositionBits = maskPositionBits;
                this.variablePointer = 0;
                this.bitPointer = -1;
                this.elementPointer = -1;
            }

            #region IEnumerator<int> Members

            /// <summary>
            /// Obtém o elemento da colecção apontado pelo enumerador.
            /// </summary>
            /// <returns>O elemento da colecção apontado pelo enumerador..</returns>
            public int Current
            {
                get
                {
                    if (elementPointer < 0 || elementPointer >= countBits)
                    {
                        return 0;
                    }
                    return (maskPositionBits[bitPointer] & this.elements[variablePointer]) == 0 ? 0 : 1;
                }
            }

            #endregion

            #region IDisposable Members

            /// <summary>
            /// Descarta o enumerador.
            /// </summary>
            public void Dispose()
            {
                this.elements = null;
            }

            #endregion

            #region IEnumerator Members

            /// <summary>
            /// Obtém o elemento da colecção apontado pelo enumerador.
            /// </summary>
            /// <returns>O elemento da colecção apontado pelo enumerador..</returns>
            object System.Collections.IEnumerator.Current
            {
                get
                {
                    if (elementPointer < 0 || elementPointer >= countBits)
                    {
                        return 0;
                    }
                    return (maskPositionBits[bitPointer] & this.elements[variablePointer]) == 0 ? 0 : 1;
                }
            }

            /// <summary>
            /// Avança o enumerador para o próximo elemento da colecção.
            /// </summary>
            /// <returns>
            /// Verdadeiro caso o enumerador avanve e falos caso se encontre no final da colecção.
            /// </returns>
            public bool MoveNext()
            {
                ++elementPointer;
                ++bitPointer;
                if (bitPointer >= bitNumber)
                {
                    ++variablePointer;
                    bitPointer = 0;
                }
                if (elementPointer >= countBits)
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// Inicializa o enumerador.
            /// </summary>
            public void Reset()
            {
                elementPointer = -1;
                bitPointer = -1;
                variablePointer = 0;
            }

            #endregion
        }
    }
}
