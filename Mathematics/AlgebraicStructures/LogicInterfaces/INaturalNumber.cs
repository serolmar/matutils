namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Permite definir formalmente um número natural a partir de um objecto que possa possuir essa propriedade.
    /// </summary>
    /// <typeparam name="NumberType">O tipo de objecto que representa o número.</typeparam>
    public interface INaturalNumber<NumberType> : IEuclidenDomain<NumberType>
    {
        /// <summary>
        /// Permite determinar o sucessor de um número.
        /// </summary>
        /// <param name="number">O número do qual pretendemos obter o sucessor.</param>
        /// <returns>O sucessor do número providenciado.</returns>
        NumberType Successor(NumberType number);

        /// <summary>
        /// Permite mapear um número natural a partir de um inteiro.
        /// </summary>
        /// <param name="number">O número inteiro a ser mapeado.</param>
        /// <returns>O número inteiro mapeado.</returns>
        NumberType MapFrom(int number);

        /// <summary>
        /// Permite mapear um número natural a partir de um longo.
        /// </summary>
        /// <param name="number">O número longo a ser mapeado.</param>
        /// <returns>O número inteiro mapeado.</returns>
        NumberType MapFrom(long number);

        /// <summary>
        /// Permite mapear um número natural a partir de um inteiro grande.
        /// </summary>
        /// <param name="number">O número inteiro grande a ser mapeado.</param>
        /// <returns>O número inteiro mapeado.</returns>
        NumberType MapFrom(BigInteger number);

        /// <summary>
        /// Tenta converter o valor inteiro para o tipo <see cref="int"/>.
        /// </summary>
        /// <param name="number">O número a ser convertido.</param>
        /// <returns>A representação do número como <see cref="int"/>.</returns>
        int ConvertToInt(NumberType number);

        /// <summary>
        /// Tenta converter o valor inteiro para o tipo <see cref="long"/>.
        /// </summary>
        /// <param name="number">O número a ser convertido.</param>
        /// <returns>A representação do número como <see cref="long"/>.</returns>
        long ConvertToLong(NumberType number);

        /// <summary>
        /// Tenta converter o valor inteiro para o tipo <see cref="BigInteger"/>.
        /// </summary>
        /// <param name="number">O número a ser convertido.</param>
        /// <returns>A representação do número como <see cref="BigInteger"/>.</returns>
        BigInteger ConvertToBigInteger(NumberType number);
    }
}
