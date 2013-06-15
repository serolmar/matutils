using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    class OperatorDefinition<SymbType>
    {
        public OperatorDefinition() { }

        public OperatorDefinition(SymbType symbol, EOperatorType operatorType)
        {
            this.Symbol = symbol;
            this.OperatorType = operatorType;
        }

        public SymbType Symbol { get; set; }

        public EOperatorType OperatorType { get; set; }
    }
}
