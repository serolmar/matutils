using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class FastDivisionFreeCharPolynomCalculator<ElementType, RingType> 
        : IAlgorithm<IMatrix<ElementType>, UnivariatePolynomialNormalForm<ElementType, RingType>>
        where RingType : IRing<ElementType>
    {
        protected string variableName;

        protected RingType ring;

        public FastDivisionFreeCharPolynomCalculator(string variableName, RingType ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (string.IsNullOrWhiteSpace(variableName))
            {
                throw new ArgumentException("Variable name must be non-empty.");
            }
            else
            {
                this.ring = ring;
                this.variableName = variableName;
            }
        }

        public UnivariatePolynomialNormalForm<ElementType, RingType> Run(IMatrix<ElementType> data)
        {
            if (data == null)
            {
                return new UnivariatePolynomialNormalForm<ElementType, RingType>(this.variableName, this.ring);
            }
            else
            {
                var lines = data.GetLength(0);
                var columns = data.GetLength(1);
                if (lines != columns)
                {
                    throw new MathematicsException("Determinants can only be applied to square matrices.");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
