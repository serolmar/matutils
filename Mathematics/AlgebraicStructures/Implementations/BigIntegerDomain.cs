namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    public class BigIntegerDomain : IRing<BigInteger>, IEuclidenDomain<BigInteger>
    {
        public BigInteger AdditiveUnity
        {
            get {
                return 0;
            }
        }

        public BigInteger MultiplicativeUnity
        {
            get {
                return 1;
            }
        }

        public BigInteger AddRepeated(BigInteger element, int times)
        {
            return element * times;
        }

        public BigInteger AdditiveInverse(BigInteger number)
        {
            return -number;
        }

        public bool IsAdditiveUnity(BigInteger value)
        {
            return value.IsZero;
        }

        public bool Equals(BigInteger x, BigInteger y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(BigInteger obj)
        {
            return obj.GetHashCode();
        }

        public BigInteger Add(BigInteger left, BigInteger right)
        {
            return left + right;
        }

        public bool IsMultiplicativeUnity(BigInteger value)
        {
            return value.IsOne;
        }

        public BigInteger Multiply(BigInteger left, BigInteger right)
        {
            return left * right;
        }

        public BigInteger Quo(BigInteger dividend, BigInteger divisor)
        {
            return dividend / divisor;
        }

        public BigInteger Rem(BigInteger dividend, BigInteger divisor)
        {
            return dividend % divisor;
        }

        public DomainResult<BigInteger> GetQuotientAndRemainder(BigInteger dividend, BigInteger divisor)
        {
            var remainder = default(BigInteger);
            var quotient = BigInteger.DivRem(dividend, divisor, out remainder);
            return new DomainResult<BigInteger>(quotient, remainder);
        }

        public uint Degree(BigInteger value)
        {
            return (uint)value;
        }
    }
}
