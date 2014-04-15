namespace OdmpProblem
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class OdmpLabelsMatrixRow : IOdmpMatrixRow<int, int, double>
    {
        /// <summary>
        /// O conjunto de referências.
        /// </summary>
        private List<Label> labels;

        /// <summary>
        /// O índices da referência de cobertura.
        /// </summary>
        private int line;

        public OdmpLabelsMatrixRow(int line, List<Label> labels)
        {
            this.line = line;
            this.labels = labels;
        }

        public IOdmpMatrixColumn<int, double> this[int columnIndex]
        {
            get
            {
                if (columnIndex < 0 || columnIndex >= this.labels.Count)
                {
                    throw new IndexOutOfRangeException("The column index must be greater than zero and less than the number of references.");
                }
                else if (columnIndex < this.line)
                {
                    return new OdmpLabelsMatrixColumn(columnIndex, double.NaN);
                }
                else
                {
                    var currentLineLabel = this.labels[line];
                    if (columnIndex == line)
                    {
                        var value = currentLineLabel.Price * currentLineLabel.CarsNumber;
                        return new OdmpLabelsMatrixColumn(columnIndex, value);
                    }
                    else
                    {
                        var otherLabel = this.labels[columnIndex];
                        if (this.Covers(currentLineLabel, otherLabel))
                        {
                            var value = (currentLineLabel.Price - otherLabel.Price) * otherLabel.CarsNumber;
                            return new OdmpLabelsMatrixColumn(columnIndex, value);
                        }
                        else
                        {
                            return new OdmpLabelsMatrixColumn(columnIndex, double.NaN);
                        }
                    }
                }
            }
        }

        public int Line
        {
            get
            {
                return this.line;
            }
        }

        public int Count
        {
            get
            {
                return this.labels.Count;
            }
        }

        public bool ContainsColumn(int index)
        {
            if (index < 0 || index >= this.labels.Count)
            {
                return false;
            }
            else
            {
                var currentLineLabel = this.labels[this.line];
                var currentColumnLabel = this.labels[index];
                return this.Covers(currentLineLabel, currentColumnLabel);
            }
        }

        public IEnumerator<IOdmpMatrixColumn<int, double>> GetEnumerator()
        {
            var currentLineLabel = this.labels[this.line];

            yield return new OdmpLabelsMatrixColumn(this.line, currentLineLabel.Price * currentLineLabel.CarsNumber);

            for (int i = this.line + 1; i < this.labels.Count; ++i)
            {
                var otherLineLable = this.labels[i];
                if (this.Covers(currentLineLabel, otherLineLable))
                {
                    var value = (currentLineLabel.Price - otherLineLable.Price) * otherLineLable.CarsNumber;
                    yield return new OdmpLabelsMatrixColumn(i, value);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Permite averiguar a cobertura de uma referência.
        /// </summary>
        /// <param name="coveringLabel">A referência de cobertura.</param>
        /// <param name="coveredLabel">A referência que poderá ser coberta.</param>
        /// <returns>Verdadeiro caso a referência de cobertura cubra a referência que poderá ser coberta.</returns>
        private bool Covers(Label coveringLabel, Label coveredLabel)
        {
            return (coveringLabel.MBits.BitListOr(coveredLabel.MBits) == coveringLabel.MBits) &&
                    (coveringLabel.MtdBits.BitListOr(coveringLabel.MtdBits) == coveringLabel.MtdBits);
        }
    }
}
