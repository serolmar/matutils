// -----------------------------------------------------------------------
// <copyright file="ConfigurationSection.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Reprsentação da secção runtime do ficheiro de configuração.
    /// </summary>
    public class RuntimeConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Obtém ou atribui a configuração que especifica se a identidade Windows flui através
        /// de pontos de sincronização.
        /// </summary>
        [ConfigurationProperty("alwaysFlowImpersonationPolicy")]
        public AlwaysFlowsImpersonationPolicy AlwaysFlowsImpersonationPolicy
        {
            get
            {
                return (AlwaysFlowsImpersonationPolicy)this["alwaysFlowImpersonationPolicy"];
            }
            set
            {
                this["alwaysFlowImpersonationPolicy"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica a "Assembly" que provê o gerente
        /// do domínio aplicacional para o domínio aplicacional por defeito no processo.
        /// </summary>
        [ConfigurationProperty("appDomainManagerAssembly")]
        public AppDomainManagerAssembly AppDomainManagerAssembly
        {
            get
            {
                return (AppDomainManagerAssembly)this["appDomainManagerAssembly"];
            }
            set
            {
                this["appDomainManagerAssembly"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica o tipo que serve como gerente de
        /// domínio aplicacional para o domínio aplicacional por defeito.
        /// </summary>
        [ConfigurationProperty("appDomainManagerType")]
        public AppDomainManagerType AppDomainManagerType
        {
            get
            {
                return (AppDomainManagerType)this["appDomainManagerType"];
            }
            set
            {
                this["appDomainManagerType"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que instrui o motor para colectar estatísticas de todos
        /// os domínios aplicacionais no processo durante todo o seu ciclo de vida.
        /// </summary>
        [ConfigurationProperty("appDomainResourceMonitoring")]
        public AppDomainResourceMonitoring AppDomainResourceMonitoring
        {
            get
            {
                return (AppDomainResourceMonitoring)this["appDomainResourceMonitoring"];
            }
            set
            {
                this["appDomainResourceMonitoring"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que contêm informação sobre a versão de redireccinamento
        /// de "Assemblies" e as localizações das "Assemblies".
        /// </summary>
        [ConfigurationProperty("assemblyBinding")]
        public AssemblyBinding AssemblyBinding
        {
            get
            {
                return (AssemblyBinding)this["assemblyBinding"];
            }
            set
            {
                this["assemblyBinding"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração se os nomes fortes para "Assembies" devem ser ignorados.
        /// </summary>
        [ConfigurationProperty("bypassTrustedAppStrongNames")]
        public BypassTrustedAppStrongNames BypassTrustedAppStrongNames
        {
            get
            {
                return (BypassTrustedAppStrongNames)this["bypassTrustedAppStrongNames"];
            }
            set
            {
                this["bypassTrustedAppStrongNames"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se o motor deverá utilizar ordenações de
        /// cadeias de carácteres de legado.
        /// </summary>
        [ConfigurationProperty("CompatSortNLSVersion")]
        public CompatSortNLSVersion CompatSortNLSVersion
        {
            get
            {
                return (CompatSortNLSVersion)this["CompatSortNLSVersion"];
            }
            set
            {
                this["CompatSortNLSVersion"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se o motor procura por "Assemblies" nas directorias
        /// especificadas pela variável de ambiente DEVPATH.
        /// </summary>
        [ConfigurationProperty("developmentMode")]
        public DevelopmentMode DevelopmentMode
        {
            get
            {
                return (DevelopmentMode)this["developmentMode"];
            }
            set
            {
                this["developmentMode"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se a provisão de falhas de ligação, que
        /// consiste numa configuração por defeito do .NET 2.0, é desabilitado.
        /// </summary>
        [ConfigurationProperty("disableCachingBindingFailures")]
        public DisableCachingBindingFailures DisableCachingBindingFailures
        {
            get
            {
                return (DisableCachingBindingFailures)this["disableCachingBindingFailures"];
            }
            set
            {
                this["disableCachingBindingFailures"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se a pilha de linhas de fluxo é
        /// despachada ao início da linha de fluxo.
        /// </summary>
        [ConfigurationProperty("disableCommitThreadStack")]
        public DisableCommitThreadStack DisableCommitThreadStack
        {
            get
            {
                return (DisableCommitThreadStack)this["disableCommitThreadStack"];
            }
            set
            {
                this["disableCommitThreadStack"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se o comportamento por defeito, consistindo
        /// na permissão da sobrecarga das afinações das configurações para um domínio aplicacional,
        /// se econtra desabilitado.
        /// </summary>
        [ConfigurationProperty("disableFusionUpdatesFromADManager")]
        public DisableFusionUpdatesFromADManager DisableFusionUpdatesFromADManager
        {
            get
            {
                return (DisableFusionUpdatesFromADManager)this["disableFusionUpdatesFromADManager"];
            }
            set
            {
                this["disableFusionUpdatesFromADManager"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se se força o requisito de configuração
        /// de computador de que os algoritmos criptográficos se devem coadunar com o FIPS
        /// (Federal Information Processing Standards).
        /// </summary>
        [ConfigurationProperty("enforceFIPSPolicy")]
        public EnforceFIPSPolicy EnforceFIPSPolicy
        {
            get
            {
                return (EnforceFIPSPolicy)this["enforceFIPSPolicy"];
            }
            set
            {
                this["enforceFIPSPolicy"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se se permite o traçamento de eventos
        /// para Windows para os eventos do CLR (Common Language Runtime).
        /// </summary>
        [ConfigurationProperty("etwEnable")]
        public EtwEnable EtwEnable
        {
            get
            {
                return (EtwEnable)this["etwEnable"];
            }
            set
            {
                this["etwEnable"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se a PerfCounter.dll utiliza a opção
        /// CategoryOptions do registo numa aplicação do ambiente .Net, versão 1.1, para determinar
        /// se se carregam dados dos contadores de performance a partir da memória partilhada ao global
        /// específica à categoria.
        /// </summary>
        [ConfigurationProperty("forcePerformanceCounterUniqueSharedMemoryReads")]
        public ForcePerformanceCounterUniqueSharedMemoryReads ForcePerformanceCounterUniqueSharedMemoryReads
        {
            get
            {
                return (ForcePerformanceCounterUniqueSharedMemoryReads)this["forcePerformanceCounterUniqueSharedMemoryReads"];
            }
            set
            {
                this["forcePerformanceCounterUniqueSharedMemoryReads"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que possilita a criação de vectores com mais de 2 GB de memória
        /// em ambientes de 64 bits.
        /// </summary>
        [ConfigurationProperty("gcAllowVeryLargeObjects")]
        public GcAllowVeryLargeObjects GcAllowVeryLargeObjects
        {
            get
            {
                return (GcAllowVeryLargeObjects)this["gcAllowVeryLargeObjects"];
            }
            set
            {
                this["gcAllowVeryLargeObjects"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração se o motor executa a colecta de lixo de forma concorrente.
        /// </summary>
        [ConfigurationProperty("gcConcurrent")]
        public GcConcurrent GcConcurrent
        {
            get
            {
                return (GcConcurrent)this["gcConcurrent"];
            }
            set
            {
                this["gcConcurrent"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração se a colecta de lixo suporta múltiplos grupos de CPU.
        /// </summary>
        [ConfigurationProperty("GCCpuGroup")]
        public GCCpuGroup GCCpuGroup
        {
            get
            {
                return (GCCpuGroup)this["GCCpuGroup"];
            }
            set
            {
                this["GCCpuGroup"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração se motor executa a colecta de lixo de servidor.
        /// </summary>
        [ConfigurationProperty("gcServer")]
        public GcServer GcServer
        {
            get
            {
                return (GcServer)this["gcServer"];
            }
            set
            {
                this["gcServer"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se o motor utiliza a política de publicação
        /// CAS (Code Access Security).
        /// </summary>
        [ConfigurationProperty("generatePublisherEvidence")]
        public GeneratePublisherEvidence GeneratePublisherEvidence
        {
            get
            {
                return (GeneratePublisherEvidence)this["generatePublisherEvidence"];
            }
            set
            {
                this["generatePublisherEvidence"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se o motor permite ao código gerido, o
        /// tratamento de violações de acesso ou outras excepções de corrupção de estado.
        /// </summary>
        [ConfigurationProperty("legacyCorruptedStateExceptionsPolicy")]
        public LegacyCorruptedStateExceptionsPolicy LegacyCorruptedStateExceptionsPolicy
        {
            get
            {
                return (LegacyCorruptedStateExceptionsPolicy)this["legacyCorruptedStateExceptionsPolicy"];
            }
            set
            {
                this["legacyCorruptedStateExceptionsPolicy"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica que a identidade Windows não flui através
        /// de pontos de sincronização, independentemente das configurações do fluxo para o contexto
        /// de execução da linha de fluxo corrente.
        /// </summary>
        [ConfigurationProperty("legacyImpersonationPolicy")]
        public LegacyImpersonationPolicy LegacyImpersonationPolicy
        {
            get
            {
                return (LegacyImpersonationPolicy)this["legacyImpersonationPolicy"];
            }
            set
            {
                this["legacyImpersonationPolicy"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se as "Assemblies" carregadas de fontes
        /// remotas são consideradas como sendo confiáveis.
        /// </summary>
        [ConfigurationProperty("loadfromRemoteSources")]
        public LoadFromRemoteSources LoadFromRemoteSources
        {
            get
            {
                return (LoadFromRemoteSources)this["loadfromRemoteSources"];
            }
            set
            {
                this["loadfromRemoteSources"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se o motor utiliza código de acesso de
        /// segurança (CAS) de legado.
        /// </summary>
        [ConfigurationProperty("NetFx40_LegacySecurityPolicy")]
        public NetFx40_LegacySecurityPolicy NetFx40_LegacySecurityPolicy
        {
            get
            {
                return (NetFx40_LegacySecurityPolicy)this["NetFx40_LegacySecurityPolicy"];
            }
            set
            {
                this["NetFx40_LegacySecurityPolicy"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se o motor corrige automaticamente
        /// declarações de chamada da plataforma em tempo de execução, incorrendo em transacções
        /// mais lentas entre código gerido e não gerido.
        /// </summary>
        [ConfigurationProperty("NetFx40_PInvokeStackResilience")]
        public NetFx40_PInvokeStackResilience NetFx40_PInvokeStackResilience
        {
            get
            {
                return (NetFx40_PInvokeStackResilience)this["NetFx40_PInvokeStackResilience"];
            }
            set
            {
                this["NetFx40_PInvokeStackResilience"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se o motor utiliza uma quantidade fixa
        /// de memória para calcular códigos confusos para cadeias de carácteres.
        /// </summary>
        [ConfigurationProperty("NetFx45_CultureAwareComparerGetHashCode_LongStrings")]
        public NetFx45_CultureAwareComparerGetHashCode_LongStrings NetFx45_CultureAwareComparerGetHashCode_LongStrings
        {
            get
            {
                return (NetFx45_CultureAwareComparerGetHashCode_LongStrings)this["NetFx45_CultureAwareComparerGetHashCode_LongStrings"];
            }
            set
            {
                this["NetFx45_CultureAwareComparerGetHashCode_LongStrings"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se o motor irá utilizar o Interop COM
        /// ao invés da comunicação remota entre as fronteiras dos domínios de aplicação.
        /// </summary>
        [ConfigurationProperty("PreferComInsteadOfRemoting")]
        public PreferComInsteadOfManagedRemoting PreferComInsteadOfManagedRemoting
        {
            get
            {
                return (PreferComInsteadOfManagedRemoting)this["PreferComInsteadOfRemoting"];
            }
            set
            {
                this["PreferComInsteadOfRemoting"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração de optimização do exame de "Assemblies" satélite.
        /// </summary>
        [ConfigurationProperty("relativeBindForResources")]
        public RelativeBindForResources RelativeBindForResources
        {
            get
            {
                return (RelativeBindForResources)this["relativeBindForResources"];
            }
            set
            {
                this["relativeBindForResources"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se as "Shadow Copies" recorrem
        /// à inicialização por defeito introduzida no .NET 4 ou reverte para o comportamento de
        /// inicialização de versões anteriores do motor .NET.
        /// </summary>
        [ConfigurationProperty("shadowCopyVerifyByTimeStamp")]
        public ShadowCopyVerifyByTimestamp ShadowCopyVerifyByTimestamp
        {
            get
            {
                return (ShadowCopyVerifyByTimestamp)this["shadowCopyVerifyByTimeStamp"];
            }
            set
            {
                this["shadowCopyVerifyByTimeStamp"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica que uma aplicação pode referencial a mesma
        /// "Assembly" em duas implementações diferentes do motor .NET, desabilitando o comportamento
        /// por defeito que trata as "Assemblies" como sendo equivalentes por motivos de portabilidade.
        /// </summary>
        [ConfigurationProperty("supportPortability")]
        public SupportPortability SupportPortability
        {
            get
            {
                return (SupportPortability)this["supportPortability"];
            }
            set
            {
                this["supportPortability"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se o motor distribui linhas de fluxo
        /// geridas por vários grupos de CPU.
        /// </summary>
        [ConfigurationProperty("Thread_UseAllCpuGroups")]
        public Thread_UseAllCpuGroups Thread_UseAllCpuGroups
        {
            get
            {
                return (Thread_UseAllCpuGroups)this["Thread_UseAllCpuGroups"];
            }
            set
            {
                this["Thread_UseAllCpuGroups"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se excepções de tarefas não tratadas
        /// devem terminar o processo em execução.
        /// </summary>
        [ConfigurationProperty("ThrowUnobservedTaskExceptions")]
        public ThrowUnobservedTaskExceptions ThrowUnobservedTaskExceptions
        {
            get
            {
                return (ThrowUnobservedTaskExceptions)this["ThrowUnobservedTaskExceptions"];
            }
            set
            {
                this["ThrowUnobservedTaskExceptions"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se o motor utiliza formatos de legado
        /// para marcações temporais.
        /// </summary>
        [ConfigurationProperty("TimeSpan_LegacyFormatMode")]
        public TimeSpan_LegacyFormatMode TimeSpan_LegacyFormatMode
        {
            get
            {
                return (TimeSpan_LegacyFormatMode)this["TimeSpan_LegacyFormatMode"];
            }
            set
            {
                this["TimeSpan_LegacyFormatMode"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que especifica se o motor calcula códigos confusos
        /// para cadeias de carácteres por aplicação.
        /// </summary>
        [ConfigurationProperty("UseRandomizedStringHashAlgorithm")]
        public UseRandomizedStringHashAlgorithm UseRandomizedStringHashAlgorithm
        {
            get
            {
                return (UseRandomizedStringHashAlgorithm)this["UseRandomizedStringHashAlgorithm"];
            }
            set
            {
                this["UseRandomizedStringHashAlgorithm"] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração que requer que o motor utilize tamanhos de pilhas
        /// explícitos quando cria determinadas linhas de fluxo que usa internamente, ao
        /// invés do tamanho da pilha por defeito.
        /// </summary>
        [ConfigurationProperty("UseSmallInternalThreadStacks")]
        public UseSmallInternalThreadStacks UseSmallInternalThreadStacks
        {
            get
            {
                return (UseSmallInternalThreadStacks)this["UseSmallInternalThreadStacks"];
            }
            set
            {
                this["UseSmallInternalThreadStacks"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica que a representação de um utilizador de Windows flui
    /// sempre através de pontos de sincronização.
    /// </summary>
    public class AlwaysFlowsImpersonationPolicy : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", DefaultValue = "false", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica o nome da "Assembly".
    /// </summary>
    public class AppDomainManagerAssembly : ConfigurationElement
    {
        /// <summary>
        /// Obém o nome da "Assembly".
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return (string)this["value"];
            }
            set
            {
                this["value"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica o tipo da "Assembly".
    /// </summary>
    public class AppDomainManagerType : ConfigurationElement
    {
        /// <summary>
        /// Otbém o tipo da "Assembly".
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return (string)this["value"];
            }
            set
            {
                this["value"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se os recursos se encontram a ser monitorizados.
    /// </summary>
    public class AppDomainResourceMonitoring : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", DefaultValue = "false", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Contém informação sobre o redireccionamento da "Assembly".
    /// </summary>
    public class AssemblyBinding : ConfigurationElement
    {
        /// <summary>
        /// Otbém o tipo da "Assembly".
        /// </summary>
        [ConfigurationProperty("xmlns", IsRequired = true)]
        public string Xmlns
        {
            get
            {
                return (string)this["xmlns"];
            }
            set
            {
                this["xmlns"] = value;
            }
        }

        /// <summary>
        /// Otbém a versão da "Assembly" .NET à qual o redireccionamento é aplicável.
        /// </summary>
        [ConfigurationProperty("appliesTo", IsRequired = true)]
        public string AppliesTo
        {
            get
            {
                return (string)this["appliesTo"];
            }
            set
            {
                this["appliesTo"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se se pretende ignorar a validação com base em nomes fortes.
    /// </summary>
    public class BypassTrustedAppStrongNames : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", DefaultValue = "false", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica o identificador da localidade.
    /// </summary>
    public class CompatSortNLSVersion : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", DefaultValue = "4096", IsRequired = true)]
        public string Enabled
        {
            get
            {
                return (string)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica o modo de desenvolvimento.
    /// </summary>
    public class DevelopmentMode : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("developerInstallation", DefaultValue = "true", IsRequired = true)]
        public bool DeveloperInstallation
        {
            get
            {
                return (bool)this["developerInstallation"];
            }
            set
            {
                this["developerInstallation"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se se realiza o aprovisonamento das falhas de ligação.
    /// </summary>
    public class DisableCachingBindingFailures : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public short Enabled
        {
            get
            {
                return (short)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se está disponível a funcionalidade de despacho
    /// ao início da linha de fluxo.
    /// </summary>
    public class DisableCommitThreadStack : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public short Enabled
        {
            get
            {
                return (short)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se a pilha de controlo de fluxo é despachada quando
    /// uma linha de fluxo é iniciada.
    /// </summary>
    public class DisableFusionUpdatesFromADManager : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public short Enabled
        {
            get
            {
                return (short)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se é forçado o requerimento de que o algoritmo de
    /// encriptação deve ser compatível com o padrão "Federal Information Processing Standards" (FPIS).
    /// </summary>
    public class EnforceFIPSPolicy : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se é para ser habilitado o traçamento de eventos.
    /// </summary>
    public class EtwEnable : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se PerfCounter.dll usa o registo CategoryOptions para determinar
    /// se são para ser carregados dados dos contadores de performance da memória específiva de categoria.
    /// </summary>
    public class ForcePerformanceCounterUniqueSharedMemoryReads : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se serão permitidas instâncias de objectos muito grandes.
    /// </summary>
    public class GcAllowVeryLargeObjects : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se a recolha do lixo se realiza de forma concorrente.
    /// </summary>
    public class GcConcurrent : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se a recolha do lixo suporta vários grupos de CPU.
    /// </summary>
    public class GCCpuGroup : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se o motor executa a colecção de lixo de servidor.
    /// </summary>
    public class GcServer : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se o motor publica a evidência do "Publisher"
    /// para o código de segurança de acesso (Code Access Security - CAS).
    /// </summary>
    public class GeneratePublisherEvidence : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se é permitido ao motor interseptar violações de acesso
    /// e outras excepções associadas a corrupções de estado.
    /// </summary>
    public class LegacyCorruptedStateExceptionsPolicy : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica que a identidade Windows não flui através de pontos de
    /// sincronização, independentemente das configurações de fluxo para o contexto de execução
    /// da linha de fluxo corrente.
    /// </summary>
    public class LegacyImpersonationPolicy : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se é atribuída completa confiança a "Assmblies" carregadas
    /// remotamente.
    /// </summary>
    public class LoadFromRemoteSources : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se é utilizado código de segurança de acesso legado.
    /// </summary>
    public class NetFx40_LegacySecurityPolicy : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se o motor corrige automaticamente declarações de chamada
    /// em tempo de execução, incorrendo em transições mais lentas entre código gerido e não gerido.
    /// </summary>
    public class NetFx40_PInvokeStackResilience : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public short Enabled
        {
            get
            {
                return (short)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se o motor utiliza uma quantidade fixa de memória para calcular
    /// os códigos confusos para a comparação de cadeias de texto.
    /// </summary>
    public class NetFx45_CultureAwareComparerGetHashCode_LongStrings : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public short Enabled
        {
            get
            {
                return (short)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se o motor irá dar prevalência a chamadas COM relativamente a
    /// chamadas remotas entre fronteiras de domínios de aplicação.
    /// </summary>
    public class PreferComInsteadOfManagedRemoting : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que indica para optimizar o exame de "Assemblies" satélite.
    /// </summary>
    public class RelativeBindForResources : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que indica para optimizar o exame de "Assemblies" satélite.
    /// </summary>
    public class ShadowCopyVerifyByTimestamp : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que indica que uma aplicação pode referenciar a mesma "Assembly"
    /// em duas implementações diferentes do ambiente .NET, sobrecarregando o comportamento
    /// definido por defeito.
    /// </summary>
    public class SupportPortability : ConfigurationElement
    {
        /// <summary>
        /// Especifica o "Public Key Token" da "Assembly" afectada.
        /// </summary>
        [ConfigurationProperty("PKT", IsRequired = true)]
        public string Pkt
        {
            get
            {
                return (string)this["PKT"];
            }
            set
            {
                this["PKT"] = value;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se o motor distribui linhas de fluxo geridas
    /// por todos os grupos de CPU.
    /// </summary>
    public class Thread_UseAllCpuGroups : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica que execpções de tarefas não manuseadas devem
    /// terminar um processo em execução.
    /// </summary>
    public class ThrowUnobservedTaskExceptions : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se o motor utiliza formatos de legado para
    /// marcações temporais.
    /// </summary>
    public class TimeSpan_LegacyFormatMode : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Configuração que especifica se o motor calcula códigos confusos para cadeias de carácteres
    /// por aplicação.
    /// </summary>
    public class UseRandomizedStringHashAlgorithm : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public short Enabled
        {
            get
            {
                return (short)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    /// <summary>
    /// Força a que o CLR ("Common Language Runtime") reduza memória, especificando
    /// explicitamente tamanhos de pilhas aquando da criação de linhas de fluxo a serem utilizadas internamente
    /// ao invés de utilizar a pilha por defeito para essas linhas de fluxo.
    /// </summary>
    public class UseSmallInternalThreadStacks : ConfigurationElement
    {
        /// <summary>
        /// Obtém um valor que indica se a configuração se encontra disponível.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }
}
