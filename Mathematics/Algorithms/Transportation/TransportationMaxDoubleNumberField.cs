using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Permite somar, subtrair e comparar números máximos, à semelhança dos números complexos.
    /// </summary>
    class TransportationMaxDoubleNumberField
    {
        double FiniteComponent { get; set; }
        double MaximumComponent { get; set; }

        public TransportationMaxDoubleNumberField() { }

        public TransportationMaxDoubleNumberField(double enteringValue)
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

        public static TransportationMaxDoubleNumberField operator +(TransportationMaxDoubleNumberField left, TransportationMaxDoubleNumberField right)
        {
            return new TransportationMaxDoubleNumberField()
            {
                FiniteComponent = left.FiniteComponent + right.FiniteComponent,
                MaximumComponent = left.MaximumComponent + right.MaximumComponent
            };
        }

        public static TransportationMaxDoubleNumberField operator -(TransportationMaxDoubleNumberField left, TransportationMaxDoubleNumberField right)
        {
            return new TransportationMaxDoubleNumberField()
            {
                FiniteComponent = left.FiniteComponent - right.FiniteComponent,
                MaximumComponent = left.MaximumComponent - right.MaximumComponent
            };
        }

        public static TransportationMaxDoubleNumberField operator -(double left, TransportationMaxDoubleNumberField right)
        {
            if (left == double.MaxValue)
            {
                return new TransportationMaxDoubleNumberField()
                {
                    FiniteComponent = -right.FiniteComponent,
                    MaximumComponent = 1 - right.MaximumComponent
                };
            }
            else if (left == double.MinValue)
            {
                return new TransportationMaxDoubleNumberField()
                {
                    FiniteComponent = -right.FiniteComponent,
                    MaximumComponent = -1 - right.MaximumComponent
                };
            }
            else
            {
                return new TransportationMaxDoubleNumberField()
                {
                    FiniteComponent = left - right.FiniteComponent,
                    MaximumComponent = -right.MaximumComponent
                };
            }
        }

        public static bool operator >(TransportationMaxDoubleNumberField left, TransportationMaxDoubleNumberField right)
        {
            return left.MaximumComponent > right.MaximumComponent ||
                (left.MaximumComponent == right.MaximumComponent && left.FiniteComponent > right.FiniteComponent);
        }

        public static bool operator >(TransportationMaxDoubleNumberField left, double right)
        {
            return (left.MaximumComponent > 1 && right == double.MaxValue) ||
                (left.MaximumComponent == 1 && right != double.MaxValue) ||
                (left.MaximumComponent > -1 && right == double.MinValue) ||
                (left.MaximumComponent == 0 && left.FiniteComponent > right);
        }

        public static bool operator >=(TransportationMaxDoubleNumberField left, TransportationMaxDoubleNumberField right)
        {
            return left.MaximumComponent >= right.MaximumComponent ||
                (left.MaximumComponent == right.MaximumComponent && left.FiniteComponent >= right.FiniteComponent);
        }

        public static bool operator <(TransportationMaxDoubleNumberField left, TransportationMaxDoubleNumberField right)
        {
            return left.MaximumComponent < right.MaximumComponent ||
                (left.MaximumComponent == right.MaximumComponent && left.FiniteComponent < right.FiniteComponent);
        }

        public static bool operator <(TransportationMaxDoubleNumberField left, double right)
        {
            return (left.MaximumComponent < 1 && right == double.MaxValue) ||
                (left.MaximumComponent < -1 && right == double.MinValue) ||
                (left.MaximumComponent == -1 && right != double.MinValue) ||
                (left.MaximumComponent == 0 && left.FiniteComponent < right);
        }

        public static bool operator <=(TransportationMaxDoubleNumberField left, TransportationMaxDoubleNumberField right)
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
            TransportationMaxDoubleNumberField objToCompare = obj as TransportationMaxDoubleNumberField;
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
