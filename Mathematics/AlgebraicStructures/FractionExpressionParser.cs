namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    public class FractionExpressionParser<ObjectType, DomainType> : IParse<Fraction<ObjectType, DomainType>, string, string>
        where DomainType : IEuclidenDomain<ObjectType>
    {
        protected FractionField<ObjectType, DomainType> fractionField;

        protected ExpressionReader<Fraction<ObjectType, DomainType>, string, string> expressionReader;

        public FractionExpressionParser(
            IParse<ObjectType, string, string> simpleObjectParser, 
            FractionField<ObjectType, DomainType> fractionField)
        {
            if (fractionField == null)
            {
                throw new ArgumentNullException("fractionField");
            }
            else if (simpleObjectParser == null)
            {
                throw new ArgumentNullException("simpleObjectParser");
            }
            else
            {
                this.fractionField = fractionField;
                this.expressionReader = new ExpressionReader<Fraction<ObjectType, DomainType>, string, string>(
                    new FractionParser<ObjectType, DomainType>(simpleObjectParser, fractionField.EuclideanDomain));
                this.expressionReader.RegisterBinaryOperator("plus", Add, 0);
                this.expressionReader.RegisterBinaryOperator("times", Multiply, 1);
                this.expressionReader.RegisterBinaryOperator("minus", Subtract, 0);
                this.expressionReader.RegisterUnaryOperator("minus", Symmetric, 0);
                this.expressionReader.RegisterBinaryOperator("over", Divide, 1);
                this.expressionReader.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
                this.expressionReader.RegisterSequenceDelimiterTypes("left_parenthesis", "right_parenthesis");
                this.expressionReader.AddVoid("blancks");
                this.expressionReader.AddVoid("space");
                this.expressionReader.AddVoid("carriage_return");
                this.expressionReader.AddVoid("new_line");
            }
        }

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out Fraction<ObjectType, DomainType> value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.expressionReader.TryParse(arrayReader, out value);
        }

        protected virtual Fraction<ObjectType, DomainType> Add(Fraction<ObjectType, DomainType> i, Fraction<ObjectType, DomainType> j)
        {
            return this.fractionField.Add(i, j);
        }

        protected virtual Fraction<ObjectType, DomainType> Subtract(Fraction<ObjectType, DomainType> i, Fraction<ObjectType, DomainType> j)
        {
            return this.fractionField.Add(i, this.fractionField.AdditiveInverse(j));
        }

        protected virtual Fraction<ObjectType, DomainType> Multiply(Fraction<ObjectType, DomainType> i, Fraction<ObjectType, DomainType> j)
        {
            return this.fractionField.Multiply(i, j);
        }

        protected virtual Fraction<ObjectType, DomainType> Divide(Fraction<ObjectType, DomainType> i, Fraction<ObjectType, DomainType> j)
        {
            return this.fractionField.Multiply(i, this.fractionField.MultiplicativeInverse(j));
        }

        protected virtual Fraction<ObjectType, DomainType> Symmetric(Fraction<ObjectType, DomainType> i)
        {
            return this.fractionField.AdditiveInverse(i);
        }
    }
}
