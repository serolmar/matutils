namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using Utilities;

    [TestClass]
    public class RevisedSimplexAlgorithmTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /// <summary>
        /// Testa o algoritmo do simplex resvisto num cenário simples de maximização cujas restrições
        /// não contêm igualdades.
        /// </summary>
        [TestMethod]
        public void RunTest_OnlyInequalitiesDouble()
        {
            var inputConstraintsMatrixText = "[[1,0,3],[0,2,2]]";
            var inputConstraintsVectorText = "[4,12,18]";
            var inputObjectiveFuncText = "[-3,-5]";
            var inverseMatrixText = "[[1,0,0],[0,1,0],[0,0,1]]";
            var cost = 0.0;
            var nonBasicVariables = new[] { 0, 1 };
            var basicVariables = new[] { 2, 3, 4 };

            // Leitura da matriz das retrições.
            var matrixFactory = new ArrayMatrixFactory<double>();
            var doubleElementsParser = new DoubleParser<string>();
            var inputConstraintsMatrix = TestsHelper.ReadMatrix<double>(
                3,
                2,
                inputConstraintsMatrixText,
                matrixFactory,
                doubleElementsParser,
                true);

            // Leitura da matriz inversa.
            var squareMatrixFactory = new ArraySquareMatrixFactory<double>();
            var inverseMatrix = TestsHelper.ReadMatrix(
                3, 
                3, 
                inverseMatrixText, 
                squareMatrixFactory, 
                doubleElementsParser, 
                true) as ISquareMathMatrix<double>;

            // Leitura do vector de restrições.
            var vectorFactory = new ArrayVectorFactory<double>();
            var inputConstraintsVector = TestsHelper.ReadVector<double>(
                3,
                inputConstraintsVectorText,
                vectorFactory,
                doubleElementsParser,
                true);

            // Leitura da função objectivo.
            var inputObjectiveFunction = TestsHelper.ReadVector<double>(
                2,
                inputObjectiveFuncText,
                vectorFactory,
                doubleElementsParser,
                true);

            // Objecto de entrada para o algoritmo do simplex.
            var simplexInput = new RevisedSimplexInput<double, double>(
                basicVariables,
                nonBasicVariables,
                inputObjectiveFunction,
                cost,
                inputConstraintsMatrix,
                inputConstraintsVector,
                inverseMatrix);

            // Executa o algoritmo do simplex.
            var doubleField = new DoubleField();
            var target = new RevisedSimplexAlgorithm<double>(Comparer<double>.Default, doubleField);
            var actual = target.Run(simplexInput);

            // Verifica o custo
            Assert.AreEqual(36, actual.Cost);

            // Verifica a solução
            Assert.AreEqual(2, actual.Solution[0]);
            Assert.AreEqual(6, actual.Solution[1]);
        }

        /// <summary>
        /// Teste ao algoritmo do simplex para resolver um problema de maximização cujas
        /// restrições contêm uma igualdade.
        /// </summary>
        [TestMethod]
        public void RunTest_OneEquality()
        {
            var inputConstraintsMatrixText = "[[1,0,3],[0,2,2]]";
            var inputConstraintsVectorText = "[4,12,18]";
            var inverseMatrixText = "[[1,0,0],[0,1,0],[0,0,1]]";
            var cost = new SimplexMaximumNumberField<double>(0, -18);
            var nonBasicVariables = new[] { 0, 1 };
            var basicVariables = new[] { 2, 3, 4 };

            // Leitura da matriz das retrições.
            var matrixFactory = new ArrayMatrixFactory<double>();
            var doubleElementsParser = new DoubleParser<string>();
            var inputConstraintsMatrix = TestsHelper.ReadMatrix<double>(
                3,
                2,
                inputConstraintsMatrixText,
                matrixFactory,
                doubleElementsParser,
                true);

            // Leitura da matriz inversa.
            var squareMatrixFactory = new ArraySquareMatrixFactory<double>();
            var inverseMatrix = TestsHelper.ReadMatrix(
                3,
                3,
                inverseMatrixText,
                squareMatrixFactory,
                doubleElementsParser,
                true) as ISquareMathMatrix<double>;

            // Leitura do vector de restrições.
            var vectorFactory = new ArrayVectorFactory<double>();
            var inputConstraintsVector = TestsHelper.ReadVector<double>(
                3,
                inputConstraintsVectorText,
                vectorFactory,
                doubleElementsParser,
                true);

            // Introdução da função objectivo.
            var inputObjectiveFunction = new ArrayMathVector<SimplexMaximumNumberField<double>>(2);
            inputObjectiveFunction[0] = new SimplexMaximumNumberField<double>(
                -3,
                -3);
            inputObjectiveFunction[1] = new SimplexMaximumNumberField<double>(
                -5,
                -2);

            // Objecto de entrada para o algoritmo do simplex.
            var simplexInput = new RevisedSimplexInput<double, SimplexMaximumNumberField<double>>(
                basicVariables,
                nonBasicVariables,
                inputObjectiveFunction,
                cost,
                inputConstraintsMatrix,
                inputConstraintsVector,
                inverseMatrix);

            var doubleField = new DoubleField();
            var target = new RevisedSimplexAlgorithm<double>(Comparer<double>.Default, doubleField);
            var actual = target.Run(simplexInput);

            // Verifica o custo
            Assert.AreEqual(36, actual.Cost);

            // Verifica a solução
            Assert.AreEqual(2, actual.Solution[0]);
            Assert.AreEqual(6, actual.Solution[1]);
        }

        /// <summary>
        /// Testa a função privada responsável pela determinação dos coeficientes dos custos.
        /// </summary>
        [TestMethod]
        public void ComputeCostsCoefficientsTest()
        {
            var field = new DoubleField();
            var revisedSimplexAlg = new RevisedSimplexAlgorithm<double>(
                Comparer<double>.Default,
                field);
            var target = new PrivateObject(revisedSimplexAlg);
            var etaVector = new double[4];

            // Fábricas úteis para o teste dos quatro cenários.
            var arrayMatrixFactory = new ArrayMatrixFactory<double>();
            var squareArrayMatrixFactory = new ArraySquareMatrixFactory<double>();
            var sparseMatrixFactory = new SparseDictionaryMatrixFactory<double>();
            var squareSparseMatrixFactory = new SparseDictionarySquareMatrixFactory<double>();

            this.TestComputeCostsCoefficients(target, squareArrayMatrixFactory, 0, arrayMatrixFactory, 0, etaVector);
            this.TestComputeCostsCoefficients(target, squareArrayMatrixFactory, 0, sparseMatrixFactory, 0, etaVector);
            this.TestComputeCostsCoefficients(target, squareSparseMatrixFactory, 0, arrayMatrixFactory, 0, etaVector);
            this.TestComputeCostsCoefficients(target, squareSparseMatrixFactory, 0, sparseMatrixFactory, 0, etaVector);
        }

        /// <summary>
        /// Realiza o teste sobre a função que permite calcular os custos.
        /// </summary>
        /// <param name="target">O objecto responsável pelo algoritmo.</param>
        /// <param name="inverseMatrixFactory">O objecto responsável pela criação da matriz inversa.</param>
        /// <param name="inverseMatrixDefaultValue">O valor por defeito na matriz inversa.</param>
        /// <param name="constraintsMatrixFactory">O objecto responsável pela criação da matriz das restrições.</param>
        /// <param name="constraintsMatrixDefaultValue">O valor por defeito na matriz das restrições.</param>
        /// <param name="etaVector">O vector que contém o resultado da função.</param>
        private void TestComputeCostsCoefficients(
            PrivateObject target,
            IMatrixFactory<double> inverseMatrixFactory,
            double inverseMatrixDefaultValue,
            IMatrixFactory<double> constraintsMatrixFactory,
            double constraintsMatrixDefaultValue,
            double[] etaVector)
        {
            var enteringResult = new double[6][];
            enteringResult[0] = new double[4] { 14.3, 16.8, 4, 14 };
            enteringResult[1] = new double[4] { 14.5, 17.2, 2, 16 };

            enteringResult[2] = new double[4] { 1.1, 0, 0, 4 };
            enteringResult[3] = new double[4] { 0, 1.5, 1, 3 };
            enteringResult[4] = new double[4] { 2.1, 4.2, 0, 2 };
            enteringResult[5] = new double[4] { 3, 2.3, 1, 1 };

            for (int i = 0; i < enteringResult.Length; ++i)
            {
                var expected = enteringResult[i];
                var arguments = this.GenerateComputeCostsArguments(
                i,
                inverseMatrixFactory,
                0,
                constraintsMatrixFactory,
                0,
                etaVector);
                target.Invoke("ComputeCostsCoefficients", arguments);
                for (int j = 0; j < expected.Length; ++j)
                {
                    this.AssertDoubles(expected[j], etaVector[j]);
                }
            }
        }

        /// <summary>
        /// Cria os argumentos para a função que permite calcular os custos com base no produto de matrizes.
        /// </summary>
        /// <param name="enteringVariable">O valor da variável de entrada.</param>
        /// <param name="inverseMatrixFactory">O objecto responsável pela ciração da matriz inversa.</param>
        /// <param name="inverseMatrixDefaultValue">O valor por defeito na matriz inversa.</param>
        /// <param name="constraintsMatrixFactory">O objecto responsável pela criação da matriz das restrições.</param>
        /// <param name="constraintsMatrixDefaultValue">O valor por defeito na matriz das restrições.</param>
        /// <param name="etaVector">O vector que contém os elementos calculados.</param>
        /// <returns>Os argumentos a utilizar no teste.</returns>
        private object[] GenerateComputeCostsArguments(
            int enteringVariable,
            IMatrixFactory<double> inverseMatrixFactory,
            double inverseMatrixDefaultValue,
            IMatrixFactory<double> constraintsMatrixFactory,
            double constraintsMatrixDefaultValue,
            double[] etaVector)
        {
            var result = new object[4];
            result[0] = enteringVariable;
            result[1] = this.GenerateTestConstraintsMatrix(constraintsMatrixFactory, constraintsMatrixDefaultValue);
            result[2] = this.GenerateTestInverseMatrix(inverseMatrixFactory, inverseMatrixDefaultValue);
            result[3] = etaVector;
            return result;
        }

        /// <summary>
        /// Gera uma matriz de testes quadrada.
        /// </summary>
        /// <remarks>
        /// Matriz gerada:
        ///     1.1 0.0 2.1 3.0
        ///     0.0 1.5 4.2 2.3
        ///     0.0 1.0 0.0 1.0
        ///     4.0 3.0 2.0 1.0
        /// </remarks>
        /// <param name="matrixFactory">O objecto responsável pela criação de matrizes.</param>
        /// <param name="defaultvalue">O valor por defeito.</param>
        /// <returns>A matriz.</returns>
        private ISquareMathMatrix<double> GenerateTestInverseMatrix(
            IMatrixFactory<double> matrixFactory,
            double defaultvalue)
        {
            var result = matrixFactory.CreateMatrix(4, 4, 0.0);

            // Primeira linha.
            result[0, 0] = 1.1;
            result[0, 1] = 0.0;
            result[0, 2] = 2.1;
            result[0, 3] = 3.0;

            // Segunda linha.
            result[1, 0] = 0.0;
            result[1, 1] = 1.5;
            result[1, 2] = 4.2;
            result[1, 3] = 2.3;

            // Terceira linha.
            result[2, 0] = 0.0;
            result[2, 1] = 1.0;
            result[2, 2] = 0.0;
            result[2, 3] = 1.0;

            // Quarta linha.
            result[3, 0] = 4.0;
            result[3, 1] = 3.0;
            result[3, 2] = 2.0;
            result[3, 3] = 1.0;

            return result as ISquareMathMatrix<double>;
        }

        /// <summary>
        /// Gera um vector de teste.
        /// </summary>
        /// <remarks>
        /// Vector:
        ///     1.0 2.0
        ///     1.0 0.0
        ///     2.0 3.0
        ///     3.0 2.0
        /// </remarks>
        /// <param name="matrixFactory">O objecto responsável pela criação de matrizes.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>O vector gerado.</returns>
        private IMathMatrix<double> GenerateTestConstraintsMatrix(
            IMatrixFactory<double> matrixFactory,
            double defaultValue)
        {
            var result = matrixFactory.CreateMatrix(4, 2, defaultValue);

            // Primeira linha.
            result[0, 0] = 1;
            result[0, 1] = 2;

            // Segunda linha.
            result[1, 0] = 1;
            result[1, 1] = 0;

            // Terceira linha.
            result[2, 0] = 2;
            result[2, 1] = 3;

            // Quarta linha.
            result[3, 0] = 3;
            result[3, 1] = 2;

            return result;
        }

        /// <summary>
        /// Assevera a igualdade de números de precisão dupla dentro de um intervalo de precisão.
        /// </summary>
        /// <param name="expected">O número esperado.</param>
        /// <param name="actual">O número obtido.</param>
        private void AssertDoubles(double expected, double actual)
        {
            var error = Math.Abs(expected - actual);
            Assert.IsTrue(error < 0.000001, string.Format("Error: expecting {0} but got {1}.", expected, actual));
        }
    }
}
