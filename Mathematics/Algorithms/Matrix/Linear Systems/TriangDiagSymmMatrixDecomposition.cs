// -----------------------------------------------------------------------
// <copyright file="TriangDiagSymmMatrixDecomposition.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Define a base para o algoritmo de decomposição LDL de uma matriz.
    /// </summary>
    /// <typeparam name="CoeffType">
    /// O tipo dos objectos que constituem as entradas dos coeficientes.
    /// </typeparam>
    public abstract class ATriangDiagSymmMatrixDecomp<CoeffType> :
        IAlgorithm<ISquareMathMatrix<CoeffType>, TriangDiagSymmMatrixDecompResult<CoeffType>>
    {
        /// <summary>
        /// Mantém o campo responsável pelas operações sobre os coeficientes.
        /// </summary>
        protected IField<CoeffType> field;

        /// <summary>
        /// Mantém o delegado responsável pela criação da matriz triangular da decompsição.
        /// </summary>
        protected Func<int, CoeffType, ISquareMathMatrix<CoeffType>> upperTriangularMatrixFactory;

        /// <summary>
        /// Mantém o delegado responsável pela criação
        /// </summary>
        protected Func<int, CoeffType, ISquareMathMatrix<CoeffType>> diagonalMatrixFactory;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ATriangDiagSymmMatrixDecomp{CoeffType}"/>.
        /// </summary>
        /// <param name="field">O campo responsável pelas operações sobre os coeficientes.</param>
        public ATriangDiagSymmMatrixDecomp(IField<CoeffType> field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            else
            {
                this.field = field;
                this.upperTriangularMatrixFactory = (i, j) => new ArrayTriangUpperMathMatrix<CoeffType>(i, j);
                this.diagonalMatrixFactory = (i, j) => new ArrayDiagonalMathMatrix<CoeffType>(i, j);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ATriangDiagSymmMatrixDecomp{CoeffType}"/>.
        /// </summary>
        /// <param name="field">O campo responsável pelas operações sobre os coeficientes.</param>
        /// <param name="upperTriangularMatrixFactory">
        /// O objecto responsável pela criação da matriz triangular superior da decomposição.
        /// </param>
        /// <param name="diagonalMatrixFactory">
        /// O delegado responsável pela criação da matriz diagonal.
        /// </param>
        public ATriangDiagSymmMatrixDecomp(
            IField<CoeffType> field,
            Func<int, CoeffType, ISquareMathMatrix<CoeffType>> upperTriangularMatrixFactory,
            Func<int, CoeffType, ISquareMathMatrix<CoeffType>> diagonalMatrixFactory)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            else if (upperTriangularMatrixFactory == null)
            {
                throw new ArgumentNullException("upperTriangularMatrixFactory");
            }
            else if (diagonalMatrixFactory == null)
            {
                throw new ArgumentNullException("diagonalMatrixFactory");
            }
            else
            {
                this.field = field;
                this.upperTriangularMatrixFactory = upperTriangularMatrixFactory;
                this.diagonalMatrixFactory = diagonalMatrixFactory;
            }
        }

        /// <summary>
        /// Obtém ou atribui o objecto responsável pelas operações sobre os coeficientes.
        /// </summary>
        public IField<CoeffType> Field
        {
            get
            {
                return this.field;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    this.field = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o delegado responsável pela criação da matriz triangular superior
        /// da decomposição.
        /// </summary>
        public Func<int, CoeffType, ISquareMathMatrix<CoeffType>> UpperTriangularMatrixFactory
        {
            get
            {
                return this.upperTriangularMatrixFactory;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    this.upperTriangularMatrixFactory = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o delegado responsável pela criação da matriz diagonal
        /// da decomposição.
        /// </summary>
        public Func<int, CoeffType, ISquareMathMatrix<CoeffType>> DiagonalMatrixFactory
        {
            get
            {
                return this.diagonalMatrixFactory;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    this.diagonalMatrixFactory = value;
                }
            }
        }

        /// <summary>
        /// Obtém a decomposição de uma matriz M=LDL^* onde L é uma matriz triangular e D é uma matriz diagonal.
        /// </summary>
        /// <remarks>
        /// Caso a matriz não seja simétrica, é considerada a matriz simétrica cujas entradas acima da diagonal
        /// conicidam com as entradas respectivas da matriz original.
        /// </remarks>
        /// <param name="matrix">A matriz.</param>
        /// <returns>As matrizes triangular e diagonal.</returns>
        public abstract TriangDiagSymmMatrixDecompResult<CoeffType> Run(
            ISquareMathMatrix<CoeffType> data);
    }

    /// <summary>
    /// Algoritmo da decomposição de Cholesky de uma matriz simétrica.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos coeficientes que constituem as entradas das matrizes.</typeparam>
    public class TriangDiagSymmMatrixDecomposition<CoeffType> : ATriangDiagSymmMatrixDecomp<CoeffType>
    {
        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TriangDiagSymmMatrixDecomposition{CoeffType}"/>.
        /// </summary>
        /// <param name="field">O objecto responsável pelas operações sobre os coeficientes.</param>
        public TriangDiagSymmMatrixDecomposition(
            IField<CoeffType> field)
            : base(field) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TriangDiagSymmMatrixDecomposition{CoeffType}"/>.
        /// </summary>
        /// <param name="field">O objecto responsável pelas operações sobre os coeficientes.</param>
        /// <param name="upperTriangularMatrixFactory">
        /// O objecto responsável pela criação da matriz triangular superior da decomposição.
        /// </param>
        /// <param name="diagonalMatrixFactory">
        /// O objecto responsável pela criação da matriz diagonal da decomposição.
        /// </param>
        public TriangDiagSymmMatrixDecomposition(
            IField<CoeffType> field,
            Func<int, CoeffType, ISquareMathMatrix<CoeffType>> upperTriangularMatrixFactory,
            Func<int, CoeffType, ISquareMathMatrix<CoeffType>> diagonalMatrixFactory)
            : base(
                field, upperTriangularMatrixFactory,
                diagonalMatrixFactory) { }

        /// <summary>
        /// Obtém a decomposição de uma matriz M=LDL^* onde L é uma matriz triangular e D é uma matriz diagonal.
        /// </summary>
        /// <remarks>
        /// Caso a matriz não seja simétrica, é considerada a matriz simétrica cujas entradas acima da diagonal
        /// conicidam com as entradas respectivas da matriz original.
        /// </remarks>
        /// <param name="matrix">A matriz.</param>
        /// <returns>As matrizes triangular e diagonal.</returns>
        public override TriangDiagSymmMatrixDecompResult<CoeffType> Run(
            ISquareMathMatrix<CoeffType> matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }
            else
            {
                var matrixDimension = matrix.GetLength(0);
                var triangularMatrix = this.upperTriangularMatrixFactory.Invoke(
                    matrixDimension,
                    this.field.AdditiveUnity);
                var diagonalMatrix = this.diagonalMatrixFactory.Invoke(
                    matrixDimension,
                    this.field.AdditiveUnity);
                for (int i = 0; i < matrixDimension; ++i)
                {
                    var sumValue = this.field.AdditiveUnity;
                    for (int j = 0; j < i; ++j)
                    {
                        var diag = diagonalMatrix[j, j];
                        if (!this.field.IsAdditiveUnity(diag))
                        {
                            var value = triangularMatrix[j, i];
                            value = this.field.Multiply(value, value);
                            value = this.field.Multiply(value, diag);
                            sumValue = this.field.Add(sumValue, value);
                        }
                    }

                    sumValue = this.field.Add(
                        matrix[i, i],
                        this.field.AdditiveInverse(sumValue));
                    diagonalMatrix[i, i] = sumValue;

                    for (var j = i + 1; j < matrixDimension; ++j)
                    {
                        sumValue = this.field.AdditiveUnity;
                        for (int k = 0; k < i; ++k)
                        {
                            var diag = diagonalMatrix[k, k];
                            if (!this.field.IsAdditiveUnity(diag))
                            {
                                var value = triangularMatrix[k, i];
                                value = this.field.Multiply(value, triangularMatrix[k, j]);
                                value = this.field.Multiply(value, diagonalMatrix[k, k]);
                                sumValue = this.field.Add(sumValue, value);
                            }
                        }

                        triangularMatrix[i, j] = this.field.Add(
                                 matrix[i, j],
                                 this.field.AdditiveInverse(sumValue));
                    }

                    var diagonalValue = diagonalMatrix[i, i];
                    if (!this.field.IsAdditiveUnity(diagonalValue))
                    {
                        triangularMatrix.ScalarLineMultiplication(
                                i,
                                this.field.MultiplicativeInverse(diagonalMatrix[i, i]),
                                this.field);
                    }

                    triangularMatrix[i, i] = this.field.MultiplicativeUnity;
                }

                return new TriangDiagSymmMatrixDecompResult<CoeffType>(
                    triangularMatrix,
                    diagonalMatrix);
            }
        }
    }


    /// <summary>
    /// Algoritmo da decomposição de Cholesky de uma matriz simétrica.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos coeficientes que constituem as entradas das matrizes.</typeparam>
    public class ParallelTriangDiagSymmMatrixDecomp<CoeffType> : ATriangDiagSymmMatrixDecomp<CoeffType>
    {
        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ParallelTriangDiagSymmMatrixDecomp{CoeffType}"/>.
        /// </summary>
        /// <param name="field">O objecto responsável pelas operações sobre os coeficientes.</param>
        public ParallelTriangDiagSymmMatrixDecomp(
            IField<CoeffType> field)
            : base(field) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ParallelTriangDiagSymmMatrixDecomp{CoeffType}"/>.
        /// </summary>
        /// <param name="field">O objecto responsável pelas operações sobre os coeficientes.</param>
        /// <param name="upperTriangularMatrixFactory">
        /// O objecto responsável pela criação da matriz triangular superior da decomposição.
        /// </param>
        /// <param name="diagonalMatrixFactory">
        /// O objecto responsável pela criação da matriz diagonal da decomposição.
        /// </param>
        public ParallelTriangDiagSymmMatrixDecomp(
            IField<CoeffType> field,
            Func<int, CoeffType, ISquareMathMatrix<CoeffType>> upperTriangularMatrixFactory,
            Func<int, CoeffType, ISquareMathMatrix<CoeffType>> diagonalMatrixFactory)
            : base(
                field, upperTriangularMatrixFactory,
                diagonalMatrixFactory) { }

        /// <summary>
        /// Obtém a decomposição de uma matriz M=LDL^* onde L é uma matriz triangular e D é uma matriz diagonal.
        /// </summary>
        /// <param name="matrix">A matriz.</param>
        /// <param name="field">O corpo responsável pelas operações sobre os coeficientes.</param>
        /// <param name="upperTriangularMatrixFactory">A fábrica responsável pela criação do contentor para a
        /// matriz triangular.
        /// </param>
        /// <param name="diagonalMatrixFactory">A fábrica para o contentor da matriz diagonal.</param>
        /// <returns>As matrizes triangular e diagonal.</returns>
        public override TriangDiagSymmMatrixDecompResult<CoeffType> Run(
            ISquareMathMatrix<CoeffType> matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }
            else
            {
                var matrixDimension = matrix.GetLength(0);
                var triangularMatrix = this.upperTriangularMatrixFactory.Invoke(
                    matrixDimension,
                    this.field.AdditiveUnity);
                var diagonalMatrix = this.diagonalMatrixFactory.Invoke(
                    matrixDimension,
                    this.field.AdditiveUnity);
                for (int i = 0; i < matrixDimension; ++i)
                {
                    var diagonalTask = new Task(() =>
                    {
                        var sumValue = this.field.AdditiveUnity;
                        for (int j = 0; j < i; ++j)
                        {
                            var diag = diagonalMatrix[j, j];
                            if (!this.field.IsAdditiveUnity(diag))
                            {
                                var value = triangularMatrix[j, i];
                                value = this.field.Multiply(value, value);
                                value = this.field.Multiply(value, diag);
                                sumValue = this.field.Add(sumValue, value);
                            }
                        }

                        sumValue = this.field.Add(
                            matrix[i, i],
                            this.field.AdditiveInverse(sumValue));
                        diagonalMatrix[i, i] = sumValue;
                    });

                    var triangularTask = new Task(() =>
                    {
                        Parallel.For(i + 1, matrixDimension, (j, state) =>
                        {
                            var sumValue = this.field.AdditiveUnity;
                            for (int k = 0; k < i; ++k)
                            {
                                var value = triangularMatrix[k, i];
                                value = this.field.Multiply(value, triangularMatrix[k, j]);
                                value = this.field.Multiply(value, diagonalMatrix[k, k]);
                                sumValue = this.field.Add(sumValue, value);
                            }

                            triangularMatrix[i, j] = this.field.Add(
                                     matrix[i, j],
                                     this.field.AdditiveInverse(sumValue));
                        });
                    });

                    diagonalTask.Start();
                    triangularTask.Start();
                    Task.WaitAll(new[] { diagonalTask, triangularTask });

                    var diagonalValue = diagonalMatrix[i, i];
                    if (!this.field.IsAdditiveUnity(diagonalValue))
                    {
                        triangularMatrix.ScalarLineMultiplication(
                                i,
                                this.field.MultiplicativeInverse(diagonalMatrix[i, i]),
                                this.field);
                    }

                    triangularMatrix[i, i] = this.field.MultiplicativeUnity;
                }

                return new TriangDiagSymmMatrixDecompResult<CoeffType>(
                    triangularMatrix,
                    diagonalMatrix);
            }
        }
    }
}
