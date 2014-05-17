namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa a condensação de Gauss Jordan para resolver um sistema linear.
    /// </summary>
    /// <typeparam name="ElementType">O tipo de dados dos coeficientes.</typeparam>
    public class DenseCondensationMethodAlgorithm<ElementType>
        : IAlgorithm<IMatrix<ElementType>, IMatrix<ElementType>, bool>
    {
        /// <summary>
        /// O corpo responsável pelas operações.
        /// </summary>
        IField<ElementType> field;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="DenseCondensationMethodAlgorithm{ElementType}"/>.
        /// </summary>
        /// <param name="field">O corpo responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="System.ArgumentNullException">Se o corpo for nulo.</exception>
        public DenseCondensationMethodAlgorithm(IField<ElementType> field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            else
            {
                this.field = field;
            }
        }

        /// <summary>
        /// Aplica o método de condensação a uma matriz dependente baseando-se na matriz inicial.
        /// </summary>
        /// <remarks>
        /// As matrizes de entrada serão alteradas. No final, a matriz inicial é condensada e as mesmas operações
        /// são efectuadas sobre todas as linhas da segunda matriz.
        /// </remarks>
        /// <param name="linearSystemMatrix">A matriz de coeficientes.</param>
        /// <param name="independentVector">A matriz dependente.</param>
        /// <returns>Verdadeiro caso tenha sido realizada alguma operação e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        /// <exception cref="MathematicsException">
        /// Se o número de linhas da matriz não conicidir com o tamanho do vector.
        /// </exception>
        public bool Run(
            IMatrix<ElementType> initialMatrix,
            IMatrix<ElementType> finalMatrix)
        {
            if (initialMatrix == null)
            {
                throw new ArgumentNullException("initialMatrix");
            }
            else if (finalMatrix == null)
            {
                throw new ArgumentNullException("finalMatrix");
            }
            else if (initialMatrix.GetLength(0) != finalMatrix.GetLength(0))
            {
                throw new MathematicsException(
                    "Linear system matrix must have the same number of lines as independent vector.");
            }
            else if (initialMatrix.GetLength(0) == 0)
            {
                return false;
            }
            else
            {
                var result = false;
                var initialMatrixLines = initialMatrix.GetLength(0);
                var initialMatrixColumns = initialMatrix.GetLength(1);
                var matrixDimension = Math.Min(initialMatrixLines, initialMatrixColumns);
                var finalMatrixColumns = finalMatrix.GetLength(1);

                var i = 0;
                var currentPivotColumn = 0;
                while (i < initialMatrixLines - 1 && currentPivotColumn < initialMatrixColumns)
                {
                    var pivotValue = initialMatrix[i, currentPivotColumn];
                    if (this.field.IsAdditiveUnity(pivotValue))
                    {
                        var nextPivotCandidate = this.GetNextNonEmptyPivotLineNumber(
                            i,
                            currentPivotColumn,
                            initialMatrix);
                        if (nextPivotCandidate != -1)
                        {
                            initialMatrix.SwapLines(i, nextPivotCandidate);
                            finalMatrix.SwapLines(i, nextPivotCandidate);
                            pivotValue = initialMatrix[i, currentPivotColumn];
                            result = true;
                        }
                    }

                    if (!this.field.IsAdditiveUnity(pivotValue))
                    {
                        if (!this.field.IsMultiplicativeUnity(pivotValue))
                        {
                            result = true;
                            initialMatrix[i, currentPivotColumn] = this.field.MultiplicativeUnity;
                            for (int j = currentPivotColumn + 1; j < initialMatrix.GetLength(1); ++j)
                            {
                                var value = initialMatrix[i, j];
                                if (!this.field.IsAdditiveUnity(value))
                                {
                                    value = this.field.Multiply(
                                        value,
                                        this.field.MultiplicativeInverse(pivotValue));
                                    initialMatrix[i, j] = value;
                                }
                            }

                            for (int j = 0; j < finalMatrixColumns; ++j)
                            {
                                var value = finalMatrix[i, j];
                                if (!this.field.IsAdditiveUnity(value))
                                {
                                    value = this.field.Multiply(
                                        value,
                                        this.field.MultiplicativeInverse(pivotValue));
                                    finalMatrix[i, j] = value;
                                }
                            }
                        }

                        for (int j = i + 1; j < initialMatrixLines; ++j)
                        {
                            var value = initialMatrix[j, currentPivotColumn];
                            if (!this.field.IsAdditiveUnity(value))
                            {
                                result = true;
                                initialMatrix[j, i] = this.field.AdditiveUnity;
                                for (int k = i + 1; k < matrixDimension; ++k)
                                {
                                    var temp = this.field.Multiply(value, initialMatrix[i, k]);
                                    initialMatrix[j, k] = this.field.Add(
                                        initialMatrix[j, k],
                                        this.field.AdditiveInverse(temp));
                                }

                                for (int k = 0; k < finalMatrixColumns; ++k)
                                {
                                    var temp = this.field.Multiply(value, finalMatrix[i, k]);
                                    finalMatrix[j, k] = this.field.Add(
                                        finalMatrix[j, k],
                                        this.field.AdditiveInverse(temp));
                                }
                            }
                        }

                        ++i;
                    }

                    ++currentPivotColumn;
                }

                i = Math.Min(initialMatrixLines - 1, initialMatrixColumns - 1);
                currentPivotColumn = i;
                var state = true;
                while (state)
                {
                    var currenPivotValue = initialMatrix[i, currentPivotColumn];
                    if (this.field.IsMultiplicativeUnity(currenPivotValue))
                    {
                        state = false;
                    }
                    else if (this.field.IsAdditiveUnity(currenPivotValue))
                    {
                        ++currentPivotColumn;
                        if (currentPivotColumn >= initialMatrixColumns)
                        {
                            --i;
                            if (i < 0)
                            {
                                state = false;
                            }
                            else
                            {
                                currentPivotColumn = i;
                            }
                        }
                    }
                }

                while (i > 0 && currentPivotColumn > 0)
                {
                    var pivotValue = initialMatrix[i, currentPivotColumn];
                    if (this.field.IsMultiplicativeUnity(pivotValue))
                    {
                        for (int j = i - 1; j >= 0; --j)
                        {
                            var value = initialMatrix[j, currentPivotColumn];
                            if (!this.field.IsAdditiveUnity(value))
                            {
                                result = true;
                                initialMatrix[j, currentPivotColumn] = this.field.AdditiveUnity;
                                for (int k = currentPivotColumn + 1; k < matrixDimension; ++k)
                                {
                                    var temp = this.field.Multiply(value, initialMatrix[i, k]);
                                    initialMatrix[j, k] = this.field.Add(
                                        initialMatrix[j, k],
                                        this.field.AdditiveInverse(temp));
                                }

                                for (int k = 0; k < finalMatrixColumns; ++k)
                                {
                                    var temp = this.field.Multiply(value, finalMatrix[i, k]);
                                    finalMatrix[j, k] = this.field.Add(
                                        finalMatrix[j, k],
                                        this.field.AdditiveInverse(temp));
                                }
                            }
                        }
                    }

                    --i;
                    --currentPivotColumn;
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o próximo pivô não nulo da matriz.
        /// </summary>
        /// <param name="startLine">A linha onde é iniciada a pesquisa.</param>
        /// <param name="pivotColumn">O coluna pivô.</param>
        /// <param name="data">A matriz.</param>
        /// <returns>A linha cujo pivô seja não nulo e -1 caso não exista.</returns>
        private int GetNextNonEmptyPivotLineNumber(int startLine, int pivotColumn, IMatrix<ElementType> data)
        {
            var result = -1;
            var matrixDimension = data.GetLength(0);
            for (int i = startLine + 1; i < matrixDimension; ++i)
            {
                if (!this.field.IsAdditiveUnity(data[i, pivotColumn]))
                {
                    result = i;
                    i = matrixDimension;
                }
            }

            return result;
        }
    }
}
