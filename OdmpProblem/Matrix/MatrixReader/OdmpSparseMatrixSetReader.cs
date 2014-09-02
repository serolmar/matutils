namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities;
    using System.Globalization;

    public class OdmpSparseMatrixSetReader<ComponentType, LineType, ColumnType, T>
    {
        private string[] voidTypes = new string[] { "space", "carriage_return", "new_line" };

        private Dictionary<string, List<string>> elementsDelimiterTypes = new Dictionary<string, List<string>>();

        private List<IState<string, string>> states = new List<IState<string, string>>();

        private ComponentType componentCoord;

        private LineType lineCoord;

        private ColumnType columnCoord;

        private int coordState;

        private OdmpMatrixSet<ComponentType, LineType, ColumnType, T> matrixSet;

        private IParse<T, string, string> objectElementsReader;

        private IParse<ComponentType, string, string> componentElementsReader;

        private IParse<LineType, string, string> lineElementsReader;

        private IParse<ColumnType, string, string> columnElementsReader;

        private T defaultValue;

        private IEqualityComparer<ComponentType> componentComparer;

        private IEqualityComparer<LineType> lineComparer;

        private IEqualityComparer<ColumnType> columnComparer;

        // Mantém o valor lido quando é encontrado um delimitador.
        private List<ISymbol<string, string>> currentReadingValues = new List<ISymbol<string, string>>();

        public OdmpSparseMatrixSetReader(
            IParse<T, string, string> objectElementsReader,
            IParse<ComponentType, string, string> componentElementsReader,
            IParse<LineType, string, string> lineElementsReader,
            IParse<ColumnType, string, string> columnElementsReader)
        {
            if (objectElementsReader == null)
            {
                throw new OdmpProblemException("An object parser must be provided.");
            }
            else if (componentElementsReader == null)
            {
                throw new OdmpProblemException("A component parser must be provided.");
            }
            else if (lineElementsReader == null)
            {
                throw new OdmpProblemException("A line parser must be provided.");
            }
            else if (columnElementsReader == null)
            {
                throw new OdmpProblemException("A column parser must be provided.");
            }
            else
            {
                this.matrixSet = new OdmpMatrixSet<ComponentType, LineType, ColumnType, T>(this.componentComparer);
                this.objectElementsReader = objectElementsReader;
                this.componentElementsReader = componentElementsReader;
                this.lineElementsReader = lineElementsReader;
                this.columnElementsReader = columnElementsReader;

                this.SetStates();
            }
        }

        public IOdmpMatrixSet<ComponentType, LineType, ColumnType, T> Read(Stream stream,
            T defaultValue = default(T),
            IEqualityComparer<ComponentType> componentComparer = null,
            IEqualityComparer<LineType> lineComparer = null,
            IEqualityComparer<ColumnType> columnComparer = null)
        {
            this.defaultValue = defaultValue;
            this.componentComparer = componentComparer;
            this.lineComparer = lineComparer;
            this.columnComparer = columnComparer;
            this.coordState = 0;

            var reader = new StreamReader(stream);
            var symbolReader = new StringSymbolReader(reader, true);
            this.matrixSet.Clear();

            var stateMachine = new StateMachine<string, string>(
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
                throw new OdmpProblemException("Open delimiter type can't be null.");
            }
            else if (string.IsNullOrWhiteSpace(closeDelimiterType))
            {
                throw new OdmpProblemException("Close delimiter type can't be null.");
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
            this.states.Add(new DelegateStateImplementation<string, string>(0, "Start", this.StartState));
            this.states.Add(new DelegateStateImplementation<string, string>(1, "End", this.EndState));
            this.states.Add(new DelegateStateImplementation<string, string>(2, "Inside Brackets", this.InsideBracketsState));
            this.states.Add(new DelegateStateImplementation<string, string>(3, "Inside Parenthesis", this.InsideParenthesisState));
            this.states.Add(new DelegateStateImplementation<string, string>(4, "Value", this.ValueState));
        }

        /// <summary>
        /// A transição inicial - estado 0.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> StartState(ISymbolReader<string, string> reader)
        {
            this.IgnoreVoids(reader);
            var readed = reader.Get();
            if (reader.IsAtEOF())
            {
                throw new OdmpProblemException("Unexpected end of file.");
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
        private IState<string, string> EndState(ISymbolReader<string, string> reader)
        {
            return null;
        }

        /// <summary>
        /// Estado de leitura da colecção de matrizes - estado 2.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> InsideBracketsState(ISymbolReader<string, string> reader)
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
                throw new OdmpProblemException(string.Format("Unexpected symbol {0}.", readed.SymbolValue));
            }
        }

        /// <summary>
        /// Estado de leitura das coordenadas - estado 3.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> InsideParenthesisState(ISymbolReader<string, string> reader)
        {
            this.IgnoreVoids(reader);
            var readed = reader.Get();
            if (reader.IsAtEOF())
            {
                throw new OdmpProblemException("Unexpected end of file.");
            }

            this.currentReadingValues.Clear();
            if (readed.SymbolType == "right_parenthesis")
            {
                if (this.coordState != 3)
                {
                    throw new OdmpProblemException(string.Format(
                        "Wrong number, {0}, of coordinates. Expecting 3.",
                        this.coordState));
                }

                this.coordState = 0;
                return this.states[4];
            }
            else if (readed.SymbolType == "left_parenthesis")
            {
                this.ProcessInnerParenthesis(readed, reader);
            }
            else if (this.elementsDelimiterTypes.ContainsKey(readed.SymbolType))
            {
                this.ProcessDelimiteres(readed, reader);
            }
            else
            {
                this.currentReadingValues.Add(readed);
            }

            switch (this.coordState)
            {
                case 0:
                    if (!this.componentElementsReader.TryParse(this.currentReadingValues.ToArray(), out this.componentCoord))
                    {
                        throw new OdmpProblemException(string.Format("Can't parse component coordinate: {0}.", readed.SymbolValue));
                    }

                    break;
                case 1:
                    if (!this.lineElementsReader.TryParse(this.currentReadingValues.ToArray(), out this.lineCoord))
                    {
                        throw new OdmpProblemException(string.Format("Can't parse line coordinate: {0}.", readed.SymbolValue));
                    }

                    break;
                case 2:
                    if (!this.columnElementsReader.TryParse(this.currentReadingValues.ToArray(), out this.columnCoord))
                    {
                        throw new OdmpProblemException(string.Format("Can't parse column coordinate: {0}.", readed.SymbolValue));
                    }

                    break;
                default:
                    throw new OdmpProblemException("An internal error has occured.");
            }

            ++this.coordState;
            return this.states[3];
        }

        /// <summary>
        /// Estado da leitura do valor - estado 4.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> ValueState(ISymbolReader<string, string> reader)
        {
            this.IgnoreVoids(reader);
            var readed = reader.Get();
            if (reader.IsAtEOF())
            {
                throw new OdmpProblemException("Unexpected end of file.");
            }

            this.currentReadingValues.Clear();
            if (this.elementsDelimiterTypes.ContainsKey(readed.SymbolType))
            {
                this.ProcessDelimiteres(readed, reader);
            }
            else if (readed.SymbolType == "left_parenthesis")
            {
                this.ProcessInnerParenthesis(readed, reader);
            }
            else
            {
                this.currentReadingValues.Add(readed);
            }

            var value = default(T);
            if (!this.objectElementsReader.TryParse(this.currentReadingValues.ToArray(), out value))
            {
                throw new OdmpProblemException(string.Format("Can't parse value {0}.", readed.SymbolValue));
            }

            this.SetValueInMatrixSet(this.componentCoord, this.lineCoord, this.columnCoord, value);
            this.coordState = 0;

            return this.states[2];
        }

        private void IgnoreVoids(ISymbolReader<string, string> reader)
        {
            var readed = reader.Peek();
            while (readed.SymbolType == "blancks" || readed.SymbolType == "carriage_return" || readed.SymbolType == "new_line")
            {
                reader.Get();
                readed = reader.Peek();
            }
        }

        private void SetValueInMatrixSet(ComponentType component, LineType line, ColumnType column, T value)
        {
            IOdmpMatrix<ComponentType, LineType, ColumnType, T> matrix = null;
            if (!this.matrixSet.Components.TryGetValue(component, out matrix))
            {
                var innerMatrix = new OdmpSparseDictionaryMatrix<ComponentType, LineType, ColumnType, T>(component, this.defaultValue);
                innerMatrix[line, column] = value;
                this.matrixSet.Components.Add(component, innerMatrix);
            }
            else
            {
                var innerMatrix = matrix as OdmpSparseDictionaryMatrix<ComponentType, LineType, ColumnType, T>;
                innerMatrix[line, column] = value;
            }
        }

        private void ProcessDelimiteres(ISymbol<string, string> readed, ISymbolReader<string, string> reader)
        {
            var closeDelimiters = this.elementsDelimiterTypes[readed.SymbolType];
            this.currentReadingValues.Add(readed);
            do
            {
                if (reader.IsAtEOF())
                {
                    throw new OdmpProblemException("Matriz set is in a wrong format.");
                }
                else
                {
                    readed = reader.Get();
                    this.currentReadingValues.Add(readed);
                }
            } while (!closeDelimiters.Contains(readed.SymbolType));
        }

        private void ProcessInnerParenthesis(ISymbol<string, string> readed, ISymbolReader<string, string> reader)
        {
            var parenthesisStatus = 1;
            this.currentReadingValues.Add(readed);

            do
            {
                if (reader.IsAtEOF())
                {
                    throw new OdmpProblemException("Matriz set is in a wrong format.");
                }
                else
                {
                    readed = reader.Get();
                    if (this.elementsDelimiterTypes.ContainsKey(readed.SymbolType))
                    {
                        this.ProcessDelimiteres(readed, reader);
                    }
                    else if (readed.SymbolType == "left_parenthesis")
                    {
                        ++parenthesisStatus;
                    }
                    else if (readed.SymbolType == "right_parenthesis")
                    {
                        --parenthesisStatus;
                    }

                    this.currentReadingValues.Add(readed);
                }
            } while (parenthesisStatus > 0);
        }
    }
}
