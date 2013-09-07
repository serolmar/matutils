using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Permite somar, subtrair e comparar números máximos, à semelhança dos números complexos.
    /// </summary>
    class MaxDoubleNumberField
    {
        double FiniteComponent { get; set; }
        int MaximumComponent { get; set; }

        public MaxDoubleNumberField() { }

        public MaxDoubleNumberField(double enteringValue)
        {
            if (enteringValue == double.MaxValue)
            {
                this.MaximumComponent = 1;
            }
            else if (enteringValue == double.MinValue)
            {
                this.MaximumComponent = -1;
            }
            else
            {
                this.FiniteComponent = enteringValue;
            }
        }

        public static MaxDoubleNumberField operator +(MaxDoubleNumberField left, MaxDoubleNumberField right)
        {
            return new MaxDoubleNumberField()
            {
                FiniteComponent = left.FiniteComponent + right.FiniteComponent,
                MaximumComponent = left.MaximumComponent + right.MaximumComponent
            };
        }

        public static MaxDoubleNumberField operator -(MaxDoubleNumberField left, MaxDoubleNumberField right)
        {
            return new MaxDoubleNumberField()
            {
                FiniteComponent = left.FiniteComponent - right.FiniteComponent,
                MaximumComponent = left.MaximumComponent - right.MaximumComponent
            };
        }

        public static MaxDoubleNumberField operator -(double left, MaxDoubleNumberField right)
        {
            if (left == double.MaxValue)
            {
                return new MaxDoubleNumberField()
                {
                    FiniteComponent = -right.FiniteComponent,
                    MaximumComponent = 1 - right.MaximumComponent
                };
            }
            else if (left == double.MinValue)
            {
                return new MaxDoubleNumberField()
                {
                    FiniteComponent = -right.FiniteComponent,
                    MaximumComponent = -1 - right.MaximumComponent
                };
            }
            else
            {
                return new MaxDoubleNumberField()
                {
                    FiniteComponent = left - right.FiniteComponent,
                    MaximumComponent = -right.MaximumComponent
                };
            }
        }

        public static bool operator >(MaxDoubleNumberField left, MaxDoubleNumberField right)
        {
            return left.MaximumComponent > right.MaximumComponent ||
                (left.MaximumComponent == right.MaximumComponent && left.FiniteComponent > right.FiniteComponent);
        }

        public static bool operator >(MaxDoubleNumberField left, double right)
        {
            return (left.MaximumComponent > 1 && right == double.MaxValue) ||
                (left.MaximumComponent == 1 && right != double.MaxValue) ||
                (left.MaximumComponent > -1 && right == double.MinValue) ||
                (left.MaximumComponent == 0 && left.FiniteComponent > right);
        }

        public static bool operator >=(MaxDoubleNumberField left, MaxDoubleNumberField right)
        {
            return left.MaximumComponent >= right.MaximumComponent ||
                (left.MaximumComponent == right.MaximumComponent && left.FiniteComponent >= right.FiniteComponent);
        }

        public static bool operator <(MaxDoubleNumberField left, MaxDoubleNumberField right)
        {
            return left.MaximumComponent < right.MaximumComponent ||
                (left.MaximumComponent == right.MaximumComponent && left.FiniteComponent < right.FiniteComponent);
        }

        public static bool operator <(MaxDoubleNumberField left, double right)
        {
            return (left.MaximumComponent < 1 && right == double.MaxValue) ||
                (left.MaximumComponent < -1 && right == double.MinValue) ||
                (left.MaximumComponent == -1 && right != double.MinValue) ||
                (left.MaximumComponent == 0 && left.FiniteComponent < right);
        }

        public static bool operator <=(MaxDoubleNumberField left, MaxDoubleNumberField right)
        {
            return left.MaximumComponent <= right.MaximumComponent ||
                (left.MaximumComponent == right.MaximumComponent && left.FiniteComponent <= right.FiniteComponent);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            MaxDoubleNumberField objToCompare = obj as MaxDoubleNumberField;
            if (objToCompare == null)
            {
                return base.Equals(obj);
            }
            else
            {
                return this.MaximumComponent == objToCompare.MaximumComponent && this.FiniteComponent == objToCompare.FiniteComponent;
            }
        }

        public override int GetHashCode()
        {
            return (this.MaximumComponent.GetHashCode() ^ this.FiniteComponent.GetHashCode()).GetHashCode();
        }
    }
}
