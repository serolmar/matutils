using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Apresenta um ponto de entrada para qualquer algoritmo que permita calcular os coeficientes
    /// de Baché-Bezout.
    /// </summary>
    /// <typeparam name="InputType">O tipo de variável sobre o qual é executado o algoritmo.</typeparam>
    /// <typeparam name="DomainType">O domínio que define as operações sobre o tipo de entrada.</typeparam>
    public interface IBachetBezoutAlgorithm<InputType, out DomainType> 
        : IAlgorithm<InputType, InputType, BacheBezoutResult<InputType>>
        where DomainType : IEuclidenDomain<InputType>
    {
        DomainType Domain { get; }
    }
}
