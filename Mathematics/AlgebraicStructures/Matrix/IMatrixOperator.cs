using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IMatrixOperator<ObjectType, RingType>
        where RingType : IRing<ObjectType>
    {
        IMatrix<ObjectType> Add(IMatrix<ObjectType> left, IMatrix<ObjectType> right);

        IMatrix<ObjectType> Multiply(IMatrix<ObjectType> left, IMatrix<ObjectType> right);
    }
}
