namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um afectador de combinações geral.
    /// </summary>
    public class CombinationAffector : GenericAffector
    {
        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CombinationAffector"/>.
        /// </summary>
        /// <param name="count">O número de elementos a ser combinado.</param>
        public CombinationAffector(int count) : base(count) { }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CombinationAffector"/>.
        /// </summary>
        /// <param name="count">O número de elementos a ser combinado.</param>
        /// <param name="numberOfPlaces">O número de lugares para a combinação.</param>
        public CombinationAffector(int count, int numberOfPlaces) : base(count, numberOfPlaces) { }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CombinationAffector"/>.
        /// </summary>
        /// <param name="count">O número de elementos a ser combinado.</param>
        /// <param name="numberOfPlaces">O número de lugares para a combinação.</param>
        /// <param name="possibleAffectionsByIndice">As afectações possíveis discriminadas por índice.</param>
        public CombinationAffector(int count, int numberOfPlaces, ICollection<int> possibleAffectionsByIndice) 
            : base(count, numberOfPlaces, possibleAffectionsByIndice) { }

        /// <summary>
        /// Obtém um enumerador genérico para o conjunto de combinações.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public override IEnumerator<int[]> GetEnumerator()
        {
            return new CombinationEnumerator(this);
        }

        /// <summary>
        /// Define um enumerador para as combinações.
        /// </summary>
        public class CombinationEnumerator : GenericAffectorEnumerator
        {
            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="CombinationEnumerator"/>.
            /// </summary>
            /// <param name="affector">O afectador de combinações.</param>
            public CombinationEnumerator(CombinationAffector affector)
                : base(affector)
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

                if (this.currentAffectationIndices[this.currentPointer] == this.thisFastAffector.Count) return false;
                return true;
            }
        }
    }
}
