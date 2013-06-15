using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.MathematicsInterpreter
{
    public enum EMathematicsType
    {
        ASSIGN,
        CONDITION,
        NAME,
        INTEGER_VALUE,
        DOUBLE_VALUE,
        BOOLEAN_VALUE,
        STRING_VALUE,
        POLYNOMIAL,
        RANGE,
        LIST,
        SET,
        EMPTY
    }
}
