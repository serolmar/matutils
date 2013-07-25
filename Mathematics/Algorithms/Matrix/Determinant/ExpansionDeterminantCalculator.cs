using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.Algorithms.Matrix.Determinant
{
    public class ExpansionDeterminantCalculator<ElementsType, RingType> : ADeterminant<ElementsType, RingType>
        where RingType : IRing<ElementsType>
    {
        public ExpansionDeterminantCalculator(RingType ring)
            : base(ring)
        {
        }

        protected override ElementsType ComputeDeterminant(IMatrix<ElementsType> data)
        {
            throw new NotImplementedException();
        }
    }
}
