using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Mathematics.MathematicsInterpreter
{
    class StringMathematicsObject : AMathematicsObject
    {
        private string value;

        public StringMathematicsObject()
            : base(EMathematicsType.STRING_VALUE, false)
        {
        }

        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        public override bool CanConvertTo(EMathematicsType mathematicsType)
        {
            if (mathematicsType == EMathematicsType.STRING_VALUE)
            {
                return true;
            }
            else if (mathematicsType == EMathematicsType.INTEGER_VALUE)
            {
                var temp = 0;
                if (int.TryParse(this.value, out temp))
                {
                    return true;
                }
            }
            else if (mathematicsType == EMathematicsType.ASSIGN)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.BOOLEAN_VALUE)
            {
                var temp = true;
                if (bool.TryParse(this.value, out temp))
                {
                    return true;
                }
            }
            else if (mathematicsType == EMathematicsType.CONDITION)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.DOUBLE_VALUE)
            {
                var temp = 0.0;
                if (double.TryParse(this.value, NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out temp))
                {
                    return true;
                }
            }
            else if (mathematicsType == EMathematicsType.EMPTY)
            {
                if (string.IsNullOrWhiteSpace(this.value))
                {
                    return true;
                }
            }
            else if (mathematicsType == EMathematicsType.LIST)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.NAME)
            {
            }
            else if (mathematicsType == EMathematicsType.POLYNOMIAL)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.RANGE)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.SET)
            {
                throw new NotImplementedException();
            }

            return false;
        }

        public override AMathematicsObject ConvertTo(EMathematicsType mathematicsType)
        {
            if (mathematicsType == EMathematicsType.STRING_VALUE)
            {
                return this;
            }
            else if (mathematicsType == EMathematicsType.INTEGER_VALUE)
            {
                var temp = 0;
                if (int.TryParse(this.value, out temp))
                {
                    return new IntegerMathematicsObject() { Value = temp };
                }
            }
            else if (mathematicsType == EMathematicsType.ASSIGN)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.BOOLEAN_VALUE)
            {
                var temp = true;
                if (bool.TryParse(this.value, out temp))
                {
                    return new BooleanMathematicsObject() { Value = temp };
                }
            }
            else if (mathematicsType == EMathematicsType.CONDITION)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.DOUBLE_VALUE)
            {
                var temp = 0.0;
                if (double.TryParse(this.value, NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out temp))
                {
                    return new DoubleMathematicsObject() { Value = temp };
                }
            }
            else if (mathematicsType == EMathematicsType.EMPTY)
            {
                if (string.IsNullOrWhiteSpace(this.value))
                {
                    return new EmptyMathematicsObject();
                }
            }
            else if (mathematicsType == EMathematicsType.LIST)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.NAME)
            {
            }
            else if (mathematicsType == EMathematicsType.POLYNOMIAL)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.RANGE)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.SET)
            {
                throw new NotImplementedException();
            }

            throw new ExpressionInterpreterException(string.Format("Can't convert from type {0} to type {1}.", this.mathematicsType, mathematicsType));
        }
    }
}
