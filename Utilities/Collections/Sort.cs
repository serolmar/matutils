namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define a interface para um ordenador.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos a serem ordenados.</typeparam>
    public interface ISorter<T>
    {
        /// <summary>
        /// Ordena uma coleção tomando o comparador por defeito.
        /// </summary>
        /// <param name="collection">A colecçao a ser ordenada.</param>
        void Sort(IList<T> collection);

        /// <summary>
        /// Ordena uma colecção, tomando em conta o comparador proporcionado.
        /// </summary>
        /// <param name="collection">A colecção a ser ordenada.</param>
        /// <param name="comparer">O comparador.</param>
        void Sort(IList<T> collection, IComparer<T> comparer);
    }

    /// <summary>
    /// Aplica o processo de ordenação por junção de sub-listas.
    /// </summary>
    /// <remarks>
    /// Trata-se de um processo de ordenação estável com complexidade logarítmica.
    /// </remarks>
    /// <typeparam name="T">O tipo dos objectos que são ordenados.</typeparam>
    public class MergeSorter<T> : ISorter<T>
    {
        /// <summary>
        /// Ordena uma coleção tomando o comparador por defeito.
        /// </summary>
        /// <param name="collection">A colecçao a ser ordenada.</param>
        public virtual void Sort(IList<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                this.InnerSort(collection, Comparer<T>.Default);
            }
        }

        /// <summary>
        /// Ordena uma colecção, tomando em conta o comparador proporcionado.
        /// </summary>
        /// <param name="collection">A colecção a ser ordenada.</param>
        /// <param name="comparer">O comparador.</param>
        public virtual void Sort(IList<T> collection, IComparer<T> comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.InnerSort(collection, comparer);
            }
        }

        /// <summary>
        /// Permite ordenar um conjunto de elementos retornando o número de trocas efectuadas caso
        /// a ordenação fosse realizada com o algoritmo de ordenação por borbulhamento.
        /// </summary>
        /// <remarks>Aqui é usado o comparador por defeito.</remarks>
        /// <param name="collection">Os elementos a serem ordenados.</param>
        /// <returns>O número de trocas ocorridas.</returns>
        public int SortCountSwaps(IList<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                return this.InnerSortCountSwaps(collection, Comparer<T>.Default);
            }
        }

        /// <summary>
        /// Permite ordenar um conjunto de elementos retornando o número de trocas efectuadas caso
        /// a ordenação fosse realizada com o algoritmo de ordenação por borbulhamento.
        /// </summary>
        /// <param name="collection">Os elementos a serem ordenados.</param>
        /// <param name="comparer">O comparador.</param>
        /// <returns>O número de trocas ocorridas.</returns>
        public int SortCountSwaps(IList<T> collection, IComparer<T> comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                return this.InnerSortCountSwaps(collection, comparer);
            }
        }

        /// <summary>
        /// Permite aplicar a ordenação por fusão ao conjunto de elementos.
        /// </summary>
        /// <param name="elements">O conjunto de elementos.</param>
        /// <param name="comparer">O comparador.</param>
        protected virtual void InnerSort(IList<T> elements, IComparer<T> comparer)
        {
            var elementsCount = elements.Count;
            if (elementsCount >= 2)
            {
                var innerComparer = comparer;
                if (comparer == null)
                {
                    innerComparer = Comparer<T>.Default;
                }

                // Primeira iteração do algoritmo
                var first = 0;
                var second = 1;

                while (second < elementsCount)
                {
                    var firstListItem = elements[first];
                    var secondListItem = elements[second];
                    if (innerComparer.Compare(secondListItem, firstListItem) < 0)
                    {
                        // A troca é efectuada
                        elements[first] = secondListItem;
                        elements[second] = firstListItem;
                    }

                    first += 2;
                    second += 2;
                }

                var size = 2;
                var swap = new T[elementsCount];
                while (size < elementsCount)
                {
                    first = 0;
                    second = size;
                    var third = size + size;
                    while (third < elementsCount)
                    {
                        // Código para a fusão das listas
                        var currentWrite = 0;
                        var i = first;
                        var j = second;
                        while (i < second && j < third)
                        {
                            var firstCurrent = elements[i];
                            var secondCurrent = elements[j];
                            if (comparer.Compare(secondCurrent, firstCurrent) < 0)
                            {
                                swap[currentWrite] = secondCurrent;
                                ++j;
                            }
                            else
                            {
                                swap[currentWrite] = firstCurrent;
                                ++i;
                            }

                            ++currentWrite;
                        }

                        while (i < second)
                        {
                            swap[currentWrite++] = elements[i++];
                        }

                        while (j < third)
                        {
                            swap[currentWrite++] = elements[j++];
                        }

                        // Copia o vector para a colecção inicial
                        for (int k = 0; first < third; ++first, ++k)
                        {
                            elements[first] = swap[k];
                        }

                        // Actualização das variáveis
                        first = third;
                        second = first + size;
                        third = second + size;
                    }

                    // Caso o número de elementos da colecção não seja igual a uma potência binária
                    if (second < elementsCount)
                    {
                        third = Math.Min(third, elementsCount);
                        // Código para a fusão das listas
                        var currentWrite = 0;
                        var i = first;
                        var j = second;
                        while (i < second && j < third)
                        {
                            var firstCurrent = elements[i];
                            var secondCurrent = elements[j];
                            if (comparer.Compare(secondCurrent, firstCurrent) < 0)
                            {
                                swap[currentWrite] = secondCurrent;
                                ++j;
                            }
                            else
                            {
                                swap[currentWrite] = firstCurrent;
                                ++i;
                            }

                            ++currentWrite;
                        }

                        while (i < second)
                        {
                            swap[currentWrite++] = elements[i++];
                        }

                        while (j < third)
                        {
                            swap[currentWrite++] = elements[j++];
                        }

                        // Copia o vector para a colecção inicial
                        for (int k = 0; first < third; ++first, ++k)
                        {
                            elements[first] = swap[k];
                        }
                    }

                    // Actualização da variável tamanho
                    size <<= 1;
                }
            }
        }

        /// <summary>
        /// Permite ordenar um conjunto de elementos retornando o número de trocas efectuadas caso
        /// a ordenação fosse realizada com o algoritmo de ordenação por borbulhamento.
        /// </summary>
        /// <param name="elements">Os elementos a serem ordenados.</param>
        /// <param name="comparer">O comparador.</param>
        /// <returns>O número de trocas ocorridas.</returns>
        protected virtual int InnerSortCountSwaps(IList<T> elements, IComparer<T> comparer)
        {
            var elementsCount = elements.Count;
            if (elementsCount >= 2)
            {
                var innerComparer = comparer;
                if (comparer == null)
                {
                    innerComparer = Comparer<T>.Default;
                }

                // Primeira iteração do algoritmo
                var first = 0;
                var second = 1;
                var counts = new int[(int)Math.Ceiling((double)elementsCount / 2)];
                var countsWrite = 0;

                while (second < elementsCount)
                {
                    var firstListItem = elements[first];
                    var secondListItem = elements[second];
                    if (innerComparer.Compare(secondListItem, firstListItem) < 0)
                    {
                        // A troca é efectuada
                        elements[first] = secondListItem;
                        elements[second] = firstListItem;
                        ++counts[countsWrite];
                    }

                    first += 2;
                    second += 2;
                    ++countsWrite;
                }

                var size = 2;
                var swap = new T[elementsCount];
                while (size < elementsCount)
                {
                    first = 0;
                    second = size;
                    var third = size + size;
                    var countsRead = 0;
                    countsWrite = 0;

                    while (third < elementsCount)
                    {
                        // Código para a fusão das listas
                        var currentWrite = 0;
                        var i = first;
                        var j = second;
                        var swapsNumber = 0;
                        while (i < second && j < third)
                        {
                            var firstCurrent = elements[i];
                            var secondCurrent = elements[j];
                            if (comparer.Compare(secondCurrent, firstCurrent) < 0)
                            {
                                swap[currentWrite] = secondCurrent;
                                swapsNumber += size - i + first + 1;
                                ++j;
                            }
                            else
                            {
                                swap[currentWrite] = firstCurrent;
                                ++i;
                            }

                            ++currentWrite;
                        }

                        while (i < second)
                        {
                            swap[currentWrite++] = elements[i++];
                        }

                        while (j < third)
                        {
                            swap[currentWrite++] = elements[j++];
                        }

                        // Copia o vector para a colecção inicial
                        for (int k = 0; first < third; ++first, ++k)
                        {
                            elements[first] = swap[k];
                        }

                        // Escreve o valor das contagens
                        counts[countsWrite++] = counts[countsRead++] + counts[countsRead++] + swapsNumber;

                        // Actualização das variáveis
                        first = third;
                        second = first + size;
                        third = second + size;
                    }

                    // Caso o número de elementos da colecção não seja igual a uma potência binária
                    if (second < elementsCount)
                    {
                        third = Math.Min(third, elementsCount);
                        // Código para a fusão das listas
                        var currentWrite = 0;
                        var i = first;
                        var j = second;
                        var swapsNumber = 0;
                        while (i < second && j < third)
                        {
                            var firstCurrent = elements[i];
                            var secondCurrent = elements[j];
                            if (comparer.Compare(secondCurrent, firstCurrent) < 0)
                            {
                                swap[currentWrite] = secondCurrent;
                                swapsNumber += size - i + 1;
                                ++j;
                            }
                            else
                            {
                                swap[currentWrite] = firstCurrent;
                                ++i;
                            }

                            ++currentWrite;
                        }

                        while (i < second)
                        {
                            swap[currentWrite++] = elements[i++];
                        }

                        while (j < third)
                        {
                            swap[currentWrite++] = elements[j++];
                        }

                        // Copia o vector para a colecção inicial
                        for (int k = 0; first < third; ++first, ++k)
                        {
                            elements[first] = swap[k];
                        }

                        // Escreve o valor das contagens
                        counts[countsWrite++] = counts[countsRead++] + counts[countsRead++] + swapsNumber;
                    }

                    // Actualização da variável tamanho
                    size <<= 1;
                }

                return counts[0];
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// Apica o processo de ordenação por borbulhamento.
    /// </summary>
    /// <remarks>
    /// Trata-se de um processo de ordenação estável com complexidade quadrática.
    /// </remarks>
    /// <typeparam name="T">O tipo de objectos a serem ordenados.</typeparam>
    public class BubbleSorter<T> : ISorter<T>
    {

        /// <summary>
        /// Ordena uma coleção tomando o comparador por defeito.
        /// </summary>
        /// <param name="collection">A colecçao a ser ordenada.</param>
        public void Sort(IList<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                this.InnerSort(collection, Comparer<T>.Default);
            }
        }

        /// <summary>
        /// Ordena uma colecção, tomando em conta o comparador proporcionado.
        /// </summary>
        /// <param name="collection">A colecção a ser ordenada.</param>
        /// <param name="comparer">O comparador.</param>
        public void Sort(IList<T> collection, IComparer<T> comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.InnerSort(collection, comparer);
            }
        }

        /// <summary>
        /// Ordena uma coleção tomando o comparador por defeito, contando o número de trocas.
        /// </summary>
        /// <param name="collection">A colecçao a ser ordenada.</param>
        /// <returns>O número de trocas efectuadas durante a ordenação.</returns>
        public int SortCountSwaps(IList<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                return this.InnerSortCountSwaps(collection, Comparer<T>.Default);
            }
        }

        /// <summary>
        /// Ordena uma colecção, tomando em conta o comparador proporcionado.
        /// </summary>
        /// <param name="collection">A colecção a ser ordenada.</param>
        /// <param name="comparer">O comparador.</param>
        /// <returns>O número de trocas efectuadas durante a ordenação.</returns>
        public int SortCountSwaps(IList<T> collection, IComparer<T> comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                return this.InnerSortCountSwaps(collection, comparer);
            }
        }

        /// <summary>
        /// Permite aplicar a ordenação por fusão ao conjunto de elementos.
        /// </summary>
        /// <param name="elements">O conjunto de elementos.</param>
        /// <param name="comparer">O comparador.</param>
        protected virtual void InnerSort(IList<T> elements, IComparer<T> comparer)
        {
            var elementsCount = elements.Count;
            if (elementsCount >= 2)
            {
                var innerComparer = comparer;
                if (comparer == null)
                {
                    innerComparer = Comparer<T>.Default;
                }

                var lastElement = elementsCount - 1;
                for (int i = 0; i < lastElement; ++i)
                {
                    for (int j = i + 1; j < elementsCount; ++j)
                    {
                        var firstElement = elements[i];
                        var secondElement = elements[j];
                        if (innerComparer.Compare(secondElement, firstElement) < 0)
                        {
                            elements[i] = secondElement;
                            elements[j] = firstElement;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Permite ordenar um conjunto de elementos retornando o número de trocas efectuadas caso
        /// a ordenação fosse realizada com o algoritmo de ordenação por borbulhamento.
        /// </summary>
        /// <param name="elements">Os elementos a serem ordenados.</param>
        /// <param name="comparer">O comparador.</param>
        /// <returns>O número de trocas ocorridas.</returns>
        protected virtual int InnerSortCountSwaps(IList<T> elements, IComparer<T> comparer)
        {
            var result = 0;
            var elementsCount = elements.Count;
            if (elementsCount >= 2)
            {
                var innerComparer = comparer;
                if (comparer == null)
                {
                    innerComparer = Comparer<T>.Default;
                }

                var lastElement = elementsCount - 1;
                for (int i = 0; i < lastElement; ++i)
                {
                    for (int j = i + 1; j < elementsCount; ++j)
                    {
                        var firstElement = elements[i];
                        var secondElement = elements[j];
                        if (innerComparer.Compare(secondElement, firstElement) < 0)
                        {
                            ++result;
                            elements[i] = secondElement;
                            elements[j] = firstElement;
                        }
                    }
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Aplica o processo de ordenação com base na construção de uma pilha.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos a serem ordenados.</typeparam>
    public class HeapSorter<T> : ISorter<T>
    {
        /// <summary>
        /// Ordena uma coleção tomando o comparador por defeito.
        /// </summary>
        /// <param name="collection">A colecçao a ser ordenada.</param>
        public void Sort(IList<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                var count = collection.Count;
                if (count > 1)
                {
                    this.InnerHeapSort(collection, count, Comparer<T>.Default);
                }
            }
        }

        /// <summary>
        /// Ordena uma colecção, tomando em conta o comparador proporcionado.
        /// </summary>
        /// <param name="collection">A colecção a ser ordenada.</param>
        /// <param name="comparer">O comparador.</param>
        public void Sort(IList<T> collection, IComparer<T> comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                var count = collection.Count;
                if (count > 1)
                {
                    this.InnerHeapSort(collection, count, comparer);
                }
            }
        }

        /// <summary>
        /// Realiza a ordenação da colecção proporcionada com base no comparador.
        /// </summary>
        /// <param name="collection">A colecção.</param>
        /// <param name="count">O número de elementos na colecção.</param>
        /// <param name="comparer">O comparador.</param>
        private void InnerHeapSort(
            IList<T> collection,
            int count,
            IComparer<T> comparer)
        {
            this.Heapify(collection, count, comparer);

            var end = collection.Count - 1;
            while (end > 0)
            {
                var temp = collection[end];
                collection[end] = collection[0];
                collection[0] = temp;
                --end;
                this.SiftDown(
                    collection,
                    0,
                    end,
                    comparer);
            }
        }

        /// <summary>
        /// Obtém o índice onde se encontra o nó ascendente dado
        /// o nó descendente.
        /// </summary>
        /// <param name="i">O índice do nó descendente.</param>
        /// <returns>O índice do nó ascendente.</returns>
        private int Parent(int i)
        {
            return (i - 1) >> 1;
        }

        /// <summary>
        /// Obtém o índice descendente que se encontra à esquerda.
        /// </summary>
        /// <param name="i">O índice do nó ascendente.</param>
        /// <returns>O índice o nó ascendente que se encontra à esquerda.</returns>
        private int Left(int i)
        {
            return 2 * i + 1;
        }

        /// <summary>
        /// Obtém o índice do nó descendente que se encontra à direita.
        /// </summary>
        /// <param name="i">O índice do nó ascendente.</param>
        /// <returns>O índice do nó ascendente que se encontra à direita.</returns>
        private int Right(int i)
        {
            return 2 * i + 2;
        }

        /// <summary>
        /// Desloca o elemento inicial de um segmento da colecção.
        /// </summary>
        /// <param name="collection">A colecção.</param>
        /// <param name="start">O índice do elemento inicial.</param>
        /// <param name="end">O índice após o elemento final.</param>
        /// <param name="comparer">O comparador.</param>
        private void SiftDown(
            IList<T> collection,
            int start,
            int end,
            IComparer<T> comparer)
        {
            var root = start;
            var leftRootIndex = this.Left(root);
            while (leftRootIndex <= end)
            {
                var swap = root;
                var swapValue = collection[root];
                var childValue = collection[leftRootIndex];
                if (comparer.Compare(
                    swapValue,
                    childValue) < 0)
                {
                    swap = leftRootIndex;
                }

                ++leftRootIndex;
                if (leftRootIndex <= end)
                {
                    var temp = collection[leftRootIndex];
                    if (comparer.Compare(
                        childValue,
                        temp) < 0)
                    {
                        swap = leftRootIndex;
                        childValue = temp;
                    }
                }

                if (swap == root)
                {
                    return;
                }
                else
                {
                    collection[swap] = swapValue;
                    collection[root] = childValue;
                    root = swap;
                    leftRootIndex = this.Left(root);
                }
            }
        }

        /// <summary>
        /// Organiza a colecção de modo que esta representa uma árvore binária.
        /// </summary>
        /// <param name="collection">A colecção.</param>
        /// <param name="count">O número de itens a considerar no grupo.</param>
        /// <param name="comparer">O comparador de elementos.</param>
        private void Heapify(
            IList<T> collection,
            int count,
            IComparer<T> comparer)
        {
            var start = this.Parent(count - 1);
            for (; start > -1; --start)
            {
                this.SiftDown(
                       collection,
                       start,
                       count - 1,
                       comparer);
            }
        }
    }

    /// <summary>
    /// Implementa o algoritmo rápido de ordenação.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que constituem as entradas da colecção.</typeparam>
    public class QuickSorter<T> : ISorter<T>
    {
        /// <summary>
        /// Ordena uma coleção tomando o comparador por defeito.
        /// </summary>
        /// <param name="collection">A colecçao a ser ordenada.</param>
        public void Sort(IList<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                var count = collection.Count;
                if (count > 1)
                {
                    this.InnerQuickSort(collection, count, Comparer<T>.Default);
                }
            }
        }

        /// <summary>
        /// Ordena a colecção tendo em conta o particionardor.
        /// </summary>
        /// <param name="collection">A colecção a ser ordenada.</param>
        /// <param name="partitioner">O particionador.</param>
        private void Sort(
            IList<T> collection,
            QuickSorter<T>.IPartitioner partitioner)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else if (partitioner == null)
            {
                throw new ArgumentNullException("partitioner");
            }
            else
            {
                var count = collection.Count;
                if (count > 1)
                {
                    this.InnerQuickSort(
                        collection,
                        count,
                        Comparer<T>.Default,
                        partitioner);
                }
            }
        }

        /// <summary>
        /// Ordena uma colecção, tomando em conta o comparador proporcionado.
        /// </summary>
        /// <param name="collection">A colecção a ser ordenada.</param>
        /// <param name="comparer">O comparador.</param>
        public void Sort(IList<T> collection, IComparer<T> comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                var count = collection.Count;
                if (count > 1)
                {
                    this.InnerQuickSort(collection, count, comparer);
                }
            }
        }

        /// <summary>
        /// Ordena uma colecção, tendo em conta o comparador e o particionador.
        /// </summary>
        /// <param name="collection">A colecção.</param>
        /// <param name="comparer">O comparador.</param>
        /// <param name="partitioner">O particionador.</param>
        public void Sort(
            IList<T> collection,
            IComparer<T> comparer,
            QuickSorter<T>.IPartitioner partitioner)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else if (partitioner == null)
            {
                throw new ArgumentNullException("partitioner");
            }
            else
            {
                var count = collection.Count;
                if (count > 1)
                {
                    this.InnerQuickSort(
                        collection,
                        count,
                        comparer,
                        partitioner);
                }
            }
        }

        /// <summary>
        /// Função que implementa a ordenação rápida.
        /// </summary>
        /// <param name="collection">A colecção.</param>
        /// <param name="count">O número de elementos na colecção.</param>
        /// <param name="comparer">
        /// O comparador responsável pela comparação dos elementos
        /// da colecção.
        /// </param>
        private void InnerQuickSort(
            IList<T> collection,
            int count,
            IComparer<T> comparer)
        {
            var partitionStack = new Stack<Tuple<int, int>>();

            // Aplica a partição ao vector completo
            var index = count - 1;
            var pivot = this.DefaultPartition(collection, 0, index, comparer);
            var incPivot = pivot + 1;
            if (incPivot < index)
            {
                // Coloca o alcance na pilha para análise futura
                partitionStack.Push(Tuple.Create(incPivot, index));
            }

            // Processa o primeiro ramo da árvore de execução
            while (0 < pivot)
            {
                var prevPivot = this.DefaultPartition(
                    collection,
                    0,
                    pivot,
                    comparer);
                incPivot = prevPivot + 1;
                if (incPivot < pivot)
                {
                    partitionStack.Push(Tuple.Create(incPivot, pivot));
                }

                if (comparer.Compare(collection[0], collection[prevPivot]) == 0)
                {
                    pivot = 0;
                }
                else
                {
                    pivot = prevPivot;
                }
            }

            while (partitionStack.Count > 0)
            {
                var top = partitionStack.Pop();
                index = top.Item2;
                pivot = this.DefaultPartition(
                    collection,
                    top.Item1,
                    index,
                    comparer);
                incPivot = pivot + 1;
                if (incPivot < index)
                {
                    // Coloca o alcance na pilha para análise futura
                    partitionStack.Push(Tuple.Create(incPivot, index));
                }

                // Processa o novo ramo da árvore
                index = top.Item1;
                while (index < pivot)
                {
                    var prevPivot = this.DefaultPartition(
                        collection,
                        index,
                        pivot,
                        comparer);
                    incPivot = prevPivot + 1;
                    if (incPivot < pivot)
                    {
                        partitionStack.Push(Tuple.Create(incPivot, pivot));
                    }

                    if (comparer.Compare(collection[index], collection[prevPivot]) == 0)
                    {
                        pivot = index;
                    }
                    else
                    {
                        pivot = prevPivot;
                    }
                }
            }
        }

        /// <summary>
        /// Função que implementa a ordenação rápida, tendo em conta um particionador.
        /// </summary>
        /// <param name="collection">A colecção.</param>
        /// <param name="count">O número de elementos na colecção.</param>
        /// <param name="comparer">O comparador.</param>
        /// <param name="partitioner">O particionador.</param>
        private void InnerQuickSort(
            IList<T> collection,
            int count,
            IComparer<T> comparer,
            QuickSorter<T>.IPartitioner partitioner)
        {
            var partitionStack = new Stack<Tuple<int, int>>();

            // Aplica a partição ao vector completo
            var index = count - 1;
            var pivot = this.DefaultPartition(collection, 0, index, comparer);
            var incPivot = pivot + 1;
            if (incPivot < index)
            {
                // Coloca o alcance na pilha para análise futura
                partitionStack.Push(Tuple.Create(incPivot, index));
            }

            // Processa o primeiro ramo da árvore de execução
            while (0 < pivot)
            {
                var prevPivot = this.DefaultPartition(
                    collection,
                    0,
                    pivot,
                    comparer);
                incPivot = prevPivot + 1;
                if (incPivot < pivot)
                {
                    partitionStack.Push(Tuple.Create(incPivot, pivot));
                }

                if (comparer.Compare(collection[0], collection[prevPivot]) == 0)
                {
                    pivot = 0;
                }
                else
                {
                    pivot = prevPivot;
                }
            }

            while (partitionStack.Count > 0)
            {
                var top = partitionStack.Pop();
                index = top.Item2;
                pivot = partitioner.Partition(
                    collection,
                    top.Item1,
                    index,
                    comparer);
                incPivot = pivot + 1;
                if (incPivot < index)
                {
                    // Coloca o alcance na pilha para análise futura
                    partitionStack.Push(Tuple.Create(incPivot, index));
                }

                // Processa o novo ramo da árvore
                index = top.Item1;
                while (index < pivot)
                {
                    var prevPivot = partitioner.Partition(
                        collection,
                        index,
                        pivot,
                        comparer);
                    incPivot = prevPivot + 1;
                    if (incPivot < pivot)
                    {
                        partitionStack.Push(Tuple.Create(incPivot, pivot));
                    }

                    if (comparer.Compare(collection[index], collection[prevPivot]) == 0)
                    {
                        pivot = index;
                    }
                    else
                    {
                        pivot = prevPivot;
                    }
                }
            }
        }

        /// <summary>
        /// Estabelece a função por defeito que efectua partições.
        /// </summary>
        /// <param name="collection">A colecção.</param>
        /// <param name="start">O índice inicial da partição da colecção a ser considerada.</param>
        /// <param name="end">O índice final da partição da colecção a ser considerada.</param>
        /// <param name="comparer">O comparador.</param>
        /// <returns>O índice do pivô calculado.</returns>
        private int DefaultPartition(
            IList<T> collection,
            int start,
            int end,
            IComparer<T> comparer)
        {
            var pivot = collection[start];
            var j = end;
            var jVal = collection[j];
            while (comparer.Compare(jVal, pivot) > 0)
            {
                --j;
                jVal = collection[j];
            }

            if (j == start)
            {
                return start;
            }
            else
            {
                // Troca o pivô com o valor em j
                collection[start] = jVal;
                collection[j] = pivot;
                jVal = pivot;

                var i = start + 1;
                var iVal = collection[i];
                while (comparer.Compare(iVal, pivot) < 0)
                {
                    ++i;
                    iVal = collection[i];
                }

                if (i < j)
                {
                    collection[i] = jVal;
                    collection[j] = iVal;
                    iVal = collection[i];

                    while (true)
                    {
                        --j;
                        jVal = collection[j];
                        while (comparer.Compare(jVal, pivot) > 0)
                        {
                            --j;
                            jVal = collection[j];
                        }

                        if (i < j)
                        {
                            collection[i] = jVal;
                            collection[j] = iVal;
                            jVal = collection[j];

                            ++i;
                            iVal = collection[i];
                            while (comparer.Compare(iVal, pivot) < 0)
                            {
                                ++i;
                                iVal = collection[i];
                            }

                            if (i < j)
                            {
                                collection[i] = jVal;
                                collection[j] = iVal;
                                iVal = collection[i];
                            }
                            else
                            {
                                return j;
                            }
                        }
                        else
                        {
                            return i;
                        }
                    }
                }
                else
                {
                    return j;
                }
            }
        }

        /// <summary>
        /// Define o particionador para o algoritmo.
        /// </summary>
        public interface IPartitioner
        {
            /// <summary>
            /// Função responsável por efecutar a partição das entradas.
            /// </summary>
            /// <remarks>
            /// A partição começa por identificar um pivô, trocando os elementos
            /// cujo valor é superior ao pivô e se encontram às esquerda com os elementos
            /// inferiores ao pivô e se encontram à sua direita.
            /// </remarks>
            /// <param name="collection">A colecção.</param>
            /// <param name="low">
            /// O índice inicial da partição da colecção a ser considerada.
            /// </param>
            /// <param name="high">
            /// O índicie final da partição da colecção a ser considerada.
            /// </param>
            /// <param name="comparer">O comparador.</param>
            /// <returns>O índice do pivô determinado.</returns>
            int Partition(
                IList<T> collection,
                int low,
                int high,
                IComparer<T> comparer);
        }
    }

    /// <summary>
    /// Implementa o algoritmo de inserção clássico.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas da colecção.</typeparam>
    public class InsertionSorter<T> : ISorter<T>
    {
        /// <summary>
        /// Ordena uma coleção tomando o comparador por defeito.
        /// </summary>
        /// <param name="collection">A colecçao a ser ordenada.</param>
        public void Sort(IList<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                this.InnerSort(collection, Comparer<T>.Default);
            }
        }

        /// <summary>
        /// Ordena uma colecção, tomando em conta o comparador proporcionado.
        /// </summary>
        /// <param name="collection">A colecção a ser ordenada.</param>
        /// <param name="comparer">O comparador.</param>
        public void Sort(IList<T> collection, IComparer<T> comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.InnerSort(collection, comparer);
            }
        }

        /// <summary>
        /// Ordena uma colecção, tendo em conta o comparador.
        /// </summary>
        /// <param name="collection">A colecção a ser ordenada.</param>
        /// <param name="comparer">O comparador.</param>
        private void InnerSort(IList<T> collection, IComparer<T> comparer)
        {
            var length = collection.Count;
            for (int i = 1; i < length; ++i)
            {
                var update = i;
                var aux = collection[i];
                for (int j = i - 1; j > -1; --j)
                {
                    var current = collection[j];
                    if (comparer.Compare(aux, collection[j]) < 0)
                    {
                        collection[update--] = current;
                    }
                }

                collection[update] = aux;
            }
        }
    }

    /// <summary>
    /// Implementa uma implementação diferente do algoritmo de inserção.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas da colecção.</typeparam>
    public class BinarySearchInsertionSorter<T> : ISorter<T>
    {
        /// <summary>
        /// Ordena uma coleção tomando o comparador por defeito.
        /// </summary>
        /// <param name="collection">A colecçao a ser ordenada.</param>
        public void Sort(IList<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else if (collection.GetType().IsArray)
            {
                var array = (T[])collection;
                this.InnerSort(array, Comparer<T>.Default);
            }
            else
            {
                this.InnerSort(collection, Comparer<T>.Default);
            }
        }

        /// <summary>
        /// Ordena uma colecção, tomando em conta o comparador proporcionado.
        /// </summary>
        /// <param name="collection">A colecção a ser ordenada.</param>
        /// <param name="comparer">O comparador.</param>
        public void Sort(IList<T> collection, IComparer<T> comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else if (collection.GetType().IsArray)
            {
                var array = (T[])collection;
                this.InnerSort(array, comparer);
            }
            else
            {
                this.InnerSort(collection, comparer);
            }
        }

        /// <summary>
        /// Ordena a colecção.
        /// </summary>
        /// <param name="collection">A colecção.</param>
        /// <param name="comparer">O comparador.</param>
        private void InnerSort(IList<T> collection, IComparer<T> comparer)
        {
            var length = collection.Count;
            if (length > 1)
            {
                var previous = collection[0];
                var current = collection[1];
                var index = 1;
                if (comparer.Compare(current, previous) < 0)
                {
                    collection[0] = current;
                    collection[1] = previous;
                    index = 0;
                }

                previous = current;
                for (int i = 2; i < length; ++i)
                {
                    current = collection[i];
                    if (comparer.Compare(current, previous) <= 0)
                    {
                        index = this.FindGreatestPosition(
                            collection,
                            current,
                            comparer,
                            0,
                            index);
                    }
                    else
                    {
                        index = this.FindGreatestPosition(
                            collection,
                            current,
                            comparer,
                            index + 1,
                            i - 1);
                    }

                    // Move os elementos
                    var j = i - 1;
                    var k = i;
                    for (; j > index; --j, --k)
                    {
                        collection[j] = collection[k];
                    }

                    collection[index] = current;
                    previous = current;
                }
            }
        }

        /// <summary>
        /// Ordena o vector.
        /// </summary>
        /// <param name="collection">O vector.</param>
        /// <param name="comparer">O comparador de elementos.</param>
        private void InnerSort(T[] collection, IComparer<T> comparer)
        {
            var length = collection.Length;
            if (length > 1)
            {
                var previous = collection[0];
                var current = collection[1];
                var index = 1;
                if (comparer.Compare(current, previous) < 0)
                {
                    collection[0] = current;
                    collection[1] = previous;
                    index = 0;
                }

                previous = current;
                for (int i = 2; i < length; ++i)
                {
                    current = collection[i];
                    if (comparer.Compare(current, previous) <= 0)
                    {
                        index = this.FindGreatestPosition(
                            collection,
                            current,
                            comparer,
                            0,
                            index);
                    }
                    else
                    {
                        index = this.FindGreatestPosition(
                            collection,
                            current,
                            comparer,
                            index + 1,
                            i - 1);
                    }

                    // Move os elementos
                    var j = i - 1;
                    var k = i;
                    for (; j > index; --j, --k)
                    {
                        collection[j] = collection[k];
                    }

                    collection[index] = current;
                    previous = current;
                }
            }
        }

        /// <summary>
        /// Encontra a posição onde o elemento especificado se encontra.
        /// </summary>
        /// <param name="elements">A colecção onde será realizada a procura.</param>
        /// <param name="objectToInsert">O elemento a ser procurado.</param>
        /// <param name="comparer">O comparador.</param>
        /// <param name="elementsCount">O índice da última posição.</param>
        /// <returns>O índice da posição onde o elemento se encontra.</returns>
        private int FindGreatestPosition(
            IList<T> elements, 
            T objectToInsert,
            IComparer<T> comparer,
            int start,
            int end)
        {
            if (comparer.Compare(objectToInsert, elements[end]) >= 0)
            {
                return end + 1;
            }
            else if (comparer.Compare(objectToInsert, elements[0]) < 0)
            {
                return 0;
            }
            else
            {
                int low = 0;
                int high = end - 1;
                while (low < high)
                {
                    int sum = high + low;
                    int intermediaryIndex = sum / 2;
                    if (sum % 2 == 0)
                    {
                        if (comparer.Compare(objectToInsert, elements[intermediaryIndex]) < 0)
                        {
                            high = intermediaryIndex;
                        }
                        else
                        {
                            low = intermediaryIndex;
                        }
                    }
                    else
                    {
                        if (
                            comparer.Compare(objectToInsert, elements[intermediaryIndex]) > 0 &&
                            comparer.Compare(objectToInsert, elements[intermediaryIndex + 1]) < 0)
                        {
                            return intermediaryIndex + 1;
                        }
                        else if (
                            comparer.Compare(objectToInsert, elements[intermediaryIndex]) == 0 &&
                            comparer.Compare(objectToInsert, elements[intermediaryIndex + 1]) < 0)
                        {
                            return intermediaryIndex;
                        }
                        else if (comparer.Compare(objectToInsert, elements[intermediaryIndex + 1]) == 0)
                        {
                            low = intermediaryIndex + 1;
                        }
                        else if (comparer.Compare(objectToInsert, elements[intermediaryIndex]) < 0)
                        {
                            high = intermediaryIndex;
                        }
                        else
                        {
                            low = intermediaryIndex;
                        }
                    }
                }

                return high;
            }
        }
    }
}
