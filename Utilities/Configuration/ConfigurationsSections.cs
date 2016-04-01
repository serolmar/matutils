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
    public class ConfigurationsSections : ConfigurationSection
    {
    }

    /// <summary>
    /// Configuração que especifica que a representação de um utilizador pode 
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
        [ConfigurationProperty("value", IsRequired = true)]
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
}
