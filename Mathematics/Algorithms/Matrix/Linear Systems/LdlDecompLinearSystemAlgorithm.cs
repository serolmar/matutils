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
        /// Delegado responsável pela criação do vector independente da solução.
        /// </summary>
        private Func<int, IMathVector<CoeffType>> independentVectorFactory;

        /// <summary>
        /// Delegado responsável pela criação dos vectores da base da solução.
        /// </summary>
        private Func<int, CoeffType, IMathVector<CoeffType>> basisVectorFactory;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SymmetricLdlDecompLinearSystemAlgorithm{CoeffType}"/>.
        /// </summary>
        /// <param name="independentVectorFactory">
        /// O delegado responsável pela criação da matriz associada ao vector independente da solução,
        /// devendo ser possível explicitar um valor por defeito.
        /// </param>
        /// <param name="basisVectorFactory">
        /// O delegado responsável pela criação das matrizes associadas aos vectores da base da solução.
        /// </param>
        public SymmetricLdlDecompLinearSystemAlgorithm(
            Func<int, IMathVector<CoeffType>> independentVectorFactory,
            Func<int, CoeffType, IMathVector<CoeffType>> basisVectorFactory)
        {
            if (independentVectorFactory == null)
            {
                throw new ArgumentNullException("independentVectorFactory");
            }
            else if (basisVectorFactory == null)
            {
                throw new ArgumentNullException("basisVectorFactory");
            }
            else
            {
                this.independentVectorFactory = independentVectorFactory;
                this.basisVectorFactory = basisVectorFactory;
            }
        }

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
                this.independentVectorFactory = m => new ArrayMathVector<CoeffType>(m);
                this.basisVectorFactory = (m, d) => new ArrayMathVector<CoeffType>(m, d);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SymmetricLdlDecompLinearSystemAlgorithm{CoeffType}"/>.
        /// </summary>
        /// <param name="decompositionAlgorithm">O algoritmo da decomposição.</param>
        /// <param name="independentVectorFactory">
        /// O delegado responsável pela criação da matriz associada ao vector independente da solução,
        /// devendo ser possível explicitar um valor por defeito.
        /// </param>
        /// <param name="basisVectorFactory">
        /// O delegado responsável pela criação das matrizes associadas aos vectores da base da solução.
        /// </param>
        public SymmetricLdlDecompLinearSystemAlgorithm(
            ATriangDiagSymmMatrixDecomp<CoeffType> decompositionAlgorithm,
            Func<int, IMathVector<CoeffType>> independentVectorFactory,
            Func<int, CoeffType, IMathVector<CoeffType>> basisVectorFactory)
        {
            if (decompositionAlgorithm == null)
            {
                throw new ArgumentNullException("decompositionAlgorithm");
            }
            else if (independentVectorFactory == null)
            {
                throw new ArgumentNullException("independentVectorFactory");
            }
            else if (basisVectorFactory == null)
            {
                throw new ArgumentNullException("basisVectorFactory");
            }
            else
            {
                this.decompositionAlgorithm = decompositionAlgorithm;
                this.independentVectorFactory = independentVectorFactory;
                this.basisVectorFactory = basisVectorFactory;
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
        /// Obtém ou atribui o delegado responsável pela criação do vector independente da solução.
        /// </summary>
        private Func<int, IMathVector<CoeffType>> IndependentVectorFactory
        {
            get
            {
                return this.independentVectorFactory;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    this.independentVectorFactory = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o delegado responsável pela criação dos vectores da base da solução.
        /// </summary>
        private Func<int, CoeffType, IMathVector<CoeffType>> BasisVectorFactory
        {
            get
            {
                return this.basisVectorFactory;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    this.basisVectorFactory = value;
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
                    var indSol = this.ProcessFirstMatrix(
                        decompRes.UpperTriangularMatrix, 
                        second, 
                        size);
                    if (this.ProcessDiagonal(decompRes.DiagonalMatrix, indSol, size))
                    {
                        this.ProcessRemainingMatrices(
                            decompRes.UpperTriangularMatrix,
                            decompRes.DiagonalMatrix,
                            indSol,
                            size,
                            result);
                    }

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
        private IMathVector<CoeffType> ProcessFirstMatrix(
            IMathMatrix<CoeffType> upperTriangularMatrix,
            IMathMatrix<CoeffType> independent,
            int size)
        {
            var result = this.independentVectorFactory.Invoke(size);
            var field = this.decompositionAlgorithm.Field;
            for (var j = 0; j < size; ++j)
            {
                var sumValue = field.AdditiveUnity;
                for (var i = 0; i < j; ++i)
                {
                    var value = upperTriangularMatrix[i, j];
                    if (field.IsMultiplicativeUnity(value))
                    {
                        sumValue = field.Add(
                            sumValue,
                            result[i]);
                    }
                    else
                    {
                        value = field.Multiply(
                            value,
                            result[i]);
                        sumValue = field.Add(
                            sumValue,
                            value);
                    }
                }

                sumValue = field.AdditiveInverse(sumValue);
                result[j] = field.Add(
                    independent[j, 0],
                    sumValue);
            }

            return result;
        }

        /// <summary>
        /// Averigua se o sistema possui alguma solução.
        /// </summary>
        /// <param name="diagonalMatrix">A matriz diagonal.</param>
        /// <param name="indSolVector">O vector independente da solução.</param>
        /// <param name="size">A dimensão das matrizes.</param>
        /// <returns>
        /// Verdadeiro caso o sistema tenha solução e falso caso contrário.
        /// </returns>
        private bool ProcessDiagonal(
            IMathMatrix<CoeffType> diagonalMatrix,
            IMathVector<CoeffType> indSolVector,
            int size)
        {
            var field = this.decompositionAlgorithm.Field;
            for (var i = 0; i < size; ++i)
            {
                var diagValue = diagonalMatrix[i, i];
                if (field.IsAdditiveUnity(diagValue))
                {
                    var indValue = indSolVector[i];
                    if (!field.IsAdditiveUnity(indValue))
                    {
                        return false;
                    }
                }
                else
                {
                    indSolVector[i] = field.Multiply(
                        indSolVector[i],
                        field.MultiplicativeInverse(diagValue));
                }
            }

            return true;
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
            IMathVector<CoeffType> independent,
            int size,
            LinearSystemSolution<CoeffType> solution)
        {
            var field = this.decompositionAlgorithm.Field;
            var innerSize = size - 1;
            for (var i = innerSize; i >= 0; --i)
            {
                var diagValue = diagMatrix[i, i];
                if (field.IsAdditiveUnity(diagValue))
                {
                    var basisVector = this.basisVectorFactory.Invoke(
                        size,
                        field.AdditiveUnity);
                    var basisVal = field.MultiplicativeUnity;
                    basisVector[i] = basisVal;

                    for (var j = i - 1; j >= 0; --j)
                    {
                        if (!field.IsAdditiveUnity(diagMatrix[j, j]))
                        {
                            var curr = upperTriangMatrix[j, i];
                            curr = field.Multiply(
                                curr,
                                basisVal);
                            curr = field.AdditiveInverse(curr);
                            basisVector[j] = field.Add(
                                basisVector[j],
                                curr);
                        }
                    }

                    solution.VectorSpaceBasis.Add(basisVector);
                }
                else
                {
                    var value = independent[i];
                    if (!field.IsAdditiveUnity(value))
                    {
                        for (var j = i - 1; j >= 0; --j)
                        {
                            if (!field.IsAdditiveUnity(diagMatrix[j, j]))
                            {
                                var curr = upperTriangMatrix[j, i];
                                curr = field.Multiply(
                                    curr,
                                    value);
                                curr = field.AdditiveInverse(curr);
                                independent[j] = field.Add(
                                    independent[j],
                                    curr);
                            }
                        }
                    }

                    var basis = solution.VectorSpaceBasis;
                    var basisCount = basis.Count;
                    for (var k = 0; k < basisCount; ++k)
                    {
                        var vec = basis[k];
                        var basisVal = vec[i];
                        if (!field.IsAdditiveUnity(basisVal))
                        {
                            for (var j = i - 1; j >= 0; --j)
                            {
                                if (!field.IsAdditiveUnity(diagMatrix[j, j]))
                                {
                                    var curr = upperTriangMatrix[j, i];
                                    curr = field.Multiply(
                                        curr,
                                        basisVal);
                                    curr = field.AdditiveInverse(curr);
                                    vec[j] = field.Add(
                                        vec[j],
                                        curr);
                                }
                            }
                        }
                    }
                }
            }

            solution.Vector = independent;
        }
    }

    /// <summary>
    /// Implementa o algoritmo para determinação da solução de um sistema
    /// linear de equações com auxílio da decomposição LDL.
    /// </summary>
    public class LdlDecompLinearSystemAlgorithm<CoeffType> : IAlgorithm<
        IMathMatrix<CoeffType>,
        IMathMatrix<CoeffType>,
        LinearSystemSolution<CoeffType>>
    {
        /// <summary>
        /// O algoritmo que permite resolver sistemas simétricos.
        /// </summary>
        private IAlgorithm<
            ISquareMathMatrix<CoeffType>,
            IMathMatrix<CoeffType>,
            LinearSystemSolution<CoeffType>> symmSolvAlg;

        /// <summary>
        /// O anel responsável pelas operações sobre os coeficientes.
        /// </summary>
        private IRing<CoeffType> ring;

        /// <summary>
        /// Delegado responsável pela criação da matriz quadrada intermédia.
        /// </summary>
        private Func<int, ISquareMathMatrix<CoeffType>> squareMatrixFactory;

        /// <summary>
        /// Delegado responsável pela criação da matriz dos coeficientes independentes.
        /// </summary>
        private Func<int, IMathMatrix<CoeffType>> independentVectorFactory;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LdlDecompLinearSystemAlgorithm{Coefftype}"/>.
        /// </summary>
        /// <param name="symmSolvAlg">O algoritmo que permite resolver sistemas simétricos.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        public LdlDecompLinearSystemAlgorithm(IAlgorithm<
            ISquareMathMatrix<CoeffType>,
            IMathMatrix<CoeffType>,
            LinearSystemSolution<CoeffType>> symmSolvAlg,
            IRing<CoeffType> ring)
        {
            if (symmSolvAlg == null)
            {
                throw new ArgumentNullException("symmSolvAlg");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                this.symmSolvAlg = symmSolvAlg;
                this.ring = ring;
                this.squareMatrixFactory = i => new ArraySquareMathMatrix<CoeffType>(i);
                this.independentVectorFactory = i => new ArrayMathMatrix<CoeffType>(i, 1);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LdlDecompLinearSystemAlgorithm{Coefftype}"/>.
        /// </summary>
        /// <param name="symmSolvAlg">O algoritmo que permite resolver sistemas simétricos.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <param name="squareMatrixFactory">O delegado responsável pela criação da matriz quadrada.</param>
        public LdlDecompLinearSystemAlgorithm(IAlgorithm<
            ISquareMathMatrix<CoeffType>,
            IMathMatrix<CoeffType>,
            LinearSystemSolution<CoeffType>> symmSolvAlg,
            IRing<CoeffType> ring,
            Func<int, ISquareMathMatrix<CoeffType>> squareMatrixFactory,
            Func<int, IMathMatrix<CoeffType>> independentVectorFactory)
        {
            if (symmSolvAlg == null)
            {
                throw new ArgumentNullException("symmSolvAlg");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (squareMatrixFactory == null)
            {
                throw new ArgumentNullException("squareMatrixFactory");
            }
            else if (independentVectorFactory == null)
            {
                throw new ArgumentNullException("independentVectorFactory");
            }
            else
            {
                this.symmSolvAlg = symmSolvAlg;
                this.ring = ring;
                this.squareMatrixFactory = squareMatrixFactory;
                this.independentVectorFactory = independentVectorFactory;
            }
        }

        /// <summary>
        /// Obtém ou atribui o anel responsável pelas operações sobre os coeficientes.
        /// </summary>
        public IRing<CoeffType> Ring
        {
            get
            {
                return this.ring;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    this.ring = value;
                }
            }
        }

        /// <summary>
        /// Delegado responsável pela criação da matriz quadrada intermédia.
        /// </summary>
        public Func<int, ISquareMathMatrix<CoeffType>> SquareMatrixFactory
        {
            get
            {
                return this.squareMatrixFactory;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    this.squareMatrixFactory = value;
                }
            }
        }

        /// <summary>
        /// Delegado responsável pela criação da matriz dos coeficientes independentes.
        /// </summary>
        public Func<int, IMathMatrix<CoeffType>> IndependentVectorFactory
        {
            get
            {
                return this.independentVectorFactory;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    this.independentVectorFactory = value;
                }
            }
        }

        /// <summary>
        /// Resolve o sistema de equações com o auxílio da decomposição LDL.
        /// </summary>
        /// <param name="first">A matriz dos coeficientes das variáveis.</param>
        /// <param name="second">O vector dos coeficientes independentes.</param>
        /// <returns>A solução do sistema.</returns>
        public LinearSystemSolution<CoeffType> Run(
            IMathMatrix<CoeffType> first, 
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
                var squareMatrix = this.GetSymmetricMatrix(first);
                var vec = this.GetTransformedVector(first, second);
                var solution = this.symmSolvAlg.Run(squareMatrix, vec);
                this.TestSolution(first, second, solution);
                return solution;
            }
        }

        /// <summary>
        /// Obtém a simetrização da matriz.
        /// </summary>
        /// <param name="matrix">A matriz original.</param>
        /// <returns>A matriz simétrica.</returns>
        private ISquareMathMatrix<CoeffType> GetSymmetricMatrix(
            IMathMatrix<CoeffType> matrix)
        {
            var lines = matrix.GetLength(0);
            var columns = matrix.GetLength(1);
            var result = this.squareMatrixFactory.Invoke(columns);

            for (var i = 0; i < columns; ++i)
            {
                for (var j = 0; j < columns; ++j)
                {
                    var sum = this.ring.AdditiveUnity;
                    for (var k = 0; k < lines; ++k)
                    {
                        var value = this.ring.Multiply(
                            matrix[k, i],
                            matrix[k, j]);
                        sum = this.ring.Add(
                            sum,
                            value);
                    }

                    result[i, j] = sum;
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém o vector dos coeficientes independentes após a transformação.
        /// </summary>
        /// <param name="matrix">A matriz dos coeficientes das variáveis.</param>
        /// <param name="independent">O vector dos coeficientes independentes.</param>
        /// <returns>O vector transformado.</returns>
        private IMathMatrix<CoeffType> GetTransformedVector(
            IMathMatrix<CoeffType> matrix,
            IMathMatrix<CoeffType> independent)
        {
            var lines = matrix.GetLength(0);
            var columns = matrix.GetLength(1);
            var result = this.independentVectorFactory.Invoke(columns);
            for (var i = 0; i < columns; ++i)
            {
                var sum = this.ring.AdditiveUnity;
                for (var j = 0; j < lines; ++j)
                {
                    var value = this.ring.Multiply(
                        matrix[j, i],
                        independent[j,0]);
                    sum = this.ring.Add(
                        sum,
                        value);
                }

                result[i,0] = sum;
            }

            return result;
        }

        /// <summary>
        /// Testa a solução do sistema de equações.
        /// </summary>
        /// <param name="matrix">A matriz dos coeficientes das variáveis.</param>
        /// <param name="independent">O vector dos coeficientes independentes.</param>
        /// <param name="solution">A solução do sistema simétrico.</param>
        private void TestSolution(
            IMathMatrix<CoeffType> matrix,
            IMathMatrix<CoeffType> independent,
            LinearSystemSolution<CoeffType> solution)
        {
            if (solution.Vector != null)
            {
                var vec = solution.Vector;
                var lines = matrix.GetLength(0);
                var columns = matrix.GetLength(1);
                for (var i = 0; i < lines; ++i)
                {
                    var sum = this.ring.AdditiveUnity;
                    for (var j = 0; j < columns; ++j)
                    {
                        var value = this.ring.Multiply(
                            matrix[i, j],
                            vec[j]);
                        sum = this.ring.Add(
                            sum,
                            value);
                    }

                    if (!this.ring.Equals(sum, independent[i, 0]))
                    {
                        solution.Vector = null;
                        solution.VectorSpaceBasis.Clear();
                    }
                }
            }
        }
    }
}
