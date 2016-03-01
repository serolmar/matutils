namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Representa um sub-vector definido com base nos índices.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem as entradas dos vectores.</typeparam>
    internal class SubVector<CoeffType> : IMathVector<CoeffType>
    {
        /// <summary>
        /// O vector original.
        /// </summary>
        private IMathVector<CoeffType> vector;

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
        public SubVector(IMathVector<CoeffType> vector, int[] subVectorIndices)
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
}
