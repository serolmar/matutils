namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma linha da matriz esparsa baseada em dicionários.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas das matrizes.</typeparam>
    internal class SparseDictionaryMatrixLine<ObjectType> : ISparseMatrixLine<ObjectType>
    {
        /// <summary>
        /// A matriz da qual a linha faz parte.
        /// </summary>
        private SparseDictionaryMatrix<ObjectType> owner;

        /// <summary>
        /// As entradas.
        /// </summary>
        private Dictionary<int, ObjectType> matrixEntries = new Dictionary<int, ObjectType>();

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="SparseDictionaryMatrixLine{ObjectType}"/>.
        /// </summary>
        /// <param name="owner">A matriz à qual pertence a linha..</param>
        public SparseDictionaryMatrixLine(SparseDictionaryMatrix<ObjectType> owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Obtém ou atribui o valor da coluna especificada pelo índice.
        /// </summary>
        /// <value>
        /// O valor.
        /// </value>
        /// <param name="index">O índice da linha.</param>
        /// <returns>O valor.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o índice for negativo ou não for inferior ao número de colunas na matriz.
        /// </exception>
        public ObjectType this[int index]
        {
            get
            {
                if (index < 0 || index >= this.owner.GetLength(1))
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
                if (index < 0 || index >= this.owner.GetLength(1))
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                else
                {
                    if (!object.ReferenceEquals(this.owner.DefaultValue, value) &&
                        this.owner.DefaultValue == null ||
                        (this.owner.DefaultValue != null &&
                        !this.owner.DefaultValue.Equals(value)))
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
        }

        /// <summary>
        /// Obtém o comprimento total da linha que iguala a dimensão da matriz.
        /// </summary>
        /// <value>
        /// O comrpimento total da linha que iguala a dimensão da matriz.
        /// </value>
        public int Length
        {
            get
            {
                return this.owner.GetLength(1);
            }
        }

        /// <summary>
        /// Obtém o número de entradas não nulas.
        /// </summary>
        /// <value>
        /// O número de entradas não nulas.
        /// </value>
        public int NumberOfColumns
        {
            get
            {
                return this.matrixEntries.Count;
            }
        }

        /// <summary>
        /// Obtém as entradas das matrizes.
        /// </summary>
        /// <value>
        /// As entradas das matrizes.
        /// </value>
        internal Dictionary<int, ObjectType> MatrixEntries
        {
            get
            {
                return this.matrixEntries;
            }
        }

        /// <summary>
        /// Obtém as colunas.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<int, ObjectType>> GetColumns()
        {
            return this.matrixEntries;
        }

        /// <summary>
        /// Remove uma entrada da linha.
        /// </summary>
        /// <param name="columnIndex">O índice da coluna a remover.</param>
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

        /// <summary>
        /// Tenta obter o valor da coluna caso esta exista na linha da matriz esparsa.
        /// </summary>
        /// <param name="column">O índice da coluna.</param>
        /// <param name="value">O valor na coluna.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryGetColumnValue(int column, out ObjectType value)
        {
            value = default(ObjectType);
            if (column < 0 || column >= this.owner.GetLength(1))
            {
                return false;
            }
            else
            {
                return this.matrixEntries.TryGetValue(column, out value);
            }
        }

        /// <summary>
        /// Obtém um enumerador para todas as entradas da linha da matriz.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<KeyValuePair<int,ObjectType>> GetEnumerator()
        {
            return this.matrixEntries.GetEnumerator();
        }

        /// <summary>
        /// Obtém um enumerador não genérico para as entradas da linha da matriz.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
