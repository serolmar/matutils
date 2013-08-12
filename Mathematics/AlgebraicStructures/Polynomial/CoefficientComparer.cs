using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    internal class CoefficientComparer<CoeffType> : Comparer<CoefficientDegreePair<CoeffType>>
    {
        private IComparer<int> degreeComparer;

        public CoefficientComparer(IComparer<int> degreeComparer)
        {
            this.degreeComparer = degreeComparer;
        }

        public IComparer<int> DegreeComparer
        {
            get
            {
                return this.degreeComparer;
            }
        }

        public override int Compare(
            CoefficientDegreePair<CoeffType> x, 
            CoefficientDegreePair<CoeffType> y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            else if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            else
            {
                return this.degreeComparer.Compare(x.Degree, y.Degree);
            }
        }
    }
}
