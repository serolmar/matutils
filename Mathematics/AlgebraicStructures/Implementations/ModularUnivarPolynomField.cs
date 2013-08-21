using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.AlgebraicStructures.Implementations
{
    public class ModularUnivarPolynomField<ObjectType, FieldType> : IField<UnivariatePolynomialNormalForm<ObjectType, FieldType>>
        where FieldType : IField<ObjectType>
    {
        private UnivariatePolynomialNormalForm<ObjectType, FieldType> module;

        public UnivariatePolynomialNormalForm<ObjectType, FieldType> MultiplicativeUnity
        {
            get { throw new NotImplementedException(); }
        }

        public UnivariatePolynomialNormalForm<ObjectType, FieldType> AdditiveUnity
        {
            get { throw new NotImplementedException(); }
        }

        public UnivariatePolynomialNormalForm<ObjectType, FieldType> MultiplicativeInverse(UnivariatePolynomialNormalForm<ObjectType, FieldType> number)
        {
            throw new NotImplementedException();
        }

        public UnivariatePolynomialNormalForm<ObjectType, FieldType> AddRepeated(UnivariatePolynomialNormalForm<ObjectType, FieldType> element, int times)
        {
            throw new NotImplementedException();
        }

        public UnivariatePolynomialNormalForm<ObjectType, FieldType> AdditiveInverse(UnivariatePolynomialNormalForm<ObjectType, FieldType> number)
        {
            throw new NotImplementedException();
        }

        public bool IsAdditiveUnity(UnivariatePolynomialNormalForm<ObjectType, FieldType> value)
        {
            throw new NotImplementedException();
        }

        public bool Equals(
            UnivariatePolynomialNormalForm<ObjectType, FieldType> x, 
            UnivariatePolynomialNormalForm<ObjectType, FieldType> y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(UnivariatePolynomialNormalForm<ObjectType, FieldType> obj)
        {
            throw new NotImplementedException();
        }

        public UnivariatePolynomialNormalForm<ObjectType, FieldType> Add(
            UnivariatePolynomialNormalForm<ObjectType, FieldType> left, 
            UnivariatePolynomialNormalForm<ObjectType, FieldType> right)
        {
            throw new NotImplementedException();
        }

        public bool IsMultiplicativeUnity(UnivariatePolynomialNormalForm<ObjectType, FieldType> value)
        {
            throw new NotImplementedException();
        }

        public UnivariatePolynomialNormalForm<ObjectType, FieldType> Multiply(
            UnivariatePolynomialNormalForm<ObjectType, FieldType> left, 
            UnivariatePolynomialNormalForm<ObjectType, FieldType> right)
        {
            throw new NotImplementedException();
        }
    }
}
