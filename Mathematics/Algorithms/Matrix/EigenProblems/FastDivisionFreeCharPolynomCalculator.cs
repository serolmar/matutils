namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Permite determinar o polinómio característico de uma matriz sem necessitar
    /// divisões. Implementa o algoritmo de Berkowitz.
    /// </summary>
    /// <typeparam name="ElementType">O tipo das entradas da matriz.</typeparam>
    public class FastDivisionFreeCharPolynomCalculator<ElementType>
        : IAlgorithm<ISquareMathMatrix<ElementType>, UnivariatePolynomialNormalForm<ElementType>>
    {
        /// <summary>
        /// O nome da variável.
        /// </summary>
        protected string variableName;

        /// <summary>
        /// O anel responsável pelas operações sobre os coeficientes.
        /// </summary>
        protected IRing<ElementType> ring;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="FastDivisionFreeCharPolynomCalculator{ElementType}"/>.
        /// </summary>
        /// <param name="variableName">O nome da variável.</param>
        /// <param name="ring">O anel responsável pelas operações sobre as entradas das matrizes.</param>
        /// <exception cref="ArgumentNullException">Se o anel for nulo.</exception>
        /// <exception cref="ArgumentException">Se a variável for nula ou vazia.</exception>
        public FastDivisionFreeCharPolynomCalculator(string variableName, IRing<ElementType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (string.IsNullOrWhiteSpace(variableName))
            {
                throw new ArgumentException("Variable name must be non-empty.");
            }
            else
            {
                this.ring = ring;
                this.variableName = variableName;
            }
        }

        /// <summary>
        /// Determina o polinómio característico de uma matriz.
        /// </summary>
        /// <param name="data">A matriz.</param>
        /// <returns>O polinómio característico.</returns>
        public UnivariatePolynomialNormalForm<ElementType> Run(ISquareMathMatrix<ElementType> data)
        {
            if (data == null)
            {
                return new UnivariatePolynomialNormalForm<ElementType>(this.variableName);
            }
            else
            {
                var lines = data.GetLength(0);
                if (lines == 0)
                {
                    return new UnivariatePolynomialNormalForm<ElementType>(this.variableName);
                }
                else if (lines == 1)
                {
                    var entry = data[0, 0];
                    var result = new UnivariatePolynomialNormalForm<ElementType>(
                        this.ring.MultiplicativeUnity,
                        1,
                        this.variableName,
                        this.ring);
                    result = result.Add(this.ring.AdditiveInverse(entry), 0, this.ring);
                    return result;
                }
                else if (lines == 2)
                {
                    var variablePol = new UnivariatePolynomialNormalForm<ElementType>(
                        this.ring.MultiplicativeUnity,
                        1,
                        this.variableName,
                        this.ring);
                    var firstDiagonalElement = variablePol.Add(
                        this.ring.AdditiveInverse(data[0, 0]),
                        this.ring);
                    var secondDiagonalElement = variablePol.Add(
                        this.ring.AdditiveInverse(data[1, 1]),
                        this.ring);
                    var result = firstDiagonalElement.Multiply(secondDiagonalElement, this.ring);
                    result = result.Add(
                        this.ring.AdditiveInverse(this.ring.Multiply(data[0, 1], data[1, 0])),
                        this.ring);
                    return result;
                }
                else
                {
                    var matrixFactory = new ArrayMatrixFactory<ElementType>();
                    var matrixMultiplicator = new MatrixMultiplicationOperation<ElementType>(
                        matrixFactory, this.ring, this.ring);
                    var subMatrixSequence = new IntegerSequence();
                    var singleValueSequence = new IntegerSequence();

                    IMatrix<ElementType> multiplicationMatrix = new ArrayMathMatrix<ElementType>(
                        lines + 1,
                        lines,
                        this.ring.AdditiveUnity);
                    subMatrixSequence.Add(1, lines - 1);
                    singleValueSequence.Add(0);
                    this.FillMultiplicationMatrix(
                        data,
                        data[0, 0],
                        subMatrixSequence,
                        singleValueSequence,
                        matrixMultiplicator,
                        multiplicationMatrix);

                    var currentDimension = 1;
                    while (currentDimension < lines - 1)
                    {
                        subMatrixSequence.Clear();
                        singleValueSequence.Clear();
                        subMatrixSequence.Add(currentDimension + 1, lines - 1);
                        singleValueSequence.Add(currentDimension);
                        var otherLines = lines - currentDimension;
                        var otherMultiplicationMatrix = new ArrayMathMatrix<ElementType>(
                            otherLines + 1, 
                            otherLines, 
                            this.ring.AdditiveUnity);

                        this.FillMultiplicationMatrix(
                            data,
                            data[currentDimension, currentDimension],
                            subMatrixSequence,
                            singleValueSequence,
                            matrixMultiplicator,
                            otherMultiplicationMatrix);

                        multiplicationMatrix = matrixMultiplicator.Multiply(
                            multiplicationMatrix, 
                            otherMultiplicationMatrix);
                        ++currentDimension;
                    }

                    var lastOtherMultiplicationMatrix = new ArrayMathMatrix<ElementType>(
                            2, 
                            1, 
                            this.ring.AdditiveUnity);
                    lastOtherMultiplicationMatrix[0, 0] = this.ring.MultiplicativeUnity;
                    lastOtherMultiplicationMatrix[1, 0] = this.ring.AdditiveInverse(data[currentDimension, currentDimension]);
                    multiplicationMatrix = matrixMultiplicator.Multiply(
                            multiplicationMatrix,
                            lastOtherMultiplicationMatrix);

                    var result = new UnivariatePolynomialNormalForm<ElementType>(
                        multiplicationMatrix[0, 0],
                        lines,
                        this.variableName,
                        this.ring);
                    for (int i = 1; i <= lines; ++i)
                    {
                        result = result.Add(multiplicationMatrix[i, 0], lines - i, this.ring);
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Calcula as entradas da matriz de multiplicação.
        /// </summary>
        /// <param name="data">A matriz.</param>
        /// <param name="diagonalElement">O elemento na diagonal.</param>
        /// <param name="subMatrixSequence">A sequência que define a sub-matriz.</param>
        /// <param name="singleValueSequence">A sequência que define o valor.</param>
        /// <param name="multiplicator">O objecto responsável pela multiplicação de matrizes.</param>
        /// <param name="multiplicationMatrix">A matriz que comporta o resultado da multiplicação.</param>
        private void FillMultiplicationMatrix(
            IMathMatrix<ElementType> data,
            ElementType diagonalElement,
            IntegerSequence subMatrixSequence,
            IntegerSequence singleValueSequence,
            MatrixMultiplicationOperation<ElementType> multiplicator,
            IMatrix<ElementType> multiplicationMatrix)
        {
            var dimension = multiplicationMatrix.GetLength(1);
            var rowSubMatrix = data.GetSubMatrix(singleValueSequence, subMatrixSequence);
            var columnSubMatrix = data.GetSubMatrix(subMatrixSequence, singleValueSequence);
            var mainSubMatrix = data.GetSubMatrix(subMatrixSequence, subMatrixSequence);
            for (int i = 0; i < dimension; ++i)
            {
                multiplicationMatrix[i, i] = this.ring.MultiplicativeUnity;
            }

            for (int i = 0; i < dimension; ++i)
            {
                multiplicationMatrix[i + 1, i] = this.ring.AdditiveInverse(diagonalElement);
            }

            var vectorsMultiply = multiplicator.Multiply(rowSubMatrix, columnSubMatrix);
            for (int i = 0; i < dimension - 1; ++i)
            {
                multiplicationMatrix[i + 2, i] = this.ring.AdditiveInverse(vectorsMultiply[0, 0]);
            }

            vectorsMultiply = multiplicator.Multiply(rowSubMatrix, mainSubMatrix);
            vectorsMultiply = multiplicator.Multiply(vectorsMultiply, columnSubMatrix);
            for (int i = 0; i < dimension - 2; ++i)
            {
                multiplicationMatrix[i + 3, i] = this.ring.AdditiveInverse(vectorsMultiply[0, 0]);
            }

            var subMatrix = mainSubMatrix;
            for (int i = 3; i < dimension; ++i)
            {
                subMatrix = multiplicator.Multiply(mainSubMatrix, subMatrix);
                vectorsMultiply = multiplicator.Multiply(rowSubMatrix, subMatrix);
                vectorsMultiply = multiplicator.Multiply(vectorsMultiply, columnSubMatrix);
                for (int j = 0; j < dimension - i; ++j)
                {
                    multiplicationMatrix[j + i + 1, j] = this.ring.AdditiveInverse(vectorsMultiply[0, 0]);
                }
            }
        }
    }
}
