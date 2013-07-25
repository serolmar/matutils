using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Collections.Affectations;

namespace Mathematics
{
    /// <summary>
    /// Calcula o determinante de uma matriz recorrendo apenas ao método da permutação das linhas ou das colunas.
    /// </summary>
    /// <remarks>
    /// Este método é extremamente ineficaz e serve apenas para efeitos académicos.
    /// </remarks>
    public class PermutationDeterminantCalculator<ElementsType, RingType> : ADeterminant<ElementsType, RingType>
        where RingType : IRing<ElementsType>
    {
        /// <summary>
        /// Mantém um valor que indica se é para manter as linhas fixas e permuta as colunas ou vice-versa.
        /// </summary>
        private bool fixLines;

        public PermutationDeterminantCalculator(RingType ring, bool fixLines = true)
            : base(ring)
        {
            this.fixLines = fixLines;
        }

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
