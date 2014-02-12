namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Permite representar as operações sobre o subconjunto dos números inteiros representáveis por uma
    /// variável do tipo inteiro.
    /// </summary>
    public class IntegerDomain : IIntegerNumber<int>
    {
        public int MultiplicativeUnity
        {
            get { return 1; }
        }

        public int AdditiveUnity
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

        public int Multiply(int left, int right)
        {
            checked
            {
                return left * right;
            }
        }

        public int AdditiveInverse(int number)
        {
            return -number;
        }

        public int Add(int left, int right)
        {
            checked
            {
                return left + right;
            }
        }

        public int AddRepeated(int left, int right)
        {
            checked
            {
                return left * right;
            }
        }

        public bool Equals(int x, int y)
        {
            return x == y;
        }

        public int GetHashCode(int obj)
        {
            return obj.GetHashCode();
        }


        public bool IsAdditiveUnity(int value)
        {
            return value == 0;
        }

        public bool IsMultiplicativeUnity(int value)
        {
            return value == 1;
        }

        public int Quo(int dividend, int divisor)
        {
            return dividend / divisor;
        }

        public int Rem(int dividend, int divisor)
        {
            return dividend % divisor;
        }

        public uint Degree(int value)
        {
            return (uint)Math.Abs(value);
        }

        public DomainResult<int> GetQuotientAndRemainder(int dividend, int divisor)
        {
            return new DomainResult<int>(dividend / divisor, dividend % divisor);
        }

        public IEnumerable<int> Units
        {
            get
            {
                yield return 1;
                yield return -1;
            }
        }

        public int Predecessor(int number)
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

        public int Successor(int number)
        {
            if (number == int.MaxValue)
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

        public int MapFrom(int number)
        {
            return number;
        }

        public int MapFrom(long number)
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

        public int MapFrom(System.Numerics.BigInteger number)
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

        public int GetNorm(int value)
        {
            return (int)Math.Abs(value);
        }

        public int Compare(int x, int y)
        {
            return Comparer<int>.Default.Compare(x, y);
        }

        public int ConvertToInt(int number)
        {
            return number;
        }

        public long ConvertToLong(int number)
        {
            return number;
        }

        public BigInteger ConvertToBigInteger(int number)
        {
            return number;
        }
    }
}
