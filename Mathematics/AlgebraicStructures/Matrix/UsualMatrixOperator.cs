using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class UsualMatrixOperator<ObjectType, RingType> : IMatrixOperator<ObjectType, RingType>
        where RingType : IRing<ObjectType>
    {
        private IMatrixFactory<ObjectType> matrixFactory;

        private RingType ring;

        public UsualMatrixOperator(IMatrixFactory<ObjectType> matrixFactory, RingType ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (matrixFactory == null)
            {
                throw new ArgumentNullException("matrixFactory");
            }
            else
            {
                this.ring = ring;
                this.matrixFactory = matrixFactory;
            }
        }

        public IMatrix<ObjectType> Add(IMatrix<ObjectType> left, IMatrix<ObjectType> right)
        {
            throw new NotImplementedException();
        }

        public IMatrix<ObjectType> Multiply(IMatrix<ObjectType> left, IMatrix<ObjectType> right)
        {
            throw new NotImplementedException();
        }
    }
}
