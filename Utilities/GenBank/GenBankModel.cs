namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A divisão GenBank ao qual o gene pertence.
    /// </summary>
    public enum EGenBankDivision
    {
        /// <summary>
        /// Sequências de primatas.
        /// </summary>
        PRI = 1,

        /// <summary>
        /// Sequências de roedores.
        /// </summary>
        PROD = 2,

        /// <summary>
        /// Sequências de outros mamíferos.
        /// </summary>
        MAM = 3,

        /// <summary>
        /// Sequências de outros vertegrados.
        /// </summary>
        VRT = 4,

        /// <summary>
        /// Sequências de invertegrados.
        /// </summary>
        INV = 5,

        /// <summary>
        /// Sequências de plantas, fungos e algas.
        /// </summary>
        PLN = 6,

        /// <summary>
        /// Sequências de bactérias.
        /// </summary>
        BCT = 7,

        /// <summary>
        /// Sequências de vírus.
        /// </summary>
        VRL = 8,

        /// <summary>
        /// Sequências de bacteriófagos (vírus que infectam apenas bactérias).
        /// </summary>
        PHG = 9,

        /// <summary>
        /// Sequências sintéticas.
        /// </summary>
        SYN = 10,

        /// <summary>
        /// Sequências não anotadas.
        /// </summary>
        UNA = 11,

        /// <summary>
        /// Marcador de sequências genética (sub-sequência curta de ADN complementar).
        /// </summary>
        EST = 12,

        /// <summary>
        /// Sequências patenteadas.
        /// </summary>
        PAT = 13,

        /// <summary>
        /// Sequências de marcadores locais (relacionados com reacções de polimerase em cadeia).
        /// </summary>
        STS = 14,

        /// <summary>
        /// Sequência de genomas para pesquisa.
        /// </summary>
        GSS = 15,

        /// <summary>
        /// Sequências de genomas de alta taxa de transferência.
        /// </summary>
        HTG = 16,

        /// <summary>
        /// Sequências de ADN complementar não terminado de alta taxa de transferência.
        /// </summary>
        HTC = 17,

        /// <summary>
        /// Sequências de amostragem ambiente.
        /// </summary>
        ENV = 18
    }

    /// <summary>
    /// Possibilidades para a classificação do campo CDS.
    /// </summary>
    public enum ECdsFeatureClass
    {
        /// <summary>
        /// Completas.
        /// </summary>
        COMPLETE = 1,

        /// <summary>
        /// Parcial na terminação 5'.
        /// </summary>
        PARTIAL_5 = 2,

        /// <summary>
        /// Parcial na terminação 3'.
        /// </summary>
        PARTIAL_3 = 2
    }

    /// <summary>
    /// Contém uma série de elementos de dados associados a cada registo.
    /// </summary>
    public class Locus
    {
        /// <summary>
        /// O nome do locus é único entre registos e, por norma, constitui um resumo dos
        /// outros campos.
        /// </summary>
        private string locusName;

        /// <summary>
        /// Número de pares de bases de nucleótidos.
        /// </summary>
        private string sequenceLength;

        /// <summary>
        /// O tipo da molécula que foi sequenciada.
        /// </summary>
        private string moleculeType;

        /// <summary>
        /// A divisão GenBank à qual o registo pertence.
        /// </summary>
        private EGenBankDivision genBankDivision;

        /// <summary>
        /// A data de modificação.
        /// </summary>
        private DateTime modificationDate;

        /// <summary>
        /// Obtém ou atribui o nome do locus é único entre registos e, por norma, constitui um resumo dos
        /// outros campos.
        /// </summary>
        public string LocusName
        {
            get
            {
                return this.locusName;
            }
            set
            {
                this.locusName = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o número de pares de bases de nucleótidos.
        /// </summary>
        public string SequenceLength
        {
            get
            {
                return this.sequenceLength;
            }
            set
            {
                this.sequenceLength = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o tipo da molécula que foi sequenciada.
        /// </summary>
        public string MoleculeType
        {
            get
            {
                return this.moleculeType;
            }
            set
            {
                this.moleculeType = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a divisão GenBank à qual o registo pertence.
        /// </summary>
        public EGenBankDivision GenBankDivision
        {
            get
            {
                return this.genBankDivision;
            }
            set
            {
                this.genBankDivision = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a data de modificação.
        /// </summary>
        public DateTime ModificationDate
        {
            get
            {
                return this.modificationDate;
            }
            set
            {
                this.modificationDate = value;
            }
        }
    }

    /// <summary>
    /// Mantém a informação do número de identificação de sequência na base-de-dados
    /// do GenBank.
    /// </summary>
    public class VersionDesecription
    {
        /// <summary>
        /// Número de identificação.
        /// </summary>
        private string version;

        /// <summary>
        /// Número para a sequência nucleótida que é atribuído sempre que se verifique
        /// algumas alteração.
        /// </summary>
        private string genInfoIdentifier;

        /// <summary>
        /// Obtém ou atribui o número de identificação.
        /// </summary>
        public string Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o número para a sequência nucleótida que é atribuído sempre que se verifique
        /// algumas alteração.
        /// </summary>
        public string GenInfoIdentifier
        {
            get
            {
                return this.genInfoIdentifier;
            }
            set
            {
                this.genInfoIdentifier = value;
            }
        }
    }

    /// <summary>
    /// Mantém a informação da fonte, incluindo uma forma abreviada do nome do
    /// organismo.
    /// </summary>
    public class SourceDescription
    {
        /// <summary>
        /// A informação da fonte.
        /// </summary>
        private string source;

        /// <summary>
        /// O nome científico formal do organismo.
        /// </summary>
        private string organism;

        /// <summary>
        /// Obtém ou atribui a informação da fonte.
        /// </summary>
        public string Source
        {
            get
            {
                return this.source;
            }
            set
            {
                this.source = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o nome científico formal do organismo.
        /// </summary>
        public string Organism
        {
            get
            {
                return this.organism;
            }
            set
            {
                this.organism = value;
            }
        }
    }

    /// <summary>
    /// Mantém a informação das publicações onde são reportados os dados
    /// do registo.
    /// </summary>
    public class ReferenceDescription
    {
        /// <summary>
        /// O número ordinal da referência.
        /// </summary>
        private int ordinal;

        /// <summary>
        /// O número da base inicial.
        /// </summary>
        private int startBase;

        /// <summary>
        /// O número da base final.
        /// </summary>
        private int endBase;

        /// <summary>
        /// A lista de autores.
        /// </summary>
        private List<string> authors;

        /// <summary>
        /// O título da publicação.
        /// </summary>
        private string title;

        /// <summary>
        /// O jornal de publicação.
        /// </summary>
        private string journal;

        /// <summary>
        /// O identificador no PubMed (http://www.ncbi.nlm.nih.gov/pubmed).
        /// </summary>
        private string pubmedId;

        /// <summary>
        /// Informação de contacto do submissor.
        /// </summary>
        private string directSubmission;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ReferenceDescription"/>.
        /// </summary>
        public ReferenceDescription()
        {
            this.authors = new List<string>();
        }

        /// <summary>
        /// Obtém ou atribui o número ordinal da referência.
        /// </summary>
        public int Ordinal
        {
            get
            {
                return this.ordinal;
            }
            set
            {
                this.ordinal = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o número da base inicial.
        /// </summary>
        public int StartBase
        {
            get
            {
                return this.startBase;
            }
            set
            {
                this.startBase = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o número da base final.
        /// </summary>
        public int EndBase
        {
            get
            {
                return this.endBase;
            }
            set
            {
                this.endBase = value;
            }
        }

        /// <summary>
        /// Obtém a lista de autores.
        /// </summary>
        public List<string> Authors
        {
            get
            {
                return this.authors;
            }
        }

        /// <summary>
        /// Obtém ou atribui o título da publicação.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o jornal de publicação.
        /// </summary>
        public string Journal
        {
            get
            {
                return this.journal;
            }
            set
            {
                this.journal = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o identificador no PubMed (http://www.ncbi.nlm.nih.gov/pubmed).
        /// </summary>
        public string PubmedId
        {
            get
            {
                return this.pubmedId;
            }
            set
            {
                this.pubmedId = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a informação de contacto do submissor.
        /// </summary>
        public string DirectSubmission
        {
            get
            {
                return this.directSubmission;
            }
            set
            {
                this.directSubmission = value;
            }
        }
    }

    /// <summary>
    /// Classe base dos objectos que mantêm uma mapeamento de atributos.
    /// </summary>
    public class AttributedFeatures
    {
        /// <summary>
        /// O mapeamento de atributos e valores que caracterizam a fonte.
        /// </summary>
        protected Dictionary<string, string> attributes;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="AttributedFeatures"/>.
        /// </summary>
        public AttributedFeatures()
        {
            this.attributes = new Dictionary<string, string>(
               StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Obtém o mapeamento de atributos e valores que caracterizam a fonte.
        /// </summary>
        public Dictionary<string, string> Attributes
        {
            get
            {
                return this.attributes;
            }
        }
    }

    /// <summary>
    /// Tipo de objectos do GenBank que são caracterizados por um conjunto
    /// de pares propriedade/valor.
    /// </summary>
    public class AttributedBaseDescFeatures
    {
        /// <summary>
        /// O número da base inicial.
        /// </summary>
        protected int startBase;

        /// <summary>
        /// O número da base final.
        /// </summary>
        protected int endBase;

        /// <summary>
        /// Obtém ou atribui número da base inicial.
        /// </summary>
        public int StartBase
        {
            get
            {
                return this.startBase;
            }
            set
            {
                this.startBase = value;
            }
        }

        /// <summary>
        /// Obém ou atribui o número da base final.
        /// </summary>
        public int EndBase
        {
            get
            {
                return this.endBase;
            }
            set
            {
                this.endBase = value;
            }
        }
    }

    /// <summary>
    /// Define a codificação da sequência.
    /// </summary>
    public class FeaturesCds : AttributedBaseDescFeatures
    {
        /// <summary>
        /// O tipo de codificação.
        /// </summary>
        protected ECdsFeatureClass featureClass;

        /// <summary>
        /// Obtém o tipo da codificação.
        /// </summary>
        public ECdsFeatureClass FeatureClass
        {
            get
            {
                return this.featureClass;
            }
            set
            {
                this.featureClass = value;
            }
        }
    }
}
