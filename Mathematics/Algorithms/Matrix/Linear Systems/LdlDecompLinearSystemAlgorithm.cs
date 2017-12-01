// -----------------------------------------------------------------------
// <copyright file="LdlDecompLinearSystemAlgorithm.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------


namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo para a determinação da solução de um sistema
    /// linear de equações simétrico com o auxílio da decomposição LDL.
    /// </summary>
    /// <remarks>
    /// Caso a matriz associada ao sistema não ser simétrica, 
    /// </remarks>
    /// <typeparam name="CoeffType">
    /// O tipo dos objectos que constituem os coeficientes do sistema.
    /// </typeparam>
    public class SymmetricLdlDecompLinearSystemAlgorithm<CoeffType> : IAlgorithm<
        ISquareMathMatrix<CoeffType>,
        IMathMatrix<CoeffType>,
        LinearSystemSolution<CoeffType>>
    {
        /// <summary>
        /// Mantém o objecto responsável pela decomposição.
        /// </summary>
        private ATriangDiagSymmMatrixDecomp<CoeffType> decompositionAlgorithm;


        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SymmetricLdlDecompLinearSystemAlgorithm{CoeffType}"/>.
        /// </summary>
        /// <param name="decompositionAlgorithm">O algoritmo da decomposição.</param>
        public SymmetricLdlDecompLinearSystemAlgorithm(
            ATriangDiagSymmMatrixDecomp<CoeffType> decompositionAlgorithm)
        {
            if (decompositionAlgorithm == null)
            {
                throw new ArgumentNullException("decompositionAlgorithm");
            }
            else
            {
                this.decompositionAlgorithm = decompositionAlgorithm;
            }
        }

        /// <summary>
        /// Obtém ou atribui o objecto responsável pela decomposição da matriz.
        /// </summary>
        public ATriangDiagSymmMatrixDecomp<CoeffType> DecompositionAlgorithm
        {
            get
            {
                return this.decompositionAlgorithm;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    this.decompositionAlgorithm = value;
                }
            }
        }

        /// <summary>
        /// Executa o algorimtmo que permite determinar a solução do sistema simétrico
        /// com base na decomposição LDL^T.
        /// </summary>
        /// <param name="first">A matriz dos coeficientes das variáveis.</param>
        /// <param name="second">O vector dos coeficientes independentes.</param>
        /// <returns>A solução do sistema.</returns>
        public LinearSystemSolution<CoeffType> Run(
            ISquareMathMatrix<CoeffType> first,
            IMathMatrix<CoeffType> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            else if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            else
            {
                var size = first.GetLength(0);
                if (second.GetLength(0) != size)
                {
                    throw new ArgumentException(
                        "The number of columns in coefficients matrix must equal the number of lines in independent vector.");
                }
                else
                {
                    var result = new LinearSystemSolution<CoeffType>();
                    var decompRes = this.decompositionAlgorithm.Run(
                        first);
                    this.ProcessFirstMatrix(decompRes.UpperTriangularMatrix, second, size);
                    this.ProcessRemainingMatrices(
                        decompRes.UpperTriangularMatrix,
                        decompRes.DiagonalMatrix,
                        second,
                        size,
                        result);
                    return result;
                }
            }
        }

        /// <summary>
        /// Processa a primeira matriz.
        /// </summary>
        /// <param name="upperTriangularMatrix">A matriz triangular superior.</param>
        /// <param name="independent">O vector independente.</param>
        /// <param name="size">O tamanho das matrizes.</param>
        private void ProcessFirstMatrix(
            IMathMatrix<CoeffType> upperTriangularMatrix,
            IMathMatrix<CoeffType> independent,
            int size)
        {
            var field = this.decompositionAlgorithm.Field;
            var innerSize = size - 1;
            for (var i = innerSize - 1; i >= 0; --i)
            {
                var sumValue = field.AdditiveUnity;
                for (var j = innerSize; j > i; ++j)
                {
                    var value = upperTriangularMatrix[i, j];
                    if (field.IsMultiplicativeUnity(value))
                    {
                        sumValue = field.Add(
                            sumValue,
                            independent[j,0]);
                    }
                    else
                    {
                        value = field.Multiply(
                            value,
                            independent[j, 0]);
                    }
                }

                sumValue = field.AdditiveInverse(sumValue);
                independent[i, 0] = field.Add(
                    independent[i, 0],
                    sumValue);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Processa as restantes matrizes.
        /// </summary>
        /// <param name="upperTriangMatrix">A matriz triangular superior.</param>
        /// <param name="diagMatrix">A matriz diagonal.</param>
        /// <param name="independent">O vector independente.</param>
        /// <param name="size">O tamanho das matrizes.</param>
        /// <param name="solution">A solução do sistema.</param>
        private void ProcessRemainingMatrices(
            IMathMatrix<CoeffType> upperTriangMatrix,
            IMathMatrix<CoeffType> diagMatrix,
            IMathMatrix<CoeffType> independent,
            int size,
            LinearSystemSolution<CoeffType> solution)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Implementa o algoritmo para determinação da solução de um sistema
    /// linear de equações com auxílio da decomposição LDL.
    /// </summary>
    public class LdlDecompLinearSystemAlgorithm
    {
    }
}
