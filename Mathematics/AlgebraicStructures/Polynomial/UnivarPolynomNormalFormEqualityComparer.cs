namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UnivarPolynomNormalFormEqualityComparer<CoeffType> : EqualityComparer<UnivariatePolynomialNormalForm<CoeffType>>
    {
        private IEqualityComparer<CoeffType> coeffsComparer;

        public UnivarPolynomNormalFormEqualityComparer(IEqualityComparer<CoeffType> coeffsComparer)
        {
            if (coeffsComparer == null)
            {
                throw new ArgumentNullException("coeffsComparer");
            }
            else
            {
                this.coeffsComparer = coeffsComparer;
            }
        }

        public override bool Equals(
            UnivariatePolynomialNormalForm<CoeffType> x, 
            UnivariatePolynomialNormalForm<CoeffType> y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x == null)
            {
                return false;
            }
            else if (y == null)
            {
                return false;
            }
            else
            {
                return x.Equals(y, this.coeffsComparer);
            }
        }

        public override int GetHashCode(UnivariatePolynomialNormalForm<CoeffType> obj)
        {
            if (obj == null)
            {
                return typeof(UnivariatePolynomialNormalForm<CoeffType>).GetHashCode();
            }
            else
            {
                return obj.GetHashCode(this.coeffsComparer);
            }
        }
    }
}
