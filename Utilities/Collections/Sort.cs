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
        /// O comparador de elementos.
        /// </summary>
        private IComparer<T> comparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="MergeSorter{T}"/>.
        /// </summary>
        public MergeSorter()
        {
            this.comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="MergeSorter{T}"/>.
        /// </summary>
        /// <param name="comparer">O comparador dos elementos.</param>
        public MergeSorter(IComparer<T> comparer)
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
        /// Obtém ou atribui o comparador de elementos.
        /// </summary>
        public IComparer<T> Comparer
        {
            get
            {
                return this.comparer;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesException("Comparer can't be null.");
                }
                else
                {
                    this.comparer = value;
                }
            }
        }

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
                this.InnerSort(collection, this.comparer);
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
                return this.InnerSortCountSwaps(collection, this.comparer);
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
        /// O comparador de elementos.
        /// </summary>
        private IComparer<T> comparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipos <see cref="BubbleSorter{T}"/>.
        /// </summary>
        public BubbleSorter()
        {
            this.comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipos <see cref="BubbleSorter{T}"/>.
        /// </summary>
        /// <param name="comparer">O comparador.</param>
        public BubbleSorter(IComparer<T> comparer)
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
        /// Obtém ou atribui o comparador de elementos.
        /// </summary>
        public IComparer<T> Comparer
        {
            get
            {
                return this.comparer;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesException("Comparer can't be null.");
                }
                else
                {
                    this.comparer = value;
                }
            }
        }

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
                this.InnerSort(collection, this.comparer);
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
                return this.InnerSortCountSwaps(collection, this.comparer);
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
        /// O comparador de elementos.
        /// </summary>
        private IComparer<T> comparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipos <see cref="HeapSorter{T}"/>.
        /// </summary>
        public HeapSorter()
        {
            this.comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipos <see cref="HeapSorter{T}"/>.
        /// </summary>
        /// <param name="comparer">O comparador de elementos.</param>
        public HeapSorter(IComparer<T> comparer)
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
        /// Obtém ou atribui o comparador de elementos.
        /// </summary>
        public IComparer<T> Comparer
        {
            get
            {
                return this.comparer;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesException("Comparer can't be null.");
                }
                else
                {
                    this.comparer = value;
                }
            }
        }

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
                    this.InnerHeapSort(collection, count, this.comparer);
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
            var innerComparer = comparer;
            this.Heapify(collection, count, innerComparer);

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
                    innerComparer);
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
        /// O comparador de elementos.
        /// </summary>
        private IComparer<T> comparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="IComparer{T}"/>.
        /// </summary>
        public QuickSorter()
        {
            this.comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="comparer">O comparador de elementos.</param>
        public QuickSorter(IComparer<T> comparer)
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
        /// Obtém ou atribui o comparador de elementos.
        /// </summary>
        public IComparer<T> Comparer
        {
            get
            {
                return this.comparer;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesException("Comparer can't be null.");
                }
                else
                {
                    this.comparer = value;
                }
            }
        }

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
                    this.InnerQuickSort(collection, count, this.comparer);
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
                        this.comparer,
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
            var innerComparer = comparer;
            var partitionStack = new Stack<Tuple<int, int>>();

            // Aplica a partição ao vector completo
            var index = count - 1;
            var pivot = this.DefaultPartition(collection, 0, index, innerComparer);
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
                    innerComparer);
                incPivot = prevPivot + 1;
                if (incPivot < pivot)
                {
                    partitionStack.Push(Tuple.Create(incPivot, pivot));
                }

                if (innerComparer.Compare(collection[0], collection[prevPivot]) == 0)
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
                    innerComparer);
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
                        innerComparer);
                    incPivot = prevPivot + 1;
                    if (incPivot < pivot)
                    {
                        partitionStack.Push(Tuple.Create(incPivot, pivot));
                    }

                    if (innerComparer.Compare(collection[index], collection[prevPivot]) == 0)
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
            var innerComparer = comparer;
            var partitionStack = new Stack<Tuple<int, int>>();

            // Aplica a partição ao vector completo
            var index = count - 1;
            var pivot = this.DefaultPartition(collection, 0, index, innerComparer);
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
                    innerComparer);
                incPivot = prevPivot + 1;
                if (incPivot < pivot)
                {
                    partitionStack.Push(Tuple.Create(incPivot, pivot));
                }

                if (innerComparer.Compare(collection[0], collection[prevPivot]) == 0)
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
                    innerComparer);
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
                        innerComparer);
                    incPivot = prevPivot + 1;
                    if (incPivot < pivot)
                    {
                        partitionStack.Push(Tuple.Create(incPivot, pivot));
                    }

                    if (innerComparer.Compare(collection[index], collection[prevPivot]) == 0)
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
        /// O comparador de elementos.
        /// </summary>
        private IComparer<T> comparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="InsertionSorter{T}"/>.
        /// </summary>
        public InsertionSorter()
        {
            this.comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="InsertionSorter{T}"/>.
        /// </summary>
        /// <param name="comparer">O comparador de elementos.</param>
        public InsertionSorter(IComparer<T> comparer)
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
        /// Obtém ou atribui o comparador de elementos.
        /// </summary>
        public IComparer<T> Comparer
        {
            get
            {
                return this.comparer;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesException("Comparer can't be null.");
                }
                else
                {
                    this.comparer = value;
                }
            }
        }

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
                this.InnerSort(collection, this.comparer);
            }
        }

        /// <summary>
        /// Ordena uma colecção, tendo em conta o comparador.
        /// </summary>
        /// <param name="collection">A colecção a ser ordenada.</param>
        /// <param name="comparer">O comparador.</param>
        private void InnerSort(IList<T> collection, IComparer<T> comparer)
        {
            var innerComparer = comparer;
            var length = collection.Count;
            for (int i = 1; i < length; ++i)
            {
                var update = i;
                var aux = collection[i];
                for (int j = i - 1; j > -1; --j)
                {
                    var current = collection[j];
                    if (innerComparer.Compare(aux, collection[j]) < 0)
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
        /// O comparador de elementos.
        /// </summary>
        private IComparer<T> comparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="BinarySearchInsertionSorter{T}"/>.
        /// </summary>
        public BinarySearchInsertionSorter()
        {
            this.comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="BinarySearchInsertionSorter{T}"/>.
        /// </summary>
        /// <param name="comparer">O comparador de elementos.</param>
        public BinarySearchInsertionSorter(IComparer<T> comparer)
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
        /// Obtém ou atribui o comparador de elementos.
        /// </summary>
        public IComparer<T> Comparer
        {
            get
            {
                return this.comparer;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesException("Comparer can't be null.");
                }
                else
                {
                    this.comparer = value;
                }
            }
        }

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
                this.InnerSort(array, this.comparer);
            }
            else
            {
                this.InnerSort(collection, this.comparer);
            }
        }

        /// <summary>
        /// Ordena a colecção.
        /// </summary>
        /// <param name="collection">A colecção.</param>
        /// <param name="comparer">O comparador.</param>
        private void InnerSort(IList<T> collection, IComparer<T> comparer)
        {
            var innerComparer = comparer;
            var length = collection.Count;
            if (length > 1)
            {
                var previous = collection[0];
                var current = collection[1];
                var index = 1;
                if (innerComparer.Compare(current, previous) < 0)
                {
                    collection[0] = current;
                    collection[1] = previous;
                    index = 0;
                }

                previous = current;
                for (int i = 2; i < length; ++i)
                {
                    current = collection[i];
                    if (innerComparer.Compare(current, previous) <= 0)
                    {
                        index = this.FindGreatestPosition(
                            collection,
                            current,
                            innerComparer,
                            0,
                            index);
                    }
                    else
                    {
                        index = this.FindGreatestPosition(
                            collection,
                            current,
                            innerComparer,
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
            var innerComparer = comparer;
            var length = collection.Length;
            if (length > 1)
            {
                var previous = collection[0];
                var current = collection[1];
                var index = 1;
                if (innerComparer.Compare(current, previous) < 0)
                {
                    collection[0] = current;
                    collection[1] = previous;
                    index = 0;
                }

                previous = current;
                for (int i = 2; i < length; ++i)
                {
                    current = collection[i];
                    if (innerComparer.Compare(current, previous) <= 0)
                    {
                        index = this.FindGreatestPosition(
                            collection,
                            current,
                            innerComparer,
                            0,
                            index);
                    }
                    else
                    {
                        index = this.FindGreatestPosition(
                            collection,
                            current,
                            innerComparer,
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
        /// <param name="start">O índice da primeira posição.</param>
        /// <param name="end">O índice da última posição.</param>
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

    /// <summary>
    /// Implmenta o ordenador de inteiros por comparação.
    /// </summary>
    public class CountingSorter<T> : ISorter<T>
    {
        /// <summary>
        /// A função que permite mapear o objecto a um inteiro.
        /// </summary>
        private Func<T, uint> map;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CountingSorter{T}"/>.
        /// </summary>
        /// <param name="map">O mapeador que, ao objecto, atribui um valor inteiro.</param>
        public CountingSorter(Func<T, uint> map)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }
            else
            {
                this.map = map;
            }
        }

        /// <summary>
        /// Ordena uma coleção tomando o comparador por defeito.
        /// </summary>
        /// <param name="collection">A colecçao a ser ordenada.</param>
        public void Sort(IList<T> collection)
        {
            var temp = this.GetSorted(collection);
            var count = temp.Count;
            for (int i = 0; i < count; ++i)
            {
                collection[i] = temp[i];
            }
        }

        /// <summary>
        /// Obtém a versão ordenada da colecção de inteiros.
        /// </summary>
        /// <param name="collection">A colecção.</param>
        /// <returns>A colecção ordenada.</returns>
        public IList<T> GetSorted(IList<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                var count = collection.Count;
                if (count == 0)
                {
                    return new T[0];
                }
                else
                {
                    var max = this.map.Invoke(collection[0]);
                    for (int i = 1; i < count; ++i)
                    {
                        var current = this.map.Invoke(collection[i]);
                        if (current > max)
                        {
                            max = current;
                        }
                    }

                    ++max;
                    var counter = new int[max];
                    for (int i = 0; i < count; ++i)
                    {
                        ++counter[this.map.Invoke(collection[i])];
                    }

                    var result = new T[count];
                    var accumulate = 0;
                    for (int i = 0; i < max; ++i)
                    {
                        var oldCount = counter[i];
                        counter[i] = accumulate;
                        accumulate += oldCount;
                    }

                    for (int i = 0; i < count; ++i)
                    {
                        var current = collection[i];
                        var mapped = this.map.Invoke(current);
                        result[counter[mapped]] = current;
                        ++counter[mapped];
                    }

                    return result;
                }
            }
        }
    }

    /// <summary>
    /// Implmenta o ordenador de inteiros por contagem.
    /// </summary>
    public class CountingSorter : ISorter<uint>
    {
        /// <summary>
        /// Ordena uma coleção tomando o comparador por defeito.
        /// </summary>
        /// <param name="collection">A colecçao a ser ordenada.</param>
        public void Sort(IList<uint> collection)
        {
            var temp = this.GetSorted(collection);
            var count = temp.Count;
            for (int i = 0; i < count; ++i)
            {
                collection[i] = temp[i];
            }
        }

        /// <summary>
        /// Obtém a versão ordenada da colecção de inteiros.
        /// </summary>
        /// <param name="collection">A colecção.</param>
        /// <returns>A colecção ordenada.</returns>
        public IList<uint> GetSorted(IList<uint> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                var count = collection.Count;
                if (count == 0)
                {
                    return new uint[0];
                }
                else
                {
                    var max = collection[0];
                    for (int i = 1; i < count; ++i)
                    {
                        var current = collection[i];
                        if (current > max)
                        {
                            max = current;
                        }
                    }

                    ++max;
                    var counter = new uint[max];
                    for (int i = 0; i < count; ++i)
                    {
                        ++counter[collection[i]];
                    }

                    var result = new uint[count];
                    var accumulate = 0U;
                    for (int i = 0; i < max; ++i)
                    {
                        var oldCount = counter[i];
                        counter[i] = accumulate;
                        accumulate += oldCount;
                    }

                    for (int i = 0; i < count; ++i)
                    {
                        var current = collection[i];
                        result[counter[current]] = current;
                        ++counter[current];
                    }

                    return result;
                }
            }
        }
    }

    /// <summary>
    /// Implmenta o ordenador de inteiros por raiz.
    /// </summary>
    public class IterativeLsdRadixSorter : ISorter<uint>
    {
        /// <summary>
        /// A base da divisão.
        /// </summary>
        private int radix;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="IterativeLsdRadixSorter"/>.
        /// </summary>
        /// <param name="radix">A base da divisão.</param>
        public IterativeLsdRadixSorter(int radix)
        {
            if (radix < 2)
            {
                throw new UtilitiesException("Radix can't be less than 2 in Itearative LSD radix sorter.");
            }
            else
            {
                this.radix = radix;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor da base.
        /// </summary>
        public int Radix
        {
            get
            {
                return this.radix;
            }
            set
            {
                if (radix < 2)
                {
                    throw new UtilitiesException("Radix can't be less than 2 in Itearative LSD radix sorter.");

                }
                else
                {
                    this.radix = value;
                }
            }
        }

        /// <summary>
        /// Ordena uma coleção tomando o comparador por defeito.
        /// </summary>
        /// <param name="collection">A colecçao a ser ordenada.</param>
        public void Sort(IList<uint> collection)
        {
            var count = collection.Count;
            if (count > 0)
            {
                var prevRadix = 1;
                var innerRadix = this.radix;
                var radixCount = innerRadix;
                var buckets = new Queue<uint>[radixCount];
                for (int i = 0; i < innerRadix; ++i)
                {
                    buckets[i] = new Queue<uint>();
                }

                var state = true;
                while (state)
                {
                    state = false;
                    for (int i = 0; i < count; ++i)
                    {
                        var current = collection[i];
                        buckets[(current % innerRadix) / prevRadix].Enqueue(current);
                        if (current >= innerRadix)
                        {
                            state = true;
                        }
                    }

                    // Inclusão das filas na colecção.
                    var index = 0;
                    for (int i = 0; i < radixCount; ++i)
                    {
                        var currentBucket = buckets[i];
                        while (currentBucket.Count > 0)
                        {
                            collection[index] = currentBucket.Dequeue();
                            ++index;
                        }
                    }

                    prevRadix = innerRadix;
                    innerRadix *= radixCount;
                }
            }
        }
    }

    /// <summary>
    /// Aplica a ordenação lexicográfia de elementos baseada numa árvore associativa.
    /// </summary>
    /// <typeparam name="ItemType">O tipo de elementos que constituem as entradas da colecção.</typeparam>
    /// <typeparam name="CollectionType">O tipo de elementos que constituem a colecção.</typeparam>
    public class TrieLexicographicCollectionSorter<ItemType, CollectionType>
        : ISorter<CollectionType>
    {
        /// <summary>
        /// O indexador da colecção.
        /// </summary>
        private Func<CollectionType, IEnumerable<ItemType>> indexer;

        /// <summary>
        /// O comparador de itens.
        /// </summary>
        private IComparer<ItemType> itemsComparer;

        /// <summary>
        /// Determina o tipo de ordenação a ser aplicada.
        /// </summary>
        private OrderingType orderingType;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TrieLexicographicCollectionSorter{ItemType, CollectionType}"/>.
        /// </summary>
        /// <param name="indexer">O indexador da colecção.</param>
        /// <param name="orderingType">O tipo de ordenação a ser aplicada.</param>
        public TrieLexicographicCollectionSorter(
            Func<CollectionType, IEnumerable<ItemType>> indexer,
            OrderingType orderingType = OrderingType.LEXICOGRAPHIC)
        {
            if (indexer == null)
            {
                throw new ArgumentNullException("indexer");
            }
            else
            {
                this.indexer = indexer;
                this.itemsComparer = Comparer<ItemType>.Default;
                this.orderingType = orderingType;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TrieLexicographicCollectionSorter{ItemType, CollectionType}"/>.
        /// </summary>
        /// <param name="indexer">O indexador da colecção.</param>
        /// <param name="itemsComparer">O comparador dos constituintes da colecção.</param>
        /// <param name="orderingType">O tipo de ordenação a ser aplicada.</param>
        public TrieLexicographicCollectionSorter(
            Func<CollectionType, IEnumerable<ItemType>> indexer,
            IComparer<ItemType> itemsComparer,
            OrderingType orderingType = OrderingType.LEXICOGRAPHIC)
        {
            if (indexer == null)
            {
                throw new ArgumentNullException("indexer");
            }
            else if (itemsComparer == null)
            {
                throw new ArgumentNullException("itemsComparer");
            }
            else
            {
                this.indexer = indexer;
                this.itemsComparer = itemsComparer;
                this.orderingType = orderingType;
            }
        }

        /// <summary>
        /// Ordena uma coleção tomando o comparador por defeito.
        /// </summary>
        /// <param name="collection">A colecçao a ser ordenada.</param>
        public void Sort(IList<CollectionType> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                var counter = default(AuxTrieCounter);
                if (this.orderingType == OrderingType.LEXICOGRAPHIC)
                {
                    counter = new AuxTrieCounter(
                        collection,
                        this.itemsComparer,
                        this.indexer);
                }
                else if (this.orderingType == OrderingType.SHORTLEX)
                {
                    counter = new AuxShortLexTrieCounter(
                        collection,
                        this.itemsComparer,
                        this.indexer);
                }

                counter.FillInOrder(collection);
            }
        }

        /// <summary>
        /// Define o tipo de ordenação lexicográfica.
        /// </summary>
        public enum OrderingType
        {
            /// <summary>
            /// Ordenação lexicográfica normal.
            /// </summary>
            LEXICOGRAPHIC = 0,

            /// <summary>
            /// Ordenação lexicográfica dependente do comprimento.
            /// </summary>
            SHORTLEX = 1
        }

        /// <summary>
        /// Define uma árvore auxiliar.
        /// </summary>
        private class AuxTrieCounter
        {
            /// <summary>
            /// Mantém o valor das chaves.
            /// </summary>
            protected List<CollectionType> keys;

            /// <summary>
            /// Mantém o valor das contagens.
            /// </summary>
            protected List<int> counts;

            /// <summary>
            /// Mantém uma referência para a raiz.
            /// </summary>
            protected TrieNode root;

            /// <summary>
            /// O comparador de elementos.
            /// </summary>
            private IComparer<ItemType> comparer;

            /// <summary>
            /// O indexador.
            /// </summary>
            private Func<CollectionType, IEnumerable<ItemType>> indexer;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="AuxTrieCounter"/>.
            /// </summary>
            /// <param name="collection">A colecção.</param>
            /// <param name="comparer">O comparador de elementos.</param>
            /// <param name="indexer">O indexador.</param>
            public AuxTrieCounter(
                IList<CollectionType> collection, 
                IComparer<ItemType> comparer,
                Func<CollectionType, IEnumerable<ItemType>> indexer)
            {
                this.keys = new List<CollectionType>();
                this.counts = new List<int>();
                this.comparer = comparer;
                this.indexer = indexer;
                this.BuilTrie(collection, comparer);
            }

            /// <summary>
            /// Preenche a colecção com os itens na ordem correcta.
            /// </summary>
            /// <param name="collection">A colecção a ser preenchida.</param>
            public virtual void FillInOrder(IList<CollectionType> collection)
            {
                var stack = new Stack<IEnumerator<KeyValuePair<ItemType, TrieNode>>>();

                var index = 0;
                var current = this.root;
                var nodeNumber = current.NodeNumber;
                if (nodeNumber != -1)
                {
                    var key = this.keys[nodeNumber];
                    var itemsNumber = this.counts[nodeNumber];
                    for (int i = 0; i < itemsNumber; ++i)
                    {
                        collection[index] = key;
                        ++index;
                    }
                }

                stack.Push(this.root.ChildNodes.GetEnumerator());
                while (stack.Count > 0)
                {
                    var top = stack.Pop();
                    if (top.MoveNext())
                    {
                        stack.Push(top);
                        current = top.Current.Value;
                        nodeNumber = current.NodeNumber;
                        if (nodeNumber != -1)
                        {
                            var key = this.keys[nodeNumber];
                            var itemsNumber = this.counts[nodeNumber];
                            for (int i = 0; i < itemsNumber; ++i)
                            {
                                collection[index] = key;
                                ++index;
                            }
                        }

                        stack.Push(current.ChildNodes.GetEnumerator());
                    }
                }
            }

            /// <summary>
            /// Adiciona uma colecção, associando um índice, caso esta não exista.
            /// </summary>
            /// <param name="colEnumerator">O enumerador a ser adicionado.</param>
            /// <param name="associatedIndex">O índice que ficará associado.</param>
            /// <returns>
            /// O índice do objecto encontrado ou o índice que ficará associado
            /// se o objecto for inexistente.
            /// </returns>
            protected int AddIfNotExists(
                IEnumerator<ItemType> colEnumerator,
                int associatedIndex)
            {
                var currentNode = this.root;
                var state = colEnumerator.MoveNext();
                var aux = state;
                while (aux)
                {
                    var nextNode = default(TrieNode);
                    if (currentNode.ChildNodes.TryGetValue(
                        colEnumerator.Current,
                        out nextNode))
                    {
                        currentNode = nextNode;
                        aux = state = colEnumerator.MoveNext();
                    }
                    else
                    {
                        aux = false;
                    }
                }

                while (state)
                {
                    var nextNode = new TrieNode(this.comparer);
                    currentNode.ChildNodes.Add(colEnumerator.Current, nextNode);
                    currentNode = nextNode;
                    state = colEnumerator.MoveNext();
                }

                if (currentNode.NodeNumber == -1)
                {
                    currentNode.NodeNumber = associatedIndex;
                    return associatedIndex;
                }
                else
                {
                    // O item já existe.
                    return currentNode.NodeNumber;
                }
            }

            /// <summary>
            /// Constrói a árvore associativa com a contagem.
            /// </summary>
            /// <param name="collection">A colecção.</param>
            /// <param name="comparer">O comparador.</param>
            protected void BuilTrie(
                IList<CollectionType> collection, 
                IComparer<ItemType> comparer)
            {
                this.root = new TrieNode(comparer);
                var count = collection.Count;
                var index = 0;
                for (int i = 0; i < count; ++i)
                {
                    var current = collection[i];
                    var currentEnumerable = this.indexer.Invoke(current);
                    var nodeIndex = this.AddIfNotExists(
                        currentEnumerable.GetEnumerator(),
                        index);
                    if (nodeIndex == index)
                    {
                        this.keys.Add(current);
                        this.counts.Add(1);
                        ++index;
                    }
                    else
                    {
                        ++this.counts[nodeIndex];
                    }
                }
            }
        }

        /// <summary>
        /// Árvore auxiliar que permite preencher o vector na ordenação
        /// lexicográfica dependente do comprimento.
        /// </summary>
        private class AuxShortLexTrieCounter : AuxTrieCounter
        {
            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="AuxShortLexTrieCounter"/>.
            /// </summary>
            /// <param name="collection">A colecção.</param>
            /// <param name="comparer">O comparador.</param>
            /// <param name="indexer">O indexador.</param>
            public AuxShortLexTrieCounter(
                IList<CollectionType> collection,
                IComparer<ItemType> comparer,
                Func<CollectionType, IEnumerable<ItemType>> indexer)
                : base(collection, comparer, indexer) { }

            /// <summary>
            /// Preenche a colecção com os itens na ordem correcta.
            /// </summary>
            /// <param name="collection">A colecção a ser preenchida.</param>
            public override void FillInOrder(IList<CollectionType> collection)
            {
                var queue = new Queue<TrieNode>();

                var index = 0;
                queue.Enqueue(this.root);
                while (queue.Count > 0)
                {
                    var currentNode = queue.Dequeue();
                    var nodeNumber = currentNode.NodeNumber;
                    if (nodeNumber != -1)
                    {
                        var key = this.keys[nodeNumber];
                        var value = this.counts[nodeNumber];
                        for (int i = 0; i < value; ++i)
                        {
                            collection[index] = key;
                            ++index;
                        }
                    }

                    var nodeChilds = currentNode.ChildNodes;
                    foreach (var kvp in nodeChilds)
                    {
                        queue.Enqueue(kvp.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Define um nó da árvore associativa auxiliar.
        /// </summary>
        private class TrieNode
        {
            /// <summary>
            /// Mantém o número associado ao nó.
            /// </summary>
            private int nodeNumber;

            /// <summary>
            /// Mantém a lista dos descendentes.
            /// </summary>
            private SortedDictionary<ItemType, TrieNode> childNodes;

            /// <summary>
            /// Instancia um nova instância de objectos do tipo <see cref="TrieNode"/>.
            /// </summary>
            /// <param name="comparer">O comparador de itens.</param>
            public TrieNode(IComparer<ItemType> comparer)
            {
                this.childNodes = new SortedDictionary<ItemType, TrieNode>(
                    comparer);
                this.nodeNumber = -1;
            }

            /// <summary>
            /// Instancia um nova instância de objectos do tipo <see cref="TrieNode"/>.
            /// </summary>
            /// <param name="nodeNumber">O número do nó.</param>
            /// <param name="comparer">O comparador de itens.</param>
            public TrieNode(int nodeNumber, IComparer<ItemType> comparer)
            {
                this.childNodes = new SortedDictionary<ItemType, TrieNode>(
                    comparer);
                this.nodeNumber = nodeNumber;
            }

            /// <summary>
            /// Obtém ou atribui o número do nó.
            /// </summary>
            public int NodeNumber
            {
                get
                {
                    return this.nodeNumber;
                }
                set
                {
                    this.nodeNumber = value;
                }
            }

            /// <summary>
            /// Obtém uma referência para o dicionário que contém os nós descendentes.
            /// </summary>
            public IDictionary<ItemType, TrieNode> ChildNodes
            {
                get
                {
                    return this.childNodes;
                }
            }
        }
    }
}