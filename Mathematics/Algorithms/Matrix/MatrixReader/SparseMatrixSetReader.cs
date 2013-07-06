using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utilities.Parsers;
using System.Globalization;

namespace Mathematics
{
    public class SparseMatrixSetReader<Component, Line, Column, T>
    {
        private string[] voidTypes = new string[] { "space", "carriage_return", "new_line" };

        private Dictionary<string, List<string>> elementsDelimiterTypes = new Dictionary<string, List<string>>();

        private List<IState<CharSymbolReader, string, string>> states = new List<IState<CharSymbolReader, string, string>>();

        private Component componentCoord;

        private Line lineCoord;

        private Column columnCoord;

        private int coordState;

        private MatrixSet<Component, Line, Column, T> matrixSet;

        private IParse<T, string, string> objectElementsReader;

        private IParse<Component, string, string> componentElementsReader;

        private IParse<Line, string, string> lineElementsReader;

        private IParse<Column, string, string> columnElementsReader;

        private T defaultValue;

        private IEqualityComparer<Component> componentComparer;

        private IEqualityComparer<Line> lineComparer;

        private IEqualityComparer<Column> columnComparer;

        // Mantém o valor lido quando é encontrado um delimitador.
        private List<ISymbol<string, string>> currentReadingValues = new List<ISymbol<string, string>>();

        public SparseMatrixSetReader(
            IParse<T, string, string> objectElementsReader,
            IParse<Component, string, string> componentElementsReader,
            IParse<Line, string, string> lineElementsReader,
            IParse<Column, string, string> columnElementsReader)
        {
            if (objectElementsReader == null)
            {
                throw new MathematicsException("An object parser must be provided.");
            }
            else if (componentElementsReader == null)
            {
                throw new MathematicsException("A component parser must be provided.");
            }
            else if (lineElementsReader == null)
            {
                throw new MathematicsException("A line parser must be provided.");
            }
            else if (columnElementsReader == null)
            {
                throw new MathematicsException("A column parser must be provided.");
            }
            else
            {
                this.matrixSet = new MatrixSet<Component, Line, Column, T>(this.componentComparer);
                this.objectElementsReader = objectElementsReader;
                this.componentElementsReader = componentElementsReader;
                this.lineElementsReader = lineElementsReader;
                this.columnElementsReader = columnElementsReader;
            }
        }

        public ImatrixSet<Component, Line, Column, T> Read(Stream stream,
            T defaultValue = default(T),
            IEqualityComparer<Component> componentComparer = null,
            IEqualityComparer<Line> lineComparer = null,
            IEqualityComparer<Column> columnComparer = null)
        {
            this.defaultValue = defaultValue;
            this.componentComparer = componentComparer;
            this.lineComparer = lineComparer;
            this.columnComparer = columnComparer;
            this.coordState = 0;

            var reader = new StreamReader(stream);
            var symbolReader = new StringSymbolReader(reader, true);
            this.matrixSet.Clear();

            var stateMachine = new StateMachine<CharSymbolReader, string, string>(
                this.states[0],
                this.states[1]);

            stateMachine.RunMachine(symbolReader);
            return this.matrixSet;
        }


        /// <summary>
        /// Regista delimitadores para os elementos da matriz. Sempre que um delimitador de abertura é encontrado,
        /// são lidos todos os símbolos até que seja encontrado o próximo delimitador. Os delimitadores são incluídos
        /// no valor lido.
        /// </summary>
        /// <param name="openDelimiterType">O tipo do delimtiador de abertura como é conhecido na classe <see cref="StringSymbolReader"/></param>
        /// <param name="closeDelimiterType">O tipo do delimtiador de fecho como é conhecido na classe <see cref="StringSymbolReader"/></param>
        public void RegisterElementDelimiterType(string openDelimiterType, string closeDelimiterType)
        {
            if (string.IsNullOrWhiteSpace(openDelimiterType))
            {
                throw new MathematicsException("Open delimiter type can't be null.");
            }
            else if (string.IsNullOrWhiteSpace(closeDelimiterType))
            {
                throw new MathematicsException("Close delimiter type can't be null.");
            }
            else
            {
                List<string> delimiters = null;
                if (this.elementsDelimiterTypes.TryGetValue(openDelimiterType, out delimiters))
                {
                    if (!delimiters.Contains(closeDelimiterType))
                    {
                        delimiters.Add(closeDelimiterType);
                    }
                }
                else
                {
                    this.elementsDelimiterTypes.Add(openDelimiterType, new List<string>() { closeDelimiterType });
                }
            }
        }

