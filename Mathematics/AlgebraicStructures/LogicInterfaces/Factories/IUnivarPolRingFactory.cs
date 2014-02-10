namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um conjunto de objectos que permitem criar instâncias de aneis polinomiais.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente.</typeparam>
    public interface IUnivarPolRingFactory<CoeffType>
    {
        /// <summary>
        /// Obtém a instância de um objecto que permite efectuar operações sobre um polinómio.
        /// </summary>
        /// <param name="variableName">O nome da variável associado ao anel.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O anel polinomial.</returns>
        UnivarPolynomRing<CoeffType> CreateInstance(string variableName, IRing<CoeffType> ring);
    }
}
