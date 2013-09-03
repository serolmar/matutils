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
        /// onde AT corresponde à transposta de A e sobre o vector AT * v, onde v representa o vector
        /// independente inicial.
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
                var innerIndependentVector = independentVector;
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
                        innerIndependentVector = this.multiplicationOperation.Multiply(
                            transposed,
                            independentVector);
                    }

                    var w0 = innerIndependentVector;
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
                        var psi = this.multiplicationOperation.Multiply(
                            transposed,
                            squaredMatrix);
                        var eta = this.multiplicationOperation.Multiply(
                            transposed,
                            innerLinearSystemMatrix);
                        var chi = this.multiplicationOperation.Multiply(eta,
                            w0)[0,0];

                        resultVector = this.GetNewSolution(
                            resultVector,
                            innerIndependentVector,
                            w0,
                            transposed,
                            chi);

                        var w1 = w0;
                        w0 = this.ComputeVector(w1, w1, psi, chi);
                        var additionVector = this.multiplicationOperation.Multiply(
                            innerLinearSystemMatrix,
                            w1);
                        w0 = this.additionOperation.Add(additionVector, w0);
                        while (!this.IsEmptyVector(w0))
                        {
                            additionVector = this.ComputeVector(w1, w0, psi, chi);
                            transposed = new TransposeMatrix<ElementType>(w0);
                            psi = this.multiplicationOperation.Multiply(
                            transposed,
                            squaredMatrix);
                            eta = this.multiplicationOperation.Multiply(
                            transposed,
                            innerLinearSystemMatrix);
                            chi = this.multiplicationOperation.Multiply(eta,
                            w0)[0, 0];

                            resultVector = this.GetNewSolution(
                            resultVector,
                            innerIndependentVector,
                            w0,
                            transposed,
                            chi);

                            additionVector = this.additionOperation.Add(
                                additionVector,
                                this.ComputeVector(w0, w0, psi, chi));
                            w1 = w0;
                            w0 = this.multiplicationOperation.Multiply(
                            innerLinearSystemMatrix,
                            w0);
                            w0 = this.additionOperation.Add(additionVector, w0);
                        }
                    }

                    return resultVector;
                }
            }
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
        /// <param name="previousVector">O vector sobre o qual se pretende calcular o coeficiente.</param>
        /// <param name="vector">O vector correspondente à iteração.</param>
        /// <param name="psi">O valor pré-calculado do numerador.</param>
        /// <param name="chi">O valor do denominador.</param>
        /// <returns>O coeficiente procurado.</returns>
        private IMatrix<ElementType> ComputeVector(
            IMatrix<ElementType> previousVector,
            IMatrix<ElementType> vector,
            IMatrix<ElementType> psi, 
            ElementType chi)
        {
            var temp = this.multiplicationOperation.Multiply(
                psi,
                vector)[0,0];
            temp = this.field.Multiply(temp, this.field.MultiplicativeInverse(chi));
            temp = this.field.AdditiveInverse(temp);
            var result = this.matrixFactory.CreateMatrix(previousVector.GetLength(0), 1, this.field.AdditiveUnity);
            for (int i = 0; i < previousVector.GetLength(0); ++i)
            {
                result[i, 0] = this.field.Multiply(temp, previousVector[i, 0]);
            }

            return result;
        }

        /// <summary>
        /// Obtém a nova solução sendo proporcionados os elementos que permitem o respectivo
        /// cálculo.
        /// </summary>
        /// <param name="oldSolution">A solução anterior.</param>
        /// <param name="systemVector">O vector independente do sistema de equações.</param>
        /// <param name="currentVector">O vector actual da iteração.</param>
        /// <param name="transposedVector">O vector actual transposto.</param>
        /// <param name="chi">O valor do denominador.</param>
        /// <returns>A nova aproximação para a solução.</returns>
        private IMatrix<ElementType> GetNewSolution(
            IMatrix<ElementType> oldSolution,
            IMatrix<ElementType> systemVector,
            IMatrix<ElementType> currentVector,
            TransposeMatrix<ElementType> transposedVector,
            ElementType chi)
        {
            var coeff = this.multiplicationOperation.Multiply(
                transposedVector,
                systemVector)[0, 0];
            coeff = this.field.Multiply(coeff, this.field.MultiplicativeInverse(chi));
            var result = this.matrixFactory.CreateMatrix(
                currentVector.GetLength(0),
                1,
                this.field.AdditiveUnity);
            for (int i = 0; i < currentVector.GetLength(0); ++i)
            {
                result[i, 0] = this.field.Multiply(coeff, currentVector[i, 0]);
            }

            result = this.additionOperation.Add(result, oldSolution);
            return result;
        }
    }
}
