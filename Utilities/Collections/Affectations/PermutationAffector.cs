namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um afectador de permutações.
    /// </summary>
    public class PermutationAffector : GenericAffector
    {
        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="PermutationAffector"/>.
        /// </summary>
        /// <param name="count">O número de elementos a permutar.</param>
        public PermutationAffector(int count)
            : base(count)
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="PermutationAffector"/>.
        /// </summary>
        /// <param name="count">O número de elementos a permutar ou combinar.</param>
        /// <param name="numberOfPlaces">O número de lugares para as permutações ou combinações.</param>
        public PermutationAffector(int count, int numberOfPlaces)
            : base(count, numberOfPlaces)
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="PermutationAffector"/>.
        /// </summary>
        /// <param name="count">O número de elementos a permutar ou combinar.</param>
        /// <param name="numberOfPlaces">O número de lugares para as permutações ou combinações.</param>
        /// <param name="possibleAffectionsByIndice">Um vector que contém o número de vezes que um índice pode ser afectado.</param>
        public PermutationAffector(int count, int numberOfPlaces, ICollection<int> possibleAffectionsByIndice)
            : base(count, numberOfPlaces, possibleAffectionsByIndice)
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
            return new PermutationEnumerator(this);
        }

        /// <summary>
        /// Implementa um enumerador para o fectador de permutações.
        /// </summary>
        public class PermutationEnumerator : GenericAffectorEnumerator
        {
            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="PermutationEnumerator"/>.
            /// </summary>
            /// <param name="affector">O afectador.</param>
            public PermutationEnumerator(PermutationAffector affector)
                : base(affector)
            {
            }

            /// <summary>
            /// Cria um cópia do enumerador.
            /// </summary>
            /// <returns>A cópia.</returns>
            public PermutationEnumerator Clone()
            {
                PermutationEnumerator resultEnumerator = new PermutationEnumerator(this.thisFastAffector as PermutationAffector);
                resultEnumerator.currentPointer = this.currentPointer;
                resultEnumerator.isBeforeStart = this.isBeforeStart;
                resultEnumerator.isAfterEnd = this.isAfterEnd;
                resultEnumerator.affectedIndices = new int[this.affectedIndices.Length];
                Array.Copy(this.affectedIndices, resultEnumerator.affectedIndices, this.affectedIndices.Length);
                resultEnumerator.currentAffectationIndices = new int[this.currentAffectationIndices.Length];
                Array.Copy(this.currentAffectationIndices, resultEnumerator.currentAffectationIndices, this.currentAffectationIndices.Length);

                return resultEnumerator;
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

                if (this.currentAffectationIndices[this.currentPointer] == this.thisFastAffector.Count)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
