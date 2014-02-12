namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    public class BigIntegerDomain : IIntegerNumber<BigInteger>
    {
        public BigInteger AdditiveUnity
        {
            get
            {
                return 0;
            }
        }

        public BigInteger MultiplicativeUnity
        {
            get
            {
                return 1;
            }
        }

        public int UnitsCount
        {
            get
            {
                return 2;
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

        public IEnumerable<BigInteger> Units
        {
            get
            {
                yield return 1;
                yield return -1;
            }
        }

        public BigInteger Predecessor(BigInteger number)
        {
            return number - 1;
        }

        public BigInteger Successor(BigInteger number)
        {
            return number + 1;
        }

        public BigInteger MapFrom(int number)
        {
            return number;
        }

        public BigInteger MapFrom(long number)
        {
            return number;
        }

        public BigInteger MapFrom(BigInteger number)
        {
            return number;
        }

        public BigInteger GetNorm(BigInteger value)
        {
            return BigInteger.Abs(value);
        }

        public int Compare(BigInteger x, BigInteger y)
        {
            return Comparer<BigInteger>.Default.Compare(x, y);
        }

        public int ConvertToInt(BigInteger number)
        {
            return (int)number;
        }

        public long ConvertToLong(BigInteger number)
        {
            return (long)number;
        }

        public BigInteger ConvertToBigInteger(BigInteger number)
        {
            return number;
        }
    }
}
