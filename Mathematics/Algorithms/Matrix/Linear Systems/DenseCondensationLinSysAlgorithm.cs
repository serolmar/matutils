namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Algoritmo que permite resolver sistemas de equações com base no método de Gauss-Jordan.
    /// </summary>
    public class DenseCondensationLinSysAlgorithm<ElementType>
        : IAlgorithm<IMatrix<ElementType>, IMatrix<ElementType>, LinearSystemSolution<ElementType>>
    {
        /// <summary>
        /// O corpo responsável pelas operações.
        /// </summary>
        private IField<ElementType> field;

        /// <summary>
        /// O algoritmo responsável pela condensação das matrizes.
        /// </summary>
        IAlgorithm<IMatrix<ElementType>, IMatrix<ElementType>, bool> condensationAlgorithm;

        public DenseCondensationLinSysAlgorithm(IField<ElementType> field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            else
            {
                this.field = field;
                this.condensationAlgorithm = new DenseCondensationMethodAlgorithm<ElementType>(
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
            IMatrix<ElementType> coefficientsMatrix,
            IMatrix<ElementType> independentVector)
        {
            this.condensationAlgorithm.Run(coefficientsMatrix, independentVector);
            var result = new LinearSystemSolution<ElementType>();
            var matrixLines = coefficientsMatrix.GetLength(0);
            var matrixColumns = coefficientsMatrix.GetLength(1);
            var currentPivotLine = 0;
            var currentPivotColumn = 0;
            var lastNonZeroColumn = -1;
            var independentSolutionVector = new ArrayVector<ElementType>(matrixColumns, this.field.AdditiveUnity);
            while (currentPivotLine < matrixLines && currentPivotColumn < matrixColumns)
            {
                var pivotValue = coefficientsMatrix[currentPivotLine, currentPivotColumn];
                if (this.field.IsAdditiveUnity(pivotValue))
                {
                    if (this.field.IsAdditiveUnity(independentSolutionVector[currentPivotLine]))
                    {
                        var basisVector = new ArrayVector<ElementType>(matrixColumns,this.field.AdditiveUnity);
                        basisVector[currentPivotColumn] = this.field.AdditiveInverse(
                            this.field.MultiplicativeUnity);
                        var i = currentPivotLine - 1;
                        var j = lastNonZeroColumn;
                        while (i >= 0 && j >= 0)
                        {
                            var linePivotValue = coefficientsMatrix[i, j];
                            if (this.field.IsMultiplicativeUnity(linePivotValue))
                            {
                                basisVector[j] = coefficientsMatrix[i, currentPivotColumn];
                                --i;
                            }

                            --j;
                        }

                        result.VectorSpaceBasis.Add(basisVector);
                    }
                    else
                    {
                        result.Vector = null;
                        result.VectorSpaceBasis.Clear();
                        return result;
                    }
                }
                else
                {
                    lastNonZeroColumn = currentPivotColumn;
                    independentSolutionVector[currentPivotColumn] = independentVector[currentPivotLine, 0];
                    ++currentPivotLine;
                }

                ++currentPivotColumn;
            }

            result.Vector = independentSolutionVector;
            return result;
        }
    }
}
