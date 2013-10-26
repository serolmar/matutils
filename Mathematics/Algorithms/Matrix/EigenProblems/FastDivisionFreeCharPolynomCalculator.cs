namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Permite determinar o polinómio característico de uma matriz sem necessitar
    /// divisões. Implementa o algoritmo de Berkowitz.
    /// </summary>
    /// <typeparam name="ElementType">O tipo das entradas da matriz.</typeparam>
    /// <typeparam name="RingType">O anel responsável pelas operações.</typeparam>
    public class FastDivisionFreeCharPolynomCalculator<ElementType>
        : IAlgorithm<IMatrix<ElementType>, UnivariatePolynomialNormalForm<ElementType>>
    {
        protected string variableName;

        protected IRing<ElementType> ring;

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

        public UnivariatePolynomialNormalForm<ElementType> Run(IMatrix<ElementType> data)
        {
            if (data == null)
            {
                return new UnivariatePolynomialNormalForm<ElementType>(this.variableName);
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
                        var matrixMultiplicator = new MatrixMultiplicationOperation
                            <ElementType, IRing<ElementType>, IRing<ElementType>>(
                            matrixFactory, this.ring, this.ring);
                        var subMatrixSequence = new IntegerSequence();
                        var singleValueSequence = new IntegerSequence();

                        IMatrix<ElementType> multiplicationMatrix = new ArrayMatrix<ElementType>(
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
                        while (currentDimension < lines)
                        {
                            subMatrixSequence.Clear();
                            singleValueSequence.Clear();
                            subMatrixSequence.Add(currentDimension, lines - 1);
                            singleValueSequence.Add(currentDimension);
                            var otherLines = lines - currentDimension;
                            var otherMultiplicationMatrix = new ArrayMatrix<ElementType>(otherLines + 1, otherLines, this.ring.AdditiveUnity);
                            this.FillMultiplicationMatrix(data,
                                data[currentDimension, currentDimension],
                                subMatrixSequence,
                                singleValueSequence,
                                matrixMultiplicator,
                                otherMultiplicationMatrix);

                            multiplicationMatrix = matrixMultiplicator.Multiply(multiplicationMatrix, otherMultiplicationMatrix);
                            ++currentDimension;
                        }

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
        }

        private void FillMultiplicationMatrix(IMatrix<ElementType> data,
            ElementType diagonalElement,
            IntegerSequence subMatrixSequence,
            IntegerSequence singleValueSequence,
            MatrixMultiplicationOperation<ElementType, IRing<ElementType>, IRing<ElementType>> multiplicator,
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
                multiplicationMatrix[i + 2, i] = vectorsMultiply[0, 0];
            }

            vectorsMultiply = multiplicator.Multiply(rowSubMatrix, mainSubMatrix);
            vectorsMultiply = multiplicator.Multiply(vectorsMultiply, columnSubMatrix);
            for (int i = 0; i < dimension - 2; ++i)
            {
                multiplicationMatrix[i + 3, i] = this.ring.AdditiveInverse(vectorsMultiply[0, 0]);
            }

            for (int i = 3; i < dimension; ++i)
            {
                mainSubMatrix = multiplicator.Multiply(mainSubMatrix, mainSubMatrix);
                vectorsMultiply = multiplicator.Multiply(rowSubMatrix, mainSubMatrix);
                vectorsMultiply = multiplicator.Multiply(mainSubMatrix, columnSubMatrix);
                for (int j = 0; i < dimension - i; ++j)
                {
                    multiplicationMatrix[i + 3, i] = this.ring.AdditiveInverse(vectorsMultiply[0, 0]);
                }
            }
        }
    }
}
