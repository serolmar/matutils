namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um afectador de permutações onde cada lugar admite um número fixo de itens.
    /// </summary>
    public class PermutationBoxAffector : GenericBoxAffector
    {
        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="PermutationBoxAffector"/>.
        /// </summary>
        /// <param name="elementsCount">O número de repetições admissíveis.</param>
        public PermutationBoxAffector(int[] elementsCount)
            : base(elementsCount)
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="PermutationBoxAffector"/>.
        /// </summary>
        /// <param name="elementsCount">O número de repetições admissíveis.</param>
        /// <param name="numberOfPlaces">O número de lugares.</param>
        public PermutationBoxAffector(int[] elementsCount, int numberOfPlaces)
            : base(elementsCount, numberOfPlaces)
        {
        }

        /// <summary>
        /// Obtém um enumerador para o afectador.
        /// </summary>
        /// <returns>
        /// O enumerador.
        /// </returns>
        public override IEnumerator<int[]> GetEnumerator()
        {
            return new PermutationBoxAffectorEnumerator(this, this.elementsCount.Length);
        }

        /// <summary>
        /// Implementa um enumerador para o afectador.
        /// </summary>
        public class PermutationBoxAffectorEnumerator : GenericBoxAffectorEnumerator
        {
            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="PermutationBoxAffectorEnumerator"/>.
            /// </summary>
            /// <param name="affector">O afectador.</param>
            /// <param name="numberOfAffectationIndices">O número de índices de afectação.</param>
            public PermutationBoxAffectorEnumerator(PermutationBoxAffector affector, int numberOfAffectationIndices)
                : base(affector, numberOfAffectationIndices)
            {
            }

            /// <summary>
            /// Inicializa o índice actual.
            /// </summary>
            protected override void ResetPointedIndex()
            {
                this.currentAffectationIndices[this.currentPointer] = 0;
            }

            /// <summary>
            /// Incremente o índice actual.
            /// </summary>
            /// <returns>
            /// Verdadeiro se o índice pode ser incrementado e falso caso contrário.
            /// </returns>
            protected override bool IncrementCurrent()
            {
                ++this.currentAffectationIndices[this.currentPointer];
                if (this.currentAffectationIndices[this.currentPointer] == this.numberOfAffectationIndices)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
