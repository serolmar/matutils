namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um afectador de combinações onde cada lugar admite um número fixo de itens.
    /// </summary>
    public class CombinationBoxAffector : GenericBoxAffector
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="GenericBoxAffector"/>.
        /// </summary>
        /// <param name="elements">O número de repetições admissíveis.</param>
        public CombinationBoxAffector(int[] elementsCount)
            : base(elementsCount)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="GenericAffector"/>.
        /// </summary>
        /// <param name="elementsCount">O número de repetições admissíveis.</param>
        /// <param name="numberOfPlaces">O número de lugares.</param>
        public CombinationBoxAffector(int[] elementsCount, int numberOfPlaces)
            : base(elementsCount, numberOfPlaces)
        {
        }

        /// <summary>
        /// Obtém um enumerador para o afectador.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public override IEnumerator<int[]> GetEnumerator()
        {
            return new CombinationBoxAffectorEnumerator(this, this.elementsCount.Length);
        }

        /// <summary>
        /// Implementa o enumerador.
        /// </summary>
        public class CombinationBoxAffectorEnumerator : GenericBoxAffectorEnumerator
        {
            /// <summary>
            /// Inicializa uma nova instância da classe <see cref="CombinationBoxAffectorEnumerator"/>.
            /// </summary>
            /// <param name="affector">O afectador.</param>
            /// <param name="numberOfAffectationIndices">O número de índices a afectar.</param>
            public CombinationBoxAffectorEnumerator(GenericBoxAffector affector, int numberOfAffectationIndices)
                : base(affector, numberOfAffectationIndices)
            {
            }

            /// <summary>
            /// Inicializa os apontadores.
            /// </summary>
            protected override void ResetPointedIndex()
            {
                this.currentAffectationIndices[this.currentPointer] = this.currentAffectationIndices[this.currentPointer - 1];
            }

            /// <summary>
            /// Incrementa o apontador corrente.
            /// </summary>
            /// <returns>Verdadeiro caso o incremento seja possível e falso caso contrário.</returns>
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
