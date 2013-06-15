using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.AlgebraicStructures
{
    public class IntegerRing : IRing<int>
    {
        public int MultiplicativeUnity
        {
            get { return 1; }
        }

        public int Multiply(int left, int right)
        {
            return left * right;
        }

        public int AdditiveInverse(int number)
        {
            return -number;
        }

        public int AdditiveUnity
        {
            get { return 0; }
        }

        public int Add(int left, int right)
        {
            return left + right;
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
    }
}
