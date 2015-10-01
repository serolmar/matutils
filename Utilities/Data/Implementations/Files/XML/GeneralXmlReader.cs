namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Define o tipo de leitores de elementos do XML.
    /// </summary>
    public enum ReaderType
    {
        /// <summary>
        /// Trata-se da leitura de um elemento.
        /// </summary>
        ELEMENT,

        /// <summary>
        /// Trata-se da leitura de um atributo.
        /// </summary>
        ATTRIBUTE,

        /// <summary>
        /// Trata-se da leitura de texto.
        /// </summary>
        TEXT
    }

    /// <summary>
    /// Define o tipo para o leitor geral.
    /// </summary>
    public interface IXmlReader
    {
        /// <summary>
        /// Obtém o nome do elemento.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Obtém o tipo de leitor.
        /// </summary>
        ReaderType ReaderType { get; }

        /// <summary>
        /// Realiza a leitura da informação contida no elemento.
        /// </summary>
        /// <param name="reader">O leitor de XML.</param>
        /// <param name="updatable">O objecto que irá conter a informação lida.</param>
        void Read(XmlReader reader, object updatable);
    }

    /// <summary>
    /// Define o leitor de elementos.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que constituem os contentores da informação.</typeparam>
    /// <typeparam name="P">O tipo dos objectos que são lidos e adicionados aos contentores.</typeparam>
    public interface IXmlElementReader<T, P> : IXmlReader
    {
        /// <summary>
        /// Obtém a lista de leitores de atributos associada ao leitor de elementos.
        /// </summary>
        IXmlAttributeReader<T> AttributesReader { get; }

        /// <summary>
        /// Regista um leitor de elementos que permitirá realizar a leitura sobre
        /// elementos descendentes.
        /// </summary>
        /// <param name="textAction">A função de atribuição do texto.</param>
        /// <param name="attachFunction">A função de atribuição do objecto associado ao elemento.</param>
        /// <param name="elementName">O nome do elemento.</param>
        /// <param name="ignoreNotRegisteredElements">
        /// Valor que indica se os elementos não registados serão ignorados.
        /// </param>
        /// <param name="ignoreOrder">Valor que indica se a ordem será ignorada.</param>
        /// <param name="ignoreNotRegisteredAttributes">
        /// Valor que indica se os atributos não registados são para ignorar.
        /// </param>
        /// <param name="minimum">O mínimo de elementos do tipo que se espera encontrar.</param>
        /// <param name="maximum">O máximo de elementos do tipo que se espera encontrar.</param>
        /// <returns>O leitor.</returns>
        IXmlElementReader<T, P> RegisterElementReader(
            Action<T, string> textAction,
            Func<T, P> attachFunction,
            string elementName,
            bool ignoreNotRegisteredElements,
            bool ignoreOrder,
            bool ignoreNotRegisteredAttributes,
            int minimum,
            int maximum);
    }

    /// <summary>
    /// Define o leitor de atributos.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem os contentores da informação.</typeparam>
    public interface IXmlAttributeReader<T> : IXmlReader
    {
        /// <summary>
        /// Regista um leitor de atributos.
        /// </summary>
        /// <param name="name">O nome do atributo.</param>
        /// <param name="action">A função que permite associar a informação lida ao objecto.</param>
        /// <param name="mandatory">Valor que indica se o atributo é obrigatório.</param>
        void RegisterAttribute(
            string name,
            Action<T, string> action,
            bool mandatory);
    }

    /// <summary>
    /// Implementa um leitor de atributos.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem os contentores da informação.</typeparam>
    internal class AttributesReader<T> : IXmlAttributeReader<T>
    {
        /// <summary>
        /// Dicionário que,a o nome do atributo, associa a acção de actualização
        /// e um valor que indica se o atributo é obrigatório.
        /// </summary>
        private Dictionary<string, Tuple<Action<T, string>, bool>> attributes;

        /// <summary>
        /// Valor que indica se os atributos não registados poderão ser ignorados.
        /// </summary>
        private bool ignoreNotRegistered;

        /// <summary>
        /// O leitor de elementos ao qual o leitor de atributos pertence.
        /// </summary>
        private IXmlReader elementReader;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="AttributesReader{T}"/>
        /// </summary>
        /// <param name="elementReader">O leitor de elementos ao qual o leitor de atributos pertence.</param>
        /// <param name="ignoreNotRegistered">Valor que indica se os atributos não registados podem ser ignorados.</param>
        public AttributesReader(IXmlReader elementReader, bool ignoreNotRegistered)
        {
            this.ignoreNotRegistered = ignoreNotRegistered;
            this.attributes = new Dictionary<string, Tuple<Action<T, string>, bool>>(
                        StringComparer.InvariantCultureIgnoreCase);
            this.elementReader = elementReader;
        }

        /// <summary>
        /// Obtém o nome associado ao leitor.
        /// </summary>
        public string Name
        {
            get
            {
                return this.elementReader.Name;
            }
        }

        /// <summary>
        /// Otbém o tipo do leitor.
        /// </summary>
        public ReaderType ReaderType
        {
            get
            {
                return ReaderType.ATTRIBUTE;
            }
        }

        /// <summary>
        /// Permite registar um leitor de atributos.
        /// </summary>
        /// <param name="name">O nome do atributo.</param>
        /// <param name="action">A função responsável atribuir a informação ao contentor.</param>
        /// <param name="mandatory">Valor que indica se o atributo é obrigatório.</param>
        public void RegisterAttribute(
            string name,
            Action<T, string> action,
            bool mandatory)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The attribute's name can't be null nor empty.");
            }
            else if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            else
            {
                if (this.attributes.ContainsKey(name))
                {
                    throw new ArgumentException("An attribute with the provided name already exists.");
                }
                else
                {
                    this.attributes.Add(
                        name,
                        Tuple.Create(action, mandatory));
                }
            }
        }

        /// <summary>
        /// Efectua a leitura sobre um leitor do tipo <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">O leitor de XML.</param>
        /// <param name="updatable">O contentor.</param>
        public void Read(XmlReader reader, object updatable)
        {
            var readedAttributes = new Dictionary<string, string>(
                StringComparer.InvariantCultureIgnoreCase);
            if (reader.MoveToFirstAttribute())
            {
                var attributeName = reader.Name;
                if (readedAttributes.ContainsKey(attributeName))
                {
                    throw new Exception(string.Format(
                        "The attribute '{0}' appears twice in element '{1}'.",
                        attributeName,
                        this.elementReader.Name));
                }
                else
                {
                    readedAttributes.Add(attributeName, reader.Value);
                }
            }

            foreach (var attributeReader in this.attributes)
            {
                var currentAttributeName = attributeReader.Key;
                var currentAttributeReader = attributeReader.Value;
                var readedAttributeValue = default(string);
                if (readedAttributes.TryGetValue(currentAttributeName, out readedAttributeValue))
                {
                    currentAttributeReader.Item1.Invoke((T)updatable, readedAttributeValue);
                }
                else if (currentAttributeReader.Item2)
                {
                    throw new Exception(string.Format(
                        "The attribute '{0}' was no found in element '{1}'.",
                        currentAttributeName,
                        this.elementReader.Name));
                }
            }

            if (!this.ignoreNotRegistered)
            {
                foreach (var kvp in this.attributes)
                {
                    if (!readedAttributes.ContainsKey(kvp.Key))
                    {
                        throw new Exception(string.Format(
                        "The attribute '{0}' was not expected in element '{1}'.",
                        kvp.Key,
                        this.elementReader.Name));
                    }
                }
            }
        }
    }

    /// <summary>
    /// Implementa um leitor de elementos.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que constituem os contentores.</typeparam>
    /// <typeparam name="P">O tipo de objectos que constituem o resultado da leitura do elemento.</typeparam>
    internal class ElementReader<T, P> : IXmlElementReader<T, P>
    {
        /// <summary>
        /// O leitor de atributos.
        /// </summary>
        private AttributesReader<T> attributesReader;

        /// <summary>
        /// Lista de tuplos que contêm o leitor dos elementos, o número mínimo de elementos
        /// expectáveis e o número máximo de elementos expectáveis.
        /// </summary>
        private List<Tuple<IXmlReader, int, int>> elementReaders;

        /// <summary>
        /// A associação dos elementos ao respectivo índice.
        /// </summary>
        private Dictionary<string, int> elementReadersIndexer;

        /// <summary>
        /// A função responsável pela atribuição do texto lido ao contentor.
        /// </summary>
        private Action<T, string> textAction;

        /// <summary>
        /// A função que permite associar o objecto lido ao contentor.
        /// </summary>
        Func<T, P> attachFunction;

        /// <summary>
        /// O nome do elemento.
        /// </summary>
        private string name;

        /// <summary>
        /// Valor que indica se os elmentos não registados serão ignorados.
        /// </summary>
        private bool ignoreNotRegisteredElements;

        /// <summary>
        /// Valor que indica se a ordem pode ser ignorada.
        /// </summary>
        private bool ignoreOrder;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ElementReader{T, P}"/>
        /// </summary>
        /// <param name="textAction">A função responsável pela atribuição do texto lido ao contentor.</param>
        /// <param name="attachFunction">
        /// A função responsável pela atribuição do objecto associado ao elemento ao contentor.</param>
        /// <param name="name">O nome do elemento.</param>
        /// <param name="ignoreNotRegisteredElements">
        /// Valor que indica se os elementos não registados podem ser ignorados.
        /// </param>
        /// <param name="ignoreOrder">Valor que indica se a ordem pode ser ignorada.</param>
        /// <param name="ignoreNotRegisteredAttributes">
        /// Valor que indica se os atributos não registados podem ser ignorados.
        /// </param>
        public ElementReader(
            Action<T, string> textAction,
            Func<T, P> attachFunction,
            string name,
            bool ignoreNotRegisteredElements,
            bool ignoreOrder,
            bool ignoreNotRegisteredAttributes)
        {
            if (textAction == null)
            {
                throw new ArgumentNullException("textAction");
            }
            else if (attachFunction == null)
            {
                throw new ArgumentNullException("attachFunction");
            }
            else if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The element's name can't be null nor empty.");
            }
            else
            {
                this.textAction = textAction;
                this.attachFunction = attachFunction;
                this.name = name;
                this.ignoreNotRegisteredElements = ignoreNotRegisteredElements;
                this.ignoreOrder = ignoreOrder;

                this.attributesReader = new AttributesReader<T>(this, ignoreNotRegisteredAttributes);
                this.elementReaders = new List<Tuple<IXmlReader, int, int>>();
                this.elementReadersIndexer = new Dictionary<string, int>(
                    StringComparer.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Obtém o tipo do leitor.
        /// </summary>
        public ReaderType ReaderType
        {
            get
            {
                return ReaderType.ELEMENT;
            }
        }

        /// <summary>
        /// Obtém o nome do elemento.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Obtém o objecto que encapsula os leitores de atributos.
        /// </summary>
        public IXmlAttributeReader<T> AttributesReader
        {
            get
            {
                return this.attributesReader;
            }
        }

        /// <summary>
        /// Regista um leitor de elementos descendentes.
        /// </summary>
        /// <param name="textAction">A função responsável pela atribuição do texto lido ao contentor.</param>
        /// <param name="attachFunction">
        /// A função responsável pela atribuição do objecto associado ao elemento ao contentor.
        /// </param>
        /// <param name="elementName">O nome do elemento.</param>
        /// <param name="ignoreNotRegisteredElements">
        /// Valor que indica se os elementos não registados podem ser ignorados.
        /// </param>
        /// <param name="ignoreOrder">Valor que indica se a ordem pode ser ignorada.</param>
        /// <param name="ignoreNotRegisteredAttributes">
        /// Valor que indica se os atributos não registados podem ser ignorados.
        /// </param>
        /// <param name="minimum">O mínimo de elementos do tipo que podem ser encontrados.</param>
        /// <param name="maximum">O número máximo de elmentos do tipo que podem ser encontrados.</param>
        /// <returns>O leitor de elementos.</returns>
        public IXmlElementReader<T, P> RegisterElementReader(
            Action<T, string> textAction,
            Func<T, P> attachFunction,
            string elementName,
            bool ignoreNotRegisteredElements,
            bool ignoreOrder,
            bool ignoreNotRegisteredAttributes,
            int minimum,
            int maximum)
        {
            if (maximum < minimum)
            {
                throw new ArgumentException("The maximum value must be greater or equal than the minimum value.");
            }
            else
            {
                if (this.elementReadersIndexer.ContainsKey(elementName))
                {
                    throw new ArgumentException("A reader with the same name was already provided.");
                }
                else
                {
                    var element = new ElementReader<T, P>(
                    textAction,
                    attachFunction,
                    elementName,
                    ignoreNotRegisteredElements,
                    ignoreOrder,
                    ignoreNotRegisteredAttributes);
                    this.elementReadersIndexer.Add(
                        elementName,
                        this.elementReaders.Count);
                    this.elementReaders.Add(
                        Tuple.Create((IXmlReader)element, minimum, maximum));
                    return element;
                }
            }
        }

        /// <summary>
        /// Efectua a leitura sobre um leitor do tipo <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">O leitor de XML.</param>
        /// <param name="updatable">O contentor.</param>
        public void Read(XmlReader reader, object updatable)
        {
            var created = this.attachFunction.Invoke((T)updatable);
            if (this.ignoreOrder)
            {
                this.ReadIgnoreOrder(reader, (T)updatable, created);
            }
            else
            {
                this.ReadNotIgnoreOrder(reader, (T)updatable, created);
            }
        }

        /// <summary>
        /// Efectua a leitura do elemento a partir de um leitor do tipo <see cref="XmlReader"/> sem
        /// ignorar a ordem.
        /// </summary>
        /// <param name="reader">O leitor de XML.</param>
        /// <param name="updatable">O contentor.</param>
        /// <param name="created">O objecto associado ao elemento.</param>
        private void ReadNotIgnoreOrder(XmlReader reader, T updatable, P created)
        {
            // Leitura dos atributos
            this.attributesReader.Read(reader, created);

            // Leitura do texto e elementos
            var text = string.Empty;
            if (reader.NodeType == XmlNodeType.EndElement)
            {
                for (int i = 0; i < this.elementReaders.Count; ++i)
                {
                    var currentReader = this.elementReaders[i];
                    if (currentReader.Item2 > 0)
                    {
                        throw new Exception(string.Format(
                            "It seams that element '{0}' is empty but it was expected at least one child: '{1}'",
                            this.name,
                            currentReader.Item1.Name));
                    }
                }
            }
            else if (this.ignoreNotRegisteredElements)
            {
                while (reader.Read() &&
                    reader.NodeType != XmlNodeType.Element &&
                    reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        text += reader.Value;
                    }
                }

                // Não tem nós descendentes
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    for (int i = 0; i < this.elementReaders.Count; ++i)
                    {
                        var currentReader = this.elementReaders[i];
                        if (currentReader.Item2 > 0)
                        {
                            throw new Exception(string.Format(
                                "It seams that element '{0}' is empty but it was expected at least one child: '{1}'",
                                this.name,
                                currentReader.Item1.Name));
                        }
                    }
                }
                else
                {
                    // Trata-se de um elemento descendente
                    var readedIndex = -1;
                    var currentCount = 0;
                    while (reader.Read() &&
                        reader.NodeType != XmlNodeType.EndElement)
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            var currentName = reader.Name;
                            var currentIndex = default(int);
                            if (this.elementReadersIndexer.TryGetValue(
                                currentName,
                                out currentIndex))
                            {
                                if (currentIndex > readedIndex)
                                {
                                    // Verifica se todos os elementos estão de acordo com as restrições
                                    for (int i = readedIndex + 1; i < currentIndex; ++i)
                                    {
                                        var testReader = this.elementReaders[i];
                                        if (testReader.Item2 > 0)
                                        {
                                            throw new Exception(string.Format(
                                                "Expected at least {0} elements '{1}' as childs of '{2}' but no one was found in that order.",
                                                testReader.Item2,
                                                testReader.Item1.Name,
                                                this.name));
                                        }
                                    }

                                    currentCount = 1;
                                    readedIndex = currentIndex;
                                    var currentReader = this.elementReaders[currentIndex];
                                    if (currentCount <= currentReader.Item3)
                                    {
                                        currentReader.Item1.Read(reader, created);
                                    }
                                    else
                                    {
                                        throw new Exception(string.Format(
                                            "There are {0} elements named '{1}' but it was expected less that {2} as childs of '{3}'.",
                                            currentCount,
                                            currentReader.Item1.Name,
                                            currentReader.Item3,
                                            this.name));
                                    }
                                }
                                else if (currentIndex == readedIndex)
                                {
                                    ++currentCount;
                                    var currentReader = this.elementReaders[currentIndex];
                                    if (currentCount <= currentReader.Item3)
                                    {
                                        currentReader.Item1.Read(reader, created);
                                    }
                                    else
                                    {
                                        throw new Exception(string.Format(
                                            "There are {0} elements named '{1}' but it was expected less that {2} as childs of '{3}'.",
                                            currentCount,
                                            currentReader.Item1.Name,
                                            currentReader.Item3,
                                            this.name));
                                    }
                                }
                                else
                                {
                                    // Os elementos não satisfazem qualquer ordenação
                                    throw new Exception(string.Format(
                                        "Wrong order in child elements of element '{0}'.",
                                        this.name));
                                }
                            }
                            else
                            {
                                // Ignora o elemento encontrado.
                                this.ReadIgnore(reader);
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Text)
                        {
                            // O texto é considerado no seu total
                            text += reader.Value;
                        }
                    }
                }
            }
            else
            {
                while (reader.Read() &&
                    reader.NodeType != XmlNodeType.Element &&
                    reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        text += reader.Value;
                    }
                }

                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (this.elementReaders.Count == 0)
                    {
                        throw new Exception(string.Format(
                            "Found child '{0}' in element '{1}' but none is expected.",
                            reader.Name,
                            this.name));
                    }
                    else
                    {
                        var currentName = reader.Name;
                        var currentCount = 0;
                        var currentIndex = 0;

                        var currentReader = this.elementReaders[currentIndex];
                        var status = !StringComparer.InvariantCultureIgnoreCase.Equals(
                                currentName,
                                currentReader.Item1.Name);
                        while (status)
                        {
                            if (currentReader.Item2 > currentCount)
                            {
                                throw new Exception(string.Format(
                                    "Expected at least {0} child elements named '{1}' in '{2}' but there is only {3}.",
                                    currentReader.Item2,
                                    currentReader.Item1.Name,
                                    this.name,
                                    currentCount));
                            }

                            ++currentIndex;
                            if (currentIndex < this.elementReaders.Count)
                            {
                                currentReader = this.elementReaders[currentIndex];
                                status = !StringComparer.InvariantCultureIgnoreCase.Equals(
                                currentName,
                                currentReader.Item1.Name);
                            }
                            else
                            {
                                status = false;
                            }
                        }

                        if (currentIndex < this.elementReaders.Count)
                        {
                            ++currentCount;
                            if (currentCount <= currentReader.Item3)
                            {
                                currentReader.Item1.Read(reader, created);
                            }
                            else
                            {
                                throw new Exception(string.Format(
                                    "There are {0} elements named '{1}' but it was expected less that {2} as childs of '{3}'.",
                                    currentCount,
                                    currentReader.Item1.Name,
                                    currentReader.Item3,
                                    this.name));
                            }

                            while (reader.Read() &&
                                reader.NodeType != XmlNodeType.EndElement)
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    text += reader.Value;
                                }
                                else if (reader.NodeType == XmlNodeType.Element)
                                {
                                    currentReader = this.elementReaders[currentIndex];
                                    var currentReaderName = currentReader.Item1.Name;
                                    if (StringComparer.InvariantCultureIgnoreCase.Equals(
                                        currentName,
                                        currentReaderName))
                                    {
                                        ++currentCount;
                                        if (currentCount <= currentReader.Item3)
                                        {
                                            currentReader.Item1.Read(reader, created);
                                        }
                                        else
                                        {
                                            throw new Exception(string.Format(
                                                "There are {0} elements named '{1}' but it was expected less that {2} as childs of '{3}'.",
                                                currentCount,
                                                currentReader.Item1.Name,
                                                currentReader.Item3,
                                                this.name));
                                        }
                                    }
                                    else
                                    {
                                        currentCount = 0;
                                        status = !StringComparer.InvariantCultureIgnoreCase.Equals(
                                            currentName,
                                            currentReader.Item1.Name);
                                        while (status)
                                        {
                                            if (currentReader.Item2 > currentCount)
                                            {
                                                throw new Exception(string.Format(
                                                    "Expected at least {0} child elements named '{1}' in '{2}' but there is only {3}.",
                                                    currentReader.Item2,
                                                    currentReader.Item1.Name,
                                                    this.name,
                                                    currentCount));
                                            }

                                            ++currentIndex;
                                            if (currentIndex < this.elementReaders.Count)
                                            {
                                                currentReader = this.elementReaders[currentIndex];
                                                status = !StringComparer.InvariantCultureIgnoreCase.Equals(
                                                currentName,
                                                currentReader.Item1.Name);
                                            }
                                            else
                                            {
                                                status = false;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                else
                {
                    // Fim do elemento actual
                    for (int i = 0; i < this.elementReaders.Count; ++i)
                    {
                        var testReader = this.elementReaders[i];
                        if (testReader.Item2 > 0)
                        {
                            throw new Exception(string.Format(
                                "Expected at least {0} elements '{1}' as childs of '{2}' but no one was found in that order.",
                                testReader.Item2,
                                testReader.Item1.Name,
                                this.name));
                        }
                    }
                }
            }

            if (this.textAction != null)
            {
                this.textAction.Invoke((T)updatable, text);
            }
        }

        /// <summary>
        /// Efectua a leitura do elemento a partir de um leitor do tipo <see cref="XmlReader"/>, podendo
        /// ser a ordem ignorada.
        /// </summary>
        /// <param name="reader">O leitor de XML.</param>
        /// <param name="updatable">O contentor.</param>
        /// <param name="created">O objecto associado ao elemento.</param>
        private void ReadIgnoreOrder(XmlReader reader, T updatable, P created)
        {
            this.attributesReader.Read(reader, created);
            var text = string.Empty;
            var readed = new Dictionary<string, int>(
                StringComparer.InvariantCultureIgnoreCase);
            if (this.ignoreNotRegisteredElements)
            {
                while (reader.Read() &&
                    reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        text += reader.Value;
                    }
                    else if (reader.NodeType == XmlNodeType.Element)
                    {
                        var currentReaderIndex = default(int);
                        if (this.elementReadersIndexer.TryGetValue(
                            reader.Name,
                            out currentReaderIndex))
                        {
                            var currentReader = this.elementReaders[currentReaderIndex];
                            var currentCount = default(int);
                            if (readed.TryGetValue(
                                currentReader.Item1.Name,
                                out currentCount))
                            {
                                ++currentCount;
                                if (currentCount <= currentReader.Item3)
                                {
                                    currentReader.Item1.Read(reader, created);
                                    readed[currentReader.Item1.Name] = currentCount;
                                }
                                else
                                {
                                    throw new Exception(string.Format(
                                                   "There are {0} elements named '{1}' but it was expected less than {2} as childs of '{3}'.",
                                                   currentCount,
                                                   currentReader.Item1.Name,
                                                   currentReader.Item3,
                                                   this.name));
                                }
                            }
                            else
                            {
                                if (currentReader.Item3 > 0)
                                {
                                    currentReader.Item1.Read(reader, created);
                                    readed.Add(currentReader.Item1.Name, currentCount);
                                }
                                else
                                {
                                    throw new Exception(string.Format(
                                                      "There are {0} elements named '{1}' but it was expected less than {2} as childs of '{3}'.",
                                                      currentCount,
                                                      currentReader.Item1.Name,
                                                      currentReader.Item3,
                                                      this.name));
                                }
                            }
                        }
                        else
                        {
                            this.ReadIgnore(reader);
                        }
                    }
                }
            }
            else
            {
                while (reader.Read() &&
                    reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        text += reader.Value;
                    }
                    else if (reader.NodeType == XmlNodeType.Element)
                    {
                        var currentReaderIndex = default(int);
                        if (this.elementReadersIndexer.TryGetValue(
                            reader.Name,
                            out currentReaderIndex))
                        {
                            var currentReader = this.elementReaders[currentReaderIndex];
                            var currentCount = default(int);
                            if (readed.TryGetValue(
                                currentReader.Item1.Name,
                                out currentCount))
                            {
                                ++currentCount;
                                if (currentCount <= currentReader.Item3)
                                {
                                    currentReader.Item1.Read(reader, created);
                                }
                                else
                                {
                                    throw new Exception(string.Format(
                                                   "There are {0} elements named '{1}' but it was expected less that {2} as childs of '{3}'.",
                                                   currentCount,
                                                   currentReader.Item1.Name,
                                                   currentReader.Item3,
                                                   this.name));
                                }
                            }
                        }
                        else
                        {
                            throw new Exception(string.Format(
                                "Unexpected child element '{0}' in element '{1}'.",
                                reader.Name,
                                this.name));
                        }
                    }
                }
            }

            // Valida os elementos lidos
            for (int i = 0; i < this.elementReaders.Count; ++i)
            {
                var currentReader = this.elementReaders[i];
                var currentCount = default(int);
                if (readed.TryGetValue(
                    currentReader.Item1.Name,
                    out currentCount))
                {
                    if (currentReader.Item2 > currentCount)
                    {
                        throw new Exception(string.Format(
                                "Expected at least {0} elements '{1}' as childs of '{2}' but found {3}.",
                                currentReader.Item2,
                                currentReader.Item1.Name,
                                this.name,
                                currentCount));
                    }
                }
                else
                {
                    if (currentReader.Item2 > 0)
                    {
                        throw new Exception(string.Format(
                                "Expected at least {0} elements '{1}' as childs of '{2}' but no one was found.",
                                currentReader.Item2,
                                currentReader.Item1.Name,
                                this.name));
                    }
                }
            }

            if (this.textAction != null)
            {
                this.textAction.Invoke(updatable, text);
            }
        }

        /// <summary>
        /// Permite ignorar elemento actual.
        /// </summary>
        /// <param name="reader">O leitor de XML.</param>
        private void ReadIgnore(XmlReader reader)
        {
            if (!reader.IsEmptyElement)
            {
                if (reader.NodeType != XmlNodeType.EndElement)
                {
                    var count = 1;
                    var elementName = reader.Name;
                    var status = reader.Read();
                    while (status)
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (StringComparer.InvariantCultureIgnoreCase.Equals(
                                elementName,
                                reader.Name))
                            {
                                ++count;
                            }

                            reader.Read();
                        }
                        else if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            if (StringComparer.InvariantCultureIgnoreCase.Equals(
                                   elementName,
                                   reader.Name))
                            {
                                --count;
                                if (count == 0)
                                {
                                    status = false;
                                }
                                else
                                {
                                    status = reader.Read();
                                }
                            }
                            else
                            {
                                status = reader.Read();
                            }
                        }
                        else
                        {
                            status = reader.Read();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Implementa um leitor de documentos XML no qual a ordem não importa.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que constituem os contentores.</typeparam>
    public class UnorderedDocumentReader<T>
    {
        /// <summary>
        /// A função responsável pela criação de objectos.
        /// </summary>
        private Func<T> objectFactory;

        /// <summary>
        /// O conjunto de leitores de elementos.
        /// </summary>
        private Dictionary<string, IXmlReader> elementsReader;

        /// <summary>
        /// O objecto actual.
        /// </summary>
        private T current;

        /// <summary>
        /// Valor que indica se o leitor se encontra iniciado.
        /// </summary>
        private bool started;

        /// <summary>
        /// Valor que indica se o leitor já atingiu o final.
        /// </summary>
        private bool ended;

        /// <summary>
        /// O nome da raiz associada ao documento.
        /// </summary>
        private string rootName;

        /// <summary>
        /// O leitor de XML.
        /// </summary>
        private XmlReader reader;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UnorderedDocumentReader{T}"/>.
        /// </summary>
        /// <param name="reader">O leitor de XML.</param>
        /// <param name="rootName">O nome da raiz.</param>
        /// <param name="objectFactory">A função responsável pela criação do contentor.</param>
        public UnorderedDocumentReader(
            XmlReader reader,
            string rootName,
            Func<T> objectFactory)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else if (objectFactory == null)
            {
                throw new ArgumentNullException("objectFactory");
            }
            else if (string.IsNullOrWhiteSpace(rootName))
            {
                throw new ArgumentException("The root name can't be null nor empty.");
            }
            else
            {
                this.reader = reader;
                this.rootName = rootName;
                this.started = false;
                this.ended = false;
                this.objectFactory = objectFactory;
                this.elementsReader = new Dictionary<string, IXmlReader>(
                    StringComparer.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Obtém o elemento lido corrente.
        /// </summary>
        public T Current
        {
            get
            {
                if (!this.started)
                {
                    throw new Exception("The reader was not started yet.");
                }
                else if (this.ended)
                {
                    throw new Exception("The reader has already ended.");
                }
                else
                {
                    return this.current;
                }
            }
        }

        /// <summary>
        /// Efectua o registo de um leitor de elementos associado ao leitor de documentos.
        /// </summary>
        /// <typeparam name="P">O tipo de objectos associados aos leitores de elementos.</typeparam>
        /// <param name="textAction">A função responsável pela atribuição do texto lido ao contentor.</param>
        /// <param name="attachFunction">
        /// A função responsável pela atribuição do objecto associado ao leitor ao contentor.
        /// </param>
        /// <param name="elementName">O nome do elemento.</param>
        /// <param name="ignoreNotRegisteredElements">
        /// Valor que indica se os elementos não registados poderão ser ignorados.
        /// </param>
        /// <param name="ignoreOrder">Valor que indica se a ordem pode ser ignorada.</param>
        /// <param name="ignoreNotRegisteredAttributes">
        /// Valor que indica se os atributos não registados podem ser ignorados.
        /// </param>
        /// <returns></returns>
        public IXmlReader RegisterElementReader<P>(
            Action<T, string> textAction,
            Func<T, P> attachFunction,
            string elementName,
            bool ignoreNotRegisteredElements,
            bool ignoreOrder,
            bool ignoreNotRegisteredAttributes)
        {
            if (this.elementsReader.ContainsKey(elementName))
            {
                throw new ArgumentException("The provided element name was already registered.");
            }
            else
            {
                var element = new ElementReader<T, P>(
                    textAction,
                    attachFunction,
                    elementName,
                    ignoreNotRegisteredElements,
                    ignoreOrder,
                    ignoreNotRegisteredAttributes);
                this.elementsReader.Add(elementName, element);
                return element;
            }
        }

        /// <summary>
        /// Efectua a leitura do próximo elemento.
        /// </summary>
        /// <returns>Verdadeiro caso a leitura seja bem sucedida e falso caso contrário.</returns>
        public bool ReadNext()
        {
            if (this.ended)
            {
                return false;
            }
            else if (!this.started)
            {
                if (reader.Read())
                {
                    return this.InnerReadStart();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (reader.Read())
                {
                    return this.InnerReadNext();
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Efectua a leitura do próximo elemento.
        /// </summary>
        /// <remarks>
        /// Esta função é usada apenas por classes internas.
        /// </remarks>
        /// <returns>Verdadeiro caso a leitura seja bem sucedida e falso caso contrário.</returns>
        internal bool InternalReadNext()
        {
            if (this.ended)
            {
                return false;
            }
            else if (!this.started)
            {
                return this.InnerReadStart();
            }
            else
            {
                return this.InnerReadNext();
            }
        }

        /// <summary>
        /// Efectua a leitura do primeiro elemento.
        /// </summary>
        /// <returns>Verdadeiro caso a leitura seja bem sucedida e falso caso contrário.</returns>
        private bool InnerReadStart()
        {
            var status = true;
            var foundElement = false;
            while (status)
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    foundElement = true;
                    status = false;
                }
                else
                {
                    status = reader.Read();
                }
            }

            if (foundElement)
            {
                if (StringComparer.InvariantCultureIgnoreCase.Equals(
                    reader.Name,
                    this.rootName))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            var currentReader = default(IXmlReader);
                            if (this.elementsReader.TryGetValue(
                                reader.Name,
                                out currentReader))
                            {
                                this.current = this.objectFactory.Invoke();
                                currentReader.Read(reader, this.current);
                                this.started = true;
                                return true;
                            }
                            else
                            {
                                this.ReadIgnore(reader);
                            }
                        }
                    }

                    this.started = true;
                    this.ended = true;
                    return false;
                }
                else
                {
                    throw new Exception(string.Format(
                        "Expected '{0}' as root element but found '{1}'. Maybe the XML reader was already started.",
                        this.rootName,
                        reader.Name));
                }
            }
            else
            {
                throw new Exception("No element was found in the provided XML reader. Maybe the XML reader was already started.");
            }
        }

        /// <summary>
        /// Efectua a leitura do próximo elemento.
        /// </summary>
        /// <returns>Verdadeiro caso a leitura seja bem sucedida e falso caso contrário.</returns>
        private bool InnerReadNext()
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                var currentReader = default(IXmlReader);
                if (this.elementsReader.TryGetValue(
                    reader.Name,
                    out currentReader))
                {
                    this.current = this.objectFactory.Invoke();
                    currentReader.Read(reader, this.current);
                    return true;
                }
                else
                {
                    this.ReadIgnore(reader);
                }
            }
            else if (reader.NodeType == XmlNodeType.EndElement)
            {
                this.ended = true;
                return false;
            }

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    var currentReader = default(IXmlReader);
                    if (this.elementsReader.TryGetValue(
                        reader.Name,
                        out currentReader))
                    {
                        this.current = this.objectFactory.Invoke();
                        currentReader.Read(reader, this.current);
                        return true;
                    }
                    else
                    {
                        this.ReadIgnore(reader);
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    this.ended = true;
                    return false;
                }
            }

            this.ended = true;
            return false;
        }

        /// <summary>
        /// Permite ignorar elemento actual.
        /// </summary>
        /// <param name="reader">O leitor de XML.</param>
        private void ReadIgnore(XmlReader reader)
        {
            if (reader.NodeType != XmlNodeType.EndElement)
            {
                var count = 1;
                var elementName = reader.Name;
                var status = reader.Read();
                while (status)
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (StringComparer.InvariantCultureIgnoreCase.Equals(
                            elementName,
                            reader.Name))
                        {
                            ++count;
                        }

                        reader.Read();
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (StringComparer.InvariantCultureIgnoreCase.Equals(
                               elementName,
                               reader.Name))
                        {
                            --count;
                            if (count == 0)
                            {
                                status = false;
                            }
                            else
                            {
                                status = reader.Read();
                            }
                        }
                        else
                        {
                            status = reader.Read();
                        }
                    }
                    else
                    {
                        status = reader.Read();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Permite efectuar a leitura multi-canal de um XML.
    /// </summary>
    /// <remarks>
    /// Um XML pode ser deifnido por uma raiz que contém um conjunto de elementos que poderão estar
    /// associados a objectos diferentes. Cada um desses elementos constitui um canal.
    /// </remarks>
    public class UnorderedMutlichannelReader
    {
        /// <summary>
        /// O conjunto de leitores de sub-documentos.
        /// </summary>
        private Dictionary<string, UnorderedDocumentReader<object>> documentReaders;

        /// <summary>
        /// O valor lido actual.
        /// </summary>
        private Tuple<string, object> current;

        /// <summary>
        /// Indica se o leitor multi-canal foi iniciado.
        /// </summary>
        private bool started;

        /// <summary>
        /// Indica se o leitor multi-canal finalizou.
        /// </summary>
        private bool ended;

        /// <summary>
        /// O nome do elemento que será considerado como sendo a raiz.
        /// </summary>
        string rootName;

        /// <summary>
        /// O leitor de XML.
        /// </summary>
        private XmlReader reader;

        /// <summary>
        /// O leitor de documentos.
        /// </summary>
        private UnorderedDocumentReader<object> currentDocumentReader;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UnorderedMutlichannelReader"/>.
        /// </summary>
        /// <param name="reader">O leitor de XML.</param>
        /// <param name="rootName">O nome da raiz.</param>
        public UnorderedMutlichannelReader(
            XmlReader reader,
            string rootName)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else if (string.IsNullOrWhiteSpace(rootName))
            {
                throw new ArgumentException("The root name can't be null nor empty.");
            }
            else
            {
                this.reader = reader;
                this.rootName = rootName;
                this.started = false;
                this.ended = false;
                this.documentReaders = new Dictionary<string, UnorderedDocumentReader<object>>(
                    StringComparer.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Obtém o par nome do canal / objecto associado à última leirua realizada.
        /// </summary>
        public Tuple<string, object> Current
        {
            get
            {
                if (this.ended)
                {
                    throw new Exception("The reader has already ended.");
                }
                else if (this.started)
                {
                    return this.current;
                }
                else
                {
                    throw new Exception("The reader wasn't started yet.");
                }
            }
        }

        /// <summary>
        /// Regista um leitor de documentos.
        /// </summary>
        /// <param name="elementName">O nome associado ao leitor.</param>
        /// <param name="objectFactory">A função responsável pela criação do contentor para o elemento.</param>
        /// <returns>O leitor de documentos.</returns>
        public UnorderedDocumentReader<object> RegisterDocumentReader(
            string elementName,
            Func<object> objectFactory)
        {
            if (this.documentReaders.ContainsKey(elementName))
            {
                throw new ArgumentException(string.Format(
                    "A document reader with the same name was already added: {0}.",
                    elementName));
            }
            else
            {
                var documentReader = new UnorderedDocumentReader<object>(
                    this.reader,
                    elementName,
                    objectFactory);
                this.documentReaders.Add(elementName, documentReader);
                return documentReader;
            }
        }

        /// <summary>
        /// Realiza a leitura do próximo elmento.
        /// </summary>
        /// <returns>Verdadeiro caso a leitura seja bem sucedida e falso caso contrário.</returns>
        public bool ReadNext()
        {
            if (this.ended)
            {
                return false;
            }
            else if (!this.started)
            {
                this.started = true;
                var status = reader.Read();
                var foundElement = false;
                while (status)
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        foundElement = true;
                        status = false;
                    }
                    else
                    {
                        status = reader.Read();
                    }
                }

                if (foundElement)
                {
                    if (StringComparer.InvariantCultureIgnoreCase.Equals(
                        reader.Name,
                        this.rootName))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.EndElement)
                            {
                                this.started = true;
                                this.ended = true;
                                return false;
                            }
                            else if (reader.NodeType == XmlNodeType.Element)
                            {
                                if (this.documentReaders.TryGetValue(
                                    reader.Name,
                                    out this.currentDocumentReader))
                                {
                                    var currentName = reader.Name.Trim();
                                    if (this.currentDocumentReader.InternalReadNext())
                                    {
                                        this.current = Tuple.Create(
                                            currentName,
                                            this.currentDocumentReader.Current);
                                        return true;
                                    }
                                }
                                else
                                {
                                    this.ReadIgnore(reader);
                                }
                            }
                        }

                        this.ended = true;
                        return false;
                    }
                    else
                    {
                        throw new Exception(string.Format(
                            "Expected '{0}' as root element but found '{1}'. Maybe the XML reader was already started.",
                            this.rootName,
                            reader.Name));
                    }
                }
                else
                {
                    throw new Exception("No element was found in the provided XML reader. Maybe the XML reader was already started.");
                }
            }
            else
            {
                if (this.currentDocumentReader == null)
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            this.ended = true;
                            return false;
                        }
                        else if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (this.documentReaders.TryGetValue(
                                reader.Name,
                                out this.currentDocumentReader))
                            {
                                if (this.currentDocumentReader.InternalReadNext())
                                {
                                    this.current = Tuple.Create(
                                        reader.Name,
                                        this.currentDocumentReader.Current);
                                    return true;
                                }
                                else
                                {
                                    this.currentDocumentReader = null;
                                }
                            }
                            else
                            {
                                this.ReadIgnore(reader);
                            }
                        }
                    }

                    this.ended = true;
                    return false;
                }
                else
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            this.ended = true;
                            return false;
                        }
                        else if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (this.currentDocumentReader.InternalReadNext())
                            {
                                this.current = Tuple.Create(
                                    reader.Name,
                                    this.currentDocumentReader.Current);
                                return true;
                            }
                            else
                            {
                                this.currentDocumentReader = null;
                            }
                        }
                    }

                    this.ended = true;
                    return false;
                }
            }
        }

        /// <summary>
        /// Permite ignorar elemento actual.
        /// </summary>
        /// <param name="reader">O leitor de XML.</param>
        private void ReadIgnore(XmlReader reader)
        {
            if (!reader.IsEmptyElement)
            {
                if (reader.NodeType != XmlNodeType.EndElement)
                {
                    var count = 1;
                    var elementName = reader.Name;
                    var status = reader.Read();
                    while (status)
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (StringComparer.InvariantCultureIgnoreCase.Equals(
                                elementName,
                                reader.Name))
                            {
                                ++count;
                            }

                            reader.Read();
                        }
                        else if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            if (StringComparer.InvariantCultureIgnoreCase.Equals(
                                   elementName,
                                   reader.Name))
                            {
                                --count;
                                if (count == 0)
                                {
                                    status = false;
                                }
                                else
                                {
                                    status = reader.Read();
                                }
                            }
                            else
                            {
                                status = reader.Read();
                            }
                        }
                        else
                        {
                            status = reader.Read();
                        }
                    }
                }
            }
        }
    }
}
