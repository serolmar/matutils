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
        /// <param name="elements">Os elementos a serem ordenados.</param>
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
        /// <param name="elements">Os elementos a serem ordenados.</param>
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
    public class BubbleSort<T> : ISorter<T>
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
        /// <returns>O número de trocas efectuadas durante a ordenação.</retreturns>
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
        /// <returns>O número de trocas efectuadas durante a ordenação.</retreturns>
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
}
