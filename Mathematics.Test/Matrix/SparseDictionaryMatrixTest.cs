namespace Mathematics
{
    using Utilities.Collections;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    /// <summary>
    /// Implementa testes sobre matrizes esparsas definidas com base em dicionários.
    /// </summary>
    [TestClass]
    public class SparseDictionaryMatrixTest
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

        #region Combinação de linhas com valor nulo por defeito

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        public void CombineLinesTest_NullDefaultValuesZeroZero()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            // Constrói a linha a ser analisada.
            var matrixColumns = target.GetLength(1);
            var firstLine = new Fraction<int>[matrixColumns];
            for (int i = 0; i < matrixColumns; ++i)
            {
                if (target[0, i] != null)
                {
                    firstLine[i] = fractionField.AdditiveUnity;
                }
            }

            target.CombineLines(0, 1, fractionField.AdditiveUnity, fractionField.AdditiveUnity, fractionField);
            for (int i = 0; i < matrixColumns; ++i)
            {
                Assert.AreEqual(firstLine[i], target[0, i]);
            }

            this.AssertLinesExcept(0, target);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        public void CombineLinesTest_NullDefaultValuesOneZero()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            // Constrói a linha a ser analisada.
            var matrixColumns = target.GetLength(1);
            var firstLine = new Fraction<int>[matrixColumns];
            for (int i = 0; i < matrixColumns; ++i)
            {
                firstLine[i] = target[0, i];
            }

            target.CombineLines(0, 1, fractionField.MultiplicativeUnity, fractionField.AdditiveUnity, fractionField);
            for (int i = 0; i < matrixColumns; ++i)
            {
                Assert.AreEqual(firstLine[i], target[0, i]);
            }

            this.AssertLinesExcept(0, target);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        public void CombineLinesTest_NullDefaultValuesZeroOne()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            // Constrói a linha a ser analisada.
            var matrixColumns = target.GetLength(1);
            var firstLine = new Fraction<int>[matrixColumns];
            for (int i = 0; i < matrixColumns; ++i)
            {
                firstLine[i] = target[1, i];
            }

            target.CombineLines(0, 1, fractionField.AdditiveUnity, fractionField.MultiplicativeUnity, fractionField);
            for (int i = 0; i < matrixColumns; ++i)
            {
                Assert.AreEqual(firstLine[i], target[0, i]);
            }

            this.AssertLinesExcept(0, target);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        public void CombineLinesTest_NullDefaultValuesOneOne()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            // Constrói a linha a ser analisada.
            var matrixColumns = target.GetLength(1);
            var firstLine = new Fraction<int>[matrixColumns];
            for (int i = 0; i < matrixColumns; ++i)
            {
                var firstValue = target[0, i];
                var secondValue = target[1, i];
                if (firstValue != null && secondValue != null)
                {
                    firstLine[i] = fractionField.Add(firstValue, secondValue);
                }
            }

            target.CombineLines(0, 1, fractionField.MultiplicativeUnity, fractionField.MultiplicativeUnity, fractionField);
            for (int i = 0; i < matrixColumns; ++i)
            {
                Assert.AreEqual(firstLine[i], target[0, i]);
            }

            this.AssertLinesExcept(0, target);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        public void CombineLinesTest_NullDefaultValuesAnyZero()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            // Constrói a linha a ser analisada.
            var matrixColumns = target.GetLength(1);
            var firstLine = new Fraction<int>[matrixColumns];
            var product = new Fraction<int>(2, 1, integerDomain);
            for (int i = 0; i < matrixColumns; ++i)
            {
                var value = target[0, i];
                if (value != null)
                {
                    firstLine[i] = fractionField.Multiply(product, value);
                }
            }

            target.CombineLines(0, 1, product, fractionField.AdditiveUnity, fractionField);
            for (int i = 0; i < matrixColumns; ++i)
            {
                Assert.AreEqual(firstLine[i], target[0, i]);
            }

            this.AssertLinesExcept(0, target);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        public void CombineLinesTest_NullDefaultValuesZeroAny()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            // Constrói a linha a ser analisada.
            var matrixColumns = target.GetLength(1);
            var firstLine = new Fraction<int>[matrixColumns];
            var product = new Fraction<int>(2, 1, integerDomain);
            for (int i = 0; i < matrixColumns; ++i)
            {
                var value = target[1, i];
                if (value != null)
                {
                    firstLine[i] = fractionField.Multiply(product, value);
                }
            }

            target.CombineLines(0, 1, fractionField.AdditiveUnity, product, fractionField);
            for (int i = 0; i < matrixColumns; ++i)
            {
                Assert.AreEqual(firstLine[i], target[0, i]);
            }

            this.AssertLinesExcept(0, target);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        public void CombineLinesTest_NullDefaultValuesAnyOne()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            // Constrói a linha a ser analisada.
            var matrixColumns = target.GetLength(1);
            var firstLine = new Fraction<int>[matrixColumns];
            var product = new Fraction<int>(2, 1, integerDomain);
            for (int i = 0; i < matrixColumns; ++i)
            {
                var value = target[0, i];
                if (value != null)
                {
                    value = fractionField.Multiply(product, value);
                    value = fractionField.Add(value, target[1, i]);
                    firstLine[i] = value;
                }
            }

            target.CombineLines(0, 1, product, fractionField.MultiplicativeUnity, fractionField);
            for (int i = 0; i < matrixColumns; ++i)
            {
                Assert.AreEqual(firstLine[i], target[0, i]);
            }

            this.AssertLinesExcept(0, target);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        public void CombineLinesTest_NullDefaultValuesOneAny()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            // Constrói a linha a ser analisada.
            var matrixColumns = target.GetLength(1);
            var firstLine = new Fraction<int>[matrixColumns];
            var product = new Fraction<int>(2, 1, integerDomain);
            for (int i = 0; i < matrixColumns; ++i)
            {
                var value = target[1, i];
                if (value != null)
                {
                    value = fractionField.Multiply(product, value);
                    value = fractionField.Add(value, target[0, i]);
                    firstLine[i] = value;
                }
            }

            target.CombineLines(0, 1, fractionField.MultiplicativeUnity, product, fractionField);
            for (int i = 0; i < matrixColumns; ++i)
            {
                Assert.AreEqual(firstLine[i], target[0, i]);
            }

            this.AssertLinesExcept(0, target);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        public void CombineLinesTest_NullDefaultValuesAnyAny()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            // Constrói a linha a ser analisada.
            var matrixColumns = target.GetLength(1);
            var firstLine = new Fraction<int>[matrixColumns];
            var firstProduct = new Fraction<int>(3, 1, integerDomain);
            var secondProduct = new Fraction<int>(2, 1, integerDomain);
            for (int i = 0; i < matrixColumns; ++i)
            {
                var firstValue = target[0, i];
                var secondValue = target[1, i];
                if (firstValue != null && secondValue != null)
                {
                    firstValue = fractionField.Multiply(firstProduct, firstValue);
                    secondValue = fractionField.Multiply(secondProduct, secondValue);
                    var value = fractionField.Add(firstValue, secondValue);
                    firstLine[i] = value;
                }
            }

            target.CombineLines(0, 1, firstProduct, secondProduct, fractionField);
            for (int i = 0; i < matrixColumns; ++i)
            {
                Assert.AreEqual(firstLine[i], target[0, i]);
            }

            this.AssertLinesExcept(0, target);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MathematicsException))]
        public void CombineLinesTest_NullDefaultValuesZeroZeroException()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            target.CombineLines(1, 2, fractionField.AdditiveUnity, fractionField.AdditiveUnity, fractionField);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MathematicsException))]
        public void CombineLinesTest_NullDefaultValuesZeroZeroOneException()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            target.CombineLines(1, 2, fractionField.AdditiveUnity, fractionField.MultiplicativeUnity, fractionField);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MathematicsException))]
        public void CombineLinesTest_NullDefaultValuesOneZeroException()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            target.CombineLines(1, 2, fractionField.MultiplicativeUnity, fractionField.AdditiveUnity, fractionField);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MathematicsException))]
        public void CombineLinesTest_NullDefaultValuesOneOneException()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            target.CombineLines(1, 2, fractionField.MultiplicativeUnity, fractionField.MultiplicativeUnity, fractionField);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MathematicsException))]
        public void CombineLinesTest_NullDefaultValuesZeroAnyException()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            var secondProduct = new Fraction<int>(2, 1, integerDomain);
            target.CombineLines(1, 2, fractionField.AdditiveUnity, secondProduct, fractionField);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MathematicsException))]
        public void CombineLinesTest_NullDefaultValuesAnyZeroException()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            var firstProduct = new Fraction<int>(2, 1, integerDomain);
            target.CombineLines(1, 2, firstProduct, fractionField.AdditiveUnity, fractionField);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MathematicsException))]
        public void CombineLinesTest_NullDefaultValuesOneAnyException()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            var secondProduct = new Fraction<int>(2, 1, integerDomain);
            target.CombineLines(1, 2, fractionField.MultiplicativeUnity, secondProduct, fractionField);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MathematicsException))]
        public void CombineLinesTest_NullDefaultValuesAnyOneException()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            var firstProduct = new Fraction<int>(2, 1, integerDomain);
            target.CombineLines(1, 2, firstProduct, fractionField.MultiplicativeUnity, fractionField);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é nulo.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MathematicsException))]
        public void CombineLinesTest_NullDefaultValuesAnyAnyException()
        {
            var target = this.GetDefaultMatrix(null);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            var firstProduct = new Fraction<int>(2, 1, integerDomain);
            var secondProduct = new Fraction<int>(3, 1, integerDomain);
            target.CombineLines(1, 2, firstProduct, fractionField.AdditiveUnity, fractionField);
        }

        #endregion Combinação de linhas com valor nulo por defeito

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é zero.
        /// </summary>
        [TestMethod]
        public void CombineLinesTest_ZeroDefaultValuesZeroZero()
        {
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);
            var target = this.GetDefaultMatrix(fractionField.AdditiveUnity);

            var numberOfProcessedLines = target.GetLength(0) - 1;
            for (int i = 0; i < numberOfProcessedLines; ++i)
            {
                target.CombineLines(i, i + 1, fractionField.AdditiveUnity, fractionField.AdditiveUnity, fractionField);
            }

            // Resta apenas a última linha.
            Assert.AreEqual(1, target.GetLines().Count());
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é zero.
        /// </summary>
        [TestMethod]
        public void CombineLinesTest_ZeroDefaultValuesGeneral()
        {
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);

            // Zero e Um
            var target = this.GetDefaultMatrix(fractionField.AdditiveUnity);
            this.AssertTarget(target, fractionField.AdditiveUnity, fractionField.MultiplicativeUnity, fractionField);

            // Um e Zero
            target = this.GetDefaultMatrix(fractionField.AdditiveUnity);
            this.AssertTarget(target, fractionField.MultiplicativeUnity, fractionField.AdditiveUnity, fractionField);

            // Um e Um
            target = this.GetDefaultMatrix(fractionField.AdditiveUnity);
            this.AssertTarget(target, fractionField.MultiplicativeUnity, fractionField.MultiplicativeUnity, fractionField);

            // Arbitrário e Zero
            var product = new Fraction<int>(2, 1, integerDomain);
            target = this.GetDefaultMatrix(fractionField.AdditiveUnity);
            this.AssertTarget(target, product, fractionField.AdditiveUnity, fractionField);

            // Zero e Arbitrário
            product = new Fraction<int>(2, 1, integerDomain);
            target = this.GetDefaultMatrix(fractionField.AdditiveUnity);
            this.AssertTarget(target, fractionField.AdditiveUnity, product, fractionField);

            // Arbitrário e Um
            product = new Fraction<int>(2, 1, integerDomain);
            target = this.GetDefaultMatrix(fractionField.AdditiveUnity);
            this.AssertTarget(target, product, fractionField.MultiplicativeUnity, fractionField);

            // Um e arbitrário
            product = new Fraction<int>(2, 1, integerDomain);
            target = this.GetDefaultMatrix(fractionField.AdditiveUnity);
            this.AssertTarget(target, fractionField.MultiplicativeUnity, product, fractionField);

            // Arbitrário e arbitrário
            var firstProduct = new Fraction<int>(2, 1, integerDomain);
            var secondProduct = new Fraction<int>(3, 1, integerDomain);
            target = this.GetDefaultMatrix(fractionField.AdditiveUnity);
            this.AssertTarget(target, firstProduct, secondProduct, fractionField);
        }

        /// <summary>
        /// Permite testar a combinação de linhas numa matriz na qual o valor por defeito é zero.
        /// </summary>
        [TestMethod]
        public void CombineLinesTest_ArbitraryDefaultValuesGeneral()
        {
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);
            var defaultValue = fractionField.AdditiveUnity;

            // Zero e Zero
            var target = this.GetDefaultMatrix(defaultValue);
            this.AssertTarget(target, fractionField.AdditiveUnity, fractionField.AdditiveUnity, fractionField);

            // Zero e Um
            target = this.GetDefaultMatrix(fractionField.AdditiveUnity);
            this.AssertTarget(target, fractionField.AdditiveUnity, fractionField.MultiplicativeUnity, fractionField);

            // Um e Zero
            target = this.GetDefaultMatrix(defaultValue);
            this.AssertTarget(target, fractionField.MultiplicativeUnity, fractionField.AdditiveUnity, fractionField);

            // Um e Um
            target = this.GetDefaultMatrix(defaultValue);
            this.AssertTarget(target, fractionField.MultiplicativeUnity, fractionField.MultiplicativeUnity, fractionField);

            // Arbitrário e Zero
            var product = new Fraction<int>(2, 1, integerDomain);
            target = this.GetDefaultMatrix(defaultValue);
            this.AssertTarget(target, product, fractionField.AdditiveUnity, fractionField);

            // Zero e Arbitrário
            product = new Fraction<int>(2, 1, integerDomain);
            target = this.GetDefaultMatrix(defaultValue);
            this.AssertTarget(target, fractionField.AdditiveUnity, product, fractionField);

            // Arbitrário e Um
            product = new Fraction<int>(2, 1, integerDomain);
            target = this.GetDefaultMatrix(defaultValue);
            this.AssertTarget(target, product, fractionField.MultiplicativeUnity, fractionField);

            // Um e arbitrário
            product = new Fraction<int>(2, 1, integerDomain);
            target = this.GetDefaultMatrix(defaultValue);
            this.AssertTarget(target, fractionField.MultiplicativeUnity, product, fractionField);

            // Arbitrário e arbitrário
            var firstProduct = new Fraction<int>(2, 1, integerDomain);
            var secondProduct = new Fraction<int>(3, 1, integerDomain);
            target = this.GetDefaultMatrix(defaultValue);
            this.AssertTarget(target, firstProduct, secondProduct, fractionField);
        }

        #region Métodos auxiliares

        /// <summary>
        /// Constrói uma matriz de teste na qual o valor por defeito é nulo.
        /// </summary>
        /// <remarks>
        ///     - 1 - 2 -
        ///     - 3 - 4 -
        ///     1 2 - - 3
        ///     - - - - -
        ///     3 1 4 5 2
        /// </remarks>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>A matriz de teste.</returns>
        private SparseDictionaryMatrix<Fraction<int>> GetDefaultMatrix(Fraction<int> defaultValue)
        {
            var result = new SparseDictionaryMatrix<Fraction<int>>(5, 5, defaultValue);
            var integerDomain = new IntegerDomain();

            // Primeira linha.
            result[0, 1] = new Fraction<int>(1, 1, integerDomain);
            result[0, 3] = new Fraction<int>(1, 1, integerDomain);

            // Segunda linha.
            result[1, 1] = new Fraction<int>(3, 1, integerDomain);
            result[1, 3] = new Fraction<int>(4, 1, integerDomain);

            // Terceira linha.
            result[2, 0] = new Fraction<int>(1, 1, integerDomain);
            result[2, 1] = new Fraction<int>(2, 1, integerDomain);
            result[2, 4] = new Fraction<int>(3, 1, integerDomain);

            // Quinta linha.
            result[4, 0] = new Fraction<int>(3, 1, integerDomain);
            result[4, 1] = new Fraction<int>(1, 1, integerDomain);
            result[4, 2] = new Fraction<int>(4, 1, integerDomain);
            result[4, 3] = new Fraction<int>(5, 1, integerDomain);
            result[4, 4] = new Fraction<int>(2, 1, integerDomain);

            return result;
        }

        /// <summary>
        /// Verifica a validade de todas as linhas com excepção da linha especificada.
        /// </summary>
        /// <param name="line">A linha a ser ignorada.</param>
        /// <param name="matrix">A matriz a ser verificada.</param>
        private void AssertLinesExcept(int line, SparseDictionaryMatrix<Fraction<int>> matrix)
        {
            var lineDimension = matrix.GetLength(0);
            var columnDimension = matrix.GetLength(1);
            var assertMatrix = this.GetDefaultMatrix(matrix.DefaultValue);
            for (int i = 0; i < line; ++i)
            {
                for (int j = 0; j < columnDimension; ++j)
                {
                    Assert.AreEqual(assertMatrix[i, j], matrix[i, j]);
                }
            }

            for (int i = line + 1; i < lineDimension; ++i)
            {
                for (int j = 0; j < columnDimension; ++j)
                {
                    Assert.AreEqual(assertMatrix[i, j], matrix[i, j]);
                }
            }
        }

        /// <summary>
        /// Obtém um vector com todos os elementos de uma linha, incluindo os valores por defeito.
        /// </summary>
        /// <typeparam name="T">O tipo de objectos que constituem as entradas da matriz.</typeparam>
        /// <param name="line">A linha.</param>
        /// <param name="matrix">A matriz.</param>
        /// <returns>O vector com as entradas correspondentes à linha da matriz especificada.</returns>
        private T[] GetLineAsArray<T>(int line, SparseDictionaryMatrix<T> matrix)
        {
            var matrixColumnsNumber = matrix.GetLength(1);
            var currentMatrixLine =default(ISparseMatrixLine<T>);
            var result = new T[matrixColumnsNumber];
            if (matrix.TryGetLine(line, out currentMatrixLine))
            {
                for (int i = 0; i < matrixColumnsNumber; ++i)
                {
                    var currentValue = default(T);
                    if (currentMatrixLine.TryGetColumnValue(i, out currentValue))
                    {
                        result[i] = currentMatrixLine[i];
                    }
                    else
                    {
                        result[i] = matrix.DefaultValue;
                    }
                }
            }
            else
            {
                for (int i = 0; i < result.Length; ++i)
                {
                    result[i] = matrix.DefaultValue;
                }
            }

            return result;
        }

        /// <summary>
        /// Efectua a combinação de vectores, colocando o resultado no primeiro.
        /// </summary>
        /// <typeparam name="T">O tipo dos objectos que constituem as entradas dos vectores.</typeparam>
        /// <param name="replacementArray">O vector a ser substituído.</param>
        /// <param name="combinationArray">O vector a ser combinado.</param>
        /// <param name="firstFactor">O primeiro factor.</param>
        /// <param name="secondFactor">O segundo factor.</param>
        /// <param name="ring">O objecto responsáel pelas operações sobre as entradas dos vectores.</param>
        private void CombineArrays<T>(
            T[] replacementArray,
            T[] combinationArray,
            T firstFactor,
            T secondFactor,
            IRing<T> ring)
        {
            var length = replacementArray.Length;
            for (int i = 0; i < length; ++i)
            {
                var firstValue = ring.Multiply(firstFactor, replacementArray[i]);
                var secondValue = ring.Multiply(secondFactor, combinationArray[i]);
                replacementArray[i] = ring.Add(firstValue, secondValue);
            }
        }

        /// <summary>
        /// Verifica a validade da combinação de uma linha com a próxima tendo em conta os factores proporcionados.
        /// </summary>
        /// <typeparam name="T">O tipo de objectos que constituem as entradas das matrizes.</typeparam>
        /// <param name="target">A matriz.</param>
        /// <param name="firstFactor">O factor afecto à linha a ser substituída.</param>
        /// <param name="secondFactor">O factor afecto à linha a ser combinada.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        private void AssertTarget<T>(
            SparseDictionaryMatrix<T> target,
            T firstFactor,
            T secondFactor,
            IRing<T> ring)
        {
            var numberOfLines = target.GetLength(0);
            var numberOfColumns = target.GetLength(1);

            var firstLine = 0;
            var secondLine = 1;
            while (secondLine < numberOfLines)
            {
                var firstArray = this.GetLineAsArray(firstLine, target);
                var secondArray = this.GetLineAsArray(secondLine, target);
                this.CombineArrays(firstArray, secondArray, firstFactor, secondFactor, ring);

                target.CombineLines(firstLine, secondLine, firstFactor, secondFactor, ring);

                var targetLine = default(ISparseMatrixLine<T>);
                if (target.TryGetLine(firstLine, out targetLine))
                {
                    for (int i = 0; i < numberOfColumns; ++i)
                    {
                        var targetValue = default(T);
                        if (!targetLine.TryGetColumnValue(i, out targetValue))
                        {
                            targetValue = target.DefaultValue;
                        }

                        Assert.AreEqual(firstArray[i], targetValue);
                    }
                }
                else
                {
                    for (int i = 0; i < numberOfColumns; ++i)
                    {
                        Assert.AreEqual(firstArray[i], target.DefaultValue);
                    }
                }

                ++firstLine;
                ++secondLine;
            }
        }

        #endregion Métodos auxiliares
    }
}
