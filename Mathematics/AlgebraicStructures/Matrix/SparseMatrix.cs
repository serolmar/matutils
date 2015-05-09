namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;
    using Utilities.Collections;

    /// <summary>
    /// Representação em termos de coordenadas de uma matriz esparsa.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem as entradas da matriz.</typeparam>
    public class CoordinateSparseMatrix<CoeffType> : ISparseMatrix<CoeffType>
    {
        /// <summary>
        /// Mantém o valor por defeito.
        /// </summary>
        private CoeffType defaultValue;

        /// <summary>
        /// Mantém a lista dos elementos.
        /// </summary>
        private SortedDictionary<Tuple<int, int>, MutableTuple<CoeffType>> elements;

        /// <summary>
        /// O comparador que permite averiguara igualdade com o coeficiente por defeito.
        /// </summary>
        private IEqualityComparer<CoeffType> comparer;

        /// <summary>
        /// Mantém o número de linhas.
        /// </summary>
        private int numberOfLines;

        /// <summary>
        /// Mantém o número de colunas.
        /// </summary>
        private int numberOfColumns;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordinateSparseMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="defaultValue">O valor por defeito a ser assumido pela matriz.</param>
        public CoordinateSparseMatrix(CoeffType defaultValue)
        {
            this.defaultValue = default(CoeffType);
            this.comparer = EqualityComparer<CoeffType>.Default;
            this.elements = new SortedDictionary<Tuple<int, int>, MutableTuple<CoeffType>>(
                Comparer<Tuple<int, int>>.Default);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordinateSparseMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="lines">O número de linha contidas na matriz.</param>
        /// <param name="columns">O número de colunas contidas na matriz.</param>
        public CoordinateSparseMatrix(int lines, int columns)
            : this(default(CoeffType))
        {
            if (lines < 0)
            {
                throw new ArgumentOutOfRangeException("lines");
            }
            else if (columns < 0)
            {
                throw new ArgumentOutOfRangeException("columns");
            }
            else
            {
                this.numberOfColumns = lines;
                this.numberOfColumns = columns;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordinateSparseMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="lines">O número de linha contidas na matriz.</param>
        /// <param name="columns">O número de colunas contidas na matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public CoordinateSparseMatrix(int lines, int columns, CoeffType defaultValue)
            : this(defaultValue)
        {
            if (lines < 0)
            {
                throw new ArgumentOutOfRangeException("lines");
            }
            else if (columns < 0)
            {
                throw new ArgumentOutOfRangeException("columns");
            }
            else
            {
                this.numberOfColumns = lines;
                this.numberOfColumns = columns;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordinateSparseMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="lines">O número de linha contidas na matriz.</param>
        /// <param name="columns">O número de colunas contidas na matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <param name="comparer">O comparador que permite identificar os valores por defeito inseridos.</param>
        public CoordinateSparseMatrix(
            int lines,
            int columns,
            CoeffType defaultValue,
            IEqualityComparer<CoeffType> comparer)
            : this(lines, columns, defaultValue)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.comparer = comparer;
            }
        }

        /// <summary>
        /// Obtém a linha pelo respectivo valor.
        /// </summary>
        /// <param name="line">O índice.</param>
        /// <returns>A linha caso exista.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se a linha especificada for negativa ou não for inferior ao número de linhas.
        /// </exception>
        /// <exception cref="MathematicsException">Se a linha não existir.</exception>
        public ISparseMatrixLine<CoeffType> this[int line]
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número de linhas ou o número de colunas for negativo ou não for inferior ao tamanho
        /// da dimensão respectiva.
        /// </exception>
        public CoeffType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException("The parameter line is out of bounds.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException("The parameter column is out of bounds.");
                }
                else
                {
                    var value = default(MutableTuple<CoeffType>);
                    var key = Tuple.Create(line, column);
                    if (this.elements.TryGetValue(key, out value))
                    {
                        return value.Item1;
                    }
                    else
                    {
                        return this.defaultValue;
                    }
                }
            }
            set
            {
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException("The parameter line is out of bounds.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException("The parameter column is out of bounds.");
                }
                else
                {
                    if (this.comparer.Equals(value, this.defaultValue))
                    {
                        var key = Tuple.Create(line, column);
                        this.elements.Remove(key);
                    }
                    else
                    {
                        var current = default(MutableTuple<CoeffType>);
                        var key = Tuple.Create(line, column);
                        if (this.elements.TryGetValue(key, out  current))
                        {
                            current.Item1 = value;
                        }
                        else
                        {
                            this.elements.Add(key, MutableTuple.Create(value));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o valor por defeito que está associado à matriz.
        /// </summary>
        public CoeffType DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }

        /// <summary>
        /// Obtém o número de linhas da matriz.
        /// </summary>
        public int NumberOfLines
        {
            get
            {
                return this.numberOfLines;
            }
        }

        public IEnumerable<KeyValuePair<int, ISparseMatrixLine<CoeffType>>> GetLines()
        {
            throw new NotImplementedException();
        }

        public void Remove(int lineNumber)
        {
            throw new NotImplementedException();
        }

        public bool ContainsLine(int line)
        {
            throw new NotImplementedException();
        }

        public bool TryGetLine(int index, out ISparseMatrixLine<CoeffType> line)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<int, CoeffType>> GetColumns(int line)
        {
            throw new NotImplementedException();
        }

        public int GetLength(int dimension)
        {
            throw new NotImplementedException();
        }

        public IMatrix<CoeffType> GetSubMatrix(int[] lines, int[] columns)
        {
            throw new NotImplementedException();
        }

        public IMatrix<CoeffType> GetSubMatrix(
            IntegerSequence lines,
            IntegerSequence columns)
        {
            throw new NotImplementedException();
        }

        public void SwapLines(int i, int j)
        {
            throw new NotImplementedException();
        }

        public void SwapColumns(int i, int j)
        {
            throw new NotImplementedException();
        }

        public void ScalarLineMultiplication(int line, CoeffType scalar, IRing<CoeffType> ring)
        {
            throw new NotImplementedException();
        }

        public void CombineLines(int i, int j, CoeffType a, CoeffType b, IRing<CoeffType> ring)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém um enumerador para todos os valores não nulos da matriz.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<CoeffType> GetEnumerator()
        {
            foreach (var kvp in this.elements)
            {
                yield return kvp.Value.Item1;
            }
        }

        /// <summary>
        /// Obtém um enumerador não genérico para todos os valores não nulos da matriz.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
