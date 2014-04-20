namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GeneralMatrixAlgebra<CoeffType>
        : GeneralMatrixRing<CoeffType>, IAlgebra<CoeffType, IMatrix<CoeffType>>
    {
        private GeneralMatrixVectorSpace<CoeffType> generalMatrixVectorSpace;

        public GeneralMatrixAlgebra(
            int dimension,
            IMatrixFactory<CoeffType> matrixFactory,
            IRing<CoeffType> coeffsRing,
            IField<CoeffType> scalarField)
            : base(dimension, matrixFactory, coeffsRing)
        {
            this.generalMatrixVectorSpace = new GeneralMatrixVectorSpace<CoeffType>(
                dimension,
                dimension,
                matrixFactory,
                coeffsRing,
                scalarField);
        }

        public IField<CoeffType> Field
        {
            get
            {
                return this.Field;
            }
        }

        public IMatrix<CoeffType> MultiplyScalar(
            CoeffType coefficientElement,
            IMatrix<CoeffType> vectorSpaceElement)
        {
            return this.generalMatrixVectorSpace.MultiplyScalar(coefficientElement, vectorSpaceElement);
        }
    }
}
