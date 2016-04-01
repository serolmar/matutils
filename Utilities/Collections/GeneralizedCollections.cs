namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define uma lista indexada por inteiros longos sobre
    /// a ordenação habitual.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas da lista.</typeparam>
    public sealed class LongSystemArray<T> : ILongList<T>, IList<T>
    {
        /// <summary>
        /// A ordenação.
        /// </summary>
        private T[] array;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LongSystemArray{T}"/>.
        /// </summary>
        /// <param name="array">A ordenação.</param>
        private LongSystemArray(T[] array)
        {
            this.array = array;
        }

        /// <summary>
        /// Efectua a conversão implícita de uma ordenação habitual para uma
        /// ordenação indexada por inteiros longos.
        /// </summary>
        /// <param name="array">A ordenação a ser convertida.</param>
        /// <returns>O resultado da covnersão.</returns>
        public static implicit operator LongSystemArray<T>(T[] array)
        {
            if (array == null)
            {
                return null;
            }
            else
            {
                return new LongSystemArray<T>(array);
            }
        }

        /// <summary>
        /// Efectua a conversão implícita de uma ordenação indexada por inteiros longos
        /// para uma ordenação habitual.
        /// </summary>
        /// <param name="array">A ordenação indexada por inteiros longos.</param>
        /// <returns>A ordenação habitual.</returns>
        public static implicit operator T[](LongSystemArray<T> array)
        {
            if (null == (object)array)
            {
                return null;
            }
            else
            {
                return array.array;
            }
        }

        /// <summary>
        /// Determina a igualdade entre duas ordenações.
        /// </summary>
        /// <param name="first">A primeira ordenação.</param>
        /// <param name="second">A segunda ordenação.</param>
        /// <returns>Verdadeiro se ambas as ordenações forem iguais e falso caso contrário.</returns>
        public static bool operator ==(
            LongSystemArray<T> first,
            LongSystemArray<T> second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return true;
            }
            else if (null == (object)first)
            {
                return false;
            }
            else if (null == (object)second)
            {
                return false;
            }
            else
            {
                return first.array == second.array;
            }
        }

        /// <summary>
        /// Determina a não igualdade entre duas ordenações.
        /// </summary>
        /// <param name="first">A primeira ordenação.</param>
        /// <param name="second">A segunda ordenação.</param>
        /// <returns>Verdadeiro se ambas as ordenações forem diferentes e falso caso contrário.</returns>
        public static bool operator !=(
            LongSystemArray<T> first,
            LongSystemArray<T> second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return false;
            }
            else if (null == (object)first)
            {
                return true;
            }
            else if (null == (object)second)
            {
                return true;
            }
            else
            {
                return first.array != second.array;
            }
        }

        /// <summary>
        /// Averigua a igualdade entre uma ordenação habitual e uma ordenação indexada por
        /// inteiros longos.
        /// </summary>
        /// <param name="first">O primeiro elemento da comparação.</param>
        /// <param name="second">O segundo elemento da comparação.</param>
        /// <returns>Verdadeiro se ambas as ordenações forem iguais e falso caso contrário.</returns>
        public static bool operator ==(LongSystemArray<T> first, T[] second)
        {
            if (null == (object)first && null == second)
            {
                return true;
            }
            else if (null == (object)first)
            {
                return false;
            }
            else if (null == second)
            {
                return false;
            }
            else
            {
                return first.array == second;
            }
        }

        /// <summary>
        /// Averigua a não igualdade entre uma ordenação habitual e uma ordenação indexada por
        /// inteiros longos.
        /// </summary>
        /// <param name="first">O primeiro elemento da comparação.</param>
        /// <param name="second">O segundo elemento da comparação.</param>
        /// <returns>Verdadeiro se ambas as ordenações forem diferentes e falso caso contrário.</returns>
        public static bool operator !=(LongSystemArray<T> first, T[] second)
        {
            if (null == (object)first && null == second)
            {
                return false;
            }
            else if (null == (object)first)
            {
                return true;
            }
            else if (null == second)
            {
                return true;
            }
            else
            {
                return first.array != second;
            }
        }

        /// <summary>
        /// Averigua a igualdade entre uma ordenação habitual e uma ordenação indexada por
        /// inteiros longos.
        /// </summary>
        /// <param name="first">O primeiro elemento da comparação.</param>
        /// <param name="second">O segundo elemento da comparação.</param>
        /// <returns>Verdadeiro se ambas as ordenações forem iguais e falso caso contrário.</returns>
        public static bool operator ==(T[] first, LongSystemArray<T> second)
        {
            if (null == first && null == (object)second)
            {
                return true;
            }
            else if (null == first)
            {
                return false;
            }
            else if (null == (object)second)
            {
                return false;
            }
            else
            {
                return first == second.array;
            }
        }

        /// <summary>
        /// Averigua a não igualdade entre uma ordenação habitual e uma ordenação indexada por
        /// inteiros longos.
        /// </summary>
        /// <param name="first">O primeiro elemento da comparação.</param>
        /// <param name="second">O segundo elemento da comparação.</param>
        /// <returns>Verdadeiro se ambas as ordenações forem diferentes e falso caso contrário.</returns>
        public static bool operator !=(T[] first, LongSystemArray<T> second)
        {
            if (null == first && null == (object)second)
            {
                return false;
            }
            else if (null == first)
            {
                return true;
            }
            else if (null == (object)second)
            {
                return true;
            }
            else
            {
                return first != second.array;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[int index]
        {
            get
            {
                return this.array[index];
            }
            set
            {
                this.array[index] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[long index]
        {
            get
            {
                return this.array[index];
            }
            set
            {
                this.array[index] = value;
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public int Length
        {
            get
            {
                return this.array.Length;
            }
        }

        /// <summary>
        /// Obtém o tamanho longo do vector.
        /// </summary>
        public long LongLength
        {
            get
            {
                return this.array.LongLength;
            }
        }

        /// <summary>
        /// Obtém o número de elementos na lista.
        /// </summary>
        public int Count
        {
            get
            {
                return this.array.Length;
            }
        }

        /// <summary>
        /// Obtém o número de elementos na lista.
        /// </summary>
        public long LongCount
        {
            get
            {
                return this.array.LongLength;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a lista é apenas de leitura.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public int IndexOf(T item)
        {
            return Array.IndexOf(this.array, item);
        }

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public long LongIndexOf(T item)
        {
            return Array.IndexOf(this.array, item);
        }

        /// <summary>
        /// Insere o item na posição especificada.
        /// </summary>
        /// <param name="index">O índice da posição.</param>
        /// <param name="item">O item a ser inserido.</param>
        public void Insert(int index, T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Insere o item na posição especificada.
        /// </summary>
        /// <param name="index">O índice da posição.</param>
        /// <param name="item">O item a ser inserido.</param>
        public void Insert(long index, T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Remove o item da posição indicada.
        /// </summary>
        /// <param name="index">O índice da posição do item a ser removid.</param>
        public void RemoveAt(int index)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Remove o item da posição indicada.
        /// </summary>
        /// <param name="index">O índice da posição do item a ser removid.</param>
        public void RemoveAt(long index)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Adiciona o item ao final da lista.
        /// </summary>
        /// <param name="item">O item a ser adicionado.</param>
        public void Add(T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Limpa a lista.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException("Collection is read-only.");
        }

        /// <summary>
        /// Retorna um valor que indica se o item se encontra na lista.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>Verdadeiro se o item se encontrar na lista e falso caso contrário.</returns>
        public bool Contains(T item)
        {
            return Array.IndexOf(this.array, item) != -1;
        }

        /// <summary>
        /// Copia os valores da lista para uma ordenação.
        /// </summary>
        /// <param name="array">A ordenação de destino.</param>
        /// <param name="arrayIndex">O índice de destino onde será iniciada a cópia.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(this.array, 0, array, arrayIndex, this.array.Length);
        }

        /// <summary>
        /// Remove o item da lista.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>Verdadeiro se a remoção for bem-sucedida e falso caso contrário.</returns>
        public bool Remove(T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Obtém um enumerador para a lista.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IList<T>)this.array).GetEnumerator();
        }

        /// <summary>
        /// Verifica se a ordenação corrente iguala o objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>Verdadeiro caso os objectos sejam iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (object.ReferenceEquals(this.array, obj))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Obtém o código confuso para o objecto.
        /// </summary>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode()
        {
            return this.array.GetHashCode();
        }

        /// <summary>
        /// Obtém a representação textual do objecto.
        /// </summary>
        /// <returns>A representação textual do objecto.</returns>
        public override string ToString()
        {
            return this.array.ToString();
        }

        /// <summary>
        /// Obtém um enumerador não genérico para a lista.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Implmenta um vector cujo tamanho não se encontra limitado aos 2 GB.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas do vector.</typeparam>
    public sealed class GeneralLongArray<T> : ILongList<T>, IList<T>
    {
        /// <summary>
        /// A máscara para efectura o resto da divisão.
        /// </summary>
        private static int mask;

        /// <summary>
        /// A potência na base 2 para o número máximo de itens que podem ser
        /// contidos num vector de sistema.
        /// </summary>
        private static int maxBinaryPower;

        /// <summary>
        /// Mantém os elementos.
        /// </summary>
        private T[][][] elements;

        /// <summary>
        /// Mantém o tamanho total do vector.
        /// </summary>
        private long length;

        /// <summary>
        /// Inicializa o tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        static GeneralLongArray()
        {
            var objType = typeof(T);
            if (objType == typeof(byte) ||
                objType == typeof(sbyte) ||
                objType == typeof(bool))
            {
                maxBinaryPower = 30;
                mask = 1073741823;
            }
            else if (objType == typeof(char)
                || objType == typeof(short)
                || objType == typeof(ushort))
            {
                maxBinaryPower = 29;
                mask = 536870911;
            }
            else if (objType == typeof(int)
                || objType == typeof(uint)
                || objType == typeof(double))
            {
                maxBinaryPower = 28;
                mask = 268435455;
            }
            else if (objType == typeof(long)
                || objType == typeof(ulong)
                || objType == typeof(double))
            {
                maxBinaryPower = 27;
                mask = 134217727;
            }
            else
            {
                maxBinaryPower = 26;
                mask = 67108863;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        public GeneralLongArray()
        {
            this.elements = new T[0][][];
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        public GeneralLongArray(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.Instantiate(length);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        public GeneralLongArray(long length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.Instantiate(length);
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[long index]
        {
            get
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        return this.elements[0][0][thirdDim];
                    }
                    else
                    {
                        var secondDim = firstDim & 67108863;
                        firstDim >>= 26;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        this.elements[0][0][thirdDim] = value;
                    }
                    else
                    {
                        var secondDim = firstDim & 67108863;
                        firstDim >>= 26;
                        this.elements[firstDim][secondDim][thirdDim] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        return this.elements[0][0][thirdDim];
                    }
                    else
                    {
                        var secondDim = firstDim & 67108863;
                        firstDim >>= 26;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        this.elements[0][0][thirdDim] = value;
                    }
                    else
                    {
                        var secondDim = firstDim & 67108863;
                        firstDim >>= 27;
                        this.elements[firstDim][secondDim][thirdDim] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o número de elementos no vector.
        /// </summary>
        public int Count
        {
            get
            {
                if (this.length > int.MaxValue)
                {
                    throw new CollectionsException("The length of vector is too big. Please use LongCount instaed.");
                }
                else
                {
                    return (int)this.length;
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public long LongCount
        {
            get
            {
                return this.length;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a colecção é apenas de leitura.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public long LongIndexOf(T item)
        {
            return this.IndexOfAux(item);
        }

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public int IndexOf(T item)
        {
            var result = this.IndexOfAux(item);
            if (result > int.MaxValue)
            {
                throw new CollectionsException("The index value is too big. Please use LongIndexOf function.");
            }
            else
            {
                return (int)result;
            }
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(long index, T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(int index, T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(long index)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(int index)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Adiciona um item à colecção.
        /// </summary>
        /// <param name="item">O item a ser adicionado.</param>
        public void Add(T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Elimina todos os itens da colecção.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Retorna um valor que indica se o item especificado se encontra na colecção.
        /// </summary>
        /// <param name="item">O item a procurar.</param>
        /// <returns>Verdadeiro caso o item exista no vector e falso caso contrário.</returns>
        public bool Contains(T item)
        {
            var firstLength = this.elements.Length;
            for (int i = 0; i < firstLength; ++i)
            {
                var curr = this.elements[i];
                var secondLength = curr.Length;
                for (int j = 0; j < secondLength; ++j)
                {
                    if (((IList<T>)curr[j]).Contains(item))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Copia o conteúdo da colecção para um vector de sistema.
        /// </summary>
        /// <param name="array">O vector de sistema de destino.</param>
        /// <param name="arrayIndex">O índice a partir do qual é efectuada a cópia.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            var firstLength = this.elements.Length;
            var currentIndex = arrayIndex;
            for (int i = 0; i < firstLength; ++i)
            {
                var curr = this.elements[i];
                var secondLength = curr.Length;
                for (int j = 0; j < secondLength; ++j)
                {
                    var elem = curr[j];
                    var thirdLength = elem.Length;
                    Array.Copy(elem, 0, array, currentIndex, thirdLength);
                    currentIndex += thirdLength;
                }
            }
        }

        /// <summary>
        /// Remove um item da colecção.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Obtém um enumerador genérico para a colecção.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var firstLength = this.elements.Length;
            for (int i = 0; i < firstLength; ++i)
            {
                var curr = this.elements[i];
                var secondLength = curr.Length;
                for (int j = 0; j < secondLength; ++j)
                {
                    var elem = curr[j];
                    var thirdLength = elem.Length;
                    for (int k = 0; k < thirdLength; ++k)
                    {
                        yield return elem[k];
                    }
                }
            }
        }

        /// <summary>
        /// Obtém um enumerador não genérico para a colecção.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Instancia a estrutura interna.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        private void Instantiate(long length)
        {
            this.length = length;
            var size = mask + 1;
            if (length == 0)
            {
                this.elements = new T[0][][];
            }
            else
            {
                var thirdDim = length & mask;
                var firstDim = length >> maxBinaryPower;
                if (firstDim == 0)
                {
                    var elems = new T[1][][];
                    var innerElems = new T[1][];
                    innerElems[0] = new T[thirdDim];
                    elems[0] = innerElems;
                    this.elements = elems;
                }
                else
                {
                    var secondDim = firstDim & 67108863;
                    firstDim >>= 26;
                    if (thirdDim == 0)
                    {
                        if (secondDim == 0)
                        {
                            var elems = new T[firstDim][][];
                            this.elements = elems;
                            for (int i = 0; i < firstDim; ++i)
                            {
                                var innerElem = new T[67108864][];
                                elems[i] = innerElem;
                                for (int j = 0; j < 67108864; ++j)
                                {
                                    innerElem[j] = new T[size];
                                }
                            }
                        }
                        else
                        {
                            var elems = new T[firstDim + 1][][];
                            this.elements = elems;
                            for (int i = 0; i < firstDim; ++i)
                            {
                                var innerElem = new T[67108864][];
                                elems[i] = innerElem;
                                for (int j = 0; j < 67108864; ++j)
                                {
                                    innerElem[j] = new T[size];
                                }
                            }

                            var innerElemOut = new T[secondDim][];
                            elems[firstDim] = innerElemOut;
                            for (int j = 0; j < secondDim; ++j)
                            {
                                innerElemOut[j] = new T[size];
                            }
                        }
                    }
                    else if (secondDim == 67108864)
                    {
                        var elems = new T[firstDim + 1][][];
                        this.elements = elems;
                        for (int i = 0; i < firstDim; ++i)
                        {
                            var innerElem = new T[67108864][];
                            elems[i] = innerElem;
                            for (int j = 0; j < 67108864; ++j)
                            {
                                innerElem[j] = new T[size];
                            }
                        }

                        var innerElemOut = new T[1][];
                        elems[firstDim] = innerElemOut;
                        innerElemOut[0] = new T[thirdDim];
                    }
                    else
                    {
                        var elems = new T[firstDim + 1][][];
                        this.elements = elems;
                        for (int i = 0; i < firstDim; ++i)
                        {
                            var innerElem = new T[67108864][];
                            elems[i] = innerElem;
                            for (int j = 0; j < 67108864; ++j)
                            {
                                innerElem[j] = new T[size];
                            }
                        }

                        var innerElemOut = new T[secondDim + 1][];
                        elems[firstDim] = innerElemOut;
                        for (int i = 0; i < secondDim; ++i)
                        {
                            innerElemOut[i] = new T[size];
                        }

                        innerElemOut[secondDim] = new T[thirdDim];
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o índice do primeiro elemento igual ao item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>
        /// O índice do primeiro item encontrado e -1 caso este não esteja contido no vector.
        /// </returns>
        private long IndexOfAux(T item)
        {
            var firstLength = this.elements.Length;
            for (int i = 0; i < firstLength; ++i)
            {
                var secondElem = this.elements[i];
                var secondLength = secondElem.Length;
                for (int j = 0; j < secondLength; ++j)
                {
                    var index = ((IList<T>)secondElem[j]).IndexOf(item);
                    if (index > -1)
                    {
                        var result = ((long)i * 67108864 + j) * (mask + 1) + index;
                        return result;
                    }
                }
            }

            return -1L;
        }
    }
}
