using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    public class CombinationAffector : GenericAffector
    {
        public CombinationAffector(int count) : base(count) { }
        public CombinationAffector(int count, int numberOfPlaces) : base(count, numberOfPlaces) { }
        public CombinationAffector(int count, int numberOfPlaces, ICollection<int> possibleAffectionsByIndice) : base(count, numberOfPlaces, possibleAffectionsByIndice) { }

        public override IEnumerator<int[]> GetEnumerator()
        {
            return new CombinationEnumerator(this);
        }

        public class CombinationEnumerator : GenericAffectorEnumerator
        {
            public CombinationEnumerator(CombinationAffector affector)
                : base(affector)
            {
            }

            protected override void ResetPointedIndex()
            {
                this.currentAffectationIndices[this.currentPointer] = this.currentAffectationIndices[this.currentPointer - 1];
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
