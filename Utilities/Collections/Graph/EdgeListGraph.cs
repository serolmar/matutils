namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Define as funções essenciais a um grafo definido por uma lista de arestas
    /// que não são identificadas por qualquer etiqueta.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos que constituem os vértices.</typeparam>
    /// <typeparam name="EdgeType">O tipo de objectos que constituem as arestas.</typeparam>
    public abstract class AEdgeListGraph<VertexType, EdgeType>
        : IEdgeGraph<VertexType, EdgeType>
        where EdgeType : IEdge<VertexType>
    {
        /// <summary>
        /// Indica se o grafo é direccionado.
        /// </summary>
        protected bool directed = false;

        /// <summary>
        /// O comparador de vértices.
        /// </summary>
        protected IEqualityComparer<VertexType> vertexEqualityComparer;

        /// <summary>
        /// Mapeia cada vértice às arestas adjacentes.
        /// </summary>
        protected Dictionary<VertexType, List<EdgeType>> vertices;

        /// <summary>
        /// O contentor das arestas.
        /// </summary>
        protected List<EdgeType> edges;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="AEdgeListGraph{VertexType, EdgeType}"/>.
        /// </summary>
        /// <param name="directed">Verdadeiro caso o grafo seja directo e falso caso contrário.</param>
        public AEdgeListGraph(bool directed = false)
        {
            this.edges = new List<EdgeType>();
            this.vertices = new Dictionary<VertexType, List<EdgeType>>();
            this.vertexEqualityComparer = this.vertices.Comparer;
            this.directed = directed;
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="AEdgeListGraph{VertexType, EdgeType}"/>.
        /// </summary>
        /// <param name="vertexEqualityComparer">O comparador de vértices.</param>
        /// <param name="directed">Verdadeiro caso o grafo seja dirigido e falso caso contrário.</param>
        public AEdgeListGraph(
            IEqualityComparer<VertexType> vertexEqualityComparer,
            bool directed = false)
        {
            if (this.vertexEqualityComparer == null)
            {
                this.edges = new List<EdgeType>();
                this.vertices = new Dictionary<VertexType, List<EdgeType>>(vertexEqualityComparer);
                this.vertexEqualityComparer = this.vertices.Comparer;
                this.directed = directed;
            }
            else
            {
                this.edges = new List<EdgeType>();
                this.vertices = new Dictionary<VertexType, List<EdgeType>>(vertexEqualityComparer);
                this.vertexEqualityComparer = vertexEqualityComparer;
                this.directed = directed;
            }
        }

        /// <summary>
        /// Obtém um enumerador para o conjunto de vértices.
        /// </summary>
        /// <value>
        /// O enumerador.
        /// </value>
        public IEnumerable<VertexType> Vertices
        {
            get
            {
                return this.vertices.Keys;
            }
        }

        /// <summary>
        /// Obtém um enumerador para o conjunto de arestas.
        /// </summary>
        /// <value>
        /// O enumerador.
        /// </value>
        public IEnumerable<EdgeType> Edges
        {
            get
            {
                return this.edges;
            }
        }

        /// <summary>
        /// Obtém um valor indicando se o grafo é dirigido.
        /// </summary>
        /// <value>
        /// Verdadeiro se o grafo for dirigido e falso caso contrário.
        /// </value>
        public bool Directed
        {
            get
            {
                return this.directed;
            }
        }

        /// <summary>
        /// Retorna os vizinhos de um determinado vértice.
        /// </summary>
        /// <remarks>
        /// Se o grafo for dirigido, os vizinhos de um vértice são todos os vértices que são
        /// atingíveis por uma aresta com início nesse vértice.
        /// </remarks>
        /// <param name="vertex">O vértice.</param>
        /// <returns>O conjunto dos vértices vizinhos.</returns>
        public IEnumerable<VertexType> GetNeighbours(
            VertexType vertex)
        {
            var result = default(List<EdgeType>);
            if (!this.vertices.TryGetValue(vertex, out result))
            {
                result = new List<EdgeType>();
            }

            foreach (var edge in result)
            {
                if (this.vertexEqualityComparer != null)
                {
                    if (this.vertexEqualityComparer.Equals(vertex, edge.InitialVertex))
                    {
                        yield return edge.FinalVertex;
                    }
                    else if (!this.directed)
                    {
                        yield return edge.InitialVertex;
                    }
                }
                else
                {
                    if (vertex.Equals(edge.InitialVertex))
                    {
                        yield return edge.FinalVertex;
                    }
                    else if (!this.directed)
                    {
                        yield return edge.InitialVertex;
                    }
                }
            }
        }

        /// <summary>
        /// Adiciona um vértice ao grafo.
        /// </summary>
        /// <param name="vertex">O vértice.</param>
        /// <exception cref="ArgumentNullException">Se o vértice for nulo.</exception>
        /// <exception cref="UtilitiesException">Se o vértice já existe.</exception>
        public void AddVertex(VertexType vertex)
        {
            if (vertex == null)
            {
                throw new ArgumentNullException("vertex");
            }
            else if (this.vertices.ContainsKey(vertex))
            {
                throw new UtilitiesException("Vertex was already inserted.");
            }
            else
            {
                this.vertices.Add(vertex, new List<EdgeType>());
            }
        }

        /// <summary>
        /// Adiciona uma aresta ao grafo cujo valor é dado por defeito.
        /// </summary>
        /// <param name="initialVertex">O vértice inicial.</param>
        /// <param name="finalVertex">O vértice final.</param>
        /// <exception cref="ArgumentNullException">Se algum dos vértices for nulo.</exception>
        public abstract void AddEdge(VertexType initialVertex, VertexType finalVertex);

        /// <summary>
        /// Adiciona uma aresta ao grafo.
        /// </summary>
        /// <param name="edge">A aresta.</param>
        public void AddEdge(EdgeType edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException("edge");
            }
            else
            {
                this.InnerAddEdge(edge);
            }
        }

        /// <summary>
        /// Remove um vértice do grafo.
        /// </summary>
        /// <param name="vertex">O vértice a ser removido.</param>
        public void RemoveVertex(VertexType vertex)
        {
            var vertexEdge = default(List<EdgeType>);
            if (this.vertices.TryGetValue(vertex, out vertexEdge))
            {
                while (vertexEdge.Count > 0)
                {
                    this.edges.Remove(vertexEdge[0]);
                    vertexEdge.RemoveAt(0);
                }

                this.vertices.Remove(vertex);
            }
        }

        /// <summary>
        /// Remove o conjunto de arestas com origem num vértice e extremidade no outro.
        /// </summary>
        /// <param name="initialVertex">O vértice inicial.</param>
        /// <param name="finalVertex">O vértice final.</param>
        public void RemoveEdges(VertexType initialVertex, VertexType finalVertex)
        {
            var initialVertexEdges = default(List<EdgeType>);
            if (this.vertices.TryGetValue(initialVertex, out initialVertexEdges))
            {
                var finalVertexEdges = default(List<EdgeType>);
                if (this.vertices.TryGetValue(finalVertex, out finalVertexEdges))
                {
                    var count = 0;
                    while (count < initialVertexEdges.Count)
                    {
                        var currentEdge = initialVertexEdges[count];
                        if (this.vertexEqualityComparer == null)
                        {
                            if (currentEdge.InitialVertex.Equals(initialVertex) && currentEdge.FinalVertex.Equals(finalVertex))
                            {
                                initialVertexEdges.RemoveAt(count);
                            }
                            else
                            {
                                ++count;
                            }
                        }
                        else
                        {
                            if (this.vertexEqualityComparer.Equals(currentEdge.InitialVertex, initialVertex) &&
                                this.vertexEqualityComparer.Equals(currentEdge.FinalVertex, finalVertex))
                            {
                                initialVertexEdges.RemoveAt(count);
                            }
                            else
                            {
                                ++count;
                            }
                        }

                        finalVertexEdges.Remove(currentEdge);
                        this.edges.Remove(currentEdge);
                    }
                }
            }
        }

        /// <summary>
        /// Efectua uma pesquisa em extensão sobre o grafo.
        /// </summary>
        /// <param name="vertices">Os vértices de partida.</param>
        /// <param name="initVerticesFunc">
        /// A função a ser executada durante a visita aos vértices de partida.
        /// </param>
        /// <param name="connectedVerticesFunc">
        /// A função a ser executada em cada visita aos vértices conectados.</param>
        /// <remarks>
        /// A função que é executada aquando da visita a um vértice conectado
        /// recebe, como argumentos, o vértice que está a ser visitado, o vértice anterior,
        /// um valor que indica se o vértice corrente já foi visitado e a aresta seguida.
        /// </remarks>
        public void BreathFirstSearch(
            IEnumerable<VertexType> vertices,
            Action<VertexType> initVerticesFunc,
            Action<VertexType, VertexType, bool, EdgeType> connectedVerticesFunc)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException("vertices");
            }
            else if (initVerticesFunc == null)
            {
                throw new ArgumentNullException("initVerticesFunc");
            }
            else if (connectedVerticesFunc == null)
            {
                throw new ArgumentNullException("connectedVerticesFunc");
            }
            else
            {
                this.CheckVertices(vertices);
                var queue = new Queue<Tuple<VertexType, VertexType, EdgeType>>();
                var visited = new Dictionary<VertexType, bool>();
                var processedEdges = new Dictionary<EdgeType, bool>();
                var verticesEnumerator = vertices.GetEnumerator();

                while (verticesEnumerator.MoveNext())
                {
                    var curr = verticesEnumerator.Current;
                    if (!visited.ContainsKey(curr))
                    {
                        initVerticesFunc.Invoke(curr);
                        visited.Add(curr, true);
                        var innerEdges = this.vertices[curr];
                        var edgesLength = innerEdges.Count;
                        for (var i = 0; i < edgesLength; ++i)
                        {
                            var currEdge = innerEdges[i];
                            if (!processedEdges.ContainsKey(currEdge))
                            {
                                var reverse = true;
                                if (currEdge.InitialVertex.Equals(curr))
                                {
                                    reverse = false;
                                }

                                if (reverse)
                                {
                                    if (!this.directed)
                                    {
                                        var finalVertex = currEdge.InitialVertex;
                                        if (visited.ContainsKey(finalVertex))
                                        {
                                            connectedVerticesFunc.Invoke(
                                                currEdge.InitialVertex,
                                                currEdge.FinalVertex,
                                                true,
                                                currEdge);
                                        }
                                        else
                                        {
                                            queue.Enqueue(Tuple.Create(
                                                currEdge.InitialVertex,
                                                currEdge.FinalVertex,
                                                currEdge));
                                        }

                                        processedEdges.Add(currEdge, true);
                                    }
                                }
                                else
                                {
                                    var finalVertex = currEdge.FinalVertex;
                                    if (visited.ContainsKey(finalVertex))
                                    {
                                        connectedVerticesFunc.Invoke(
                                            currEdge.FinalVertex,
                                            currEdge.InitialVertex,
                                            true,
                                            currEdge);
                                    }
                                    else
                                    {
                                        queue.Enqueue(Tuple.Create(
                                            currEdge.FinalVertex,
                                            currEdge.InitialVertex,
                                            currEdge));
                                        visited.Add(finalVertex, true);
                                    }

                                    processedEdges.Add(currEdge, true);
                                }
                            }
                        }
                    }
                }

                while (queue.Count > 0)
                {
                    var pair = queue.Dequeue();
                    var currVertex = pair.Item1;
                    var prevVertex = pair.Item2;
                    var edge = pair.Item3;
                    connectedVerticesFunc.Invoke(
                        currVertex,
                        prevVertex,
                        false,
                        edge);

                    var innerEdges = this.vertices[currVertex];
                    var edgesLength = innerEdges.Count;
                    for (var i = 0; i < edgesLength; ++i)
                    {
                        var currEdge = innerEdges[i];
                        if (!processedEdges.ContainsKey(currEdge))
                        {
                            var reverse = true;
                            if (currEdge.InitialVertex.Equals(currVertex))
                            {
                                reverse = false;
                            }

                            if (reverse)
                            {
                                if (!this.directed)
                                {
                                    var finalVertex = currEdge.InitialVertex;
                                    if (visited.ContainsKey(finalVertex))
                                    {
                                        connectedVerticesFunc.Invoke(
                                            currEdge.InitialVertex,
                                            currEdge.FinalVertex,
                                            true,
                                            currEdge);
                                    }
                                    else
                                    {
                                        queue.Enqueue(Tuple.Create(
                                            currEdge.InitialVertex,
                                            currEdge.FinalVertex,
                                            currEdge));
                                        visited.Add(finalVertex, true);
                                    }

                                    processedEdges.Add(currEdge, true);
                                }
                            }
                            else
                            {
                                var finalVertex = currEdge.FinalVertex;
                                if (visited.ContainsKey(finalVertex))
                                {
                                    connectedVerticesFunc.Invoke(
                                        currEdge.FinalVertex,
                                        currEdge.InitialVertex,
                                        true,
                                        currEdge);
                                }
                                else
                                {
                                    queue.Enqueue(Tuple.Create(
                                        currEdge.FinalVertex,
                                        currEdge.InitialVertex,
                                        currEdge));
                                    visited.Add(finalVertex, true);
                                }

                                processedEdges.Add(currEdge, true);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Efectua uma pesquisa em profunidade sobre o grafo.
        /// </summary>
        /// <param name="vertices">Os vértices de partida.</param>
        /// <param name="initialVerticesFunc">
        /// A função a ser executada nos vértices de entrada.
        /// </param>
        /// <param name="connectedVerticesFunc">
        /// A função que é executada aquando da visita a um vértice conectado
        /// recebe, como argumentos, o vértice que está a ser visitado, o vértice anterior,
        /// um valor que indica se o vértice corrente já foi visitado e a aresta seguida.
        /// </param>
        public void DepthFirstSearch(
            IEnumerable<VertexType> vertices,
            Action<VertexType> initialVerticesFunc,
            Action<VertexType, VertexType, bool, EdgeType> connectedVerticesFunc)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException("vertices");
            }
            else if (initialVerticesFunc == null)
            {
                throw new ArgumentNullException("initialVerticesFunc");
            }
            else if (connectedVerticesFunc == null)
            {
                throw new ArgumentNullException("connectedVerticesFunc");
            }
            else
            {
                this.CheckVertices(vertices);
                var stack = new Stack<MutableTuple<VertexType, VertexType, EdgeType, List<EdgeType>, int>>();
                Dictionary<VertexType, bool> visited = new Dictionary<VertexType, bool>();
                Dictionary<EdgeType, bool> processedEdges = new Dictionary<EdgeType, bool>();
                var verticesEnumerator = vertices.GetEnumerator();

                while (verticesEnumerator.MoveNext())
                {
                    var currVertex = verticesEnumerator.Current;
                    if (!visited.ContainsKey(currVertex))
                    {
                        visited.Add(currVertex, true);
                        var innerEdges = this.vertices[currVertex];
                        var edgesLength = innerEdges.Count;
                        var isTerminal = true;
                        for (var i = 0; i < edgesLength; ++i)
                        {
                            var currEdge = innerEdges[i];
                            if (!processedEdges.ContainsKey(currEdge))
                            {
                                var reverse = true;
                                if (currEdge.InitialVertex.Equals(currVertex))
                                {
                                    reverse = false;
                                }

                                if (reverse)
                                {
                                    if (!directed)
                                    {
                                        var otherVertex = currEdge.InitialVertex;
                                        processedEdges.Add(currEdge, true);
                                        if (visited.ContainsKey(otherVertex))
                                        {
                                            connectedVerticesFunc.Invoke(
                                                otherVertex,
                                                currVertex,
                                                true,
                                                currEdge);
                                        }
                                        else
                                        {
                                            visited.Add(otherVertex, true);
                                            stack.Push(MutableTuple.Create(
                                                otherVertex,
                                                currEdge.FinalVertex,
                                                currEdge,
                                                this.vertices[otherVertex],
                                                0));
                                            this.ProcessDepthSearchVertex(
                                                stack,
                                                visited,
                                                processedEdges,
                                                connectedVerticesFunc);
                                        }
                                    }
                                }
                                else
                                {
                                    var otherVertex = currEdge.FinalVertex;
                                    processedEdges.Add(currEdge, true);
                                    if (visited.ContainsKey(otherVertex))
                                    {
                                        connectedVerticesFunc.Invoke(
                                            otherVertex,
                                            currVertex,
                                            true,
                                            currEdge);
                                    }
                                    else
                                    {
                                        visited.Add(otherVertex, true);
                                        stack.Push(MutableTuple.Create(
                                            otherVertex,
                                            currEdge.InitialVertex,
                                            currEdge,
                                            this.vertices[otherVertex],
                                            0));
                                        this.ProcessDepthSearchVertex(
                                                stack,
                                                visited,
                                                processedEdges,
                                                connectedVerticesFunc);
                                    }
                                }
                            }
                        }

                        if (isTerminal)
                        {
                            initialVerticesFunc.Invoke(
                                currVertex);
                        }
                    } // !visited
                } // while
            }
        }

        /// <summary>
        /// Adiciona uma aresta ao grafo actual.
        /// </summary>
        /// <param name="edge">A aresta a ser adicionada.</param>
        protected void InnerAddEdge(EdgeType edge)
        {
            this.edges.Add(edge);
            var edgesWithVertex = default(List<EdgeType>);
            if (this.vertices.TryGetValue(edge.InitialVertex, out edgesWithVertex))
            {
                edgesWithVertex.Add(edge);
            }
            else
            {
                this.vertices.Add(edge.InitialVertex, new List<EdgeType>() { edge });
            }

            edgesWithVertex = null;
            if (this.vertices.TryGetValue(edge.FinalVertex, out edgesWithVertex))
            {
                edgesWithVertex.Add(edge);
            }
            else
            {
                this.vertices.Add(edge.FinalVertex, new List<EdgeType>() { edge });
            }
        }

        /// <summary>
        /// Cria a fila que será usada para o processamento da pesquisa
        /// em extensão.
        /// </summary>
        /// <param name="vertices">Os vértices.</param>
        private void CheckVertices(
            IEnumerable<VertexType> vertices)
        {
            var enumerator = vertices.GetEnumerator();
            if (enumerator.MoveNext())
            {
                var curr = enumerator.Current;
                if (!this.vertices.ContainsKey(curr))
                {
                    throw new ArgumentException("Provided list has vertices not contained in graph.");
                }

                while (enumerator.MoveNext())
                {
                    curr = enumerator.Current;
                    if (!this.vertices.ContainsKey(curr))
                    {
                        throw new ArgumentException("Provided list has vertices not contained in graph.");
                    }
                }
            }
            else
            {
                throw new ArgumentException("Provided list is empty.");
            }
        }

        /// <summary>
        /// Processa os vértices restantes na pesquisa em profundidade.
        /// </summary>
        /// <param name="stack">A pilha auxiliar.</param>
        /// <param name="visited">Os vértices visitados.</param>
        /// <param name="processedEdges">As arestas processadas.</param>
        /// <param name="connectedVerticesFunc">
        /// A função que é executada aquando da visita a um vértice.
        /// </param>
        private void ProcessDepthSearchVertex(
            Stack<MutableTuple<VertexType, VertexType, EdgeType, List<EdgeType>, int>> stack,
            Dictionary<VertexType, bool> visited,
            Dictionary<EdgeType, bool> processedEdges,
            Action<VertexType, VertexType, bool, EdgeType> connectedVerticesFunc)
        {
            while (stack.Count > 0)
            {
                var pair = stack.Peek();
                var currVertex = pair.Item1;
                var currEdges = pair.Item4;
                var pointer = pair.Item5;

                if (pointer < currEdges.Count)
                {
                    var currEdge = currEdges[pointer];
                    if (!processedEdges.ContainsKey(currEdge))
                    {
                        var reverse = true;
                        if (currEdge.InitialVertex.Equals(currVertex))
                        {
                            reverse = false;
                        }

                        if (reverse)
                        {
                            if (!directed)
                            {
                                var otherVertex = currEdge.InitialVertex;
                                processedEdges.Add(currEdge, true);
                                if (visited.ContainsKey(otherVertex))
                                {
                                    connectedVerticesFunc.Invoke(
                                        otherVertex,
                                        currEdge.FinalVertex,
                                        true,
                                        currEdge);
                                }
                                else
                                {
                                    visited.Add(otherVertex,true);
                                    stack.Push(MutableTuple.Create(
                                    otherVertex,
                                    currVertex,
                                    currEdge,
                                    this.vertices[otherVertex],
                                    0));
                                }
                            }
                        }
                        else
                        {
                            var otherVertex = currEdge.FinalVertex;
                            processedEdges.Add(currEdge, true);
                            if (visited.ContainsKey(otherVertex))
                            {
                                connectedVerticesFunc.Invoke(
                                        otherVertex,
                                        currEdge.InitialVertex,
                                        true,
                                        currEdge);
                            }
                            else
                            {
                                visited.Add(otherVertex, true);
                                stack.Push(MutableTuple.Create(
                                    otherVertex,
                                    currVertex,
                                    currEdge,
                                    this.vertices[otherVertex],
                                    0));
                            }
                        }
                    }

                    pair.Item5 = pointer + 1;
                }
                else
                {
                    stack.Pop();
                    connectedVerticesFunc.Invoke(
                        currVertex,
                        pair.Item2,
                        false,
                        pair.Item3);
                }
            }
        }
    }

    /// <summary>
    /// Representa um grafo cujas arestas não são etiquetadas.
    /// </summary>
    /// <typeparam name="VertexType">O tipo dos objectos que constituem os vértices.</typeparam>
    public class EdgeListGraph<VertexType>
        : AEdgeListGraph<VertexType, IEdge<VertexType>>
    {
        /// <summary>
        /// Função que define a geração das arestas.
        /// </summary>
        protected Func<VertexType, VertexType, IEdge<VertexType>> edgeFactory =
            (initialVertex, finalVertex) => new Edge<VertexType>(initialVertex, finalVertex);

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="EdgeListGraph{VertexType}"/>.
        /// </summary>
        /// <param name="directed">Verdadeiro caso o grafo seja directo e falso caso contrário.</param>
        public EdgeListGraph(bool directed = false) : base(directed) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="EdgeListGraph{VertexType}"/>.
        /// </summary>
        /// <param name="vertexEqualityComparer">O comparador de vértices.</param>
        /// <param name="directed">Verdadeiro caso o grafo seja directo e falso caso contrário.</param>
        public EdgeListGraph(
            IEqualityComparer<VertexType> vertexEqualityComparer,
            bool directed = false)
            : base(vertexEqualityComparer, directed) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="EdgeListGraph{VertexType}"/>.
        /// </summary>
        /// <param name="edgeFactory">A fábrica responsável pela criação das arestas.</param>
        /// <param name="directed">Verdadeiro caso o grafo seja directo e falso caso contrário.</param>
        public EdgeListGraph(
            Func<VertexType, VertexType,
            IEdge<VertexType>> edgeFactory,
            bool directed = false)
            : base(directed)
        {
            if (edgeFactory == null)
            {
                throw new ArgumentNullException("edgeFactory");
            }
            else
            {
                this.edgeFactory = edgeFactory;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="EdgeListGraph{VertexType}"/>.
        /// </summary>
        /// <param name="edgeFactory">A fábrica responsável pela criação das arestas.</param>
        /// <param name="vertexEqualityComparer">O comparador de vértices.</param>
        /// <param name="directed">Verdadeiro caso o grafo seja directo e falso caso contrário.</param>
        public EdgeListGraph(
            Func<VertexType, VertexType, IEdge<VertexType>> edgeFactory,
            IEqualityComparer<VertexType> vertexEqualityComparer,
            bool directed = false)
            : base(vertexEqualityComparer, directed)
        {
            if (edgeFactory == null)
            {
                throw new ArgumentNullException("edgeFactory");
            }
            else
            {
                this.edgeFactory = edgeFactory;
            }
        }

        /// <summary>
        /// Obtém ou atribui a fábrica responsável pela criação das arestas.
        /// </summary>
        public Func<VertexType, VertexType, IEdge<VertexType>> EdgeFactory
        {
            get
            {
                return this.edgeFactory;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Edge factory can't be a null value.");
                }
                else
                {
                    this.edgeFactory = value;
                }
            }
        }

        /// <summary>
        /// Adiciona uma aresta ao grafo cujo valor é dado por defeito.
        /// </summary>
        /// <param name="initialVertex">O vértice inicial.</param>
        /// <param name="finalVertex">O vértice final.</param>
        /// <exception cref="ArgumentNullException">Se algum dos vértices for nulo.</exception>
        public override void AddEdge(
            VertexType initialVertex,
            VertexType finalVertex)
        {
            if (initialVertex == null)
            {
                throw new ArgumentNullException("initialVertex");
            }
            else if (finalVertex == null)
            {
                throw new ArgumentNullException("finalVertex");
            }
            else
            {
                var edge = this.edgeFactory.Invoke(initialVertex, finalVertex);
                this.InnerAddEdge(
                    edge);
            }
        }
    }

    /// <summary>
    /// Grafo representado por uma lista de arestas com etiqueta.
    /// </summary>
    /// <typeparam name="VertexType">O tipo que representa o vértice.</typeparam>
    /// <typeparam name="EdgeValueType">O tipo do objecto associado à aresta.</typeparam>
    public class LabeledEdgeListGraph<VertexType, EdgeValueType>
        : AEdgeListGraph<VertexType, ILabeledEdge<VertexType, EdgeValueType>>,
        ILabeledEdgeGraph<VertexType, EdgeValueType>
    {
        /// <summary>
        /// A fábrica responsável pela criação de arestas.
        /// </summary>
        protected Func<VertexType, VertexType, EdgeValueType, ILabeledEdge<VertexType, EdgeValueType>>
            edgeFactory = (initialVertex, finalVertex, edgeValue) => new LabeledEdge<VertexType, EdgeValueType>(
                initialVertex,
                finalVertex,
                edgeValue);

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LabeledEdgeListGraph{VertexType, EdgeValueType}"/>.
        /// </summary>
        /// <param name="edgeFactory">O objecto responsável pela criação de arestas.</param>
        /// <param name="directed">Verdadeiro caso o grafo seja directo e falso caso contrário.</param>
        public LabeledEdgeListGraph(
            Func<VertexType, VertexType, EdgeValueType, ILabeledEdge<VertexType, EdgeValueType>> edgeFactory,
            bool directed = false)
            : base(directed)
        {
            if (edgeFactory == null)
            {
                throw new ArgumentNullException("edgeFactory");
            }
            else
            {
                this.edgeFactory = edgeFactory;
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LabeledEdgeListGraph{VertexType, EdgeValueType}"/>.
        /// </summary>
        /// <param name="edgeFactory">O objecto responsável pela criação de arestas.</param>.
        /// <param name="vertexEqualityComparer">O comparador de vértices.</param>
        /// <param name="directed">Verdadeiro caso o grafo seja dirigido e falso caso contrário.</param>
        public LabeledEdgeListGraph(
            Func<VertexType, VertexType, EdgeValueType, ILabeledEdge<VertexType, EdgeValueType>> edgeFactory,
            IEqualityComparer<VertexType> vertexEqualityComparer,
            bool directed = false)
            : base(vertexEqualityComparer, directed)
        {
            if (edgeFactory == null)
            {
                throw new ArgumentNullException("edgeFactory");
            }
            else
            {
                this.edgeFactory = edgeFactory;
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LabeledEdgeListGraph{VertexType, EdgeValueType}"/>.
        /// </summary>
        /// <param name="directed">Verdadeiro caso o grafo seja directo e falso caso contrário.</param>
        public LabeledEdgeListGraph(
            bool directed = false)
            : base(directed)
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LabeledEdgeListGraph{VertexType, EdgeValueType}"/>.
        /// </summary>
        /// <param name="vertexEqualityComparer">O comparador de vértices.</param>
        /// <param name="directed">Verdadeiro caso o grafo seja dirigido e falso caso contrário.</param>
        public LabeledEdgeListGraph(
            IEqualityComparer<VertexType> vertexEqualityComparer,
            bool directed = false)
            : base(vertexEqualityComparer, directed)
        {
        }

        /// <summary>
        /// Adiciona uma aresta ao grafo cujo valor é dado por defeito.
        /// </summary>
        /// <param name="initialVertex">O vértice inicial.</param>
        /// <param name="finalVertex">O vértice final.</param>
        /// <exception cref="ArgumentNullException">Se algum dos vértices for nulo.</exception>
        public override void AddEdge(
            VertexType initialVertex,
            VertexType finalVertex)
        {
            if (initialVertex == null)
            {
                throw new ArgumentNullException("initialVertex");
            }
            else if (finalVertex == null)
            {
                throw new ArgumentNullException("finalVertex");
            }
            else
            {
                var edge = this.edgeFactory.Invoke(
                    initialVertex,
                    finalVertex,
                    default(EdgeValueType));
                this.InnerAddEdge(
                    edge);
            }
        }

        /// <summary>
        /// Adiciona uma aresta ao grafo.
        /// </summary>
        /// <param name="initialVertex">O vértice inicial.</param>
        /// <param name="finalVertex">O vértice final.</param>
        /// <param name="value">O valor associado à aresta.</param>
        /// <exception cref="ArgumentNullException">Se algum dos vértices for nulo.</exception>
        public void AddEdge(VertexType initialVertex, VertexType finalVertex, EdgeValueType value)
        {
            if (initialVertex == null)
            {
                throw new ArgumentNullException("initialVertex");
            }
            else if (finalVertex == null)
            {
                throw new ArgumentNullException("finalVertex");
            }
            else
            {
                var edge = this.edgeFactory.Invoke(
                    initialVertex,
                    finalVertex,
                    value);
                this.InnerAddEdge(
                    edge);
            }
        }
    }
}
