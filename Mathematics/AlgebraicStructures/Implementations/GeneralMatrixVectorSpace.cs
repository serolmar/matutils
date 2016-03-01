namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Define operações de espaço vectorial sobre um conjunto de coeficientes e de matrizes.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente.</typeparam>
    public class GeneralMatrixVectorSpace<CoeffType>
        : GeneralMatrixGroup<CoeffType>, IVectorSpace<CoeffType, IMatrix<CoeffType>>
    {
        /// <summary>
        /// O corpo responsável pelas operações sobre os coeficientes.
        /// </summary>
        private IField<CoeffType> scalarField;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="GeneralMatrixVectorSpace{CoeffType}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número colunas.</param>
        /// <param name="matrixFactory">A fábrica responsável pela criação de matrizes.</param>
        /// <param name="scalarField">The scalar field.</param>
        /// <exception cref="ArgumentNullException">scalarField</exception>
        public GeneralMatrixVectorSpace(
            int lines,
            int columns,
            IMatrixFactory<CoeffType> matrixFactory,
            IField<CoeffType> scalarField)
            : base(lines, columns, matrixFactory, scalarField)
        {
            if (scalarField == null)
            {
                throw new ArgumentNullException("scalarField");
            }
            else
            {
                this.scalarField = scalarField;
            }
        }

        /// <summary>
        /// Obtém o corpo sobre o qual funciona o espaço vectorial.
        /// </summary>
        /// <value>
        /// O corpo sobre o qual funciona o espaço vectorial.
        /// </value>
        public IField<CoeffType> Field
        {
            get
            {
                return this.scalarField;
            }
        }

        /// <summary>
        /// Define a multiplicação do anel ou campo com o elemento do espaço vectorial.
        /// </summary>
        /// <param name="coefficientElement">O coeficiente.</param>
        /// <param name="vectorSpaceElement">O vector.</param>
        /// <returns>O vector resultante.</returns>
        /// <exception cref="ArgumentNullException">Caso algum dos argumentos seja nulo.</exception>
        /// <exception cref="MathematicsException">
        /// Caso as dimensões da matriz não se coadunem com as especificadas
        /// no espaço vectorial corrente.
        /// </exception>
        public IMatrix<CoeffType> MultiplyScalar(
            CoeffType coefficientElement,
            IMatrix<CoeffType> vectorSpaceElement)
        {
            if (coefficientElement == null)
            {
                throw new ArgumentNullException("coefficientElement");
            }
            else if (vectorSpaceElement == null)
            {
                throw new ArgumentNullException("vectorSpaceElement");
            }
            else if (vectorSpaceElement.GetLength(0) != this.lines)
            {
                throw new MathematicsException(string.Format("Can only operate over matrix with {0} lines.", this.lines));
            }
            else if (vectorSpaceElement.GetLength(1) != this.columns)
            {
                throw new MathematicsException(string.Format("Can only operate over matrix with {0} columns.", this.columns));
            }
            else
            {
                var resultMatrix = this.matrixFactory.CreateMatrix(
                    this.lines,
                    this.columns);
                for (int i = 0; i < this.lines; ++i)
                {
                    for (int j = 0; j < this.columns; ++j)
                    {
                        resultMatrix[i, j] = this.scalarField.Multiply(coefficientElement, vectorSpaceElement[i, j]);
                    }
                }

                return resultMatrix;
            }
        }
    }
}
