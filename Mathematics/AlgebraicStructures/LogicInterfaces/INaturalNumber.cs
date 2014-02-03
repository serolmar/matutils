namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Numerics;

    /// <summary>
    /// Permite definir formalmente um número natural a partir de um objecto que possa possuir essa propriedade.
    /// </summary>
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
    }
}
