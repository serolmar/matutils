using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    /// <summary>
    /// Implementa de uma forma geral um distribuidor que constrói
    /// todas as permutações/combinações de colocar bolas num conjunto de caixas
    /// de modo que a cada corresponda apenas uma bola.
    /// </summary>
    public abstract class GenericBoxAffector : FastAfector
    {
        protected int[] elementsCount;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="GenericBoxAffector"/>.
        /// </summary>
        /// <param name="elements">O número de repetições admissíveis.</param>
        /// <exception cref="ArgumentNullException">
        /// Se o número de repetições for proporcionado por um apontador nulo.
        /// </exception>
        public GenericBoxAffector(int[] elementsCount)
        {
            if (elementsCount == null)
            {
                throw new ArgumentNullException("elementsCount");
            }

            this.count = this.TotalNumberOfElements(elementsCount);
            this.elementsCount = elementsCount;
            this.numberOfPlaces = this.Count;
        }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="GenericAffector"/>.
        /// </summary>
        /// <param name="elementsCount">O número de repetições admissíveis.</param>
        /// <param name="numberOfPlaces">O número de lugares.</param>
        /// <exception cref="ArgumentNullException">
        /// Se o número de repetições for proporcionado por um apontador nulo.
        /// </exception>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o número de lugares não se encontrar dentro dos limites.
        /// </exception>
        public GenericBoxAffector(int[] elementsCount, int numberOfPlaces)
        {
            if (elementsCount == null)
            {
                throw new ArgumentNullException("elementsCount");
            }

            this.count = this.TotalNumberOfElements(elementsCount);
            this.elementsCount = elementsCount;
            if (numberOfPlaces < 0 || numberOfPlaces > this.count)
            {
                throw new IndexOutOfRangeException(
                    "The number of places must be less than the total number of permutations indices.");
            }

            this.numberOfPlaces = numberOfPlaces;
        }

        /// <summary>
        /// Obtém o número total de elementos a serem permutados.
        /// </summary>
        /// <param name="elementsCount">A contagem dos índices a serem permutados.</param>
        /// <returns>O número total de elementos a serem permutados.</returns>
        /// <exception cref="ArgumentException">Se o número de elementos se encontrar fora dos limites.</exception>
        private int TotalNumberOfElements(int[] elementsCount)
        {
            var count = 0;
            for (int i = 0; i < elementsCount.Length; ++i)
            {
                if (elementsCount[i] <= 0)
                {
                    throw new ArgumentException("Parameter count must be greater than zero.");
                }

                count += elementsCount[i];
            }

            return count;
        }

        /// <summary>
        /// O enumerador definido para o distribuidor geral.
        /// </summary>
        public abstract class GenericBoxAffectorEnumerator : FastAffectorEnumerator
        {
            /// <summary>
            /// O número de índices a serem afectados.
            /// </summary>
            protected int numberOfAffectationIndices;

            /// <summary>
            /// A contagem dos índices afectados.
            /// </summary>
            protected int[] affectedIndices;

            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="GenericBoxAffectorEnumerator"/>.
            /// </summary>
            /// <param name="genericBoxAffector">O afectador.</param>
            /// <param name="numberOfAffectationIndices">O número de índices de afectação.</param>
            public GenericBoxAffectorEnumerator(
                GenericBoxAffector genericBoxAffector, 
                int numberOfAffectationIndices)
                : base(genericBoxAffector)
            {
                this.numberOfAffectationIndices = numberOfAffectationIndices;
                this.currentAffectationIndices = new int[genericBoxAffector.NumberOfPlaces];
                this.affectedIndices = new int[this.thisFastAffector.Count];
                Array.Clear(this.affectedIndices, 0, this.thisFastAffector.NumberOfPlaces);
            }

            /// <summary>
            /// Verifica se o índice afectado durante o procecsso é válido. Neste caso,
            /// a permutação não poderá conter índices não especificados na contagem.
            /// </summary>
            /// <returns>Verdadeiro se o índice é permitido e falso caso contrário.</returns>
            protected override bool CheckForCurrAffectIndicesValidity()
            {
                var affector = this.thisFastAffector as GenericBoxAffector;
                return this.currentAffectationIndices[this.currentPointer] != affector.elementsCount.Length;
            }

            /// <summary>
            /// Verifica se o elemento corrente constitui ou não uma repetição.
            /// </summary>
            /// <returns>Verdadeiro caso existam repetições e falso caso contrário.</returns>
            protected override bool VerifyRepetitions()
            {
                var affector = this.thisFastAffector as GenericBoxAffector;
                int indexBeingAffected = this.currentAffectationIndices[this.currentPointer];

                if (this.affectedIndices[indexBeingAffected] == affector.elementsCount[indexBeingAffected])
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            /// <summary>
            /// Incrementa o contador para a verificação de repetições.
            /// </summary>
            protected override void IncrementAffectations()
            {
                ++this.affectedIndices[this.currentAffectationIndices[this.currentPointer]];
            }

            /// <summary>
            /// Decremente o contador para verificação de repetições.
            /// </summary>
            protected override void DecrementAffectations()
            {
                --this.affectedIndices[this.currentAffectationIndices[this.currentPointer]];
            }
        }
    }
}
