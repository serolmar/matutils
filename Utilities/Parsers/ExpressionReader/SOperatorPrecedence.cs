using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public struct SOperatorPrecedence<TypeDelegate> : IComparer<SOperatorPrecedence<TypeDelegate>>
    {
        public TypeDelegate Op { get; set; }
        public int Precedence { get; set; }

        #region IComparer<OperatorPrecedence<TypeDelegate>> Members

        public int Compare(SOperatorPrecedence<TypeDelegate> x, SOperatorPrecedence<TypeDelegate> y)
        {
            return x.Precedence.CompareTo(y.Precedence);
        }

        #endregion
    }
}
