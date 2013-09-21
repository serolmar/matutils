namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Algoritmo que permite resolver sistemas de equações com base no método de Gauss-Jordan.
    /// </summary>
    public class ArrayMatrixCondLinSysAlgorithm<ElementType>
        : IAlgorithm<ArrayMatrix<ElementType>, ArrayMatrix<ElementType>, LinearSystemSolution<ElementType>>
    {
        /// <summary>
        /// O corpo responsável pelas operações.
        /// </summary>
        private IField<ElementType> field;

        /// <summary>
        /// O algoritmo responsável pela condensação das matrizes.
        /// </summary>
        IAlgorithm<ArrayMatrix<ElementType>, ArrayMatrix<ElementType>, bool> condensationAlgorithm;

        public ArrayMatrixCondLinSysAlgorithm(IField<ElementType> field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            else
            {
                this.field = field;
                this.condensationAlgorithm = new ArrayMatrixCondensationMethodAlgorithm<ElementType>(
                    field);
            }
        }

        /// <summary>
        /// Determina a solução de um sistema linear de equações.
        /// </summary>
        /// <param name="coefficientsMatrix">A matriz dos coeficientes.</param>
        /// <param name="independentVector">O vector independente.</param>
        /// <returns>A solução do sistema.</returns>
        public LinearSystemSolution<ElementType> Run(
            ArrayMatrix<ElementType> coefficientsMatrix,
            ArrayMatrix<ElementType> independentVector)
        {
            this.condensationAlgorithm.Run(coefficientsMatrix, independentVector);
            var result = new LinearSystemSolution<ElementType>();
            var matrixLines = coefficientsMatrix.GetLength(0);
            var matrixColumns = coefficientsMatrix.GetLength(1);
            var currentPivotLine = 0;
            var currentPivotColumn = 0;
            var independentSolutionVector = new ArrayMatrix<ElementType>(matrixColumns, 1, this.field.AdditiveUnity);
            while (currentPivotLine < matrixLines && currentPivotColumn < matrixColumns)
            {
                var pivotValue = coefficientsMatrix[currentPivotLine, currentPivotColumn];
                if (this.field.IsAdditiveUnity(pivotValue))
                {
                    var basisVector = new ArrayMatrix<ElementType>(matrixColumns, 1, this.field.AdditiveUnity);
                    basisVector[currentPivotColumn, 0] = this.field.AdditiveInverse(
                        this.field.MultiplicativeUnity);
                    for (int i = currentPivotLine - 1; i >= 0; --i)
                    {
                        basisVector[i, 0] = coefficientsMatrix[i, currentPivotColumn];
                    }

                    result.VectorSpaceBasis.Add(basisVector);
                }
                else
                {
                    independentSolutionVector[currentPivotColumn, 0] = independentVector[currentPivotLine, 0];
                    ++currentPivotLine;
                }

                ++currentPivotColumn;
            }

            result.Vector = independentSolutionVector;
            return result;
        }
    }
}