        /// <summary>
        /// Elimina todos os mapeamentos de delimitadores.
        /// </summary>
        public void ClearDelimiterTypes()
        {
            this.elementsDelimiterTypes.Clear();
        }

        private void SetStates()
        {
            this.states.Add(new DelegateStateImplementation<CharSymbolReader, string, string>(0, "Start", this.StartState));
            this.states.Add(new DelegateStateImplementation<CharSymbolReader, string, string>(1, "End", this.EndState));
            this.states.Add(new DelegateStateImplementation<CharSymbolReader, string, string>(2, "Inside Brackets", this.InsideBracketsState));
            this.states.Add(new DelegateStateImplementation<CharSymbolReader, string, string>(3, "Inside Parenthesis", this.InsideParenthesisState));
            this.states.Add(new DelegateStateImplementation<CharSymbolReader, string, string>(4, "Value", this.ValueState));
        }

        /// <summary>
        /// A transição inicial - estado 0.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState<CharSymbolReader, string, string> StartState(SymbolReader<CharSymbolReader, string, string> reader)
        {
            this.IgnoreVoids(reader);
            var readed = reader.Get();
            if (reader.IsAtEOF())
            {
                throw new MathematicsException("Unexpected end of file.");
            }

            if (readed.SymbolType == "left_bracket")
            {
                return this.states[2];
            }

            return this.states[0];
        }

        /// <summary>
        /// Estado final - estado 1.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState<CharSymbolReader, string, string> EndState(SymbolReader<CharSymbolReader, string, string> reader)
        {
            return null;
        }

        /// <summary>
        /// Estado de leitura da colecção de matrizes - estado 2.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState<CharSymbolReader, string, string> InsideBracketsState(SymbolReader<CharSymbolReader, string, string> reader)
        {
            this.IgnoreVoids(reader);
            var readed = reader.Get();
            if (reader.IsAtEOF())
            {
                return this.states[1];
            }

            if (readed.SymbolType == "left_parenthesis")
            {
                return this.states[3];
            }
            else if (readed.SymbolType == "right_bracket")
            {
                return this.states[1];
            }
            else
            {
                throw new MathematicsException(string.Format("Unexpected symbol {0}.", readed.SymbolValue));
            }
        }

        /// <summary>
        /// Estado de leitura das coordenadas - estado 3.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState<CharSymbolReader, string, string> InsideParenthesisState(SymbolReader<CharSymbolReader, string, string> reader)
        {
            this.IgnoreVoids(reader);
            var readed = reader.Get();
            if (reader.IsAtEOF())
            {
                throw new MathematicsException("Unexpected end of file.");
            }

            if (readed.SymbolType == "right_parenthesis")
            {
                if (this.coordState != 3)
                {
                    throw new MathematicsException(string.Format(
                        "Wrong number, {0}, of coordinates. Expecting 3.",
                        this.coordState));
                }

                return this.states[4];
            }
            else if (this.elementsDelimiterTypes.ContainsKey(readed.SymbolType))
            {
                var closeDelimiters = this.elementsDelimiterTypes[readed.SymbolType];
                this.currentReadingValues.Clear();
                this.currentReadingValues.Add(readed);
                readed = reader.Get();
                while (!closeDelimiters.Contains(readed.SymbolType))
                {
                    this.currentReadingValues.Add(readed);
                    if (reader.IsAtEOF())
                    {
                        throw new MathematicsException("Matriz set is in a wrong format.");
                    }
                    else
                    {
                        readed = reader.Get();
                    }
                }

                this.currentReadingValues.Add(readed);
                switch (this.coordState)
                {
                    case 0:
                        if (!this.componentElementsReader.TryParse(this.currentReadingValues.ToArray(), out this.componentCoord))
                        {
                            throw new MathematicsException(string.Format("Can't parse component coordinate: {0}.", readed.SymbolValue));
                        }

                        break;
                    case 1:
                        if (!this.lineElementsReader.TryParse(this.currentReadingValues.ToArray(), out this.lineCoord))
                        {
                            throw new MathematicsException(string.Format("Can't parse line coordinate: {0}.", readed.SymbolValue));
                        }

                        break;
                    case 2:
                        if (!this.columnElementsReader.TryParse(this.currentReadingValues.ToArray(), out this.columnCoord))
                        {
                            throw new MathematicsException(string.Format("Can't parse column coordinate: {0}.", readed.SymbolValue));
                        }

                        break;
                    default:
                        throw new MathematicsException("An internal error has occured.");
                }

                return this.states[3];
            }
            else
            {
                switch (this.coordState)
                {
                    case 0:
                        if (!this.componentElementsReader.TryParse(new[] { readed }, out this.componentCoord))
                        {
                            throw new MathematicsException(string.Format("Can't parse component coordinate: {0}.", readed.SymbolValue));
                        }

                        break;
                    case 1:
                        if (!this.lineElementsReader.TryParse(new[] { readed }, out this.lineCoord))
                        {
                            throw new MathematicsException(string.Format("Can't parse line coordinate: {0}.", readed.SymbolValue));
                        }

                        break;
                    case 2:
                        if (!this.columnElementsReader.TryParse(new[] { readed }, out this.columnCoord))
                        {
                            throw new MathematicsException(string.Format("Can't parse column coordinate: {0}.", readed.SymbolValue));
                        }

                        break;
                    default:
                        throw new MathematicsException("An internal error has occured.");
                }

                return this.states[3];
            }
        }

