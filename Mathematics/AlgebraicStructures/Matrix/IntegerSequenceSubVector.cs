namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Representa um sub-vector especificado por uma sequência de inteiros.
    /// </summary>
    /// <typeparam name="CoeffType">O tiop de objectos que constituem as entradas dos vectores.</typeparam>
    internal class IntegerSequenceSubVector<CoeffType> : IMathVector<CoeffType>
    {
        /// <summary>
        /// O vector principal.
        /// </summary>
        private IMathVector<CoeffType> vector;

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
        public IntegerSequenceSubVector(IMathVector<CoeffType> vector, IntegerSequence indicesSequence)
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
        /// <exception cref="MathematicsException">
        /// Sempre.
        /// </exception>
        public void SwapElements(int first, int second)
        {
            throw new MathematicsException("Can't swap integer sequence sub-vector entries.");
        }

        /// <summary>
        /// Determina se o vector actual é zero relativamente a um monóide.
        /// </summary>
        /// <param name="monoid">O monóide.</param>
        /// <returns>Verdadeiro caso o vector seja nulo e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Se o monóide fornecido for nulo.</exception>
        public bool IsNull(IMonoid<CoeffType> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else
            {
                for (int i = 0; i < this.indicesSequence.Count; ++i)
                {
                    if (!monoid.IsAdditiveUnity(this.vector[this.indicesSequence[i]]))
                    {
                        return false;
                    }
                }

                return true;
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
