namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Possibilita o cálculo da raiz quadrada inteira de um inteiro grande.
    /// </summary>
    /// <remarks>
    /// Ver a primeira resposta em: http://stackoverflow.com/questions/3432412/calculate-square-root-of-a-biginteger-system-numerics-biginteger
    /// </remarks>
    public class BigIntSquareRootAlgorithm : IAlgorithm<BigInteger, BigInteger>
    {
        public BigInteger Run(BigInteger data)
        {
            if (data < 0)
            {
                throw new ArgumentException("Can't compute the square root of a negative number.");
            }
            else if (data == 0)
            {
                return data;
            }
            else
            {
                var allBytes = data.ToByteArray();
                var bytesNumber = allBytes.Length;
                var b = bytesNumber << 2;
                --b;
                var r = BigInteger.Zero;
                var r2 = BigInteger.Zero; 
                while (b >= 0)
                {
                    var sr2 = r2;
                    var sr = r;

                    r2 += ((r << (1 + b)) + (BigInteger.One << (b + b)));
                    r += (BigInteger.One << b);
                    if (r2 > data)
                    {
                        r = sr;
                        r2 = sr2;
                    }

                    b--;
                }

                return r;
            }
        }
    }
}
