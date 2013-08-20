using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    /// <summary>
    /// Compounds the delimiter with operation it represents.
    /// </summary>
    /// <typeparam name="ObjType">The type of object being parsed.</typeparam>
    public class ExpressionCompoundDelimiter<ObjType, SymbType>
    {
        public ExpressionCompoundDelimiter()
        {
            this.DelimiterType = default(SymbType);
        }

        public SymbType DelimiterType { get; set; }
        public UnaryOperator<ObjType> DelimiterOperator { get; set; }

        public override bool Equals(object obj)
        {
            var delim = obj as ExpressionCompoundDelimiter<ObjType, SymbType>;
            if (delim == null)
            {
                return base.Equals(obj);
            }
            else
            {
                return this.DelimiterType.Equals(delim.DelimiterType);
            }
        }

        public override int GetHashCode()
        {
            if (this.DelimiterType == null)
            {
                return base.GetHashCode();
            }
            else
            {
                return this.DelimiterType.GetHashCode();
            }
        }
    }
}
