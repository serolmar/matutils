using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Collections;

namespace Mathematics
{
    /// <summary>
    /// Calcula o determinante de uma matriz recorrendo apenas ao método da permutação das linhas ou das colunas.
    /// </summary>
    /// <remarks>
    /// Este método é extremamente ineficaz e serve apenas para efeitos académicos.
    /// </remarks>
    public class PermutationDeterminantCalculator<ElementsType> : ADeterminant<ElementsType>
    {
        /// <summary>
        /// Mantém um valor que indica se é para manter as linhas fixas e permuta as colunas ou vice-versa.
        /// </summary>
        private bool fixLines;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="PermutationDeterminantCalculator{ElementsType}"/>.
        /// </summary>
        /// <param name="ring">O anel responsável pelas operações sobre as entradas.</param>
        /// <param name="fixLines">Se for verdadeiro, faz a expansão por linhas senão faz por colunas.</param>
        public PermutationDeterminantCalculator(IRing<ElementsType> ring, bool fixLines = true)
            : base(ring)
        {
            this.fixLines = fixLines;
        }

        /// <summary>
        /// Calcula o determinante da matriz.
        /// </summary>
        /// <param name="data">A matriz.</param>
        /// <returns>O determinante.</returns>
        protected override ElementsType ComputeDeterminant(IMatrix<ElementsType> data)
        {
            var swapPermutationsAffector = new SwapPermutationsGenerator(data.GetLength(0));
            var currentSignalNegative = false;
            var inverseValue = this.ring.AdditiveInverse(this.ring.MultiplicativeUnity);
            var result = this.ring.AdditiveUnity;
            if (this.fixLines)
            {
                foreach (var swapPermutation in swapPermutationsAffector)
                {
                    var temporary = this.GetProductByColumns(swapPermutation, data);
                    if (currentSignalNegative)
                    {
                        temporary = this.ring.Multiply(temporary, inverseValue);
                    }

                    result = this.ring.Add(result, temporary);
                    currentSignalNegative = !currentSignalNegative;
                }
            }
            else
            {
                foreach (var swapPermutation in swapPermutationsAffector)
                {
                    var temporary = this.GetProductByLines(swapPermutation, data);
                    if (currentSignalNegative)
                    {
                        temporary = this.ring.Multiply(temporary, inverseValue);
                    }

                    result = this.ring.Add(result, temporary);
                    currentSignalNegative = !currentSignalNegative;
                }
            }

            return result;
        }

        /// <summary>
        /// Calcula o produto dos termos por colunas.
        /// </summary>
        /// <param name="columns">As colunas.</param>
        /// <param name="data">A matriz.</param>
        /// <returns>O produto dos termos.</returns>
        private ElementsType GetProductByColumns(int[] columns, IMatrix<ElementsType> data)
        {
            var result = this.ring.MultiplicativeUnity;
            for (int i = 0; i < columns.Length; ++i)
            {
                var currentColumn = columns[i];
                var matrixValue = data[i, currentColumn];
                if (this.ring.IsAdditiveUnity(matrixValue))
                {
                    result = this.ring.AdditiveUnity;
                    i = columns.Length;
                }
                else
                {
                    result = this.ring.Multiply(result, matrixValue);
                }
            }

            return result;
        }

        /// <summary>
        /// Calcula o produto dos termos por linhas.
        /// </summary>
        /// <param name="lines">As linhas.</param>
        /// <param name="data">A matriz.</param>
        /// <returns>O produto dos termos.</returns>
        private ElementsType GetProductByLines(int[] lines, IMatrix<ElementsType> data)
        {
            var result = this.ring.MultiplicativeUnity;
            for (int i = 0; i < lines.Length; ++i)
            {
                var currentLine = lines[i];
                var matrixValue = data[currentLine, i];
                if (this.ring.IsAdditiveUnity(matrixValue))
                {
                    result = this.ring.AdditiveUnity;
                    i = lines.Length;
                }
                else
                {
                    result = this.ring.Multiply(result, matrixValue);
                }
            }

            return result;
        }
    }
}
