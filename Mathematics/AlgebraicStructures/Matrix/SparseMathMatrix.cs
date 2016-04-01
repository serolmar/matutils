namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Representação em termos de coordenadas de uma matriz esparsa.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem as entradas da matriz.</typeparam>
    public class CoordinateSparseMathMatrix<CoeffType>
        : SparseCoordinateMatrix<CoeffType>, ILongSparseMathMatrix<CoeffType>
    {
        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordinateSparseMathMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="defaultValue">O valor por defeito a ser assumido pela matriz.</param>
        public CoordinateSparseMathMatrix(CoeffType defaultValue)
            : base(defaultValue) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordinateSparseMathMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="lines">O número de linha contidas na matriz.</param>
        /// <param name="columns">O número de colunas contidas na matriz.</param>
        public CoordinateSparseMathMatrix(long lines, long columns)
            : base(lines, columns) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordinateSparseMathMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="lines">O número de linha contidas na matriz.</param>
        /// <param name="columns">O número de colunas contidas na matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public CoordinateSparseMathMatrix(long lines, long columns, CoeffType defaultValue)
            : base(lines, columns, defaultValue) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordinateSparseMathMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="lines">O número de linha contidas na matriz.</param>
        /// <param name="columns">O número de colunas contidas na matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <param name="comparer">O comparador que permite identificar os valores por defeito inseridos.</param>
        public CoordinateSparseMathMatrix(
            long lines,
            long columns,
            CoeffType defaultValue,
            IEqualityComparer<CoeffType> comparer)
            : base(lines, columns, defaultValue, comparer) { }

        /// <summary>
        /// Multiplica os valores da linha pelo escalar definido.
        /// </summary>
        /// <param name="line">A linha a ser considerada.</param>
        /// <param name="scalar">O escalar a ser multiplicado.</param>
        /// <param name="ring">O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.</param>
        public void ScalarLineMultiplication(int line, CoeffType scalar, IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (line < 0 || line >= this.afterLastLine)
            {
                throw new IndexOutOfRangeException("The parameter line is out of bounds.");
            }
            else
            {
                var elementsCount = this.elements.Count;
                if (elementsCount > 0)
                {
                    var both = this.FindBothPositions(line, 0, elementsCount);
                    var start = both.Item1;
                    if (start < elementsCount)
                    {
                        var end = both.Item2;
                        var defaultMultiplied = ring.Multiply(this.defaultValue, scalar);
                        if (this.comparer.Equals(this.defaultValue, defaultMultiplied))
                        {
                            // A multiplicação pelo valor por defeito continua a resultar no valor por defeito
                            var currentIndex = start;
                            while (currentIndex < end)
                            {
                                var current = this.elements[currentIndex];
                                var result = ring.Multiply(current.Item3.Item1, scalar);
                                if (this.comparer.Equals(this.defaultValue, result))
                                {
                                    this.elements.RemoveAt(currentIndex);
                                    --end;
                                }
                                else
                                {
                                    current.Item3.Item1 = result;
                                    ++currentIndex;
                                }
                            }
                        }
                        else
                        {
                            // A multiplicação pelo valor por defeito deixa de ser um valor por defeito
                            var currentIndex = start;
                            var column = 0;
                            while (currentIndex < end)
                            {
                                var current = this.elements[currentIndex];
                                var currentColumn = current.Item2;
                                while (column < currentColumn)
                                {
                                    this.elements.Insert(
                                        currentIndex,
                                        MutableTuple.Create<long,long,MutableTuple<CoeffType>>(line, column, MutableTuple.Create(defaultMultiplied)));
                                    ++column;
                                    ++currentIndex;
                                    ++end;
                                }

                                var result = ring.Multiply(current.Item3.Item1, scalar);
                                if (this.comparer.Equals(this.defaultValue, result))
                                {
                                    this.elements.RemoveAt(currentIndex);
                                    --end;
                                }
                                else
                                {
                                    current.Item3.Item1 = result;
                                    ++currentIndex;
                                }

                                ++column;
                            }

                            // Insere os últimos valores na linha
                            for (; column < this.afterLastColumn; ++column)
                            {
                                this.elements.Insert(
                                        currentIndex,
                                        MutableTuple.Create<long, long, MutableTuple<CoeffType>>(line, column, MutableTuple.Create(defaultMultiplied)));
                            }
                        }
                    }
                    else
                    {
                        // A linha é superior a todos os elementos existentes
                        var multiple = ring.Multiply(this.defaultValue, scalar);
                        if (!this.comparer.Equals(multiple, this.defaultValue))
                        {
                            // O valor obtido terá de ser adicionado para todas as colunas
                            for (int i = 0; i < this.afterLastColumn; ++i)
                            {
                                this.elements.Add(MutableTuple.Create<long, long, MutableTuple<CoeffType>>(
                                    line,
                                    i,
                                    MutableTuple.Create(multiple)));
                            }
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
        /// <param name="ring">O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.</param>
        public void ScalarLineMultiplication(long line, CoeffType scalar, IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (line < 0 || line >= this.afterLastLine)
            {
                throw new IndexOutOfRangeException("The parameter line is out of bounds.");
            }
            else
            {
                var elementsCount = this.elements.Count;
                if (elementsCount > 0)
                {
                    var both = this.FindBothPositions(line, 0, elementsCount);
                    var start = both.Item1;
                    if (start < elementsCount)
                    {
                        var end = both.Item2;
                        var defaultMultiplied = ring.Multiply(this.defaultValue, scalar);
                        if (this.comparer.Equals(this.defaultValue, defaultMultiplied))
                        {
                            // A multiplicação pelo valor por defeito continua a resultar no valor por defeito
                            var currentIndex = start;
                            while (currentIndex < end)
                            {
                                var current = this.elements[currentIndex];
                                var result = ring.Multiply(current.Item3.Item1, scalar);
                                if (this.comparer.Equals(this.defaultValue, result))
                                {
                                    this.elements.RemoveAt(currentIndex);
                                    --end;
                                }
                                else
                                {
                                    current.Item3.Item1 = result;
                                    ++currentIndex;
                                }
                            }
                        }
                        else
                        {
                            // A multiplicação pelo valor por defeito deixa de ser um valor por defeito
                            var currentIndex = start;
                            var column = 0L;
                            while (currentIndex < end)
                            {
                                var current = this.elements[currentIndex];
                                var currentColumn = current.Item2;
                                while (column < currentColumn)
                                {
                                    this.elements.Insert(
                                        currentIndex,
                                        MutableTuple.Create(line, column, MutableTuple.Create(defaultMultiplied)));
                                    ++column;
                                    ++currentIndex;
                                    ++end;
                                }

                                var result = ring.Multiply(current.Item3.Item1, scalar);
                                if (this.comparer.Equals(this.defaultValue, result))
                                {
                                    this.elements.RemoveAt(currentIndex);
                                    --end;
                                }
                                else
                                {
                                    current.Item3.Item1 = result;
                                    ++currentIndex;
                                }

                                ++column;
                            }

                            // Insere os últimos valores na linha
                            for (; column < this.afterLastColumn; ++column)
                            {
                                this.elements.Insert(
                                        currentIndex,
                                        MutableTuple.Create(line, column, MutableTuple.Create(defaultMultiplied)));
                            }
                        }
                    }
                    else
                    {
                        // A linha é superior a todos os elementos existentes
                        var multiple = ring.Multiply(this.defaultValue, scalar);
                        if (!this.comparer.Equals(multiple, this.defaultValue))
                        {
                            // O valor obtido terá de ser adicionado para todas as colunas
                            for (var i = 0L; i < this.afterLastColumn; ++i)
                            {
                                this.elements.Add(MutableTuple.Create(
                                    line,
                                    i,
                                    MutableTuple.Create(multiple)));
                            }
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
        public void CombineLines(int i, int j, CoeffType a, CoeffType b, IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (i < 0 || i >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else if (i == j)
            {
                var scalar = ring.Add(a, b);
                this.ScalarLineMultiplication(i, scalar, ring);
            }
            else
            {
                var elementsCount = this.elements.Count;
                if (elementsCount > 0)
                {
                    var firstValues = default(Tuple<int, int>);
                    var secondValues = default(Tuple<int, int>);
                    if (i < j)
                    {
                        // A linha a substituir é anterior à linha a ser combinada
                        firstValues = this.FindBothPositions(i, 0, elementsCount);
                        secondValues = this.FindBothPositions(j, firstValues.Item2, elementsCount);
                    }
                    else
                    {
                        // A linha a substituir é posterior à linha a ser combinada
                        secondValues = this.FindBothPositions(j, 0, elementsCount);
                        firstValues = this.FindBothPositions(i, secondValues.Item2, elementsCount);
                    }

                    // Se a combinação de valores por defeito for um valor por defeito, o cálculo simplifica-se
                    var firstScalar = ring.Multiply(this.defaultValue, a);
                    var secondScalar = ring.Multiply(this.defaultValue, b);
                    var additionValue = ring.Add(firstScalar, secondScalar);
                    if (this.comparer.Equals(additionValue, this.defaultValue))
                    {
                        // Neste caso os valores por defeito são ignorados
                        var k = firstValues.Item1;
                        var n = firstValues.Item2;
                        var l = secondValues.Item1;
                        var m = secondValues.Item2;

                        while (k < n && l < m)
                        {
                            var currentFirst = this.elements[k];
                            var currentSecond = this.elements[l];
                            if (currentFirst.Item2 < currentSecond.Item2)
                            {
                                var secondCurrentScalar = ring.Multiply(
                                    b,
                                    currentSecond.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstScalar,
                                    secondCurrentScalar);
                                if (!this.comparer.Equals(this.defaultValue, additionValue))
                                {
                                    // A soma não consiste no valor por defeito
                                    this.elements.Insert(
                                        k,
                                        MutableTuple.Create<long, long, MutableTuple<CoeffType>>(i, currentSecond.Item2, MutableTuple.Create(currentAddition)));

                                    // Como um elemento foi inserido, todos os índices terão de ser alterados
                                    ++k;
                                    ++n;
                                    ++l;
                                    ++m;
                                }

                                ++l;
                            }
                            else if (currentFirst.Item2 == currentSecond.Item2)
                            {
                                var firstCurrentScalar = ring.Multiply(
                                    a,
                                    currentFirst.Item3.Item1);
                                var secondCurrentScalar = ring.Multiply(
                                    b,
                                    currentSecond.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstCurrentScalar,
                                    secondCurrentScalar);
                                if (this.comparer.Equals(
                                    currentAddition,
                                    this.defaultValue))
                                {
                                    // É marcado o item para remoção
                                    this.elements.RemoveAt(k);
                                    --k;
                                    --l;
                                    --m;
                                    --n;
                                }
                                else
                                {
                                    currentFirst.Item3.Item1 = additionValue;
                                }
                            }
                            else
                            {
                                var firstCurrentScalar = ring.Multiply(
                                    a,
                                    currentFirst.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstCurrentScalar,
                                    this.defaultValue);
                                if (this.comparer.Equals(currentAddition, this.defaultValue))
                                {
                                    this.elements.RemoveAt(k);
                                    --k;
                                    --l;
                                    --m;
                                    --n;
                                }
                                else
                                {
                                    currentFirst.Item3.Item1 = currentAddition;
                                }

                                ++k;
                            }
                        }

                        // Continuação para os termos restantes
                        while (k < n)
                        {
                            var currentFirst = this.elements[k];
                            var firstCurrentScalar = ring.Multiply(
                                       a,
                                       currentFirst.Item3.Item1);
                            var currentAddition = ring.Add(
                                firstCurrentScalar,
                                this.defaultValue);
                            if (this.comparer.Equals(currentAddition, this.defaultValue))
                            {
                                this.elements.RemoveAt(k);
                                --k;
                                --l;
                                --m;
                                --n;
                            }
                            else
                            {
                                currentFirst.Item3.Item1 = currentAddition;
                            }

                            ++k;
                        }

                        while (l < m)
                        {
                            var currentSecond = this.elements[l];
                            var secondCurrentScalar = ring.Multiply(
                                       b,
                                       currentSecond.Item3.Item1);
                            var currentAddition = ring.Add(
                                firstScalar,
                                secondCurrentScalar);
                            if (!this.comparer.Equals(this.defaultValue, additionValue))
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create<long, long, MutableTuple<CoeffType>>(i, currentSecond.Item2, MutableTuple.Create(currentAddition)));

                                // Como um elemento foi inserido, todos os índices terão de ser alterados
                                ++k;
                                ++n;
                                ++l;
                                ++m;
                            }

                            ++l;
                        }
                    }
                    else
                    {
                        // Neste caso os valores por defeito não são ignorados
                        var k = firstValues.Item1;
                        var n = firstValues.Item2;
                        var l = secondValues.Item1;
                        var m = secondValues.Item2;

                        while (k < n && l < m)
                        {
                            var currentFirst = this.elements[k];
                            var currentSecond = this.elements[l];
                            if (currentFirst.Item2 < currentSecond.Item2)
                            {
                                for (var aux = currentFirst.Item2 + 1; aux < currentSecond.Item2; ++aux)
                                {
                                    // A soma não consiste no valor por defeito
                                    this.elements.Insert(
                                        k,
                                        MutableTuple.Create<long, long, MutableTuple<CoeffType>>(i, aux, MutableTuple.Create(additionValue)));

                                    // Como um elemento foi inserido, todos os índices terão de ser alterados
                                    ++k;
                                    ++n;
                                    ++l;
                                    ++m;
                                }

                                var secondCurrentScalar = ring.Multiply(
                                    b,
                                    currentSecond.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstScalar,
                                    secondCurrentScalar);
                                if (!this.comparer.Equals(this.defaultValue, additionValue))
                                {
                                    // A soma não consiste no valor por defeito
                                    this.elements.Insert(
                                        k,
                                        MutableTuple.Create<long, long, MutableTuple<CoeffType>>(i, currentSecond.Item2, MutableTuple.Create(currentAddition)));

                                    // Como um elemento foi inserido, todos os índices terão de ser alterados
                                    ++k;
                                    ++n;
                                    ++l;
                                    ++m;
                                }

                                ++l;
                            }
                            else if (currentFirst.Item2 == currentSecond.Item2)
                            {
                                var firstCurrentScalar = ring.Multiply(
                                    a,
                                    currentFirst.Item3.Item1);
                                var secondCurrentScalar = ring.Multiply(
                                    b,
                                    currentSecond.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstCurrentScalar,
                                    secondCurrentScalar);
                                if (this.comparer.Equals(
                                    currentAddition,
                                    this.defaultValue))
                                {
                                    // É marcado o item para remoção
                                    this.elements.RemoveAt(k);
                                    --k;
                                    --l;
                                    --m;
                                    --n;
                                }
                                else
                                {
                                    currentFirst.Item3.Item1 = additionValue;
                                }
                            }
                            else
                            {
                                for (var aux = currentSecond.Item2 + 1; aux < currentFirst.Item2; ++aux)
                                {
                                    // A soma não consiste no valor por defeito
                                    this.elements.Insert(
                                        k,
                                        MutableTuple.Create<long, long, MutableTuple<CoeffType>>(i, aux, MutableTuple.Create(additionValue)));

                                    // Como um elemento foi inserido, todos os índices terão de ser alterados
                                    ++k;
                                    ++n;
                                    ++l;
                                    ++m;
                                }

                                var firstCurrentScalar = ring.Multiply(
                                    a,
                                    currentFirst.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstCurrentScalar,
                                    this.defaultValue);
                                if (this.comparer.Equals(currentAddition, this.defaultValue))
                                {
                                    this.elements.RemoveAt(k);
                                    --k;
                                    --l;
                                    --m;
                                    --n;
                                }
                                else
                                {
                                    currentFirst.Item3.Item1 = currentAddition;
                                }

                                ++k;
                            }
                        }

                        // Continuação para os termos restantes
                        while (k < n)
                        {
                            var currentFirst = this.elements[k];
                            for (int aux = m; aux < currentFirst.Item2; ++aux)
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create<long, long, MutableTuple<CoeffType>>(i, aux, MutableTuple.Create(additionValue)));

                                // Como um elemento foi inserido, todos os índices terão de ser alterados
                                ++k;
                                ++n;
                                ++l;
                                ++m;
                            }

                            var firstCurrentScalar = ring.Multiply(
                                       a,
                                       currentFirst.Item3.Item1);
                            var currentAddition = ring.Add(
                                firstCurrentScalar,
                                this.defaultValue);
                            if (this.comparer.Equals(currentAddition, this.defaultValue))
                            {
                                this.elements.RemoveAt(k);
                                --l;
                                --m;
                                --n;
                            }
                            else
                            {
                                currentFirst.Item3.Item1 = currentAddition;
                                ++k;
                            }
                        }

                        while (l < m)
                        {
                            var currentSecond = this.elements[l];
                            for (int aux = n; aux < currentSecond.Item2; ++aux)
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create<long, long, MutableTuple<CoeffType>>(i, aux, MutableTuple.Create(additionValue)));

                                // Como um elemento foi inserido, todos os índices terão de ser alterados
                                ++k;
                                ++n;
                                ++l;
                                ++m;
                            }

                            var secondCurrentScalar = ring.Multiply(
                                       b,
                                       currentSecond.Item3.Item1);
                            var currentAddition = ring.Add(
                                firstScalar,
                                secondCurrentScalar);
                            if (!this.comparer.Equals(this.defaultValue, additionValue))
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create<long, long, MutableTuple<CoeffType>>(i, currentSecond.Item2, MutableTuple.Create(currentAddition)));

                                // Como um elemento foi inserido, todos os índices terão de ser alterados
                                ++k;
                                ++n;
                                ++l;
                                ++m;
                            }

                            ++l;
                        }

                        if (m <= n)
                        {
                            for (int aux = k; aux < this.afterLastColumn; ++aux)
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create<long, long, MutableTuple<CoeffType>>(i, aux, MutableTuple.Create(additionValue)));
                                ++k;
                            }
                        }
                        else
                        {
                            for (int aux = l; aux < this.afterLastColumn; ++aux)
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create<long, long, MutableTuple<CoeffType>>(i, aux, MutableTuple.Create(additionValue)));
                                ++k;
                            }
                        }
                    }
                }
                else
                {
                    var firstScalar = ring.Multiply(this.defaultValue, a);
                    var secondScalar = ring.Multiply(this.defaultValue, b);
                    var additionValue = ring.Add(firstScalar, secondScalar);
                    if (!this.comparer.Equals(additionValue, this.defaultValue))
                    {
                        // O valor obtido terá de ser adicionado para todas as colunas
                        for (int k = 0; k < this.afterLastColumn; ++k)
                        {
                            this.elements.Add(MutableTuple.Create<long, long, MutableTuple<CoeffType>>(
                                i,
                                k,
                                MutableTuple.Create(additionValue)));
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
        public void CombineLines(long i, long j, CoeffType a, CoeffType b, IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (i < 0 || i >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else if (i == j)
            {
                var scalar = ring.Add(a, b);
                this.ScalarLineMultiplication(i, scalar, ring);
            }
            else
            {
                var elementsCount = this.elements.Count;
                if (elementsCount > 0)
                {
                    var firstValues = default(Tuple<int, int>);
                    var secondValues = default(Tuple<int, int>);
                    if (i < j)
                    {
                        // A linha a substituir é anterior à linha a ser combinada
                        firstValues = this.FindBothPositions(i, 0, elementsCount);
                        secondValues = this.FindBothPositions(j, firstValues.Item2, elementsCount);
                    }
                    else
                    {
                        // A linha a substituir é posterior à linha a ser combinada
                        secondValues = this.FindBothPositions(j, 0, elementsCount);
                        firstValues = this.FindBothPositions(i, secondValues.Item2, elementsCount);
                    }

                    // Se a combinação de valores por defeito for um valor por defeito, o cálculo simplifica-se
                    var firstScalar = ring.Multiply(this.defaultValue, a);
                    var secondScalar = ring.Multiply(this.defaultValue, b);
                    var additionValue = ring.Add(firstScalar, secondScalar);
                    if (this.comparer.Equals(additionValue, this.defaultValue))
                    {
                        // Neste caso os valores por defeito são ignorados
                        var k = firstValues.Item1;
                        var n = firstValues.Item2;
                        var l = secondValues.Item1;
                        var m = secondValues.Item2;

                        while (k < n && l < m)
                        {
                            var currentFirst = this.elements[k];
                            var currentSecond = this.elements[l];
                            if (currentFirst.Item2 < currentSecond.Item2)
                            {
                                var secondCurrentScalar = ring.Multiply(
                                    b,
                                    currentSecond.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstScalar,
                                    secondCurrentScalar);
                                if (!this.comparer.Equals(this.defaultValue, additionValue))
                                {
                                    // A soma não consiste no valor por defeito
                                    this.elements.Insert(
                                        k,
                                        MutableTuple.Create(i, currentSecond.Item2, MutableTuple.Create(currentAddition)));

                                    // Como um elemento foi inserido, todos os índices terão de ser alterados
                                    ++k;
                                    ++n;
                                    ++l;
                                    ++m;
                                }

                                ++l;
                            }
                            else if (currentFirst.Item2 == currentSecond.Item2)
                            {
                                var firstCurrentScalar = ring.Multiply(
                                    a,
                                    currentFirst.Item3.Item1);
                                var secondCurrentScalar = ring.Multiply(
                                    b,
                                    currentSecond.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstCurrentScalar,
                                    secondCurrentScalar);
                                if (this.comparer.Equals(
                                    currentAddition,
                                    this.defaultValue))
                                {
                                    // É marcado o item para remoção
                                    this.elements.RemoveAt(k);
                                    --k;
                                    --l;
                                    --m;
                                    --n;
                                }
                                else
                                {
                                    currentFirst.Item3.Item1 = additionValue;
                                }
                            }
                            else
                            {
                                var firstCurrentScalar = ring.Multiply(
                                    a,
                                    currentFirst.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstCurrentScalar,
                                    this.defaultValue);
                                if (this.comparer.Equals(currentAddition, this.defaultValue))
                                {
                                    this.elements.RemoveAt(k);
                                    --k;
                                    --l;
                                    --m;
                                    --n;
                                }
                                else
                                {
                                    currentFirst.Item3.Item1 = currentAddition;
                                }

                                ++k;
                            }
                        }

                        // Continuação para os termos restantes
                        while (k < n)
                        {
                            var currentFirst = this.elements[k];
                            var firstCurrentScalar = ring.Multiply(
                                       a,
                                       currentFirst.Item3.Item1);
                            var currentAddition = ring.Add(
                                firstCurrentScalar,
                                this.defaultValue);
                            if (this.comparer.Equals(currentAddition, this.defaultValue))
                            {
                                this.elements.RemoveAt(k);
                                --k;
                                --l;
                                --m;
                                --n;
                            }
                            else
                            {
                                currentFirst.Item3.Item1 = currentAddition;
                            }

                            ++k;
                        }

                        while (l < m)
                        {
                            var currentSecond = this.elements[l];
                            var secondCurrentScalar = ring.Multiply(
                                       b,
                                       currentSecond.Item3.Item1);
                            var currentAddition = ring.Add(
                                firstScalar,
                                secondCurrentScalar);
                            if (!this.comparer.Equals(this.defaultValue, additionValue))
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create(i, currentSecond.Item2, MutableTuple.Create(currentAddition)));

                                // Como um elemento foi inserido, todos os índices terão de ser alterados
                                ++k;
                                ++n;
                                ++l;
                                ++m;
                            }

                            ++l;
                        }
                    }
                    else
                    {
                        // Neste caso os valores por defeito não são ignorados
                        var k = firstValues.Item1;
                        var n = firstValues.Item2;
                        var l = secondValues.Item1;
                        var m = secondValues.Item2;

                        while (k < n && l < m)
                        {
                            var currentFirst = this.elements[k];
                            var currentSecond = this.elements[l];
                            if (currentFirst.Item2 < currentSecond.Item2)
                            {
                                for (var aux = currentFirst.Item2 + 1; aux < currentSecond.Item2; ++aux)
                                {
                                    // A soma não consiste no valor por defeito
                                    this.elements.Insert(
                                        k,
                                        MutableTuple.Create(i, aux, MutableTuple.Create(additionValue)));

                                    // Como um elemento foi inserido, todos os índices terão de ser alterados
                                    ++k;
                                    ++n;
                                    ++l;
                                    ++m;
                                }

                                var secondCurrentScalar = ring.Multiply(
                                    b,
                                    currentSecond.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstScalar,
                                    secondCurrentScalar);
                                if (!this.comparer.Equals(this.defaultValue, additionValue))
                                {
                                    // A soma não consiste no valor por defeito
                                    this.elements.Insert(
                                        k,
                                        MutableTuple.Create(i, currentSecond.Item2, MutableTuple.Create(currentAddition)));

                                    // Como um elemento foi inserido, todos os índices terão de ser alterados
                                    ++k;
                                    ++n;
                                    ++l;
                                    ++m;
                                }

                                ++l;
                            }
                            else if (currentFirst.Item2 == currentSecond.Item2)
                            {
                                var firstCurrentScalar = ring.Multiply(
                                    a,
                                    currentFirst.Item3.Item1);
                                var secondCurrentScalar = ring.Multiply(
                                    b,
                                    currentSecond.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstCurrentScalar,
                                    secondCurrentScalar);
                                if (this.comparer.Equals(
                                    currentAddition,
                                    this.defaultValue))
                                {
                                    // É marcado o item para remoção
                                    this.elements.RemoveAt(k);
                                    --k;
                                    --l;
                                    --m;
                                    --n;
                                }
                                else
                                {
                                    currentFirst.Item3.Item1 = additionValue;
                                }
                            }
                            else
                            {
                                for (var aux = currentSecond.Item2 + 1; aux < currentFirst.Item2; ++aux)
                                {
                                    // A soma não consiste no valor por defeito
                                    this.elements.Insert(
                                        k,
                                        MutableTuple.Create(i, aux, MutableTuple.Create(additionValue)));

                                    // Como um elemento foi inserido, todos os índices terão de ser alterados
                                    ++k;
                                    ++n;
                                    ++l;
                                    ++m;
                                }

                                var firstCurrentScalar = ring.Multiply(
                                    a,
                                    currentFirst.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstCurrentScalar,
                                    this.defaultValue);
                                if (this.comparer.Equals(currentAddition, this.defaultValue))
                                {
                                    this.elements.RemoveAt(k);
                                    --k;
                                    --l;
                                    --m;
                                    --n;
                                }
                                else
                                {
                                    currentFirst.Item3.Item1 = currentAddition;
                                }

                                ++k;
                            }
                        }

                        // Continuação para os termos restantes
                        while (k < n)
                        {
                            var currentFirst = this.elements[k];
                            for (long aux = m; aux < currentFirst.Item2; ++aux)
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create(i, aux, MutableTuple.Create(additionValue)));

                                // Como um elemento foi inserido, todos os índices terão de ser alterados
                                ++k;
                                ++n;
                                ++l;
                                ++m;
                            }

                            var firstCurrentScalar = ring.Multiply(
                                       a,
                                       currentFirst.Item3.Item1);
                            var currentAddition = ring.Add(
                                firstCurrentScalar,
                                this.defaultValue);
                            if (this.comparer.Equals(currentAddition, this.defaultValue))
                            {
                                this.elements.RemoveAt(k);
                                --l;
                                --m;
                                --n;
                            }
                            else
                            {
                                currentFirst.Item3.Item1 = currentAddition;
                                ++k;
                            }
                        }

                        while (l < m)
                        {
                            var currentSecond = this.elements[l];
                            for (long aux = n; aux < currentSecond.Item2; ++aux)
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create(i, aux, MutableTuple.Create(additionValue)));

                                // Como um elemento foi inserido, todos os índices terão de ser alterados
                                ++k;
                                ++n;
                                ++l;
                                ++m;
                            }

                            var secondCurrentScalar = ring.Multiply(
                                       b,
                                       currentSecond.Item3.Item1);
                            var currentAddition = ring.Add(
                                firstScalar,
                                secondCurrentScalar);
                            if (!this.comparer.Equals(this.defaultValue, additionValue))
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create(i, currentSecond.Item2, MutableTuple.Create(currentAddition)));

                                // Como um elemento foi inserido, todos os índices terão de ser alterados
                                ++k;
                                ++n;
                                ++l;
                                ++m;
                            }

                            ++l;
                        }

                        if (m <= n)
                        {
                            for (long aux = k; aux < this.afterLastColumn; ++aux)
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create(i, aux, MutableTuple.Create(additionValue)));
                                ++k;
                            }
                        }
                        else
                        {
                            for (long aux = l; aux < this.afterLastColumn; ++aux)
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create(i, aux, MutableTuple.Create(additionValue)));
                                ++k;
                            }
                        }
                    }
                }
                else
                {
                    var firstScalar = ring.Multiply(this.defaultValue, a);
                    var secondScalar = ring.Multiply(this.defaultValue, b);
                    var additionValue = ring.Add(firstScalar, secondScalar);
                    if (!this.comparer.Equals(additionValue, this.defaultValue))
                    {
                        // O valor obtido terá de ser adicionado para todas as colunas
                        for (var k = 0L; k < this.afterLastColumn; ++k)
                        {
                            this.elements.Add(MutableTuple.Create(
                                i,
                                k,
                                MutableTuple.Create(additionValue)));
                        }
                    }
                }
            }
        }
    }
}
