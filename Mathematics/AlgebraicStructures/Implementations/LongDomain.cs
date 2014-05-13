namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Permite representar as operações sobre o subconjunto dos números inteiros representáveis por uma
    /// variável do tipo <see cref="System.long"/>.
    /// </summary>
    public class LongDomain : IIntegerNumber<long>
    {
        /// <summary>
        /// Obtém a unidade multiplicativa.
        /// </summary>
        /// <value>
        /// A unidade multiplicativa.
        /// </value>
        public long MultiplicativeUnity
        {
            get { return 1; }
        }

        /// <summary>
        /// Obtém a unidade aditiva.
        /// </summary>
        /// <value>
        /// A unidade aditiva.
        /// </value>
        public long AdditiveUnity
        {
            get { return 0; }
        }

        /// <summary>
        /// Obtém o núemro de unidades.
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
        /// Obtém um enumerado para as unidades.
        /// </summary>
        /// <value>
        /// O enumerador para as unidades.
        /// </value>
        public IEnumerable<long> Units
        {
            get
            {
                yield return 1;
                yield return -1;
            }
        }

        /// <summary>
        /// Calcula o produto de dois números.
        /// </summary>
        /// <param name="left">O primeiro número a ser multiplicado.</param>
        /// <param name="right">O segundo número a ser multiplicado.</param>
        /// <returns>O resultado do produto.</returns>
        public long Multiply(long left, long right)
        {
            checked
            {
                return left * right;
            }
        }

        /// <summary>
        /// Obtém o inverso aditivo de um número.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O inverso aditivo do número.</returns>
        public long AdditiveInverse(long number)
        {
            return -number;
        }

        /// <summary>
        /// Calcula a soma de dois números.
        /// </summary>
        /// <param name="left">O primeiro número a ser adicionado.</param>
        /// <param name="right">O segundo número a ser adicionado.</param>
        /// <returns>O resultado da adição.</returns>
        public long Add(long left, long right)
        {
            checked
            {
                return left + right;
            }
        }

        /// <summary>
        /// Calcula a adição repetida de um número.
        /// </summary>
        /// <remarks>
        /// A adição repetida de um longo é rapidamente determinada com base na operação de multiplicação.
        /// </remarks>
        /// <param name="left">O número a ser adicionado.</param>
        /// <param name="right">O número de vezes que é realizada a adição.</param>
        /// <returns>O resultado da adição repetida.</returns>
        public long AddRepeated(long left, int right)
        {
            checked
            {
                return left * right;
            }
        }

        /// <summary>
        /// Determina se ambos os números são iguais.
        /// </summary>
        /// <param name="x">O primeiro número a ser comparado.</param>
        /// <param name="y">O segundo número a ser comparado.</param>
        /// <returns>
        /// Verdadeiro se ambos os números forem iguais e falso caso contrário.
        /// </returns>
        public bool Equals(long x, long y)
        {
            return x == y;
        }

        /// <summary>
        /// Obtém o código confuso de um número.
        /// </summary>
        /// <param name="obj">O número.</param>
        /// <returns>
        /// O código confuso do número adequado à utilização em alguns algoritmos habituais.
        /// </returns>
        public int GetHashCode(long obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Determina se um número é uma unidade aditiva.
        /// </summary>
        /// <param name="value">O número.</param>
        /// <returns>Verdadeiro caso o número seja uma unidade aditiva e falso caso contrário.</returns>
        public bool IsAdditiveUnity(long value)
        {
            return value == 0;
        }

        /// <summary>
        /// Determina se o número é uma unidade multiplicativa.
        /// </summary>
        /// <param name="value">O número.</param>
        /// <returns>Verdadeiro caso o número seja uma unidade aditiva e falso caso contrário.</returns>
        public bool IsMultiplicativeUnity(long value)
        {
            return value == 1;
        }

        /// <summary>
        /// Determina o quociente entre dois números.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>O reusltado do quociente.</returns>
        public long Quo(long dividend, long divisor)
        {
            return dividend / divisor;
        }

        /// <summary>
        /// Determina o resto da divisão de dois números.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>O resultado do resto.</returns>
        public long Rem(long dividend, long divisor)
        {
            return dividend % divisor;
        }

        /// <summary>
        /// Determina o grau de um número.
        /// </summary>
        /// <remarks>
        /// Em operações de divisão habituais, o grau de um número corresponde ao seu valor absoluto.
        /// </remarks>
        /// <param name="value">O número.</param>
        /// <returns>O grau.</returns>
        public uint Degree(long value)
        {
            return (uint)Math.Abs(value);
        }

        /// <summary>
        /// Calcula o quociente e o resto da divisão de dois números.
        /// </summary>
        /// <remarks>
        /// Apesar do cálculo do quociente e do resto serem independentes no caso de números longos,
        /// em outras estruturas, o cálculo de um implica implicitamente o cálculo de outro. Deste modo,
        /// é conveniente apresentar uam função que apresente os dois resultados.
        /// </remarks>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O diviosr.</param>
        /// <returns>Um objecto que contém ambos os valores.</returns>
        public DomainResult<long> GetQuotientAndRemainder(long dividend, long divisor)
        {
            return new DomainResult<long>(dividend / divisor, dividend % divisor);
        }

        /// <summary>
        /// Retorna o predecessor de um número.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O predecessor.</returns>
        /// <exception cref="System.ArgumentException">Se o número proporcionado for o menor possível.</exception>
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

        /// <summary>
        /// Retorna o sucessor de um número.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O sucessor do número.</returns>
        /// <exception cref="System.ArgumentException">Se o número proporcionado for o maior possível.</exception>
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

        /// <summary>
        /// Mapeia um número longo a partir de um número inteiro.
        /// </summary>
        /// <param name="number">O número inteiro a ser mapeado.</param>
        /// <returns>O número longo mapeado.</returns>
        public long MapFrom(int number)
        {
            return number;
        }

        /// <summary>
        /// Mapeia um número longo a partir de um número longo.
        /// </summary>
        /// <param name="number">O número longo a ser mapeado.</param>
        /// <returns>O número longo mapeado.</returns>
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

        /// <summary>
        /// Mapeia um número longo a partir de um número inteiro de precisão arbitrária.
        /// </summary>
        /// <param name="number">O número inteiro de precisão arbitrária a ser mapeado.</param>
        /// <returns>O número longo mapeado.</returns>
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

        /// <summary>
        /// Obtém a norma de um número.
        /// </summary>
        /// <param name="value">O número.</param>
        /// <returns>A norma.</returns>
        public long GetNorm(long value)
        {
            return (long)Math.Abs(value);
        }

        /// <summary>
        /// Compara dois números retornando um valor que indica o primeiro é menor, igual ou maior do que o segundo.
        /// </summary>
        /// <param name="x">O primeiro objecto a ser comparado.</param>
        /// <param name="y">O segundo objecto a ser comparado.</param>
        /// <returns>
        /// O valor -1 se o primeiro número for menor do que o segundo, 0 se ambos forem iguais e 1 se o primeiro
        /// número for maior do que o segundo.
        /// </returns>
        public int Compare(long x, long y)
        {
            return Comparer<long>.Default.Compare(x, y);
        }

        /// <summary>
        /// Converte um número longo num número inteiro.
        /// </summary>
        /// <param name="number">O número longo.</param>
        /// <returns>O número inteiro.</returns>
        public int ConvertToInt(long number)
        {
            return (int)number;
        }

        /// <summary>
        /// Converte um número longo num número longo.
        /// </summary>
        /// <remarks>
        /// Esta função constitui uma identidade sobre o argumento.
        /// </remarks>
        /// <param name="number">O número.</param>
        /// <returns>O mesmo número.</returns>
        public long ConvertToLong(long number)
        {
            return number;
        }

        /// <summary>
        /// Converte um número longo num número inteiro de precisão arbitrária.
        /// </summary>
        /// <param name="number">O número longo.</param>
        /// <returns>O número inteiro de precisão arbitrária.</returns>
        public BigInteger ConvertToBigInteger(long number)
        {
            return number;
        }
    }
}
