namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Define um grupo sobre matrizes.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem os coeficientes.</typeparam>
    public interface IMatrixGroup<CoeffType> : IGroup<IMatrix<CoeffType>>
    {
        /// <summary>
        /// Obtém o número de linhas das matrizes que fazem parte do grupo.
        /// </summary>
        int Lines { get; }

        /// <summary>
        /// Obtém o número de colunas das matrizes que fazem parte do grupo.
        /// </summary>
        int Columns { get; }

        /// <summary>
        /// Obtém a fábrica responsável pela instanciação de matrizes resultantes das operações.
        /// </summary>
        IMatrixFactory<CoeffType> Factory { get; }
    }
}
