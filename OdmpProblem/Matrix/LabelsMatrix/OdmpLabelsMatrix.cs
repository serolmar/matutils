namespace OdmpProblem
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa a matriz que se obtém por intermédio do cálculo do custo de cobertura
    /// recorrendo às referências existentes.
    /// </summary>
    public class OdmpLabelsMatrix : IOdmpMatrix<int, int, int, double>
    {
        /// <summary>
        /// O conjunto de referências.
        /// </summary>
        private List<Label> labels;

        /// <summary>
        /// A componente associada à matriz.
        /// </summary>
        private int component;

        public OdmpLabelsMatrix(List<Label> labels, int component)
        {
            if (labels == null)
            {
                throw new ArgumentNullException("labels");
            }
            else
            {
                this.labels = new List<Label>();
                this.labels.AddRange(labels);
                this.labels.Sort(new LabelsComparer());
                this.component = component;
            }
        }

        public IOdmpMatrixRow<int, int, double> this[int line]
        {
            get
            {
                if (line < 0 || line >= this.labels.Count)
                {
                    throw new IndexOutOfRangeException("The value of line must be between zero and the number of references.");
                }
                else
                {
                    return new OdmpLabelsMatrixRow(line, this.labels);
                }
            }
        }

        public double this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.labels.Count)
                {
                    throw new IndexOutOfRangeException("The value of line must be between zero and the number of references.");
                }
                else if (column < 0 || column >= this.labels.Count)
                {
                    throw new IndexOutOfRangeException("The value of column must be between zero and the number of references.");
                }
                else
                {
                    if (column < line)
                    {
                        return double.NaN;
                    }
                    else if (column == line)
                    {
                        var currentLabel = this.labels[line];
                        return currentLabel.Price * currentLabel.CarsNumber;
                    }
                    else
                    {
                        var lineLabel = this.labels[line];
                        var columnLabel = this.labels[column];
                        if (this.Covers(lineLabel, columnLabel))
                        {
                            return (lineLabel.Price - columnLabel.Price) * columnLabel.CarsNumber;
                        }
                        else
                        {
                            return double.NaN;
                        }
                    }
                }
            }
        }

        public int Count
        {
            get
            {
                return this.labels.Count;
            }
        }

        public int Component
        {
            get
            {
                return this.component;
            }
        }

        public bool ContainsLine(int line)
        {
            return line >= 0 && line < this.labels.Count;
        }

        public bool ContainsColumn(int line, int column)
        {
            if (line < 0 || line >= this.labels.Count)
            {
                return false;
            }
            else if (column < 0 || column >= this.labels.Count)
            {
                return false;
            }
            else
            {
                var coveringLabel = this.labels[line];
                var coveredLabel = this.labels[column];
                return this.Covers(coveringLabel, coveredLabel);
            }
        }

        public IEnumerator<IOdmpMatrixRow<int, int, double>> GetEnumerator()
        {
            throw new NotImplementedException();
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
