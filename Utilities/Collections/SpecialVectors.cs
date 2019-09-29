namespace Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um sub-vector definido com base nos índices.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem as entradas dos vectores.</typeparam>
    public class SubVector<CoeffType> : IVector<CoeffType>
    {
        /// <summary>
        /// O vector original.
        /// </summary>
        private IVector<CoeffType> vector;

        /// <summary>
        /// Os índices que definem o sub-vector.
        /// </summary>
        private int[] subVectorIndices;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SubVector{CoeffType}"/>.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="subVectorIndices">The sub vector indices.</param>
        /// <exception cref="ArgumentNullException">subVectorIndices</exception>
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
        /// <value>A entrada do vector especificada pelo índice.</value>
        /// <param name="index">O índice que identifica a entrada do vector.</param>
        /// <returns>A entrada do vector.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice for negativo ou não for inferior ao tamanho do vector.
        /// </exception>
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
        /// <value>O tamanho do vector.</value>
        public int Length
        {
            get
            {
                return this.subVectorIndices.Length;
            }
        }

        /// <summary>
        /// Otbém o tamanho do vector.
        /// </summary>
        /// <value>O tamanho do vector.</value>
        public long LongLength
        {
            get
            {
                return this.subVectorIndices.LongLength;
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
            return new IntegerSequenceSubVector<CoeffType>(this, indices);
        }

        /// <summary>
        /// Permite trocar dois elementos do sub-vector sem influenciar o vector original.
        /// </summary>
        /// <param name="first">A primeira entrada a ser trocada.</param>
        /// <param name="second">A segunda entrada a ser trocada.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número da linha ou o número coluna for negativo ou não for inferior ao tamanho
        /// da respectiva dimensão.
        /// </exception>
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
            else if (index < 0 || index > this.subVectorIndices.LongLength)
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
                    var indices = this.subVectorIndices;
                    var indicesLength = indices.Length;
                    if (index + indicesLength < array.LongLength)
                    {
                        var pointer = index;
                        for (var i = 0; i < indicesLength; ++i)
                        {
                            array.SetValue(this.vector[this.subVectorIndices[i]], pointer++);
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
            else if (index < 0 || index > this.subVectorIndices.LongLength)
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
                    var indices = this.subVectorIndices;
                    var indicesLength = indices.LongLength;
                    if (index + indicesLength < array.LongLength)
                    {
                        var pointer = index;
                        for (var i = 0; i < indicesLength; ++i)
                        {
                            array.SetValue(this.vector[this.subVectorIndices[i]], pointer++);
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
        /// Obtém uma cópia do vector corrente.
        /// </summary>
        /// <returns>A cópia.</returns>
        public IVector<CoeffType> Clone()
        {
            var result = new SubVector<CoeffType>();
            result.vector = this.vector;
            result.subVectorIndices = new int[this.subVectorIndices.Length];
            Array.Copy(this.subVectorIndices, result.subVectorIndices, this.subVectorIndices.Length);
            return result;
        }

        /// <summary>
        /// Obtém o enumerador genérico para as entradas do vector.
        /// </summary>
        /// <returns>O enumerador genérico.</returns>
        public IEnumerator<CoeffType> GetEnumerator()
        {
            for (int i = 0; i < this.subVectorIndices.Length; ++i)
            {
                yield return this.vector[this.subVectorIndices[i]];
            }
        }

        /// <summary>
        /// Obtém o enumerador não genérico para as entradas do vector.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Representa um sub-vector especificado por uma sequência de inteiros.
    /// </summary>
    /// <typeparam name="CoeffType">O tiop de objectos que constituem as entradas dos vectores.</typeparam>
    public class IntegerSequenceSubVector<CoeffType> : IVector<CoeffType>
    {
        /// <summary>
        /// O vector principal.
        /// </summary>
        private IVector<CoeffType> vector;

        /// <summary>
        /// Os índices que definem o sub-vector.
        /// </summary>
        private IntegerSequence indicesSequence;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="IntegerSequenceSubVector{CoeffType}"/>.
        /// </summary>
        /// <param name="vector">O vector principal.</param>
        /// <param name="indicesSequence">A sequência de inteiros que define o sub-vector.</param>
        /// <exception cref="ArgumentNullException">Se o vector proporcionado for nulo.</exception>
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

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada pelo índice respectivo.
        /// </summary>
        /// <param name="index">O índice da entrada.</param>
        /// <returns>O valor contido na posição especificada pelo índice.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice for negativo ou não for inferior ao tamanho do vector.
        /// </exception>
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
                return this.indicesSequence.Count;
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
                return this.indicesSequence.Count;
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
        /// A troca de dois elementos de um sub-vector definido por uma sequência de inteiros não é suportada.
        /// </summary>
        /// <param name="first">A posição do primeiro elemento.</param>
        /// <param name="second">A posição do segundo elemento.</param>
        /// <exception cref="UtilitiesException">
        /// Sempre.
        /// </exception>
        public void SwapElements(int first, int second)
        {
            throw new UtilitiesException("Can't swap integer sequence sub-vector entries.");
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
            else if (index < 0 || index > this.indicesSequence.Count)
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
                    var indices = this.indicesSequence;
                    var indicesLength = indices.Count;
                    if (index + indicesLength < array.LongLength)
                    {
                        var pointer = index;
                        for (var i = 0; i < indicesLength; ++i)
                        {
                            // TODO: Melhorar o desempenho, considerando funções de CopyTo nos vectores e usando a função foreachblock
                            array.SetValue(this.vector[this.indicesSequence[i]], pointer++);
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
            else if (index < 0 || index > this.indicesSequence.Count)
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
                    var indices = this.indicesSequence;
                    var indicesLength = indices.Count;
                    if (index + indicesLength < array.LongLength)
                    {
                        var pointer = index;
                        for (var i = 0; i < indicesLength; ++i)
                        {
                            // TODO: Melhorar o desempenho, considerando funções de CopyTo nos vectores e usando a função foreachblock
                            array.SetValue(this.vector[this.indicesSequence[i]], pointer++);
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
        /// Constrói uma cópida do vector actual.
        /// </summary>
        /// <returns>A cópia do vector actual.</returns>
        public IVector<CoeffType> Clone()
        {
            return new IntegerSequenceSubVector<CoeffType>(this.vector, this.indicesSequence.Clone());
        }

        /// <summary>
        /// Obtém um enumerador para os elementos do vector.
        /// </summary>
        /// <returns>O enumerador para os elementos do vector.</returns>
        public IEnumerator<CoeffType> GetEnumerator()
        {
            foreach (var index in this.indicesSequence)
            {
                yield return this.vector[index];
            }
        }

        /// <summary>
        /// Obtém o enumerador não genérico para os elementos do vector.
        /// </summary>
        /// <returns>O enumerador não genérico para os elementos do vector.</returns>
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
