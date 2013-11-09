using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    public class PermutationBoxAffector : GenericBoxAffector
    {
        public PermutationBoxAffector(int[] elementsCount)
            : base(elementsCount)
        {
        }

        public PermutationBoxAffector(int[] elementsCount, int numberOfPlaces)
            : base(elementsCount, numberOfPlaces)
        {
        }

        public override IEnumerator<int[]> GetEnumerator()
        {
            return new PermutationBoxAffectorEnumerator(this, this.elementsCount.Length);
        }

        public class PermutationBoxAffectorEnumerator : GenericBoxAffectorEnumerator
        {
            public PermutationBoxAffectorEnumerator(PermutationBoxAffector affector, int numberOfAffectationIndices)
                : base(affector, numberOfAffectationIndices)
            {
            }

            protected override void ResetPointedIndex()
            {
                this.currentAffectationIndices[this.currentPointer] = 0;
            }

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
