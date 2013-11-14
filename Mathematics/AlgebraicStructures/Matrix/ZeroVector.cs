namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Representa o vector de valores nulos relativamente a um monóide.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente.</typeparam>
    public class ZeroVector<CoeffType> : IVector<CoeffType>
    {
        /// <summary>
        /// O monóide responsável pela determinação do valor nulo.
        /// </summary>
        private IMonoid<CoeffType> monoid;

        /// <summary>
        /// O tamanho do vector.
        /// </summary>
        private int length;

        public ZeroVector(int length, IMonoid<CoeffType> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else if (length < 0)
            {
                throw new ArgumentException("The vector length must be non-negative.");
            }
            else
            {
                this.monoid = monoid;
                this.length = length;
            }
        }

        /// <summary>
        /// Obtém e atribui um valor à posição especificada do vector.
        /// </summary>
        /// <param name="index">O índice da posição.</param>
        /// <returns>O valor na posição.</returns>
        public CoeffType this[int index]
        {
            get
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("The index must be non-negative and less than the size of vector.");
                }
                else
                {
                    return this.monoid.AdditiveUnity;
                }
            }
            set
            {
                throw new MathematicsException("Zero vector is constant.");
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
        /// Obtém o sub-vector especificado pelo conjunto de índices.
        /// </summary>
        /// <param name="indices">Os índices que especificam o sub-vector.</param>
        /// <returns>O sub-vector.</returns>
        public IVector<CoeffType> GetSubVector(int[] indices)
        {
            return new SubVector<CoeffType>(this, indices);
        }

        public IVector<CoeffType> GetSubVector(IntegerSequence indices)
        {
            return new IntegerSequenceSubVector<CoeffType>(this, indices);
        }

        /// <summary>
        /// Troca dois elementos do vector.
        /// </summary>
        /// <param name="first">A posição do primeiro elemento.</param>
        /// <param name="second">A posição do segundo elemento.</param>
        public void SwapElements(int first, int second)
        {
        }

        /// <summary>
        /// Obtém um enumerador para o vector.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<CoeffType> GetEnumerator()
        {
            for (int i = 0; i < this.length; ++i)
            {
                yield return this.monoid.AdditiveUnity;
            }
        }

        /// <summary>
        /// Obtém um enumerador não genérico para o vector.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
