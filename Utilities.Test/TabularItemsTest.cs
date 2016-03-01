namespace Utilities.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Utilities;

    /// <summary>
    /// Permite implementar testes aos conjuntos tabulares.
    /// </summary>
    [TestClass]
    public class TabularItemsTest
    {
        /// <summary>
        /// Permite testar a implementação da leitura de CSV.
        /// </summary>
        [Description("Tests the CSV file reader.")]
        [TestMethod]
        public void CsvFileReaderWriter_ReadTest()
        {
            var csvText = "1;2;3\n\"1;2\";\"1;2\n";
            var reader = new StringReader(csvText);
            var target = new CsvFileReaderWriter(
                '\n',
                ';',
                '"',
                (i, j, txt) => txt.Trim(new[] { '"' }));
            var tabularItem = new TabularListsItem();
            target.Read(tabularItem, reader);

            // Número de linhas
            Assert.AreEqual(2, tabularItem.Count);

            // Primeira linha
            var row = tabularItem[0];
            Assert.AreEqual(3, row.Count);
            Assert.AreEqual("1", row[0].GetCellValue<string>());
            Assert.AreEqual("2", row[1].GetCellValue<string>());
            Assert.AreEqual("3", row[2].GetCellValue<string>());

            // Segunda linha
            row = tabularItem[1];
            Assert.AreEqual(2, row.Count);
            Assert.AreEqual("1;2", row[0].GetCellValue<string>());
            Assert.AreEqual("1;2\n", row[1].GetCellValue<string>());

            // Aplicação de validações sem validar dados existentes.
            var validation = new FuncDrivenColumnValudation<object>();
            validation.RegisterValidator(
                0,
                v => { if (v is int) return true; else return false; });
            tabularItem.AddValidation(validation, false);

            // Aplicação de validações com dados existentes
            validation = new FuncDrivenColumnValudation<object>();
            validation.RegisterValidator(
                0,
                v => { return v is string; });
            tabularItem.AddValidation(validation, true);
        }

        /// <summary>
        /// Testa o funcionamento das validações.
        /// </summary>
        [Description("Tests the data validations.")]
        [TestMethod]
        public void TabularItem_ValidationPassTest()
        {
            var csvText = "1;2\n3;4\n5;6";
            var reader = new StringReader(csvText);
            var csvFileReader = new CsvFileReaderWriter(
                '\n',
                ';',
                '"',
                (i, j, txt) => int.Parse(txt.Trim(new[] { '"' })));
            var tabularItem = new TabularListsItem();
            var dataValidation = new FuncDrivenColumnValudation<object>();
            Func<object, bool> validator = o => o is int;
            dataValidation.RegisterValidator(0, validator);
            dataValidation.RegisterValidator(1, validator);
            csvFileReader.Read(tabularItem, reader);
        }

        /// <summary>
        /// Testa a função de validação quando esta é aplicada a uma coluna que não existe.
        /// </summary>
        [Description("Tests the validation when it is applied to an non existing column.")]
        [TestMethod]
        [ExpectedException(typeof(UtilitiesDataException))]
        public void TabularItem_ValidationAfterLastColTestFail()
        {
            var csvText = "1;2\n3;4\n5;6";
            var reader = new StringReader(csvText);
            var csvFileReader = new CsvFileReaderWriter(
                '\n',
                ';',
                '"',
                (i, j, txt) => int.Parse(txt.Trim(new[] { '"' })));
            var tabularItem = new TabularListsItem();
            var dataValidation = new FuncDrivenColumnValudation<object>();
            Func<object, bool> validator = o => o != null;
            dataValidation.RegisterValidator(0, validator);
            dataValidation.RegisterValidator(1, validator);
            dataValidation.RegisterValidator(2, validator);
            tabularItem.AddValidation(dataValidation);
            csvFileReader.Read(tabularItem, reader);
        }

        /// <summary>
        /// Testa a validação da alteração do valor de uma célula.
        /// </summary>
        [Description("Tests the alter cell validation.")]
        [TestMethod]
        [ExpectedException(typeof(UtilitiesDataException))]
        public void TabularItem_ValidationChangeCellValueFailTest()
        {
            var csvText = "1;2\n3;4\n5;6";
            var reader = new StringReader(csvText);
            var csvFileReader = new CsvFileReaderWriter(
                '\n',
                ';',
                '"',
                (i, j, txt) => int.Parse(txt.Trim(new[] { '"' })));
            var tabularItem = new TabularListsItem();
            csvFileReader.Read(tabularItem, reader);
            Func<object, bool> validator = o => o is int;
            var dataValidation = new FuncDrivenColumnValudation<object>();
            dataValidation.RegisterValidator(0, validator);
            dataValidation.RegisterValidator(1, validator);
            tabularItem.AddValidation(dataValidation, false);

            // Tenta alterar a célula para um valor não inteiro
            tabularItem[0][0].SetCellValue<string>("1");
        }

        /// <summary>
        /// Define um validador baseado em operações funcionais.
        /// </summary>
        /// <typeparam name="ObjType"></typeparam>
        private class FuncDrivenColumnValudation<ObjType> : IDataValidation<int, ObjType>
        {
            /// <summary>
            /// As colunas a serem validadas.
            /// </summary>
            private SortedDictionary<int, List<Func<ObjType, bool>>> columnValidation;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="FuncDrivenColumnValudation{ObjType}"/>.
            /// </summary>
            public FuncDrivenColumnValudation()
            {
                this.columnValidation = new SortedDictionary<int, List<Func<ObjType, bool>>>();
            }

            /// <summary>
            /// Regista uma validador para a coluna.
            /// </summary>
            /// <param name="columnIndex">O índice da coluna.</param>
            /// <param name="validator">O validador.</param>
            public void RegisterValidator(
                int columnIndex,
                Func<ObjType, bool> validator)
            {
                if (columnIndex < 0)
                {
                    throw new ArgumentOutOfRangeException("columnIndex");
                }
                else if (validator == null)
                {
                    throw new ArgumentNullException("validator");
                }
                else
                {
                    var columnValidators = default(List<Func<ObjType, bool>>);
                    if (this.columnValidation.TryGetValue(columnIndex, out columnValidators))
                    {
                        columnValidators.Add(validator);
                    }
                    else
                    {
                        this.columnValidation.Add(
                            columnIndex,
                            new List<Func<ObjType, bool>>() { validator });
                    }
                }
            }

            /// <summary>
            /// Elimina todos os validadores.
            /// </summary>
            public void ClearValidators()
            {
                this.columnValidation.Clear();
            }

            /// <summary>
            /// Obtém as colunas a serem validadas.
            /// </summary>
            public IEnumerable<int> Columns
            {
                get
                {
                    return this.columnValidation.Keys;
                }
            }

            /// <summary>
            /// Valida o elemento especificado.
            /// </summary>
            /// <remarks>
            /// O emitente das validações define as colunas que requerem validação. O utente das validações
            /// passa os valores das linhas correspondentes para a função de validação na ordem em que as
            /// colunas são definidas.
            /// </remarks>
            /// <param name="element">O elemento a ser validado.</param>
            /// <returns>Veradeiro caso o elemento seja válido e falso caso contrário.</returns>
            public bool Validate(IEnumerable<ObjType> element)
            {
                var elementEnumerator = element.GetEnumerator();
                var valEnumerator = this.columnValidation.GetEnumerator();
                var state = elementEnumerator.MoveNext() &&
                    valEnumerator.MoveNext();
                while (state)
                {
                    foreach (var validation in valEnumerator.Current.Value)
                    {
                        if (!validation.Invoke(elementEnumerator.Current))
                        {
                            return false;
                        }
                    }

                    state = elementEnumerator.MoveNext() &&
                        valEnumerator.MoveNext();
                }

                return true;
            }
        }
    }
}
