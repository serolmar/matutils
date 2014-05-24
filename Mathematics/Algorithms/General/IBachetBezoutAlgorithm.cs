namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Apresenta um ponto de entrada para qualquer algoritmo que permita calcular os coeficientes
    /// de Baché-Bezout.
    /// </summary>
    /// <typeparam name="InputType">O tipo de variável sobre o qual é executado o algoritmo.</typeparam>
    public interface IBachetBezoutAlgorithm<InputType> 
        : IAlgorithm<InputType, InputType, BacheBezoutResult<InputType>>
    {
        /// <summary>
        /// O domínio responsável pelas operações sobre os objectos.
        /// </summary>
        IEuclidenDomain<InputType> Domain { get; }
    }
}
