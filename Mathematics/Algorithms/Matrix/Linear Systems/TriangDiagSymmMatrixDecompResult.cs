namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Resultado da factorização M=LDL^* da matriz M num produto de uma matriz triangular
    /// inferior, uma matriz diagonal e a transposta da conjugada da matriz triangular inferior.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficientes que constituem as entradas das matrizes.</typeparam>
    public class TriangDiagSymmMatrixDecompResult<CoeffType>
    {
        /// <summary>
        /// A matriz triangular superior que integra a factorização.
        /// </summary>
        private IMathMatrix<CoeffType> upperTriangularMatrix;

        /// <summary>
        /// A matriz diagonal.
        /// </summary>
        private IMathMatrix<CoeffType> diagonalMatrix;

        /// <summary>
        /// Cria uma nova instância de um objecto do tipo <see cref="TriangDiagSymmMatrixDecompResult{CoeffType}"/>.
        /// </summary>
        /// <param name="upperTriangularMatrix">A matriz triangular inferior.</param>
        /// <param name="diagonalMatrix">A matriz diagonal.</param>
        internal TriangDiagSymmMatrixDecompResult(
            IMathMatrix<CoeffType> upperTriangularMatrix,
            IMathMatrix<CoeffType> diagonalMatrix)
        {
            this.upperTriangularMatrix = upperTriangularMatrix;
            this.diagonalMatrix = diagonalMatrix;
        }

        /// <summary>
        /// Obtém a matriz triagular superior que integra a factorizalão.
        /// </summary>
        public IMathMatrix<CoeffType> UpperTriangularMatrix
        {
            get
            {
                return this.upperTriangularMatrix;
            }
        }

        /// <summary>
        /// Obtém a matriz diagonal.
        /// </summary>
        public IMathMatrix<CoeffType> DiagonalMatrix
        {
            get
            {
                return this.diagonalMatrix;
            }
        }
    }
}
