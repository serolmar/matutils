// -----------------------------------------------------------------------
// <copyright file="GeneralizedCollections.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

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
        public T this[uint index]
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
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[ulong index]
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
        /// Obtém o tamanho do vector.
        /// </summary>
        public uint UintLength
        {
            get
            {
                return (uint)this.array.Length;
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
        /// Obtém o tamanho longo do vector.
        /// </summary>
        public ulong ULongLength
        {
            get
            {
                return (ulong)this.array.LongLength;
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
        public uint UintCount
        {
            get
            {
                return (uint)this.array.Length;
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
        /// Obtém o número de elementos na lista.
        /// </summary>
        public ulong UlongCount
        {
            get
            {
                return (ulong)this.array.LongLength;
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
        public void Insert(uint index, T item)
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
        /// Insere o item na posição especificada.
        /// </summary>
        /// <param name="index">O índice da posição.</param>
        /// <param name="item">O item a ser inserido.</param>
        public void Insert(ulong index, T item)
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
        public void RemoveAt(uint index)
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
        /// Remove o item da posição indicada.
        /// </summary>
        /// <param name="index">O índice da posição do item a ser removid.</param>
        public void RemoveAt(ulong index)
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
        /// Remove o item da lista.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>Verdadeiro se a remoção for bem-sucedida e falso caso contrário.</returns>
        public bool Remove(T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
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
        /// Copia o conteúdo da colecção para uma ordenação geral em forma de matriz.
        /// </summary>
        /// <param name="array">A ordenação geral.</param>
        /// <param name="dimensions">
        /// Os valores que identificam a entrada da matriz onde a cópia será iniciada.
        /// </param>
        public void CopyTo(
            Array array,
            long[] dimensions)
        {
            throw new NotImplementedException();
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
        /// Define uma matriz vazia a ser utilizada.
        /// </summary>
        private static readonly T[][][] emptyArray = new T[0][][];

        /// <summary>
        /// A máscara para efectuar o resto da divisão nos vectores de objectos.
        /// </summary>
        private static uint mask;

        /// <summary>
        /// A potência na base 2 para o número máximo de itens que podem ser
        /// contidos num vector de sistema.
        /// </summary>
        private static int maxBinaryPower;

        /// <summary>
        /// A potência máxima para um objecto.
        /// </summary>
        private static int objMaxBinaryPower;

        /// <summary>
        /// A máscara para efectuar o resto da divisão nos vectores de vectores.
        /// </summary>
        private static uint generalMask;

        /// <summary>
        /// Variável que indica se se irá avaliar a memória disponível
        /// em caso de instância.
        /// </summary>
        private bool assertMemory;

        /// <summary>
        /// Mantém os elementos.
        /// </summary>
        private T[][][] elements;

        /// <summary>
        /// Mantém o tamanho total do vector.
        /// </summary>
        private ulong length;

        /// <summary>
        /// Inicializa o tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        static GeneralLongArray()
        {
            var configSection = Utils.GetRuntimeConfiguration();
            if (configSection.GcAllowVeryLargeObjects.Enabled)
            {
                mask = 2147483647;
                maxBinaryPower = 31;
                objMaxBinaryPower = 31;
                generalMask = 2147483647;
            }
            else
            {
                generalMask = 67108863;
                objMaxBinaryPower = 26;
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
                    mask = generalMask;
                }
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongArray(bool assertMemory = true)
        {
            this.elements = emptyArray;
            this.length = 0;
            this.assertMemory = assertMemory;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma lista geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a ordenação de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="length">O tamanho do vector.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongArray(int length, bool assertMemory = true)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.assertMemory = assertMemory;
                this.AssertVisibleMemory((ulong)length);
                this.Instantiate((ulong)length);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="length">O tamanho do vector.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongArray(uint length, bool assertMemory = true)
        {
            this.assertMemory = assertMemory;
            this.AssertVisibleMemory(length);
            this.Instantiate(length);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="length">O tamanho do vector.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongArray(long length, bool assertMemory = true)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.assertMemory = assertMemory;
                this.AssertVisibleMemory((ulong)length);
                this.Instantiate((ulong)length);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="length">O tamanho do vector.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongArray(ulong length, bool assertMemory = true)
        {
            this.assertMemory = assertMemory;
            this.AssertVisibleMemory((ulong)length);
            this.Instantiate((ulong)length);
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
                if (index < 0 || (ulong)index >= this.length)
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || (ulong)index >= this.length)
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
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
        public T this[ulong index]
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || (ulong)index >= this.length)
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
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
                if (index < 0 || (ulong)index >= this.length)
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || (ulong)index >= this.length)
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
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
        public T this[uint index]
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        this.elements[firstDim][secondDim][thirdDim] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
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
        public uint UintCount
        {
            get
            {
                if (this.length > uint.MaxValue)
                {
                    throw new CollectionsException("The length of vector is too big. Please use LongCount instaed.");
                }
                else
                {
                    return (uint)this.length;
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
                if (this.length > long.MaxValue)
                {
                    throw new CollectionsException("The length of vector is too big. Please use UlongCount instaed.");
                }
                else
                {
                    return (long)this.length;
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public ulong UlongCount
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

        #region Propriedades internas

        /// <summary>
        /// Obtém ou atribui a potência binária máxima para uma ordenação dos
        /// objectos do tipo das entradas da colecção.
        /// </summary>
        internal static int MaxBinaryPower
        {
            get
            {
                return maxBinaryPower;
            }
            set
            {
                maxBinaryPower = value;
                mask = (1U << value) - 1;
            }
        }

        /// <summary>
        /// Obtém o valor da máscara para os objectos do tipo dos das entradas
        /// da colecção.
        /// </summary>
        internal static uint Mask
        {
            get
            {
                return mask;
            }
        }

        /// <summary>
        /// Obtém ou atribui a potência binária máxima para uma ordenação dos
        /// objectos do tipo geral.
        /// </summary>
        internal static int ObjMaxBinaryPower
        {
            get
            {
                return objMaxBinaryPower;
            }
            set
            {
                objMaxBinaryPower = value;
                generalMask = (1U << value) - 1;
            }
        }

        /// <summary>
        /// Obtém o valor da máscara para os objectos do tipo geral.
        /// </summary>
        internal static uint GeneralMask
        {
            get
            {
                return generalMask;

            }
        }

        #endregion Propriedades internas

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public long LongIndexOf(T item)
        {
            var result = default(ulong);
            if (this.TryGetIndexOfAux(item, out result))
            {
                if (result > long.MaxValue)
                {
                    throw new CollectionsException("The index value is too big. Please use TryGetIndexOf function instead.");
                }
                else
                {
                    return (long)result;
                }
            }
            else
            {
                return -1L;
            }
        }

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public int IndexOf(T item)
        {
            var result = default(ulong);
            if (this.TryGetIndexOfAux(item, out result))
            {
                if (result > int.MaxValue)
                {
                    throw new CollectionsException("The index value is too big. Please use LongIndexOf function instead.");
                }
                else
                {
                    return (int)result;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Tenta obter o índice da primeira ocorrência do item especificado.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <param name="index">O índice da primeira ocorrência do item.</param>
        /// <returns>Verdadeiro se o item se encontrar na colecção e falso caso contrário.</returns>
        private bool TryGetIndexOf(T item, out ulong index)
        {
            return this.TryGetIndexOfAux(item, out index);
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
        public void Insert(ulong index, T item)
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
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(uint index, T item)
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
        public void RemoveAt(ulong index)
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
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(uint index)
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
            if (item == null)
            {
                for (int i = 0; i < firstLength; ++i)
                {
                    var curr = this.elements[i];
                    var secondLength = curr.Length;
                    for (int j = 0; j < secondLength; ++j)
                    {
                        var innerCurr = curr[j];
                        var thirdLength = innerCurr.Length;
                        for (int k = 0; k < thirdLength; ++k)
                        {
                            if (innerCurr[k] == null)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                var comparer = EqualityComparer<T>.Default;
                for (int i = 0; i < firstLength; ++i)
                {
                    var curr = this.elements[i];
                    var secondLength = curr.Length;
                    for (int j = 0; j < secondLength; ++j)
                    {
                        var innerCurr = curr[j];
                        var thirdLength = innerCurr.Length;
                        for (int k = 0; k < thirdLength; ++k)
                        {
                            if (comparer.Equals(innerCurr[k], item))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Remove um item da colecção.
        /// </summary>
        /// <param name="item">O item a ser removido.</param>
        /// <returns>Verdadeiro se o item for removido com sucesso e falso caso contrário.</returns>
        public bool Remove(T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
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
        /// Copia o conteúdo da colecção para uma ordenação geral em forma de matriz.
        /// </summary>
        /// <param name="array">A ordenação geral.</param>
        /// <param name="dimensions">
        /// Os valores que identificam a entrada da matriz onde a cópia será iniciada.
        /// </param>
        public void CopyTo(
            Array array,
            long[] dimensions)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (dimensions == null)
            {
                throw new ArgumentNullException("dimensions");
            }
            else
            {
                var rank = dimensions.LongLength;
                if (rank == 0)
                {
                    if (this.length != 0)
                    {
                        throw new ArgumentException(
                            "Destination array was not long enough. Check destIndex and length, and the array's lower bounds.");
                    }
                }
                else if (rank == 1)
                {
                    var firstLength = this.elements.Length;
                    var currentIndex = dimensions[0];
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
                else
                {
                    this.AssertArrayStructure(array, dimensions);
                    var indexes = new long[rank];
                    var arrays = new Array[rank];
                    Array.Copy(dimensions, indexes, rank);
                    var currentArray = array;
                    arrays[0] = currentArray;
                    for (var i = 1L; i < rank; ++i)
                    {
                        var innerArray = (Array)currentArray.GetValue(indexes[i - 1]);
                        arrays[i] = innerArray;
                        currentArray = innerArray;
                    }

                    var elementsLength = this.elements.LongLength;
                    for (var i = 0L; i < elementsLength; ++i)
                    {
                        var currElem = this.elements[i];
                        var currElemLength = currElem.LongLength;
                        for (var j = 0L; j < currElemLength; ++j)
                        {
                            var current = currElem[j];
                            this.CopyCurrentArray(
                                current,
                                arrays,
                                indexes);
                        }
                    }
                }
            }
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

        #region Funções internas para testes

        /// <summary>
        /// Verifica se o tamanho da instância do vector está de acordo com
        /// a capacidade estabelecida.
        /// </summary>
        /// <returns>
        /// Verdadeiro caso o tamanho do vector esteja de acordo com a capacidade
        /// estabelecida e falso caso contrário.
        /// </returns>
        internal bool AssertSizes()
        {
            var computedLength = 0;
            for (int i = 0; i < this.elements.Length; ++i)
            {
                var current = this.elements[i];
                for (int j = 0; j < current.Length; ++j)
                {
                    computedLength += current[j].Length;
                }
            }

            return this.length == (ulong)computedLength;
        }

        #endregion Funções internas para testes

        #region Funções privadas

        /// <summary>
        /// Verifica a validade da memória visível disponibilizada pelo sistema operativo.
        /// </summary>
        /// <param name="size">O tamanho da colecção.</param>
        private void AssertVisibleMemory(ulong size)
        {
            if (this.assertMemory)
            {
                var memory = Utils.GetMemoryInfo().TotalVisibleMemorySize;
                var factor = 64UL;
                var objType = typeof(T);
                if (objType == typeof(byte) ||
                        objType == typeof(sbyte) ||
                        objType == typeof(bool))
                {
                    factor = 1024;
                }
                else if (objType == typeof(char)
                    || objType == typeof(short)
                    || objType == typeof(ushort))
                {
                    factor = 512;
                }
                else if (objType == typeof(int)
                    || objType == typeof(uint)
                    || objType == typeof(double))
                {
                    factor = 256;
                }
                else if (objType == typeof(long)
                    || objType == typeof(ulong)
                    || objType == typeof(double))
                {
                    factor = 128;
                }

                var itemsNumber = memory * factor;
                if (itemsNumber < size)
                {
                    throw new OutOfMemoryException("There is no visbile memory available to proceed.");
                }
            }
        }

        /// <summary>
        /// Instancia a estrutura interna.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        private void Instantiate(ulong length)
        {
            this.length = length;
            var size = mask + 1;
            var generalSize = generalMask + 1;
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
                    var secondDim = firstDim & generalMask;
                    firstDim >>= objMaxBinaryPower;
                    if (thirdDim == 0)
                    {
                        if (secondDim == 0)
                        {
                            var elems = new T[firstDim][][];
                            this.elements = elems;
                            for (var i = 0UL; i < firstDim; ++i)
                            {
                                var innerElem = new T[generalSize][];
                                elems[i] = innerElem;
                                for (int j = 0; j < generalSize; ++j)
                                {
                                    innerElem[j] = new T[size];
                                }
                            }
                        }
                        else
                        {
                            var elems = new T[firstDim + 1][][];
                            this.elements = elems;
                            for (var i = 0UL; i < firstDim; ++i)
                            {
                                var innerElem = new T[generalSize][];
                                elems[i] = innerElem;
                                for (int j = 0; j < generalSize; ++j)
                                {
                                    innerElem[j] = new T[size];
                                }
                            }

                            var innerElemOut = new T[secondDim][];
                            elems[firstDim] = innerElemOut;
                            for (var j = 0UL; j < secondDim; ++j)
                            {
                                innerElemOut[j] = new T[size];
                            }
                        }
                    }
                    else if (secondDim == generalSize)
                    {
                        var elems = new T[firstDim + 1][][];
                        this.elements = elems;
                        for (var i = 0UL; i < firstDim; ++i)
                        {
                            var innerElem = new T[generalSize][];
                            elems[i] = innerElem;
                            for (int j = 0; j < generalSize; ++j)
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
                        for (var i = 0UL; i < firstDim; ++i)
                        {
                            var innerElem = new T[generalSize][];
                            elems[i] = innerElem;
                            for (int j = 0; j < generalSize; ++j)
                            {
                                innerElem[j] = new T[size];
                            }
                        }

                        var innerElemOut = new T[secondDim + 1][];
                        elems[firstDim] = innerElemOut;
                        for (var i = 0UL; i < secondDim; ++i)
                        {
                            innerElemOut[i] = new T[size];
                        }

                        innerElemOut[secondDim] = new T[thirdDim];
                    }
                }
            }
        }

        /// <summary>
        /// Tenta obter o índice da primeira ocorrência do item especificado.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <param name="index">O índice da primeira ocorrência do item.</param>
        /// <returns>Verdadeiro se o item se encontrar na colecção e falso caso contrário.</returns>
        private bool TryGetIndexOfAux(T item, out ulong index)
        {
            var firstLength = (ulong)this.elements.Length;
            var generalSize = generalMask + 1;
            for (var i = 0UL; i < firstLength; ++i)
            {
                var secondElem = this.elements[i];
                var secondLength = (ulong)secondElem.Length;
                for (var j = 0UL; j < secondLength; ++j)
                {
                    var innerIndex = Array.IndexOf(secondElem, item);
                    if (innerIndex > -1)
                    {
                        checked
                        {
                            index = (i * generalSize + j) * (mask + 1) + (ulong)innerIndex;
                            return true;
                        }
                    }
                }
            }

            index = 0;
            return false;
        }

        /// <summary>
        /// Verifica a validade da estrutura da ordenação.
        /// </summary>
        /// <param name="array">A ordenação.</param>
        /// <param name="dimensions">A descrição das dimensões.</param>
        private void AssertArrayStructure(
            Array array,
            long[] dimensions)
        {
            var rank = dimensions.Length;
            var objType = array.GetType();
            for (var i = 0; i < rank; ++i)
            {
                if (objType.IsArray && objType.GetArrayRank() == 1)
                {
                    objType = objType.GetElementType();
                }
                else
                {
                    throw new UtilitiesException(string.Format(
                        "Array must be rank one at level {0}.",
                        i));
                }
            }

            if (!objType.IsAssignableFrom(typeof(T)))
            {
                throw new ArrayTypeMismatchException("Source array type cannot be assigned to destination array type.");
            }
        }

        /// <summary>
        /// Realiza a cópia de uma ordenação para um conjunto de ordenações.
        /// </summary>
        /// <param name="array">A ordenação de partida.</param>
        /// <param name="arrays">A definição das ordenações de destino.</param>
        /// <param name="indexes">
        /// Os índices que definem o estado das ordenações de destino.
        /// </param>
        private void CopyCurrentArray(
            T[] array,
            Array[] arrays,
            long[] indexes)
        {
            var rank = arrays.LongLength;
            var indexPointer = rank - 1;
            var currentIndex = indexes[indexPointer];
            var currentArray = arrays[indexPointer];
            var arrayLength = array.LongLength;
            var arrayIndex = 0L;
            while (arrayLength > 0)
            {
                var difference = currentArray.LongLength - currentIndex;
                if (difference < arrayLength)
                {
                    Array.Copy(
                        array,
                        arrayIndex,
                        currentArray,
                        currentIndex,
                        difference);
                    arrayLength -= difference;
                    arrayIndex += difference;

                    // Actualiza o estado dos índices
                    var state = true;
                    while (state)
                    {
                        --indexPointer;
                        if (indexPointer < 0)
                        {
                            throw new ArgumentException(
                                "Destination array was not long enough. Check destIndex and length, and the array's lower bounds.");
                        }
                        else
                        {
                            currentIndex = indexes[indexPointer];
                            currentArray = arrays[indexPointer];
                            var currentArrayLength = currentArray.LongLength;
                            ++currentIndex;
                            if (currentIndex < currentArrayLength)
                            {
                                indexes[indexPointer] = currentIndex;
                                ++indexPointer;
                                var innerArray = (Array)currentArray.GetValue(currentIndex);
                                arrays[indexPointer] = innerArray;
                                indexes[indexPointer] = 0L;
                                currentArray = innerArray;
                                ++indexPointer;
                                for (; indexPointer < rank; ++indexPointer)
                                {
                                    innerArray = (Array)currentArray.GetValue(0);
                                    arrays[indexPointer] = innerArray;
                                    indexes[indexPointer] = 0L;
                                    currentArray = innerArray;
                                }

                                --indexPointer;
                                currentIndex = 0;
                                state = false;
                            }
                        }
                    }
                }
                else
                {
                    Array.Copy(
                        array,
                        arrayIndex,
                        currentArray,
                        currentIndex,
                        arrayLength);
                    currentIndex += arrayLength;
                    indexes[indexPointer] = currentIndex;

                    arrayLength = 0;
                }
            }
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Implementa uma lista cujo tamanho não se encontra limitado aos 2 GB.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas da lista.</typeparam>
    public class GeneralLongList<T> : ILongList<T>, IList<T>
    {
        /// <summary>
        /// Mantém uma instância do vector vazio.
        /// </summary>
        private static readonly T[][][] emptyArray = new T[0][][];

        /// <summary>
        /// A máscara para efectuar o resto da divisão nos vectores de objectos.
        /// </summary>
        private static uint mask;

        /// <summary>
        /// A potência na base 2 para o número máximo de itens que podem ser
        /// contidos num vector de sistema.
        /// </summary>
        private static int maxBinaryPower;

        /// <summary>
        /// A potência máxima para um objecto.
        /// </summary>
        private static int objMaxBinaryPower;

        /// <summary>
        /// A máscara para efectuar o resto da divisão nos vectores de vectores.
        /// </summary>
        private static uint generalMask;

        /// <summary>
        /// Estabelece a capacidade estabelecida por defeito aquando da adição do primeiro elemento
        /// a uma lista cuja capacidade inicial é nula.
        /// </summary>
        private static ulong defaultCapacity = 1024;

        /// <summary>
        /// Variável que indica se se irá avaliar a memória disponível
        /// em caso de instância.
        /// </summary>
        private bool assertMemory = true;

        /// <summary>
        /// Mantém os elementos.
        /// </summary>
        private T[][][] elements;

        /// <summary>
        /// Mantém o tamanho total do vector.
        /// </summary>
        private ulong length;

        /// <summary>
        /// A primeira dimensão do comprimento da lista.
        /// </summary>
        private ulong firstDimLength;

        /// <summary>
        /// A segunda dimensão do comprimento da lista.
        /// </summary>
        private ulong secondDimLength;

        /// <summary>
        /// A terceira dimensão do comprimento da lista.
        /// </summary>
        private ulong thirdDimLength;

        /// <summary>
        /// Mantém o valor da capacidade do vector. 
        /// </summary>
        private ulong capacity;

        /// <summary>
        /// Inicializa o tipo <see cref="GeneralLongList{T}"/>.
        /// </summary>
        static GeneralLongList()
        {
            var configSection = Utils.GetRuntimeConfiguration();
            if (configSection.GcAllowVeryLargeObjects.Enabled)
            {
                mask = 2147483647;
                maxBinaryPower = 31;
                objMaxBinaryPower = 31;
                generalMask = 2147483647;
            }
            else
            {
                generalMask = 67108863;
                objMaxBinaryPower = 26;
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
                    mask = generalMask;
                }
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongList{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongList(bool assertMemory = true)
        {
            this.length = 0;
            this.capacity = 0;
            this.elements = emptyArray;
            this.assertMemory = assertMemory;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongList{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="capacity">O valor da capacidade da lista.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongList(int capacity, bool assertMemory = true)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }
            else
            {
                this.assertMemory = assertMemory;
                this.AssertVisibleMemory((ulong)capacity);
                this.Instantiate((ulong)capacity);
                this.length = 0;
                this.firstDimLength = 0;
                this.secondDimLength = 0;
                this.thirdDimLength = 0;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongList{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="capacity">O valor da capacidade da lista.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongList(uint capacity, bool assertMemory = true)
        {
            this.assertMemory = assertMemory;
            this.AssertVisibleMemory(capacity);
            this.Instantiate(capacity);
            this.length = 0;
            this.firstDimLength = 0;
            this.secondDimLength = 0;
            this.thirdDimLength = 0;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongList{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="capacity">O valor da capacidade da lista.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongList(long capacity, bool assertMemory = true)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }
            else
            {
                this.assertMemory = assertMemory;
                this.AssertVisibleMemory((ulong)capacity);
                this.Instantiate((ulong)capacity);
                this.length = 0;
                this.firstDimLength = 0;
                this.secondDimLength = 0;
                this.thirdDimLength = 0;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongList{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="capacity">O valor da capacidade da lista.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongList(ulong capacity, bool assertMemory = true)
        {
            this.assertMemory = assertMemory;
            this.AssertVisibleMemory(capacity);
            this.Instantiate(capacity);
            this.length = 0;
            this.firstDimLength = 0;
            this.secondDimLength = 0;
            this.thirdDimLength = 0;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongList{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// 
        /// A lista criada será populada com os elementos contidos na colecção. De modo a manter-se a compatibilidade
        /// com o CLR (Common Language Runtime), caso seja passada um objecto do tipo <see cref="ICollection{T}"/>,
        /// a respectiva cópia será realizada por intermédio da chamada à função de cópia definida. Assim, caso o número
        /// de elementos na colecção proporcionada exceda o máximo permitido para um inteiro, a cópia resultará numa excepção
        /// ou não será bem sucedida.
        /// </remarks>
        /// <param name="collection">A colecção.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongList(IEnumerable<T> collection, bool assertMemory = true)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                this.assertMemory = assertMemory;
                var longCollection = collection as ILongCollection<T>;
                if (longCollection == null)
                {
                    var intCollection = collection as ICollection<T>;
                    if (intCollection == null)
                    {
                        var enumerator = collection.GetEnumerator();
                        this.length = 0;
                        this.elements = emptyArray;
                        while (enumerator.MoveNext())
                        {
                            this.Add(enumerator.Current);
                        }
                    }
                    else
                    {
                        var count = intCollection.Count;
                        if (count == 0)
                        {
                            this.length = 0;
                            this.firstDimLength = 0;
                            this.secondDimLength = 0;
                            this.thirdDimLength = 0;
                            this.elements = emptyArray;
                        }
                        else
                        {
                            var innerMostArray = new T[count];
                            intCollection.CopyTo(innerMostArray, 0);
                            this.length = (ulong)count;
                            var sizes = this.GetSizes(this.length);
                            this.firstDimLength = sizes.Item1;
                            this.secondDimLength = sizes.Item2;
                            this.thirdDimLength = sizes.Item3;
                        }
                    }
                }
                else
                {
                    var count = longCollection.UlongCount;
                    this.Instantiate(count);
                    longCollection.CopyTo(
                        this.elements,
                        new[] { 0L, 0L, 0L });
                    this.length = count;
                    var sizes = this.GetSizes(this.length);
                    this.firstDimLength = sizes.Item1;
                    this.secondDimLength = sizes.Item2;
                    this.thirdDimLength = sizes.Item3;
                }
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
                if (index < 0 || (ulong)index >= this.length)
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || (ulong)index >= this.length)
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
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
        public T this[ulong index]
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
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
                if (index < 0 || (ulong)index >= this.length)
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || (ulong)index >= this.length)
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= 27;
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
        public T this[uint index]
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
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
                        var secondDim = firstDim & generalMask;
                        firstDim >>= 27;
                        this.elements[firstDim][secondDim][thirdDim] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
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
        public uint UintCount
        {
            get
            {
                if (this.length > uint.MaxValue)
                {
                    throw new CollectionsException("The length of vector is too big. Please use UlongCount instaed.");
                }
                else
                {
                    return (uint)this.length;
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
                if (this.length > long.MaxValue)
                {
                    throw new CollectionsException("The length of vector is too big. Please use UlongCount instaed.");
                }
                else
                {
                    return (long)this.length;
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public ulong UlongCount
        {
            get
            {
                return this.length;
            }
        }

        /// <summary>
        /// Obtém o valor da capacidade da lista.
        /// </summary>
        public int Capacity
        {
            get
            {
                if (this.capacity > int.MaxValue)
                {
                    throw new CollectionsException("The capacity of the list is too big. Please use LongCapacity instaed.");
                }
                else
                {
                    return (int)this.capacity;
                }
            }
            set
            {
                var innerValue = (ulong)value;
                if (value < 0)
                {
                    innerValue = 0;
                }

                if (innerValue < this.length)
                {
                    throw new CollectionsException("The provided capacity is too small.");
                }
                else if (innerValue > this.capacity)
                {
                    this.AssertVisibleMemory(innerValue);
                    this.IncreaseCapacityTo(innerValue);
                }
                else if (innerValue < this.capacity)
                {
                    this.DecreaseCapcityTo(innerValue);
                }
            }
        }

        /// <summary>
        /// Obtém o valor da capacidade da lista.
        /// </summary>
        public uint UintCapacity
        {
            get
            {
                if (this.capacity > uint.MaxValue)
                {
                    throw new CollectionsException("The capacity of the list is too big. Please use LongCapacity instaed.");
                }
                else
                {
                    return (uint)this.capacity;
                }
            }
            set
            {
                if (value < this.length)
                {
                    throw new CollectionsException("The provided capacity is too small.");
                }
                else if (value > this.capacity)
                {
                    this.AssertVisibleMemory(value);
                    this.IncreaseCapacityTo(value);
                }
                else if (value < this.capacity)
                {
                    this.DecreaseCapcityTo(value);
                }
            }
        }

        /// <summary>
        /// Obtém o valor da capacidade da lista.
        /// </summary>
        public long LongCapacity
        {
            get
            {
                if (this.capacity > long.MaxValue)
                {
                    throw new CollectionsException("The capacity of the list is too big. Please use UlongCapacity instaed.");
                }
                else
                {
                    return (long)this.capacity;
                }
            }
            set
            {
                var innerValue = (ulong)value;
                if (value < 0)
                {
                    innerValue = 0;
                }

                if (innerValue < this.length)
                {
                    throw new CollectionsException("The provided capacity is too small.");
                }
                else if (innerValue > this.capacity)
                {
                    this.AssertVisibleMemory(innerValue);
                    this.IncreaseCapacityTo(innerValue);
                }
                else if (innerValue < this.capacity)
                {
                    this.DecreaseCapcityTo(innerValue);
                }
            }
        }

        /// <summary>
        /// Obtém o valor da capacidade da lista.
        /// </summary>
        public ulong UlongCapacity
        {
            get
            {
                return this.capacity;
            }
            set
            {
                if (value < this.length)
                {
                    throw new CollectionsException("The provided capacity is too small.");
                }
                else if (value > this.capacity)
                {
                    this.AssertVisibleMemory(value);
                    this.IncreaseCapacityTo(value);
                }
                else if (value < this.capacity)
                {
                    this.DecreaseCapcityTo(value);
                }
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a colecção é apenas de leitura.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #region Propriedades internas

        /// <summary>
        /// Obtém ou atribui a potência binária máxima para uma ordenação dos
        /// objectos do tipo das entradas da colecção.
        /// </summary>
        internal static int MaxBinaryPower
        {
            get
            {
                return maxBinaryPower;
            }
            set
            {
                maxBinaryPower = value;
                mask = (1U << value) - 1;
            }
        }

        /// <summary>
        /// Obtém o valor da máscara para os objectos do tipo dos das entradas
        /// da colecção.
        /// </summary>
        internal static uint Mask
        {
            get
            {
                return mask;
            }
        }

        /// <summary>
        /// Obtém ou atribui a potência binária máxima para uma ordenação dos
        /// objectos do tipo geral.
        /// </summary>
        internal static int ObjMaxBinaryPower
        {
            get
            {
                return objMaxBinaryPower;
            }
            set
            {
                objMaxBinaryPower = value;
                generalMask = (1U << value) - 1;
            }
        }

        /// <summary>
        /// Obtém o valor da máscara para os objectos do tipo geral.
        /// </summary>
        internal static uint GeneralMask
        {
            get
            {
                return generalMask;

            }
        }

        #endregion Propriedades internas

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public int IndexOf(T item)
        {
            var result = default(ulong);
            if (this.TryGetIndexOfAux(item, out result))
            {
                if (result > int.MaxValue)
                {
                    throw new CollectionsException("The index value is too big. Please use LongIndexOf function.");
                }
                else
                {
                    return (int)result;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public long LongIndexOf(T item)
        {
            var result = default(ulong);
            if (this.TryGetIndexOfAux(item, out result))
            {
                if (result > long.MaxValue)
                {
                    throw new CollectionsException("The index value is too big. Please use TryGetIndexOf function instead.");
                }
                else
                {
                    return (long)result;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Tenta obter o índice da primeira ocorrência do item especificado.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <param name="index">O índice da primeira ocorrência do item.</param>
        /// <returns>Verdadeiro se o item se encontrar na colecção e falso caso contrário.</returns>
        public bool TryGetIndexOf(T item, out ulong index)
        {
            return this.TryGetIndexOfAux(item, out index);
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(int index, T item)
        {
            if (index < 0 || (ulong)index > this.length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                this.InnerInsert((ulong)index, item);
            }
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(uint index, T item)
        {
            if (index > this.length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                this.InnerInsert(index, item);
            }
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(long index, T item)
        {
            if (index < 0 || (ulong)index > this.length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                this.InnerInsert((ulong)index, item);
            }
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(ulong index, T item)
        {
            if (index > this.length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                this.InnerInsert(index, item);
            }
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || (ulong)index >= this.length)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.InnerRemoveAt((ulong)index);
            }
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(uint index)
        {
            if (index >= this.length)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.InnerRemoveAt(index);
            }
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(long index)
        {
            if (index < 0 || (ulong)index >= this.length)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.InnerRemoveAt((ulong)index);
            }
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(ulong index)
        {
            if (index >= this.length)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.InnerRemoveAt(index);
            }
        }

        /// <summary>
        /// Adiciona um item à colecção.
        /// </summary>
        /// <param name="item">O item a ser adicionado.</param>
        public void Add(T item)
        {
            this.InnerInsert(
                this.length,
                item);
        }

        /// <summary>
        /// Elimina todos os itens da colecção.
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retorna um valor que indica se o item especificado se encontra na colecção.
        /// </summary>
        /// <param name="item">O item a procurar.</param>
        /// <returns>Verdadeiro caso o item exista no vector e falso caso contrário.</returns>
        public bool Contains(T item)
        {
            return this.InnerContains(item, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Retorna um valor que indica se o item especificado se encontra na colecção.
        /// </summary>
        /// <param name="item">O item a procurar.</param>
        /// <param name="comparer">O comparador de itens.</param>
        /// <returns>Verdadeiro caso o item exista no vector e falso caso contrário.</returns>
        public bool Contains(T item, EqualityComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                return this.InnerContains(item, comparer);
            }
        }

        /// <summary>
        /// Copia o conteúdo da colecção para um vector de sistema.
        /// </summary>
        /// <param name="array">O vector de sistema de destino.</param>
        /// <param name="arrayIndex">O índice a partir do qual é efectuada a cópia.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            var firstLength = this.firstDimLength;
            var currentIndex = arrayIndex;
            for (var i = 0UL; i < firstLength; ++i)
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

            var outerSecondlength = this.secondDimLength;
            var outerCurr = this.elements[firstLength];
            for (var i = 0UL; i < outerSecondlength; ++i)
            {
                var thirdCurr = outerCurr[i];
                var thirdLength = thirdCurr.Length;
                Array.Copy(thirdCurr, 0, array, currentIndex, thirdLength);
                currentIndex += thirdLength;
            }

            if ((long)outerSecondlength < outerCurr.LongLength)
            {
                var outerThirdCurr = outerCurr[outerSecondlength];
                Array.Copy(outerThirdCurr, 0, array, currentIndex, (long)this.thirdDimLength);
            }
        }

        /// <summary>
        /// Copia o conteúdo da colecção para uma ordenação geral em forma de matriz.
        /// </summary>
        /// <param name="array">A ordenação geral.</param>
        /// <param name="dimensions">
        /// Os valores que identificam a entrada da matriz onde a cópia será iniciada.
        /// </param>
        public void CopyTo(
            Array array,
            long[] dimensions)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (dimensions == null)
            {
                throw new ArgumentNullException("dimensions");
            }
            else
            {
                var rank = dimensions.LongLength;
                if (rank == 0)
                {
                    if (this.length != 0)
                    {
                        throw new ArgumentException(
                            "Destination array was not long enough. Check destIndex and length, and the array's lower bounds.");
                    }
                }
                else if (rank == 1)
                {
                    var firstLength = this.elements.Length;
                    var currentIndex = dimensions[0];
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
                else
                {
                    var size = mask + 1;
                    var generalSize = generalMask + 1;
                    this.AssertArrayStructure(array, dimensions);
                    var indexes = new long[rank];
                    var arrays = new Array[rank];
                    Array.Copy(dimensions, indexes, rank);
                    var currentArray = array;
                    arrays[0] = currentArray;
                    for (var i = 1L; i < rank; ++i)
                    {
                        var innerArray = (Array)currentArray.GetValue(indexes[i - 1]);
                        arrays[i] = innerArray;
                        currentArray = innerArray;
                    }

                    // Rever
                    var elementsLength = this.firstDimLength;
                    if (elementsLength > 0)
                    {
                        --elementsLength;
                        for (var i = 0UL; i < elementsLength; ++i)
                        {
                            var currElem = this.elements[i];
                            for (var j = 0L; j < generalSize; ++j)
                            {
                                var current = currElem[j];
                                this.CopyCurrentArray(
                                    current,
                                    size,
                                    arrays,
                                    indexes);
                            }
                        }
                    }

                    var outerCurrElemn = this.elements[elementsLength];
                    elementsLength = this.secondDimLength;
                    if (elementsLength > 0)
                    {
                        --elementsLength;
                        for (var i = 0UL; i < elementsLength; ++i)
                        {
                            var innerCurrent = outerCurrElemn[i];
                            this.CopyCurrentArray(
                                innerCurrent,
                                size,
                                arrays,
                                indexes);
                        }
                    }

                    var lastArray = outerCurrElemn[elementsLength];
                    this.CopyCurrentArray(
                        lastArray,
                        (long)this.thirdDimLength,
                        arrays,
                        indexes);
                }
            }
        }

        /// <summary>
        /// Remove um item da colecção.
        /// </summary>
        /// <param name="item">O item a ser removido.</param>
        /// <returns>Verdadeiro se o item for removido com sucesso e falso caso contrário.</returns>
        public bool Remove(T item)
        {
            var index = default(ulong);
            if (this.TryGetIndexOfAux(item, out index))
            {
                this.InnerRemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
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
            return this.GetEnumerator();
        }

        #region Funções internas para testes

        /// <summary>
        /// Verifica se o tamanho da instância do vector está de acordo com
        /// a capacidade estabelecida.
        /// </summary>
        /// <returns>
        /// Verdadeiro caso o tamanho do vector esteja de acordo com a capacidade
        /// estabelecida e falso caso contrário.
        /// </returns>
        internal bool AssertSizes()
        {
            var computedLength = 0;
            for (int i = 0; i < this.elements.Length; ++i)
            {
                var current = this.elements[i];
                for (int j = 0; j < current.Length; ++j)
                {
                    computedLength += current[j].Length;
                }
            }

            return this.capacity == (ulong)computedLength;
        }

        #endregion Funções internas para testes

        #region Funções privadas

        /// <summary>
        /// Insere um elemento na colecção.
        /// </summary>
        /// <param name="index">O índice onde o item será inserido.</param>
        /// <param name="item">O item a ser inserido.</param>
        private void InnerInsert(ulong index, T item)
        {
            if (this.length == this.capacity)
            {
                this.ExpandCapacity(this.length + 1);
            }

            var size = mask + 1;
            var generalSize = generalMask + 1;
            if (index < this.length)
            {
                // Copia os elementos a partir do índice
                var thirdDim = index & mask;
                var firstDim = index >> maxBinaryPower;
                if (firstDim == 0)
                {
                    this.InsertMoveItems(
                        this.elements,
                        0,
                        0,
                        thirdDim,
                        this.firstDimLength,
                        this.secondDimLength,
                        this.thirdDimLength);

                    this.elements[0][0][thirdDim] = item;
                }
                else
                {
                    var secondDim = firstDim & generalMask;
                    firstDim >>= objMaxBinaryPower;
                    this.InsertMoveItems(
                        this.elements,
                        firstDim,
                        secondDim,
                        thirdDim,
                        this.firstDimLength,
                        this.secondDimLength,
                        this.thirdDimLength);
                    this.elements[firstDim][secondDim][thirdDim] = item;
                }
            }
            else
            {
                var firstIndex = this.firstDimLength;
                var secondIndex = this.secondDimLength;
                var thirdIndex = this.thirdDimLength;

                if (thirdIndex < size)
                {
                    this.elements[firstIndex][secondIndex][thirdIndex] = item;
                }
                else
                {
                    if (secondIndex < generalSize)
                    {
                        this.elements[firstIndex][secondIndex][0] = item;
                    }
                    else
                    {
                        this.elements[firstIndex][0][0] = item;
                    }
                }
            }

            this.IncrementLength();
        }

        /// <summary>
        /// Remove o elemento que se encontra na posição especificada.
        /// </summary>
        /// <param name="index">O índice do item a ser removido.</param>
        private void InnerRemoveAt(ulong index)
        {
            var size = mask + 1;
            var generalSize = generalMask + 1;
            if (index < this.length)
            {
                var firstEnd = this.firstDimLength;
                var secondEnd = this.secondDimLength;
                var thirdEnd = this.thirdDimLength;
                if (thirdEnd == 0)
                {
                    thirdEnd = mask;
                    if (secondEnd == 0)
                    {
                        secondEnd = generalMask;
                        --firstEnd;
                    }
                    else
                    {
                        --secondEnd;
                    }
                }
                else
                {
                    --thirdEnd;
                }

                var thirdDim = index & mask;
                var firstDim = index >> maxBinaryPower;
                if (firstDim == 0)
                {
                    this.RemoveMoveItems(
                        this.elements,
                        0,
                        0,
                        thirdDim,
                        firstEnd,
                        secondEnd,
                        thirdEnd);
                    this.elements[firstEnd][secondEnd][thirdEnd] = default(T);
                }
                else
                {
                    var secondDim = firstDim & generalMask;
                    firstDim >>= objMaxBinaryPower;
                    this.RemoveMoveItems(
                        this.elements,
                        firstDim,
                        secondDim,
                        thirdDim,
                        firstEnd,
                        secondEnd,
                        thirdEnd);
                    this.elements[firstEnd][secondEnd][thirdEnd] = default(T);
                }

                this.DecrementLength();
            }
            else
            {
                throw new ArgumentOutOfRangeException("index");
            }
        }

        /// <summary>
        /// Move os itens para inserção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="firstStart">A primeira dimensão inicial.</param>
        /// <param name="secondStart">A segunda dimensão inicial.</param>
        /// <param name="thirdStart">A terceira dimensão inicial.</param>
        /// <param name="firstEndLength">A primeira dimensão final.</param>
        /// <param name="secondEndLength">A segunda dimensão final.</param>
        /// <param name="thirdEndLength">A terceira dimensão final.</param>
        private void InsertMoveItems(
            T[][][] items,
            ulong firstStart,
            ulong secondStart,
            ulong thirdStart,
            ulong firstEndLength,
            ulong secondEndLength,
            ulong thirdEndLength)
        {
            if (firstStart == firstEndLength)
            {
                var secondCurrent = items[firstStart];
                this.InsertMoveItemsFinal(
                        secondCurrent,
                        secondStart,
                        thirdStart,
                        secondEndLength,
                        thirdEndLength);
            }
            else
            {
                var firstIndex = firstStart;
                var secondCurrent = items[firstIndex];
                var carriage = this.InsertMoveItems(
                        secondCurrent,
                        secondStart,
                        thirdStart);
                ++firstIndex;
                for (; firstIndex < firstEndLength; ++firstIndex)
                {
                    secondCurrent = items[firstIndex];
                    var innerCarriage = this.InsertMoveItems(
                        secondCurrent,
                        0,
                        0);
                    secondCurrent[0][0] = carriage;
                    carriage = innerCarriage;
                }

                secondCurrent = items[firstIndex];
                this.InsertMoveItemsFinal(
                        secondCurrent,
                        0,
                        0,
                        secondEndLength,
                        thirdEndLength);
                this.elements[firstIndex][0][0] = carriage;
            }
        }

        /// <summary>
        /// Move os itens para remoção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="firstStart">A primeira dimensão inicial.</param>
        /// <param name="secondStart">A segunda dimensão inicial.</param>
        /// <param name="thirdStart">A terceira dimensão inicial.</param>
        /// <param name="firstLastIndex">A primeira dimensão final.</param>
        /// <param name="secondLastIndex">A segunda dimensão final.</param>
        /// <param name="thirdLastIndex">A terceira dimensão final.</param>
        private void RemoveMoveItems(
            T[][][] items,
            ulong firstStart,
            ulong secondStart,
            ulong thirdStart,
            ulong firstLastIndex,
            ulong secondLastIndex,
            ulong thirdLastIndex)
        {
            var lastIndex = firstLastIndex;
            if (firstStart == lastIndex)
            {
                var secondCurrent = items[firstStart];
                this.RemoveMoveItemsFinal(
                        secondCurrent,
                        secondStart,
                        thirdStart,
                        secondLastIndex,
                        thirdLastIndex);
            }
            else
            {
                var secondCurrent = items[lastIndex];
                var carriage = this.RemoveMoveItems(
                        secondCurrent,
                        secondLastIndex,
                        thirdLastIndex);
                --lastIndex;
                for (; lastIndex > firstStart; --lastIndex)
                {
                    secondCurrent = items[lastIndex];
                    var innerCarriage = this.RemoveMoveItems(
                        secondCurrent,
                        generalMask,
                        mask);
                    secondCurrent[generalMask][mask] = carriage;
                    carriage = innerCarriage;
                }

                secondCurrent = items[lastIndex];
                this.RemoveMoveItemsFinal(
                        secondCurrent,
                        secondStart,
                        thirdStart,
                        generalMask,
                        mask);
                this.elements[lastIndex][generalMask][mask] = carriage;
            }
        }

        /// <summary>
        /// Move os itens para inserção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="startSecondDim">A segunda dimensão inicial.</param>
        /// <param name="startThirdDim">A terceira dimensão inicial.</param>
        /// <returns>O valor do item que resta no final da ordenação dupla.</returns>
        private T InsertMoveItems(
            T[][] items,
            ulong startSecondDim,
            ulong startThirdDim)
        {
            var generalSize = generalMask + 1;
            var secondIndex = startSecondDim;
            var thirdCurrent = items[secondIndex];
            var result = this.InsertMoveItems(
                thirdCurrent,
                startThirdDim);
            ++secondIndex;
            for (; secondIndex < generalSize; ++secondIndex)
            {
                var thirdNext = items[secondIndex];
                var innerResult = this.InsertMoveItems(
                    thirdNext,
                    0);
                thirdNext[0] = result;
                thirdCurrent = thirdNext;
                result = innerResult;
            }

            return result;
        }

        /// <summary>
        /// Move os itens para remoção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="secondLastIndex">A segunda dimensão final.</param>
        /// <param name="thirdLastIndex">A terceira dimensão final.</param>
        /// <returns>O valor do item que resta no final da ordenação dupla.</returns>
        private T RemoveMoveItems(
            T[][] items,
            ulong secondLastIndex,
            ulong thirdLastIndex)
        {
            var secondIndex = secondLastIndex;
            var thirdCurrent = items[secondIndex];
            var result = this.RemoveMoveItems(
                thirdCurrent,
                thirdLastIndex);

            for (; secondIndex > 0; )
            {
                --secondIndex;
                var thirdNext = items[secondIndex];
                var innerResult = this.RemoveMoveItems(
                    thirdNext,
                    mask);
                thirdNext[mask] = result;
                thirdCurrent = thirdNext;
                result = innerResult;
            }

            return result;
        }

        /// <summary>
        /// Move os itens para inserção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="startSecondDim">A segunda dimensão inicial.</param>
        /// <param name="startThirdDim">A terceira dimensão inicial.</param>
        /// <param name="secondDimLength">O comprimento da segunda dimensão final.</param>
        /// <param name="thirdDimLength">O comprimento da terceira dimensão final.</param>
        private void InsertMoveItemsFinal(
            T[][] items,
            ulong startSecondDim,
            ulong startThirdDim,
            ulong secondDimLength,
            ulong thirdDimLength)
        {
            if (startSecondDim == secondDimLength)
            {
                var thirdCurrent = items[startSecondDim];
                this.InsertMoveItemsFinal(
                    thirdCurrent,
                    startThirdDim,
                    thirdDimLength - startThirdDim);
            }
            else
            {
                var secondIndex = startSecondDim;
                var thirdCurrent = items[secondIndex];
                var carriage = this.InsertMoveItems(
                       thirdCurrent,
                       startThirdDim);
                ++secondIndex;
                for (; secondIndex < secondDimLength; ++secondIndex)
                {
                    thirdCurrent = items[secondIndex];
                    var innerCarriage = this.InsertMoveItems(
                        thirdCurrent,
                        0L);
                    thirdCurrent[0] = carriage;
                    carriage = innerCarriage;
                }

                thirdCurrent = items[secondIndex];
                this.InsertMoveItemsFinal(
                    thirdCurrent,
                    0,
                    thirdDimLength);
                thirdCurrent[0] = carriage;
            }
        }

        /// <summary>
        /// Move os itens para remoção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="startSecondDim">A segunda dimensão inicial.</param>
        /// <param name="startThirdDim">A terceira dimensão inicial.</param>
        /// <param name="secondLastIndex">A segunda dimensão final.</param>
        /// <param name="thirdLastIndex">A terceira dimensão final.</param>
        private void RemoveMoveItemsFinal(
            T[][] items,
            ulong startSecondDim,
            ulong startThirdDim,
            ulong secondLastIndex,
            ulong thirdLastIndex)
        {
            if (startSecondDim == secondLastIndex)
            {
                var thirdCurrent = items[startSecondDim];
                this.RemoveMoveItemsFinal(
                    thirdCurrent,
                    startThirdDim,
                    thirdLastIndex - startThirdDim);
            }
            else
            {
                var secondIndex = secondLastIndex;
                var thirdCurrent = items[secondIndex];
                var carriage = this.RemoveMoveItems(
                       thirdCurrent,
                       thirdLastIndex);
                --secondIndex;
                for (; secondIndex > 0; --secondIndex)
                {
                    thirdCurrent = items[secondIndex];
                    var innerCarriage = this.RemoveMoveItems(
                        thirdCurrent,
                        mask);
                    thirdCurrent[mask] = carriage;
                    carriage = innerCarriage;
                }

                thirdCurrent = items[secondIndex];
                this.RemoveMoveItemsFinal(
                    thirdCurrent,
                    startThirdDim,
                    mask - startThirdDim);
                thirdCurrent[mask] = carriage;
            }
        }

        /// <summary>
        /// Move os itens para inserão.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="startThirdDim">A terceira diemnsão inicial.</param>
        /// <returns>O item que resta no final da ordenação.</returns>
        private T InsertMoveItems(
            T[] items,
            ulong startThirdDim)
        {
            var result = items[mask];
            Array.Copy(
                items,
                (long)startThirdDim,
                items,
                (long)(startThirdDim + 1),
                (long)(mask - startThirdDim));
            return result;
        }

        /// <summary>
        /// Move os itens para remoção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="lastThirdIndex">A terceira dimensão final.</param>
        /// <returns>O item que resta no final da ordenação.</returns>
        private T RemoveMoveItems(
            T[] items,
            ulong lastThirdIndex)
        {
            var result = items[0];
            Array.Copy(
                items,
                1,
                items,
                0,
                (long)lastThirdIndex);

            return result;
        }

        /// <summary>
        /// Move os itens para inserção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="startThirdDim">A terceira dimensão inicial.</param>
        /// <param name="thirdDimLength">O comprimento da terceira dimensão.</param>
        private void InsertMoveItemsFinal(
            T[] items,
            ulong startThirdDim,
            ulong thirdDimLength)
        {
            Array.Copy(
                items,
                (long)startThirdDim,
                items,
                (long)(startThirdDim + 1),
                (long)thirdDimLength);
        }

        /// <summary>
        /// Move os itens para inserção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="startThirdDim">A terceira dimensão inicial.</param>
        /// <param name="lastThirdIndex">A terceira dimensão final.</param>
        private void RemoveMoveItemsFinal(
            T[] items,
            ulong startThirdDim,
            ulong lastThirdIndex)
        {
            Array.Copy(
                items,
                (long)(startThirdDim + 1),
                items,
                (long)startThirdDim,
                (long)lastThirdIndex);
        }

        /// <summary>
        /// Obtém um valor que indica se o item proporcionado se encontra na colecção,
        /// de acordo com o comparador especificado.
        /// </summary>
        /// <param name="item">O item a ser verificado.</param>
        /// <param name="comparer">O comparador de itens.</param>
        /// <returns>Verdadeiro caso o item esteja contido na colecção e falso caso contrário.</returns>
        private bool InnerContains(T item, IEqualityComparer<T> comparer)
        {
            var firstLength = this.elements.Length;
            if (item == null)
            {
                for (int i = 0; i < firstLength; ++i)
                {
                    var curr = this.elements[i];
                    var secondLength = curr.Length;
                    for (int j = 0; j < secondLength; ++j)
                    {
                        var innerCurr = curr[j];
                        var thirdLength = innerCurr.Length;
                        for (int k = 0; k < thirdLength; ++k)
                        {
                            if (innerCurr[k] == null)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < firstLength; ++i)
                {
                    var curr = this.elements[i];
                    var secondLength = curr.Length;
                    for (int j = 0; j < secondLength; ++j)
                    {
                        var innerCurr = curr[j];
                        var thirdLength = innerCurr.Length;
                        for (int k = 0; k < thirdLength; ++k)
                        {
                            if (comparer.Equals(innerCurr[k], item))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Expande a capacidade nas funções de adição e inserção.
        /// </summary>
        /// <param name="minimum">O mínimo da expansão.</param>
        private void ExpandCapacity(ulong minimum)
        {
            if (this.capacity < minimum)
            {
                var maximumNumber = ulong.MaxValue;
                if (this.assertMemory)
                {
                    maximumNumber = this.GetMaximumAllocation();
                }

                if (maximumNumber <= this.capacity)
                {
                    throw new OutOfMemoryException("There is visible available memory to proceed.");
                }
                else if (this.capacity > (maximumNumber >> 1))
                {
                    this.capacity = minimum < maximumNumber ? minimum : maximumNumber;
                }
                else if (this.capacity == 0)
                {
                    var newCapacity = minimum < defaultCapacity ? minimum : defaultCapacity;
                    this.Instantiate(newCapacity);
                }
                else
                {
                    var aux = this.capacity << 1;
                    var newCapacity = aux < minimum ? minimum : aux;
                    this.IncreaseCapacityTo(newCapacity);
                }
            }
        }

        /// <summary>
        /// Obtém o número máximo de itens que podem ser alocados.
        /// </summary>
        /// <returns>O número máximo de itens.</returns>
        private ulong GetMaximumAllocation()
        {
            var memory = Utils.GetMemoryInfo().TotalVisibleMemorySize;
            var factor = 64UL;
            var objType = typeof(T);
            if (objType == typeof(byte) ||
                    objType == typeof(sbyte) ||
                    objType == typeof(bool))
            {
                factor = 1024;
            }
            else if (objType == typeof(char)
                || objType == typeof(short)
                || objType == typeof(ushort))
            {
                factor = 512;
            }
            else if (objType == typeof(int)
                || objType == typeof(uint)
                || objType == typeof(double))
            {
                factor = 256;
            }
            else if (objType == typeof(long)
                || objType == typeof(ulong)
                || objType == typeof(double))
            {
                factor = 128;
            }

            var itemsNumber = memory * factor;
            return itemsNumber;
        }

        /// <summary>
        /// Verifica a validade da memória visível disponibilizada pelo sistema operativo.
        /// </summary>
        /// <param name="size">O tamanho da colecção.</param>
        private void AssertVisibleMemory(ulong size)
        {
            if (this.assertMemory)
            {
                var itemsNumber = this.GetMaximumAllocation();
                if (itemsNumber < size)
                {
                    throw new OutOfMemoryException("There is no engough visible memory to proceed.");
                }
            }
        }

        /// <summary>
        /// Instancia a estrutura interna.
        /// </summary>
        /// <param name="capacity">O tamanho do vector.</param>
        private void Instantiate(ulong capacity)
        {
            var size = mask + 1;
            var generalSize = generalMask + 1;
            if (capacity == 0)
            {
                this.elements = new T[0][][];
            }
            else
            {
                var thirdDim = capacity & mask;
                var firstDim = capacity >> maxBinaryPower;
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
                    var secondDim = firstDim & generalMask;
                    firstDim >>= objMaxBinaryPower;
                    if (thirdDim == 0)
                    {
                        if (secondDim == 0)
                        {
                            var elems = new T[firstDim][][];
                            this.elements = elems;
                            for (var i = 0UL; i < firstDim; ++i)
                            {
                                var innerElem = new T[generalSize][];
                                elems[i] = innerElem;
                                for (int j = 0; j < generalSize; ++j)
                                {
                                    innerElem[j] = new T[size];
                                }
                            }
                        }
                        else
                        {
                            var elems = new T[firstDim + 1][][];
                            this.elements = elems;
                            for (var i = 0UL; i < firstDim; ++i)
                            {
                                var innerElem = new T[generalSize][];
                                elems[i] = innerElem;
                                for (int j = 0; j < generalSize; ++j)
                                {
                                    innerElem[j] = new T[size];
                                }
                            }

                            var innerElemOut = new T[secondDim][];
                            elems[firstDim] = innerElemOut;
                            for (var j = 0UL; j < secondDim; ++j)
                            {
                                innerElemOut[j] = new T[size];
                            }
                        }
                    }
                    else if (secondDim == generalSize)
                    {
                        var elems = new T[firstDim + 1][][];
                        this.elements = elems;
                        for (var i = 0UL; i < firstDim; ++i)
                        {
                            var innerElem = new T[generalSize][];
                            elems[i] = innerElem;
                            for (int j = 0; j < generalSize; ++j)
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
                        for (var i = 0UL; i < firstDim; ++i)
                        {
                            var innerElem = new T[generalSize][];
                            elems[i] = innerElem;
                            for (int j = 0; j < generalSize; ++j)
                            {
                                innerElem[j] = new T[size];
                            }
                        }

                        var innerElemOut = new T[secondDim + 1][];
                        elems[firstDim] = innerElemOut;
                        for (var i = 0UL; i < secondDim; ++i)
                        {
                            innerElemOut[i] = new T[size];
                        }

                        innerElemOut[secondDim] = new T[thirdDim];
                    }
                }
            }

            this.capacity = capacity;
        }

        /// <summary>
        /// Tenta obter o índice da primeira ocorrência do item especificado.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <param name="index">O índice da primeira ocorrência do item.</param>
        /// <returns>Verdadeiro se o item se encontrar na colecção e falso caso contrário.</returns>
        private bool TryGetIndexOfAux(T item, out ulong index)
        {
            var firstLength = (ulong)this.elements.Length;
            var generalSize = generalMask + 1;
            for (var i = 0UL; i < firstLength; ++i)
            {
                var secondElem = this.elements[i];
                var secondLength = (ulong)secondElem.Length;
                for (var j = 0UL; j < secondLength; ++j)
                {
                    var innerIndex = Array.IndexOf(secondElem, item);
                    if (innerIndex > -1)
                    {
                        checked
                        {
                            index = (i * generalSize + j) * (mask + 1) + (ulong)innerIndex;
                            return true;
                        }
                    }
                }
            }

            index = 0;
            return false;
        }

        /// <summary>
        /// Aumenta a capacidade da lista.
        /// </summary>
        /// <remarks>
        /// Supõe-se aqui que o valor da nova capacidade se encontra validado e que
        /// é possível estabelecer uma nova capacidade para a lista.
        /// </remarks>
        /// <param name="newCapacity">A nova capacidade a ser estabelecida.</param>
        private void IncreaseCapacityTo(ulong newCapacity)
        {
            var thirdDim = newCapacity & mask;
            var firstDim = newCapacity >> maxBinaryPower;
            var length = this.elements.Length;
            if (length == 0)
            {
                this.Instantiate(newCapacity);
            }
            else if (firstDim == 0)
            {
                this.elements[0][0] = this.ExpandArray(
                        this.elements[0][0],
                        newCapacity);
            }
            else
            {
                var size = mask + 1;
                var generalSize = generalMask + 1;
                var secondDim = firstDim & generalMask;
                firstDim >>= objMaxBinaryPower;
                if (thirdDim == 0)
                {
                    if (secondDim == 0)
                    {
                        var elementsLength = (ulong)this.elements.LongLength;
                        if (firstDim == elementsLength)
                        {
                            --elementsLength;
                            var current = this.elements[elementsLength];
                            if (current.LongLength == generalSize)
                            {
                                var genSize = generalSize - 1;
                                var innerCurrent = current[genSize];
                                if (innerCurrent.LongLength < size)
                                {
                                    current[genSize] = this.ExpandArray(
                                        innerCurrent,
                                        size);
                                }
                            }
                            else
                            {
                                this.elements[elementsLength] = this.ExpandDoubleArray(
                                    current,
                                    generalSize);

                            }
                        }
                        else
                        {
                            this.elements = this.ExpandTripleArray(
                                this.elements,
                                firstDim);
                        }
                    }
                    else
                    {
                        if (firstDim > (ulong)this.elements.Length - 1)
                        {
                            this.elements = this.ExpandTripleArray(
                                this.elements,
                                firstDim,
                                secondDim);
                        }
                        else
                        {
                            var current = this.elements[firstDim];
                            var currLength = (ulong)current.LongLength;
                            if (secondDim > currLength)
                            {
                                this.elements[firstDim] = this.ExpandDoubleArray(
                                this.elements[firstDim],
                                secondDim);
                            }
                            else
                            {
                                --currLength;
                                var innerCurrent = current[currLength];
                                current[currLength] = this.ExpandArray(
                                    innerCurrent,
                                    size);
                            }
                        }
                    }
                }
                else if (secondDim == generalSize)
                {
                    this.elements = this.ExpandTripleArray(
                        this.elements,
                        firstDim,
                        1,
                        thirdDim);
                }
                else
                {
                    var firstLength = (ulong)this.elements.LongLength;
                    if (firstDim == firstLength - 1)
                    {
                        var current = this.elements[firstDim];
                        var curreLen = (ulong)current.LongLength - 1;
                        if (secondDim == curreLen)
                        {
                            current[curreLen] = this.ExpandArray(
                                current[curreLen],
                                thirdDim);
                        }
                        else
                        {
                            this.elements[firstDim] = this.ExpandDoubleArray(
                                current,
                                secondDim,
                                thirdDim);
                        }
                    }
                    else
                    {
                        this.elements = this.ExpandTripleArray(
                            this.elements,
                            firstDim,
                            secondDim,
                            thirdDim);
                    }
                }
            }

            this.capacity = newCapacity;
        }

        /// <summary>
        /// Diminui a capacidade da lista.
        /// </summary>
        /// <param name="newCapacity">A nova capacidade.</param>
        private void DecreaseCapcityTo(ulong newCapacity)
        {
            var thirdDim = newCapacity & mask;
            var firstDim = newCapacity >> maxBinaryPower;
            var length = this.elements.Length;
            if (length == 0)
            {
                this.Instantiate(newCapacity);
            }
            if (firstDim == 0)
            {
                if (thirdDim == 0)
                {
                    this.elements = new T[0][][];
                }
                else
                {
                    this.elements = this.ContractTripleArray(
                        this.elements,
                        0,
                        0,
                        thirdDim);
                }
            }
            else
            {
                var size = mask + 1;
                var generalSize = generalMask + 1;
                var secondDim = firstDim & generalMask;
                firstDim >>= objMaxBinaryPower;
                if (thirdDim == 0)
                {
                    if (secondDim == 0)
                    {
                        this.elements = this.ContractTripleArray(
                            this.elements,
                            firstDim);
                    }
                    else
                    {
                        if (firstDim < (ulong)this.elements.Length - 1)
                        {
                            this.elements = this.ContractTripleArray(
                                this.elements,
                                firstDim,
                                secondDim);
                        }
                        else
                        {
                            this.elements[firstDim] = this.ContractDoubleArray(
                                this.elements[firstDim],
                                secondDim);
                        }
                    }
                }
                else if (secondDim == generalSize)
                {
                    if (firstDim < (ulong)this.elements.Length - 1)
                    {
                        this.elements = this.ContractTripleArray(
                            this.elements,
                            firstDim,
                            1,
                            thirdDim);
                    }
                    else
                    {
                        var current = this.ContractDoubleArray(
                            this.elements[firstDim],
                            1,
                            thirdDim);
                        this.elements[firstDim] = current;
                    }
                }
                else
                {
                    var firstLength = (ulong)this.elements.LongLength;
                    this.elements = this.ContractTripleArray(
                        this.elements,
                        firstDim,
                        secondDim,
                        thirdDim);
                }
            }

            this.capacity = newCapacity;
        }

        /// <summary>
        /// Expande a ordenação até ao valor especificado.
        /// </summary>
        /// <param name="array">A ordenação.</param>
        /// <param name="thirdDim">O valor especificado.</param>
        /// <returns>A ordenação expandida.</returns>
        private T[] ExpandArray(T[] array, ulong thirdDim)
        {
            var newArray = new T[thirdDim];
            Array.Copy(array, newArray, array.Length);
            return newArray;
        }

        /// <summary>
        /// Contrai a ordenação até ao valor especificado.
        /// </summary>
        /// <param name="array">A ordenação a ser contraída.</param>
        /// <param name="thirdDim">O valor especificado.</param>
        /// <returns>A ordenação contraída.</returns>
        private T[] ContractArray(T[] array, ulong thirdDim)
        {
            var newArray = new T[thirdDim];
            Array.Copy(array, newArray, newArray.LongLength);
            return newArray;
        }

        /// <summary>
        /// Expande a dupla ordenação até aos valores especificados.
        /// </summary>
        /// <remarks>
        /// O tamanho exterior refere-se apenas ao tamanho das ordenações
        /// completamente preenchidas, sendo automaticamente aumentado em uma unidade
        /// no caso do terceiro parâmetro.
        /// </remarks>
        /// <param name="doubleArray">A dupla ordenação.</param>
        /// <param name="secondDim">O tamanho da ordenação exterior.</param>
        /// <param name="thirdDim">O tamanho da última ordenação interior.</param>
        /// <returns>A ordenação expandida.</returns>
        private T[][] ExpandDoubleArray(
            T[][] doubleArray,
            ulong secondDim,
            ulong thirdDim)
        {
            var result = new T[secondDim + 1][];
            var outLength = doubleArray.LongLength;
            Array.Copy(doubleArray, result, outLength);
            var size = mask + 1;
            for (var i = (ulong)outLength; i < secondDim; ++i)
            {
                result[i] = new T[size];
            }

            result[secondDim] = new T[thirdDim];

            --outLength;
            var last = doubleArray[outLength];
            if (last.Length < size)
            {
                last = this.ExpandArray(last, size);
                result[outLength] = last;
            }

            return result;
        }

        /// <summary>
        /// Contrai uma ordenação dupla.
        /// </summary>
        /// <param name="doubleArray">A ordenação dupla.</param>
        /// <param name="secondDim">A segunda dimensão.</param>
        /// <param name="thirdDim">A terceira dimensão.</param>
        /// <returns>A ordenação contraída.</returns>
        private T[][] ContractDoubleArray(
            T[][] doubleArray,
            ulong secondDim,
            ulong thirdDim)
        {
            var result = new T[secondDim + 1][];
            var outLength = result.LongLength;
            Array.Copy(doubleArray, result, outLength);

            var newThirdArray = new T[thirdDim];
            var thirdArray = result[secondDim];
            Array.Copy(thirdArray, newThirdArray, newThirdArray.Length);
            result[secondDim] = newThirdArray;
            return result;
        }

        /// <summary>
        /// Expande a ordenação dupla até ao máximo em cada entrada.
        /// </summary>
        /// <param name="doubleArray">A ordenação dupla.</param>
        /// <param name="secondDim">O novo tamanho da expansão.</param>
        /// <returns>A ordenação expandida.</returns>
        private T[][] ExpandDoubleArray(
            T[][] doubleArray,
            ulong secondDim)
        {
            var result = new T[secondDim][];
            var outLength = doubleArray.LongLength;
            Array.Copy(doubleArray, result, outLength);
            var size = mask + 1;
            for (var i = (ulong)outLength; i < secondDim; ++i)
            {
                result[i] = new T[size];
            }

            --outLength;
            var last = doubleArray[outLength];
            if (last.Length < size)
            {
                last = this.ExpandArray(last, size);
                result[outLength] = last;
            }

            return result;
        }

        /// <summary>
        /// Contrai tamanho ao nível da segunda dimensão.
        /// </summary>
        /// <param name="doubleArray">A ordenação dupla.</param>
        /// <param name="secondDim">A segunda dimensão.</param>
        /// <returns>O resultado da contracção.</returns>
        private T[][] ContractDoubleArray(
            T[][] doubleArray,
            ulong secondDim)
        {
            var result = new T[secondDim][];
            var outLength = result.LongLength;
            Array.Copy(doubleArray, result, outLength);
            return result;
        }

        /// <summary>
        /// Expande a ordenação tripla ate ao máximo permitido.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla.</param>
        /// <param name="firstDim">O tamanho exterior máximo.</param>
        /// <returns>A ordenação tripla expandida.</returns>
        private T[][][] ExpandTripleArray(
            T[][][] tripleArray,
            ulong firstDim)
        {
            var result = new T[firstDim][][];
            var tripleLength = tripleArray.LongLength;
            Array.Copy(tripleArray, result, tripleLength);
            var generalSize = generalMask + 1;
            var size = mask + 1;
            for (var i = (ulong)tripleLength; i < firstDim; ++i)
            {
                var newArray = new T[generalSize][];
                result[i] = newArray;
                for (int j = 0; j < generalSize; ++j)
                {
                    newArray[j] = new T[size];
                }
            }

            --tripleLength;
            var outerCurrent = result[tripleLength];
            if (outerCurrent.Length < generalSize)
            {
                result[tripleLength] = this.ExpandDoubleArray(
                    outerCurrent,
                    generalSize);
            }
            else
            {
                var genSize = generalSize - 1;
                var innerCurrent = outerCurrent[genSize];
                if (innerCurrent.LongLength < size)
                {
                    outerCurrent[genSize] = this.ExpandArray(
                        innerCurrent,
                        size);
                }
            }

            return result;
        }

        /// <summary>
        /// Contrai a ordenação tripla.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla.</param>
        /// <param name="firstDim">A dimensão de contracção.</param>
        /// <returns>A ordenação contraída.</returns>
        private T[][][] ContractTripleArray(
            T[][][] tripleArray,
            ulong firstDim)
        {
            var result = new T[firstDim][][];
            var tripleLength = result.LongLength;
            Array.Copy(tripleArray, result, tripleLength);
            return result;
        }

        /// <summary>
        /// Expande a ordenação tripla até ao máximo permitido, adendando um novo item
        /// especificado pelo parâmetro.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla a ser expandida.</param>
        /// <param name="firstDim">O tamanho exterior que contém o máximo a ser atribuído.</param>
        /// <param name="secondDim">O tamanho da entrada interior a ser atribuída.</param>
        /// <returns>A ordenação tripla expandida.</returns>
        private T[][][] ExpandTripleArray(
            T[][][] tripleArray,
            ulong firstDim,
            ulong secondDim)
        {
            var result = new T[firstDim + 1][][];
            var tripleLength = tripleArray.LongLength;
            Array.Copy(tripleArray, result, tripleLength);
            var generalSize = generalMask + 1;
            var size = mask + 1;
            for (var i = (ulong)tripleLength; i < firstDim; ++i)
            {
                var newArray = new T[generalSize][];
                result[i] = newArray;
                for (int j = 0; j < generalSize; ++j)
                {
                    newArray[j] = new T[size];
                }
            }

            var newInnerArray = new T[secondDim][];
            result[firstDim] = newInnerArray;
            for (var i = 0UL; i < secondDim; ++i)
            {
                newInnerArray[i] = new T[size];
            }

            --tripleLength;
            var outerCurrent = result[tripleLength];
            if (outerCurrent.Length < generalSize)
            {
                result[tripleLength] = this.ExpandDoubleArray(
                    outerCurrent,
                    generalSize);
            }
            else
            {
                var genSize = generalSize - 1;
                var innerCurrent = outerCurrent[genSize];
                if (innerCurrent.LongLength < size)
                {
                    outerCurrent[genSize] = this.ExpandArray(
                        innerCurrent,
                        size);
                }
            }

            return result;
        }

        /// <summary>
        /// Contrai a ordenação tripla.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla a ser contraída.</param>
        /// <param name="firstDim">A primeira dimensão.</param>
        /// <param name="secondDim">A segunda dimensão.</param>
        /// <returns>A ordenação tripla contraída.</returns>
        private T[][][] ContractTripleArray(
            T[][][] tripleArray,
            ulong firstDim,
            ulong secondDim)
        {
            var result = new T[firstDim + 1][][];
            var tripleLength = result.LongLength;
            Array.Copy(tripleArray, result, tripleLength);

            --tripleLength;
            var outerCurrent = result[tripleLength];
            if ((ulong)outerCurrent.LongLength > secondDim)
            {
                result[tripleLength] = this.ContractDoubleArray(
                    outerCurrent,
                    secondDim);
            }

            return result;
        }

        /// <summary>
        /// Expande a ordenação tripla até ao máximo permitido, adendando
        /// um item cujo tamanho é especificado pelos restantes parâmetros.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla a ser expandida.</param>
        /// <param name="firstDim">O tamanho exterior que contém o máximo a ser atribuído.</param>
        /// <param name="secondDim">O tamanho interior que contém o máximo a ser atribuído.</param>
        /// <param name="thirdDim">O tamanho do item interior final.</param>
        /// <returns>A ordenação expandida.</returns>
        private T[][][] ExpandTripleArray(
            T[][][] tripleArray,
            ulong firstDim,
            ulong secondDim,
            ulong thirdDim)
        {
            var result = new T[firstDim + 1][][];
            var tripleLength = tripleArray.LongLength;
            Array.Copy(tripleArray, result, tripleLength);
            var generalSize = generalMask + 1;
            var size = mask + 1;
            for (var i = (ulong)tripleLength; i < firstDim; ++i)
            {
                var newArray = new T[generalSize][];
                result[i] = newArray;
                for (int j = 0; j < generalSize; ++j)
                {
                    newArray[j] = new T[size];
                }
            }

            var newInnerArray = new T[secondDim + 1][];
            result[firstDim] = newInnerArray;
            for (var i = 0UL; i < secondDim; ++i)
            {
                newInnerArray[i] = new T[size];
            }

            newInnerArray[secondDim] = new T[thirdDim];

            --tripleLength;
            var outerCurrent = result[tripleLength];
            if (outerCurrent.Length < generalSize)
            {
                result[tripleLength] = this.ExpandDoubleArray(
                    outerCurrent,
                    generalSize);
            }
            else
            {
                var genSize = generalSize - 1;
                var innerCurrent = outerCurrent[genSize];
                if (innerCurrent.LongLength < size)
                {
                    outerCurrent[genSize] = this.ExpandArray(
                        innerCurrent,
                        size);
                }
            }

            return result;
        }

        /// <summary>
        /// Contrai a ordenação tripla.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla a ser contraída.</param>
        /// <param name="firstDim">A primeira dimensão.</param>
        /// <param name="secondDim">A segunda dimensão.</param>
        /// <param name="thirdDim">A terceria dimensão.</param>
        /// <returns>A ordenação contraída.</returns>
        private T[][][] ContractTripleArray(
            T[][][] tripleArray,
            ulong firstDim,
            ulong secondDim,
            ulong thirdDim)
        {
            var result = new T[firstDim + 1][][];

            var tripleLength = result.LongLength;
            Array.Copy(tripleArray, result, tripleLength);

            --tripleLength;
            var outerCurrent = result[tripleLength];
            if ((ulong)outerCurrent.Length > secondDim)
            {
                result[tripleLength] = this.ContractDoubleArray(
                    outerCurrent,
                    secondDim,
                    thirdDim);
            }

            return result;
        }

        /// <summary>
        /// Obtém os tamanhos das dimensões associados ao valor especificado.
        /// </summary>
        /// <param name="value">O valor especificado.</param>
        /// <returns>Os tamanhos.</returns>
        private Tuple<ulong, ulong, ulong> GetSizes(ulong value)
        {
            var thirdDim = value & mask;
            var firstDim = value >> maxBinaryPower;
            if (firstDim == 0)
            {
                if (thirdDim == 0)
                {
                    return Tuple.Create(0UL, 0UL, 0UL);
                }
                else
                {
                    return Tuple.Create(1UL, 1UL, thirdDim);
                }
            }
            else
            {
                var generalSize = generalMask + 1;
                var secondDim = firstDim & generalMask;
                firstDim >>= objMaxBinaryPower;
                if (thirdDim == 0)
                {
                    if (secondDim > 0)
                    {
                        ++firstDim;
                    }
                }
                else if (secondDim == generalSize)
                {
                    secondDim = 1;
                    ++firstDim;
                }
                else
                {
                    ++secondDim;
                    ++firstDim;
                }

                return Tuple.Create(firstDim, secondDim, thirdDim);
            }
        }

        /// <summary>
        /// Verifica a validade da estrutura da ordenação.
        /// </summary>
        /// <param name="array">A ordenação.</param>
        /// <param name="dimensions">A descrição das dimensões.</param>
        private void AssertArrayStructure(
            Array array,
            long[] dimensions)
        {
            var rank = dimensions.Length;
            var objType = array.GetType();
            for (var i = 0; i < rank; ++i)
            {
                if (objType.IsArray && objType.GetArrayRank() == 1)
                {
                    objType = objType.GetElementType();
                }
                else
                {
                    throw new UtilitiesException(string.Format(
                        "Array must be rank one at level {0}.",
                        i));
                }
            }

            if (!objType.IsAssignableFrom(typeof(T)))
            {
                throw new ArrayTypeMismatchException("Source array type cannot be assigned to destination array type.");
            }
        }

        /// <summary>
        /// Realiza a cópia de uma ordenação para um conjunto de ordenações.
        /// </summary>
        /// <param name="array">A ordenação de partida.</param>
        /// <param name="length">O tamanho do vector a ser copiado.</param>
        /// <param name="arrays">A definição das ordenações de destino.</param>
        /// <param name="indexes">
        /// Os índices que definem o estado das ordenações de destino.
        /// </param>
        private void CopyCurrentArray(
            T[] array,
            long length,
            Array[] arrays,
            long[] indexes)
        {
            var rank = arrays.LongLength;
            var indexPointer = rank - 1;
            var currentIndex = indexes[indexPointer];
            var currentArray = arrays[indexPointer];
            var arrayLength = length;
            var arrayIndex = 0L;
            while (arrayLength > 0)
            {
                var difference = currentArray.LongLength - currentIndex;
                if (difference < arrayLength)
                {
                    Array.Copy(
                        array,
                        arrayIndex,
                        currentArray,
                        currentIndex,
                        difference);
                    arrayLength -= difference;
                    arrayIndex += difference;

                    // Actualiza o estado dos índices
                    var state = true;
                    while (state)
                    {
                        --indexPointer;
                        if (indexPointer < 0)
                        {
                            throw new ArgumentException(
                                "Destination array was not long enough. Check destIndex and length, and the array's lower bounds.");
                        }
                        else
                        {
                            currentIndex = indexes[indexPointer];
                            currentArray = arrays[indexPointer];
                            var currentArrayLength = currentArray.LongLength;
                            ++currentIndex;
                            if (currentIndex < currentArrayLength)
                            {
                                indexes[indexPointer] = currentIndex;
                                ++indexPointer;
                                var innerArray = (Array)currentArray.GetValue(currentIndex);
                                arrays[indexPointer] = innerArray;
                                indexes[indexPointer] = 0L;
                                currentArray = innerArray;
                                ++indexPointer;
                                for (; indexPointer < rank; ++indexPointer)
                                {
                                    innerArray = (Array)currentArray.GetValue(0);
                                    arrays[indexPointer] = innerArray;
                                    indexes[indexPointer] = 0L;
                                    currentArray = innerArray;
                                }

                                --indexPointer;
                                currentIndex = 0;
                                state = false;
                            }
                        }
                    }
                }
                else
                {
                    Array.Copy(
                        array,
                        arrayIndex,
                        currentArray,
                        currentIndex,
                        arrayLength);
                    currentIndex += arrayLength;
                    indexes[indexPointer] = currentIndex;

                    arrayLength = 0;
                }
            }
        }

        /// <summary>
        /// Incrementa as variáveis de comprimento em uma unidade.
        /// </summary>
        private void IncrementLength()
        {
            ++this.length;
            if (this.thirdDimLength == mask)
            {

                this.thirdDimLength = 0;
                if (this.secondDimLength == generalMask)
                {
                    this.secondDimLength = 0;
                    ++firstDimLength;
                }
                else
                {
                    ++this.secondDimLength;
                }
            }
            else
            {
                ++this.thirdDimLength;
            }
        }

        /// <summary>
        /// Decrementa as variáveis de comprimento em uma unidade.
        /// </summary>
        private void DecrementLength()
        {
            --this.length;
            if (this.thirdDimLength == 0)
            {
                this.thirdDimLength = mask;
                if (this.secondDimLength == 0)
                {
                    this.secondDimLength = generalMask;
                    --this.firstDimLength;
                }
                else
                {
                    --this.secondDimLength;
                }
            }
            else
            {
                --this.thirdDimLength;
            }
        }

        /// <summary>
        /// Aumenta as variáveis de tamanho em num número especificado
        /// de unidades.
        /// </summary>
        /// <param name="value">O número de unidades.</param>
        private void IncrementLength(ulong value)
        {
            this.length += value;
            var thirdDim = value & mask;
            var firstDim = value >> maxBinaryPower;
            var size = mask + 1;
            var generalSize = generalMask + 1;
            if (firstDim == 0)
            {
                if (thirdDim != 0)
                {
                    var lengthDiff = size - this.thirdDimLength;
                    if (lengthDiff > thirdDim)
                    {
                        this.thirdDimLength += thirdDim;
                    }
                    else
                    {
                        this.thirdDimLength = thirdDim - lengthDiff;
                        if (this.secondDimLength == generalSize)
                        {
                            this.secondDimLength = 0;
                            ++this.firstDimLength;
                        }
                        else
                        {
                            ++this.secondDimLength;
                        }
                    }
                }
                else
                {
                    this.thirdDimLength = value;
                }
            }
            else
            {
                var secondDim = firstDim & generalMask;
                firstDim >>= objMaxBinaryPower;
                var lengthdiff = size - this.thirdDimLength;
                if (lengthdiff > thirdDim)
                {
                    this.thirdDimLength += thirdDim;
                    lengthdiff = generalSize - this.secondDimLength;
                    if (lengthdiff > secondDim)
                    {
                        this.secondDimLength += secondDim;
                        this.firstDimLength += firstDim;
                    }
                    else
                    {
                        this.secondDimLength = secondDim - lengthdiff;
                        this.firstDimLength += firstDim + 1;
                    }
                }
                else
                {
                    this.thirdDimLength = thirdDim - lengthdiff;
                    lengthdiff = generalSize - this.secondDimLength - 1;
                    if (lengthdiff > secondDim)
                    {
                        this.secondDimLength += (secondDim + 1);
                        this.firstDimLength += firstDim;
                    }
                    else
                    {
                        this.secondDimLength = secondDim - lengthdiff;
                        this.firstDimLength += firstDim + 1;
                    }
                }
            }
        }

        /// <summary>
        /// Diminui as variáveis de comprimento em um número especificado
        /// de unidades.
        /// </summary>
        /// <param name="value">O número de unidades.</param>
        private void DecrementLength(ulong value)
        {
            this.length -= value;
            var thirdDim = value & mask;
            var firstDim = value >> maxBinaryPower;
            var size = mask + 1;
            var generalSize = generalMask + 1;
            if (firstDim == 0)
            {
                this.thirdDimLength -= value;
            }
            else
            {
                var secondDim = firstDim & generalMask;
                firstDim >>= objMaxBinaryPower;
                if (this.thirdDimLength < value)
                {
                    var innerValue = value - this.thirdDimLength;
                    if (this.secondDimLength < innerValue)
                    {
                        innerValue -= this.secondDimLength;
                        this.firstDimLength -= innerValue;
                    }
                    else
                    {
                        this.secondDimLength -= innerValue;
                    }
                }
                else
                {
                    this.thirdDimLength -= value;
                }
            }
        }

        #endregion Funções privadas
    }

    public class GeneralHashtable<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public TValue this[TKey key]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public ICollection<TKey> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public ICollection<TValue> Values
        {
            get { throw new NotImplementedException(); }
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private struct Bucket
        {
            public TKey Key;

            public TValue Value;


        }
    }
}
