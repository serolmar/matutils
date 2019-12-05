namespace Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um vector cujo contentor consiste num vector do sistema.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem as entradas do vector.</typeparam>
    public class ArrayVector<CoeffType> : IVector<CoeffType>
    {
        /// <summary>
        /// Mantém o conjunto de entradas do vector.
        /// </summary>
        protected CoeffType[] vectorEntries;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayVector{CoeffType}"/>.
        /// </summary>
        /// <param name="length">O comprimento do vector.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o comprimento for negativo.</exception>
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

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayVector{CoeffType}"/>.
        /// </summary>
        /// <param name="length">O comprimento do vector.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o comprimento for negativo.</exception>
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

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ArrayVector{CoeffType}"/>.
        /// </summary>
        protected ArrayVector()
        {
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
        /// Obtém e atribui o valor da entrada especificada pelo índice respectivo.
        /// </summary>
        /// <param name="index">O índice da entrada.</param>
        /// <returns>O valor contido na posição especificada pelo índice.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice for negativo ou não for inferior ao tamanho do vector.
        /// </exception>
        public CoeffType this[long index]
        {
            get
            {
                if (index < 0L || index >= this.vectorEntries.LongLength)
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
                if (index < 0 || index >= this.vectorEntries.LongLength)
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
        /// <value>
        /// O tamanho do vector.
        /// </value>
        public int Length
        {
            get
            {
                return this.vectorEntries.Length;
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
                return this.vectorEntries.LongLength;
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
        /// <param name="first">A posição do primeiro elemento.</param>
        /// <param name="second">A posição do segundo elemento.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se algum dos índices for negativo ou não for inferior ao tamanho do vector.
        /// </exception>
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

        /// <summary>
        /// Copia o conteúdo do vector para um alcance.
        /// </summary>
        /// <param name="array">O alcance.</param>
        /// <param name="index">O índice a partir do qual se inicia a cópia.</param>
        public void CopyTo(Array array, int index)
        {
            this.vectorEntries.CopyTo(array, index);
        }

        /// <summary>
        /// Copia o conteúdo do vector para um alcance.
        /// </summary>
        /// <param name="array">O alcance.</param>
        /// <param name="index">O índice a partir do qual se inicia a cópia.</param>
        public void CopyTo(Array array, long index)
        {
            this.vectorEntries.CopyTo(array, index);
        }

        /// <summary>
        /// Obtém um enumerador para os elementos do vector.
        /// </summary>
        /// <returns>O enumerador para os elementos do vector.</returns>
        public IEnumerator<CoeffType> GetEnumerator()
        {
            for (int i = 0; i < this.vectorEntries.Length; ++i)
            {
                yield return this.vectorEntries[i];
            }
        }

        /// <summary>
        /// Constrói uma cópida do vector actual.
        /// </summary>
        /// <returns>A cópia do vector actual.</returns>
        public virtual IVector<CoeffType> Clone()
        {
            var result = new ArrayVector<CoeffType>();
            result.vectorEntries = new CoeffType[this.vectorEntries.Length];
            Array.Copy(this.vectorEntries, result.vectorEntries, this.vectorEntries.Length);
            return result;
        }

        /// <summary>
        /// Obtém uma representação textual do vector.
        /// </summary>
        /// <returns>A representação textual do vector.</returns>
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