        /// <summary>
        /// Estado da leitura do valor - estado 4.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState<CharSymbolReader, string, string> ValueState(SymbolReader<CharSymbolReader, string, string> reader)
        {
            this.IgnoreVoids(reader);
            var readed = reader.Get();
            if (reader.IsAtEOF())
            {
                throw new MathematicsException("Unexpected end of file.");
            }

            if (this.elementsDelimiterTypes.ContainsKey(readed.SymbolType))
            {
                var closeDelimiters = this.elementsDelimiterTypes[readed.SymbolType];
                this.currentReadingValues.Clear();
                this.currentReadingValues.Add(readed);
                readed = reader.Get();
                while (!closeDelimiters.Contains(readed.SymbolType))
                {
                    this.currentReadingValues.Add(readed);
                    if (reader.IsAtEOF())
                    {
                        throw new MathematicsException("Matriz set is in a wrong format.");
                    }
                    else
                    {
                        readed = reader.Get();
                    }
                }

                this.currentReadingValues.Add(readed);
                var value = default(T);
                if (!this.objectElementsReader.TryParse(this.currentReadingValues.ToArray(), out value))
                {
                    throw new MathematicsException(string.Format("Can't parse value {0}.", readed.SymbolValue));
                }

                this.SetValueInMatrixSet(this.componentCoord, this.lineCoord, this.columnCoord, value);
                this.coordState = 0;
            }
            else
            {
                var value = default(T);
                if (!this.objectElementsReader.TryParse(new[] { readed }, out value))
                {
                    throw new MathematicsException(string.Format("Can't parse value {0}.", readed.SymbolValue));
                }

                this.SetValueInMatrixSet(this.componentCoord, this.lineCoord, this.columnCoord, value);
                this.coordState = 0;
            }

            return this.states[2];
        }

        private void IgnoreVoids(SymbolReader<CharSymbolReader, string, string> reader)
        {
            var readed = reader.Peek();
            while (readed.SymbolType == "blancks" || readed.SymbolType == "carriage_return" || readed.SymbolType == "new_line")
            {
                reader.Get();
                readed = reader.Peek();
            }
        }

        private void SetValueInMatrixSet(Component component, Line line, Column column, T value)
        {
            IMatrix<Line, Column, T> matrix = null;
            if (!this.matrixSet.Components.TryGetValue(component, out matrix))
            {
                var innerMatrix = new SparseDictionaryMatrix<Line, Column, T>(this.defaultValue);
                innerMatrix[line, column] = value;
            }
            else
            {
                var innerMatrix = matrix as SparseDictionaryMatrix<Line, Column, T>;
                innerMatrix[line, column] = value;
            }
        }
    }
}
