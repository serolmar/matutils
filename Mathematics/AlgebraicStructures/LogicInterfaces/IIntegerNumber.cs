namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define as operações aritméticas de um inteiro sobre um objecto genérico.
    /// </summary>
    /// <typeparam name="NumberType">O tipo do objecto.</typeparam>
    public interface IIntegerNumber<NumberType> : INaturalNumber<NumberType>, INormSpace<NumberType, NumberType>
    {
        /// <summary>
        /// Obtém o antecessor de um objecto.
        /// </summary>
        /// <param name="number">O objecto do qual se pretende obter o antecessor.</param>
        /// <returns>O antecessor do objecto.</returns>
        NumberType Predecessor(NumberType number);
    }
}
