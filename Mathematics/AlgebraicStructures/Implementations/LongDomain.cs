namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    public class LongDomain : IIntegerNumber<long>
    {
        public long MultiplicativeUnity
        {
            get { return 1; }
        }

        public long AdditiveUnity
        {
            get { return 0; }
        }

        public int UnitsCount
        {
            get
            {
                return 2;
            }
        }

        public long Multiply(long left, long right)
        {
            checked
            {
                return left * right;
            }
        }

        public long AdditiveInverse(long number)
        {
            return -number;
        }

        public long Add(long left, long right)
        {
            checked
            {
                return left + right;
            }
        }

        public long AddRepeated(long left, int right)
        {
            checked
            {
                return left * right;
            }
        }

        public bool Equals(long x, long y)
        {
            return x == y;
        }

        public int GetHashCode(long obj)
        {
            return obj.GetHashCode();
        }

        public bool IsAdditiveUnity(long value)
        {
            return value == 0;
        }

        public bool IsMultiplicativeUnity(long value)
        {
            return value == 1;
        }

        public long Quo(long dividend, long divisor)
        {
            return dividend / divisor;
        }

        public long Rem(long dividend, long divisor)
        {
            return dividend % divisor;
        }

        public uint Degree(long value)
        {
            return (uint)Math.Abs(value);
        }

        public DomainResult<long> GetQuotientAndRemainder(long dividend, long divisor)
        {
            return new DomainResult<long>(dividend / divisor, dividend % divisor);
        }

        public IEnumerable<long> Units
        {
            get
            {
                yield return 1;
                yield return -1;
            }
        }

        public long Predecessor(long number)
        {
            if (number == int.MinValue)
            {
                throw new ArgumentException("The least number has no predecessor.");
            }
            else
            {
                checked
                {
                    return number - 1;
                }
            }
        }

        public long Successor(long number)
        {
            if (number == long.MaxValue)
            {
                throw new ArgumentException("The greatest number has no successor.");
            }
            else
            {
                checked
                {
                    return number + 1;
                }
            }
        }

        public long MapFrom(int number)
        {
            return number;
        }

        public long MapFrom(long number)
        {
            if (number < int.MinValue || number > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("number");
            }
            else
            {
                return (int)number;
            }
        }

        public long MapFrom(BigInteger number)
        {
            if (number < int.MinValue || number > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("number");
            }
            else
            {
                return (int)number;
            }
        }

        public long GetNorm(long value)
        {
            return (long)Math.Abs(value);
        }

        public int Compare(long x, long y)
        {
            return Comparer<long>.Default.Compare(x, y);
        }

        public int ConvertToInt(long number)
        {
            return (int)number;
        }

        public long ConvertToLong(long number)
        {
            return number;
        }

        public BigInteger ConvertToBigInteger(long number)
        {
            return number;
        }
    }
}
