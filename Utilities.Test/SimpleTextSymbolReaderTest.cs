namespace Utitlities.Test
{
    using Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;

    [TestClass()]
    public class SimpleTextSymbolReaderTest
    {
        /// <summary>
        /// O contexto do teste.
        /// </summary>
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
        /// Testa a função quer permite obter os carácteres a partir de um leitor.
        ///</summary>
        [TestMethod()]
        public void Get_Test()
        {
            var testString = "Amanhã. Tempo...\n\n\nNão! Porquê?";
            TextReader reader = new StringReader(testString);
            var endOfFileSymbType = "eof";
            var charSymbolReader = new CharSymbolReader<string>(reader, "desconhecido", "final");
            charSymbolReader.RegisterCharRangeType('a', 'z', "palavra");
            charSymbolReader.RegisterCharRangeType('A', 'Z', "palavra");
            charSymbolReader.RegisterCharType('ã', "palavra");
            charSymbolReader.RegisterCharType('ê', "palavra");
            charSymbolReader.RegisterCharType('.', "pontuação");
            charSymbolReader.RegisterCharType('!', "pontuação");
            charSymbolReader.RegisterCharType('?', "pontuação");
            charSymbolReader.RegisterCharType('\n', "brancos");
            charSymbolReader.RegisterCharType(' ', "brancos");

            var target = new SimpleTextSymbolReader<string>(
                charSymbolReader, 
                endOfFileSymbType,
                StringComparer.InvariantCultureIgnoreCase);

            // Todos os símbolos que se espera encontrar na expressão de acordo com a configuração.
            var expectedSymbols = new GeneralSymbol<string, string>[]{
                new GeneralSymbol<string,string>(){ SymbolValue = "Amanhã", SymbolType = "palavra"},
                new GeneralSymbol<string,string>(){ SymbolValue = ".", SymbolType = "pontuação"},
                new GeneralSymbol<string,string>(){ SymbolValue = " ", SymbolType = "brancos"},
                new GeneralSymbol<string,string>(){ SymbolValue = "Tempo", SymbolType = "palavra"},
                new GeneralSymbol<string,string>(){ SymbolValue = "...", SymbolType = "pontuação"},
                new GeneralSymbol<string,string>(){ SymbolValue = "\n\n\n", SymbolType = "brancos"},
                new GeneralSymbol<string,string>(){ SymbolValue = "Não", SymbolType = "palavra"},
                new GeneralSymbol<string,string>(){ SymbolValue = "!", SymbolType = "pontuação"},
                new GeneralSymbol<string,string>(){ SymbolValue = " ", SymbolType = "brancos"},
                new GeneralSymbol<string,string>(){ SymbolValue = "Porquê", SymbolType = "palavra"},
                new GeneralSymbol<string,string>(){ SymbolValue = "?", SymbolType = "pontuação"}
            };

            for (int i = 0; i < expectedSymbols.Length; ++i)
            {
                var expected = expectedSymbols[i];
                var actual = target.Get();
                Assert.AreEqual(expected.SymbolValue, actual.SymbolValue);
                Assert.AreEqual(expected.SymbolType, actual.SymbolType);
            }

            // Verifica se não existem mais símbolos a serem lidos.
            Assert.IsTrue(target.IsAtEOF());
        }
    }
}
