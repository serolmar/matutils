namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define as operações essenciais num algoritmo que permita calcular determinantes de matrizes.
    /// </summary>
    /// <typeparam name="ElementsType">O tipo de objectos que constituem as entradas das matrizes.</typeparam>
    public abstract class ADeterminant<ElementsType> : IAlgorithm<IMatrix<ElementsType>, ElementsType>
    {
        /// <summary>
        /// O anel responsável pelas operações sobre as entradas das matrizes.
        /// </summary>
        protected IRing<ElementsType> ring;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ADeterminant{ElementsType}"/>.
        /// </summary>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="System.ArgumentNullException">Se o anel for nulo.</exception>
        public ADeterminant(IRing<ElementsType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                this.ring = ring;
            }
        }

        /// <summary>
        /// Executa o algoritmo sobre a matriz.
        /// </summary>
        /// <param name="data">A matriz.</param>
        /// <returns>O determinante da matriz.</returns>
        /// <exception cref="MathematicsException">Se a matriz não for quadrada.</exception>
        public ElementsType Run(IMatrix<ElementsType> data)
        {
            if (data == null)
            {
                return this.ring.AdditiveUnity;
            }
            else
            {
                var lines = data.GetLength(0);
                var columns = data.GetLength(1);
                if (lines != columns)
                {
                    throw new MathematicsException("Determinants can only be applied to square matrices.");
                }
                else
                {
                    return this.ComputeDeterminant(data);
                }
            }
        }

        /// <summary>
        /// Calcula o determinante da matriz.
        /// </summary>
        /// <param name="data">A matriz.</param>
        /// <returns>O determinante.</returns>
        protected abstract ElementsType ComputeDeterminant(IMatrix<ElementsType> data);
    }
}
