using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Mathematics
{
    public class UnivarPolynomNormalFormParser<
        CoeffType, 
        RingType,
        SymbType,
        SymbValue,
        InputReader>
        where RingType : IRing<CoeffType>
    {
        private RingType coeffRing;

        public UnivarPolynomNormalFormParser(RingType ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                this.coeffRing = ring;
            }
        }

        public UnivariatePolynomialNormalForm<CoeffType, RingType> ParsePolynomial(
             MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<CoeffType, SymbValue, SymbType> parser)
        {
            //var expressionReader = new ExpressionReader<UnivariatePolynomialNormalForm<CoeffType, RingType>, InputReader>(
            throw new NotImplementedException();
        }
    }
}
