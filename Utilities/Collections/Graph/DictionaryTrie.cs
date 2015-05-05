namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;
    using Utilities.Collections;

    /// <summary>
    /// Árvore associativa cujos nós são indexados por dicionários.
    /// </summary>
    /// <typeparam name="LabelType">O tipo dos objectos que constituem as etiquetas.</typeparam>
    /// <typeparam name="ColType">O tipo de objectos que representam as colecções.</typeparam>
    public class DictionaryTrie<LabelType, ColType>
        : ITrie<LabelType, ColType>, IIndexed<int, ColType>
        where ColType : IEnumerable<LabelType>
    {
        /// <summary>
        /// Fábrica responsável pela criação dos objectos de mapeamento.
        /// </summary>
        private IFactory<IDictionary<LabelType, TrieNode>> factory;

        /// <summary>
        /// Mantém uma referência para a raiz.
        /// </summary>
        private TrieNode root;

        /// <summary>
        /// Mantém uma lista com todos os elementos adicionados.
        /// </summary>
        private List<ColType> elements;

        /// <summary>
        /// Valor que indica se a colecção é só de leitura.
        /// </summary>
        private bool readOnly;

        /// <summary>
        /// Conjunto dos iteradores que se encontram activos.
        /// </summary>
        private HashSet<TrieIterator> activeIterators;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="DictionaryTrie{LabelType, ColType}"/>
        /// </summary>
        public DictionaryTrie()
        {
            this.readOnly = false;
            this.elements = new List<ColType>();
            this.activeIterators = new HashSet<TrieIterator>();
            this.factory =
                new DictionaryEqualityComparerFactory<LabelType,
                    TrieNode>(
                EqualityComparer<LabelType>.Default);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="DictionaryTrie{LabelType, ColType}"/>
        /// </summary>
        /// <param name="factory">A fábrica responsável pela criação dos contentores.</param>
        public DictionaryTrie(IFactory<IDictionary<LabelType,
            TrieNode>> factory)
        {
            if (factory == null)
            {
                this.factory = new DictionaryEqualityComparerFactory<LabelType,
                    TrieNode>(
                   EqualityComparer<LabelType>.Default);
            }
            else
            {
                this.factory = factory;
            }

            this.root = new TrieNode(this.factory);
            this.elements = new List<ColType>();
            this.activeIterators = new HashSet<TrieIterator>();
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="DictionaryTrie{LabelType, ColType}"/>
        /// </summary>
        /// <param name="cols">Um enumerável de colecções a serem adicionadas.</param>
        /// <param name="isReadOnly">Um parâmetro que indica se a colecção é só de leitura.</param>
        /// <param name="factory">A fábrica responsável pela criação dos contentores.</param>
        public DictionaryTrie(
            IEnumerable<ColType> cols,
            bool isReadOnly,
            IFactory<IDictionary<LabelType, TrieNode>> factory)
            : this(factory)
        {
            if (cols == null)
            {
                throw new ArgumentNullException("col");
            }
            else
            {
                this.readOnly = isReadOnly;

                // Adiciona cada uma das colecções à lista.
                foreach (var col in cols)
                {
                    this.InnerAdd(col);
                }
            }
        }

        /// <summary>
        /// Obtém o objecto especificado pelo índice.
        /// </summary>
        /// <value>
        /// O objecto.
        /// </value>
        /// <param name="index">O índice.</param>
        /// <returns>O objecto.</returns>
        public ColType this[int index]
        {
            get
            {
                return this.elements[index];
            }
        }

        /// <summary>
        /// Obtém o número de elementos na colecção.
        /// </summary>
        public int Count
        {
            get
            {
                return this.elements.Count;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a colecção é só de leitura.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return this.readOnly;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a colecção é segura em termos de vários 
        /// fluxos de execução.
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// O objecto sobre o qual é realizada a sincronização.
        /// </summary>
        /// <value>O valor da instância actual.</value>
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Obtém um iterador para a árvore.
        /// </summary>
        /// <returns>O iterador.</returns>
        public ITrieIterator<LabelType, ColType> GetIterator()
        {
            var iterator = new TrieIterator(this);
            this.activeIterators.Add(iterator);
            return iterator;
        }

        /// <summary>
        /// Adiciona um item à colecção.
        /// </summary>
        /// <param name="item">O item a ser adicionado.</param>
        /// <returns>Verdadeiro caso o item seja adicionado e falso caso contrário.</returns>
        public bool Add(ColType item)
        {
            if (this.readOnly)
            {
                throw new UtilitiesException("The collection is readonly.");
            }
            else
            {
                return this.InnerAdd(item);
            }
        }

        /// <summary>
        /// Remove todos os elementos especificados da colecção.
        /// </summary>
        /// <param name="other">O enumerável com os elementos a serem removidos.</param>
        public void ExceptWith(IEnumerable<ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.readOnly)
            {
                throw new UtilitiesException("The collection is readonly.");
            }
            else
            {
                foreach (var col in other)
                {
                    this.Remove(col);
                }
            }
        }

        /// <summary>
        /// Remove todos os elementos especificados da colecção.
        /// </summary>
        /// <param name="other">A árvore associativa com os elementos a serem removidos.</param>
        public void ExceptWith(DictionaryTrie<LabelType, ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.readOnly)
            {
                throw new UtilitiesException("The collection is readonly.");
            }
            else
            {
                var thisStack = new Stack<Tuple<
                    TrieNode,
                    List<LabelType>,
                    IEnumerator<KeyValuePair<LabelType, TrieNode>>>>();
                var otherStack = new Stack<TrieNode>();
                var remove = new InsertionSortedCollection<int>(Comparer<int>.Default);

                if (this.root.NodeNumber != -1 &&
                    other.root.NodeNumber != -1)
                {
                    remove.Add(this.root.NodeNumber);
                }

                thisStack.Push(Tuple.Create(
                    this.root,
                    new List<LabelType>(),
                    this.root.ChildNodes.GetEnumerator()));
                otherStack.Push(other.root);
                while (thisStack.Count > 0)
                {
                    var thisTop = thisStack.Peek();
                    if (thisTop.Item3.MoveNext())
                    {
                        var current = thisTop.Item3.Current;
                        var otherNode = otherStack.Peek();
                        var otherNextNode = default(TrieNode);
                        if (otherNode.ChildNodes.TryGetValue(
                            current.Key,
                            out otherNextNode))
                        {
                            if (current.Value.NodeNumber != -1)
                            {
                                if (otherNextNode.NodeNumber != -1)
                                {
                                    remove.Add(current.Value.NodeNumber);
                                    current.Value.NodeNumber = -1;
                                }
                            }

                            // Adicionam-se os nós do nível seguinte
                            thisStack.Push(Tuple.Create(
                                current.Value,
                                new List<LabelType>(),
                                current.Value.ChildNodes.GetEnumerator()));
                            otherStack.Push(otherNextNode);
                        }
                    }
                    else
                    {
                        // Remoção de todos os itens marcados para serem removidos
                        foreach (var item in thisTop.Item2)
                        {
                            thisTop.Item1.ChildNodes.Remove(item);
                        }

                        thisStack.Pop();
                        otherStack.Pop();
                        if (thisTop.Item1.NodeNumber == -1
                            && thisTop.Item1.ChildNodes.Count == 0)
                        {
                            if (thisStack.Count > 0)
                            {
                                var previousNode = thisStack.Peek();
                                previousNode.Item2.Add(
                                    previousNode.Item3.Current.Key);
                            }
                        }
                    }
                }

                // Remove todos os eleemntos marcados
                var offset = 0;
                foreach (var index in remove)
                {
                    this.elements.RemoveAt(index - offset);
                    ++offset;
                }
            }
        }

        /// <summary>
        /// Altera a colecção actual de modo a conter apenas os elementos que
        /// também se encontram especificados numa árvore associativa.
        /// </summary>
        /// <param name="other">A árvore associativa.</param>
        public void IntersectWith(IEnumerable<ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.readOnly)
            {
                throw new UtilitiesException("The collection is readonly.");
            }
            else
            {
                var existing = new HashSet<int>();
                foreach (var col in other)
                {
                    var nodeValue = this.RemoveCollectionFromTrie(col);
                    if (nodeValue != -1)
                    {
                        existing.Add(nodeValue);
                    }
                }

                foreach (var i in existing)
                {
                    this.elements.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Altera a colecção actual de modo a conter apenas os elementos que
        /// também se encontram especificados numa árvore associativa.
        /// </summary>
        /// <param name="other">A árvore associativa.</param>
        public void IntersectWith(DictionaryTrie<LabelType, ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.readOnly)
            {
                throw new UtilitiesException("The collection is readonly.");
            }
            else
            {
                var thisStack = new Stack<Tuple<
                    TrieNode,
                    List<LabelType>,
                    IEnumerator<KeyValuePair<LabelType, TrieNode>>>>();
                var otherStack = new Stack<TrieNode>();
                var remove = new InsertionSortedCollection<int>(Comparer<int>.Default);

                if (this.root.NodeNumber != -1 &&
                    other.root.NodeNumber == -1)
                {
                    remove.Add(this.root.NodeNumber);
                }

                thisStack.Push(Tuple.Create(
                    this.root,
                    new List<LabelType>(),
                    this.root.ChildNodes.GetEnumerator()));
                otherStack.Push(other.root);
                while (thisStack.Count > 0)
                {
                    var thisTop = thisStack.Peek();
                    var current = thisTop.Item3.Current;
                    if (thisTop.Item3.MoveNext())
                    {
                        var otherNode = otherStack.Peek();
                        var otherNextNode = default(TrieNode);
                        if (otherNode.ChildNodes.TryGetValue(
                            current.Key,
                            out otherNextNode))
                        {
                            if (current.Value.NodeNumber != -1)
                            {
                                if (otherNextNode.NodeNumber == -1)
                                {
                                    remove.Add(current.Value.NodeNumber);
                                }
                            }

                            // Adicionam-se os nós do nível seguinte
                            thisStack.Push(Tuple.Create(
                                current.Value,
                                new List<LabelType>(),
                                current.Value.ChildNodes.GetEnumerator()));
                            otherStack.Push(otherNextNode);
                        }
                        else
                        {
                            thisTop.Item2.Add(current.Key);
                            this.RemoveNodeDescendants(current.Value, remove);
                        }
                    }
                    else
                    {
                        // Remoção de todos os itens marcados para serem removidos
                        foreach (var item in thisTop.Item2)
                        {
                            thisTop.Item1.ChildNodes.Remove(item);
                        }

                        if (current.Value.NodeNumber == -1
                            && thisTop.Item1.ChildNodes.Count == 0)
                        {
                            if (thisStack.Count > 0)
                            {
                                var previousNode = thisStack.Peek();
                                previousNode.Item2.Add(
                                    previousNode.Item3.Current.Key);
                            }
                        }

                        thisStack.Pop();
                        otherStack.Pop();
                    }
                }

                // Remove todos os eleemntos marcados
                foreach (var index in remove)
                {
                    this.elements.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// Determina se a colecção actual é subconjunto próprio de um enumerável.
        /// </summary>
        /// <param name="other">O enumerável.</param>
        /// <returns>
        /// Veradeiro caso a colecção actual seja um subconjunto do enumerável e falso caso contrário.
        /// </returns>
        public bool IsProperSubsetOf(IEnumerable<ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else
            {
                var found = new HashSet<int>();
                var notFound = false;
                foreach (var col in other)
                {
                    var nodeValue = this.GetAssociatedNodeValue(col);
                    if (nodeValue != -1)
                    {
                        if (!found.Contains(nodeValue))
                        {
                            found.Add(nodeValue);
                        }
                    }
                    else
                    {
                        notFound = true;
                    }
                }

                return found.Count == this.elements.Count && notFound;
            }
        }

        /// <summary>
        /// Determina se a colecção actual é subconjunto próprio de uma árvore associativa.
        /// </summary>
        /// <param name="other">A árvore associativa.</param>
        /// <returns>
        /// Veradeiro caso a colecção actual seja um subconjunto da árvore associativa e falso caso contrário.
        /// </returns>
        public bool IsProperSubsetOf(DictionaryTrie<LabelType, ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.elements.Count < other.elements.Count)
            {
                if (this.root.NodeNumber == -1 || other.root.NodeNumber != -1)
                {
                    if (this.root.ChildNodes.Count <= other.root.ChildNodes.Count)
                    {
                        var thisStack = new Stack<IEnumerator<KeyValuePair<LabelType, TrieNode>>>();
                        var otherStack = new Stack<TrieNode>();
                        thisStack.Push(this.root.ChildNodes.GetEnumerator());
                        otherStack.Push(other.root);
                        while (otherStack.Count > 0)
                        {
                            var top = thisStack.Peek();
                            if (top.MoveNext())
                            {
                                var otherNode = otherStack.Peek();
                                var current = top.Current;
                                var nextNode = default(TrieNode);
                                if (otherNode.ChildNodes.TryGetValue(
                                    current.Key,
                                    out nextNode))
                                {
                                    if (current.Value.ChildNodes.Count <= nextNode.ChildNodes.Count)
                                    {
                                        if (current.Value.NodeNumber == -1 || otherNode.NodeNumber != -1)
                                        {
                                            thisStack.Push(current.Value.ChildNodes.GetEnumerator());
                                            otherStack.Push(nextNode);
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                thisStack.Pop();
                                otherStack.Pop();
                            }
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determina se a colecção actual é um superconjunto próprio de um enumerável.
        /// </summary>
        /// <param name="other">O enumerável.</param>
        /// <returns>
        /// Verdadeiro caso a coleçcão actual seja um superconjunto do enumerável e falso caso contrário.
        /// </returns>
        public bool IsProperSupersetOf(IEnumerable<ColType> other)
        {
            var found = new HashSet<int>();
            foreach (var col in other)
            {
                var associatedNodeValue = this.GetAssociatedNodeValue(col);
                if (associatedNodeValue == -1)
                {
                    return false;
                }
                else
                {
                    if (!found.Contains(associatedNodeValue))
                    {
                        found.Add(associatedNodeValue);
                    }
                }
            }

            return found.Count < this.elements.Count;
        }

        /// <summary>
        /// Determina se a colecção actual é um superconjunto próprio de uma árvore associativa.
        /// </summary>
        /// <param name="other">A árvore associativa.</param>
        /// <returns>
        /// Verdadeiro caso a coleçcão actual seja um superconjunto da árvore associativa e falso caso contrário.
        /// </returns>
        public bool IsProperSupersetOf(DictionaryTrie<LabelType, ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.elements.Count > other.elements.Count)
            {
                if (this.root.ChildNodes.Count >= other.root.ChildNodes.Count)
                {
                    if (this.root.NodeNumber != -1 || other.root.NodeNumber == -1)
                    {
                        var otherStack = new Stack<IEnumerator<KeyValuePair<LabelType, TrieNode>>>();
                        var thisStack = new Stack<TrieNode>();
                        otherStack.Push(other.root.ChildNodes.GetEnumerator());
                        thisStack.Push(this.root);
                        while (otherStack.Count > 0)
                        {
                            var otherTop = otherStack.Peek();
                            if (otherTop.MoveNext())
                            {
                                var otherCurrent = otherTop.Current;
                                var thisNode = thisStack.Peek();
                                var nextNode = default(TrieNode);
                                if (thisNode.ChildNodes.TryGetValue(
                                    otherCurrent.Key,
                                    out nextNode))
                                {
                                    if (nextNode.ChildNodes.Count >= otherCurrent.Value.ChildNodes.Count)
                                    {
                                        if (nextNode.NodeNumber != -1 || otherCurrent.Value.NodeNumber == -1)
                                        {
                                            otherStack.Push(otherCurrent.Value.ChildNodes.GetEnumerator());
                                            thisStack.Push(nextNode);
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                otherStack.Pop();
                                thisStack.Pop();
                            }
                        }

                        throw new NotImplementedException();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determina se a colecção actual é subconjunto de um enumerável.
        /// </summary>
        /// <param name="other">O enumerável.</param>
        /// <returns>
        /// Veradeiro caso a colecção actual seja um subconjunto do enumerável e falso caso contrário.
        /// </returns>
        public bool IsSubsetOf(IEnumerable<ColType> other)
        {
            var found = new HashSet<int>();
            foreach (var col in other)
            {
                var nodeValue = this.GetAssociatedNodeValue(col);
                if (nodeValue != -1)
                {
                    if (!found.Contains(nodeValue))
                    {
                        found.Add(nodeValue);
                    }
                }
            }

            return found.Count == this.elements.Count;
        }

        /// <summary>
        /// Determina se a colecção actual é subconjunto de uma árvore associativa.
        /// </summary>
        /// <param name="other">A árvore associativa.</param>
        /// <returns>
        /// Veradeiro caso a colecção actual seja um subconjunto da árvore associativa e falso caso contrário.
        /// </returns>
        public bool IsSubsetOf(DictionaryTrie<LabelType, ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.elements.Count <= other.elements.Count)
            {
                if (this.root.NodeNumber == -1 || other.root.NodeNumber != -1)
                {
                    if (this.root.ChildNodes.Count <= other.root.ChildNodes.Count)
                    {
                        var thisStack = new Stack<IEnumerator<KeyValuePair<LabelType, TrieNode>>>();
                        var otherStack = new Stack<TrieNode>();
                        thisStack.Push(this.root.ChildNodes.GetEnumerator());
                        otherStack.Push(other.root);
                        while (otherStack.Count > 0)
                        {
                            var top = thisStack.Peek();
                            if (top.MoveNext())
                            {
                                var otherNode = otherStack.Peek();
                                var current = top.Current;
                                var nextNode = default(TrieNode);
                                if (otherNode.ChildNodes.TryGetValue(
                                    current.Key,
                                    out nextNode))
                                {
                                    if (current.Value.ChildNodes.Count <= nextNode.ChildNodes.Count)
                                    {
                                        if (current.Value.NodeNumber == -1 || otherNode.NodeNumber != -1)
                                        {
                                            thisStack.Push(current.Value.ChildNodes.GetEnumerator());
                                            otherStack.Push(nextNode);
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                thisStack.Pop();
                                otherStack.Pop();
                            }
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determina se a colecção actual é um superconjunto de um enumerável.
        /// </summary>
        /// <param name="other">O enumerável.</param>
        /// <returns>
        /// Verdadeiro caso a coleçcão actual seja um superconjunto do enumerável e falso caso contrário.
        /// </returns>
        public bool IsSupersetOf(IEnumerable<ColType> other)
        {
            foreach (var col in other)
            {
                if (this.GetAssociatedNodeValue(col) == -1)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determina se a colecção actual é um superconjunto de uma árvore associativa.
        /// </summary>
        /// <param name="other">A árvore associativa.</param>
        /// <returns>
        /// Verdadeiro caso a coleçcão actual seja um superconjunto da árvore associativa e falso caso contrário.
        /// </returns>
        public bool IsSupersetOf(DictionaryTrie<LabelType, ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.elements.Count >= other.elements.Count)
            {
                if (this.root.ChildNodes.Count >= other.root.ChildNodes.Count)
                {
                    if (this.root.NodeNumber != -1 || other.root.NodeNumber == -1)
                    {
                        var otherStack = new Stack<IEnumerator<KeyValuePair<LabelType, TrieNode>>>();
                        var thisStack = new Stack<TrieNode>();
                        otherStack.Push(other.root.ChildNodes.GetEnumerator());
                        thisStack.Push(this.root);
                        while (otherStack.Count > 0)
                        {
                            var otherTop = otherStack.Peek();
                            if (otherTop.MoveNext())
                            {
                                var otherCurrent = otherTop.Current;
                                var thisNode = thisStack.Peek();
                                var nextNode = default(TrieNode);
                                if (thisNode.ChildNodes.TryGetValue(
                                    otherCurrent.Key,
                                    out nextNode))
                                {
                                    if (nextNode.ChildNodes.Count >= otherCurrent.Value.ChildNodes.Count)
                                    {
                                        if (nextNode.NodeNumber != -1 || otherCurrent.Value.NodeNumber == -1)
                                        {
                                            otherStack.Push(otherCurrent.Value.ChildNodes.GetEnumerator());
                                            thisStack.Push(nextNode);
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                otherStack.Pop();
                                thisStack.Pop();
                            }
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica se a colecção actual contém elementos em comum com
        /// um enumerável.
        /// </summary>
        /// <param name="other">O enumerável.</param>
        /// <returns>Verdadeiro caso a colecção possua algum elemento em comum e falso caso contrário.</returns>
        public bool Overlaps(IEnumerable<ColType> other)
        {
            foreach (var col in other)
            {
                if (this.GetAssociatedNodeValue(col) != -1)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Verifica se a colecção actual contém elementos em comum com
        /// uma árvore associativa.
        /// </summary>
        /// <param name="other">A árvore associativa.</param>
        /// <returns>Verdadeiro caso a colecção possua algum elemento em comum e falso caso contrário.</returns>
        public bool Overlaps(DictionaryTrie<LabelType, ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.root.NodeNumber != -1 && other.root.NodeNumber != -1)
            {
                return true;
            }
            else
            {
                var thisStack = new Stack<IEnumerator<KeyValuePair<LabelType, TrieNode>>>();
                var otherStack = new Stack<TrieNode>();
                thisStack.Push(this.root.ChildNodes.GetEnumerator());
                otherStack.Push(other.root);
                while (thisStack.Count > 0)
                {
                    var top = thisStack.Peek();
                    if (top.MoveNext())
                    {
                        var otherTop = otherStack.Peek();
                        var otherNode = default(TrieNode);
                        if (otherTop.ChildNodes.TryGetValue(
                            top.Current.Key,
                            out otherNode))
                        {
                            if (otherNode.NodeNumber != -1 && top.Current.Value.NodeNumber != -1)
                            {
                                return true;
                            }
                            else
                            {
                                thisStack.Push(top.Current.Value.ChildNodes.GetEnumerator());
                                otherStack.Push(otherNode);
                            }
                        }
                    }
                    else
                    {
                        thisStack.Pop();
                        otherStack.Pop();
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Verifica se a colecção actual possui os mesmos elementos que um enumerável.
        /// </summary>
        /// <param name="other">O enumerável.</param>
        /// <returns>
        /// Verdadeiro caso a colecção e o enumerável possuam os mesmos elementos e falso caso contrário.
        /// </returns>
        public bool SetEquals(IEnumerable<ColType> other)
        {
            var found = new HashSet<int>();
            foreach (var col in other)
            {
                var nodeValue = this.GetAssociatedNodeValue(col);
                if (nodeValue == -1)
                {
                    return false;
                }
                else
                {
                    if (!found.Contains(nodeValue))
                    {
                        found.Add(nodeValue);
                    }
                }
            }

            return found.Count == this.elements.Count;
        }

        /// <summary>
        /// Verifica se a colecção actual possui os mesmos elementos que uma árvore associativa.
        /// </summary>
        /// <param name="other">A árvore associativa.</param>
        /// <returns>
        /// Verdadeiro caso a colecção e a árvore associativa possuam os mesmos elementos e falso caso contrário.
        /// </returns>
        public bool SetEquals(DictionaryTrie<LabelType, ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.elements.Count == other.elements.Count)
            {
                if ((this.root.NodeNumber != -1 && other.root.NodeNumber == -1) ||
                    this.root.NodeNumber == -1 && other.root.NodeNumber != -1)
                {
                    // A sequência vazia não se encontra em ambos os conjuntos.
                    return false;
                }
                else if (this.root.ChildNodes.Count != other.root.ChildNodes.Count)
                {
                    // O número de ramos é diferente
                    return false;
                }
                else
                {
                    var thisStack = new Stack<IEnumerator<KeyValuePair<LabelType, TrieNode>>>();
                    var otherStack = new Stack<TrieNode>();
                    thisStack.Push(this.root.ChildNodes.GetEnumerator());
                    otherStack.Push(other.root);
                    while (thisStack.Count > 0)
                    {
                        var top = thisStack.Peek();
                        var otherNode = otherStack.Peek();
                        if (top.MoveNext())
                        {
                            var current = top.Current;
                            var nextNode = default(TrieNode);
                            if (otherNode.ChildNodes.TryGetValue(
                                current.Key,
                                out nextNode))
                            {
                                if ((current.Value.NodeNumber == -1 && nextNode.NodeNumber != -1) ||
                                    (current.Value.NodeNumber != -1 && nextNode.NodeNumber == -1))
                                {
                                    return false;
                                }
                                else
                                {
                                    if (current.Value.ChildNodes.Count == nextNode.ChildNodes.Count)
                                    {
                                        thisStack.Push(current.Value.ChildNodes.GetEnumerator());
                                        otherStack.Push(nextNode);
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            thisStack.Pop();
                            otherStack.Pop();
                        }
                    }

                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Altera a colecção actual de modo a que possua apenas os elementos que
        /// estão contidos na colecção ou no enumeravel mas não em ambos simultaneamente.
        /// </summary>
        /// <param name="other">O enumerável.</param>
        public void SymmetricExceptWith(IEnumerable<ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.readOnly)
            {
                throw new UtilitiesException("The collection is readonly.");
            }
            else
            {
                var found = new HashSet<int>();
                foreach (var col in other)
                {
                    var nodeValue = this.GetAssociatedNodeValue(col);
                    if (nodeValue != -1)
                    {
                        if (!found.Contains(nodeValue))
                        {
                            found.Add(nodeValue);
                        }
                    }
                }

                var remove = new List<int>();
                for (var i = 0; i < this.elements.Count; ++i)
                {
                    if (!found.Contains(i))
                    {
                        remove.Add(i);
                    }
                }

                for (int i = 0; i < remove.Count; ++i)
                {
                    this.RemoveCollectionFromTrie(this.elements[i]);
                }

                for (int i = 0; i < remove.Count; ++i)
                {
                    this.elements.RemoveAt(remove[i]);
                }
            }
        }

        /// <summary>
        /// Altera a colecção actual de modo a que possua apenas os elementos que
        /// estão contidos na colecção ou na árvore associativa mas não em ambos simultaneamente.
        /// </summary>
        /// <param name="other">A árvore associativa.</param>
        public void SymmetricExceptWith(DictionaryTrie<LabelType, ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.readOnly)
            {
                throw new UtilitiesException("The collection is readonly.");
            }
            else
            {
                var elementsToRemove = new InsertionSortedCollection<int>(Comparer<int>.Default);

                // Remove a lista vazia caso não se encontre em ambos os conjuntos
                if (other.root.NodeNumber == -1)
                {
                    if (this.root.NodeNumber != -1)
                    {
                        elementsToRemove.Add(this.root.NodeNumber);
                        this.root.NodeNumber = -1;
                    }
                }

                // Em cada iteração mantém o estado do enumerador e o conjunto de elementos a serem removidos
                var thisStack = new Stack<Tuple<TrieNode, List<LabelType>, IEnumerator<KeyValuePair<LabelType, TrieNode>>>>();

                var otherStack = new Stack<TrieNode>();
                thisStack.Push(Tuple.Create(this.root, new List<LabelType>(), this.root.ChildNodes.GetEnumerator()));
                otherStack.Push(other.root);
                while (thisStack.Count > 0)
                {
                    var top = thisStack.Peek();
                    if (top.Item3.MoveNext())
                    {
                        var otherNode = otherStack.Peek();
                        var current = top.Item3.Current;
                        var otherNextNode = default(TrieNode);
                        if (otherNode.ChildNodes.TryGetValue(
                            current.Key,
                            out otherNextNode))
                        {
                            if (current.Value.ChildNodes.Count == 0)
                            {
                                if (otherNextNode.NodeNumber == -1)
                                {
                                    if (current.Value.NodeNumber != -1)
                                    {
                                        elementsToRemove.Add(current.Value.NodeNumber);
                                    }

                                    // Marca para remoção
                                    top.Item2.Add(current.Key);
                                }
                            }
                            else
                            {
                                if (otherNextNode.NodeNumber == -1)
                                {
                                    if (current.Value.NodeNumber != -1)
                                    {
                                        elementsToRemove.Add(current.Value.NodeNumber);
                                        current.Value.NodeNumber = -1;
                                    }
                                }

                                thisStack.Push(Tuple.Create(
                                    current.Value,
                                    new List<LabelType>(),
                                    current.Value.ChildNodes.GetEnumerator()));
                                otherStack.Push(otherNextNode);
                            }
                        }
                        else
                        {
                            this.RemoveNodeDescendants(current.Value, elementsToRemove);
                            top.Item2.Add(current.Key);
                        }
                    }
                    else
                    {
                        thisStack.Pop();
                        otherStack.Pop();
                        foreach (var item in top.Item2)
                        {
                            top.Item1.ChildNodes.Remove(item);
                        }

                        if (top.Item1.NodeNumber == -1
                            && top.Item1.ChildNodes.Count == 0)
                        {
                            if (thisStack.Count > 0)
                            {
                                // Marca o nó para ser removido
                                var previousNode = thisStack.Peek();
                                previousNode.Item2.Add(
                                    previousNode.Item3.Current.Key);
                            }
                        }
                    }

                    // Remove todos os eleemntos marcados
                    foreach (var index in elementsToRemove)
                    {
                        this.elements.RemoveAt(index);
                    }
                }
            }
        }

        /// <summary>
        /// Determina a união da colecção actual com um enumerável.
        /// </summary>
        /// <param name="other">O enumerável.</param>
        public void UnionWith(IEnumerable<ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.readOnly)
            {
                throw new UtilitiesException("The collection is readonly.");
            }
            else
            {
                foreach (var col in other)
                {
                    this.InnerAdd(col);
                }
            }
        }

        /// <summary>
        /// Determina a união da colecção actual com uma árvore associativa.
        /// </summary>
        /// <param name="other">A árvore associativa.</param>
        public void UnionWith(DictionaryTrie<LabelType, ColType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (this.readOnly)
            {
                throw new UtilitiesException("The collection is readonly.");
            }
            else
            {
                // Verifica se a raiz possui a sequência vazia
                if (this.root.NodeNumber == -1 && other.root.NodeNumber != -1)
                {
                    this.root.NodeNumber = this.elements.Count;
                    this.elements.Add(other.elements[other.root.NodeNumber]);
                }

                if (other.root.ChildNodes.Count > 0)
                {
                    var otherStack = new Stack<IEnumerator<KeyValuePair<LabelType, TrieNode>>>();
                    var thisStack = new Stack<TrieNode>();
                    otherStack.Push(other.root.ChildNodes.GetEnumerator());
                    thisStack.Push(this.root);
                    while (otherStack.Count > 0)
                    {
                        var top = otherStack.Peek();
                        if (top.MoveNext())
                        {
                            var thisNode = thisStack.Peek();
                            var otherCurrent = top.Current;
                            var thisNextNode = default(TrieNode);
                            if (thisNode.ChildNodes.TryGetValue(
                                otherCurrent.Key,
                                out thisNextNode))
                            {
                                if (thisNextNode.NodeNumber == -1 && otherCurrent.Value.NodeNumber != -1)
                                {
                                    thisNextNode.NodeNumber = this.elements.Count;
                                    this.elements.Add(other.elements[otherCurrent.Value.NodeNumber]);
                                }

                                if (otherCurrent.Value.ChildNodes.Count > 0)
                                {
                                    otherStack.Push(otherCurrent.Value.ChildNodes.GetEnumerator());
                                    thisStack.Push(thisNextNode);
                                }
                            }
                            else
                            {
                                // A sub-árvore não existe e terá de ser aqui construída
                                this.AppendSubTree(thisNode, otherCurrent.Value, other.elements);
                            }
                        }
                        else
                        {
                            otherStack.Pop();
                            thisStack.Pop();
                        }
                    }

                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Adiciona um item à colecção.
        /// </summary>
        /// <param name="item">O item a ser adicionado.</param>
        void ICollection<ColType>.Add(ColType item)
        {
            this.Add(item);
        }

        /// <summary>
        /// Elimina todos os elementos da colecção.
        /// </summary>
        public void Clear()
        {
            if (this.readOnly)
            {
                throw new UtilitiesException("The collection is readonly.");
            }
            else
            {
                var nodeStack = new Stack<Tuple<TrieNode, IEnumerator<KeyValuePair<LabelType, TrieNode>>>>();
                nodeStack.Push(Tuple.Create(this.root, this.root.ChildNodes.GetEnumerator()));
                while (nodeStack.Count > 0)
                {
                    var currentInfo = nodeStack.Pop();
                    if (currentInfo.Item2.MoveNext())
                    {
                        var nextNode = currentInfo.Item2.Current.Value;
                        nodeStack.Push(currentInfo);
                        nodeStack.Push(
                            Tuple.Create(nextNode, nextNode.ChildNodes.GetEnumerator()));
                    }
                    else
                    {
                        currentInfo.Item1.ChildNodes.Clear();
                    }
                }

                this.elements.Clear();
            }
        }

        /// <summary>
        /// Verifica se um determinado item se encontra na colecção.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>Verdadeiro caso o item se encontre na colecção e falso caso contrário.</returns>
        public bool Contains(ColType item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            else
            {
                var nodeValue = this.GetAssociatedNodeValue(item);
                return nodeValue != -1;
            }
        }

        /// <summary>
        /// Copia os elementos da colecção para um vector.
        /// </summary>
        /// <param name="array">O vector.</param>
        /// <param name="arrayIndex">O índice no vector onde se inicia a cópia.</param>
        public void CopyTo(ColType[] array, int arrayIndex)
        {
            this.elements.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Elimina um item da colecção.
        /// </summary>
        /// <param name="item">O item a ser eliminado.</param>
        /// <returns>Verdadeiro se a operação for bem-sucedida e falso caso contrário.</returns>
        public bool Remove(ColType item)
        {
            if (item == null)
            {
                return false;
            }
            else if (this.readOnly)
            {
                throw new UtilitiesException("The collection is readonly.");
            }
            else
            {
                var deletedIndex = this.RemoveCollectionFromTrie(item);
                if (deletedIndex == -1)
                {
                    return false;
                }
                else
                {
                    this.UpdateTrieNodeNumbers(deletedIndex);
                    this.elements.RemoveAt(deletedIndex);
                    return true;
                }
            }
        }

        /// <summary>
        /// Copia os elementos da colecção para o vector.
        /// </summary>
        /// <param name="array">O vector.</param>
        /// <param name="index">A posição do vector onde se pretende iniciar a cópia.</param>
        public void CopyTo(Array array, int index)
        {
            (this.elements as ICollection).CopyTo(
                array, 
                index);
        }

        /// <summary>
        /// Obtém um enumerador para a colecção.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<ColType> GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }

        /// <summary>
        /// Otbém um enumerador não genérico para a colecção.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }

        /// <summary>
        /// Permite remover uma determinada colecção da árvore associativa
        /// e retornar o índice do elemento removido.
        /// </summary>
        /// <param name="col">A colecção a ser removida.</param>
        /// <returns>Um índice superior a 0 caso exista e -1 caso contrário.</returns>
        private int RemoveCollectionFromTrie(ColType col)
        {
            var currentNode = this.root;
            var nodeStack = new Stack<TrieNode>();
            var itemStack = new Stack<LabelType>();
            nodeStack.Push(currentNode);

            var itemEnumerator = col.GetEnumerator();
            var state = itemEnumerator.MoveNext();

            while (state)
            {
                var currentItem = itemEnumerator.Current;
                var nextNode = default(TrieNode);
                if (currentNode.ChildNodes.TryGetValue(
                    itemEnumerator.Current,
                    out nextNode))
                {
                    nodeStack.Push(nextNode);
                    itemStack.Push(currentItem);
                    currentNode = nextNode;
                    state = itemEnumerator.MoveNext();
                }
                else
                {
                    return -1;
                }
            }

            if (currentNode.NodeNumber == -1)
            {
                return -1;
            }
            else
            {
                currentNode = nodeStack.Pop();
                var deleteIndex = currentNode.NodeNumber;
                currentNode.NodeNumber = -1;
                if (currentNode.ChildNodes.Count == 0)
                {
                    state = nodeStack.Count > 0;
                    if (state)
                    {
                        var previousNode = nodeStack.Pop();
                        if (previousNode.ChildNodes.Count == 1)
                        {
                            previousNode.ChildNodes.Clear();
                            var lastRemovedNode = currentNode;
                            currentNode = previousNode;
                            state = nodeStack.Count > 0;
                            while (state)
                            {
                                previousNode = nodeStack.Pop();
                                if (previousNode.ChildNodes.Count > 1)
                                {
                                    previousNode.ChildNodes.Clear();
                                    lastRemovedNode = currentNode;
                                    currentNode = previousNode;
                                    state = nodeStack.Count > 0;
                                }
                                else
                                {
                                    state = false;
                                }
                            }

                            foreach (var activeIterator in this.activeIterators)
                            {
                                activeIterator.Invalidate(lastRemovedNode);
                            }
                        }
                    }
                }

                return deleteIndex;
            }
        }

        /// <summary>
        /// Actualiza os nós da árvore associativa.
        /// </summary>
        /// <param name="remove">O item marcado para remoção.</param>
        private void UpdateTrieNodeNumbers(int remove)
        {
            if (this.root.NodeNumber != -1 && this.root.NodeNumber > remove)
            {
                --this.root.NodeNumber;
            }

            var stack = new Stack<IEnumerator<KeyValuePair<LabelType, TrieNode>>>();
            stack.Push(this.root.ChildNodes.GetEnumerator());
            while (stack.Count > 0)
            {
                var top = stack.Peek();
                if (top.MoveNext())
                {
                    var child = top.Current.Value;
                    if (child.NodeNumber != -1 && child.NodeNumber > remove)
                    {
                        --child.NodeNumber;
                    }

                    stack.Push(child.ChildNodes.GetEnumerator());
                }
                else
                {
                    stack.Pop();
                }
            }
        }

        /// <summary>
        /// Actualiza os nós da árvore associativa.
        /// </summary>
        /// <param name="remove">Os itens marcados para remoção.</param>
        private void UpdateTrieNodeNumbers(InsertionSortedCollection<int> remove)
        {
            if (this.root.NodeNumber != -1)
            {
                var count = remove.CountLessThan(this.root.NodeNumber);
                this.root.NodeNumber -= count;
            }

            var stack = new Stack<IEnumerator<KeyValuePair<LabelType, TrieNode>>>();
            stack.Push(this.root.ChildNodes.GetEnumerator());
            while (stack.Count > 0)
            {
                var top = stack.Peek();
                if (top.MoveNext())
                {
                    var child = top.Current.Value;
                    if (child.NodeNumber != -1)
                    {
                        var count = remove.CountLessThan(child.NodeNumber);
                        child.NodeNumber -= count;
                    }

                    stack.Push(child.ChildNodes.GetEnumerator());
                }
                else
                {
                    stack.Pop();
                }
            }
        }

        /// <summary>
        /// Adiciona um item à colecção.
        /// </summary>
        /// <remarks>
        /// Esta função será executada por dois métodos públicos
        /// idênticos mas pertencentes a interfaces implementadas diferentes.
        /// </remarks>
        /// <param name="item">O item a ser adicionado.</param>
        /// <returns>Verdadeiro se a operação for bem-sucdedida e falso caso contrário.</returns>
        private bool InnerAdd(ColType item)
        {
            if (item == null)
            {
                return true;
            }
            else
            {
                var currentNode = this.root;
                var colEnumerator = item.GetEnumerator();
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
                    var nextNode = new TrieNode(
                        this.factory);
                    currentNode.ChildNodes.Add(colEnumerator.Current, nextNode);
                    currentNode = nextNode;
                    state = colEnumerator.MoveNext();
                }

                if (currentNode.NodeNumber == -1)
                {
                    currentNode.NodeNumber = this.elements.Count;
                    this.elements.Add(item);
                    return true;
                }
                else
                {
                    // O item já existe.
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém o nó associado à colecção.
        /// </summary>
        /// <param name="col">A colecção.</param>
        /// <returns>O nó.</returns>
        private TrieNode GetAssociatedNode(ColType col)
        {
            var currentNode = this.root;
            foreach (var item in col)
            {
                var nextNode = default(TrieNode);
                if (currentNode.ChildNodes.TryGetValue(item, out nextNode))
                {
                    currentNode = nextNode;
                }
                else
                {
                    return null;
                }
            }

            return currentNode;
        }

        /// <summary>
        /// Obtém o valor do nó associado à colecção.
        /// </summary>
        /// <param name="col">A colecção.</param>
        /// <returns>O valor nó.</returns>
        private int GetAssociatedNodeValue(ColType col)
        {
            var currentNode = this.root;
            foreach (var item in col)
            {
                var nextNode = default(TrieNode);
                if (currentNode.ChildNodes.TryGetValue(item, out nextNode))
                {
                    currentNode = nextNode;
                }
                else
                {
                    return -1;
                }
            }

            return currentNode.NodeNumber;
        }

        /// <summary>
        /// Remove os descendentes de um determinado nó, indicando as colecções removidas.
        /// </summary>
        /// <param name="node">O nó.</param>
        /// <param name="removed">Recebe os números das colecções removidas.</param>
        private void RemoveNodeDescendants(
            TrieNode node, 
            InsertionSortedCollection<int> removed)
        {
            var nodeStack = new Stack<Tuple<TrieNode, IEnumerator<KeyValuePair<LabelType, TrieNode>>>>();
            nodeStack.Push(Tuple.Create(node, node.ChildNodes.GetEnumerator()));
            while (nodeStack.Count > 0)
            {
                var top = nodeStack.Peek();
                if (top.Item2.MoveNext())
                {
                    var nextNode = top.Item2.Current.Value;
                    if (nextNode.NodeNumber != -1)
                    {
                        removed.Add(nextNode.NodeNumber);
                    }

                    if (nextNode.ChildNodes.Count > 0)
                    {
                        nodeStack.Push(Tuple.Create(nextNode, nextNode.ChildNodes.GetEnumerator()));
                    }
                }
                else
                {
                    top.Item1.ChildNodes.Clear();
                    nodeStack.Pop();
                }
            }
        }

        /// <summary>
        /// Permite adendar a sub-árvore com raiz num nó da fonte
        /// ao nó especificado no destino.
        /// </summary>
        /// <param name="target">O nó de destino da cópia.</param>
        /// <param name="source">O nó da fonte.</param>
        /// <param name="sourceElements">Os valores que se encontram registados na fonte.</param>
        private void AppendSubTree(
            TrieNode target, 
            TrieNode source, 
            List<ColType> sourceElements)
        {
            var sourceStack = new Stack<IEnumerator<KeyValuePair<LabelType, TrieNode>>>();
            var targetStack = new Stack<TrieNode>();
            sourceStack.Push(source.ChildNodes.GetEnumerator());
            targetStack.Push(target);
            while (sourceStack.Count > 0)
            {
                var sourceTop = sourceStack.Peek();
                if (sourceTop.MoveNext())
                {
                    var targetParentNode = targetStack.Peek();
                    var sourceChildNode = sourceTop.Current.Value;
                    if (sourceChildNode.NodeNumber == -1)
                    {
                        var targetChildNode = new TrieNode(this.factory);
                        targetParentNode.ChildNodes.Add(sourceTop.Current.Key, targetChildNode);

                        if (sourceChildNode.ChildNodes.Count > 0)
                        {
                            targetStack.Push(targetParentNode);
                            sourceStack.Push(sourceChildNode.ChildNodes.GetEnumerator());
                        }
                        else
                        {
                            sourceStack.Pop();
                        }
                    }
                    else
                    {
                        var targetChildNode = new TrieNode(this.factory) { NodeNumber = this.elements.Count };
                        this.elements.Add(sourceElements[sourceChildNode.NodeNumber]);

                        if (sourceChildNode.ChildNodes.Count > 0)
                        {
                            targetStack.Push(targetParentNode);
                            sourceStack.Push(sourceChildNode.ChildNodes.GetEnumerator());
                        }
                        else
                        {
                            sourceStack.Pop();
                        }
                    }
                }
                else
                {
                    sourceStack.Pop();
                    targetStack.Pop();
                }
            }
        }

        /// <summary>
        /// Representa um nó da árvore associativa.
        /// </summary>
        public class TrieNode
        {
            /// <summary>
            /// Mantém o número associado ao nó.
            /// </summary>
            private int nodeNumber;

            /// <summary>
            /// Mantém a lista dos descendentes.
            /// </summary>
            private IDictionary<LabelType, TrieNode> childNodes;

            /// <summary>
            /// Instancia um nova instância de objectos do tipo <see cref="TrieNode"/>.
            /// </summary>
            /// <param name="factory">A fábrica responsável pela criação dos dicionários.</param>
            public TrieNode(
                IFactory<IDictionary<LabelType, TrieNode>> factory)
            {
                this.nodeNumber = -1;
                this.childNodes = factory.Create();
            }

            /// <summary>
            /// Instancia um nova instância de objectos do tipo <see cref="TrieNode"/>.
            /// </summary>
            /// <param name="nodeNumber">O número do nó.</param>
            /// <param name="factory">A fábrica responsável pela criação dos dicionários.</param>
            public TrieNode(
                int nodeNumber,
                IFactory<IDictionary<LabelType, TrieNode>> factory)
                : this(factory)
            {
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
            public IDictionary<LabelType, TrieNode> ChildNodes
            {
                get
                {
                    return this.childNodes;
                }
            }
        }

        /// <summary>
        /// Implementa um iterador para a árvore associativa.
        /// </summary>
        protected class TrieIterator : ITrieIterator<LabelType, ColType>
        {
            /// <summary>
            /// O conjunto de colecções.
            /// </summary>
            private DictionaryTrie<LabelType, ColType> owner;

            /// <summary>
            /// Mantém a lista dos nós visitados.
            /// </summary>
            private Stack<TrieNode> visitedChilds;

            /// <summary>
            /// Mantém um valor que indica se o iterador é válido.
            /// </summary>
            private bool valid;

            /// <summary>
            /// Instancia um nova instância de objectos do tipo <see cref="TrieIterator"/>.
            /// </summary>
            /// <param name="owner"></param>
            public TrieIterator(DictionaryTrie<LabelType, ColType> owner)
            {
                this.valid = true;
                this.owner = owner;
                this.visitedChilds = new Stack<TrieNode>();
                this.visitedChilds.Push(owner.root);
            }

            /// <summary>
            /// Obtém o prefixo actual.
            /// </summary>
            public ColType Current
            {
                get
                {
                    this.ValidateIteratorStatus();
                    if (this.visitedChilds.Count == 1)
                    {
                        var topNode = this.visitedChilds.Peek();
                        var nodeNumber = topNode.NodeNumber;
                        if (nodeNumber == -1)
                        {
                            throw new UtilitiesException("Iterator is pointing to a non terminal node.");
                        }
                        else
                        {
                            return default(ColType);
                        }
                    }
                    else
                    {
                        var topNode = this.visitedChilds.Peek();
                        var nodeNumber = topNode.NodeNumber;
                        if (nodeNumber == -1)
                        {
                            throw new UtilitiesException("Iterator is pointing to a non terminal node.");
                        }
                        else
                        {
                            return this.owner.elements[topNode.NodeNumber];
                        }
                    }
                }
            }

            /// <summary>
            /// Obtém um valor que indica se existe colecção associada ao nó apontado pelo iterador.
            /// </summary>
            public bool Exists
            {
                get
                {
                    this.ValidateIteratorStatus();
                    var topNode = this.visitedChilds.Peek();
                    return topNode.NodeNumber != -1;
                }
            }

            /// <summary>
            /// Otbém o conjunto de etiquetas.
            /// </summary>
            public IEnumerable<LabelType> Labels
            {
                get
                {
                    this.ValidateIteratorStatus();
                    var topNode = this.visitedChilds.Peek();
                    return topNode.ChildNodes.Keys;
                }
            }

            /// <summary>
            /// Verifica se é possível avançar para a etiqueta proporcionada.
            /// </summary>
            /// <param name="label">A etiqueta.</param>
            /// <returns>Verdadeiro caso seja possível avançar e falso caso contrário.</returns>
            public bool CanAdvance(LabelType label)
            {
                this.ValidateIteratorStatus();
                var topNode = this.visitedChilds.Peek();
                return topNode.ChildNodes.ContainsKey(label);
            }

            /// <summary>
            /// Tenta obter o objecto actual.
            /// </summary>
            /// <param name="col">O parâmetro que recebe o objecto caso este exitsta.</param>
            /// <returns>Verdadeiro se o objecto existir e falso caso contrário.</returns>
            public bool TryGetCurrent(out ColType col)
            {
                this.ValidateIteratorStatus();
                col = default(ColType);
                if (this.visitedChilds.Count == 1)
                {
                    return true;
                }
                else
                {
                    var topNode = this.visitedChilds.Peek();
                    var nodeNumber = topNode.NodeNumber;
                    if (nodeNumber == -1)
                    {
                        return false;
                    }
                    else
                    {
                        col = this.owner.elements[topNode.NodeNumber];
                        return true;
                    }
                }
            }

            /// <summary>
            /// Avança na árvore para o próximo elemento especificado pela etiqueta.
            /// </summary>
            /// <param name="label">A etiqueta.</param>
            /// <returns>Verdadeiro se for possível avançar e falso caso contrário.</returns>
            public bool GoForward(LabelType label)
            {
                this.ValidateIteratorStatus();
                var currentNode = this.visitedChilds.Peek();
                var nextNode = default(TrieNode);
                if (currentNode.ChildNodes.TryGetValue(
                    label,
                    out nextNode))
                {
                    this.visitedChilds.Push(nextNode);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Retrocede uma posição.
            /// </summary>
            /// <returns>Verdadeiro caso seja possível retroceder e falso caso contrário.</returns>
            public bool GoBack()
            {
                this.ValidateIteratorStatus();
                if (this.visitedChilds.Count > 1)
                {
                    this.visitedChilds.Pop();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Coloca o iterador na raíz da árvore.
            /// </summary>
            public void Reset()
            {
                this.ValidateIteratorStatus();
                this.visitedChilds.Clear();
                this.visitedChilds.Push(owner.root);
            }

            /// <summary>
            /// Descarta o iterador.
            /// </summary>
            public void Dispose()
            {
                if (this.owner != null)
                {
                    var currentOwner = this.owner;
                    this.owner = null;
                    currentOwner.activeIterators.Remove(this);
                }
            }

            /// <summary>
            /// Invalida o iterador actual se este se encontrar no caminho de algum
            /// nó que tenha sido removido.
            /// </summary>
            /// <param name="firstRemovedNode">O primeiro nó a ser removido.</param>
            public void Invalidate(TrieNode firstRemovedNode)
            {
                this.valid = this.visitedChilds.Contains(firstRemovedNode);
            }

            /// <summary>
            /// Permite validar o estado do iterador.
            /// </summary>
            /// <exception cref="UtilitiesException">
            /// Se o iterador tiver sido dispensado ou invalidado.
            /// </exception>
            private void ValidateIteratorStatus()
            {
                if (this.owner == null)
                {
                    throw new UtilitiesException("The iterator was disposed.");
                }
                else if (!this.valid)
                {
                    throw new UtilitiesException("Some change in the trie has invalidated the iterator.");
                }
            }
        }
    }
}
