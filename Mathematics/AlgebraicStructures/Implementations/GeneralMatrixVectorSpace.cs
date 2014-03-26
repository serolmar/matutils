namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GeneralMatrixVectorSpace<CoeffType>
        : GeneralMatrixGroup<CoeffType>, IVectorSpace<CoeffType, IMatrix<CoeffType>>
    {
        private IField<CoeffType> scalarField;

        public GeneralMatrixVectorSpace(
            int lines,
            int columns,
            IMatrixFactory<CoeffType> matrixFactory,
            IGroup<CoeffType> coeffsGroup,
            IField<CoeffType> scalarField)
            : base(lines, columns, matrixFactory, coeffsGroup)
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

        public IField<CoeffType> Field
        {
            get
            {
                return this.scalarField;
            }
        }

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
