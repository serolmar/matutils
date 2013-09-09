using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    internal class SparseDictionaryMatrixLine<ObjectType> : ISparseMatrixLine<ObjectType>
    {
        /// <summary>
        /// A matriz da qual a linha faz parte.
        /// </summary>
        private SparseDictionaryMatrix<ObjectType> owner;

        /// <summary>
        /// O valor que sucede a entrada com o maior número da linha.
        /// </summary>
        private int afterLastColumnNumber;

        /// <summary>
        /// As entradas.
        /// </summary>
        private Dictionary<int, ObjectType> matrixEntries = new Dictionary<int, ObjectType>();

        public SparseDictionaryMatrixLine(SparseDictionaryMatrix<ObjectType> owner)
        {
            this.owner = owner;
        }

        public ObjectType this[int index]
        {
            get
            {
                if (index < 0 || index >= this.afterLastColumnNumber)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                else
                {
                    var value = default(ObjectType);
                    if (this.matrixEntries.TryGetValue(index, out value))
                    {
                        return value;
                    }
                    else
                    {
                        return this.owner.DefaultValue;
                    }
                }
            }
            set
            {
                if (index < 0 || index >= this.afterLastColumnNumber)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                else
                {
                    if (this.matrixEntries.ContainsKey(index))
                    {
                        this.matrixEntries[index] = value;
                    }
                    else
                    {
                        this.matrixEntries.Add(index, value);
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o número de entradas não nulas.
        /// </summary>
        public int NumberOfColumns
        {
            get
            {
                return this.matrixEntries.Count;
            }
        }

        /// <summary>
        /// Obtém o número que sucede o da última coluna.
        /// </summary>
        public int AfterLastColumnNumber
        {
            get
            {
                return this.afterLastColumnNumber;
            }
            internal set
            {
                this.afterLastColumnNumber = value;
            }
        }

        internal Dictionary<int, ObjectType> MatrixEntries
        {
            get
            {
                return this.matrixEntries;
            }
        }

        public IEnumerable<KeyValuePair<int, ObjectType>> GetColumns()
        {
            return this.matrixEntries;
        }

        public void Remove(int columnIndex)
        {
            this.matrixEntries.Remove(columnIndex);
        }

        /// <summary>
        /// Verifica se a linha esparsa contém a coluna especificada.
        /// </summary>
        /// <param name="column">A coluna.</param>
        /// <returns>Verdadeiro caso a linha contenha a coluna e falso caso contrário.</returns>
        public bool ContainsColumn(int column)
        {
            return this.matrixEntries.ContainsKey(column);
        }

        public IEnumerator<KeyValuePair<int,ObjectType>> GetEnumerator()
        {
            return this.matrixEntries.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
