using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// Implementa um afectador genérico que permite resolver vários tipos de problemas.
    /// </summary>
    public abstract class GenericAffector : FastAfector
    {
        /// <summary>
        /// O número de afectações possíveis por índice.
        /// </summary>
        private int[] numberOfPossibleAffectationsByIndice = null;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="GenericAffector"/>.
        /// </summary>
        /// <param name="count">O número de elementos a permutar ou combinar.</param>
        public GenericAffector(int count) : this(count, count)
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="GenericAffector"/>.
        /// </summary>
        /// <param name="count">O número de elementos a permutar ou combinar.</param>
        /// <param name="numberOfPlaces">O número de lugares para as permutações ou combinações.</param>
        /// <exception cref="ArgumentException">Se os argumentos não se encontrarem dentro dos limites.</exception>
        public GenericAffector(int count, int numberOfPlaces)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Parameter count must be greater than zero.");
            }

            if (numberOfPlaces <= 0 || numberOfPlaces > count)
            {
                throw new ArgumentException("Parameter numberOfPlaces must be between one and count.");
            }

            this.count = count;
            this.numberOfPlaces = numberOfPlaces;
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="GenericAffector"/>.
        /// </summary>
        /// <param name="count">O número de elementos a permutar ou combinar.</param>
        /// <param name="numberOfPlaces">O número de lugares para as permutações ou combinações.</param>
        /// <param name="possibleAffectionsByIndice">
        /// Um vector que contém o número de vezes que um índice pode ser afectado.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Se os argumentos não coincidirem em termos de número de afectações.
        /// </exception>
        public GenericAffector(int count, int numberOfPlaces, ICollection<int> possibleAffectionsByIndice)
            : this(count, numberOfPlaces)
        {
            if (possibleAffectionsByIndice.Count != count)
            {
                throw new ArgumentException(
                    "Parameter possibleAffectionsByIndice must have a number of elements given by count.");
            }

            this.numberOfPossibleAffectationsByIndice = new int[possibleAffectionsByIndice.Count];

            foreach (var item in possibleAffectionsByIndice)
            {
                if (item < 0)
                {
                    throw new ArgumentException(
                        "Every element in parameter possibleAffectationsByIndices must be greater than zero.");
                }
            }

            possibleAffectionsByIndice.CopyTo(this.numberOfPossibleAffectationsByIndice, 0);
        }

        /// <summary>
        /// Implementa um enumerador para um afectador genérico.
        /// </summary>
        public abstract class GenericAffectorEnumerator : FastAffectorEnumerator
        {
            /// <summary>
            /// O afectador.
            /// </summary>
            private GenericAffector currentAffector;

            /// <summary>
            /// Os índices afectados.
            /// </summary>
            protected int[] affectedIndices;

            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="GenericAffectorEnumerator"/>.
            /// </summary>
            /// <param name="affector">O afectador.</param>
            public GenericAffectorEnumerator(GenericAffector affector)
                : base(affector)
            {
                this.currentAffectationIndices = new int[affector.NumberOfPlaces];
                this.affectedIndices = new int[this.thisFastAffector.Count];
                this.currentAffector = affector;
                Array.Clear(this.affectedIndices, 0, this.thisFastAffector.NumberOfPlaces);
            }

            /// <summary>
            /// Verifica se o elemento corrente constitui ou não uma repetição.
            /// </summary>
            /// <returns>Verdadeiro caso existam repetições e falso caso contrário.</returns>
            protected override bool VerifyRepetitions()
            {
                int indexBeingAffected = this.currentAffectationIndices[this.currentPointer];

                if (this.currentAffector.numberOfPossibleAffectationsByIndice != null)
                {
                    if (this.affectedIndices[indexBeingAffected] == 
                        this.currentAffector.numberOfPossibleAffectationsByIndice[indexBeingAffected])
                    {
                        return false;
                    }
                }
                else
                {
                    if (this.affectedIndices[indexBeingAffected] == 1)
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// Verifica a validade da afectação actual.
            /// </summary>
            /// <returns>Verdadeiro caso a afectação seja vaálida e falso caso contrário.</returns>
            protected override bool CheckForCurrAffectIndicesValidity()
            {
                return this.currentAffectationIndices[this.currentPointer] != this.thisFastAffector.Count;
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
