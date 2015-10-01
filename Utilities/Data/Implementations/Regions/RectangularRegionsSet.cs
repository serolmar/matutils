namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Collections;

    /// <summary>
    /// Define um conjunto de regiões rectangulares que não se intersectam.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as coordenadas das regiões.</typeparam>
    /// <typeparam name="R">O tipo dos objectos que constituem as regiões.</typeparam>
    public class NonIntersectingMergingRegionsSet<T, R> : IEnumerable<R>
        where R : IMergingRegion<T>
    {
        /// <summary>
        /// Mantém o comparador de coordenadas.
        /// </summary>
        protected IComparer<T> coordinatesComparer;

        /// <summary>
        /// Mantém a lista de todas as arestas verticais adjacentes aos rectângulos.
        /// </summary>
        protected List<MutableTuple<T, SortedSet<int>>> xEdges;

        /// <summary>
        /// Mantém a lista de todas as arestas horizontais adjacentes aos rectângulos.
        /// </summary>
        protected List<MutableTuple<T, SortedSet<int>>> yEdges;

        /// <summary>
        /// Mantém a lista de todos os rectângulos.
        /// </summary>
        protected List<R> mergingRegions;

        /// <summary>
        /// Instancia um nova instância de objectos do tipo <see cref="NonIntersectingMergingRegionsSet{T, R}"/>.
        /// </summary>
        public NonIntersectingMergingRegionsSet()
        {
            this.coordinatesComparer = Comparer<T>.Default;
            this.xEdges = new List<MutableTuple<T, SortedSet<int>>>();
            this.yEdges = new List<MutableTuple<T, SortedSet<int>>>();
            this.mergingRegions = new List<R>();
        }

        /// <summary>
        /// Instancia um nova instância de objectos do tipo <see cref="NonIntersectingMergingRegionsSet{T, R}"/>.
        /// </summary>
        /// <param name="coordinatesComparer">O comparador de coordenadas.</param>
        public NonIntersectingMergingRegionsSet(
            IComparer<T> coordinatesComparer)
        {
            if (coordinatesComparer == null)
            {
                throw new ArgumentNullException("coordinatesComparer");
            }
            else
            {
                this.coordinatesComparer = coordinatesComparer;
                this.mergingRegions = new List<R>();
            }
        }

        /// <summary>
        /// Obtém a contagem das regiões rectangulares registadas.
        /// </summary>
        public int Count
        {
            get
            {
                return this.mergingRegions.Count;
            }
        }

        /// <summary>
        /// Adiciona uma região rectangular ao conjunto de regiões.
        /// </summary>
        /// <param name="mergingRegion">A região a ser adicionada.</param>
        public void Add(R mergingRegion)
        {
            if (mergingRegion == null)
            {
                throw new ArgumentNullException("mergingRegion");
            }
            else if (this.coordinatesComparer != mergingRegion.Comparer)
            {
                throw new UtilitiesDataException(
                    "The comparer provided in merging region doesn't match the current comparer.");
            }
            else
            {
                var xEdgesCount = this.xEdges.Count;
                if (xEdgesCount == 0)
                {
                    this.InternalAddFirstRegion(mergingRegion);
                }
                else
                {
                    var firstX = this.FindEdge(
                        mergingRegion.TopLeftX,
                        0,
                        this.xEdges.Count,
                        this.xEdges);

                    if (firstX < this.xEdges.Count) // A primeira aresta encontra-se no interior da lista
                    {
                        var mergedRegionsCount = this.mergingRegions.Count;
                        var yEdgesCount = this.yEdges.Count;

                        // Procura-se pela primeira aresta
                        var firstY = this.FindEdge(
                            mergingRegion.TopLeftY,
                            0,
                            yEdgesCount,
                            this.yEdges);
                        if (firstY < yEdgesCount)
                        {
                            this.InternalAddRegion(firstX, firstY, mergingRegion);
                        }
                        else // É seguro adicionar a região
                        {
                            this.InternalAddRegion(firstX, mergingRegion);
                        }
                    }
                    else // Adiciona-se a região uma vez que a primeira aresta X cai fora da lista
                    {
                        this.InternalAddRegion(mergingRegion);
                    }
                }
            }
        }

        /// <summary>
        /// Remove a região especificada do conjunto de regiões.
        /// </summary>
        /// <remarks>
        /// Se a região indicada não existir, mesmo que esta se encontre completamente no interior de
        /// uma outra regiõa, então não será eliminada.
        /// </remarks>
        /// <param name="mergingRegion">A região a ser removida.</param>
        /// <returns>Verdadeiro caso a região tenha sido eliminada e falso caso contrário.</returns>
        public bool RemoveRegion(IMergingRegion<T> mergingRegion)
        {
            if (mergingRegion == null)
            {
                return false;
            }
            else
            {
                var xEdgesCount = this.xEdges.Count;
                var firstX = this.FindEdge(
                    mergingRegion.TopLeftX,
                    0,
                    xEdgesCount,
                    this.xEdges);
                if (firstX < xEdgesCount)
                {
                    var currentFirstX = this.xEdges[firstX];
                    if (this.coordinatesComparer.Compare(
                        mergingRegion.TopLeftX,
                        currentFirstX.Item1) == 0)
                    {
                        var yEdgesCount = this.yEdges.Count;
                        var firstY = this.FindEdge(
                            mergingRegion.TopLeftY,
                            0,
                            yEdgesCount,
                            this.yEdges);

                        if (firstY < yEdgesCount)
                        {
                            var currentFirstY = this.yEdges[firstY];
                            if (this.coordinatesComparer.Compare(
                                mergingRegion.TopLeftY,
                                currentFirstY.Item1) == 0)
                            {
                                var secondX = firstX;
                                if (this.coordinatesComparer.Compare(
                                    mergingRegion.TopLeftX,
                                    mergingRegion.BottomRightX) < 0)
                                {
                                    secondX = this.FindEdge(
                                        mergingRegion.BottomRightX,
                                        firstX + 1,
                                        xEdgesCount,
                                        this.xEdges);
                                }

                                if (secondX < xEdgesCount)
                                {
                                    var currentSecondX = this.xEdges[secondX];
                                    if (this.coordinatesComparer.Compare(
                                        mergingRegion.BottomRightX,
                                        currentSecondX.Item1) == 0)
                                    {
                                        var secondY = firstY;
                                        if (this.coordinatesComparer.Compare(
                                            mergingRegion.TopLeftY,
                                            mergingRegion.BottomRightY) < 0)
                                        {
                                            secondY = this.FindEdge(
                                                mergingRegion.BottomRightY,
                                                firstY + 1,
                                                yEdgesCount,
                                                this.yEdges);
                                        }

                                        if (secondY < yEdgesCount)
                                        {
                                            var currentSecondY = this.yEdges[secondY];
                                            if (this.coordinatesComparer.Compare(
                                                mergingRegion.BottomRightY,
                                                currentSecondY.Item1) == 0)
                                            {
                                                // Neste ponto todas as arestas foram encontradas
                                                var firstEnumX = currentFirstX.Item2.GetEnumerator();
                                                var secondEnumX = currentSecondX.Item2.GetEnumerator();
                                                var firstEnumY = currentFirstY.Item2.GetEnumerator();
                                                var secondEnumY = currentSecondY.Item2.GetEnumerator();
                                                var state = firstEnumX.MoveNext() && secondEnumX.MoveNext()
                                                    && firstEnumY.MoveNext() && secondEnumY.MoveNext();
                                                while (state)
                                                {
                                                    // Determinar o valor mais alto entre os quatro proporcionados
                                                    var max = firstEnumX.Current;
                                                    if (max < secondEnumX.Current)
                                                    {
                                                        max = secondEnumX.Current;
                                                        var innerState = firstEnumX.MoveNext();
                                                        while (innerState)
                                                        {
                                                            innerState = firstEnumX.Current < max;
                                                            innerState &= firstEnumX.MoveNext();
                                                        }

                                                        state = innerState;
                                                    }
                                                    else if (secondEnumX.Current < max)
                                                    {
                                                        var innerState = secondEnumX.MoveNext();
                                                        while (innerState)
                                                        {
                                                            innerState = secondEnumX.Current < max;
                                                            innerState &= secondEnumX.MoveNext();
                                                        }

                                                        state = innerState;
                                                    }

                                                    if (max < firstEnumY.Current)
                                                    {
                                                        var innerState = firstEnumX.MoveNext();
                                                        while (innerState)
                                                        {
                                                            innerState = firstEnumX.Current < max;
                                                            innerState &= firstEnumX.MoveNext();
                                                        }

                                                        if (innerState)
                                                        {
                                                            innerState = secondEnumX.MoveNext();
                                                            while (innerState)
                                                            {
                                                                innerState = secondEnumX.Current < max;
                                                                innerState &= secondEnumX.MoveNext();
                                                            }
                                                        }

                                                        state = innerState;
                                                    }
                                                    else if (firstEnumY.Current < max)
                                                    {
                                                        var innerState = firstEnumY.MoveNext();
                                                        while (innerState)
                                                        {
                                                            innerState = firstEnumY.Current < max;
                                                            innerState &= firstEnumY.MoveNext();
                                                        }

                                                        state = innerState;
                                                    }

                                                    if (max < secondEnumY.Current)
                                                    {
                                                        var innerState = firstEnumX.MoveNext();
                                                        while (innerState)
                                                        {
                                                            innerState = firstEnumX.Current < max;
                                                            innerState &= firstEnumX.MoveNext();
                                                        }

                                                        if (innerState)
                                                        {
                                                            innerState = secondEnumX.MoveNext();
                                                            while (innerState)
                                                            {
                                                                innerState = secondEnumX.Current < max;
                                                                innerState &= secondEnumX.MoveNext();
                                                            }

                                                            if (innerState)
                                                            {
                                                                innerState = firstEnumY.MoveNext();
                                                                while (innerState)
                                                                {
                                                                    innerState = firstEnumY.Current < max;
                                                                    innerState &= firstEnumY.MoveNext();
                                                                }
                                                            }
                                                        }

                                                        state = innerState;

                                                    }
                                                    else if (secondEnumY.Current < max)
                                                    {
                                                        var innerState = secondEnumY.MoveNext();
                                                        while (innerState)
                                                        {
                                                            innerState = secondEnumY.Current < max;
                                                            innerState &= secondEnumY.MoveNext();
                                                        }
                                                    }

                                                    // Comparação dos valores
                                                    if (firstEnumX.Current == max && secondEnumX.Current == max
                                                        && firstEnumY.Current == max && secondEnumY.Current == max)
                                                    {
                                                        // É possível remover a região
                                                        this.mergingRegions.RemoveAt(max);
                                                        currentFirstX.Item2.Remove(max);
                                                        currentFirstY.Item2.Remove(max);
                                                        currentSecondX.Item2.Remove(max);
                                                        currentSecondY.Item2.Remove(max);

                                                        if (currentFirstX.Item2.Count == 0)
                                                        {
                                                            this.xEdges.Remove(currentFirstX);
                                                        }

                                                        if (currentSecondX.Item2.Count == 0)
                                                        {
                                                            this.xEdges.Remove(currentSecondX);
                                                        }

                                                        if (currentFirstY.Item2.Count == 0)
                                                        {
                                                            this.yEdges.Remove(currentFirstY);
                                                        }

                                                        if (currentSecondY.Item2.Count == 0)
                                                        {
                                                            this.yEdges.Remove(currentSecondY);
                                                        }

                                                        // É necessário actualizar os índices
                                                        this.UpdateIndexAfterRemove(this.xEdges, max);
                                                        this.UpdateIndexAfterRemove(this.yEdges, max);

                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Obtém a região pertencente ao conjunto que contém a célula especificada.
        /// </summary>
        /// <param name="x">A primeira coordenada da célula.</param>
        /// <param name="y">A segunda coordenada da célula.</param>
        /// <returns>A região que envolve a célula se existir e nulo caso não exista.</returns>
        public IMergingRegion<T> GetMergingRegionForCell(T x, T y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            else if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            else
            {
                var xEdgesCount = this.xEdges.Count;
                var firstX = this.FindEdge(
                    x,
                    0,
                    xEdgesCount,
                    this.xEdges);
                if (firstX < xEdgesCount)
                {
                    var yEdgesCount = this.yEdges.Count;
                    var firstY = this.FindEdge(
                        y,
                        0,
                        yEdgesCount,
                        this.yEdges);
                    if (firstY < yEdgesCount)
                    {
                        var foundIntersection = -1;
                        var xCandidates = this.GetXCandiadates(firstX, firstX, x, x);
                        var yCandidates = this.GetYCandiadates(firstY, firstY, y, y);

                        var xCandidatesEnum = xCandidates.GetEnumerator();
                        var yCandidateEnum = yCandidates.GetEnumerator();
                        var state = xCandidatesEnum.MoveNext() && yCandidateEnum.MoveNext();
                        while (state)
                        {
                            var currentX = xCandidatesEnum.Current;
                            var currentY = yCandidateEnum.Current;
                            if (currentX < currentY)
                            {
                                state = xCandidatesEnum.MoveNext();
                            }
                            else if (currentX == currentY)
                            {
                                foundIntersection = currentX;
                                state = false;
                            }
                            else
                            {
                                state = yCandidateEnum.MoveNext();
                            }
                        }

                        if (foundIntersection == -1)
                        {
                            return null;
                        }
                        else
                        {
                            return this.mergingRegions[foundIntersection];
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Obtém todas as regiões do conjunto que possuem intersecção não vazia
        /// com a região proporcionada.
        /// </summary>
        /// <param name="mergingRegion">A região.</param>
        /// <returns>A lista de regiões que intersectam a região proporcionada.</returns>
        public List<R> GetIntersectingRegionsFor(IMergingRegion<T> mergingRegion)
        {
            var result = new List<R>();
            if (mergingRegion == null)
            {
                return result;
            }
            else
            {
                var intersectionIndices = this.GetIntersectingRegionsIndices(mergingRegion);
                var length = intersectionIndices.Count;
                foreach (var item in intersectionIndices)
                {
                    result.Add(this.mergingRegions[item]);
                }

                return result;
            }
        }

        /// <summary>
        /// Remove todas as regiões que se intersectam com a região proporcionada.
        /// </summary>
        /// <param name="mergingRegion">A região.</param>
        /// <returns>A lista de regiões que foram removidas.</returns>
        public List<R> RemoveIntersectingRegionsWith(IMergingRegion<T> mergingRegion)
        {
            var result = new List<R>();
            if (mergingRegion == null)
            {
                return result;
            }
            else
            {
                var xEdgesCount = this.xEdges.Count;
                var yEdgesCount = this.yEdges.Count;
                var intersectionIndices = this.GetIntersectingRegionsIndices(mergingRegion);
                var length = intersectionIndices.Count;
                var offset = 0;
                foreach (var item in intersectionIndices)
                {
                    var innerItem = item - offset;
                    result.Add(this.mergingRegions[innerItem]);
                    this.mergingRegions.RemoveAt(innerItem);
                    for (int i = 0; i < xEdgesCount; ++i)
                    {
                        this.xEdges[i].Item2.Remove(item);
                    }

                    for (int i = 0; i < yEdgesCount; ++i)
                    {
                        this.yEdges[i].Item2.Remove(item);
                    }

                    ++offset;
                }

                // Actualiza todos os índices após a remoção
                var index = 0;
                while (index < xEdgesCount)
                {
                    var current = this.xEdges[index];
                    if (current.Item2.Count == 0)
                    {
                        this.xEdges.RemoveAt(index);
                        --xEdgesCount;
                    }
                    else
                    {
                        ++index;
                    }
                }

                index = 0;
                while (index < yEdgesCount)
                {
                    var current = this.yEdges[index];
                    if (current.Item2.Count == 0)
                    {
                        this.yEdges.RemoveAt(index);
                        --yEdgesCount;
                    }
                    else
                    {
                        ++index;
                    }
                }

                this.UpdateIndexAfterRemove(this.xEdges, intersectionIndices);
                this.UpdateIndexAfterRemove(this.yEdges, intersectionIndices);

                return result;
            }
        }

        /// <summary>
        /// Obtém um enumerador para o conjunto de regiões rectangulares.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<R> GetEnumerator()
        {
            return this.mergingRegions.GetEnumerator();
        }

        /// <summary>
        /// Obtém o enumerador não genérico para o conjunto de regiões rectangulares.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #region Funções protegidas

        /// <summary>
        /// Procura uma aresta na lista de arestas especificada.
        /// </summary>
        /// <param name="edgeValue">O valor da aresta.</param>
        /// <param name="startIndex">O índice inicial da procura.</param>
        /// <param name="endIndex">O índicie final da procura.</param>
        /// <param name="edges">A lista ordenada de arestas.</param>
        /// <returns>O índice onde se encontra a aresta ou onde esta poderá ser inserida.</returns>
        protected int FindEdge(
            T edgeValue,
            int startIndex,
            int endIndex,
            List<MutableTuple<T, SortedSet<int>>> edges)
        {
            if (this.coordinatesComparer.Compare(
                edgeValue,
                edges[startIndex].Item1) <= 0)
            {
                return startIndex;
            }
            else if (this.coordinatesComparer.Compare(
                edgeValue,
                edges[endIndex - 1].Item1) > 0)
            {
                return endIndex;
            }
            else
            {
                // Efectua o resto da pesquisa
                var innerLow = startIndex;
                var innerHigh = endIndex;
                while (innerLow < innerHigh)
                {
                    int sum = innerHigh + innerLow;
                    int intermediaryIndex = sum >> 1;
                    if ((sum & 1) == 0)
                    {
                        var intermediaryEdge = edges[intermediaryIndex].Item1;
                        if (this.coordinatesComparer.Compare(
                            edgeValue,
                            intermediaryEdge) == 0)
                        {
                            return intermediaryIndex;
                        }
                        else if (this.coordinatesComparer.Compare(
                            edgeValue,
                            intermediaryEdge) < 0)
                        {
                            innerHigh = intermediaryIndex;
                        }
                        else
                        {
                            innerLow = intermediaryIndex;
                        }
                    }
                    else
                    {
                        var intermediaryEdge = edges[intermediaryIndex].Item1;
                        var nextIntermediaryEdge = edges[intermediaryIndex + 1].Item1;
                        if (
                            this.coordinatesComparer.Compare(edgeValue, intermediaryEdge) > 0 &&
                            this.coordinatesComparer.Compare(edgeValue, nextIntermediaryEdge) <= 0)
                        {
                            return intermediaryIndex + 1;
                        }
                        else if (
                            this.coordinatesComparer.Compare(edgeValue, intermediaryEdge) == 0)
                        {
                            return intermediaryIndex;
                        }
                        else if (this.coordinatesComparer.Compare(edgeValue, intermediaryEdge) > 0)
                        {
                            innerLow = intermediaryIndex;
                        }
                        else
                        {
                            innerHigh = intermediaryIndex;
                        }
                    }
                }

                return innerLow;
            }
        }

        /// <summary>
        /// Obtém os índices das regições que se intersectam com a região proporcionada.
        /// </summary>
        /// <param name="mergingRegion">A região a ser testada.</param>
        /// <returns>A lista os índices.</returns>
        protected IntegerSequence GetIntersectingRegionsIndices(IMergingRegion<T> mergingRegion)
        {
            var result = new IntegerSequence();
            var xEdgesCount = this.xEdges.Count;
            if (xEdgesCount > 0)
            {
                var firstX = this.FindEdge(
                    mergingRegion.TopLeftX,
                    0,
                    xEdgesCount,
                    this.xEdges);

                // Se a primeira aresta for superior ao número de elementos indexados então
                // a região a ser considerada não intersecta nenhuma das existentes.
                if (firstX < xEdgesCount)
                {
                    var secondX = firstX;
                    if (this.coordinatesComparer.Compare(
                        mergingRegion.TopLeftX,
                        mergingRegion.BottomRightX) < 0)
                    {
                        secondX = this.FindEdge(
                            mergingRegion.BottomRightX,
                            firstX,
                            xEdgesCount,
                            this.xEdges);
                    }

                    // Se o item de maior ordem for inferior a todos os outros não se dá intersecção.
                    if (secondX != 0
                        || this.coordinatesComparer.Compare(
                        this.xEdges[secondX].Item1,
                        mergingRegion.BottomRightX) == 0)
                    {
                        var yEdgesCount = this.yEdges.Count;
                        var firstY = this.FindEdge(
                            mergingRegion.TopLeftY,
                            0,
                            yEdgesCount,
                            this.yEdges);
                        if (firstY < yEdgesCount)
                        {
                            var secondY = firstY;
                            if (this.coordinatesComparer.Compare(
                                mergingRegion.TopLeftY,
                                mergingRegion.BottomRightY) < 0)
                            {
                                secondY = this.FindEdge(
                                    mergingRegion.BottomRightY,
                                    firstY,
                                    yEdgesCount,
                                    this.yEdges);
                            }

                            if (secondY != 0
                                || this.coordinatesComparer.Compare(
                                this.yEdges[secondY].Item1,
                                mergingRegion.BottomRightY) == 0)
                            {
                                // Apenas neste ponto se poderão encontrar intersecções
                                var xCands = this.GetXCandiadates(
                                    firstX, 
                                    secondX, 
                                    mergingRegion.TopLeftX, 
                                    mergingRegion.BottomRightX);
                                var yCands = this.GetYCandiadates(
                                    firstY,
                                     secondY,
                                     mergingRegion.TopLeftY,
                                     mergingRegion.BottomRightY);
                                this.FillIntersectingIndices(xCands, yCands, result);
                            }
                        }
                    }
                }
            }

            return result;
        }

        #endregion Funções protegidas

        #region Funções para testes

        /// <summary>
        /// Assevera a integridade do conjunto após as operações.
        /// </summary>
        /// <remarks>
        /// Esta função serve apenas para propósitos de testes.
        /// </remarks>
        internal void AssertIntegrity()
        {
            // Integridade dos índices
            this.AssertIndexIntegrity(
                this.xEdges,
                t => t.TopLeftX,
                b => b.BottomRightX);
            this.AssertIndexIntegrity(
                this.yEdges,
                t => t.TopLeftY,
                b => b.BottomRightY);
        }

        /// <summary>
        /// Assevera a integridade dos índices.
        /// </summary>
        /// <param name="edges">A lista de índices a ser analisada.</param>
        /// <param name="topValue">A função que permite obter o valor de topo da região.</param>
        /// <param name="bottomValue">A função que permite obter o valor de fundo da região.</param>
        private void AssertIndexIntegrity(
            List<MutableTuple<T, SortedSet<int>>> edges,
            Func<R, T> topValue,
            Func<R, T> bottomValue)
        {
            var mergingRegionsCount = this.mergingRegions.Count;
            var edgesCount = edges.Count;
            var i = 0;
            if (i < edgesCount)
            {
                var currentEdge = edges[i];
                var currentEdgeValue = currentEdge.Item1;
                foreach (var item in currentEdge.Item2)
                {
                    if (item < mergingRegionsCount)
                    {
                        var currentMergingRegion = this.mergingRegions[item];
                        if (this.coordinatesComparer.Compare(
                            topValue.Invoke(currentMergingRegion),
                            currentEdgeValue) != 0
                            && this.coordinatesComparer.Compare(
                            bottomValue.Invoke(currentMergingRegion),
                            currentEdgeValue) != 0)
                        {
                            throw new Exception("Found one edge which value doesn't match any of vertical merging region edges.");
                        }
                    }
                    else
                    {
                        throw new Exception("Found an index outside of bounds of merging regions list.");
                    }
                }

                ++i;
                while (i < edgesCount)
                {
                    currentEdge = edges[i];
                    var tempEdgeValue = currentEdge.Item1;
                    if (this.coordinatesComparer.Compare(
                        currentEdgeValue,
                        tempEdgeValue) < 0)
                    {
                        currentEdgeValue = tempEdgeValue;
                        foreach (var item in currentEdge.Item2)
                        {
                            if (item < mergingRegionsCount)
                            {
                                var currentMergingRegion = this.mergingRegions[item];
                                if (this.coordinatesComparer.Compare(
                                    topValue.Invoke(currentMergingRegion),
                                    currentEdgeValue) != 0
                                    && this.coordinatesComparer.Compare(
                                    bottomValue.Invoke(currentMergingRegion),
                                    currentEdgeValue) != 0)
                                {
                                    throw new Exception("Found one edge which value doesn't match any of vertical merging region edges.");
                                }
                            }
                            else
                            {
                                throw new Exception("Found an index outside of bounds of merging regions list.");
                            }
                        }

                        ++i;
                    }
                    else
                    {
                        throw new Exception("Found an index out of order.");
                    }
                }
            }
        }


        #endregion Funções para testes

        #region Funções privadas

        /// <summary>
        /// Função auxiliar que permite adicionar a primeira região ao conjunto.
        /// </summary>
        /// <param name="mergingRegion">A região a ser adicionada.</param>
        private void InternalAddFirstRegion(R mergingRegion)
        {
            this.xEdges.Add(Tuple.Create(
                        mergingRegion.TopLeftX,
                        new SortedSet<int>() { 0 }));
            if (this.coordinatesComparer.Compare(
                mergingRegion.TopLeftX,
                mergingRegion.BottomRightX) < 0)
            {
                this.xEdges.Add(Tuple.Create(
                    mergingRegion.BottomRightX,
                    new SortedSet<int>() { 0 }));
            }

            this.yEdges.Add(Tuple.Create(
                mergingRegion.TopLeftY,
                new SortedSet<int>() { 0 }));
            if (this.coordinatesComparer.Compare(
                mergingRegion.TopLeftY,
                mergingRegion.BottomRightY) < 0)
            {
                this.yEdges.Add(Tuple.Create(
                    mergingRegion.BottomRightY,
                    new SortedSet<int>() { 0 }));
            }

            this.mergingRegions.Add(mergingRegion);
        }

        /// <summary>
        /// Função auxiliar que permite adicioanr uma região ao conjunto, sabendo que esta
        /// nunca irá intersectar nenhuma das outras.
        /// </summary>
        /// <param name="mergingRegion">A região a ser adicionada.</param>
        private void InternalAddRegion(R mergingRegion)
        {
            var mergedRegionCount = this.mergingRegions.Count;
            this.xEdges.Add(Tuple.Create(
            mergingRegion.TopLeftX,
            new SortedSet<int>() { mergedRegionCount }));
            if (this.coordinatesComparer.Compare(
                mergingRegion.TopLeftX,
                mergingRegion.BottomRightX) < 0)
            {
                this.xEdges.Add(Tuple.Create(
                    mergingRegion.BottomRightX,
                    new SortedSet<int>() { mergedRegionCount }));
            }

            var yEdgesCount = this.yEdges.Count;
            var findY = this.FindEdge(
                mergingRegion.TopLeftY,
                0,
                yEdgesCount,
                this.yEdges);
            if (findY < yEdgesCount)
            {
                var current = this.yEdges[findY];

                // A aresta já existe existe
                if (this.coordinatesComparer.Compare(
                    mergingRegion.TopLeftY,
                    current.Item1) == 0)
                {
                    current.Item2.Add(mergedRegionCount);
                }
                else // A aresta não existe
                {
                    this.yEdges.Insert(
                        findY,
                        Tuple.Create(mergingRegion.TopLeftY, new SortedSet<int>() { mergedRegionCount }));
                }

                if (this.coordinatesComparer.Compare(
                    mergingRegion.TopLeftY,
                    mergingRegion.BottomRightY) < 0)
                {
                    // A segunda aresta terá de ser adicionada
                    findY = this.FindEdge(
                        mergingRegion.BottomRightY,
                        findY,
                        yEdgesCount,
                        this.yEdges);
                    if (findY < yEdgesCount)
                    {
                        current = this.yEdges[findY];

                        // A aresta já existe existe
                        if (this.coordinatesComparer.Compare(
                            mergingRegion.BottomRightY,
                            current.Item1) == 0)
                        {
                            current.Item2.Add(mergedRegionCount);
                        }
                        else // A aresta não existe
                        {
                            this.yEdges.Insert(
                                findY,
                                Tuple.Create(mergingRegion.BottomRightY, new SortedSet<int>() { mergedRegionCount }));
                        }
                    }
                    else
                    {
                        this.yEdges.Add(
                            Tuple.Create(mergingRegion.BottomRightY,
                            new SortedSet<int> { mergedRegionCount }));
                    }
                }
            }
            else
            {
                this.yEdges.Add(
                    Tuple.Create(mergingRegion.TopLeftY,
                    new SortedSet<int> { mergedRegionCount }));
                if (this.coordinatesComparer.Compare(
                    mergingRegion.TopLeftY,
                    mergingRegion.BottomRightY) < 0)
                {
                    this.yEdges.Add(
                        Tuple.Create(mergingRegion.BottomRightY,
                        new SortedSet<int> { mergedRegionCount }));
                }
            }

            this.mergingRegions.Add(mergingRegion);
        }

        /// <summary>
        /// Função auxiliar que permite adicionar uma região ao conjunto, sabendo que esta
        /// nunca irá intersectar nenhuma das outras e a posição da sua primeira aresta é conhecida.
        /// </summary>
        /// <param name="firstX">A posição da primeira aresta.</param>
        /// <param name="mergingRegion">A região a ser adicionada.</param>
        private void InternalAddRegion(
            int firstX,
            R mergingRegion)
        {
            var mergedRegionsCount = this.mergingRegions.Count;
            var xEdgesCount = this.xEdges.Count;
            var yEdgesCount = this.yEdges.Count;
            var current = this.xEdges[firstX];
            if (this.coordinatesComparer.Compare(
                mergingRegion.TopLeftX,
                current.Item1) == 0)
            {
                current.Item2.Add(mergedRegionsCount);
            }
            else
            {
                this.xEdges.Insert(
                    firstX,
                    Tuple.Create(mergingRegion.TopLeftX, new SortedSet<int>() { mergedRegionsCount }));
            }

            // Adiciona a segunda aresta vertical
            if (this.coordinatesComparer.Compare(
                mergingRegion.TopLeftX,
                mergingRegion.BottomRightX) < 0)
            {
                var secondX = this.FindEdge(
                    mergingRegion.BottomRightX,
                    firstX,
                    xEdgesCount,
                    this.xEdges);
                if (secondX < xEdgesCount)
                {
                    current = this.xEdges[secondX];
                    if (this.coordinatesComparer.Compare(
                        mergingRegion.BottomRightX,
                        current.Item1) == 0)
                    {
                        current.Item2.Add(mergedRegionsCount);
                    }
                    else
                    {
                        this.xEdges.Insert(
                            secondX,
                            Tuple.Create(mergingRegion.BottomRightX, new SortedSet<int>() { mergedRegionsCount }));
                    }
                }
                else
                {
                    this.xEdges.Add(Tuple.Create(
                        mergingRegion.BottomRightX,
                        new SortedSet<int>() { mergedRegionsCount }));
                }
            }

            this.yEdges.Add(Tuple.Create(
                mergingRegion.TopLeftY,
                new SortedSet<int>() { mergedRegionsCount }));
            if (this.coordinatesComparer.Compare(
                    mergingRegion.TopLeftY,
                    mergingRegion.BottomRightY) < 0)
            {
                this.yEdges.Add(Tuple.Create(
                    mergingRegion.BottomRightY,
                    new SortedSet<int>() { mergedRegionsCount }));
            }

            this.mergingRegions.Add(mergingRegion);
        }


        /// <summary>
        /// Função auxiliar que permite adicionar a região, sabendo que ambas as primeiras areastas
        /// se encontram no interior das respectivas listas.
        /// </summary>
        /// <param name="firstX">A posição da primeira aresta.</param>
        /// <param name="firstY">A posição da segunda aresta.</param>
        /// <param name="mergingRegion">A região a ser adicionada.</param>
        private void InternalAddRegion(
            int firstX,
            int firstY,
            R mergingRegion)
        {
            var mergedRegionsCount = this.mergingRegions.Count;
            var xEdgesCount = this.xEdges.Count;
            var yEdgesCount = this.yEdges.Count;
            var secondX = firstX;
            if (this.coordinatesComparer.Compare(
                mergingRegion.TopLeftX,
                mergingRegion.BottomRightX) < 0)
            {
                secondX = this.FindEdge(
                    mergingRegion.BottomRightX,
                    firstX,
                    xEdgesCount,
                    this.xEdges);
            }

            var secondY = firstY;
            if (this.coordinatesComparer.Compare(
                mergingRegion.TopLeftY,
                mergingRegion.BottomRightY) < 0)
            {
                secondY = this.FindEdge(
                    mergingRegion.BottomRightY,
                    firstY,
                    yEdgesCount,
                    this.yEdges);
            }

            var foundIntersection = true;

            // É possível não se dar intersecção neste ponto
            var xCandidates = this.GetXCandiadates(firstX, secondX, mergingRegion.TopLeftX, mergingRegion.BottomRightX);
            var yCandidates = this.GetYCandiadates(firstY, secondY, mergingRegion.TopLeftY, mergingRegion.BottomRightY);

            var xCandidatesEnum = xCandidates.GetEnumerator();
            var yCandidateEnum = yCandidates.GetEnumerator();
            var state = xCandidatesEnum.MoveNext() && yCandidateEnum.MoveNext();
            foundIntersection = false;
            while (state)
            {
                var x = xCandidatesEnum.Current;
                var y = yCandidateEnum.Current;
                if (x < y)
                {
                    state = xCandidatesEnum.MoveNext();
                }
                else if (x == y)
                {
                    foundIntersection = true;
                    state = false;
                }
                else
                {
                    state = yCandidateEnum.MoveNext();
                }
            }

            if (foundIntersection)
            {
                throw new UtilitiesException("The provided region intersects at least one of the existing regions.");
            }
            else
            {
                if (this.coordinatesComparer.Compare(
                       mergingRegion.TopLeftX,
                       this.xEdges[firstX].Item1) == 0)
                {
                    this.xEdges[firstX].Item2.Add(mergedRegionsCount);
                }
                else
                {
                    this.xEdges.Insert(firstX, Tuple.Create(
                        mergingRegion.TopLeftX,
                        new SortedSet<int>() { mergedRegionsCount }));
                    ++secondX;
                    ++xEdgesCount;
                }

                if (secondX < xEdgesCount && this.coordinatesComparer.Compare(
                    mergingRegion.BottomRightX,
                    this.xEdges[secondX].Item1) == 0)
                {
                    this.xEdges[secondX].Item2.Add(mergedRegionsCount);
                }
                else
                {
                    this.xEdges.Insert(secondX, Tuple.Create(
                        mergingRegion.BottomRightX,
                        new SortedSet<int>() { mergedRegionsCount }));
                }

                if (this.coordinatesComparer.Compare(
                    mergingRegion.TopLeftY,
                    this.yEdges[firstY].Item1) == 0)
                {
                    this.yEdges[firstY].Item2.Add(mergedRegionsCount);
                }
                else
                {
                    this.yEdges.Insert(firstY, Tuple.Create(
                        mergingRegion.TopLeftY,
                        new SortedSet<int>() { mergedRegionsCount }));
                    ++secondY;
                    ++yEdgesCount;
                }

                if (secondY < yEdgesCount && this.coordinatesComparer.Compare(
                    mergingRegion.BottomRightY,
                    this.yEdges[secondY].Item1) == 0)
                {
                    this.yEdges[secondY].Item2.Add(mergedRegionsCount);
                }
                else
                {
                    this.yEdges.Insert(secondY, Tuple.Create(
                        mergingRegion.BottomRightY,
                        new SortedSet<int>() { mergedRegionsCount }));
                }

                this.mergingRegions.Add(mergingRegion);
            }
        }

        /// <summary>
        /// Obtém o conjunto de candidatos que se intersectam horizontalmente.
        /// </summary>
        /// <remarks>
        /// A lista retornada não pode ser alterada.
        /// </remarks>
        /// <param name="firstX">O índice da primeira aresta.</param>
        /// <param name="secondX">O índice da segunda aresta.</param>
        /// <param name="topLeftX">O valor da primeira aresta.</param>
        /// <param name="bottomRightX">O valor da segunda aresta.</param>
        /// <returns>O conjunto dos índices das regiões candidatas.</returns>
        private SortedSet<int> GetXCandiadates(
            int firstX,
            int secondX,
            T topLeftX,
            T bottomRightX)
        {
            var result = new SortedSet<int>();
            var edgesCount = this.xEdges.Count;
            if (firstX == secondX + 1 && secondX >= edgesCount)
            {
                var current = this.xEdges[firstX];
                if (this.coordinatesComparer.Compare(
                    topLeftX,
                    current.Item1) == 0)
                {
                    return current.Item2;
                }
            }
            else
            {
                var current = this.xEdges[firstX];
                if (this.coordinatesComparer.Compare(
                    topLeftX,
                    current.Item1) == 0)
                {
                    foreach (var item in current.Item2)
                    {
                        result.Add(item);
                    }
                }

                if (firstX != secondX && secondX < xEdges.Count)
                {
                    current = this.xEdges[secondX];
                    if (this.coordinatesComparer.Compare(
                        bottomRightX,
                        current.Item1) == 0)
                    {
                        foreach (var item in current.Item2)
                        {
                            result.Add(item);
                        }
                    }
                }

                for (int i = firstX + 1; i < secondX; ++i)
                {
                    current = this.xEdges[i];
                    foreach (var item in current.Item2)
                    {
                        result.Add(item);
                    }
                }

                if (firstX <= (edgesCount >> 1))
                {
                    for (int i = 0; i < firstX; ++i)
                    {
                        var regionsIndices = this.xEdges[i].Item2;
                        foreach (var regionIndex in regionsIndices)
                        {
                            var bottomValue = this.mergingRegions[regionIndex].BottomRightX;
                            if (this.coordinatesComparer.Compare(
                                bottomRightX,
                                bottomValue) < 0)
                            {
                                result.Add(regionIndex);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = edgesCount - 1; i > secondX; --i)
                    {
                        var regionIndices = this.xEdges[i].Item2;
                        foreach (var regionIndex in regionIndices)
                        {
                            var topValue = this.mergingRegions[regionIndex].TopLeftX;
                            if (this.coordinatesComparer.Compare(
                                topValue,
                                topLeftX) < 0)
                            {
                                result.Add(regionIndex);
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém o conjunto de candidatos que se intersectam verticalmente.
        /// </summary>
        /// <remarks>
        /// A lista retornada não pode ser alterada.
        /// </remarks>
        /// <param name="firstY">O índice da primeira aresta.</param>
        /// <param name="secondY">O índice da segunda aresta.</param>
        /// <param name="topLeftY">O valor da primeira aresta.</param>
        /// <param name="bottomRightY">O valor da segunda aresta.</param>
        /// <returns>O conjunto dos índices das regiões candidatas.</returns>
        private SortedSet<int> GetYCandiadates(
            int firstY,
            int secondY,
            T topLeftY,
            T bottomRightY)
        {
            var result = new SortedSet<int>();
            var yEdgesCount = this.yEdges.Count;
            if (firstY == secondY + 1 && secondY >= yEdgesCount)
            {
                var current = this.yEdges[firstY];
                if (this.coordinatesComparer.Compare(
                    topLeftY,
                    current.Item1) == 0)
                {
                    return current.Item2;
                }
            }
            else
            {
                var current = this.yEdges[firstY];
                if (this.coordinatesComparer.Compare(
                    topLeftY,
                    current.Item1) == 0)
                {
                    foreach (var item in current.Item2)
                    {
                        result.Add(item);
                    }
                }

                if (firstY != secondY && secondY < yEdgesCount)
                {
                    current = this.yEdges[secondY];
                    if (this.coordinatesComparer.Compare(
                        bottomRightY,
                        current.Item1) == 0)
                    {
                        foreach (var item in current.Item2)
                        {
                            result.Add(item);
                        }
                    }
                }

                for (int i = firstY + 1; i < secondY; ++i)
                {
                    current = this.yEdges[i];
                    foreach (var item in current.Item2)
                    {
                        result.Add(item);
                    }
                }

                if (firstY <= (yEdgesCount >> 1))
                {
                    for (int i = 0; i < firstY; ++i)
                    {
                        var regionsIndices = this.yEdges[i].Item2;
                        foreach (var regionIndex in regionsIndices)
                        {
                            var bottomValue = this.mergingRegions[regionIndex].BottomRightY;
                            if (this.coordinatesComparer.Compare(
                                bottomRightY,
                                bottomValue) < 0)
                            {
                                result.Add(regionIndex);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = yEdgesCount - 1; i > secondY; --i)
                    {
                        var regionIndices = this.yEdges[i].Item2;
                        foreach (var regionIndex in regionIndices)
                        {
                            var topValue = this.mergingRegions[regionIndex].TopLeftY;
                            if (this.coordinatesComparer.Compare(
                                topValue,
                                topLeftY) < 0)
                            {
                                result.Add(regionIndex);
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Actualiza o valor dos índices após a remoção.
        /// </summary>
        /// <param name="edges">Os índices a serem actualizados.</param>
        /// <param name="max">A posição na lista de regiões que foi removida.</param>
        private void UpdateIndexAfterRemove(
            List<MutableTuple<T, SortedSet<int>>> edges,
            int max)
        {
            var edgesCount = edges.Count;
            for (int i = 0; i < edgesCount; ++i)
            {
                var currentEdge = edges[i];
                if (currentEdge.Item2.Max > max)
                {
                    var temporarySet = new SortedSet<int>();
                    var edgeMax = currentEdge.Item2.Max;
                    while (edgeMax > max)
                    {
                        temporarySet.Add(edgeMax - 1);
                        currentEdge.Item2.Remove(edgeMax);
                        edgeMax = currentEdge.Item2.Max;
                    }

                    currentEdge.Item2.UnionWith(temporarySet);
                }
            }
        }

        /// <summary>
        /// Actualiza os índices após a remoção da lista proporcionada.
        /// </summary>
        /// <param name="edges">A lista das arestas.</param>
        /// <param name="indices">Os índices removidos.</param>
        private void UpdateIndexAfterRemove(
            List<MutableTuple<T, SortedSet<int>>> edges,
            IntegerSequence indices)
        {
            var edgesCount = edges.Count;
            var indicesCount = indices.Count;
            if (indices.Count > 0)
            {
                for (int i = 0; i < edgesCount; ++i)
                {
                    var currentEdge = edges[i];
                    var indicesEnum = indices.GetReverseEnumerator();

                    var offset = indicesCount;
                    var temporarySet = new SortedSet<int>();
                    var state = indicesEnum.MoveNext();
                    while (state)
                    {
                        var max = indicesEnum.Current;
                        if (currentEdge.Item2.Max > max)
                        {
                            var edgeMax = currentEdge.Item2.Max;
                            while (edgeMax > max)
                            {
                                temporarySet.Add(edgeMax - offset);
                                currentEdge.Item2.Remove(edgeMax);
                                edgeMax = currentEdge.Item2.Max;
                            }
                        }

                        --offset;
                        state = indicesEnum.MoveNext();
                    }

                    currentEdge.Item2.UnionWith(temporarySet);
                }
            }
        }

        /// <summary>
        /// Preenche o resultado com os índices comuns a ambos os candidatos.
        /// </summary>
        /// <param name="xCandidates">Os candidatos sobre as arestas verticais.</param>
        /// <param name="yCandidates">Os candidatos sobre as arestas horizontais.</param>
        /// <param name="result">O resultado das intersecções.</param>
        private void FillIntersectingIndices(
            SortedSet<int> xCandidates,
            SortedSet<int> yCandidates,
            IntegerSequence result)
        {
            var xCandidatesEnum = xCandidates.GetEnumerator();
            var yCandidateEnum = yCandidates.GetEnumerator();
            var state = xCandidatesEnum.MoveNext() && yCandidateEnum.MoveNext();
            while (state)
            {
                var x = xCandidatesEnum.Current;
                var y = yCandidateEnum.Current;
                if (x < y)
                {
                    state = xCandidatesEnum.MoveNext();
                }
                else if (x == y)
                {
                    result.Add(x);
                    state = state = xCandidatesEnum.MoveNext() && yCandidateEnum.MoveNext();
                }
                else
                {
                    state = yCandidateEnum.MoveNext();
                }
            }
        }

        #endregion Funções privadas
    }
}
