namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa o gerador de um sub-espaço vectorial.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de valores do espaço.</typeparam>
    public class VectorSpaceGenerator<CoeffType> : IList<IVector<CoeffType>>
    {
        /// <summary>
        /// Os vectores que constituem a base do espaço vectorial.
        /// </summary>
        private List<IVector<CoeffType>> basisVectors;

        /// <summary>
        /// O número de entradas dos vectores permitidos.
        /// </summary>
        private int vectorSpaceDimension;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="VectorSpaceGenerator{CoeffType}"/>.
        /// </summary>
        /// <param name="vectorSpaceDimension">O número de entradas dos vectores permitidos.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número de entradas dos vectores permitidos for negativo.
        /// </exception>
        public VectorSpaceGenerator(int vectorSpaceDimension)
        {
            if (vectorSpaceDimension < 0)
            {
                throw new ArgumentOutOfRangeException("vectorSpaceDimension");
            }
            else
            {
                this.basisVectors = new List<IVector<CoeffType>>();
                this.vectorSpaceDimension = vectorSpaceDimension;
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="VectorSpaceGenerator{CoeffType}"/>.
        /// </summary>
        /// <param name="vectorSpaceDimension">O número de entradas dos vectores permitidos.</param>
        /// <param name="basisVectors">Uma lista inicial de vectores da base.</param>
        private VectorSpaceGenerator(
            int vectorSpaceDimension, 
            List<IVector<CoeffType>> basisVectors)
        {
            this.vectorSpaceDimension = vectorSpaceDimension;
            this.basisVectors = basisVectors;
        }

        #region Funções da Lista

        /// <summary>
        /// Obtém e atribui os vectores da base na posição especificada pelo índice.
        /// </summary>
        /// <value>O vector que se encontra na posição especificada.</value>
        /// <param name="index">O índice.</param>
        /// <returns>O vector que se encontra na posição especificada.</returns>
        /// <exception cref="MathematicsException">
        /// Se o espaço corrente for só de leitura ou as dimensões do vector não coincidirem com a dimensão
        /// estabelecida para o espaço vectorial corrente.
        /// </exception>
        public IVector<CoeffType> this[int index]
        {
            get
            {
                return this.basisVectors[index];
            }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new MathematicsException("The basis vector is readonly.");
                }
                else if (value.Length == this.vectorSpaceDimension)
                {
                    this.basisVectors[index] = value;
                }
                else
                {
                    throw new MathematicsException("The provided vector's dimension doesn't match the main space dimension.");
                }
            }
        }

        /// <summary>
        /// Otbém o número de vectores na base.
        /// </summary>
        /// <value>
        /// O número de vectores na base.
        /// </value>
        public int Count
        {
            get
            {
                return this.basisVectors.Count;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a base é apenas de leitura.
        /// </summary>
        /// <value>
        /// O valor que indica se a base é apenas de leitura.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Procura a base pelo vector e retorna o índice da posição da sua primeira ocorrência.
        /// </summary>
        /// <param name="item">O vector a ser procudrado.</param>
        /// <returns>O índice da posição da primeira ocorrência do vector.</returns>
        public int IndexOf(IVector<CoeffType> item)
        {
            return this.basisVectors.IndexOf(item);
        }

        /// <summary>
        /// Procura a base pelo vector e retorna o índice da posição da sua primeira ocorrência.
        /// </summary>
        /// <param name="item">O vector a ser procurado.</param>
        /// <param name="index">O índice de partida.</param>
        /// <returns>O índice da primeira ocorrência do vector.</returns>
        public int IndexOf(IVector<CoeffType> item, int index)
        {
            return this.basisVectors.IndexOf(item, index);
        }

        /// <summary>
        /// Procura a base pelo vector e retorna o índice da posiçáo da sua primeira ocorrência.
        /// </summary>
        /// <param name="item">O vector a ser procurado.</param>
        /// <param name="index">O índice de partida na pesquisa.</param>
        /// <param name="count">O número de vectores a serem pesquisados.</param>
        /// <returns>O índice da posição da primeira ocorrência.</returns>
        public int IndexOf(IVector<CoeffType> item, int index, int count)
        {
            return this.basisVectors.IndexOf(item, index, count);
        }

        /// <summary>
        /// Procura a base pelo vector e retorna a sua última ocorrência.
        /// </summary>
        /// <param name="item">O vector a ser procurado.</param>
        /// <returns>O índice da posição da sua última ocorrência.</returns>
        public int LastIndexOf(IVector<CoeffType> item)
        {
            return this.basisVectors.LastIndexOf(item);
        }

        /// <summary>
        /// Procura a base pelo vector e retorna o índice da posição da sua última ocorrência.
        /// </summary>
        /// <param name="item">O vector a ser procurado.</param>
        /// <param name="index">O índice de partida.</param>
        /// <returns>O índice da última ocorrência do vector.</returns>
        public int LastIndexOf(IVector<CoeffType> item, int index)
        {
            return this.basisVectors.LastIndexOf(item, index);
        }

        /// <summary>
        /// Procura a base pelo vector e retorna o índice da posiçáo da sua última ocorrência.
        /// </summary>
        /// <param name="item">O vector a ser procurado.</param>
        /// <param name="index">O índice de partida na pesquisa.</param>
        /// <param name="count">O número de vectores a serem pesquisados.</param>
        /// <returns>O índice da posição da última ocorrência.</returns>
        public int LastIndexOf(IVector<CoeffType> item, int index, int count)
        {
            return this.basisVectors.LastIndexOf(item, index, count);
        }

        /// <summary>
        /// Insere um vector na poisção especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O vector.</param>
        /// <exception cref="MathematicsException">
        /// Se o espaço corrente for só de leitura ou a dimensão do vector não coincidor com
        /// a dimensão permitida pelo espaço vectorial corrente.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Se o item for nulo.
        /// </exception>
        public void Insert(int index, IVector<CoeffType> item)
        {
            if (this.IsReadOnly)
            {
                throw new MathematicsException("The basis vector is readonly.");
            }
            else if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            else if (item.Length == this.vectorSpaceDimension)
            {
                this.basisVectors.Insert(index, item);
            }
            else
            {
                throw new MathematicsException("The provided vector's dimension doesn't match the main space dimension.");
            }
        }

        /// <summary>
        /// Insere uma colecção de vectores na posição espeicificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="collection">A colecção.</param>
        /// <exception cref="ArgumentNullException">Se a colecção for nula.</exception>
        /// <exception cref="MathematicsException">
        /// Se o espaço corrente for só de leitura ou a dimensão do vector não coincidor com
        /// a dimensão permitida pelo espaço vectorial corrente.
        /// </exception>
        public void InsertRange(int index, IEnumerable<IVector<CoeffType>> collection)
        {
            if (this.IsReadOnly)
            {
                throw new MathematicsException("The basis vector is readonly.");
            }
            else if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                foreach (var item in collection)
                {
                    if (item.Length != this.vectorSpaceDimension)
                    {
                        throw new MathematicsException("The provided vector's dimension doesn't match the main space dimension.");
                    }
                }

                this.basisVectors.InsertRange(index, collection);
            }
        }

        /// <summary>
        /// Remove o vector que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <exception cref="MathematicsException">Se o espaço corrente for só de leitura.</exception>
        public void RemoveAt(int index)
        {
            if (this.IsReadOnly)
            {
                throw new MathematicsException("The basis vector is readonly.");
            }
            else
            {
                this.basisVectors.RemoveAt(index);
            }
        }

        /// <summary>
        /// Adiciona um vector ao final da base.
        /// </summary>
        /// <param name="item">O vector a ser adicionado.</param>
        /// <exception cref="MathematicsException">
        /// Se o espaço corrente for só de leitura ou a dimensão do vector não coincidor com
        /// a dimensão permitida pelo espaço vectorial corrente.
        /// </exception>
        /// <exception cref="ArgumentNullException">Se o item for nulo.</exception>
        public void Add(IVector<CoeffType> item)
        {
            if (this.IsReadOnly)
            {
                throw new MathematicsException("The basis vector is readonly.");
            }
            else if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            else if (item.Length == this.vectorSpaceDimension)
            {
                this.basisVectors.Add(item);
            }
            else
            {
                throw new MathematicsException("The provided vector's dimension doesn't match the main space dimension.");
            }
        }

        /// <summary>
        /// Adiciona uma colecção de vectores ao final da base.
        /// </summary>
        /// <param name="collection">A colecção de vectores a ser adicionada.</param>
        /// <exception cref="MathematicsException">
        /// Se o espaço corrente for só de leitura ou a dimensão do vector não coincidor com
        /// a dimensão permitida pelo espaço vectorial corrente.
        /// </exception>
        /// <exception cref="ArgumentNullException">Se a colecção for nula.</exception>
        public void AddRange(IEnumerable<IVector<CoeffType>> collection)
        {
            if (this.IsReadOnly)
            {
                throw new MathematicsException("The basis vector is readonly.");
            }
            else if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                foreach (var item in collection)
                {
                    if (item.Length != this.vectorSpaceDimension)
                    {
                        throw new MathematicsException("The provided vector's dimension doesn't match the main space dimension.");
                    }
                }

                this.basisVectors.AddRange(collection);
            }
        }

        /// <summary>
        /// Remove todos os vectores da base.
        /// </summary>
        /// <exception cref="MathematicsException">Se o espaço vectorial corrente for só de leitura.</exception>
        public void Clear()
        {
            if (this.IsReadOnly)
            {
                throw new MathematicsException("The basis vector is readonly.");
            }
            else
            {
                this.basisVectors.Clear();
            }
        }

        /// <summary>
        /// Averigua se um vector se encontra na base.
        /// </summary>
        /// <remarks>
        /// O vector a ser procurado é identificado de acordo com a sua referência.
        /// </remarks>
        /// <param name="item">O vector.</param>
        /// <returns>Verdadeiro caso o vector se encontre na base e falso caso contrário.</returns>
        public bool Contains(IVector<CoeffType> item)
        {
            return this.basisVectors.Contains(item);
        }

        /// <summary>
        /// Verifica se um vector se encontra na base sendo a comparação de identidade realizada por intermédio
        /// de um comparador.
        /// </summary>
        /// <param name="item">O vector.</param>
        /// <param name="comparer">O comparador.</param>
        /// <returns>Verdadeiro caso o vector se encontre  na base e falso caso contrário.</returns>
        public bool Contains(IVector<CoeffType> item, IEqualityComparer<IVector<CoeffType>> comparer)
        {
            return this.basisVectors.Contains(item, comparer);
        }

        /// <summary>
        /// Verifica se um vector se encontra na base sendo a comparação de identidade realizada por intermédio
        /// de um comparador de coeficientes. Dois vectores são iguais ou diferentes consoante contenham os mesmos
        /// coeficientes nas mesmas posições ou não.
        /// </summary>
        /// <param name="item">O vector.</param>
        /// <param name="coeffComparer">O comparador de coeficientes.</param>
        /// <returns>Verdadeoro caso o vector se encontre na base e falso caso contrário.</returns>
        public bool Contains(IVector<CoeffType> item, IEqualityComparer<CoeffType> coeffComparer)
        {
            var innerComparer = coeffComparer;
            if (innerComparer == null)
            {
                innerComparer = EqualityComparer<CoeffType>.Default;
            }

            return this.basisVectors.Any(bv => this.CompareVectors(bv, item, innerComparer));
        }

        /// <summary>
        /// Copia o conteúdo da base para um vector de memória.
        /// </summary>
        /// <param name="array">O vector de memória.</param>
        public void CopyTo(IVector<CoeffType>[] array)
        {
            this.basisVectors.CopyTo(array);
        }

        /// <summary>
        /// Copia o conteúdo da base para um vector de memória, iniciando na posição do vector de memória
        /// especificada pelo índice. 
        /// </summary>
        /// <param name="array">O vector de memória.</param>
        /// <param name="arrayIndex">O índice da posição a iniciar a cópia.</param>
        public void CopyTo(IVector<CoeffType>[] array, int arrayIndex)
        {
            this.basisVectors.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Copia o conteúdo da base para um vector de memória, iniciando na posição da base especificada por um índice.
        /// A cópia é iniciada na posição do vector de memória especificada, sendo compiados um número definido de
        /// vectores.
        /// </summary>
        /// <param name="index">O índice da posição inicial da base.</param>
        /// <param name="array">O vector de memória.</param>
        /// <param name="arrayIndex">O índice inicial da posição no vector de memória.</param>
        /// <param name="count">O número de elementos a serem copiados.</param>
        public void CopyTo(int index, IVector<CoeffType>[] array, int arrayIndex, int count)
        {
            this.basisVectors.CopyTo(index, array, arrayIndex, count);
        }

        /// <summary>
        /// Verifica se algum vector que obedeça aos critérios definidos por um predicado
        /// se encontra na base.
        /// </summary>
        /// <param name="match">O predicado.</param>
        /// <returns>Verdadeiro caso exista algum vector e falso caso contrário.</returns>
        public bool Exists(Predicate<IVector<CoeffType>> match)
        {
            return this.basisVectors.Exists(match);
        }

        /// <summary>
        /// Procura um vector na base que obedeça aos critérios definidos por um predicado.
        /// </summary>
        /// <param name="match">O predicado.</param>
        /// <returns>O vector que obedeça aos critérios.</returns>
        public IVector<CoeffType> Find(Predicate<IVector<CoeffType>> match)
        {
            return this.basisVectors.Find(match);
        }

        /// <summary>
        /// Procura todos os valores que obedeçam aos critérios definidos por um predicado.
        /// </summary>
        /// <param name="match">O predicado.</param>
        /// <returns>A lista com todos os vectores.</returns>
        public List<IVector<CoeffType>> FindAll(Predicate<IVector<CoeffType>> match)
        {
            return this.basisVectors.FindAll(match);
        }

        /// <summary>
        /// Obtém o índice da posição do primeiro vector que obedeça aos critérios definidos por um predicado.
        /// </summary>
        /// <param name="match">O predicado.</param>
        /// <returns>O índice da posição da primeira ocorrência.</returns>
        public int FindIndex(Predicate<IVector<CoeffType>> match)
        {
            return this.basisVectors.FindIndex(match);
        }

        /// <summary>
        /// Obtém o índice da posição do primeiro vector que obedeça aos critérios definidos por um predicado.
        /// </summary>
        /// <param name="startIndex">O ídndice onde é iniciada a procura.</param>
        /// <param name="match">O predicado.</param>
        /// <returns>O índice da posição da primeira ocorrência.</returns>
        public int FindIndex(int startIndex, Predicate<IVector<CoeffType>> match)
        {
            return this.basisVectors.FindIndex(startIndex, match);
        }

        /// <summary>
        /// Obtém o índice da posição do primeiro vector que obedeça aos critérios definidos por um predicado.
        /// </summary>
        /// <param name="startIndex">O ídndice onde é iniciada a procura.</param>
        /// <param name="count">O número de vectores a serem pesquisados.</param>
        /// <param name="match">O predicado.</param>
        /// <returns>O índice da posição da primeira ocorrência.</returns>
        public int FindIndex(int startIndex, int count, Predicate<IVector<CoeffType>> match)
        {
            return this.basisVectors.FindIndex(startIndex, count, match);
        }

        /// <summary>
        /// Obtém o último vector que obedeça aos critérios definidos por um predicado.
        /// </summary>
        /// <param name="match">O predicado.</param>
        /// <returns>O vector.</returns>
        public IVector<CoeffType> FindLast(Predicate<IVector<CoeffType>> match)
        {
            return this.FindLast(match);
        }

        /// <summary>
        /// Obtém o índice da posição do último vector que obedeça aos critérios definidos por um predicado.
        /// </summary>
        /// <param name="match">O predicado.</param>
        /// <returns>O índice da posição da última ocorrência.</returns>
        public int FindLastIndex(Predicate<IVector<CoeffType>> match)
        {
            return this.basisVectors.FindLastIndex(match);
        }

        /// <summary>
        /// Obtém o índice da posição do último vector que obedeça aos critérios definidos por um predicado.
        /// </summary>
        /// <param name="startIndex">O ídndice onde é iniciada a procura.</param>
        /// <param name="match">O predicado.</param>
        /// <returns>O índice da posição da última ocorrência.</returns>
        public int FindLastIndex(int startIndex, Predicate<IVector<CoeffType>> match)
        {
            return this.basisVectors.FindLastIndex(startIndex, match);
        }

        /// <summary>
        /// Obtém o índice da posição do último vector que obedeça aos critérios definidos por um predicado.
        /// </summary>
        /// <param name="startIndex">O ídndice onde é iniciada a procura.</param>
        /// <param name="count">O número de vectores a serem pesquisados.</param>
        /// <param name="match">O predicado.</param>
        /// <returns>O índice da posição da última ocorrência.</returns>
        public int FindLastIndex(int startIndex, int count, Predicate<IVector<CoeffType>> match)
        {
            return this.basisVectors.FindLastIndex(startIndex, count, match);
        }

        /// <summary>
        /// Aplica uma acção a cada um dos vectores da base.
        /// </summary>
        /// <param name="action">A acção.</param>
        public void Foreach(Action<IVector<CoeffType>> action)
        {
            this.basisVectors.ForEach(action);
        }

        /// <summary>
        /// Remove um vector da base.
        /// </summary>
        /// <param name="item">O vector a ser removido.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        /// <exception cref="MathematicsException">Se o espaço vectorial corrente for só de leitura.</exception>
        public bool Remove(IVector<CoeffType> item)
        {
            if (this.IsReadOnly)
            {
                throw new MathematicsException("The basis vector is readonly.");
            }
            else
            {
                return this.basisVectors.Remove(item);
            }
        }

        /// <summary>
        /// Remove todos os vectores que obedeçam aos critérios definidos por um predicado.
        /// </summary>
        /// <param name="match">O predicado.</param>
        /// <returns>O número de vectores removidos.</returns>
        /// <exception cref="MathematicsException">Se o espaço vectorial corrente for só de leitura.</exception>
        public int RemoveAll(Predicate<IVector<CoeffType>> match)
        {
            if (this.IsReadOnly)
            {
                throw new MathematicsException("The basis vector is readonly.");
            }
            else
            {
                return this.basisVectors.RemoveAll(match);
            }
        }

        /// <summary>
        /// Remove um número definido de vectores a partir de uma posição especificada por um índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="count">O número de vectores.</param>
        /// <exception cref="MathematicsException">Se o espaço vectorial corrente for só de leitura.</exception>
        public void RemoveRange(int index, int count)
        {
            if (this.IsReadOnly)
            {
                throw new MathematicsException("The basis vector is readonly.");
            }
            else
            {
                this.basisVectors.RemoveRange(index, count);
            }
        }

        /// <summary>
        /// Verifica se um determinado predicado é válido para todos os vectores da base.
        /// </summary>
        /// <param name="match">O preicado.</param>
        /// <returns>Verdadeiro caso todos os elmentos satisfaçam o predicado e falso caso contrário.</returns>
        public bool TrueForAll(Predicate<IVector<CoeffType>> match)
        {
            return this.basisVectors.TrueForAll(match);
        }

        /// <summary>
        /// Obtém um enumerador para todos os vectores da base.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<IVector<CoeffType>> GetEnumerator()
        {
            return this.basisVectors.GetEnumerator();
        }

        #endregion Funções da Lista

        #region Funções da Base

        /// <summary>
        /// Troca dois vectores do gerador.
        /// </summary>
        /// <param name="first">O índice da posição do primeiro vector.</param>
        /// <param name="second">O índice da posição do segundo vector.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se algum dos índices for negativo ou não for inferior ao número de vectores na base.
        /// </exception>
        public void SwapVectors(int first, int second)
        {
            if (first != second)
            {
                if (first < 0 || first >= this.basisVectors.Count)
                {
                    throw new ArgumentOutOfRangeException("first");
                }
                else if (second < 0 || second >= this.basisVectors.Count)
                {
                    throw new ArgumentOutOfRangeException("second");
                }
                else
                {
                    var swap = this.basisVectors[first];
                    this.basisVectors[first] = this.basisVectors[second];
                    this.basisVectors[second] = swap;
                }
            }
        }

        /// <summary>
        /// Obtém uma base ortogonalizada a partir da base actual.
        /// </summary>
        /// <param name="coefficientsField">O corpo responsável pelas operações sobre os coeficientes.</param>
        /// <param name="vectorSpace">O espaço responsável pela multiplicação de um escalar com um vector.</param>
        /// <param name="scalarProduct">O produto escalar.</param>
        /// <returns>A base ortogonalizada.</returns>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        public VectorSpaceGenerator<CoeffType> GetOrthogonalizedBase(
            IField<CoeffType> coefficientsField,
            IVectorSpace<CoeffType, IVector<CoeffType>> vectorSpace,
            IScalarProductSpace<IVector<CoeffType>, CoeffType> scalarProduct)
        {
            if (coefficientsField == null)
            {
                throw new ArgumentNullException("coefficientsField");
            }
            else if (vectorSpace == null)
            {
                throw new ArgumentNullException("vectorSpace");
            }
            else if (scalarProduct == null)
            {
                throw new ArgumentNullException("scalarProduct");
            }
            else
            {
                var result = new List<IVector<CoeffType>>();

                // Mantém a lista de resultados intermédios
                var intermediaryResults = new List<CoeffType>();
                if (this.basisVectors.Count > 0)
                {
                    var basisCount = this.basisVectors.Count;
                    var i = -1;
                    var control = 0;
                    while (control < basisCount)
                    {
                        var currentVector = this.basisVectors[control];
                        if (!currentVector.IsNull(coefficientsField))
                        {
                            i = control;
                            control = basisCount;
                        }
                        else
                        {
                            ++control;
                        }
                    }

                    if (i != -1)
                    {
                        --basisCount;
                        var currentVector = this.basisVectors[i];
                        result.Add(currentVector);
                        if (i < basisCount)
                        {
                            var denom = scalarProduct.Multiply(currentVector, currentVector);
                            intermediaryResults.Add(denom);
                            ++i;
                            for (; i < basisCount; ++i)
                            {
                                currentVector = this.basisVectors[i];
                                for (int j = 0; j < result.Count; ++j)
                                {
                                    denom = intermediaryResults[j];
                                    if (!coefficientsField.IsAdditiveUnity(denom))
                                    {
                                        var orthoVector = result[j];
                                        var num = scalarProduct.Multiply(currentVector, orthoVector);
                                        if (!coefficientsField.IsAdditiveUnity(num))
                                        {
                                            var scalar = coefficientsField.Multiply(
                                                num,
                                                coefficientsField.MultiplicativeInverse(denom));
                                            scalar = coefficientsField.AdditiveInverse(scalar);
                                            if (!coefficientsField.IsMultiplicativeUnity(scalar))
                                            {
                                                orthoVector = vectorSpace.MultiplyScalar(scalar, orthoVector);
                                                currentVector = vectorSpace.Add(currentVector, orthoVector);
                                            }
                                        }
                                    }
                                }

                                // Já foi calculado o vector ortogonal
                                denom = scalarProduct.Multiply(currentVector, currentVector);
                                if (coefficientsField.IsAdditiveUnity(denom))
                                {
                                    if (!currentVector.IsNull(coefficientsField))
                                    {
                                        intermediaryResults.Add(denom);
                                        result.Add(currentVector);
                                    }
                                }
                                else
                                {
                                    intermediaryResults.Add(denom);
                                    result.Add(currentVector);
                                }
                            }

                            // Na útlima iteração não é necessário calcular o produto escalar
                            currentVector = this.basisVectors[i];
                            for (int j = 0; j < result.Count; ++j)
                            {
                                denom = intermediaryResults[j];
                                if (!coefficientsField.IsAdditiveUnity(denom))
                                {
                                    var orthoVector = result[j];
                                    var num = scalarProduct.Multiply(currentVector, orthoVector);
                                    if (!coefficientsField.IsAdditiveUnity(num))
                                    {
                                        var scalar = coefficientsField.Multiply(
                                            num,
                                            coefficientsField.MultiplicativeInverse(denom));
                                        scalar = coefficientsField.AdditiveInverse(scalar);
                                        if (!coefficientsField.IsMultiplicativeUnity(scalar))
                                        {
                                            orthoVector = vectorSpace.MultiplyScalar(scalar, orthoVector);
                                            currentVector = vectorSpace.Add(currentVector, orthoVector);
                                        }
                                    }
                                }
                            }

                            // Já foi calculado o vector ortogonal
                            if (!currentVector.IsNull(coefficientsField))
                            {
                                result.Add(currentVector);
                            }
                        }
                    }
                }

                return new VectorSpaceGenerator<CoeffType>(
                    this.vectorSpaceDimension,
                    result);
            }
        }

        #endregion Funções da Base

        /// <summary>
        /// Obtém um enumerador não genérico.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Compara dois vectores para determinar se são iguais.
        /// </summary>
        /// <param name="first">O primeiro vector a ser comparado.</param>
        /// <param name="second">O segundo vector a ser comparado.</param>
        /// <param name="comparer">O comparador.</param>
        /// <returns></returns>
        private bool CompareVectors(
            IVector<CoeffType> first, 
            IVector<CoeffType> second,
            IEqualityComparer<CoeffType> comparer)
        {
            for (int i = 0; i < this.vectorSpaceDimension; ++i)
            {
                if (!comparer.Equals(first[i], second[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
