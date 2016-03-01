namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Efectua a leitura de ficheiros do GenBank:
    /// 
    /// </summary>
    public class GenBankReader
    {
        /// <summary>
        /// A cultura genérica a ser utilizada no tratamento das datas.
        /// </summary>
        private static CultureInfo cultureProvider = CultureInfo.InvariantCulture;

        /// <summary>
        /// O modelo actual.
        /// </summary>
        private GenBankModel current;

        /// <summary>
        /// Valor que indica se o leitor já foi iniciado.
        /// </summary>
        private bool started;

        /// <summary>
        /// Valor que indica se o leitor já foi terminado.
        /// </summary>
        private bool ended;

        /// <summary>
        /// O leitor de texto.
        /// </summary>
        private TextReader reader;

        /// <summary>
        /// Mantém o mapeamento entre os campos principais e os respectivos leitores.
        /// </summary>
        private Dictionary<string, AFieldReader<GenBankModel>> mainFields;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GenBankReader"/>.
        /// </summary>
        /// <param name="reader">O leitor de informação.</param>
        public GenBankReader(Stream reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else if (reader.CanRead)
            {
                this.reader = new StreamReader(reader, Encoding.UTF8, false);
                this.started = false;
                this.ended = false;
                this.SetupMainFields();
            }
            else
            {
                throw new UtilitiesException("The reader can't be read.");
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GenBankReader"/>.
        /// </summary>
        /// <param name="reader">O leitor de informação.</param>
        /// <param name="encoding">A codificação associada ao leitor.</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        /// Indica se o leitor providencia os bytes do BOM.
        /// </param>
        public GenBankReader(
            Stream reader,
            Encoding encoding,
            bool detectEncodingFromByteOrderMarks)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            else if (reader.CanRead)
            {
                this.reader = new StreamReader(
                    reader,
                    encoding,
                    detectEncodingFromByteOrderMarks);
                this.started = false;
                this.ended = false;
                this.SetupMainFields();
            }
            else
            {
                throw new UtilitiesException("The reader can't be read.");
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GenBankReader"/>.
        /// </summary>
        /// <param name="reader">O leitor de texto.</param>
        public GenBankReader(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else
            {
                this.reader = reader;
                this.started = false;
                this.ended = false;
                this.SetupMainFields();
            }
        }

        /// <summary>
        /// Obtém o modelo corrente da leitura do GenBank.
        /// </summary>
        public GenBankModel Current
        {
            get
            {
                if (!this.started)
                {
                    throw new UtilitiesException("The reader was not started.");
                }
                else if (this.ended)
                {
                    throw new UtilitiesException("The raeder has already ended.");
                }
                else
                {
                    return this.current;
                }
            }
        }

        /// <summary>
        /// Efectua a leitura do próximo registo no leitor de informação.
        /// </summary>
        /// <returns>Verdadeiro caso a leitura seja realizada e falso caso contrário.</returns>
        public bool ReadNext()
        {
            if (this.ended)
            {
                return false;
            }
            else
            {
                if (!this.started)
                {
                    this.started = true;
                }

                this.current = new GenBankModel();
                var sortedSet = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
                foreach (var kvp in this.mainFields)
                {
                    sortedSet.Add(kvp.Key);
                }

                var readedFields = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
                var field = DetectNextField(this.reader, this.mainFields);
                if (field == "END_FILE")
                {
                    this.ended = true;
                    return false;
                }
                else
                {
                    while (true)
                    {
                        if (field == "END_FILE")
                        {
                            this.ValidateReadedFields(readedFields);
                            this.ended = true;
                            return false;
                        }
                        else if (field == "END_SECTION")
                        {
                            this.ValidateReadedFields(readedFields);
                            return true;
                        }
                        else
                        {
                            var readedField = field;
                            field = this.mainFields[field].Read(this.current);
                            readedFields.Add(readedField);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Efectua a leitura da próxima palavra do leitor.
        /// </summary>
        /// <param name="reader">O leitor de informação.</param>
        /// <returns>A palavra.</returns>
        internal static string ReadWord(TextReader reader)
        {
            var peeked = reader.Peek();
            if (peeked == -1)
            {
                return null;
            }
            else
            {
                var resultBuilder = new StringBuilder();
                var readed = (char)peeked;
                if (char.IsLetterOrDigit(readed))
                {
                    resultBuilder.Append(readed);
                    reader.Read();
                    peeked = reader.Peek();
                    while (peeked != -1)
                    {
                        readed = (char)peeked;
                        if (char.IsLetterOrDigit(readed))
                        {
                            resultBuilder.Append(readed);
                            reader.Read();
                            peeked = reader.Peek();
                        }
                        else
                        {
                            peeked = -1;
                        }
                    }
                }

                return resultBuilder.ToString();
            }
        }

        /// <summary>
        /// Efectua a leitura de dígitos.
        /// </summary>
        /// <param name="reader">O leitor de informação.</param>
        /// <returns>A leitura dos dígitos.</returns>
        internal static string ReadDigits(TextReader reader)
        {
            var peeked = reader.Peek();
            if (peeked == -1)
            {
                return null;
            }
            else
            {
                var resultBuilder = new StringBuilder();
                var readed = (char)peeked;
                if (char.IsDigit(readed))
                {
                    resultBuilder.Append(readed);
                    reader.Read();
                    peeked = reader.Peek();
                    while (peeked != -1)
                    {
                        readed = (char)peeked;
                        if (char.IsDigit(readed))
                        {
                            resultBuilder.Append(readed);
                            reader.Read();
                            peeked = reader.Peek();
                        }
                        else
                        {
                            peeked = -1;
                        }
                    }
                }

                return resultBuilder.ToString();
            }
        }

        /// <summary>
        /// Efectua a leitura das cadeias de carácteres que obedeçam à
        /// restrição definida pela expressão lambda.
        /// </summary>
        /// <param name="reader">O leitor de informação.</param>
        /// <param name="lambda">A expressão lambda.</param>
        /// <returns>A cadeia de carácteres lida.</returns>
        internal static string ReadGeneral(
            TextReader reader,
            Func<char, bool> lambda)
        {
            var peeked = reader.Peek();
            if (peeked == -1)
            {
                return null;
            }
            else
            {
                var resultBuilder = new StringBuilder();
                var readed = (char)peeked;
                if (lambda.Invoke(readed))
                {
                    resultBuilder.Append(readed);
                    reader.Read();
                    peeked = reader.Peek();
                    while (peeked != -1)
                    {
                        readed = (char)peeked;
                        if (lambda.Invoke(readed))
                        {
                            resultBuilder.Append(readed);
                            reader.Read();
                            peeked = reader.Peek();
                        }
                        else
                        {
                            peeked = -1;
                        }
                    }
                }

                return resultBuilder.ToString();
            }
        }

        /// <summary>
        /// Obtém a data a partir de uma cadeia de carácteres.
        /// </summary>
        /// <param name="date">A cadeia de carácteres com a data.</param>
        /// <param name="pattern">O padrão da data.</param>
        /// <returns>O valor da data.</returns>
        internal static DateTime ParseDate(
            string date,
            string pattern = "dd-MMM-yyyy")
        {
            return DateTime.ParseExact(
                    date,
                    "dd-MMM-yyyy",
                    cultureProvider);
        }

        /// <summary>
        /// Efectua a leitura dos espaços.
        /// </summary>
        /// <param name="reader">O leitor de informação.</param>
        /// <returns>
        /// Verdadeiro caso a leitura seja possível e falso caso o leitor da informação
        /// se encontre no fim.
        /// </returns>
        internal static bool ReadWhiteSpaces(TextReader reader)
        {
            var peeked = reader.Peek();
            if (peeked == -1)
            {
                return false;
            }
            else
            {
                var readed = (char)peeked;
                if (readed == ' ')
                {
                    reader.Read();

                    peeked = reader.Peek();
                    while (peeked != -1)
                    {
                        readed = (char)peeked;
                        if (readed == ' ')
                        {
                            reader.Read();
                            peeked = reader.Peek();
                        }
                        else
                        {
                            peeked = -1;
                        }
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Detecta o primeiro campo no registo.
        /// </summary>
        /// <param name="reader">O leitor de informação.</param>
        /// <param name="mainFields">O conjunto dos campos principais.</param>
        /// <returns>O nome do campo.</returns>
        internal static string DetectNextField(
            TextReader reader,
            Dictionary<string, AFieldReader<GenBankModel>> mainFields)
        {
            var state = 0;
            while (true)
            {
                var peeked = reader.Peek();
                if (peeked == -1)
                {
                    return "END_FILE";
                }
                else
                {
                    var readed = (char)peeked;
                    if (state == 0) // Início da linha.
                    {
                        if (char.IsWhiteSpace(readed))
                        {
                            reader.Read();
                            peeked = reader.Peek();
                        }
                        else if (readed == '/')
                        {
                            var word = ReadGeneral(reader, c => c == '/');
                            if (word == "//")
                            {
                                return "END_SECTION";
                            }
                            else
                            {
                                state = 1;
                            }
                        }
                        else
                        {
                            var word = ReadWord(reader);
                            if (mainFields.ContainsKey(word))
                            {
                                return word;
                            }
                            else
                            {
                                state = 1;
                            }
                        }
                    }
                    else
                    {
                        if (readed == '\n')
                        {
                            state = 0;
                        }

                        reader.Read();
                        peeked = reader.Peek();
                    }
                }
            }
        }

        /// <summary>
        /// Estabelece os leitores principais.
        /// </summary>
        private void SetupMainFields()
        {
            var comparer = StringComparer.InvariantCultureIgnoreCase;
            this.mainFields = new Dictionary<string, AFieldReader<GenBankModel>>(
                StringComparer.InvariantCultureIgnoreCase);
            this.mainFields.Add(
                "LOCUS",
                new LocusReader(this.reader, this.mainFields, null));
            this.mainFields.Add(
                "DEFINITION",
                new GenericFieldReader<GenBankModel>(
                    this.reader,
                    this.mainFields,
                    null,
                    (m, t) => m.Descritpion = t));
            this.mainFields.Add(
                "ACCESSION",
                new GenericFieldReader<GenBankModel>(
                    this.reader,
                    this.mainFields,
                    null,
                    (m, t) => m.Accession = t));
            this.mainFields.Add(
                "VERSION",
                new VersionReader(this.reader, this.mainFields, null));
            this.mainFields.Add(
                "KEYWORDS",
                new GenericFieldReader<GenBankModel>(
                    this.reader,
                    this.mainFields,
                    null,
                    (m, t) => m.Keywords = t));
            this.mainFields.Add(
                    "SOURCE",
                    new SourceReader(this.reader, this.mainFields, null));
            this.mainFields.Add(
                "REFERENCE",
                new ReferenceReader(this.reader, this.mainFields, null));
            this.mainFields.Add(
                "FEATURES",
                new FeaturesReader(this.reader, this.mainFields, null));
            this.mainFields.Add(
                "ORIGIN",
                new OriginReader(this.reader, this.mainFields, null));
        }

        /// <summary>
        /// Verifica se todos os campos forma lidos.
        /// </summary>
        /// <param name="readedFields">Os campos lidos.</param>
        private void ValidateReadedFields(
            SortedSet<string> readedFields)
        {
            foreach (var kvp in this.mainFields)
            {
                if (!readedFields.Contains(kvp.Key))
                {
                    throw new UtilitiesException(string.Format(
                        "Field {0} was not readed.",
                        kvp.Key));
                }
            }
        }
    }

    /// <summary>
    /// Define a interface para os gerenciadore de campos.
    /// </summary>
    internal interface IFiledManager
    {
        /// <summary>
        /// Retorna um valor que indica se o texto é um campo.
        /// </summary>
        /// <param name="word">O texto.</param>
        /// <returns>Verdadeiro caso o texto seja um campo e falso caso contrário.</returns>
        bool IsField(string word);
    }

    /// <summary>
    /// Define um leitor interno genérico.
    /// </summary>
    /// <typeparam name="T">O tipo de objecto que constitui o modelo.</typeparam>
    internal abstract class AFieldReader<T> : IFiledManager
    {
        /// <summary>
        /// O gerenciador do qual o objecto corrente depende.
        /// </summary>
        protected IFiledManager parent;

        /// <summary>
        /// O leitor de informação.
        /// </summary>
        protected TextReader reader;

        /// <summary>
        /// Os campos principais.
        /// </summary>
        protected Dictionary<string, AFieldReader<T>> mainFields;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="AFieldReader{T}"/>.
        /// </summary>
        /// <param name="reader">O leitor de texto.</param>
        /// <param name="mainFields">O mapeador de leitores.</param>
        /// <param name="parent">O gerenciador do qual o objecto corrente depende.</param>
        public AFieldReader(
            TextReader reader,
            Dictionary<string, AFieldReader<T>> mainFields,
            IFiledManager parent)
        {
            this.reader = reader;
            this.mainFields = mainFields;
            this.parent = parent;
        }

        /// <summary>
        /// Efectua a leitura do campo.
        /// </summary>
        /// <param name="model">O modelo onde será guardada a informação.</param>
        /// <returns>O valor do próximo campo.</returns>
        public abstract string Read(T model);

        /// <summary>
        /// Retorna um valor que indica se o texto corresponde a um campo.
        /// </summary>
        /// <param name="word">O texto.</param>
        /// <returns>Verdadeiro caso o texto corresponda a um campo e falso caso contrário.</returns>
        public virtual bool IsField(string word)
        {
            if (this.parent == null)
            {
                return this.mainFields.ContainsKey(word);
            }
            else if (this.parent.IsField(word))
            {
                return true;
            }
            else
            {
                return this.mainFields.ContainsKey(word);
            }
        }
    }

    /// <summary>
    /// Efectua a leitura do locus.
    /// </summary>
    internal class LocusReader : AFieldReader<GenBankModel>
    {
        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LocusReader"/>.
        /// </summary>
        /// <param name="reader">O leitor de texto.</param>
        /// <param name="mainFields">O mapeador de leitores.</param>
        /// <param name="parent">O gerenciador do qual o objecto corrente depende.</param>
        public LocusReader(
            TextReader reader,
            Dictionary<string, AFieldReader<GenBankModel>> mainFields,
            IFiledManager parent)
            : base(reader, mainFields, parent)
        {
        }

        /// <summary>
        /// Efectua a leitura do campo.
        /// </summary>
        /// <param name="model">O modelo onde será guardada a informação.</param>
        /// <returns>O valor do próximo campo.</returns>
        public override string Read(GenBankModel model)
        {
            model.Locus = new Locus();
            var state = 0;
            var buffer = string.Empty;
            while (state != -1)
            {
                var peeked = this.reader.Peek();
                if (peeked == -1)
                {
                    if (state == 0)
                    {
                        throw new UtilitiesException("Found no info in locus.");
                    }
                    else
                    {
                        return "END_FILE";
                    }
                }
                else
                {
                    var readed = (char)peeked;
                    if (readed == ' ')
                    {
                        this.reader.Read();
                        peeked = this.reader.Peek();
                    }
                    else if (readed == '\r')
                    {
                        if (state == 0)
                        {
                            throw new UtilitiesException("Found no info in locus.");
                        }
                        else
                        {
                            this.reader.Read();
                            this.reader.Read();
                            state = -1;
                        }
                    }
                    else if (readed == '\n')
                    {
                        if (state == 0)
                        {
                            throw new UtilitiesException("Found no info in locus.");
                        }
                        else
                        {
                            this.reader.Read();
                            state = -1;
                        }
                    }
                    else
                    {
                        if (state == 0) // Ler nome
                        {
                            var name = GenBankReader.ReadWord(this.reader);
                            model.Locus.LocusName = name;
                            peeked = this.reader.Peek();
                            state = 1;
                        }
                        else if (state == 1) // Ler a sequência ou seguintes
                        {
                            if (char.IsDigit(readed))
                            {
                                buffer = GenBankReader.ReadDigits(this.reader);
                                state = 2;
                            }
                            else
                            {
                                state = 3;
                            }
                        }
                        else if (state == 2) // Averiguar se se trata da sequência ou da data
                        {
                            if (readed == '-')
                            {
                                buffer += readed;
                                state = 6;
                            }
                            else
                            {
                                var word = GenBankReader.ReadWord(this.reader);
                                buffer += " " + word;
                                model.Locus.SequenceLength = buffer;
                                buffer = string.Empty;
                                state = 3;
                            }
                        }
                        else if (state == 3) // Ler tipo de molécula ou seguintes
                        {
                            var word = GenBankReader.ReadWord(this.reader);
                            if (this.IsGenBankDivisionValue(word))
                            {
                                model.Locus.GenBankDivision = this.GetGenBankDivisionValue(word);
                                state = 6;
                            }
                            else
                            {
                                model.Locus.MoleculeType = word;
                                state = 4;
                            }
                        }
                        else if (state == 4) // Ler alinhamento ou seguintes
                        {
                            var word = GenBankReader.ReadWord(this.reader);
                            if (this.IsGenBankDivisionValue(word))
                            {
                                model.Locus.GenBankDivision = this.GetGenBankDivisionValue(word);
                                state = 6;
                            }
                            else
                            {
                                model.Locus.Alignement = this.GetAlignmentValue(word);
                                state = 5;
                            }
                        }
                        else if (state == 5) // Ler divisão do GenBank ou seguintes
                        {
                            var word = GenBankReader.ReadWord(this.reader);
                            if (this.IsGenBankDivisionValue(word))
                            {
                                model.Locus.GenBankDivision = this.GetGenBankDivisionValue(
                                    word);
                                state = 6;
                            }
                            else
                            {
                                buffer = word;
                            }
                        }
                        else if (state == 6) // Ler data.
                        {
                            buffer += GenBankReader.ReadGeneral(
                                this.reader,
                                c => char.IsLetterOrDigit(c) || c == '-');
                            model.Locus.ModificationDate = GenBankReader.ParseDate(
                                buffer);
                        }
                    }
                }
            }

            return GenBankReader.DetectNextField(this.reader, this.mainFields);
        }

        /// <summary>
        /// Determina se o valor é um alinhamento.
        /// </summary>
        /// <param name="value">O valor a ser verificado.</param>
        /// <returns>
        /// Verdadeiro caso o valor seja um alinhamento e falso caso contrário.
        /// </returns>
        private bool IsAlignmengValue(string value)
        {
            return Enum.GetNames(typeof(EGenBankDivision)).Contains(
                value.Trim(),
                StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Obtém o valor do alinhamento a partir da descrição.
        /// </summary>
        /// <param name="value">A descrição.</param>
        /// <returns>O valor do alinhamento.</returns>
        private EAlignment GetAlignmentValue(string value)
        {
            var innerValue = value.Trim().ToUpper();
            switch (innerValue)
            {
                case "LINEAR": return EAlignment.LINEAR;
                case "CIRCULAR": return EAlignment.CIRCULAR;
                default: return EAlignment.NOT_DEF;
            }
        }

        /// <summary>
        /// Verifica se se trata de um valor da divisão do GenBank.
        /// </summary>
        /// <param name="value">O valor a ser verificado.</param>
        /// <returns>Verdadeiro se o valor é da divisão e falso caso contrário.</returns>
        private bool IsGenBankDivisionValue(string value)
        {
            return Enum.GetNames(typeof(EGenBankDivision)).Contains(
                value.Trim(),
                StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Obtém o enumerável a partir da descrição.
        /// </summary>
        /// <param name="value">A descrição.</param>
        /// <returns>O enumerável.</returns>
        private EGenBankDivision GetGenBankDivisionValue(string value)
        {
            var innerValue = value.Trim().ToUpper();
            switch (innerValue)
            {

                case "PRI": return EGenBankDivision.PRI;
                case "PROD": return EGenBankDivision.PROD;
                case "MAM": return EGenBankDivision.MAM;
                case "VRT": return EGenBankDivision.VRT;
                case "INV": return EGenBankDivision.INV;
                case "PLN": return EGenBankDivision.PLN;
                case "BCT": return EGenBankDivision.BCT;
                case "VRL": return EGenBankDivision.VRL;
                case "PHG": return EGenBankDivision.PHG;
                case "SYN": return EGenBankDivision.SYN;
                case "UNA": return EGenBankDivision.UNA;
                case "EST": return EGenBankDivision.EST;
                case "PAT": return EGenBankDivision.PAT;
                case "STS": return EGenBankDivision.STS;
                case "GSS": return EGenBankDivision.GSS;
                case "HTG": return EGenBankDivision.HTG;
                case "HTC": return EGenBankDivision.HTC;
                case "ENV": return EGenBankDivision.ENV;
                default: return EGenBankDivision.NOT_DEF;
            }
        }
    }

    /// <summary>
    /// Efectua a leitura da definição.
    /// </summary>
    /// <typeparam name="T">O tipo do objecto que constitui o modelo.</typeparam>
    internal class GenericFieldReader<T> : AFieldReader<T>
    {
        /// <summary>
        /// A função que permite actualizar o modelo.
        /// </summary>
        private Action<T, string> updateAction;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GenericFieldReader{T}"/>.
        /// </summary>
        /// <param name="reader">O leitor de texto.</param>
        /// <param name="mainFields">O mapeador de leitores.</param>
        /// <param name="parent">O gerenciador do qual o objecto corrente depende.</param>
        /// <param name="updateAction">A acção que permite actualizar o modelo.</param>
        public GenericFieldReader(
            TextReader reader,
            Dictionary<string, AFieldReader<T>> mainFields,
            IFiledManager parent,
            Action<T, string> updateAction)
            : base(reader, mainFields, parent)
        {
            this.updateAction = updateAction;
        }

        /// <summary>
        /// Efectua a leitura do campo.
        /// </summary>
        /// <param name="model">O modelo onde será guardada a informação.</param>
        /// <returns>O valor do próximo campo.</returns>
        public override string Read(T model)
        {
            var state = 0;
            var peeked = this.reader.Peek();
            var notFirstLine = false;
            var fieldBuilder = new StringBuilder();
            while (true)
            {
                if (peeked == -1)
                {
                    this.updateAction.Invoke(model, fieldBuilder.ToString());
                    return "END_FILE";
                }
                else
                {
                    var readed = (char)peeked;
                    if (state == 0)
                    {
                        if (readed == ' ')
                        {
                            GenBankReader.ReadWhiteSpaces(this.reader);
                            peeked = this.reader.Peek();
                        }
                        else if (readed == '\r')
                        {
                            this.reader.Read();
                            peeked = this.reader.Peek();
                        }
                        else if (readed == '\n')
                        {
                            this.reader.Read();
                            peeked = this.reader.Peek();
                        }
                        else if (readed == '/')
                        {
                            var word = GenBankReader.ReadGeneral(
                                this.reader,
                                c => c == '/');
                            if (word == "//")
                            {
                                this.updateAction.Invoke(model, fieldBuilder.ToString());
                                return "END_SECTION";
                            }
                            else
                            {
                                if (notFirstLine)
                                {
                                    fieldBuilder.AppendLine();
                                }
                                else
                                {
                                    notFirstLine = true;
                                }

                                fieldBuilder.Append(word);
                                peeked = this.reader.Peek();
                                state = 1;
                            }
                        }
                        else
                        {
                            if (char.IsLetterOrDigit(readed))
                            {
                                var word = GenBankReader.ReadWord(this.reader);
                                if (this.IsField(word))
                                {
                                    this.updateAction.Invoke(model, fieldBuilder.ToString());
                                    return word;
                                }
                                else
                                {
                                    if (notFirstLine)
                                    {
                                        fieldBuilder.AppendLine();
                                    }
                                    else
                                    {
                                        notFirstLine = true;
                                    }

                                    fieldBuilder.Append(word);
                                    peeked = this.reader.Peek();
                                    state = 1;
                                }
                            }
                            else
                            {
                                if (notFirstLine)
                                {
                                    fieldBuilder.AppendLine();
                                }
                                else
                                {
                                    notFirstLine = true;
                                }

                                fieldBuilder.Append(readed);
                                this.reader.Read();
                                peeked = this.reader.Peek();
                                state = 1;
                            }
                        }
                    }
                    else
                    {
                        if (readed == '\r')
                        {
                            this.reader.Read();
                            peeked = this.reader.Peek();
                        }
                        else if (readed == '\n')
                        {
                            this.reader.Read();
                            peeked = this.reader.Peek();
                            state = 0;
                        }
                        else
                        {
                            fieldBuilder.Append(readed);
                            this.reader.Read();
                            peeked = this.reader.Peek();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Efectua a leitura da versão.
    /// </summary>
    internal class VersionReader : AFieldReader<GenBankModel>
    {
        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="VersionReader"/>.
        /// </summary>
        /// <param name="reader">O leitor de texto.</param>
        /// <param name="mainFields">O mapeador de leitores.</param>
        /// <param name="parent">O gerenciador do qual o objecto corrente depende.</param>
        public VersionReader(
            TextReader reader,
            Dictionary<string, AFieldReader<GenBankModel>> mainFields,
            IFiledManager parent)
            : base(reader, mainFields, parent)
        {
        }

        /// <summary>
        /// Efectua a leitura do campo.
        /// </summary>
        /// <param name="model">O modelo onde será guardada a informação.</param>
        /// <returns>O valor do próximo campo.</returns>
        public override string Read(GenBankModel model)
        {
            model.Version = new VersionDesecription();
            var state = 0;
            var builder = new StringBuilder();
            while (state != -1)
            {
                var peeked = this.reader.Peek();
                if (peeked == -1)
                {
                    if (state == 0)
                    {
                        throw new UtilitiesException("Found no info in version.");
                    }
                    else if (state == 1)
                    {
                        model.Version.Version = builder.ToString();
                        return "END_FILE";
                    }
                    else
                    {
                        return "END_FILE";
                    }
                }
                else
                {
                    var readed = (char)peeked;
                    var word = string.Empty;
                    if (state == 0)
                    {
                        if (readed == ' ')
                        {
                            this.reader.Read();
                            peeked = this.reader.Peek();
                        }
                        else if (readed == '\r')
                        {
                            this.reader.Read();
                            this.reader.Read();
                            state = -1;
                        }
                        else if (readed == '\n')
                        {
                            this.reader.Read();
                            throw new UtilitiesException("Found no info in version.");
                        }
                        else
                        {
                            word = GenBankReader.ReadGeneral(
                                this.reader,
                                c => char.IsLetterOrDigit(c) || c == '.');
                            if (word.ToUpper() == "GI")
                            {
                                state = 2;
                            }
                            else
                            {
                                builder.Append(word);
                                state = 1;
                            }
                        }
                    }
                    else if (state == 1)
                    {
                        if (readed == ' ')
                        {
                            this.reader.Read();
                            peeked = this.reader.Peek();
                        }
                        else if (readed == '\r')
                        {
                            model.Version.Version = builder.ToString();
                            this.reader.Read();
                            this.reader.Read();
                            state = -1;
                        }
                        else if (readed == '\n')
                        {
                            model.Version.Version = builder.ToString();
                            this.reader.Read();
                            state = -1;
                        }
                        else
                        {
                            word = GenBankReader.ReadWord(this.reader);
                            if (word.ToUpper() == "GI")
                            {
                                state = 2;
                            }
                            else
                            {
                                builder.Append(" " + word);
                            }

                            peeked = this.reader.Peek();
                        }
                    }
                    else if (state == 2)
                    {
                        if (readed == ' ')
                        {
                            this.reader.Read();
                            peeked = this.reader.Peek();
                        }
                        else if (readed == '\r')
                        {
                            builder.Append(" " + word);
                            model.Version.Version = builder.ToString();
                            this.reader.Read();
                            this.reader.Read();
                            state = -1;
                        }
                        else if (readed == '\n')
                        {
                            builder.Append(" " + word);
                            model.Version.Version = builder.ToString();
                            this.reader.Read();
                            state = -1;
                        }
                        else if (readed == ':')
                        {
                            model.Version.Version = builder.ToString();
                            builder.Clear();
                            this.reader.Read();
                            peeked = this.reader.Peek();
                            state = 3;
                        }
                    }
                    else
                    {
                        word = GenBankReader.ReadGeneral(
                            this.reader,
                            c => c != '\r' && c != '\n');
                        model.Version.GenInfoIdentifier = word;
                        state = -1;
                    }
                }
            }

            return GenBankReader.DetectNextField(
                this.reader,
                this.mainFields);
        }
    }

    /// <summary>
    /// Efectua a leitura da fonte.
    /// </summary>
    internal class SourceReader : AFieldReader<GenBankModel>
    {
        /// <summary>
        /// O leitor do campo.
        /// </summary>
        protected AFieldReader<SourceDescription> fieldReader;

        /// <summary>
        /// O mapeamento dos leitore dos sub-campos.
        /// </summary>
        protected Dictionary<string, AFieldReader<SourceDescription>> subFields;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SourceReader"/>.
        /// </summary>
        /// <param name="reader">O leitor de texto.</param>
        /// <param name="mainFields">O mapeador de leitores.</param>
        /// <param name="parent">O gerenciador do qual o objecto corrente depende.</param>
        public SourceReader(
            TextReader reader,
            Dictionary<string, AFieldReader<GenBankModel>> mainFields,
            IFiledManager parent)
            : base(reader, mainFields, parent)
        {
            this.subFields = new Dictionary<string, AFieldReader<SourceDescription>>(
                StringComparer.InvariantCultureIgnoreCase);
            this.fieldReader = new GenericFieldReader<SourceDescription>(
                reader,
                this.subFields,
                this,
                (src, str) => src.Source = str);

            var organismReader = new GenericFieldReader<SourceDescription>(
                    reader,
                    this.subFields,
                    this,
                    (src, str) => src.Organism = str);

            this.subFields.Add(
                "ORGANISM",
                organismReader);
        }

        /// <summary>
        /// Efectua a leitura do campo.
        /// </summary>
        /// <param name="model">O modelo onde será guardada a informação.</param>
        /// <returns>O valor do próximo campo.</returns>
        public override string Read(GenBankModel model)
        {
            model.Source = new SourceDescription();
            var field = this.fieldReader.Read(model.Source);
            while (true)
            {
                if (field == "END_FILE")
                {
                    return field;
                }
                else if (field == "END_SECTION")
                {
                    return field;
                }
                else
                {
                    var genReader = default(AFieldReader<SourceDescription>);
                    if (this.subFields.TryGetValue(
                        field,
                        out genReader))
                    {
                        field = genReader.Read(model.Source);
                    }
                    else if (this.mainFields.ContainsKey(field))
                    {
                        return field;
                    }
                    else
                    {
                        throw new UtilitiesException(string.Format(
                            "Unexpected field: {0}",
                            field));
                    }
                }
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o texto é um campo.
        /// </summary>
        /// <param name="word">O texto.</param>
        /// <returns>Verdadeiro caso o texto seja um campo e falso caso contrário.</returns>
        public override bool IsField(string word)
        {
            if (base.IsField(word))
            {
                return true;
            }
            else
            {
                return this.subFields.ContainsKey(word);
            }
        }
    }

    /// <summary>
    /// Efectua a leitura da referência.
    /// </summary>
    internal class ReferenceReader : AFieldReader<GenBankModel>
    {
        /// <summary>
        /// O mapeamento dos leitore dos sub-campos.
        /// </summary>
        protected Dictionary<string, AFieldReader<ReferenceDescription>> subFields;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ReferenceReader"/>.
        /// </summary>
        /// <param name="reader">O leitor de texto.</param>
        /// <param name="mainFields">O mapeador de leitores.</param>
        /// <param name="parent">O gerenciador do qual o objecto corrente depende.</param>
        public ReferenceReader(
            TextReader reader,
            Dictionary<string, AFieldReader<GenBankModel>> mainFields,
            IFiledManager parent)
            : base(reader, mainFields, parent)
        {
            this.subFields = new Dictionary<string, AFieldReader<ReferenceDescription>>(
                   StringComparer.InvariantCultureIgnoreCase);
            this.subFields.Add(
                "AUTHORS",
                new GenericFieldReader<ReferenceDescription>(
                    this.reader,
                    this.subFields,
                    this,
                    (rf, str) => rf.Authors = str));
            this.subFields.Add(
                "TITLE",
                new GenericFieldReader<ReferenceDescription>(
                    this.reader,
                    this.subFields,
                    this,
                    (rf, str) => rf.Title = str));
            this.subFields.Add(
                "JOURNAL",
                new GenericFieldReader<ReferenceDescription>(
                    this.reader,
                    this.subFields,
                    this,
                    (rf, str) => rf.Journal = str));
            this.subFields.Add(
                "PUBMED",
                new GenericFieldReader<ReferenceDescription>(
                    this.reader,
                    this.subFields,
                    this,
                    (rf, str) => rf.PubmedId = str));
        }

        /// <summary>
        /// Efectua a leitura do campo.
        /// </summary>
        /// <param name="model">O modelo onde será guardada a informação.</param>
        /// <returns>O valor do próximo campo.</returns>
        public override string Read(GenBankModel model)
        {
            var reference = new ReferenceDescription();
            var field = this.ReadReferenceLine(reference);
            while (true)
            {
                if (field == "END_FILE")
                {
                    model.References.Add(reference);
                    return field;
                }
                else if (field == "END_SECTION")
                {
                    model.References.Add(reference);
                    return field;
                }
                else
                {
                    var genReader = default(AFieldReader<ReferenceDescription>);
                    if (this.subFields.TryGetValue(
                        field,
                        out genReader))
                    {
                        field = genReader.Read(reference);
                    }
                    else if (this.mainFields.ContainsKey(field))
                    {
                        model.References.Add(reference);
                        return field;
                    }
                    else
                    {
                        throw new UtilitiesException(string.Format(
                            "Unexpected field: {0}",
                            field));
                    }
                }
            }
        }

        private string ReadReferenceLine(ReferenceDescription reference)
        {
            var state = 0;
            while (true)
            {
                var peeked = this.reader.Peek();
                if (peeked == -1)
                {
                    return "END_FILE";
                }
                else
                {
                    var readed = (char)peeked;
                    if (readed == '\r')
                    {
                        this.reader.Read();
                        this.reader.Read();
                        peeked = this.reader.Peek();
                        state = 7;
                    }
                    else if (readed == '\n')
                    {
                        this.reader.Read();
                        peeked = this.reader.Peek();
                        state = 7;
                    }
                    else if (readed == ' ')
                    {
                        this.reader.Read();
                        peeked = this.reader.Peek();
                    }
                    else
                    {
                        if (state == 0) // Leitura do ordinal da referência
                        {
                            if (char.IsDigit(readed))
                            {
                                var digits = GenBankReader.ReadDigits(this.reader);
                                reference.Ordinal = int.Parse(digits);
                                peeked = this.reader.Peek();
                            }

                            state = 1;
                        }
                        else if (state == 1) // Leitura do parêntesis de abertura
                        {
                            if (readed == '(')
                            {
                                this.reader.Read();
                                peeked = this.reader.Peek();
                            }

                            state = 2;
                        }
                        else if (state == 2) // Leitura da palavra "bases"
                        {
                            if (char.IsLetter(readed))
                            {
                                var word = GenBankReader.ReadWord(this.reader);
                                if (word == "bases")
                                {
                                    state = 3;
                                }
                            }
                            else if (char.IsDigit(readed))
                            {
                                var word = GenBankReader.ReadDigits(this.reader);
                                reference.StartBase = int.Parse(word);
                                state = 4;
                            }

                            peeked = this.reader.Peek();
                        }
                        else if (state == 3) // Leitura da primeira base
                        {
                            if (char.IsDigit(readed))
                            {
                                var word = GenBankReader.ReadDigits(this.reader);
                                reference.StartBase = int.Parse(word);
                                state = 4;
                            }
                            else if (char.IsLetter(readed))
                            {
                                var word = GenBankReader.ReadWord(this.reader);
                                if (word == "to")
                                {
                                    state = 5;
                                }

                                peeked = this.reader.Peek();
                            }
                        }
                        else if (state == 4) // Leitura da palavra "to"
                        {
                            if (char.IsLetter(readed))
                            {
                                var word = GenBankReader.ReadWord(this.reader);
                                if (word == "to")
                                {
                                    state = 5;
                                }
                            }
                            else if (char.IsDigit(readed))
                            {
                                var word = GenBankReader.ReadDigits(this.reader);
                                reference.EndBase = int.Parse(word);
                                state = 6;
                            }

                            peeked = this.reader.Peek();
                        }
                        else if (state == 5) // Leigura da segunda base
                        {
                            if (char.IsDigit(readed))
                            {
                                var word = GenBankReader.ReadDigits(this.reader);
                                reference.EndBase = int.Parse(word);
                            }

                            state = 6;
                            peeked = this.reader.Read();
                        }
                        else if (state == 6) // Leitura do parêntesis de fecho e fim de linha
                        {
                            this.reader.Read();
                            peeked = this.reader.Peek();
                        }
                        else if (state == 7) // Leitura do próximo campo
                        {
                            if (readed == '/')
                            {
                                var word = GenBankReader.ReadGeneral(
                                    this.reader,
                                    c => c == '/');
                                if (word == "//")
                                {
                                    return "END_SECTION";
                                }
                                else
                                {
                                    state = 6;
                                }
                            }
                            else
                            {
                                var word = GenBankReader.ReadWord(this.reader);
                                if (this.IsField(word))
                                {
                                    return word;
                                }
                                else
                                {
                                    peeked = this.reader.Peek();
                                    state = 6;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o texto é um campo.
        /// </summary>
        /// <param name="word">O texto.</param>
        /// <returns>Verdadeiro caso o texto seja um campo e falso caso contrário.</returns>
        public override bool IsField(string word)
        {
            if (base.IsField(word))
            {
                return true;
            }
            else
            {
                return this.subFields.ContainsKey(word);
            }
        }
    }

    /// <summary>
    /// Efectua a leitura das características.
    /// </summary>
    internal class FeaturesReader : AFieldReader<GenBankModel>
    {
        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="FeaturesReader"/>.
        /// </summary>
        /// <param name="reader">O leitor de texto.</param>
        /// <param name="mainFields">O mapeador de leitores.</param>
        /// <param name="parent">O gerenciador do qual o objecto corrente depende.</param>
        public FeaturesReader(
            TextReader reader,
            Dictionary<string, AFieldReader<GenBankModel>> mainFields,
            IFiledManager parent)
            : base(reader, mainFields, parent)
        {
        }

        /// <summary>
        /// Efectua a leitura do campo.
        /// </summary>
        /// <param name="model">O modelo onde será guardada a informação.</param>
        /// <returns>O valor do próximo campo.</returns>
        public override string Read(GenBankModel model)
        {
            this.ReadFeaturesDescritpion(model);
            model.Features.Clear();

            var currentFeaturesAttribute = "unknown";
            var attributeFeatures = new AttributedFeatures();

            var notFirst = false;
            var state = 0;
            while (true)
            {
                var peeked = this.reader.Peek();
                if (state == 0)
                {
                    // Primeira leitura do nome da característica, do nome do campo ou do nome do atributo
                    if (peeked == -1)
                    {
                        return "END_FILE";
                    }
                    else
                    {
                        var readed = (char)peeked;
                        if (readed == ' '
                            || readed == '\r'
                            || readed == '\n')
                        {
                            this.reader.Read();
                            peeked = this.reader.Peek();
                        }
                        else if (readed == '/')
                        {
                            this.reader.Read();
                            peeked = this.reader.Peek();
                            if (peeked == -1)
                            {
                                if (notFirst)
                                {
                                    model.Features.Add(Tuple.Create(
                                           currentFeaturesAttribute,
                                           attributeFeatures));
                                }

                                return "END_FILE";
                            }
                            else
                            {
                                readed = (char)peeked;
                                if (readed == '/')
                                {
                                    this.reader.Read();
                                    if (notFirst)
                                    {
                                        model.Features.Add(Tuple.Create(
                                               currentFeaturesAttribute,
                                               attributeFeatures));
                                    }

                                    return "END_SECTION";
                                }
                                else
                                {
                                    peeked = this.reader.Peek();
                                    notFirst = true;
                                    state = 1;
                                }
                            }
                        }
                        else
                        {
                            var word = GenBankReader.ReadGeneral(
                                this.reader,
                                c => c != ' ' && c != '\r' && c != '\n');

                            if (this.IsField(word) && word != "source")
                            {
                                if (notFirst)
                                {
                                    model.Features.Add(Tuple.Create(
                                           currentFeaturesAttribute,
                                           attributeFeatures));
                                }

                                return word;
                            }
                            else
                            {
                                if (notFirst)
                                {
                                    model.Features.Add(Tuple.Create(
                                        currentFeaturesAttribute,
                                        attributeFeatures));
                                    currentFeaturesAttribute = "unknown";
                                    attributeFeatures = new AttributedFeatures();
                                }

                                GenBankReader.ReadWhiteSpaces(this.reader);
                                currentFeaturesAttribute = word;
                                attributeFeatures.Attribute = this.ReadAttributeValue();
                                notFirst = true;
                            }
                        }
                    }
                }
                else if (state == 1) // Leitura do atributo
                {
                    if (peeked == -1)
                    {
                        model.Features.Add(Tuple.Create(
                            currentFeaturesAttribute,
                            attributeFeatures));
                        return "END_FILE";
                    }
                    else
                    {
                        var readead = (char)peeked;
                        if (readead == ' ' || readead == '\r')
                        {
                            this.reader.Read();
                            peeked = this.reader.Peek();
                        }
                        else if (readead == '\n')
                        {
                            this.reader.Read();
                            peeked = this.reader.Peek();
                            state = 0;
                        }
                        else if (readead == '=')
                        {
                            var value = this.ReadAttributeValue();
                            attributeFeatures.Attributes.Add(string.Empty, value);
                            peeked = this.reader.Peek();
                            state = 0;
                        }
                        else
                        {
                            var attribute = GenBankReader.ReadGeneral(
                                this.reader,
                                c=>c!= '\r' && c!= '\n' && c!= '=');
                            peeked = this.reader.Peek();
                            if (peeked == -1)
                            {
                                model.Features.Add(Tuple.Create(
                                    currentFeaturesAttribute,
                                    attributeFeatures));
                                return "END_FILE";
                            }
                            else if (readead == '\r' || readead == '\n')
                            {
                                attributeFeatures.Attributes.Add(
                                    attribute,
                                    string.Empty);
                                state = 0;
                            }
                            else
                            {
                                // Leitura do sinal
                                this.reader.Read();
                                var value = this.ReadAttributeValue();
                                attributeFeatures.Attributes.Add(
                                    attribute,
                                    value);
                                state = 0;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Efectua a leitura do valor do atributo.
        /// </summary>
        /// <returns>O valor do atributo lido.</returns>
        private string ReadAttributeValue()
        {
            var builder = new StringBuilder();
            var peeked = this.reader.Peek();
            if (peeked != -1)
            {
                var readed = (char)peeked;
                if (readed == '"')
                {
                    this.reader.Read();
                    while (true)
                    {
                        peeked = this.reader.Peek();
                        if (peeked == -1)
                        {
                            return builder.ToString();
                        }
                        else
                        {
                            readed = (char)peeked;
                            if (readed == '"')
                            {
                                this.reader.Read();
                                return builder.ToString();
                            }
                            else if (readed == '\n')
                            {
                                this.reader.Read();
                                builder.Append(readed);
                                GenBankReader.ReadWhiteSpaces(this.reader);
                            }
                            else
                            {
                                builder.Append(readed);
                                this.reader.Read();
                            }
                        }
                    }
                }
                else
                {
                    return GenBankReader.ReadGeneral(
                        this.reader,
                        c => c != '\r' && c != '\n');
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Efectua a leitura da descrição das características.
        /// </summary>
        /// <param name="model">O modelo.</param>
        private void ReadFeaturesDescritpion(GenBankModel model)
        {
            while (true)
            {
                var peeked = this.reader.Peek();
                if (peeked == -1)
                {
                    return;
                }
                else
                {
                    var readed = (char)peeked;
                    if (readed == ' ')
                    {
                        this.reader.Read();
                        peeked = this.reader.Peek();
                    }
                    else
                    {
                        model.FeaturesDescription = GenBankReader.ReadGeneral(
                            this.reader,
                            c => c != '\r' && c != '\n');
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Efectua a leitura da origem.
    /// </summary>
    internal class OriginReader : AFieldReader<GenBankModel>
    {
        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="OriginReader"/>.
        /// </summary>
        /// <param name="reader">O leitor de texto.</param>
        /// <param name="mainFields">O mapeador de leitores.</param>
        /// <param name="parent">O gerenciador do qual o objecto corrente depende.</param>
        public OriginReader(
            TextReader reader,
            Dictionary<string, AFieldReader<GenBankModel>> mainFields,
            IFiledManager parent)
            : base(reader, mainFields, parent)
        {
        }

        /// <summary>
        /// Efectua a leitura do campo.
        /// </summary>
        /// <param name="model">O modelo onde será guardada a informação.</param>
        /// <returns>O valor do próximo campo.</returns>
        public override string Read(GenBankModel model)
        {
            var builder = new StringBuilder();
            while (true)
            {
                var peeked = this.reader.Peek();
                if (peeked == -1)
                {
                    model.Origin = builder.ToString();
                    return "END_FILE";
                }
                else
                {
                    var readed = (char)peeked;
                    if (readed == '\r' || readed == ' ')
                    {
                        this.reader.Read();
                        peeked = this.reader.Peek();
                    }
                    else if (readed == '\n')
                    {
                        this.reader.Read();
                        GenBankReader.ReadWhiteSpaces(this.reader);
                        peeked = this.reader.Peek();
                        if (peeked == -1)
                        {
                            model.Origin = builder.ToString();
                            return "END_FILE";
                        }
                        else
                        {
                            readed = (char)peeked;
                            if (readed == '/')
                            {
                                this.reader.Read();
                                peeked = this.reader.Peek();
                                if (peeked == -1)
                                {
                                    throw new UtilitiesException("Unexpected end of section after origin at end of file.");
                                }
                                else
                                {
                                    readed = (char)peeked;
                                    if (readed == '/')
                                    {
                                        this.reader.Read();
                                        model.Origin = builder.ToString();
                                        return "END_SECTION";
                                    }
                                    else
                                    {
                                        throw new UtilitiesException(string.Format(
                                            "Unexpected end of section: '{0}'.",
                                            readed));
                                    }
                                }
                            }
                            else if (char.IsDigit(readed))
                            {
                                GenBankReader.ReadDigits(this.reader);
                                peeked = this.reader.Peek();
                            }
                            else
                            {
                                builder.Append(GenBankReader.ReadWord(this.reader));
                                peeked = this.reader.Peek();
                            }
                        }
                    }
                    else if (char.IsDigit(readed))
                    {
                        GenBankReader.ReadDigits(this.reader);
                        peeked = this.reader.Peek();
                    }
                    else
                    {
                        builder.Append(GenBankReader.ReadWord(this.reader));
                        peeked = this.reader.Peek();
                    }
                }
            }
        }
    }
}
