namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BlockLanczosAlgorithm<ElementType, FieldType> :
        IAlgorithm<IMatrix<ElementType>, IVector<ElementType>, IMatrix<ElementType>>
        where FieldType : IField<ElementType>
    {
        /// <summary>
        /// Mantém o corpo responsável pelas operações sobre as entradas.
        /// </summary>
        private FieldType field;

        private IMatrixFactory<ElementType> matrixFactory;

        private MatrixAdditionOperation<ElementType, FieldType> additionOperation;

        private MatrixMultiplicationOperation<ElementType, FieldType, FieldType> multiplicationOperation;

        public BlockLanczosAlgorithm(IMatrixFactory<ElementType> matrixFactory, FieldType field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            else if (matrixFactory == null)
            {
                throw new ArgumentNullException("matrixFactory");
            }
            else
            {
                this.field = field;
                this.matrixFactory = matrixFactory;
                this.additionOperation = new MatrixAdditionOperation<ElementType, FieldType>(
                    matrixFactory,
                    field);
                this.multiplicationOperation = new MatrixMultiplicationOperation<ElementType, FieldType, FieldType>(
                    matrixFactory,
                    field,
                    field);
            }
        }

        /// <summary>
        /// Obtém o corpo responsável pelas operações sobre as entradas.
        /// </summary>
        public FieldType Field
        {
            get
            {
                return this.field;
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pela criação das matrizes.
        /// </summary>
        public IMatrixFactory<ElementType> MatrixFactory
        {
            get
            {
                return this.matrixFactory;
            }
        }

        /// <summary>
        /// Obtém a solução do sistema Ax = b onde A é a matriz simétrica de entrada e b corresponde
        /// ao vector independente.
        /// </summary>
        /// <remarks>
        /// Se a matriz em questão não for simétrica, então o processo é aplicado sobre a matriz AT * A
        /// onde AT corresponde à transposta de A e sobre o vector AT * v, onde v representa o vector
        /// independente inicial.
        /// </remarks>
        /// <param name="linearSystemMatrix">A matriz de entrada.</param>
        /// <param name="independentVector">O vector independente.</param>
        /// <returns>A solução do sistema.</returns>
        public IMatrix<ElementType> Run(IMatrix<ElementType> linearSystemMatrix, IVector<ElementType> independentVector)
        {
            throw new NotImplementedException();
        }
    }
}
