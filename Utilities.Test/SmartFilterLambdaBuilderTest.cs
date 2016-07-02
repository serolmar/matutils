namespace Utilities.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Utilities;

    /// <summary>
    /// Summary description for SmartFilterLambdaBuilderTest
    /// </summary>
    [TestClass]
    public class SmartFilterLambdaBuilderTest
    {
        public SmartFilterLambdaBuilderTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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
        /// Testa o construtor de expressões lambda com texto vazio como padrão.
        /// </summary>
        [Description("Tests the smart filter lambda builder with an empty pattern")]
        [TestMethod]
        public void SmartFilterLambdaBuilder_EmptyPattern()
        {
            var target = new SmartFilterLambdaBuilder<LambdaTestClass>(
                "or",
                "and",
                "not");
            var lambda = target.BuildExpressionTree(string.Empty);

            // Verifica o resultado da compilação da expressão.
            var compiled = lambda.Compile();
            var obj = new LambdaTestClass();
            Assert.IsTrue(compiled.Invoke(obj));
            this.AssertParameter(lambda);

            var body = lambda.Body;
            Assert.AreEqual(ExpressionType.Constant, body.NodeType);
            var constant = (bool)((ConstantExpression)body).Value;
            Assert.IsTrue(constant);
        }

        /// <summary>
        /// Verifica se o parâmetro é correcto.
        /// </summary>
        /// <param name="lambda">A expressão a ser verificada.</param>
        private void AssertParameter(LambdaExpression lambda)
        {
            Assert.AreEqual(1, lambda.Parameters.Count);
            var parameter = lambda.Parameters[0];
            Assert.IsTrue(parameter.Type.IsAssignableFrom(typeof(LambdaTestClass)));
        }

        /// <summary>
        /// Tipo de objectos que são passados por argumento na expressão lambda.
        /// </summary>
        private class LambdaTestClass
        {
            #region Campos

            /// <summary>
            /// Um valor byte.
            /// </summary>
            private byte byteVal;

            /// <summary>
            /// Um valor sbyte.
            /// </summary>
            private sbyte sbyteVal;

            /// <summary>
            /// Um valor inteiro.
            /// </summary>
            private int intVal;

            /// <summary>
            /// Um valor inteiro sem sinal.
            /// </summary>
            private uint uintVal;

            /// <summary>
            /// Um valor inteiro de pequena precisão.
            /// </summary>
            private short shortVal;

            /// <summary>
            /// Um valor inteiro de pequena precisão sem sinal.
            /// </summary>
            private ushort ushortVal;

            /// <summary>
            /// Um valor longo.
            /// </summary>
            private long longVal;

            /// <summary>
            /// Um valor longo sem sinal.
            /// </summary>
            private ulong ulongVal;

            /// <summary>
            /// Um valor de vírgula flutuante de precisão simples.
            /// </summary>
            private float floatVal;

            /// <summary>
            /// Um valor de vírgula flutuante de precisão arbitrária.
            /// </summary>
            private double doubleVal;

            /// <summary>
            /// Um caráctere.
            /// </summary>
            private char charVal;

            /// <summary>
            /// Um valor lógico.
            /// </summary>
            private bool boolVal;

            /// <summary>
            /// Um valor textual.
            /// </summary>
            private string stringVal;

            /// <summary>
            /// Um valor de vírgula flutuante de grande precisão.
            /// </summary>
            private decimal decimalVal;

            /// <summary>
            /// Um objecto interno.
            /// </summary>
            private LambdaTestClass lambdaTestClassVal;

            /// <summary>
            /// Uma ordenação de bytes.
            /// </summary>
            private byte[] byteArray;

            /// <summary>
            /// Uma ordenação de sbytes.
            /// </summary>
            private sbyte[] sbyteArray;

            /// <summary>
            /// Uma ordenação de inteiros.
            /// </summary>
            private int[] intArray;

            /// <summary>
            /// Uma ordenação de inteiros sem sinal.
            /// </summary>
            private uint[] uintArray;

            /// <summary>
            /// Uma ordenação de inteiros de pequena precisão.
            /// </summary>
            private short[] shortArray;

            /// <summary>
            /// Uma ordenação de inteiros de pequena precisão sem sinal.
            /// </summary>
            private ushort[] ushortArray;

            /// <summary>
            /// Uma ordenação de longos.
            /// </summary>
            private long[] longArray;

            /// <summary>
            /// Uma ordenação de longos sem sinal.
            /// </summary>
            private ulong[] ulongArray;

            /// <summary>
            /// Uma ordenação de valores de vírgula flutuante de precisão simples.
            /// </summary>
            private float[] floatArray;

            /// <summary>
            /// Uma ordenação de valores de vírgula flutuante de precisão arbitrária.
            /// </summary>
            private double[] doubleArray;

            /// <summary>
            /// Uma ordenação de carácteres.
            /// </summary>
            private char[] charArray;

            /// <summary>
            /// Uma ordenação de valores lógicos.
            /// </summary>
            private bool[] boolArray;

            /// <summary>
            /// Uma ordenação de valores textuais.
            /// </summary>
            private string[] stringArray;

            /// <summary>
            /// Uma ordenação de valores de vírgula flutuante de grande precisão.
            /// </summary>
            private decimal[] decimalArray;

            #endregion Campos

            #region Propriedades

            /// <summary>
            /// Um valor byte.
            /// </summary>
            public byte ByteVal
            {
                get
                {
                    return this.byteVal ;
                }
                set
                {
                    this.byteVal = value;
                }
            }

            /// <summary>
            /// Um valor sbyte.
            /// </summary>
            public sbyte SbyteVal
            {
                get
                {
                    return this.sbyteVal ;
                }
                set
                {
                    this.sbyteVal = value;
                }
            }

            /// <summary>
            /// Um valor inteiro.
            /// </summary>
            public int IntVal
            {
                get
                {
                    return this.intVal ;
                }
                set
                {
                    this.intVal = value;
                }
            }

            /// <summary>
            /// Um valor inteiro sem sinal.
            /// </summary>
            public uint UintVal
            {
                get
                {
                    return this.uintVal ;
                }
                set
                {
                    this.uintVal = value;
                }
            }

            /// <summary>
            /// Um valor inteiro de pequena precisão.
            /// </summary>
            public short ShortVal
            {
                get
                {
                    return this.shortVal ;
                }
                set
                {
                    this.shortVal  = value;
                }
            }

            /// <summary>
            /// Um valor inteiro de pequena precisão sem sinal.
            /// </summary>
            public ushort UshortVal
            {
                get
                {
                    return this.ushortVal ;
                }
                set
                {
                    this.ushortVal = value;
                }
            }

            /// <summary>
            /// Um valor longo.
            /// </summary>
            public long LongVal
            {
                get
                {
                    return this.longVal ;
                }
                set
                {
                    this.longVal = value;
                }
            }

            /// <summary>
            /// Um valor longo sem sinal.
            /// </summary>
            public ulong UlongVal
            {
                get
                {
                    return this.ulongVal ;
                }
                set
                {
                    this.ulongVal = value;
                }
            }

            /// <summary>
            /// Um valor de vírgula flutuante de precisão simples.
            /// </summary>
            public float FloatVal
            {
                get
                {
                    return this.floatVal ;
                }
                set
                {
                    this.floatVal = value;
                }
            }

            /// <summary>
            /// Um valor de vírgula flutuante de precisão arbitrária.
            /// </summary>
            public double DoubleVal
            {
                get
                {
                    return this.doubleVal ;
                }
                set
                {
                    this.doubleVal = value;
                }
            }

            /// <summary>
            /// Um caráctere.
            /// </summary>
            public char CharVal
            {
                get
                {
                    return this.charVal ;
                }
                set
                {
                    this.charVal = value;
                }
            }

            /// <summary>
            /// Um valor lógico.
            /// </summary>
            public bool BoolVal
            {
                get
                {
                    return this.boolVal ;
                }
                set
                {
                    this.boolVal = value;
                }
            }

            /// <summary>
            /// Um valor textual.
            /// </summary>
            public string StringVal
            {
                get
                {
                    return this.stringVal ;
                }
                set
                {
                    this.stringVal = value;
                }
            }

            /// <summary>
            /// Um valor de vírgula flutuante de grande precisão.
            /// </summary>
            public decimal DecimalVal
            {
                get
                {
                    return this.decimalVal ;
                }
                set
                {
                    this.decimalVal = value;
                }
            }

            /// <summary>
            /// Um objecto interno.
            /// </summary>
            public LambdaTestClass LambdaTestClassVal
            {
                get
                {
                    return this.lambdaTestClassVal ;
                }
                set
                {
                    this.lambdaTestClassVal = value;
                }
            }

            /// <summary>
            /// Uma ordenação de bytes.
            /// </summary>
            public byte[] ByteArray
            {
                get
                {
                    return this.byteArray ;
                }
                set
                {
                    this.byteArray = value;
                }
            }

            /// <summary>
            /// Uma ordenação de sbytes.
            /// </summary>
            public sbyte[] SbyteArray
            {
                get
                {
                    return this.sbyteArray ;
                }
                set
                {
                    this.sbyteArray = value;
                }
            }

            /// <summary>
            /// Uma ordenação de inteiros.
            /// </summary>
            public int[] IntArray
            {
                get
                {
                    return this.intArray ;
                }
                set
                {
                    this.intArray = value;
                }
            }

            /// <summary>
            /// Uma ordenação de inteiros sem sinal.
            /// </summary>
            public uint[] UintArray
            {
                get
                {
                    return this.uintArray ;
                }
                set
                {
                    this.uintArray = value;
                }
            }

            /// <summary>
            /// Uma ordenação de inteiros de pequena precisão.
            /// </summary>
            public short[] ShortArray
            {
                get
                {
                    return this.shortArray ;
                }
                set
                {
                    this.shortArray = value;
                }
            }

            /// <summary>
            /// Uma ordenação de inteiros de pequena precisão sem sinal.
            /// </summary>
            public ushort[] UshortArray
            {
                get
                {
                    return this.ushortArray ;
                }
                set
                {
                    this.ushortArray = value;
                }
            }

            /// <summary>
            /// Uma ordenação de longos.
            /// </summary>
            public long[] LongArray
            {
                get
                {
                    return this.longArray ;
                }
                set
                {
                    this.longArray = value;
                }
            }

            /// <summary>
            /// Uma ordenação de longos sem sinal.
            /// </summary>
            public ulong[] UlongArray
            {
                get
                {
                    return this.ulongArray ;
                }
                set
                {
                    this.ulongArray = value;
                }
            }

            /// <summary>
            /// Uma ordenação de valores de vírgula flutuante de precisão simples.
            /// </summary>
            public float[] FloatArray
            {
                get
                {
                    return this.floatArray ;
                }
                set
                {
                    this.floatArray = value;
                }
            }

            /// <summary>
            /// Uma ordenação de valores de vírgula flutuante de precisão arbitrária.
            /// </summary>
            public double[] DoubleArray
            {
                get
                {
                    return this.doubleArray ;
                }
                set
                {
                    this.doubleArray = value;
                }
            }

            /// <summary>
            /// Uma ordenação de carácteres.
            /// </summary>
            public char[] CharArray
            {
                get
                {
                    return this.charArray ;
                }
                set
                {
                    this.charArray = value;
                }
            }

            /// <summary>
            /// Uma ordenação de valores lógicos.
            /// </summary>
            public bool[] BoolArray
            {
                get
                {
                    return this.boolArray ;
                }
                set
                {
                    this.boolArray = value;
                }
            }

            /// <summary>
            /// Uma ordenação de valores textuais.
            /// </summary>
            public string[] StringArray
            {
                get
                {
                    return this.stringArray ;
                }
                set
                {
                    this.stringArray = value;
                }
            }

            /// <summary>
            /// Uma ordenação de valores de vírgula flutuante de grande precisão.
            /// </summary>
            public decimal[] DecimalArray
            {
                get
                {
                    return this.decimalArray ;
                }
                set
                {
                    this.decimalArray = value;
                }
            }

            #endregion Propriedades
        }
    }
}
