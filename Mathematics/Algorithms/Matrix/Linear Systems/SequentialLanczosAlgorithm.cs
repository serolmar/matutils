namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Obtém a solução do sistema Ax = b onde A corresponde a uma matriz simétrica.
    /// </summary>
    public class SequentialLanczosAlgorithm<ElementType, FieldType> : 
        IAlgorithm<IMatrix<ElementType>, IMatrix<ElementType>, IMatrix<ElementType>>
        where FieldType : IField<ElementType>
    {
        /// <summary>
        /// Mantém o corpo responsável pelas operações sobre as entradas.
        /// </summary>
        private FieldType field;

        private IMatrixFactory<ElementType> matrixFactory;

        private MatrixAdditionOperation<ElementType, FieldType> additionOperation;

        private MatrixMultiplicationOperation<ElementType, FieldType, FieldType> multiplicationOperation;

        public SequentialLanczosAlgorithm(IMatrixFactory<ElementType> matrixFactory, FieldType field)
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
        /// onde AT corresponde à transposta de A.
        /// </remarks>
        /// <param name="linearSystemMatrix">A matriz de entrada.</param>
        /// <param name="independentVector">O vector independente.</param>
        /// <returns>A solução do sistema.</returns>
        public IMatrix<ElementType> Run(
            IMatrix<ElementType> linearSystemMatrix, 
            IMatrix<ElementType> independentVector)
        {
            if (linearSystemMatrix == null)
            {
                throw new ArgumentNullException("linearSystemMatrix");
            }
            else if (independentVector == null)
            {
                throw new ArgumentNullException("independentVector");
            }
            else
            {
                var linearSystemColumns = linearSystemMatrix.GetLength(1);
                var independentVectorLines = independentVector.GetLength(0);
                var innerLinearSystemMatrix = linearSystemMatrix;
                if (linearSystemColumns != independentVectorLines)
                {
                    throw new MathematicsException("The number of linear system matrix columns must be equal to the number of independent vector lines.");
                }
                else
                {
                    if (!linearSystemMatrix.IsSymmetric(this.field))
                    {
                        var transposed = new TransposeMatrix<ElementType>(innerLinearSystemMatrix);
                        innerLinearSystemMatrix = this.multiplicationOperation.Multiply(
                            transposed,
                            innerLinearSystemMatrix);
                    }

                    var w0 = independentVector;
                    var resultVector = this.matrixFactory.CreateMatrix(
                        linearSystemColumns,
                        1,
                        this.field.AdditiveUnity);
                    if (!this.IsEmptyVector(w0))
                    {
                        var squaredMatrix = this.multiplicationOperation.Multiply(
                            innerLinearSystemMatrix,
                            innerLinearSystemMatrix);

                        var transposed = new TransposeMatrix<ElementType>(w0);
                        var psi0 = this.multiplicationOperation.Multiply(
                            transposed,
                            squaredMatrix);
                        var eta0 = this.multiplicationOperation.Multiply(
                            transposed,
                            innerLinearSystemMatrix);
                        var chi0 = this.multiplicationOperation.Multiply(eta0,
                            w0)[0,0];

                        var w1 = w0;
                        w0 = this.ComputeVector(w1, psi0, chi0);
                        var additionVector = this.multiplicationOperation.Multiply(
                            innerLinearSystemMatrix,
                            w1);
                        w0 = this.additionOperation.Add(additionVector, w0);
                        while (!this.IsEmptyVector(w0))
                        {
                        }
                    }
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifica se o vector especificado é vazio.
        /// </summary>
        /// <param name="vector">O vector.</param>
        /// <returns>Verdadeiro caso se trate de um vector vazio e falso caso contrário.</returns>
        private bool IsEmptyVector(IMatrix<ElementType> vector)
        {
            foreach (var element in vector)
            {
                if (!this.field.IsAdditiveUnity(element))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determina o vector a ser adicionado durante a iteração.
        /// </summary>
        /// <param name="w">O vector sobre o qual se pretende calcular o coeficiente.</param>
        /// <param name="psi">O valor pré-calculado do numerador.</param>
        /// <param name="chi">O valor do denominador.</param>
        /// <returns>O coeficiente procurado.</returns>
        private IMatrix<ElementType> ComputeVector(
            IMatrix<ElementType> w,
            IMatrix<ElementType> psi, 
            ElementType chi)
        {
            var temp = this.multiplicationOperation.Multiply(
                psi,
                w)[0,0];
            temp = this.field.Multiply(temp, this.field.AdditiveInverse(chi));
            temp = this.field.AdditiveInverse(temp);
            var result = this.matrixFactory.CreateMatrix(w.GetLength(0), 1, this.field.AdditiveUnity);
            for (int i = 0; i < w.GetLength(0); ++i)
            {
                result[i, 0] = this.field.Multiply(temp, w[i, 0]);
            }

            return result;
        }

        private IMatrix<ElementType> GetNewSolution(
            IMatrix<ElementType> oldSolution,
            IMatrix<ElementType> vector,
            TransposeMatrix<ElementType> transposedVector,
            ElementType chi)
        {
            throw new NotImplementedException();
        }
    }
}
