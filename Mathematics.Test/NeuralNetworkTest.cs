// -----------------------------------------------------------------------
// <copyright file="NeuralNetworkTest.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mathematics;
    using Utilities;

    /// <summary>
    /// Testa a rede neuronal por camadas.
    /// </summary>
    [TestClass]
    public class FeedForwardNeuralNetworkTest
    {
        /// <summary>
        /// Testa o carregamento do modelo a partir de uma matriz esparsa que representa
        /// uma rede completamente ligada em camadas.
        /// </summary>
        [TestMethod]
        [Description("Tests model loading from sparse matrix with full layered connected matrix.")]
        public void FeedForwardNeuralNetwork_LoadModelSparseMatrix()
        {
            var target = new FeedForwardNeuralNetwork<double>(
                new[] { 2L, 3L, 2L },
                (u, v, l) =>
                {
                    var result = 0.0;
                    for (var i = 0L; i < l; ++i)
                    {
                        result += u[i] * v[i];
                    }

                    return result;
                },
                (d1, d2) =>
                {
                    if (d2 > d1)
                    {
                        return 1.0;
                    }
                    else
                    {
                        return 0.0;
                    }
                });

            var parser = new DoubleParser<string>();
            var matrix = TestsHelper.ReadMatrix(
                5,
                5,
                "[[-1.0, 1.0, 0.5, 0, 0], [1.0, -1.0, 0.5, 0, 0], [0, 0, 0, -1.0, 2.0], [0, 0, 0, 0.5, -1.5], [0, 0, 0, 1.0, -0.5]]",
                (i, j) => new SparseDictionaryMatrix<double>(i, j, 0),
                parser,
                true);
            var vector = TestsHelper.ReadVector(
                5,
                "[-1.0, 0.0, 1.0, -0.5, 0.5]",
                new SparseDictionaryMathVectorFactory<double>(),
                parser,
                true);
            var model = new NeuralNetworkModel<double, SparseDictionaryMatrix<double>, IMathVector<double>>(
                matrix,
                vector);
            target.LoadModelSparse<SparseDictionaryMatrix<double>, ILongSparseMatrixLine<double>, IMathVector<double>>(
                model);

            this.AssertTargetFromMatrix(
                model,
                target);
        }

        /// <summary>
        /// Testa o carregamento do modelo a partir de uma matriz esparsa complexa que representa
        /// uma rede completamente ligada em camadas.
        /// </summary>
        [TestMethod]
        [Description("Tests model loading from sparse matrix with full layered connected matrix.")]
        public void FeedForwardNeuralNetwork_LoadModelComplexSparseMatrix()
        {
            var schema = new[] { 5L, 3L, 4L, 2L, 5L };
            var target = new FeedForwardNeuralNetwork<double>(
                schema,
                (u, v, l) =>
                {
                    var result = 0.0;
                    for (var i = 0L; i < l; ++i)
                    {
                        result += u[i] * v[i];
                    }

                    return result;
                },
                (d1, d2) =>
                {
                    if (d2 > d1)
                    {
                        return 1.0;
                    }
                    else
                    {
                        return 0.0;
                    }
                });

            var model = this.GetComplexTestModel();
            target.LoadModelSparse<CoordinateSparseMathMatrix<double>, ILongSparseMatrixLine<double>, ArrayMathVector<double>>(
                model);

            // Verificação do carregamento
            this.AssertTargetFromMatrix(
                model,
                target);
        }

        /// <summary>
        /// Testa o carregamento do modelo a partir de uma matriz esparsa complexa mas desconexa
        /// que representa uma rede completamente ligada em camadas.
        /// </summary>
        [TestMethod]
        [Description("Tests model loading from sparse matrix with full layered disconnected matrix.")]
        public void FeedForwardNeuralNetwork_LoadModelComplexDisconnectedSparseMatrix()
        {
            var schema = new[] { 5L, 3L, 4L, 2L, 5L };
            var target = new FeedForwardNeuralNetwork<double>(
                schema,
                (u, v, l) =>
                {
                    var result = 0.0;
                    for (var i = 0L; i < l; ++i)
                    {
                        result += u[i] * v[i];
                    }

                    return result;
                },
                (d1, d2) =>
                {
                    if (d2 > d1)
                    {
                        return 1.0;
                    }
                    else
                    {
                        return 0.0;
                    }
                });

            var model = this.GetComplexUnconnectedTestModel();
            target.LoadModelSparse<CoordinateSparseMathMatrix<double>, ILongSparseMatrixLine<double>, ArrayMathVector<double>>(
                model);

            // Verificação do carregamento
            this.AssertTargetFromMatrix(
                model,
                target);
        }

        /// <summary>
        /// Testa o carregamento do modelo a partir de uma matriz esparsa que representa
        /// uma rede completamente ligada em camadas.
        /// </summary>
        [TestMethod]
        [Description("Tests model loading from sparse matrix with full layered connected matrix.")]
        public void FeedForwardNeuralNetwork_LoadModelDenseMatrix()
        {
            var target = new FeedForwardNeuralNetwork<double>(
                new[] { 2L, 3L, 2L },
                (u, v, l) =>
                {
                    var result = 0.0;
                    for (var i = 0L; i < l; ++i)
                    {
                        result += u[i] * v[i];
                    }

                    return result;
                },
                (d1, d2) =>
                {
                    if (d2 > d1)
                    {
                        return 1.0;
                    }
                    else
                    {
                        return 0.0;
                    }
                });

            var parser = new DoubleParser<string>();
            var matrix = TestsHelper.ReadMatrix<double>(
                5,
                5,
                "[[-1.0, 1.0, 0.5, 0, 0], [1.0, -1.0, 0.5, 0, 0], [0, 0, 0, -1.0, 2.0], [0, 0, 0, 0.5, -1.5], [0, 0, 0, 1.0, -0.5]]",
                (i, j) => new ArrayMathMatrix<double>(i, j),
                parser,
                true);
            var vector = TestsHelper.ReadVector(
                5,
                "[-1.0, 0.0, 1.0, -0.5, 0.5]",
                new ArrayVectorFactory<double>(),
                parser,
                true);
            var model = new NeuralNetworkModel<double, ILongMathMatrix<double>, IMathVector<double>>(
                matrix,
                vector);
            target.LoadModel(
                model);
            this.AssertTargetFromMatrix(
                model,
                target);
        }

        /// <summary>
        /// Testa o carregamento do modelo a partir de uma matriz densa complexa que representa
        /// uma rede completamente ligada em camadas.
        /// </summary>
        [TestMethod]
        [Description("Tests model loading from dense matrix with full layered connected matrix.")]
        public void FeedForwardNeuralNetwork_LoadModelComplexDenseMatrix()
        {
            var schema = new[] { 5L, 3L, 4L, 2L, 5L };
            var target = new FeedForwardNeuralNetwork<double>(
                schema,
                (u, v, l) =>
                {
                    var result = 0.0;
                    for (var i = 0L; i < l; ++i)
                    {
                        result += u[i] * v[i];
                    }

                    return result;
                },
                (d1, d2) =>
                {
                    if (d2 > d1)
                    {
                        return 1.0;
                    }
                    else
                    {
                        return 0.0;
                    }
                });

            var model = this.GetComplexTestModel();
            target.LoadModel<CoordinateSparseMathMatrix<double>, ArrayMathVector<double>>(
                model);

            // Verificação do carregamento
            this.AssertTargetFromMatrix(
                model,
                target);
        }

        /// <summary>
        /// Testa a execução do modelo sobre dados.
        /// </summary>
        [TestMethod]
        [Description("Tests the execution of the model over provided data.")]
        public void FeedFrowardNeuralNetwork_RunSimpleMatrixTest()
        {
            var target = new FeedForwardNeuralNetwork<double>(
                new[] {2L, 3L, 2L},
                (u, v, l) =>
                {
                    var result = 0.0;
                    for (var i = 0L; i < l; ++i)
                    {
                        result += u[i] * v[i];
                    }

                    return result;
                },
                (d1, d2) =>
                {
                    if (d2 > d1)
                    {
                        return 1.0;
                    }
                    else
                    {
                        return 0.0;
                    }
                });

            var parser = new DoubleParser<string>();
            var matrix = TestsHelper.ReadMatrix(
                5,
                5,
                "[[-1.0, 1.0, 0.5, 0, 0], [1.0, -1.0, 0.5, 0, 0], [0, 0, 0, -1.0, 2.0], [0, 0, 0, 0.5, -1.5], [0, 0, 0, 1.0, -0.5]]",
                (i, j) => new SparseDictionaryMatrix<double>(i, j, 0),
                parser,
                true);
            var vector = TestsHelper.ReadVector(
                5,
                "[0.5, 0.5, 0.5, 0.5, 0.5]",
                new SparseDictionaryMathVectorFactory<double>(),
                parser,
                true);
            var model = new NeuralNetworkModel<double, SparseDictionaryMatrix<double>, IMathVector<double>>(
                matrix,
                vector);
            target.LoadModel(model);

            var actual = target.Run(new[] { 1.0, 0.0 });
            var expected = new[] { 0.0, 0.0 };

            CollectionAssert.AreEqual(expected, actual);

            actual = target.Run(new[] { 0.0, 1.0 });
            expected = new[] { 0.0, 1.0 };

            CollectionAssert.AreEqual(expected, actual);

            actual = target.Run(new[] { 1.0, -1.0 });
            expected = new[] { 0.0, 0.0 };

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Verifica se o modelo corresponde ao que se encontra armazenado
        /// na rede.
        /// </summary>
        /// <typeparam name="C">O tipo dos objectos que constituem os coeficientes.</typeparam>
        /// <typeparam name="M">O tipo dos objectos que constituem as matrizes.</typeparam>
        /// <typeparam name="V">O tipo dos objectos que constituem os vectores.</typeparam>
        /// <param name="expected">O model com os valores esperados.</param>
        /// <param name="actual">A rede que contém os valores a comparar.</param>
        private void AssertTargetFromMatrix<C, M, V>(
            NeuralNetworkModel<C, M, V> expected,
            FeedForwardNeuralNetwork<C> actual)
            where M : ILongMatrix<C>
            where V : IVector<C>
        {
            var actualTresholds = actual.InternalTresholds;
            var expectedTresholds = expected.Tresholds;
            Assert.AreEqual(expectedTresholds.LongLength, actualTresholds.LongLength);
            for (var i = 0; i < actualTresholds.LongLength; ++i)
            {
                Assert.AreEqual(expectedTresholds[i], actualTresholds[i]);
            }

            var expectedMatrix = expected.WeightsMatrix;
            var actualMatrix = actual.InternalWeights;
            Assert.AreEqual(expectedMatrix.GetLength(0), actualMatrix.LongLength);

            var pointer = 0;
            var currCol = 0L;
            var schema = actual.Schema;
            var currLine = schema[pointer + 1];
            for (var i = 0; i < actualMatrix.LongLength; ++i)
            {
                if (i == currLine)
                {
                    currCol += schema[pointer++];
                    currLine += schema[pointer + 1];
                }

                var actualLine = actualMatrix[i];
                for (var j = 0; j < actualLine.LongLength; ++j)
                {
                    var actualVal = actualLine[j];
                    var expectedVal = expectedMatrix[i, currCol + j];
                    Assert.AreEqual(expectedVal, actualVal);
                }
            }
        }

        /// <summary>
        /// Obtém um modelo para rede complexo associado ao esquema
        /// [5, 3, 4, 2, 5]
        /// </summary>
        /// <returns>
        /// O modelo complexo.
        /// </returns>
        private NeuralNetworkModel<double, CoordinateSparseMathMatrix<double>, ArrayMathVector<double>> GetComplexTestModel()
        {
            var result = new CoordinateSparseMathMatrix<double>(14, 14, 0.0);
            var schema = new[] { 5, 3, 4, 2, 5 };
            var cycleCount = schema.Length - 1;

            // Introdução do primeiro bloco
            var pointer = 0;
            var prevCol = 0;
            var currCol = schema[pointer++];
            var prevLine = 0;
            var currLine = schema[pointer];
            var value = 1.0;

            while (pointer < cycleCount)
            {
                for (var i = prevLine; i < currLine; ++i)
                {
                    for (var j = prevCol; j < currCol; ++j)
                    {
                        result[i, j] = value;
                        value += 0.25;
                    }
                }

                prevCol = currCol;
                currCol += schema[pointer++];
                prevLine = currLine;
                currLine += schema[pointer];
            }

            // Última coluna
            for (var i = prevLine; i < currLine; ++i)
            {
                for (var j = prevCol; j < currCol; ++j)
                {
                    result[i, j] = value;
                    value += 0.25;
                }
            }

            var tresholds = new ArrayMathVector<double>(14);
            value = 0.0;
            for (var i = 0; i < 14; ++i)
            {
                tresholds[i] = value;
                value += 0.25;
            }

            return new NeuralNetworkModel<double, CoordinateSparseMathMatrix<double>, ArrayMathVector<double>>(
                result,
                tresholds);
        }

        /// <summary>
        /// Obtém um modelo para rede disconexo e complexo associado ao esquema
        /// [5, 3, 4, 2, 5]
        /// </summary>
        /// <returns>
        /// O modelo disconexo e complexo.
        /// </returns>
        private NeuralNetworkModel<double, CoordinateSparseMathMatrix<double>, ArrayMathVector<double>> GetComplexUnconnectedTestModel()
        {
            var result = new CoordinateSparseMathMatrix<double>(14, 14, 0.0);
            var schema = new[] { 5, 3, 4, 2, 5 };

            var tresholds = new ArrayMathVector<double>(14);
            var value = 0.0;
            for (var i = 0; i < 14; ++i)
            {
                tresholds[i] = value;
                value += 0.25;
            }

            return new NeuralNetworkModel<double, CoordinateSparseMathMatrix<double>, ArrayMathVector<double>>(
                result,
                tresholds);
        }
    }
}
