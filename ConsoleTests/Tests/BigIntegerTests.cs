namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Text.RegularExpressions;
    using Mathematics;

    /// <summary>
    /// Permite realizar alguams experiências com números inteiros.
    /// </summary>
    public class BigIntegerTests
    {
        /// <summary>
        /// Permite decompor um número inteiro enorme positivo num vector
        /// de longos.
        /// </summary>
        /// <param name="number">A representação textual do número a ser convertido.</param>
        /// <returns>O número convertido.</returns>
        public ulong[] DecomposeNumber(string number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                var integerExpression = new Regex("^\\s*(\\d+)\\s*$");
                if (integerExpression.IsMatch(number))
                {
                    var baseNumb = BigInteger.One << 64;
                    var bigNumb = BigInteger.Parse(number);
                    var resultList = new List<ulong>();
                    while (bigNumb > BigInteger.Zero)
                    {
                        var rem = default(BigInteger);
                        var quo = BigInteger.DivRem(bigNumb, baseNumb, out rem);
                        resultList.Add((ulong)rem);
                        bigNumb = quo;
                    }

                    return resultList.ToArray();
                }
                else
                {
                    throw new Exception("O número fornecido é inválido. São apenas admitidos números positivos.");
                }
            }
        }
    }
}
