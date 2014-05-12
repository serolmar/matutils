namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa funções que definem uma álgebra sobre matrizes.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem as entradas da matriz.</typeparam>
    public class GeneralMatrixAlgebra<CoeffType>
        : GeneralMatrixRing<CoeffType>, IAlgebra<CoeffType, IMatrix<CoeffType>>
    {
        /// <summary>
        /// Objecto responsável pelas operações de espaço vectorial sobre as matrizes.
        /// </summary>
        private GeneralMatrixVectorSpace<CoeffType> generalMatrixVectorSpace;

        /// <summary>
        /// Cria uma instância de objectos do tipo <see cref="GeneralMatrixAlgebra{CoeffType}"/>.
        /// </summary>
        /// <param name="dimension">A dimensão das matrizes.</param>
        /// <param name="matrixFactory">A fábrica responsável pela criaçõa de matrizes.</param>
        /// <param name="coeffsField">O corpo responsável pelas operações sobre os escalares.</param>
        public GeneralMatrixAlgebra(
            int dimension,
            IMatrixFactory<CoeffType> matrixFactory,
            IField<CoeffType> coeffsField)
            : base(dimension, matrixFactory, coeffsField)
        {
            this.generalMatrixVectorSpace = new GeneralMatrixVectorSpace<CoeffType>(
                dimension,
                dimension,
                matrixFactory,
                coeffsField,
                coeffsField);
        }

        /// <summary>
        /// Obtém o corpo responsável pelas operações sobre os coeficientes.
        /// </summary>
        /// <value>
        /// O corpo responsável pelas operações sobre os coeficientes.
        /// </value>
        public IField<CoeffType> Field
        {
            get
            {
                return this.Field;
            }
        }

        /// <summary>
        /// Permite determinar o produto de um escalar por uma matriz.
        /// </summary>
        /// <param name="coefficientElement">O escalar.</param>
        /// <param name="vectorSpaceElement">A matriz.</param>
        /// <returns>O resultado do produto do esacalar pela matriz.</returns>
        public IMatrix<CoeffType> MultiplyScalar(
            CoeffType coefficientElement,
            IMatrix<CoeffType> vectorSpaceElement)
        {
            return this.generalMatrixVectorSpace.MultiplyScalar(coefficientElement, vectorSpaceElement);
        }
    }
}
