namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define uma função capaz de obter um objecto do tipo alvo mais próximo do tipo fonte.
    /// </summary>
    /// <remarks>
    /// Um possível exemplo consite na determinação do inteiro que se encontra mais próximo de uma fracção ou
    /// de um decimal.
    /// </remarks>
    /// <typeparam name="SourceType">O tipo da fonte.</typeparam>
    /// <typeparam name="TargetType">O tipo do alvo.</typeparam>
    public interface INearest<in SourceType, out TargetType>
    {
        /// <summary>
        /// Otbém o objecto mais próximo da fote especificada.
        /// </summary>
        /// <param name="source">A fonte.</param>
        /// <returns>O objecto mais próximo.</returns>
        TargetType GetNearest(SourceType source);
    }
}
