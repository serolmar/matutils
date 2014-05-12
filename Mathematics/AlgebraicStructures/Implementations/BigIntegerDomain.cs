namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Proporicona operações de inteiros sobre números do tipo <see cref="BigInteger"/>.
    /// </summary>
    public class BigIntegerDomain : IIntegerNumber<BigInteger>
    {
        /// <summary>
        /// Obtém a unidade aditiva.
        /// </summary>
        /// <value>
        /// A unidade aditiva.
        /// </value>
        public BigInteger AdditiveUnity
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Obtém a unidade multiplicativa.
        /// </summary>
        /// <value>
        /// A unidade multiplicativa.
        /// </value>
        public BigInteger MultiplicativeUnity
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Obtém o número de unidades associadas ao espaço de inteiros.
        /// </summary>
        /// <value>
        /// O número de unidades.
        /// </value>
        public int UnitsCount
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Permite adicionar um número tantas vezes quantas as especificadas.
        /// </summary>
        /// <remarks>
        /// Por vezes o processo de adição repetida pode ser realizado de uma forma mais eficaz do que
        /// efectuar o algoritmo habitual que envolve um conjunto de somas.
        /// </remarks>
        /// <param name="element">O número a ser adicionado.</param>
        /// <param name="times">O número de vezes que se efectua a adição.</param>
        /// <returns>O resultado da adição repetida.</returns>
        public BigInteger AddRepeated(BigInteger element, int times)
        {
            return element * times;
        }

        /// <summary>
        /// Obtém a inversa aditiva de um número.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>A inversa aditiva.</returns>
        public BigInteger AdditiveInverse(BigInteger number)
        {
            return -number;
        }

        /// <summary>
        /// Determina se um determinado valor constitui uma unidade aditiva.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>Verdadeiro caso o valor seja uma unidade aditiva e falso caso contrário.</returns>
        public bool IsAdditiveUnity(BigInteger value)
        {
            return value.IsZero;
        }

        /// <summary>
        /// Determina se os objectos especificados são iguais.
        /// </summary>
        /// <param name="x">O primeiro objecto do tipo <paramref name="T" /> a ser comparado.</param>
        /// <param name="y">O segundo objecto do tipo <paramref name="T" /> a ser comparado.</param>
        /// <returns>
        /// Verdadeiro caso os objectos sejam iguais e falso caso contrário.
        /// </returns>
        public bool Equals(BigInteger x, BigInteger y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Retorna um código confuso da instância.
        /// </summary>
        /// <param name="obj">A instância.</param>
        /// <returns>
        /// O código confuso da instância que pode ser utilizado em vários algoritmos habituais.
        /// </returns>
        public int GetHashCode(BigInteger obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Calcula a soma de dois números.
        /// </summary>
        /// <param name="left">O primeiro número.</param>
        /// <param name="right">O segundo número.</param>
        /// <returns>O resultado da soma.</returns>
        public BigInteger Add(BigInteger left, BigInteger right)
        {
            return left + right;
        }

        /// <summary>
        /// Determina se o valor especificado é uma unidade multiplicativa.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>Verdadeiro caso o valor seja uma unidade multiplicativa e falso caso contrário.</returns>
        public bool IsMultiplicativeUnity(BigInteger value)
        {
            return value.IsOne;
        }

        /// <summary>
        /// Calcula o produto de dois números.
        /// </summary>
        /// <param name="left">O primeiro número.</param>
        /// <param name="right">O segundo número.</param>
        /// <returns>O resultado do produto.</returns>
        public BigInteger Multiply(BigInteger left, BigInteger right)
        {
            return left * right;
        }

        /// <summary>
        /// Calcula o quociente inteiro entre dois números.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>O resultado do quociente.</returns>
        public BigInteger Quo(BigInteger dividend, BigInteger divisor)
        {
            return dividend / divisor;
        }

        /// <summary>
        /// Calcula o resto da divisão entre dois números.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>O resto da divisão.</returns>
        public BigInteger Rem(BigInteger dividend, BigInteger divisor)
        {
            return dividend % divisor;
        }

        /// <summary>
        /// Obtém o quociente e o resto da divisão entre dois números.
        /// </summary>
        /// <remarks>
        /// Em determinadas ocasiões é necessário obter tanto o valor do quociente como do resto. No entanto,
        /// na maior parte das situações, ambos os valores são calculados recorrendo ao mesmo algoritmo. Assim,
        /// ao invés de executar o algoritmo para obter cada um desses valores em separado, uma única execução
        /// proporciona o resultado desejado.
        /// </remarks>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>O quociente e o resto da divisão dos números.</returns>
        public DomainResult<BigInteger> GetQuotientAndRemainder(BigInteger dividend, BigInteger divisor)
        {
            var remainder = default(BigInteger);
            var quotient = BigInteger.DivRem(dividend, divisor, out remainder);
            return new DomainResult<BigInteger>(quotient, remainder);
        }

        /// <summary>
        /// Obtém o grau do valor encarado como elemento de um domínio euclideano.
        /// Ver também <see cref="IEuclideanDomain"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>O grau do valor.</returns>
        public uint Degree(BigInteger value)
        {
            return (uint)value;
        }

        /// <summary>
        /// Obtém as unidades associadas ao conjunto de números.
        /// </summary>
        /// <value>
        /// As unidades.
        /// </value>
        public IEnumerable<BigInteger> Units
        {
            get
            {
                yield return 1;
                yield return -1;
            }
        }

        /// <summary>
        /// Obtém o predecessor de um número.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O predecessor.</returns>
        public BigInteger Predecessor(BigInteger number)
        {
            return number - 1;
        }

        /// <summary>
        /// Obtém o sucessor de um número.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O sucessor.</returns>
        public BigInteger Successor(BigInteger number)
        {
            return number + 1;
        }

        /// <summary>
        /// Mapeia um número a partir de um inteiro.
        /// </summary>
        /// <param name="number">O inteiro.</param>
        /// <returns>O número mapeado.</returns>
        public BigInteger MapFrom(int number)
        {
            return number;
        }

        /// <summary>
        /// Mapeia um número a partir de um longo.
        /// </summary>
        /// <param name="number">O longo.</param>
        /// <returns>O número.</returns>
        public BigInteger MapFrom(long number)
        {
            return number;
        }

        /// <summary>
        /// Mapeia um número a partir de um inteiro de precisão arbitrária.
        /// </summary>
        /// <param name="number">O inteiro de precisão arbitrária.</param>
        /// <returns>O número.</returns>
        public BigInteger MapFrom(BigInteger number)
        {
            return number;
        }

        /// <summary>
        /// Obtém a norma de um número que consite no seu módulo.
        /// </summary>
        /// <param name="value">O número.</param>
        /// <returns>A norma do número.</returns>
        public BigInteger GetNorm(BigInteger value)
        {
            return BigInteger.Abs(value);
        }

        /// <summary>
        /// Compara dois números e indica se o primeiro é menor, igual ou maior que o segundo.
        /// </summary>
        /// <param name="x">O primeiro número a ser comparado.</param>
        /// <param name="y">O segundo número a ser comparado.</param>
        /// <returns>
        /// O valor -1 caso o primeiro número seja menor do que o segundo, 0 caso ambos sejam iguais e
        /// 1 caso o primeiro número seja maior do que o segundo.
        /// </returns>
        public int Compare(BigInteger x, BigInteger y)
        {
            return Comparer<BigInteger>.Default.Compare(x, y);
        }

        /// <summary>
        /// Converte um número para um inteiro.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O resultado da conversão.</returns>
        public int ConvertToInt(BigInteger number)
        {
            return (int)number;
        }

        /// <summary>
        /// Converte o número para longo.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O reultado da conversão.</returns>
        public long ConvertToLong(BigInteger number)
        {
            return (long)number;
        }

        /// <summary>
        /// Converte o número para um inteiro de precisão arbitrária.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O resultado da conversão.</returns>
        public BigInteger ConvertToBigInteger(BigInteger number)
        {
            return number;
        }
    }
}
