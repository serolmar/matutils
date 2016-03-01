namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Representa o vector de valores nulos relativamente a um monóide.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente.</typeparam>
    public class ZeroVector<CoeffType> : IMathVector<CoeffType>
    {
        /// <summary>
        /// O monóide responsável pela determinação do valor nulo.
        /// </summary>
        private IMonoid<CoeffType> monoid;

        /// <summary>
        /// O tamanho do vector.
        /// </summary>
        private int length;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ZeroVector{CoeffType}"/>.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        /// <param name="monoid">O monóide responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">Se o monóide for nulo.</exception>
        /// <exception cref="ArgumentException">Se o comprimento do vector for negativo.</exception>
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
        /// <value>O valor da posição especificada.</value>
        /// <param name="index">O índice da posição.</param>
        /// <returns>O valor na posição.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se a posição especificada for negativa ou não for inferior ao tamanho do vector.
        /// </exception>
        /// <exception cref="MathematicsException">Se for realizada alguma tentativa de atribuição.</exception>
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
        /// <value>O tamanho do vector.</value>
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

        /// <summary>
        /// Obtém o sub-vector especificado pelo conjunto de índices.
        /// </summary>
        /// <param name="indices">Os índices que especificam o sub-vector.</param>
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
        public void SwapElements(int first, int second)
        {
        }

        /// <summary>
        /// Determina se o vector é nulo, o que é sempre verdadeiro.
        /// </summary>
        /// <param name="monoid">O monóide responsável pelas operações sobre os coeficientes.</param>
        /// <returns>Verdadeiro.</returns>
        public bool IsNull(IMonoid<CoeffType> monoid)
        {
            return true;
        }

        /// <summary>
        /// Constrói uma cópida do vector actual.
        /// </summary>
        /// <returns>A cópia do vector actual.</returns>
        public IVector<CoeffType> Clone()
        {
            return this;
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
