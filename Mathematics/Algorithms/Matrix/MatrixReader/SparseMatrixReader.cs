using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utilities.Parsers;
using System.Globalization;

namespace Mathematics
{
    public class SparseMatrixReader
    {
        private string[] voidTypes = new string[] { "space", "carriage_return", "new_line" };
        private List<IState<CharSymbolReader, string, string>> states = new List<IState<CharSymbolReader, string, string>>();
        private List<int> currentCoords = new List<int>();
        private MatrixSet<double> matrixSet;
        private NumberFormatInfo numberFormat;

        public SparseMatrixReader()
        {
            this.matrixSet = new MatrixSet<double>();
            this.numberFormat = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;
        }

        public ImatrixSet<int, int, int, int, double> Read(Stream stream)
        {
            var reader = new StreamReader(stream);
            var symbolReader = new StringSymbolReader(reader, true);
            this.currentCoords.Clear();
            this.matrixSet.Clear();

            var stateMachine = new StateMachine<CharSymbolReader, string, string>(
                this.states[0],
                this.states[1]);

            stateMachine.RunMachine(symbolReader);
            return this.matrixSet;
        }

        private void SetStates()
        {
            this.states.Add(new DelegateStateImplementation<CharSymbolReader, string, string>(0, "Start", this.StartState));
            this.states.Add(new DelegateStateImplementation<CharSymbolReader, string, string>(1, "End", this.EndState));
            this.states.Add(new DelegateStateImplementation<CharSymbolReader, string, string>(2, "Inside Brackets", this.InsideBracketsState));
            this.states.Add(new DelegateStateImplementation<CharSymbolReader, string, string>(3, "Inside Parenthesis", this.InsideParenthesisState));
            this.states.Add(new DelegateStateImplementation<CharSymbolReader, string, string>(4, "Value", this.ValueState));
        }

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

        private IState<CharSymbolReader, string, string> EndState(SymbolReader<CharSymbolReader, string, string> reader)
        {
            return null;
        }

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
                if (this.currentCoords.Count != 3)
                {
                    throw new MathematicsException(string.Format(
                        "Wrong number, {0}, of coordinates. Expecting 3.",
                        this.currentCoords.Count));
                }

                return this.states[4];
            }
            else if (readed.SymbolType == "integer")
            {
                var coord = 0;
                if (!int.TryParse(readed.SymbolValue, out coord))
                {
                    throw new MathematicsException(string.Format("Can't parse cood {0} to integer.", readed.SymbolValue));
                }

                this.currentCoords.Add(coord);
                return this.states[3];
            }
            else
            {
                throw new MathematicsException(string.Format("Unexpected symbol {0}.", readed.SymbolValue));
            }
        }

        private IState<CharSymbolReader, string, string> ValueState(SymbolReader<CharSymbolReader, string, string> reader)
        {
            this.IgnoreVoids(reader);
            var readed = reader.Get();
            if (reader.IsAtEOF())
            {
                throw new MathematicsException("Unexpected end of file.");
            }

            if (readed.SymbolType.Contains("double") || readed.SymbolType == "integer")
            {
                var value = 0.0;
                if (!double.TryParse(readed.SymbolValue, NumberStyles.Float, this.numberFormat, out value))
                {
                    throw new MathematicsException(string.Format("Can't parse value {0} to double.", readed.SymbolValue));
                }

                this.SetValueInMatrixSet(this.currentCoords[0], this.currentCoords[1], this.currentCoords[2], value);
                this.currentCoords.Clear();
                return this.states[2];
            }
            else
            {
                throw new MathematicsException(string.Format("Unexpected symbol {0}.", readed.SymbolValue));
            }
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

        private void SetValueInMatrixSet(int component, int line, int column, double value)
        {
            if (line < 0 || column < 0)
            {
                throw new MathematicsException("Negative lines or columns aren't allowed.");
            }

            IMatrix<int, int, int, double> matrix = null;
            if (!this.matrixSet.Components.TryGetValue(component, out matrix))
            {
                var innerMatrix = new SparseDictionaryMatrix<double>(double.MaxValue, component);
                innerMatrix[line, column] = value;
            }
            else
            {
                var innerMatrix = matrix as SparseDictionaryMatrix<double>;
                innerMatrix[line, column] = value;
            }
        }
    }
}
