namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa o algoritmo de levantamento.
    /// </summary>
    /// <typeparam name="T">O tipo de coeficiente.</typeparam>
    public interface ILinearLiftAlgorithm<T>
        : IAlgorithm<
        LinearLiftingStatus<T>,
        int,
        bool>
    {
        /// <summary>
        /// Obtém a classe responsável pelas operações sobre os números inteiros.
        /// </summary>
        IIntegerNumber<T> IntegerNumber { get; }

        /// <summary>
        /// Obtém a classe responsável pela obtenção de instâncias de um domínio polinomial.
        /// </summary>
        IUnivarPolDomainFactory<T> PolynomialDomainFactory { get; }

        /// <summary>
        /// Obtém a classe responsável pela obtenção de instâncias de um corpo modular.
        /// </summary>
        IModularFieldFactory<T> ModularFieldFactory { get; }
    }
}
