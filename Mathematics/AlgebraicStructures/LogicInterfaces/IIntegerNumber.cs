namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite representar um número inteiro.
    /// </summary>
    /// <typeparam name="NumberType">O tipo do número.</typeparam>
    public interface IIntegerNumber<NumberType> : INaturalNumber<NumberType>, INormSpace<NumberType, NumberType>
    {
        /// <summary>
        /// Obtém o antecessor de um número.
        /// </summary>
        /// <param name="number">O número em análise.</param>
        /// <returns>O antecessor do número.</returns>
        NumberType Predecessor(NumberType number);
    }
}
