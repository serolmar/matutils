using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    public class PermutationAffector : GenericAffector
    {
        protected int[] affectedIndices;
        public PermutationAffector(int count) : base(count) { }
        public PermutationAffector(int count, int numberOfPlaces) : base(count, numberOfPlaces) { }
        public PermutationAffector(int count, int numberOfPlaces, ICollection<int> possibleAffectionsByIndice) : base(count, numberOfPlaces, possibleAffectionsByIndice) { }

        public override IEnumerator<int[]> GetEnumerator()
        {
            return new PermutationEnumerator(this);
        }

        public class PermutationEnumerator : GenericAffectorEnumerator
        {
            public PermutationEnumerator(PermutationAffector affector) : base(affector)
            {
            }

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

            protected override void ResetPointedIndex()
            {
                this.currentAffectationIndices[this.currentPointer] = 0;
            }

            protected override bool IncrementCurrent()
            {
                ++this.currentAffectationIndices[this.currentPointer];

                if (this.currentAffectationIndices[this.currentPointer] == this.thisFastAffector.Count) return false;
                return true;
            }
        }
    }
}
