using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    public abstract class GenericAffector : FastAfector
    {
        private int[] numberOfPossibleAffectationsByIndice = null;

        /// <summary>
        /// Instantiates a new instance of the <see cref="GenericAffector"/> class.
        /// </summary>
        /// <param name="count">The number of elements to permute or combine.</param>
        public GenericAffector(int count) : this(count, count)
        {
        }

        /// <summary>
        /// Instantiates a new instance of the <see cref="GenericAffector"/> class.
        /// </summary>
        /// <param name="count">The number of elements to permute or combine.</param>
        /// <param name="numberOfPlaces">The number of elements to be combine. For exemple, to permute p elements from a set of n.</param>
        public GenericAffector(int count, int numberOfPlaces)
        {
            if (count <= 0) throw new ArgumentException("Parameter count must be greater than zero.");
            if (numberOfPlaces <= 0 || numberOfPlaces > count) throw new ArgumentException("Parameter numberOfPlaces must be between one and count.");

            this.Count = count;
            this.NumberOfPlaces = numberOfPlaces;
        }

        /// <summary>
        /// Instantiates a new instance of the <see cref="GenericAffector"/> class.
        /// </summary>
        /// <param name="count">The number of elements to permute or combine.</param>
        /// <param name="numberOfPlaces">The number of elements to be combine. For exemple, to permute p elements from a set of n.</param>
        /// <param name="possibleAffectionsByIndice">An array containing how many times a specific element can be repeated during the affectation.</param>
        public GenericAffector(int count, int numberOfPlaces, ICollection<int> possibleAffectionsByIndice)
            : this(count, numberOfPlaces)
        {
            if (possibleAffectionsByIndice.Count != count) throw new ArgumentException("Parameter possibleAffectionsByIndice must have a number of elements given by count.");
            this.numberOfPossibleAffectationsByIndice = new int[possibleAffectionsByIndice.Count];

            foreach (var item in possibleAffectionsByIndice)
            {
                if (item < 0) throw new ArgumentException("Every element in parameter possibleAffectationsByIndices must be greater than zero.");
            }

            possibleAffectionsByIndice.CopyTo(this.numberOfPossibleAffectationsByIndice, 0);
        }

        //public override IEnumerator<int[]> GetEnumerator();

        public abstract class GenericAffectorEnumerator : FastAffectorEnumerator
        {
            protected int[] affectedIndices;

            public GenericAffectorEnumerator(GenericAffector affector)
                : base(affector)
            {
                this.currentAffectationIndices = new int[affector.NumberOfPlaces];
                this.affectedIndices = new int[this.thisFastAffector.Count];
                Array.Clear(this.affectedIndices, 0, this.thisFastAffector.NumberOfPlaces);
            }

            protected override bool VerifyRepetitions()
            {
                GenericAffector affector = this.thisFastAffector as GenericAffector;
                int indexBeingAffected = this.currentAffectationIndices[this.currentPointer];

                if (affector.numberOfPossibleAffectationsByIndice != null)
                {
                    if (this.affectedIndices[indexBeingAffected] == affector.numberOfPossibleAffectationsByIndice[indexBeingAffected])
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

            protected override bool CheckForCurrAffectIndicesValidity()
            {
                return this.currentAffectationIndices[this.currentPointer] != this.thisFastAffector.Count;
            }

            protected override void IncrementAffectations()
            {
                ++this.affectedIndices[this.currentAffectationIndices[this.currentPointer]];
            }

            protected override void DecrementAffectations()
            {
                --this.affectedIndices[this.currentAffectationIndices[this.currentPointer]];
            }
        }
    }
}
