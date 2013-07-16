using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    public class CombinationBoxAffector : GenericBoxAffector
    {
        public CombinationBoxAffector(int[] elementsCount)
            : base(elementsCount)
        {
        }

        public CombinationBoxAffector(int[] elementsCount, int numberOfPlaces)
            : base(elementsCount, numberOfPlaces)
        {
        }

        public override IEnumerator<int[]> GetEnumerator()
        {
            return new CombinationBoxAffectorEnumerator(this, this.elementsCount.Length);
        }

        public class CombinationBoxAffectorEnumerator : GenericBoxAffectorEnumerator
        {
            public CombinationBoxAffectorEnumerator(GenericBoxAffector affector, int numberOfAffectationIndices)
                : base(affector, numberOfAffectationIndices)
            {
            }

            protected override void ResetPointedIndex()
            {
                this.currentAffectationIndices[this.currentPointer] = this.currentAffectationIndices[this.currentPointer - 1];
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
