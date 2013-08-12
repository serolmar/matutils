using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class UnivarSquareFreeDecomposition<CoeffType, FieldType> :
        IAlgorithm<UnivariatePolynomialNormalForm<CoeffType, FieldType>,
        List<UnivariatePolynomialNormalForm<CoeffType, FieldType>>>
        where FieldType : IField<CoeffType>
    {
        public List<UnivariatePolynomialNormalForm<CoeffType, FieldType>> Run(
            UnivariatePolynomialNormalForm<CoeffType, FieldType> data)
        {
            throw new NotImplementedException();
        }
    }
}
