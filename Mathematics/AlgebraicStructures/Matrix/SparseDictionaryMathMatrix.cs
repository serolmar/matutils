namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa uma matriz esparsa com base em dicionários.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem os argumentos.</typeparam>
    public class SparseDictionaryMathMatrix<ObjectType>
        : SparseDictionaryMatrix<ObjectType>, ILongSparseMathMatrix<ObjectType>
    {
        /// <summary>
        /// Cria instâncias e objectos do tipo <see cref="SparseDictionaryMathMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número de linhas ou o número de colunas for negativo.
        /// </exception>
        public SparseDictionaryMathMatrix(int lines, int columns, ObjectType defaultValue)
            : base(lines, columns, defaultValue) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SparseDictionaryMathMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <param name="comparer">O comparador que permite verificar a igualdade com o valor por defeito..</param>
        public SparseDictionaryMathMatrix(
            int lines,
            int columns,
            ObjectType defaultValue,
            IEqualityComparer<ObjectType> comparer)
            : base(lines, columns, defaultValue, comparer) { }

        /// <summary>
        /// Cria instâncias e objectos do tipo <see cref="SparseDictionaryMathMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número de linhas ou o número de colunas for negativo.
        /// </exception>
        public SparseDictionaryMathMatrix(int lines, int columns)
            : base(lines, columns) { }

        /// <summary>
        /// Cria instâncias e objectos do tipo <see cref="SparseDictionaryMathMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="defaultValue">O valor por defeito.</param>
        public SparseDictionaryMathMatrix(ObjectType defaultValue)
            : base(defaultValue) { }

        /// <summary>
        /// Multiplica os valores da linha pelo escalar definido.
        /// </summary>
        /// <param name="line">A linha a ser considerada.</param>
        /// <param name="scalar">O escalar a ser multiplicado.</param>
        /// <param name="ring">
        /// O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.
        /// </param>
        public virtual void ScalarLineMultiplication(
            int line,
            ObjectType scalar,
            IRing<ObjectType> ring)
        {
            if (line < 0 || line >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else if (scalar == null)
            {
                throw new ArgumentNullException("scalar");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                if (this.defaultValue == null)
                {
                    var currentLine = default(ILongSparseMatrixLine<ObjectType>);
                    if (this.TryGetLine(line, out currentLine))
                    {
                        if (ring.IsAdditiveUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                                Comparer<long>.Default);
                            foreach (var kvp in currentLine)
                            {
                                otherMatrixElements.Add(kvp.Key, scalar);
                            }

                            this.SetLine(line, otherMatrixElements);
                        }
                        else if (!ring.IsMultiplicativeUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                                Comparer<long>.Default);
                            foreach (var kvp in currentLine)
                            {
                                var value = kvp.Value;
                                if (ring.IsAdditiveUnity(value))
                                {
                                    otherMatrixElements.Add(kvp.Key, value);
                                }
                                else if (ring.IsMultiplicativeUnity(value))
                                {
                                    otherMatrixElements.Add(kvp.Key, scalar);
                                }
                                else
                                {
                                    otherMatrixElements.Add(
                                        kvp.Key,
                                        ring.Multiply(value, scalar));
                                }
                            }

                            this.SetLine(line, otherMatrixElements);
                        }
                    }
                }
                else if (ring.IsAdditiveUnity(this.defaultValue))
                {
                    var currentLine = default(ILongSparseMatrixLine<ObjectType>);
                    if (this.TryGetLine(line, out currentLine))
                    {
                        if (ring.IsAdditiveUnity(scalar))
                        {
                            this.Remove(line);
                        }
                        else if (!ring.IsMultiplicativeUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                                Comparer<long>.Default);
                            foreach (var kvp in currentLine)
                            {
                                var value = kvp.Value;
                                if (ring.IsMultiplicativeUnity(value))
                                {
                                    otherMatrixElements.Add(kvp.Key, scalar);
                                }
                                else
                                {
                                    otherMatrixElements.Add(
                                        kvp.Key,
                                        ring.Multiply(value, scalar));
                                }
                            }

                            this.SetLine(line, otherMatrixElements);
                        }
                    }
                }
                else
                {
                    var currentLine = default(ILongSparseMatrixLine<ObjectType>);
                    if (this.TryGetLine(line, out currentLine))
                    {
                        if (ring.IsAdditiveUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                                Comparer<long>.Default);
                            for (int i = 0; i < this.afterLastColumn; ++i)
                            {
                                otherMatrixElements.Add(i, scalar);
                            }

                            this.SetLine(line, otherMatrixElements);
                        }
                        else if (!ring.IsMultiplicativeUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                                Comparer<long>.Default);
                            var i = 0;
                            var multiplicationValue = ring.Multiply(this.defaultValue, scalar);
                            foreach (var kvp in currentLine)
                            {
                                var currentColumnValue = kvp.Value;
                                for (; i < kvp.Key; ++i)
                                {
                                    otherMatrixElements.Add(i, multiplicationValue);
                                }

                                var valueToAdd = scalar;
                                if (!ring.IsMultiplicativeUnity(this.defaultValue))
                                {
                                    valueToAdd = ring.Multiply(this.defaultValue, scalar);
                                }

                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(
                                        i,
                                        valueToAdd);
                                }

                                ++i;
                            }

                            for (; i < this.afterLastColumn; ++i)
                            {
                                otherMatrixElements.Add(i, multiplicationValue);
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(line, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(line);
                            }
                        }
                    }
                    else
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                                Comparer<long>.Default);
                        var valueToAdd = ring.Multiply(this.defaultValue, scalar); ;

                        if (!this.defaultValue.Equals(valueToAdd))
                        {
                            for (int i = 0; i < this.afterLastColumn; ++i)
                            {
                                otherMatrixElements.Add(i, valueToAdd);
                            }
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(line, otherMatrixElements);
                        }
                        else
                        {
                            this.Remove(line);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Multiplica os valores da linha pelo escalar definido.
        /// </summary>
        /// <param name="line">A linha a ser considerada.</param>
        /// <param name="scalar">O escalar a ser multiplicado.</param>
        /// <param name="ring">
        /// O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.
        /// </param>
        public virtual void ScalarLineMultiplication(
            long line,
            ObjectType scalar,
            IRing<ObjectType> ring)
        {
            if (line < 0 || line >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else if (scalar == null)
            {
                throw new ArgumentNullException("scalar");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                if (this.defaultValue == null)
                {
                    var currentLine = default(ILongSparseMatrixLine<ObjectType>);
                    if (this.TryGetLine(line, out currentLine))
                    {
                        if (ring.IsAdditiveUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                                Comparer<long>.Default);
                            foreach (var kvp in currentLine)
                            {
                                otherMatrixElements.Add(kvp.Key, scalar);
                            }

                            this.SetLine(line, otherMatrixElements);
                        }
                        else if (!ring.IsMultiplicativeUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                                Comparer<long>.Default);
                            foreach (var kvp in currentLine)
                            {
                                var value = kvp.Value;
                                if (ring.IsAdditiveUnity(value))
                                {
                                    otherMatrixElements.Add(kvp.Key, value);
                                }
                                else if (ring.IsMultiplicativeUnity(value))
                                {
                                    otherMatrixElements.Add(kvp.Key, scalar);
                                }
                                else
                                {
                                    otherMatrixElements.Add(
                                        kvp.Key,
                                        ring.Multiply(value, scalar));
                                }
                            }

                            this.SetLine(line, otherMatrixElements);
                        }
                    }
                }
                else if (ring.IsAdditiveUnity(this.defaultValue))
                {
                    var currentLine = default(ILongSparseMatrixLine<ObjectType>);
                    if (this.TryGetLine(line, out currentLine))
                    {
                        if (ring.IsAdditiveUnity(scalar))
                        {
                            this.Remove(line);
                        }
                        else if (!ring.IsMultiplicativeUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                                Comparer<long>.Default);
                            foreach (var kvp in currentLine)
                            {
                                var value = kvp.Value;
                                if (ring.IsMultiplicativeUnity(value))
                                {
                                    otherMatrixElements.Add(kvp.Key, scalar);
                                }
                                else
                                {
                                    otherMatrixElements.Add(
                                        kvp.Key,
                                        ring.Multiply(value, scalar));
                                }
                            }

                            this.SetLine(line, otherMatrixElements);
                        }
                    }
                }
                else
                {
                    var currentLine = default(ILongSparseMatrixLine<ObjectType>);
                    if (this.TryGetLine(line, out currentLine))
                    {
                        if (ring.IsAdditiveUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                                Comparer<long>.Default);
                            for (int i = 0; i < this.afterLastColumn; ++i)
                            {
                                otherMatrixElements.Add(i, scalar);
                            }

                            this.SetLine(line, otherMatrixElements);
                        }
                        else if (!ring.IsMultiplicativeUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                                Comparer<long>.Default);
                            var i = 0;
                            var multiplicationValue = ring.Multiply(this.defaultValue, scalar);
                            foreach (var kvp in currentLine)
                            {
                                var currentColumnValue = kvp.Value;
                                for (; i < kvp.Key; ++i)
                                {
                                    otherMatrixElements.Add(i, multiplicationValue);
                                }

                                var valueToAdd = scalar;
                                if (!ring.IsMultiplicativeUnity(this.defaultValue))
                                {
                                    valueToAdd = ring.Multiply(this.defaultValue, scalar);
                                }

                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(
                                        i,
                                        valueToAdd);
                                }

                                ++i;
                            }

                            for (; i < this.afterLastColumn; ++i)
                            {
                                otherMatrixElements.Add(i, multiplicationValue);
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(line, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(line);
                            }
                        }
                    }
                    else
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                                Comparer<long>.Default);
                        var valueToAdd = ring.Multiply(this.defaultValue, scalar); ;

                        if (!this.defaultValue.Equals(valueToAdd))
                        {
                            for (int i = 0; i < this.afterLastColumn; ++i)
                            {
                                otherMatrixElements.Add(i, valueToAdd);
                            }
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(line, otherMatrixElements);
                        }
                        else
                        {
                            this.Remove(line);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Substitui a linha especificada por uma combinação linear desta com uma outra. Por exemplo, li = a * li + b * lj, isto é,
        /// a linha i é substituída pela soma do produto de a pela linha i com o produto de b peloa linha j.
        /// </summary>
        /// <param name="i">A linha a ser substituída.</param>
        /// <param name="j">A linha a ser combinada.</param>
        /// <param name="a">O escalar a ser multiplicado pela primeira linha.</param>
        /// <param name="b">O escalar a ser multiplicado pela segunda linha.</param>
        /// <param name="ring">O objecto responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="MathematicsException">
        /// Se ocorrer uma tentativa de combinar algum valor nulo de uma linha com um valor não nulo de outra.
        /// </exception>
        public virtual void CombineLines(
            int i,
            int j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            if (i < 0 || i >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            else if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (this.defaultValue == null)
            {
                this.CombineLinesWithNullValueForDefault(i, j, a, b, ring);
            }
            else if (ring.IsAdditiveUnity(this.defaultValue))
            {
                this.CombineLinesWithAdditiveUnityForDefault(i, j, a, b, ring);
            }
            else
            {
                this.CombineLinesWithSomeValueForDefault(i, j, a, b, ring);
            }
        }

        /// <summary>
        /// Substitui a linha especificada por uma combinação linear desta com uma outra. Por exemplo, li = a * li + b * lj, isto é,
        /// a linha i é substituída pela soma do produto de a pela linha i com o produto de b peloa linha j.
        /// </summary>
        /// <param name="i">A linha a ser substituída.</param>
        /// <param name="j">A linha a ser combinada.</param>
        /// <param name="a">O escalar a ser multiplicado pela primeira linha.</param>
        /// <param name="b">O escalar a ser multiplicado pela segunda linha.</param>
        /// <param name="ring">O objecto responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="MathematicsException">
        /// Se ocorrer uma tentativa de combinar algum valor nulo de uma linha com um valor não nulo de outra.
        /// </exception>
        public virtual void CombineLines(
            long i,
            long j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            if (i < 0 || i >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            else if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (this.defaultValue == null)
            {
                this.CombineLinesWithNullValueForDefault(i, j, a, b, ring);
            }
            else if (ring.IsAdditiveUnity(this.defaultValue))
            {
                this.CombineLinesWithAdditiveUnityForDefault(i, j, a, b, ring);
            }
            else
            {
                this.CombineLinesWithSomeValueForDefault(i, j, a, b, ring);
            }
        }

        #region Private Methods

        /// <summary>
        /// Verifica a integridade das linhas que irão ser adicionadas caso existam valores nulos.
        /// </summary>
        /// <param name="replacementLineNumber">O número da linha a ser substituída.</param>
        /// <param name="replacementLine">A linha a ser substituída.</param>
        /// <param name="combinationLineNumber">O número da linha a ser combinada.</param>
        /// <param name="combinationLine">A linha a ser combinada.</param>
        private void CheckNullDefaultValueLinesIntegrityForCombination(
            int replacementLineNumber,
            ISparseMatrixLine<ObjectType> replacementLine,
            int combinationLineNumber,
            ISparseMatrixLine<ObjectType> combinationLine)
        {
            var replacementLineEnum = replacementLine.GetColumns().GetEnumerator();
            var combinationLineEnum = combinationLine.GetColumns().GetEnumerator();
            var replacementEnumState = replacementLineEnum.MoveNext();
            var combinationState = combinationLineEnum.MoveNext();

            while (replacementEnumState && combinationState)
            {
                if (replacementLineEnum.Current.Key != combinationLineEnum.Current.Key)
                {
                    throw new MathematicsException(string.Format(
                        "Trying to combine a null value from line {0} with a non null value from line {1}.",
                        replacementLineNumber,
                        combinationLineNumber));
                }

                replacementEnumState = replacementLineEnum.MoveNext();
                combinationState = combinationLineEnum.MoveNext();
            }

            if (replacementEnumState)
            {
                throw new MathematicsException(string.Format(
                    "Trying to combine a null value from line {0} with a non null value from line {1}.",
                    replacementLineNumber,
                    combinationLineNumber));
            }
            else if (combinationState)
            {
                throw new MathematicsException(string.Format(
                    "Trying to combine a null value from line {0} with a non null value from line {1}.",
                    replacementLineNumber,
                    combinationLineNumber));
            }

        }

        /// <summary>
        /// Verifica a integridade das linhas que irão ser adicionadas caso existam valores nulos.
        /// </summary>
        /// <param name="replacementLineNumber">O número da linha a ser substituída.</param>
        /// <param name="replacementLine">A linha a ser substituída.</param>
        /// <param name="combinationLineNumber">O número da linha a ser combinada.</param>
        /// <param name="combinationLine">A linha a ser combinada.</param>
        private void CheckNullDefaultValueLinesIntegrityForCombination(
            long replacementLineNumber,
            ISparseMatrixLine<ObjectType> replacementLine,
            long combinationLineNumber,
            ISparseMatrixLine<ObjectType> combinationLine)
        {
            var replacementLineEnum = replacementLine.GetColumns().GetEnumerator();
            var combinationLineEnum = combinationLine.GetColumns().GetEnumerator();
            var replacementEnumState = replacementLineEnum.MoveNext();
            var combinationState = combinationLineEnum.MoveNext();

            while (replacementEnumState && combinationState)
            {
                if (replacementLineEnum.Current.Key != combinationLineEnum.Current.Key)
                {
                    throw new MathematicsException(string.Format(
                        "Trying to combine a null value from line {0} with a non null value from line {1}.",
                        replacementLineNumber,
                        combinationLineNumber));
                }

                replacementEnumState = replacementLineEnum.MoveNext();
                combinationState = combinationLineEnum.MoveNext();
            }

            if (replacementEnumState)
            {
                throw new MathematicsException(string.Format(
                    "Trying to combine a null value from line {0} with a non null value from line {1}.",
                    replacementLineNumber,
                    combinationLineNumber));
            }
            else if (combinationState)
            {
                throw new MathematicsException(string.Format(
                    "Trying to combine a null value from line {0} with a non null value from line {1}.",
                    replacementLineNumber,
                    combinationLineNumber));
            }

        }

        /// <summary>
        /// Efectua a combinação das linhas caso o valor por defeito seja nulo.
        /// </summary>
        /// <param name="i">O número da linha a ser substituída.</param>
        /// <param name="j">O número da linha a ser combinada.</param>
        /// <param name="a">O factor a multiplicar pela primeira linha.</param>
        /// <param name="b">O factor a multiplicar pela segunda linha.</param>
        /// <param name="ring">O anel respons+avel pelas operações sobre as entradas da matriz.</param>
        private void CombineLinesWithNullValueForDefault(
            int i,
            int j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            var replacementLine = default(ILongSparseMatrixLine<ObjectType>);
            if (this.TryGetLine(i, out replacementLine))
            {
                var combinationLine = default(ILongSparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    // Assevera se é possível efectuar a adição e lança uma excepção em caso contrário.
                    this.CheckNullDefaultValueLinesIntegrityForCombination(
                        i,
                        replacementLine,
                        j,
                        combinationLine);

                    var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                    Comparer<long>.Default);

                    var replacementLineEnum = replacementLine.GetColumns().GetEnumerator();
                    var combinationLineEnum = combinationLine.GetColumns().GetEnumerator();
                    var replacementEnumState = replacementLineEnum.MoveNext();
                    var combinationState = combinationLineEnum.MoveNext();
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                otherMatrixElements.Add(key, ring.AdditiveUnity);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                        else if (ring.IsMultiplicativeUnity(b))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                otherMatrixElements.Add(key, combinationLineEnum.Current.Value);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                        else
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                var valueToAdd = ring.Multiply(b, combinationLineEnum.Current.Value);
                                otherMatrixElements.Add(key, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                    }
                    else if (ring.IsAdditiveUnity(b))
                    {
                        if (ring.IsMultiplicativeUnity(a))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }

                            otherMatrixElements = (replacementLine as ASparseDictionaryMatrix<ObjectType, ILongSparseMatrixLine<ObjectType>>
                                .SparseMatrixLine).MatrixEntries;
                        }
                        else
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                otherMatrixElements.Add(key, ring.Multiply(a, replacementLineEnum.Current.Value));
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (ring.IsMultiplicativeUnity(b))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                var valueToAdd = ring.Add(
                                        replacementLineEnum.Current.Value,
                                        combinationLineEnum.Current.Value);
                                otherMatrixElements.Add(key, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                        else
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                valueToAdd = ring.Add(
                                    replacementLineEnum.Current.Value,
                                    valueToAdd);
                                otherMatrixElements.Add(key, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        while (replacementEnumState && combinationState)
                        {
                            var key = replacementLineEnum.Current.Key;
                            var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            valueToAdd = ring.Add(
                                valueToAdd,
                                combinationLineEnum.Current.Value);
                            otherMatrixElements.Add(key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                            combinationState = combinationLineEnum.MoveNext();
                        }
                    }
                    else
                    {
                        while (replacementEnumState && combinationState)
                        {
                            var key = replacementLineEnum.Current.Key;
                            var firstValueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            var secondValueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                            var valueToAdd = ring.Add(
                                firstValueToAdd,
                                secondValueToAdd);
                            otherMatrixElements.Add(key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                            combinationState = combinationLineEnum.MoveNext();
                        }
                    }

                    this.SetLine(i, otherMatrixElements);
                }
                else if (replacementLine.Any())
                {
                    throw new MathematicsException(string.Format(
                            "Trying to combine a null value from line {0} with a non null value from line {1}.",
                            j,
                            i));
                }
            }
            else
            {
                var combinationLine = default(ILongSparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    if (combinationLine.Any())
                    {
                        throw new MathematicsException(string.Format(
                            "Trying to combine a null value from line {0} with a non null value from line {1}.",
                            i,
                            j));
                    }
                }
            }
        }

        /// <summary>
        /// Efectua a combinação das linhas caso o valor por defeito seja nulo.
        /// </summary>
        /// <param name="i">O número da linha a ser substituída.</param>
        /// <param name="j">O número da linha a ser combinada.</param>
        /// <param name="a">O factor a multiplicar pela primeira linha.</param>
        /// <param name="b">O factor a multiplicar pela segunda linha.</param>
        /// <param name="ring">O anel respons+avel pelas operações sobre as entradas da matriz.</param>
        private void CombineLinesWithNullValueForDefault(
            long i,
            long j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            var replacementLine = default(ILongSparseMatrixLine<ObjectType>);
            if (this.TryGetLine(i, out replacementLine))
            {
                var combinationLine = default(ILongSparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    // Assevera se é possível efectuar a adição e lança uma excepção em caso contrário.
                    this.CheckNullDefaultValueLinesIntegrityForCombination(
                        i,
                        replacementLine,
                        j,
                        combinationLine);

                    var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                    Comparer<long>.Default);

                    var replacementLineEnum = replacementLine.GetColumns().GetEnumerator();
                    var combinationLineEnum = combinationLine.GetColumns().GetEnumerator();
                    var replacementEnumState = replacementLineEnum.MoveNext();
                    var combinationState = combinationLineEnum.MoveNext();
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                otherMatrixElements.Add(key, ring.AdditiveUnity);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                        else if (ring.IsMultiplicativeUnity(b))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                otherMatrixElements.Add(key, combinationLineEnum.Current.Value);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                        else
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                var valueToAdd = ring.Multiply(b, combinationLineEnum.Current.Value);
                                otherMatrixElements.Add(key, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                    }
                    else if (ring.IsAdditiveUnity(b))
                    {
                        if (ring.IsMultiplicativeUnity(a))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }

                            otherMatrixElements = (replacementLine as ASparseDictionaryMatrix<ObjectType, ILongSparseMatrixLine<ObjectType>>
                                .SparseMatrixLine).MatrixEntries;
                        }
                        else
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                otherMatrixElements.Add(key, ring.Multiply(a, replacementLineEnum.Current.Value));
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (ring.IsMultiplicativeUnity(b))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                var valueToAdd = ring.Add(
                                        replacementLineEnum.Current.Value,
                                        combinationLineEnum.Current.Value);
                                otherMatrixElements.Add(key, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                        else
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                valueToAdd = ring.Add(
                                    replacementLineEnum.Current.Value,
                                    valueToAdd);
                                otherMatrixElements.Add(key, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        while (replacementEnumState && combinationState)
                        {
                            var key = replacementLineEnum.Current.Key;
                            var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            valueToAdd = ring.Add(
                                valueToAdd,
                                combinationLineEnum.Current.Value);
                            otherMatrixElements.Add(key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                            combinationState = combinationLineEnum.MoveNext();
                        }
                    }
                    else
                    {
                        while (replacementEnumState && combinationState)
                        {
                            var key = replacementLineEnum.Current.Key;
                            var firstValueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            var secondValueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                            var valueToAdd = ring.Add(
                                firstValueToAdd,
                                secondValueToAdd);
                            otherMatrixElements.Add(key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                            combinationState = combinationLineEnum.MoveNext();
                        }
                    }

                    this.SetLine(i, otherMatrixElements);
                }
                else if (replacementLine.Any())
                {
                    throw new MathematicsException(string.Format(
                            "Trying to combine a null value from line {0} with a non null value from line {1}.",
                            j,
                            i));
                }
            }
            else
            {
                var combinationLine = default(ILongSparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    if (combinationLine.Any())
                    {
                        throw new MathematicsException(string.Format(
                            "Trying to combine a null value from line {0} with a non null value from line {1}.",
                            i,
                            j));
                    }
                }
            }
        }

        /// <summary>
        /// Efectua a combinação das linhas caso o valor por defeito seja uma unidade aditiva.
        /// </summary>
        /// <param name="i">O número da linha a ser substituída.</param>
        /// <param name="j">O número da linha a ser combinada.</param>
        /// <param name="a">O factor a ser multipicado pela linha a ser substituída.</param>
        /// <param name="b">O factor a ser multiplicado pela linha a ser combinada.</param>
        /// <param name="ring">O anel responsável pelas operações sobre as entradas da matriz.</param>
        private void CombineLinesWithAdditiveUnityForDefault(
            int i,
            int j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            var replacementLine = default(ILongSparseMatrixLine<ObjectType>);
            if (this.TryGetLine(i, out replacementLine))
            {
                var combinationLine = default(ILongSparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b))
                        {
                            this.Remove(i);  // A linha i é removida do contexto.
                        }
                        else if (ring.IsMultiplicativeUnity(b)) // A linha i passa a ser uma cópia da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            foreach (var line in combinationLine)
                            {
                                otherMatrixElements.Add(line.Key, line.Value);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else if (!ring.IsAdditiveUnity(b)) // A linha i passa a ser múltipla da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            foreach (var line in combinationLine)
                            {
                                var valueToAdd = ring.Multiply(line.Value, b);
                                otherMatrixElements.Add(line.Key, valueToAdd);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                    else if (ring.IsAdditiveUnity(b))
                    {

                        if (!ring.IsMultiplicativeUnity(a)) // A linha i passa a ser múltipla dela própria e se a = 1, mantém-se.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            foreach (var line in replacementLine)
                            {
                                var valueToAdd = ring.Multiply(line.Value, a);
                                otherMatrixElements.Add(line.Key, valueToAdd);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (ring.IsMultiplicativeUnity(b)) // As linhas são somadas.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            var replacementLineEnum = replacementLine.GetColumns().GetEnumerator();
                            var combinationLineEnum = combinationLine.GetColumns().GetEnumerator();
                            var replacementEnumState = replacementLineEnum.MoveNext();
                            var combinationState = combinationLineEnum.MoveNext();
                            var state = replacementEnumState && combinationState;

                            while (replacementEnumState && combinationState)
                            {
                                var replacementKey = replacementLineEnum.Current.Key;
                                var combinationKey = combinationLineEnum.Current.Key;
                                if (replacementKey < combinationKey)
                                {
                                    otherMatrixElements.Add(replacementKey, replacementLineEnum.Current.Value);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    state = replacementEnumState;
                                }
                                else if (replacementKey == combinationKey)
                                {
                                    var valueToAdd = ring.Add(replacementLineEnum.Current.Value, combinationLineEnum.Current.Value);
                                    otherMatrixElements.Add(replacementKey, valueToAdd);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = replacementEnumState && combinationState;
                                }
                                else if (replacementKey > combinationKey)
                                {
                                    otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = combinationState;
                                }
                            }

                            while (replacementEnumState)
                            {
                                otherMatrixElements.Add(replacementLineEnum.Current.Key, replacementLineEnum.Current.Value);
                                replacementEnumState = replacementLineEnum.MoveNext();
                            }

                            while (combinationState)
                            {
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                                combinationState = combinationLineEnum.MoveNext();
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            var replacementLineEnum = replacementLine.GetColumns().GetEnumerator();
                            var combinationLineEnum = combinationLine.GetColumns().GetEnumerator();
                            var replacementEnumState = replacementLineEnum.MoveNext();
                            var combinationState = combinationLineEnum.MoveNext();
                            var state = replacementEnumState && combinationState;

                            while (replacementEnumState && combinationState)
                            {
                                var replacementKey = replacementLineEnum.Current.Key;
                                var combinationKey = combinationLineEnum.Current.Key;
                                if (replacementKey < combinationKey)
                                {
                                    otherMatrixElements.Add(replacementKey, replacementLineEnum.Current.Value);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    state = replacementEnumState;
                                }
                                else if (replacementKey == combinationKey)
                                {
                                    var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                    valueToAdd = ring.Add(replacementLineEnum.Current.Value, valueToAdd);
                                    otherMatrixElements.Add(replacementKey, valueToAdd);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = replacementEnumState && combinationState;
                                }
                                else if (replacementKey > combinationKey)
                                {
                                    var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                    otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = combinationState;
                                }
                            }

                            while (replacementEnumState)
                            {
                                otherMatrixElements.Add(replacementLineEnum.Current.Key, replacementLineEnum.Current.Value);
                                replacementEnumState = replacementLineEnum.MoveNext();
                            }

                            while (combinationState)
                            {
                                var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                                combinationState = combinationLineEnum.MoveNext();
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        var replacementLineEnum = replacementLine.GetColumns().GetEnumerator();
                        var combinationLineEnum = combinationLine.GetColumns().GetEnumerator();
                        var replacementEnumState = replacementLineEnum.MoveNext();
                        var combinationState = combinationLineEnum.MoveNext();
                        var state = replacementEnumState && combinationState;

                        while (replacementEnumState && combinationState)
                        {
                            var replacementKey = replacementLineEnum.Current.Key;
                            var combinationKey = combinationLineEnum.Current.Key;
                            if (replacementKey < combinationKey)
                            {
                                var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                state = replacementEnumState;
                            }
                            else if (replacementKey == combinationKey)
                            {
                                var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, combinationLineEnum.Current.Value);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                                state = replacementEnumState && combinationState;
                            }
                            else if (replacementKey > combinationKey)
                            {
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                                combinationState = combinationLineEnum.MoveNext();
                                state = combinationState;
                            }
                        }

                        while (replacementEnumState)
                        {
                            var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            otherMatrixElements.Add(replacementLineEnum.Current.Key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                        }

                        while (combinationState)
                        {
                            otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                            combinationState = combinationLineEnum.MoveNext();
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                    else
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        var replacementLineEnum = replacementLine.GetColumns().GetEnumerator();
                        var combinationLineEnum = combinationLine.GetColumns().GetEnumerator();
                        var replacementEnumState = replacementLineEnum.MoveNext();
                        var combinationState = combinationLineEnum.MoveNext();
                        var state = replacementEnumState && combinationState;

                        while (replacementEnumState && combinationState)
                        {
                            var replacementKey = replacementLineEnum.Current.Key;
                            var combinationKey = combinationLineEnum.Current.Key;
                            if (replacementKey < combinationKey)
                            {
                                var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                state = replacementEnumState;
                            }
                            else if (replacementKey == combinationKey)
                            {
                                var firstValueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                var secondValueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                var valueToAdd = ring.Add(firstValueToAdd, secondValueToAdd);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                                state = replacementEnumState && combinationState;
                            }
                            else if (replacementKey > combinationKey)
                            {
                                var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                                combinationState = combinationLineEnum.MoveNext();
                                state = combinationState;
                            }
                        }

                        while (replacementEnumState)
                        {
                            var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            otherMatrixElements.Add(replacementLineEnum.Current.Key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                        }

                        while (combinationState)
                        {
                            var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                            otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                            combinationState = combinationLineEnum.MoveNext();
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                }
                else // Existe a linha a ser substituída mas não existe a linha a combinar.
                {
                    if (ring.IsAdditiveUnity(a))
                    {
                        this.Remove(i);
                    }
                    else if (!ring.IsAdditiveUnity(a))
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                            Comparer<long>.Default);
                        foreach (var currentLine in replacementLine)
                        {
                            var valueToAdd = ring.Multiply(currentLine.Value, a);
                            otherMatrixElements.Add(currentLine.Key, valueToAdd);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                }
            }
            else // Existe a linha a combinar mas não existe a linha a ser substituída.
            {
                var combinationLine = default(ILongSparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    if (ring.IsMultiplicativeUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                            Comparer<long>.Default);
                        foreach (var currentLine in combinationLine)
                        {
                            otherMatrixElements.Add(currentLine.Key, currentLine.Value);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                    else if (!ring.IsAdditiveUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                            Comparer<long>.Default);
                        foreach (var currentLine in combinationLine)
                        {
                            var valueToAdd = ring.Multiply(currentLine.Value, b);
                            otherMatrixElements.Add(currentLine.Key, valueToAdd);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                }
            }
        }

        /// <summary>
        /// Efectua a combinação das linhas caso o valor por defeito seja uma unidade aditiva.
        /// </summary>
        /// <param name="i">O número da linha a ser substituída.</param>
        /// <param name="j">O número da linha a ser combinada.</param>
        /// <param name="a">O factor a ser multipicado pela linha a ser substituída.</param>
        /// <param name="b">O factor a ser multiplicado pela linha a ser combinada.</param>
        /// <param name="ring">O anel responsável pelas operações sobre as entradas da matriz.</param>
        private void CombineLinesWithAdditiveUnityForDefault(
            long i,
            long j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            var replacementLine = default(ILongSparseMatrixLine<ObjectType>);
            if (this.TryGetLine(i, out replacementLine))
            {
                var combinationLine = default(ILongSparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b))
                        {
                            this.Remove(i);  // A linha i é removida do contexto.
                        }
                        else if (ring.IsMultiplicativeUnity(b)) // A linha i passa a ser uma cópia da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            foreach (var line in combinationLine)
                            {
                                otherMatrixElements.Add(line.Key, line.Value);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else if (!ring.IsAdditiveUnity(b)) // A linha i passa a ser múltipla da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            foreach (var line in combinationLine)
                            {
                                var valueToAdd = ring.Multiply(line.Value, b);
                                otherMatrixElements.Add(line.Key, valueToAdd);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                    else if (ring.IsAdditiveUnity(b))
                    {

                        if (!ring.IsMultiplicativeUnity(a)) // A linha i passa a ser múltipla dela própria e se a = 1, mantém-se.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            foreach (var line in replacementLine)
                            {
                                var valueToAdd = ring.Multiply(line.Value, a);
                                otherMatrixElements.Add(line.Key, valueToAdd);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (ring.IsMultiplicativeUnity(b)) // As linhas são somadas.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            var replacementLineEnum = replacementLine.GetColumns().GetEnumerator();
                            var combinationLineEnum = combinationLine.GetColumns().GetEnumerator();
                            var replacementEnumState = replacementLineEnum.MoveNext();
                            var combinationState = combinationLineEnum.MoveNext();
                            var state = replacementEnumState && combinationState;

                            while (replacementEnumState && combinationState)
                            {
                                var replacementKey = replacementLineEnum.Current.Key;
                                var combinationKey = combinationLineEnum.Current.Key;
                                if (replacementKey < combinationKey)
                                {
                                    otherMatrixElements.Add(replacementKey, replacementLineEnum.Current.Value);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    state = replacementEnumState;
                                }
                                else if (replacementKey == combinationKey)
                                {
                                    var valueToAdd = ring.Add(replacementLineEnum.Current.Value, combinationLineEnum.Current.Value);
                                    otherMatrixElements.Add(replacementKey, valueToAdd);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = replacementEnumState && combinationState;
                                }
                                else if (replacementKey > combinationKey)
                                {
                                    otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = combinationState;
                                }
                            }

                            while (replacementEnumState)
                            {
                                otherMatrixElements.Add(replacementLineEnum.Current.Key, replacementLineEnum.Current.Value);
                                replacementEnumState = replacementLineEnum.MoveNext();
                            }

                            while (combinationState)
                            {
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                                combinationState = combinationLineEnum.MoveNext();
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            var replacementLineEnum = replacementLine.GetColumns().GetEnumerator();
                            var combinationLineEnum = combinationLine.GetColumns().GetEnumerator();
                            var replacementEnumState = replacementLineEnum.MoveNext();
                            var combinationState = combinationLineEnum.MoveNext();
                            var state = replacementEnumState && combinationState;

                            while (replacementEnumState && combinationState)
                            {
                                var replacementKey = replacementLineEnum.Current.Key;
                                var combinationKey = combinationLineEnum.Current.Key;
                                if (replacementKey < combinationKey)
                                {
                                    otherMatrixElements.Add(replacementKey, replacementLineEnum.Current.Value);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    state = replacementEnumState;
                                }
                                else if (replacementKey == combinationKey)
                                {
                                    var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                    valueToAdd = ring.Add(replacementLineEnum.Current.Value, valueToAdd);
                                    otherMatrixElements.Add(replacementKey, valueToAdd);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = replacementEnumState && combinationState;
                                }
                                else if (replacementKey > combinationKey)
                                {
                                    var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                    otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = combinationState;
                                }
                            }

                            while (replacementEnumState)
                            {
                                otherMatrixElements.Add(replacementLineEnum.Current.Key, replacementLineEnum.Current.Value);
                                replacementEnumState = replacementLineEnum.MoveNext();
                            }

                            while (combinationState)
                            {
                                var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                                combinationState = combinationLineEnum.MoveNext();
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        var replacementLineEnum = replacementLine.GetColumns().GetEnumerator();
                        var combinationLineEnum = combinationLine.GetColumns().GetEnumerator();
                        var replacementEnumState = replacementLineEnum.MoveNext();
                        var combinationState = combinationLineEnum.MoveNext();
                        var state = replacementEnumState && combinationState;

                        while (replacementEnumState && combinationState)
                        {
                            var replacementKey = replacementLineEnum.Current.Key;
                            var combinationKey = combinationLineEnum.Current.Key;
                            if (replacementKey < combinationKey)
                            {
                                var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                state = replacementEnumState;
                            }
                            else if (replacementKey == combinationKey)
                            {
                                var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, combinationLineEnum.Current.Value);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                                state = replacementEnumState && combinationState;
                            }
                            else if (replacementKey > combinationKey)
                            {
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                                combinationState = combinationLineEnum.MoveNext();
                                state = combinationState;
                            }
                        }

                        while (replacementEnumState)
                        {
                            var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            otherMatrixElements.Add(replacementLineEnum.Current.Key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                        }

                        while (combinationState)
                        {
                            otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                            combinationState = combinationLineEnum.MoveNext();
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                    else
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        var replacementLineEnum = replacementLine.GetColumns().GetEnumerator();
                        var combinationLineEnum = combinationLine.GetColumns().GetEnumerator();
                        var replacementEnumState = replacementLineEnum.MoveNext();
                        var combinationState = combinationLineEnum.MoveNext();
                        var state = replacementEnumState && combinationState;

                        while (replacementEnumState && combinationState)
                        {
                            var replacementKey = replacementLineEnum.Current.Key;
                            var combinationKey = combinationLineEnum.Current.Key;
                            if (replacementKey < combinationKey)
                            {
                                var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                state = replacementEnumState;
                            }
                            else if (replacementKey == combinationKey)
                            {
                                var firstValueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                var secondValueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                var valueToAdd = ring.Add(firstValueToAdd, secondValueToAdd);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                                state = replacementEnumState && combinationState;
                            }
                            else if (replacementKey > combinationKey)
                            {
                                var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                                combinationState = combinationLineEnum.MoveNext();
                                state = combinationState;
                            }
                        }

                        while (replacementEnumState)
                        {
                            var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            otherMatrixElements.Add(replacementLineEnum.Current.Key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                        }

                        while (combinationState)
                        {
                            var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                            otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                            combinationState = combinationLineEnum.MoveNext();
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                }
                else // Existe a linha a ser substituída mas não existe a linha a combinar.
                {
                    if (ring.IsAdditiveUnity(a))
                    {
                        this.Remove(i);
                    }
                    else if (!ring.IsAdditiveUnity(a))
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                            Comparer<long>.Default);
                        foreach (var currentLine in replacementLine)
                        {
                            var valueToAdd = ring.Multiply(currentLine.Value, a);
                            otherMatrixElements.Add(currentLine.Key, valueToAdd);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                }
            }
            else // Existe a linha a combinar mas não existe a linha a ser substituída.
            {
                var combinationLine = default(ILongSparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    if (ring.IsMultiplicativeUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                            Comparer<long>.Default);
                        foreach (var currentLine in combinationLine)
                        {
                            otherMatrixElements.Add(currentLine.Key, currentLine.Value);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                    else if (!ring.IsAdditiveUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(
                            Comparer<long>.Default);
                        foreach (var currentLine in combinationLine)
                        {
                            var valueToAdd = ring.Multiply(currentLine.Value, b);
                            otherMatrixElements.Add(currentLine.Key, valueToAdd);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                }
            }
        }

        /// <summary>
        /// Efectua a combinação das linhas caso o valor por defeito seja um valor arbitrário.
        /// </summary>
        /// <param name="i">O número da linha a ser substituída.</param>
        /// <param name="j">O número da linha a ser combinada.</param>
        /// <param name="a">O factor a ser multipicado pela linha a ser substituída.</param>
        /// <param name="b">O factor a ser multiplicado pela linha a ser combinada.</param>
        /// <param name="ring">O anel responsável pelas operações sobre as entradas da matriz.</param>
        private void CombineLinesWithSomeValueForDefault(
            int i,
            int j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            var replacementLine = default(ILongSparseMatrixLine<ObjectType>);
            if (this.TryGetLine(i, out replacementLine))
            {
                var combinationLine = default(ILongSparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b)) // É introduzida uma linha nula.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            for (int k = 0; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, ring.AdditiveUnity);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else if (ring.IsMultiplicativeUnity(b)) // A linha i passa a ser uma cópia da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            foreach (var line in combinationLine)
                            {
                                otherMatrixElements.Add(line.Key, line.Value);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else if (!ring.IsAdditiveUnity(b)) // A linha i passa a ser múltipla da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);

                            // Neste ponto é importante multiplicar os valores por defeito.
                            var defaultValueProduct = ring.Multiply(this.defaultValue, b);
                            var combEnum = combinationLine.GetColumns().GetEnumerator();
                            var k = 0;
                            while (combEnum.MoveNext())
                            {
                                var combKey = combEnum.Current.Key;
                                if (this.defaultValue.Equals(defaultValueProduct))
                                {
                                    k = combKey;
                                }
                                else
                                {
                                    while (k < combKey)
                                    {
                                        otherMatrixElements.Add(k, defaultValueProduct);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(combEnum.Current.Value, b);
                                if (!ring.Equals(this.defaultValue, valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }

                            if (!this.defaultValue.Equals(defaultValueProduct))
                            {
                                while (k < this.afterLastColumn)
                                {
                                    otherMatrixElements.Add(k, defaultValueProduct);
                                    ++k;
                                }
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(i);
                            }
                        }
                    }
                    else if (ring.IsAdditiveUnity(b))
                    {
                        if (!ring.IsMultiplicativeUnity(a)) // A linha i passa a ser múltipla dela própria e se a = 1, mantém-se.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            var defaultValueProduct = ring.Multiply(this.defaultValue, a);
                            var replaceEnum = replacementLine.GetColumns().GetEnumerator();
                            var k = 0;
                            while (replaceEnum.MoveNext())
                            {
                                var replaceKey = replaceEnum.Current.Key;
                                while (k < replaceKey)
                                {
                                    otherMatrixElements.Add(k, defaultValueProduct);
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (ring.IsMultiplicativeUnity(b)) // As linhas somam-se.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            var replaceEnum = replacementLine.GetColumns().GetEnumerator();
                            var combineEnum = combinationLine.GetColumns().GetEnumerator();
                            var replaceState = replaceEnum.MoveNext();
                            var combineState = combineEnum.MoveNext();
                            var state = replaceState && combineState;
                            var k = 0;
                            var defaultAdd = ring.Add(this.defaultValue, this.defaultValue);

                            while (state)
                            {
                                var replaceKey = replaceEnum.Current.Key;
                                var combineKey = combineEnum.Current.Key;
                                if (replaceKey < combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(this.defaultValue, replaceEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    state = replaceState;
                                }
                                else if (replaceKey == combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(replaceEnum.Current.Value, combineEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    combineState = combineEnum.MoveNext();
                                    state = replaceState && combineState;
                                }
                                else
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = combineKey;
                                    }
                                    else
                                    {
                                        while (k < combineKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    combineState = combineEnum.MoveNext();
                                    state = combineState;
                                }
                            }

                            while (replaceState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < replaceEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(this.defaultValue, replaceEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                            }

                            while (combineState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < combineEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                            }

                            // Adiciona os valores que restam.
                            if (!this.defaultValue.Equals(defaultAdd))
                            {
                                for (; k < this.afterLastColumn; ++k)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                }
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(i);
                            }
                        }
                        else // Soma da linha i com um múltiplo da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            var replaceEnum = replacementLine.GetColumns().GetEnumerator();
                            var combineEnum = combinationLine.GetColumns().GetEnumerator();
                            var replaceState = replaceEnum.MoveNext();
                            var combineState = combineEnum.MoveNext();
                            var state = replaceState && combineState;
                            var k = 0;
                            var defaultMultiplied = ring.Multiply(this.defaultValue, b);
                            var defaultAdd = ring.Add(defaultMultiplied, this.defaultValue);

                            while (state)
                            {
                                var replaceKey = replaceEnum.Current.Key;
                                var combineKey = combineEnum.Current.Key;
                                if (replaceKey < combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(defaultMultiplied, replaceEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    state = replaceState;
                                }
                                else if (replaceKey == combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                                    valueToAdd = ring.Add(replaceEnum.Current.Value, valueToAdd);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    combineState = combineEnum.MoveNext();
                                    state = replaceState && combineState;
                                }
                                else
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = combineKey;
                                    }
                                    else
                                    {
                                        while (k < combineKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    combineState = combineEnum.MoveNext();
                                    state = combineState;
                                }
                            }

                            while (replaceState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < replaceEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(defaultMultiplied, replaceEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                            }

                            while (combineState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < combineEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                            }

                            // Adiciona os valores que restam.
                            if (!this.defaultValue.Equals(defaultAdd))
                            {
                                for (; k < this.afterLastColumn; ++k)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                }
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(i);
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b)) // Soma de um múltiplo da linha i com a linha j.
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        var replaceEnum = replacementLine.GetColumns().GetEnumerator();
                        var combineEnum = combinationLine.GetColumns().GetEnumerator();
                        var replaceState = replaceEnum.MoveNext();
                        var combineState = combineEnum.MoveNext();
                        var state = replaceState && combineState;
                        var k = 0;
                        var defaultMultiplied = ring.Multiply(this.defaultValue, a);
                        var defaultAdd = ring.Add(defaultMultiplied, this.defaultValue);

                        while (state)
                        {
                            var replaceKey = replaceEnum.Current.Key;
                            var combineKey = combineEnum.Current.Key;
                            if (replaceKey < combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, this.defaultValue);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                state = replaceState;
                            }
                            else if (replaceKey == combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                combineState = combineEnum.MoveNext();
                                state = replaceState && combineState;
                            }
                            else
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineKey;
                                }
                                else
                                {
                                    while (k < combineKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(defaultMultiplied, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                                state = combineState;
                            }
                        }

                        while (replaceState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = replaceEnum.Current.Key;
                            }
                            else
                            {
                                while (k < replaceEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                            valueToAdd = ring.Add(valueToAdd, this.defaultValue);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            replaceState = replaceEnum.MoveNext();
                        }

                        while (combineState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = combineEnum.Current.Key;
                            }
                            else
                            {
                                while (k < combineEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Add(defaultMultiplied, combineEnum.Current.Value);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            combineState = combineEnum.MoveNext();
                        }

                        // Adiciona os valores que restam.
                        if (!this.defaultValue.Equals(defaultAdd))
                        {
                            for (; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, defaultAdd);
                            }
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                        else
                        {
                            this.Remove(i);
                        }
                    }
                    else // Adiciona múltiplos de ambas as linhas.
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        var replaceEnum = replacementLine.GetColumns().GetEnumerator();
                        var combineEnum = combinationLine.GetColumns().GetEnumerator();
                        var replaceState = replaceEnum.MoveNext();
                        var combineState = combineEnum.MoveNext();
                        var state = replaceState && combineState;
                        var k = 0;
                        var firstsDefaultMultiplied = ring.Multiply(this.defaultValue, a);
                        var secondDefaultMultiplied = ring.Multiply(this.defaultValue, b);
                        var defaultAdd = ring.Add(firstsDefaultMultiplied, secondDefaultMultiplied);

                        while (state)
                        {
                            var replaceKey = replaceEnum.Current.Key;
                            var combineKey = combineEnum.Current.Key;
                            if (replaceKey < combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, secondDefaultMultiplied);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                state = replaceState;
                            }
                            else if (replaceKey == combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var firstValueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                var secondValueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                                var valueToAdd = ring.Add(firstValueToAdd, secondValueToAdd);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                combineState = combineEnum.MoveNext();
                                state = replaceState && combineState;
                            }
                            else
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineKey;
                                }
                                else
                                {
                                    while (k < combineKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                                valueToAdd = ring.Add(firstsDefaultMultiplied, valueToAdd);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                                state = combineState;
                            }
                        }

                        while (replaceState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = replaceEnum.Current.Key;
                            }
                            else
                            {
                                while (k < replaceEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                            valueToAdd = ring.Add(valueToAdd, secondDefaultMultiplied);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            replaceState = replaceEnum.MoveNext();
                        }

                        while (combineState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = combineEnum.Current.Key;
                            }
                            else
                            {
                                while (k < combineEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                            valueToAdd = ring.Add(firstsDefaultMultiplied, valueToAdd);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            combineState = combineEnum.MoveNext();
                        }

                        // Adiciona os valores que restam.
                        if (!this.defaultValue.Equals(defaultAdd))
                        {
                            for (; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, defaultAdd);
                            }
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                        else
                        {
                            this.Remove(i);
                        }
                    }
                }
                else // Existe a linha a ser substituída mas não existe a linha a combinar.
                {
                    if (ring.IsAdditiveUnity(a)) // É inserida a linha nula na posição i.
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        for (int k = 0; k < this.afterLastColumn; ++k)
                        {
                            otherMatrixElements.Add(k, ring.AdditiveUnity);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (!ring.IsAdditiveUnity(b))
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            var k = 0;
                            var defaultMultiply = ring.Multiply(this.defaultValue, b);
                            foreach (var entry in replacementLine)
                            {
                                if (this.defaultValue.Equals(defaultMultiply))
                                {
                                    k = entry.Key;
                                }
                                else
                                {
                                    while (k < entry.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultMultiply);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(entry.Value, defaultMultiply);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(i);
                            }
                        }
                    }
                    else if (!ring.IsAdditiveUnity(b)) // Adiciona os valores por defeito da linha inexistente.
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        var k = 0;
                        var firstDefaultMultiply = ring.Multiply(this.defaultValue, a);
                        var secondDefaultMultiply = ring.Multiply(this.defaultValue, b);
                        var defaultAdd = ring.Add(firstDefaultMultiply, secondDefaultMultiply);

                        foreach (var entry in replacementLine)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = entry.Key;
                            }
                            else
                            {
                                while (k < entry.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(entry.Value, a);
                            valueToAdd = ring.Add(valueToAdd, secondDefaultMultiply);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                        }


                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                        else
                        {
                            this.Remove(i);
                        }
                    }
                }
            }
            else
            {
                var combinationLine = default(ILongSparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    // Existe a linha a combinar mas não existe a linha a ser substituída.
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b)) // Terá de ser acrescentada uma linha vazia.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            for (int k = 0; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, ring.AdditiveUnity);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else if (ring.IsMultiplicativeUnity(b)) // É acrescentada uma cópia da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            foreach (var entry in combinationLine)
                            {
                                otherMatrixElements.Add(entry.Key, entry.Value);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else // É acrescentado um múltiplo da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            var k = 0;
                            var defaultMultiplied = ring.Multiply(this.defaultValue, b);
                            foreach (var entry in combinationLine)
                            {
                                if (this.defaultValue.Equals(defaultMultiplied))
                                {
                                    k = entry.Key;
                                }
                                else
                                {
                                    while (k < entry.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultMultiplied);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(entry.Value, b);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        var k = 0;
                        var firstDefaultMultiplication = ring.Multiply(this.defaultValue, a);
                        var defaultValueToAdd = ring.Add(firstDefaultMultiplication, this.defaultValue);
                        foreach (var entry in combinationLine)
                        {
                            if (this.defaultValue.Equals(defaultValueToAdd))
                            {
                                k = entry.Key;
                            }
                            else
                            {
                                while (k < entry.Key)
                                {
                                    otherMatrixElements.Add(k, defaultValueToAdd);
                                }
                            }

                            var valueToAdd = ring.Add(firstDefaultMultiplication, entry.Value);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                    else
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        var k = 0;
                        var firstDefaultMultiplication = ring.Multiply(this.defaultValue, a);
                        var secondMultiplication = ring.Multiply(this.defaultValue, b);
                        var defaultValueToAdd = ring.Add(firstDefaultMultiplication, secondMultiplication);
                        foreach (var entry in combinationLine)
                        {
                            if (this.defaultValue.Equals(defaultValueToAdd))
                            {
                                k = entry.Key;
                            }
                            else
                            {
                                while (k < entry.Key)
                                {
                                    otherMatrixElements.Add(k, defaultValueToAdd);
                                }
                            }

                            var valueToAdd = ring.Multiply(entry.Value, b);
                            valueToAdd = ring.Add(firstDefaultMultiplication, valueToAdd);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                }
                else // Não existem ambas as linhas.
                {
                    var defaultValueToAdd = ring.Add(a, b);
                    defaultValueToAdd = ring.Multiply(defaultValueToAdd, this.defaultValue);
                    if (!this.defaultValue.Equals(defaultValueToAdd))
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        for (int k = 0; k < this.afterLastColumn; ++k)
                        {
                            otherMatrixElements.Add(k, defaultValueToAdd);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                }
            }
        }

        /// <summary>
        /// Efectua a combinação das linhas caso o valor por defeito seja um valor arbitrário.
        /// </summary>
        /// <param name="i">O número da linha a ser substituída.</param>
        /// <param name="j">O número da linha a ser combinada.</param>
        /// <param name="a">O factor a ser multipicado pela linha a ser substituída.</param>
        /// <param name="b">O factor a ser multiplicado pela linha a ser combinada.</param>
        /// <param name="ring">O anel responsável pelas operações sobre as entradas da matriz.</param>
        private void CombineLinesWithSomeValueForDefault(
            long i,
            long j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            var replacementLine = default(ILongSparseMatrixLine<ObjectType>);
            if (this.TryGetLine(i, out replacementLine))
            {
                var combinationLine = default(ILongSparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b)) // É introduzida uma linha nula.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            for (int k = 0; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, ring.AdditiveUnity);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else if (ring.IsMultiplicativeUnity(b)) // A linha i passa a ser uma cópia da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            foreach (var line in combinationLine)
                            {
                                otherMatrixElements.Add(line.Key, line.Value);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else if (!ring.IsAdditiveUnity(b)) // A linha i passa a ser múltipla da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);

                            // Neste ponto é importante multiplicar os valores por defeito.
                            var defaultValueProduct = ring.Multiply(this.defaultValue, b);
                            var combEnum = combinationLine.GetColumns().GetEnumerator();
                            var k = 0;
                            while (combEnum.MoveNext())
                            {
                                var combKey = combEnum.Current.Key;
                                if (this.defaultValue.Equals(defaultValueProduct))
                                {
                                    k = combKey;
                                }
                                else
                                {
                                    while (k < combKey)
                                    {
                                        otherMatrixElements.Add(k, defaultValueProduct);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(combEnum.Current.Value, b);
                                if (!ring.Equals(this.defaultValue, valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }

                            if (!this.defaultValue.Equals(defaultValueProduct))
                            {
                                while (k < this.afterLastColumn)
                                {
                                    otherMatrixElements.Add(k, defaultValueProduct);
                                    ++k;
                                }
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(i);
                            }
                        }
                    }
                    else if (ring.IsAdditiveUnity(b))
                    {
                        if (!ring.IsMultiplicativeUnity(a)) // A linha i passa a ser múltipla dela própria e se a = 1, mantém-se.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            var defaultValueProduct = ring.Multiply(this.defaultValue, a);
                            var replaceEnum = replacementLine.GetColumns().GetEnumerator();
                            var k = 0;
                            while (replaceEnum.MoveNext())
                            {
                                var replaceKey = replaceEnum.Current.Key;
                                while (k < replaceKey)
                                {
                                    otherMatrixElements.Add(k, defaultValueProduct);
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (ring.IsMultiplicativeUnity(b)) // As linhas somam-se.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            var replaceEnum = replacementLine.GetColumns().GetEnumerator();
                            var combineEnum = combinationLine.GetColumns().GetEnumerator();
                            var replaceState = replaceEnum.MoveNext();
                            var combineState = combineEnum.MoveNext();
                            var state = replaceState && combineState;
                            var k = 0;
                            var defaultAdd = ring.Add(this.defaultValue, this.defaultValue);

                            while (state)
                            {
                                var replaceKey = replaceEnum.Current.Key;
                                var combineKey = combineEnum.Current.Key;
                                if (replaceKey < combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(this.defaultValue, replaceEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    state = replaceState;
                                }
                                else if (replaceKey == combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(replaceEnum.Current.Value, combineEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    combineState = combineEnum.MoveNext();
                                    state = replaceState && combineState;
                                }
                                else
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = combineKey;
                                    }
                                    else
                                    {
                                        while (k < combineKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    combineState = combineEnum.MoveNext();
                                    state = combineState;
                                }
                            }

                            while (replaceState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < replaceEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(this.defaultValue, replaceEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                            }

                            while (combineState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < combineEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                            }

                            // Adiciona os valores que restam.
                            if (!this.defaultValue.Equals(defaultAdd))
                            {
                                for (; k < this.afterLastColumn; ++k)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                }
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(i);
                            }
                        }
                        else // Soma da linha i com um múltiplo da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            var replaceEnum = replacementLine.GetColumns().GetEnumerator();
                            var combineEnum = combinationLine.GetColumns().GetEnumerator();
                            var replaceState = replaceEnum.MoveNext();
                            var combineState = combineEnum.MoveNext();
                            var state = replaceState && combineState;
                            var k = 0;
                            var defaultMultiplied = ring.Multiply(this.defaultValue, b);
                            var defaultAdd = ring.Add(defaultMultiplied, this.defaultValue);

                            while (state)
                            {
                                var replaceKey = replaceEnum.Current.Key;
                                var combineKey = combineEnum.Current.Key;
                                if (replaceKey < combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(defaultMultiplied, replaceEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    state = replaceState;
                                }
                                else if (replaceKey == combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                                    valueToAdd = ring.Add(replaceEnum.Current.Value, valueToAdd);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    combineState = combineEnum.MoveNext();
                                    state = replaceState && combineState;
                                }
                                else
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = combineKey;
                                    }
                                    else
                                    {
                                        while (k < combineKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    combineState = combineEnum.MoveNext();
                                    state = combineState;
                                }
                            }

                            while (replaceState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < replaceEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(defaultMultiplied, replaceEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                            }

                            while (combineState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < combineEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                            }

                            // Adiciona os valores que restam.
                            if (!this.defaultValue.Equals(defaultAdd))
                            {
                                for (; k < this.afterLastColumn; ++k)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                }
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(i);
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b)) // Soma de um múltiplo da linha i com a linha j.
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        var replaceEnum = replacementLine.GetColumns().GetEnumerator();
                        var combineEnum = combinationLine.GetColumns().GetEnumerator();
                        var replaceState = replaceEnum.MoveNext();
                        var combineState = combineEnum.MoveNext();
                        var state = replaceState && combineState;
                        var k = 0;
                        var defaultMultiplied = ring.Multiply(this.defaultValue, a);
                        var defaultAdd = ring.Add(defaultMultiplied, this.defaultValue);

                        while (state)
                        {
                            var replaceKey = replaceEnum.Current.Key;
                            var combineKey = combineEnum.Current.Key;
                            if (replaceKey < combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, this.defaultValue);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                state = replaceState;
                            }
                            else if (replaceKey == combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                combineState = combineEnum.MoveNext();
                                state = replaceState && combineState;
                            }
                            else
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineKey;
                                }
                                else
                                {
                                    while (k < combineKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(defaultMultiplied, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                                state = combineState;
                            }
                        }

                        while (replaceState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = replaceEnum.Current.Key;
                            }
                            else
                            {
                                while (k < replaceEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                            valueToAdd = ring.Add(valueToAdd, this.defaultValue);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            replaceState = replaceEnum.MoveNext();
                        }

                        while (combineState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = combineEnum.Current.Key;
                            }
                            else
                            {
                                while (k < combineEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Add(defaultMultiplied, combineEnum.Current.Value);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            combineState = combineEnum.MoveNext();
                        }

                        // Adiciona os valores que restam.
                        if (!this.defaultValue.Equals(defaultAdd))
                        {
                            for (; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, defaultAdd);
                            }
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                        else
                        {
                            this.Remove(i);
                        }
                    }
                    else // Adiciona múltiplos de ambas as linhas.
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        var replaceEnum = replacementLine.GetColumns().GetEnumerator();
                        var combineEnum = combinationLine.GetColumns().GetEnumerator();
                        var replaceState = replaceEnum.MoveNext();
                        var combineState = combineEnum.MoveNext();
                        var state = replaceState && combineState;
                        var k = 0;
                        var firstsDefaultMultiplied = ring.Multiply(this.defaultValue, a);
                        var secondDefaultMultiplied = ring.Multiply(this.defaultValue, b);
                        var defaultAdd = ring.Add(firstsDefaultMultiplied, secondDefaultMultiplied);

                        while (state)
                        {
                            var replaceKey = replaceEnum.Current.Key;
                            var combineKey = combineEnum.Current.Key;
                            if (replaceKey < combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, secondDefaultMultiplied);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                state = replaceState;
                            }
                            else if (replaceKey == combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var firstValueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                var secondValueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                                var valueToAdd = ring.Add(firstValueToAdd, secondValueToAdd);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                combineState = combineEnum.MoveNext();
                                state = replaceState && combineState;
                            }
                            else
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineKey;
                                }
                                else
                                {
                                    while (k < combineKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                                valueToAdd = ring.Add(firstsDefaultMultiplied, valueToAdd);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                                state = combineState;
                            }
                        }

                        while (replaceState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = replaceEnum.Current.Key;
                            }
                            else
                            {
                                while (k < replaceEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                            valueToAdd = ring.Add(valueToAdd, secondDefaultMultiplied);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            replaceState = replaceEnum.MoveNext();
                        }

                        while (combineState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = combineEnum.Current.Key;
                            }
                            else
                            {
                                while (k < combineEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                            valueToAdd = ring.Add(firstsDefaultMultiplied, valueToAdd);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            combineState = combineEnum.MoveNext();
                        }

                        // Adiciona os valores que restam.
                        if (!this.defaultValue.Equals(defaultAdd))
                        {
                            for (; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, defaultAdd);
                            }
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                        else
                        {
                            this.Remove(i);
                        }
                    }
                }
                else // Existe a linha a ser substituída mas não existe a linha a combinar.
                {
                    if (ring.IsAdditiveUnity(a)) // É inserida a linha nula na posição i.
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        for (int k = 0; k < this.afterLastColumn; ++k)
                        {
                            otherMatrixElements.Add(k, ring.AdditiveUnity);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (!ring.IsAdditiveUnity(b))
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            var k = 0;
                            var defaultMultiply = ring.Multiply(this.defaultValue, b);
                            foreach (var entry in replacementLine)
                            {
                                if (this.defaultValue.Equals(defaultMultiply))
                                {
                                    k = entry.Key;
                                }
                                else
                                {
                                    while (k < entry.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultMultiply);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(entry.Value, defaultMultiply);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(i);
                            }
                        }
                    }
                    else if (!ring.IsAdditiveUnity(b)) // Adiciona os valores por defeito da linha inexistente.
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        var k = 0;
                        var firstDefaultMultiply = ring.Multiply(this.defaultValue, a);
                        var secondDefaultMultiply = ring.Multiply(this.defaultValue, b);
                        var defaultAdd = ring.Add(firstDefaultMultiply, secondDefaultMultiply);

                        foreach (var entry in replacementLine)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = entry.Key;
                            }
                            else
                            {
                                while (k < entry.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(entry.Value, a);
                            valueToAdd = ring.Add(valueToAdd, secondDefaultMultiply);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                        }


                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                        else
                        {
                            this.Remove(i);
                        }
                    }
                }
            }
            else
            {
                var combinationLine = default(ILongSparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    // Existe a linha a combinar mas não existe a linha a ser substituída.
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b)) // Terá de ser acrescentada uma linha vazia.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            for (int k = 0; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, ring.AdditiveUnity);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else if (ring.IsMultiplicativeUnity(b)) // É acrescentada uma cópia da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            foreach (var entry in combinationLine)
                            {
                                otherMatrixElements.Add(entry.Key, entry.Value);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else // É acrescentado um múltiplo da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                            var k = 0;
                            var defaultMultiplied = ring.Multiply(this.defaultValue, b);
                            foreach (var entry in combinationLine)
                            {
                                if (this.defaultValue.Equals(defaultMultiplied))
                                {
                                    k = entry.Key;
                                }
                                else
                                {
                                    while (k < entry.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultMultiplied);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(entry.Value, b);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        var k = 0;
                        var firstDefaultMultiplication = ring.Multiply(this.defaultValue, a);
                        var defaultValueToAdd = ring.Add(firstDefaultMultiplication, this.defaultValue);
                        foreach (var entry in combinationLine)
                        {
                            if (this.defaultValue.Equals(defaultValueToAdd))
                            {
                                k = entry.Key;
                            }
                            else
                            {
                                while (k < entry.Key)
                                {
                                    otherMatrixElements.Add(k, defaultValueToAdd);
                                }
                            }

                            var valueToAdd = ring.Add(firstDefaultMultiplication, entry.Value);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                    else
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        var k = 0;
                        var firstDefaultMultiplication = ring.Multiply(this.defaultValue, a);
                        var secondMultiplication = ring.Multiply(this.defaultValue, b);
                        var defaultValueToAdd = ring.Add(firstDefaultMultiplication, secondMultiplication);
                        foreach (var entry in combinationLine)
                        {
                            if (this.defaultValue.Equals(defaultValueToAdd))
                            {
                                k = entry.Key;
                            }
                            else
                            {
                                while (k < entry.Key)
                                {
                                    otherMatrixElements.Add(k, defaultValueToAdd);
                                }
                            }

                            var valueToAdd = ring.Multiply(entry.Value, b);
                            valueToAdd = ring.Add(firstDefaultMultiplication, valueToAdd);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                }
                else // Não existem ambas as linhas.
                {
                    var defaultValueToAdd = ring.Add(a, b);
                    defaultValueToAdd = ring.Multiply(defaultValueToAdd, this.defaultValue);
                    if (!this.defaultValue.Equals(defaultValueToAdd))
                    {
                        var otherMatrixElements = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
                        for (int k = 0; k < this.afterLastColumn; ++k)
                        {
                            otherMatrixElements.Add(k, defaultValueToAdd);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                }
            }
        }

        /// <summary>
        /// Atribui o conjunto de elementos à linha especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da linha.</param>
        /// <param name="values">O conjunto de valores.</param>
        private void SetLine(int index, SortedDictionary<long, ObjectType> values)
        {
            if (this.matrixLines.ContainsKey(index))
            {
                this.matrixLines[index] = new ASparseDictionaryMatrix<ObjectType, ILongSparseMatrixLine<ObjectType>>
                            .SparseMatrixLine(values, this);
            }
            else
            {
                this.matrixLines.Add(index, new ASparseDictionaryMatrix<ObjectType, ILongSparseMatrixLine<ObjectType>>
                            .SparseMatrixLine(values, this));
            }
        }

        /// <summary>
        /// Atribui o conjunto de elementos à linha especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da linha.</param>
        /// <param name="values">O conjunto de valores.</param>
        private void SetLine(long index, SortedDictionary<long, ObjectType> values)
        {
            if (this.matrixLines.ContainsKey(index))
            {
                this.matrixLines[index] = new ASparseDictionaryMatrix<ObjectType, ILongSparseMatrixLine<ObjectType>>
                            .SparseMatrixLine(values, this);
            }
            else
            {
                this.matrixLines.Add(index, new ASparseDictionaryMatrix<ObjectType, ILongSparseMatrixLine<ObjectType>>
                            .SparseMatrixLine(values, this));
            }
        }

        #endregion Private Methods
    }
}
