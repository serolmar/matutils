namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using Utilities;

    [TestClass]
    public class SimplexAlgorithmTest
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
        /// Testa o algoritmo do simplex num cenário simples de maximização cujas restrições
        /// não contêm igualdades.
        /// </summary>
        [TestMethod]
        public void RunTest_OnlyInequalitiesDouble()
        {
            var inputConstraintsMatrixText = "[[1,0,3],[0,2,2]]";
            var inputConstraintsVectorText = "[4,12,18]";
            var inputObjectiveFuncText = "[-3,-5]";
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
            var simplexInput = new SimplexInput<double, double>(
                basicVariables,
                nonBasicVariables,
                inputObjectiveFunction,
                cost,
                inputConstraintsMatrix,
                inputConstraintsVector);

            // Executa o algoritmo do simplex.
            var doubleField = new DoubleField();
            var target = new SimplexAlgorithm<double>(Comparer<double>.Default, doubleField);
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

            // Leitura do vector de restrições.
            var vectorFactory = new ArrayVectorFactory<double>();
            var inputConstraintsVector = TestsHelper.ReadVector<double>(
                3,
                inputConstraintsVectorText,
                vectorFactory,
                doubleElementsParser,
                true);

            // Introdução da função objectivo.
            var inputObjectiveFunction = new ArrayVector<SimplexMaximumNumberField<double>>(2);
            inputObjectiveFunction[0] = new SimplexMaximumNumberField<double>(
                -3,
                -3);
            inputObjectiveFunction[1] = new SimplexMaximumNumberField<double>(
                -5,
                -2);

            // Objecto de entrada para o algoritmo do simplex.
            var simplexInput = new SimplexInput<double, SimplexMaximumNumberField<double>>(
                basicVariables,
                nonBasicVariables,
                inputObjectiveFunction,
                cost,
                inputConstraintsMatrix,
                inputConstraintsVector);

            var doubleField = new DoubleField();
            var target = new SimplexAlgorithm<double>(Comparer<double>.Default, doubleField);
            var actual = target.Run(simplexInput);

            // Verifica o custo
            Assert.AreEqual(36, actual.Cost);

            // Verifica a solução
            Assert.AreEqual(2, actual.Solution[0]);
            Assert.AreEqual(6, actual.Solution[1]);
        }

        /// <summary>
        /// Teste ao algoritmo do simplex com igualdades para um problema de maximização que não possua
        /// uma solução admissível.
        /// </summary>
        [TestMethod]
        public void RunSimplexTest_NoFeasibleSolution()
        {
            var inputConstraintsMatrixText = "[[0.3,0.5,0.6],[0.1,0.5,0.4],[0,0,-1]]";
            var inputConstraintsVectorText = "[1.8,6,6]";
            var cost = new SimplexMaximumNumberField<double>(0, -12);
            var nonBasicVariables = new[] { 0, 1, 2 };
            var basicVariables = new[] { 3, 4, 5 };

            // Leitura da matriz das retrições.
            var matrixFactory = new ArrayMatrixFactory<double>();
            var doubleElementsParser = new DoubleParser<string>();
            var inputConstraintsMatrix = TestsHelper.ReadMatrix<double>(
                3,
                3,
                inputConstraintsMatrixText,
                matrixFactory,
                doubleElementsParser,
                true);

            // Leitura do vector de restrições.
            var vectorFactory = new ArrayVectorFactory<double>();
            var inputConstraintsVector = TestsHelper.ReadVector<double>(
                3,
                inputConstraintsVectorText,
                vectorFactory,
                doubleElementsParser,
                true);

            // Introdução da função objectivo.
            var inputObjectiveFunction = new ArrayVector<SimplexMaximumNumberField<double>>(3);
            inputObjectiveFunction[0] = new SimplexMaximumNumberField<double>(
                0.4,
                -1.1);
            inputObjectiveFunction[1] = new SimplexMaximumNumberField<double>(
                0.5,
                -0.9);
            inputObjectiveFunction[2] = new SimplexMaximumNumberField<double>(
                0,
                1);

            // Objecto de entrada para o algoritmo do simplex.
            var simplexInput = new SimplexInput<double, SimplexMaximumNumberField<double>>(
                basicVariables,
                nonBasicVariables,
                inputObjectiveFunction,
                cost,
                inputConstraintsMatrix,
                inputConstraintsVector);

            var doubleField = new DoubleField();
            var target = new SimplexAlgorithm<double>(Comparer<double>.Default, doubleField);
            var actual = target.Run(simplexInput);

            // O problema tem uma solução cujo custo é inifito.
            Assert.IsFalse(actual.HasSolution);
        }
    }
}
