namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo de Lanczos por blocos para sistemas de equações
    /// simétricos.
    /// </summary>
    /// <typeparam name="ElementType">O tipo de objectos que constituem os coeficientes das equações.</typeparam>
    /// <typeparam name="FieldType">O tipo de objecto que representa o corpo responsável pelas operações.</typeparam>
    public class BlockLanczosAlgorithm<ElementType, FieldType> :
        IAlgorithm<IMatrix<ElementType>, IVector<ElementType>, IMatrix<ElementType>>
        where FieldType : IField<ElementType>
    {
        /// <summary>
        /// Mantém o corpo responsável pelas operações sobre as entradas.
        /// </summary>
        private FieldType field;

        /// <summary>
        /// A fábrica de matrizes.
        /// </summary>
        private IMatrixFactory<ElementType> matrixFactory;

        /// <summary>
        /// O objecto responsável pela adição de matrizes.
        /// </summary>
        private MatrixAdditionOperation<ElementType> additionOperation;

        /// <summary>
        /// O objecto responsável pela multiplicação de matrizes.
        /// </summary>
        private MatrixMultiplicationOperation<ElementType> multiplicationOperation;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BlockLanczosAlgorithm{ElementType, FieldType}"/>.
        /// </summary>
        /// <param name="matrixFactory">A fábrica responsável pela criação de matrizes.</param>
        /// <param name="field">O corpo responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Se a fábrica de matrizes ou o corpo forem nulos.
        /// </exception>
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
                this.additionOperation = new MatrixAdditionOperation<ElementType>(
                    matrixFactory,
                    field);
                this.multiplicationOperation = new MatrixMultiplicationOperation<ElementType>(
                    matrixFactory,
                    field,
                    field);
            }
        }

        /// <summary>
        /// Obtém o corpo responsável pelas operações sobre as entradas.
        /// </summary>
        /// <value>O corpo responsável pelas operações sobre os coeficientes.</value>
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
        /// <value>O objecto responsável pela criação de matrizes.</value>
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
