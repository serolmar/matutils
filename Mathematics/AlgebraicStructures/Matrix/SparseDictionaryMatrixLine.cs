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
        /// Objecto responsável pela sincronização dos fluxos.
        /// </summary>
        private object lockObject = new object();

        /// <summary>
        /// A matriz da qual a linha faz parte.
        /// </summary>
        private SparseDictionaryMatrix<ObjectType> owner;

        /// <summary>
        /// As entradas.
        /// </summary>
        private SortedDictionary<int, ObjectType> matrixEntries;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="SparseDictionaryMatrixLine{ObjectType}"/>.
        /// </summary>
        /// <param name="owner">A matriz à qual pertence a linha..</param>
        public SparseDictionaryMatrixLine(SparseDictionaryMatrix<ObjectType> owner)
        {
            this.owner = owner;
            this.matrixEntries = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
        }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="SparseDictionaryMatrixLine{ObjectType}"/>.
        /// </summary>
        /// <remarks>
        /// Nenhuma verificação é realizada no que concerne à integridade das entradas proporcionadas no argumento
        /// face ao número de colunas declarado na matriz original.
        /// </remarks>
        /// <param name="matrixEntries">As entradas da matriz.</param>
        /// <param name="owner">A matriz à qual a linha actual pertence.</param>
        internal SparseDictionaryMatrixLine(
            SortedDictionary<int, ObjectType> matrixEntries,
            SparseDictionaryMatrix<ObjectType> owner)
        {
            this.owner = owner;
            this.matrixEntries = matrixEntries;
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
        /// <exception cref="MathematicsException">Se a linha foi descartada.</exception>
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
                    lock (this.lockObject)
                    {
                        if (this.owner == null)
                        {
                            throw new MathematicsException("The current line was disposed.");
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
                        lock (this.lockObject)
                        {
                            if (this.owner == null)
                            {
                                throw new MathematicsException("The line was disposed.");
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
                }
            }
        }

        /// <summary>
        /// Obtém o comprimento total da linha que iguala a dimensão da matriz.
        /// </summary>
        /// <value>
        /// O comrpimento total da linha que iguala a dimensão da matriz.
        /// </value>
        /// <exception cref="MathematicsException">Se a linha foi descartada.</exception>
        public int Length
        {
            get
            {
                lock (this.lockObject)
                {
                    if (this.owner == null)
                    {
                        throw new MathematicsException("The current line was disposed.");
                    }
                    else
                    {
                        return this.owner.GetLength(1);
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o número de entradas não nulas.
        /// </summary>
        /// <value>
        /// O número de entradas não nulas.
        /// </value>
        /// <exception cref="MathematicsException">Se a linha foi descartada.</exception>
        public int NumberOfColumns
        {
            get
            {
                lock (this.lockObject)
                {
                    if (this.owner == null)
                    {
                        throw new MathematicsException("The current line was disposed.");
                    }
                    else
                    {
                        return this.matrixEntries.Count;
                    }
                }

            }
        }

        /// <summary>
        /// Obtém as entradas das matrizes.
        /// </summary>
        /// <remarks>
        /// As entradas da matriz estão ordenadas por número de coluna.
        /// </remarks>
        /// <value>
        /// As entradas das matrizes.
        /// </value>
        /// <exception cref="MathematicsException">Se a linha foi descartada.</exception>
        internal SortedDictionary<int, ObjectType> MatrixEntries
        {
            get
            {
                lock (this.lockObject)
                {
                    if (this.owner == null)
                    {
                        throw new MathematicsException("The current line was disposed.");
                    }
                    else
                    {
                        return this.matrixEntries;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém as colunas.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MathematicsException">Se a linha foi descartada.</exception>
        public IEnumerable<KeyValuePair<int, ObjectType>> GetColumns()
        {
            lock (this.lockObject)
            {
                if (this.owner == null)
                {
                    throw new MathematicsException("The current line was disposed.");
                }
                else
                {
                    return this.matrixEntries;
                }
            }
        }

        /// <summary>
        /// Remove uma entrada da linha.
        /// </summary>
        /// <param name="columnIndex">O índice da coluna a remover.</param>
        /// <exception cref="MathematicsException">Se a linha foi descartada.</exception>
        public void Remove(int columnIndex)
        {
            lock (this.lockObject)
            {
                if (this.owner == null)
                {
                    throw new MathematicsException("The current line was disposed.");
                }
                else
                {
                    this.matrixEntries.Remove(columnIndex);
                }
            }
        }

        /// <summary>
        /// Verifica se a linha esparsa contém a coluna especificada.
        /// </summary>
        /// <param name="column">A coluna.</param>
        /// <returns>Verdadeiro caso a linha contenha a coluna e falso caso contrário.</returns>
        /// <exception cref="MathematicsException">Se a linha foi descartada.</exception>
        public bool ContainsColumn(int column)
        {
            lock (this.lockObject)
            {
                if (this.owner == null)
                {
                    throw new MathematicsException("The current line was disposed.");
                }
                else
                {
                    return this.matrixEntries.ContainsKey(column);
                }
            }
        }

        /// <summary>
        /// Tenta obter o valor da coluna caso esta exista na linha da matriz esparsa.
        /// </summary>
        /// <param name="column">O índice da coluna.</param>
        /// <param name="value">O valor na coluna.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        /// <exception cref="MathematicsException">Se a linha foi descartada.</exception>
        public bool TryGetColumnValue(int column, out ObjectType value)
        {
            lock (this.lockObject)
            {
                if (this.owner == null)
                {
                    throw new MathematicsException("The current line was disposed.");
                }
                else
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
            }
        }

        /// <summary>
        /// Obtém um enumerador para todas as entradas da linha da matriz.
        /// </summary>
        /// <returns>O enumerador.</returns>
        /// <exception cref="MathematicsException">Se a linha foi descartada.</exception>
        public IEnumerator<KeyValuePair<int, ObjectType>> GetEnumerator()
        {
            lock (this.lockObject)
            {
                if (this.owner == null)
                {
                    throw new MathematicsException("The current line was disposed.");
                }
                else
                {
                    return this.matrixEntries.GetEnumerator();
                }
            }
        }

        /// <summary>
        /// Descarta a linha.
        /// </summary>
        public void Dispose()
        {
            this.owner = null;
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
