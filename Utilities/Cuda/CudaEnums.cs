namespace Utilities.Cuda
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Enumeração de todos os erros susceptíveis de ocorrer durante a chamada de funções Cuda.
    /// </summary>
    public enum ECudaError
    {
        /// <summary>
        /// A chamada à API retornou sem erros a consulta foi bem-sucedida.
        /// </summary>
        CudaSuccess = 0,

        /// <summary>
        /// Falha no dispositivo causada normalmente pela chamada à função <see cref="CudaApi.CudaLaunch"/>
        /// por este não ter sido configurado por intermédio da chamada à função 
        /// <see cref="CudaApi.CudaConfigCall"/>.
        /// </summary>
        CudaErrorMissingConfiguration = 1,

        /// <summary>
        /// A chamada à API falhou porque não foi possível alocar memória suficiente para satisfazer
        /// a operação.
        /// </summary>
        CudaErrorMemoryAllocation = 2,

        /// <summary>
        /// A API falhou porque o condutor de Cuda ou o sistema não puderam ser inicializados.
        /// </summary>
        CudaErrorInitializationError = 3,

        /// <summary>
        /// Ocorreu uma excepçáo no dispositivo durante a execução do kernel. O rol de causas comuns inclui
        /// a má referência de um apontador para regiões no exterior do limite da memória partilhada. O dispositivo
        /// não pode ser utilizado até que seja chamada a função <see cref="CudaApi.CudaThreadExit"/>. Todas
        /// as alocações de memória existentes no dispositivo são inválidas e estas terão de ser reconstituídas
        /// de modo a ser possível continuar a usufruir dos processos Cuda.
        /// </summary>
        CudaErrorLaunchFailure = 4,

        /// <summary>
        /// Indica que um processo kernel anterior falhou e era usado na emulação de lançamentos de kernel.
        /// </summary>
        [Obsolete("Deprecated as of Cuda 3.1 since device emulation was removed.")]
        CudaErrorPriorLaunchFailure = 5,

        /// <summary>
        /// Indica que o kernel do dispositivo demorou demasiado tempo a ser executado. Isto ocorre se os
        /// limites de execução se encontrarem activos - ver as propriedades do dispositivo com o auxílio da
        /// variável <see cref="CudaApi.KernelExecTimeoutEnabled"/> para informação adicional. O dispositivo
        /// não pode ser utilizado até que seja chamada a função <see cref="CudaApi.CudaThreadExit"/>. Todas
        /// as alocações de memória são inválidas e devem ser reconstruídas de modo a ser possível continuar a
        /// usufruir dos processos Cuda.
        /// </summary>
        CudaErrorLaunchTimeout = 6,

        /// <summary>
        /// Indica que o lançamento não ocorreu porque não existem recursos apropriados. Apesar deste erro ser
        /// semelhante ao erro <see cref="ECudaError.CudaErrorInvalidConfiguration"/>, indica geralmente que o
        /// utilizador tentou passar demasiados argumentos para o kernel do dispositivo ou o lançamento do kernel
        /// especifica demasiadas linhas de fluxo para o contador de registo do kernel.
        /// </summary>
        CudaErrorLaunchOutOfResources = 7,

        /// <summary>
        /// A função de dispositivo requerida não existe ou não se encontra adequadamente compilada para a
        /// arquitectura correcta.
        /// </summary>
        CudaErrorInvalidDeviceFunction = 8,

        /// <summary>
        /// O lançamento do kernel está a requerer recursos que nunca poderão ser satisfeitos pelo dispositivo
        /// actual. O pedido de mais memória partilhada por bloco do que aquele que o dispositivo suporta
        /// despoleta este erro, assim como o pedido de demasiadas linhas de fluxo ou blocos. Ver
        /// <see cref="CudaApi.CudaDeviceProp"/> para mais limitações do dispositivo.
        /// </summary>
        CudaErrorInvalidConfiguration = 9,

        /// <summary>
        /// O ordinal de dispositivo fornecido pelo utilizador não corresponde a um dispositivo Cuda.
        /// </summary>
        CudaErrorInvalidDevice = 10,

        /// <summary>
        /// Um ou mais parâmetros passados para a chamada da API não está dentro do alcance aceitável para os
        /// valores.
        /// </summary>
        CudaErrorInvalidValue = 11,

        /// <summary>
        /// Um ou mais parâmetros relacionados com o passo passados para a chamada da API não se encontram
        /// dentro dos limites aceitáveis para esse tipo de parâmetros.
        /// </summary>
        CudaErrorInvalidPitchValue = 12,

        /// <summary>
        /// Indica que o símbolo name/identifier passado para a chamada da API não é um nome ou indentificador
        /// válido.
        /// </summary>
        CudaErrorInvalidSymbol = 13,

        /// <summary>
        /// O objecto amortecedor não pode ser mapeado.
        /// </summary>
        CudaErrorMapBufferObjectFailed = 14,

        /// <summary>
        /// Não é possível eliminar o mapeamento do objecto amortecedor.
        /// </summary>
        CudaErrorUnmapBufferObjectFailed = 15,

        /// <summary>
        /// Indica que pelo menos um apontador de anfitrião passado para a chamada da API não é um apontador
        /// de anfitrião válido.
        /// </summary>
        CudaErrorInvalidHostPointer = 16,

        /// <summary>
        /// Indica que pelo menos um apontador de dispositivo passado para a chamada da API não é um apontador
        /// de dispositivo válido.
        /// </summary>
        CudaErrorInvalidDevicePointer = 17,

        /// <summary>
        /// Indica que a textura passsada para a chamada da API não é uma textura válida.
        /// </summary>
        CudaErrorInvalidTexture = 18,

        /// <summary>
        /// A ligação da textura não é válida. Ocorre quando é chamada a função 
        /// <see cref="CudaApi.CudaGetTextureAlignementOffset"/> com uma textura que não se encontra no
        /// intervalo das texturas.
        /// </summary>
        CudaErrorInvalidTextureBinding = 19,

        /// <summary>
        /// O descritor de canal passado para a chamada da API não é válido. Ocorre se o formato não for um
        /// dos formatos especificados por <see cref="CudaApi.CudaChannelFormatKind"/> ou se uma das dimensões
        /// for inválida.
        /// </summary>
        CudaErrorInvalidChannelDescriptor = 20,

        /// <summary>
        /// Indica que a direcção da cópida de memória passada para a chamada da API não é um dos tipos
        /// especificados por <see cref="CudaApi.CudaMemcpyKind"/>.
        /// </summary>
        CudaErrorInvalidMemcpyDirection = 21,

        /// <summary>
        /// Indica que o utilizador tomou o endereço de uma variável constante.
        /// </summary>
        [Obsolete("Used in versions prior to the Cuda 3.1 release.")]
        CudaErrorAddressOfConstant = 22,

        /// <summary>
        /// Indica que a obtenção da textura não pode ser efectuada, anteriormente usado na emulação de
        /// operações sobre texturas.
        /// </summary>
        [Obsolete("Used in versions prior to the Cuda 3.1 release.")]
        CudaErrorTextureFetchFailed = 23,

        /// <summary>
        /// Indica que a textura não foi conseguida para acesso, anteriormente usado na emulação de
        /// operações sobre texturas.
        /// </summary>
        [Obsolete("Used in versions prior to the Cuda 3.1 release.")]
        CudaErrorTextureNotBound = 24,

        /// <summary>
        /// Falha na operações de sincronização, anteriormente usado na emulação de operações sobre texturas.
        /// </summary>
        [Obsolete("Used in versions prior to the Cuda 3.1 release.")]
        CudaErrorSynchronizationError = 25,

        /// <summary>
        /// Indica que a textura non-float se encontrava a ser acedida por intermédio de linear filtering.
        /// Isto não é suportado por Cuda.
        /// </summary>
        CudaErrorInvalidFilterSetting = 26,

        /// <summary>
        /// Uma tentativa de leitura de uma textura non-float como sendo uma textura normalizada. Esta operação
        /// não é suportada por Cuda.
        /// </summary>
        CudaErrorInvalidNormSetting = 27,

        /// <summary>
        /// A mistura de código de dispositivo e código de emulação de dispositivo não é permitida.
        /// </summary>
        [Obsolete("Used in versions prior to the Cuda 3.1 release.")]
        CudaErrorMixedDeviceExecution = 28,

        /// <summary>
        /// Indica que uma chamada da API Cuda não pode ser executada porque está a ser chamada durante
        /// o encerramento do processo num ponto posterior ao descarregamento do condutor de Cuda.
        /// </summary>
        CudaErrorCudartUnloading = 29,

        /// <summary>
        /// Indicação de um erro interno desconhecido.
        /// </summary>
        CudaErrorUnknown = 30,

        /// <summary>
        /// Indica que a chamada da API ainda não se encontra implementada. Ambientes de produção de Cuda
        /// nunca deverão despoletar este erro.
        /// </summary>
        [Obsolete("Used in versions prior to the Cuda 4.1.")]
        CudaErrorNotYetImplemented = 31,

        /// <summary>
        /// Indica que um apontador de dispositivo emulado excedeu o limite de 32 bit.
        /// </summary>
        [Obsolete("Used in versions prior to the Cuda 3.1 release.")]
        CudaErrorMemoryValueTooLarge = 32,

        /// <summary>
        /// Indica que o manuseador do recurso passado para a chamada da API não é válido. Os manuseadores
        /// de recursos são tipos opacos tais como <see cref="CudaStream"/> e <see cref="CudaEvent"/>.
        /// </summary>
        CudaErrorInvalidResourceHandle = 33,

        /// <summary>
        /// Operações assíncronas anteriormente emitidas ainda não foram concluídas. Este resultado não constitui
        /// exactamente um erro mas deve ser distinto de <see cref="ECudaError.CudaSuccess"/> (que indica
        /// operação bem-sucedida). Chamadas que poderão retornar este valor são 
        /// <see cref="CudaApi.CudaEventQuery"/> e <see cref="CudaApi.CudaStreamQuery"/>.
        /// </summary>
        CudaErrorNotReady = 34,

        /// <summary>
        /// Indica que o condutor de Cuda instalado é mais antigo do que a livraria que se encontra activa.
        /// Trata-se de uma configuração que não é suportada. Os utilizadores deverão instalar uma actualização
        /// do condutor da NVIDIA de modo a permiter a execução da aplicação.
        /// </summary>
        CudaErrorInsufficientDriver = 35,

        /// <summary>
        /// Indica que o utilizador chamou <see cref="CudaApi.CudaSetValidDevices"/>, 
        /// <see cref="CudaApi.CudaSetDeviceFlats"/>, <see cref="CudaApi.CudaD3D9Direct3DDevice"/>,
        /// <see cref="CudaApi.CudaD3D10SetDirect3DDevice"/>, 
        /// <see cref="CudaApi.CudaD3D11SetDirect3DDevice"/> ou <see cref="CudaApi.CudaVDPAUSetVDPAUDevice"/>
        /// após inicializar o motor Cuda por intermédio da chamada de funções externas ao dispositivo de gestão
        /// (alocação de memória e lançamentos de kernel são exemplos de tais operações de gestão). Este erro
        /// também pode ser retornado durante a utilização da interoperabilidade de motor/condutor e existe um
        /// contexto <see cref="CudaContext"/> actvio na linha de fluxo do anfitrião.
        /// </summary>
        CudaErrorSetOnActiveProcess = 36,

        /// <summary>
        /// Indica que a surface passada para a chamada da API não é uma surface válida.
        /// </summary>
        CudaErrorInvalidSurface = 37,

        /// <summary>
        /// Indica que dispositivos sem suporte Cuda foram detectados pelo condutor de Cuda instalado.
        /// </summary>
        CudaErrorNoDevice = 38,

        /// <summary>
        /// Indica que um erro de ECC que não pode ser corrigido foi detectado.
        /// </summary>
        CudaErrorECCUncorrectable = 39,

        /// <summary>
        /// Indica que não foi possível resolver a ligação para um objecto partilhado.
        /// </summary>
        CudaErrorSharedObjectSymbolNotFound = 40,

        /// <summary>
        /// Falha na inicizaliação de um objecto partilhado.
        /// </summary>
        CudaErrorSharedObjectInitFailed = 41,

        /// <summary>
        /// Indica que o <see cref="CudaApi.CudaLimit"/> passado para a chamada da API não é suportado
        /// pelo dispositivo corrente.
        /// </summary>
        CudaErrorUnsupportedLimit = 42,

        /// <summary>
        /// Indica que múltiplas variáveis globais ou constantes (ao longo de ficheiros fonte de Cuda separados
        /// na aplicação) partilham o mesmo nome de string.
        /// </summary>
        CudaErrorDuplicateVariableName = 43,

        /// <summary>
        /// Indica que múltiplas texturas (ao longo de ficheiros fonte de Cuda separados
        /// na aplicação) partilham o mesmo nome de string.
        /// </summary>
        CudaErrorDuplicateTextureName = 44,

        /// <summary>
        /// Indica que múltiplas surfaces (ao longo de ficheiros fonte de Cuda separados
        /// na aplicação) partilham o mesmo nome de string.
        /// </summary>
        CudaErrorDuplicateSurfaceName = 45,

        /// <summary>
        /// Todos os dispositivos Cuda estão ocupados or indisponíveis. Estão habitualmente ocupados/indisponíveis
        /// devido ao uso de <see cref="CudaApi.CudaComputeModeExclusive"/>, de
        /// <see cref="CudaApi.CudaComputeModeProhibited"/> ou quando kernels em execução encem o GPU e
        /// se encontram a impedir o início de novas tarefas. Podem ainda encontra-se indisponíveis devido a
        /// restrições de memória no dispositivo que já possuem tarefas Cuda em execução.
        /// </summary>
        CudaErrorDevicesUnavailable = 46,

        /// <summary>
        /// Indica que a imagem do kernel do dispositivo é inválida.
        /// </summary>
        CudaErrorInvalidKernelImage = 47,

        /// <summary>
        /// Nenhuma imagem de kernel que se adeque ao dispositivo se encontra disponível. Isto pode ocorrer
        /// quando o utilizador especifica opções de geração de codigo para um ficheiro fonte Cuda particular que
        /// não inclui a configuração do dispositivo correspondente.
        /// </summary>
        CudaErrorNoKernelImageForDevice = 48,

        /// <summary>
        /// O contexto actual não é compatiível com a execução Cuda corrente. Isto pode ocorrer se se estiver a
        /// utilizar a interoperabilidade Execução/Condutor Cuda e se tiver criado um contexto de condutor
        /// existente utilizado a API de condutor. O contexto de condutor pode ser incompatível porque
        /// o contexto de condutor foi criado, porque o recurso a uma versão antiga da API como a chamada da API de 
        /// execução espera um contexto de condutor primário e o contexto de condutor não é primário ou porque
        /// o contexto de condutor foi dstruído.
        /// </summary>
        CudaErrorIncompatibleDriverContext = 49,

        /// <summary>
        /// A chamada à função <see cref="CudaApi.CudaDeviceEnablePeerAccess"/> está a tentar reestabelecer
        /// enedereçamento de pares a partir de um conteto que já possui esse tipo de endereçamento activo.
        /// </summary>
        CudaErrorPeerAccessAlreadyEnabled = 50,

        /// <summary>
        /// A função <see cref="CudaApi.CudaDeviceDisablePeerAccess"/> está a tentar desabilitar o endereçamento
        /// de pares que ainda não foi estabelecido via <see cref="CudaApi.CudaDeviceEnablePeerAccess"/>.
        /// </summary>
        CudaErrorPeerAccessNotEnabled = 51,

        /// <summary>
        /// Uma chanada tentou aceder a um dispositivo exclusivo a linha de fluxo que está a ser utilizado
        /// por uma linha de fluxo diferente.
        /// </summary>
        CudaErrorDeviceAlreadyInUse = 54,

        /// <summary>
        /// Indica que o monitor de desempenho não foi inicializado para a execução. Pode acontecer quanod a
        /// aplicação se encontra em execução com o auxílio de ferramentas de análise de desempenho externas tais
        /// como o Visual Profiler.
        /// </summary>
        CudaErrorProfilerDisabled = 55,

        /// <summary>
        /// Já não é considerado erro a tentativa de habiligar/desabilitar o monitor de desempenho via
        /// <see cref="CudaApi.CudaProfileStart"/> ou <see cref="CudaApi.CudaProfileStop"/> sem a
        /// inicialização.
        /// </summary>
        [Obsolete("Deprecated as of Cuda 5.0.")]
        CudaErrorProfilerNotInitialized = 56,

        /// <summary>
        /// Já não é considerado erro chamar <see cref="CudaApi.CudaProfileStart"/> quando o monitor de
        /// desempenho se encontra habilitado.
        /// </summary>
        [Obsolete("Deprecated as of Cuda 5.0.")]
        CudaErrorProfilerAlreadyStarted = 57,

        /// <summary>
        /// Já não é considerado erro chamar <see cref="CudaApi.CudaProfilerStop"/> quando o monitor de
        /// desempenho se encontra desabilitado.
        /// </summary>
        [Obsolete("Deprecated as of Cuda 5.0.")]
        CudaErrorProfilerAlreadyStopped = 58,

        /// <summary>
        /// Uma asseveração foi despoletada no código de dispositivo durante a execução do kernel. O dispositivo
        /// não pode ser utilizado novamente até que seja chamada a função <see cref="CudaApi.CudaThreadExit"/>.
        /// Todas as alocações existentes são inválidas e devem ser reconstruídas se se pretender continuar a
        /// recorrer a Cuda no programa.
        /// </summary>
        CudaErrorAssert = 59,

        /// <summary>
        /// Este erro indica que os recursos de hardware requeridos para habilitar o acesso de pares foi
        /// exaurido para um ou mais dos dispositivos passados para <see cref="CudaApi.CudaEnablePeerAccess"/>.
        /// </summary>
        CudaErrorTooManyPeers = 60,

        /// <summary>
        /// Este erro indica que o limite de memória passado para <see cref="CudaApi.CudaHostRegister"/> já
        /// se encontra registado.
        /// </summary>
        CudaErrorHostMemoryAlreadyRegistered = 61,

        /// <summary>
        /// O apontador passado para <see cref="CudaApi.CudaHostUnregister"/> não corresponde a nenhuma
        /// região de memória registada.
        /// </summary>
        CudaErrorHostMemoryNotRegistered = 62,

        /// <summary>
        /// Este erro indica que uma chamada de sistema falhou.
        /// </summary>
        CudaErrorOperatingSystem = 63,

        /// <summary>
        /// Este erro indica que o acesso P2P não é suportado entre os dispositivos especificados.
        /// </summary>
        CudaErrorPeerAccessUnsupported = 64,

        /// <summary>
        /// O lançamento de uma grelha de dispositivo não ocorreu porque a profundidade da grelha dependente
        /// execederia o número máximo suportado para lançamentos de grelhas aninhadas.
        /// </summary>
        CudaErrorLaunchMaxDepthExceeded = 65,

        /// <summary>
        /// O lançamento da grelha não pode ocorrer porque o kernel utiliza texturas de escopo de ficheiro
        /// que não são suportadas pelo dispositivo em execução. Os kernels lançados por intermédio do dispositivo
        /// de execução suporta apenas texturas criadas com o objecto de textura da API.
        /// </summary>
        CudaErrorLaunchFileScopedTex = 66,

        /// <summary>
        /// O lançamento da grelha não ocorreu porque o kernel utiliza surfaces de escopo de ficheiro que não
        /// são suportadas pelo dispositivo em execução. Os kernels são lançados por intermédio do dispositivo
        /// em execução suportam apenas surfaces criadas com o objecto de surface da API.
        /// </summary>
        CudaErrorLaunchFileScopedSurf = 67,

        /// <summary>
        /// Indica que a chamada à função <see cref="CudaApi.CudaDeviceSynchronize"/> realizada a partir do
        /// dispositivo em execução falhou porque a chamada foi feita numa profundidade de grelha superior à
        /// que se encontra definida por defeito (dois níveis de grelhas) ou o utilzador especificou o limite
        /// do dispositivo em <see cref="CudaApi.CudaLimitDevRuntimeSyncDeph"/>. De modo a ser possível
        /// sincronizar com sucesso em lançamentos de grelhas num nível de profunidade superior no qual se
        /// pretende chamar <see cref="CudaApi.CudaDeviceSynchronize"/> tem de ser especificado o limite
        /// <see cref="CudaLimitDevRuntimeSyncDeph"/> por intermédio da função 
        /// <see cref="CudaApi.CudaDeviceSetLimit"/> antes do lançamento do lado do anfitrião de um kernel
        /// utilizando o dispositivo em execução. Convém lembrar que níveis adicionais de profundidade de
        /// sincronização requerem que o programa reserve grandes quantidades de memória no dispositivo que não
        /// pode ser utilizada para alocações do utilizador.
        /// </summary>
        CudaErrorSyncDepthExceeded = 68,

        /// <summary>
        /// O lançamento de grelha no dispositivo em execução falhou porque o lançamento excederia o limite
        /// <see cref="CudaApi.CudaLimitDevRuntimePendingLaunchCount"/>. De modo a que o lançamento seja
        /// realizado com sucesso deve ser chamada a função <see cref="CudaApi.CudaDeviceSetLimit"/> para
        /// aumentar <see cref="CudaApi.CudaLimitDevRuntimePendingLaunchCount"/> de modo a ultrapassar o
        /// limite superior para o número de lançamentos que podem ser emitidos para o dispositivo em execução.
        /// Convém lembrar que aumentando o limite de lançamentos em espera no dispositivo de execução requer
        /// que o programa reserve memória de dispositivo que não pode ser utilizada em alocações de utilizador.
        /// </summary>
        CudaErrorLaunchPendingCountExceeded = 69,

        /// <summary>
        /// Este erro indica que a operação experimentada não é permitida.
        /// </summary>
        CudaErrorNotPermitted = 70,

        /// <summary>
        /// Este erro indica que a operação experimentada não é suportada no sistema ou dispositivo correntes.
        /// </summary>
        CudaErrorNotSupported = 71,

        /// <summary>
        /// O dispositivo encontroj um erro na pilha de chamada durante a execução do kernel possivelmente devido
        /// à corrupção da pilha ou ao transbordo do limite do tamanho da pilha. O contexto não pode ser utilizado
        /// e por isso deve ser dstruído (e um novo deverá ser criado). Todas as alocações de memória no
        /// dispositivo são inválidas e devem ser reconstruídas se se pretender continuar a utilizar Cuda no
        /// programa.
        /// </summary>
        CudaErrorHardwareStackError = 72,

        /// <summary>
        /// O dispositivo encontrou uma instrução ilegal durante a execução do kernel. O contexto não pode ser
        /// utilizado e por isso deve ser destruído (e num novo deverá ser criado). Todas as alocações de memória
        /// existentes a partir deste contexto são inválidas e devem ser reconstruídas se se pretender que o
        /// programa continue a utilizar Cuda.
        /// </summary>
        CudaErrorIllegalInstruction = 73,

        /// <summary>
        /// O dispositivo encontrou uma instrução de carregamento ou armazenamento num endereço de memória
        /// que não se encontra alinhado. O contexto não pode ser utilizado e por isso deverá ser destruído (e
        /// um novo deverá ser criado). Todas as alocações de memória deste contexto são inválidas e devem ser
        /// reconstruídas se se pretender que o programa continue a utilizar Cuda.
        /// </summary>
        CudaErrorMisalignedAddress = 74,

        /// <summary>
        /// Durante a execução do kernel, o dispositivo encontrou uma instrução que apenas opera em localizações
        /// de memória em determinados espaços de enedereços (global, partilhado ou local), mas foi proporcionado
        /// um endereço de memória que não pertence a um espaço de endereços permitido. O contexto não pode ser
        /// utilizado e por isso deverá ser destruído (e num novo deverá ser criado). Todas as alocações de
        /// memória do dispositivo para este contexto são inválidas e deverão ser reconstruídas se se pretender
        /// que o programa continue a utilizar Cuda.
        /// </summary>
        CudaErrorInvalidAddressSpace = 75,

        /// <summary>
        /// O dispositivo encontrou um contador de programa inválido. O contexto não pode ser utilizado e por isso
        /// deverá ser destruído (e um novo deverá ser criado). Todas as alocações de memória no dispositivo nesse
        /// contexto são inválidas e deverão ser reconstruídas se se pretender que o program continue a utilizar
        /// Cuda.
        /// </summary>
        CudaErrorInvalidPc = 76,

        /**
         * The device encountered a load or store instruction on an invalid memory address.
         * The context cannot be used, so it must be destroyed (and a new one should be created).
         * All existing device memory allocations from this context are invalid
         * and must be reconstructed if the program is to continue using Cuda.
         */
        /// <summary>
        /// O dispositivo encontrou uma instrução de carga ou armazenamento num endereço de memória inválido. O
        /// contexto não pode ser utilizado e por isso deverá ser destruído (e um novo deverá ser criado). Todas
        /// as alocações de memória do dispositivo existentes são inválidas e deverão ser reconstruídas se se
        /// pretender que o programa continue a utilizar Cuda.
        /// </summary>
        CudaErrorIllegalAddress = 77,

        /// <summary>
        /// Indica uma falha na inicialização interna na execução Cuda.
        /// </summary>
        CudaErrorStartupFailure = 0x7f,

        /// <summary>
        /// Qualquer condutor Cuda não gerido é adicionado a este valor e retornado por intermédio da execução.
        /// Ambientes em produção de Cuda não deveriam retornar tais erros.
        /// </summary>
        [Obsolete("Deprecated as of Cuda 4.1.")]
        CudaErrorApiFailureBase = 10000
    }

    /// <summary>
    /// Definie os limites Cuda.
    /// </summary>
    public enum ECudaLimit
    {
        /// <summary>
        /// O tamanho da pilha na linha de execução CUDA.
        /// </summary>
        CudaLimitStackSize = 0x00,

        /// <summary>
        /// Tamanho do FIFP para printf/fprintf no GPU.
        /// </summary>
        CudaLimitPrintfFifoSize = 0x01,

        /// <summary>
        /// Tamanho do acumulador para malloc no GPU.
        /// </summary>
        CudaLimitMallocHeapSize = 0x02,

        /// <summary>
        /// Profundidade de sincronização do dispositivo GPU em execução.
        /// </summary>
        CudaLimitDevRuntimeSyncDepth = 0x03,

        /// <summary>
        /// Número de lançamentos pedentntes no dispositivo GPU em execução.
        /// </summary>
        CudaLimitDevRuntimePendingLaunchCount = 0x04  /**< GPU device runtime pending launch count */
    }

    /// <summary>
    /// Configurações de provisão de CUDA.
    /// </summary>
    public enum ECudaFuncCache
    {
        /// <summary>
        /// Configuração de provisão por defeito.
        /// </summary>
        CudaFuncCachePreferNone = 0,

        /// <summary>
        /// Preferência por maior disponibilidade de memória partilhada e menor provisão L1.
        /// </summary>
        CudaFuncCachePreferShared = 1,

        /// <summary>
        /// Preferência por maior provisão L1 e menor memória partilhada.
        /// </summary>
        CudaFuncCachePreferL1 = 2,

        /// <summary>
        /// Preferência por igual tamanho de provisão L1 e memória partilhada.
        /// </summary>
        CudaFuncCachePreferEqual = 3 
    }

    /// <summary>
    /// Configuração da memória partilhada CUDA.
    /// </summary>
    public enum CudaSharedMemConfig
    {
        /// <summary>
        /// Configuração por defeito.
        /// </summary>
        CudaSharedMemBankSizeDefault = 0,

        /// <summary>
        /// Configuração para 4 bytes.
        /// </summary>
        CudaSharedMemBankSizeFourByte = 1,

        /// <summary>
        /// Configuração para 8 bytes.
        /// </summary>
        CudaSharedMemBankSizeEightByte = 2
    }

    /// <summary>
    /// Modos de saída do monitor de desempenho.
    /// </summary>
    public enum ECudaOutputMode
    {
        /// <summary>
        /// Modo de saída em par chave/valor.
        /// </summary>
        CudaKeyValuePair = 0x00, 

        /// <summary>
        /// Modo de saída em valores separados por vírgula.
        /// </summary>
        CudaCSV = 0x01
    }

    public enum ECudaDeviceAttr
    {
        /// <summary>
        /// Número máximo de threads por bloco.
        /// </summary>
        CudaDevAttrMaxThreadsPerBlock = 1,  

        /// <summary>
        /// Dimensão de bloco X máxima.
        /// </summary>
        CudaDevAttrMaxBlockDimX = 2,

        /// <summary>
        /// Dimensão de bloco Y máxima.
        /// </summary>
        CudaDevAttrMaxBlockDimY = 3,

        /// <summary>
        /// Dimensão de bloco Z máxima.
        /// </summary>
        CudaDevAttrMaxBlockDimZ = 4,

        /// <summary>
        /// Dimensão de grelha X máxima.
        /// </summary>
        CudaDevAttrMaxGridDimX = 5,

        /// <summary>
        /// Dimensão de grelha Y máxima.
        /// </summary>
        CudaDevAttrMaxGridDimY = 6,

        /// <summary>
        /// Dimensão de grelha Z máxima.
        /// </summary>
        CudaDevAttrMaxGridDimZ = 7,

        /// <summary>
        /// Memória partilhada disponível por bloco em bytes.
        /// </summary>
        CudaDevAttrMaxSharedMemoryPerBlock = 8, 

        /// <summary>
        /// Memória disponível no dispositivo para variáveis marcadas como constantes num kernel
        /// C de CUDA em bytes.
        /// </summary>
        CudaDevAttrTotalConstantMemory = 9,  

        /// <summary>
        /// Tamanho do warp em threads.
        /// </summary>
        CudaDevAttrWarpSize = 10, 

        /// <summary>
        /// Maior passo em bytes permitido em cópias de memória.
        /// </summary>
        CudaDevAttrMaxPitch = 11, 

        /// <summary>
        /// Número máximo de registos 32-bit disponíveis por bloco.
        /// </summary>
        CudaDevAttrMaxRegistersPerBlock = 12, 

        /// <summary>
        /// Frequência do relógio.
        /// </summary>
        CudaDevAttrClockRate = 13, 

        /// <summary>
        /// Alinhamento requerido para texturas.
        /// </summary>
        CudaDevAttrTextureAlignment = 14,

        /// <summary>
        /// O dispositivo pode possivelmente copiar memória e executar um kernel de forma concorrente.
        /// </summary>
        CudaDevAttrGpuOverlap = 15, 

        /// <summary>
        /// Número de multiprocessadores no dispositov.
        /// </summary>
        CudaDevAttrMultiProcessorCount = 16, 

        /// <summary>
        /// Especifica se existe um limite de execução dos kernels.
        /// </summary>
        CudaDevAttrKernelExecTimeout = 17,

        /// <summary>
        /// O dispositivo está integrado com a memória de anfitrião.
        /// </summary>
        CudaDevAttrIntegrated = 18, 

        /// <summary>
        /// O dispositivo pode mapear memória de anfitrião no espaço de endereçamento CUDA.
        /// </summary>
        CudaDevAttrCanMapHostMemory = 19, 

        /// <summary>
        /// Nó de computação (ver <see cref="CudaApi.CudaComputeNode"/>).
        /// </summary>
        CudaDevAttrComputeMode = 20, 

        /// <summary>
        /// Máxima largura da textura 1D.
        /// </summary>
        CudaDevAttrMaxTexture1DWidth = 21, 

        /// <summary>
        /// Máxima largura da textura 2D.
        /// </summary>
        CudaDevAttrMaxTexture2DWidth = 22, 

        /// <summary>
        /// Máxima altura da textura 2D.
        /// </summary>
        CudaDevAttrMaxTexture2DHeight = 23, 

        /// <summary>
        /// Máxima largura da textura 3D.
        /// </summary>
        CudaDevAttrMaxTexture3DWidth = 24, 

        /// <summary>
        /// Máxima altura da textura 3D.
        /// </summary>
        CudaDevAttrMaxTexture3DHeight = 25, 

        /// <summary>
        /// Máxima profundidade da textura 3D.
        /// </summary>
        CudaDevAttrMaxTexture3DDepth = 26, 

        /// <summary>
        /// Máxima largura da textura 2D estratificável.
        /// </summary>
        CudaDevAttrMaxTexture2DLayeredWidth = 27, 

        /// <summary>
        /// Máxima altura da textura 2D estratificável.
        /// </summary>
        CudaDevAttrMaxTexture2DLayeredHeight = 28, 

        /// <summary>
        /// Número máximo de camadas numa textura 2D estratificável.
        /// </summary>
        CudaDevAttrMaxTexture2DLayeredLayers = 29, 

        /// <summary>
        /// Requisitos de alinhamento para surfaces.
        /// </summary>
        CudaDevAttrSurfaceAlignment = 30, 

        /// <summary>
        /// O dispositivo pode possivelmente executar múltiplos kernels concorrentemente.
        /// </summary>
        CudaDevAttrConcurrentKernels = 31, 

        /// <summary>
        /// O dispositivo tem supoyrte Ecc activo.
        /// </summary>
        CudaDevAttrEccEnabled = 32,

        /// <summary>
        /// O identificador do barramento do dispositivo.
        /// </summary>
        CudaDevAttrPciBusId = 33, 

        /// <summary>
        /// Identificador do dispositivo.
        /// </summary>
        CudaDevAttrPciDeviceId = 34, 

        /// <summary>
        /// O dispositivo encontra-se a usar o modelo de condutor TCC.
        /// </summary>
        CudaDevAttrTccDriver = 35,

        /// <summary>
        /// Frequência de relógio de memória.
        /// </summary>
        CudaDevAttrMemoryClockRate = 36, 

        /// <summary>
        /// Largura de banda do barramento de memória global em bits.
        /// </summary>
        CudaDevAttrGlobalMemoryBusWidth = 37, 

        /// <summary>
        /// Tamanho da provisão L2 em bytes.
        /// </summary>
        CudaDevAttrL2CacheSize = 38,

        /// <summary>
        /// Número máximo de threads residentes por multiprocessador.
        /// </summary>
        CudaDevAttrMaxThreadsPerMultiProcessor = 39,

        /// <summary>
        /// Número de motores assíncronos.
        /// </summary>
        CudaDevAttrAsyncEngineCount = 40, 

        /// <summary>
        /// O dispositivo partilha um espaço de endereçamento unificado com o anfitrião.
        /// </summary>
        CudaDevAttrUnifiedAddressing = 41, 

        /// <summary>
        /// Largura máxima da textura 1D estatificável.
        /// </summary>
        CudaDevAttrMaxTexture1DLayeredWidth = 42, 

        /// <summary>
        /// Número máixmo de camadas numa textura 1D estratificável.
        /// </summary>
        CudaDevAttrMaxTexture1DLayeredLayers = 43, 

        /// <summary>
        /// Largura máxima da textura 2D se estiver activa <see cref="CudaApi.CudaArrayTextureGather"/>.
        /// </summary>
        CudaDevAttrMaxTexture2DGatherWidth = 45, 

        /// <summary>
        /// Altura máxima da textura 2D se estiver activa <see cref="CudaApi.CudaArrayTextureGather"/>.
        /// </summary>
        CudaDevAttrMaxTexture2DGatherHeight = 46, 

        /// <summary>
        /// Máximo alternado da largura da textura 3D.
        /// </summary>
        CudaDevAttrMaxTexture3DWidthAlt = 47,

        /// <summary>
        /// Máximo alternado da altura da textura 3D.
        /// </summary>
        CudaDevAttrMaxTexture3DHeightAlt = 48,

        /// <summary>
        /// Máximo alternado da profundidade da textura 3D.
        /// </summary>
        CudaDevAttrMaxTexture3DDepthAlt = 49, 

        /// <summary>
        /// Identificador do domínio do PCI do dispositivo.
        /// </summary>
        CudaDevAttrPciDomainId = 50,

        /// <summary>
        /// Requisito de alinhamento de passo para texturas.
        /// </summary>
        CudaDevAttrTexturePitchAlignment = 51, 

        /// <summary>
        /// Máxima largura/altura das texturas Cubemap.
        /// </summary>
        CudaDevAttrMaxTextureCubemapWidth = 52,

        /// <summary>
        /// Máxima largura/altura das texturas Cubemap estratificáveis.
        /// </summary>
        CudaDevAttrMaxTextureCubemapLayeredWidth = 53, 

        /// <summary>
        /// Número máximo de camadas nas texturas Cubemap estratificáveis.
        /// </summary>
        CudaDevAttrMaxTextureCubemapLayeredLayers = 54, 

        /// <summary>
        /// Largura máxima de uma surface 1D.
        /// </summary>
        CudaDevAttrMaxSurface1DWidth = 55,

        /// <summary>
        /// Largura máxima de uma surface 2D.
        /// </summary>
        CudaDevAttrMaxSurface2DWidth = 56,

        /// <summary>
        /// Altura máxima de uma surface 2D.
        /// </summary>
        CudaDevAttrMaxSurface2DHeight = 57,

        /// <summary>
        /// Largura máxima de uma surface 3D.
        /// </summary>
        CudaDevAttrMaxSurface3DWidth = 58,

        /// <summary>
        /// Altura máxima de uma surface 3D.
        /// </summary>
        CudaDevAttrMaxSurface3DHeight = 59,

        /// <summary>
        /// Profundidade máxima de uma surface 3D.
        /// </summary>
        CudaDevAttrMaxSurface3DDepth = 60,

        /// <summary>
        /// Largura máxima de uma surface 1D estratificável.
        /// </summary>
        CudaDevAttrMaxSurface1DLayeredWidth = 61,

        /// <summary>
        /// Número máximo de camadas numa surface 1D estratificável.
        /// </summary>
        CudaDevAttrMaxSurface1DLayeredLayers = 62,

        /// <summary>
        /// Largura máxima de uma surface 2D estratificável.
        /// </summary>
        CudaDevAttrMaxSurface2DLayeredWidth = 63,

        /// <summary>
        /// Altura máxima de uma surface 1D estratificável.
        /// </summary>
        CudaDevAttrMaxSurface2DLayeredHeight = 64, 

        /// <summary>
        /// Número máximo de camadas numa surface 2D estratificável.
        /// </summary>
        CudaDevAttrMaxSurface2DLayeredLayers = 65, 

        /// <summary>
        /// Largura máxima de uma surface Cubemap.
        /// </summary>
        CudaDevAttrMaxSurfaceCubemapWidth = 66,

        /// <summary>
        /// Largura máxima de uma surface Cubemap estratificável.
        /// </summary>
        CudaDevAttrMaxSurfaceCubemapLayeredWidth = 67, 

        /// <summary>
        /// Número máximo de camadas numa surface estratificável.
        /// </summary>
        CudaDevAttrMaxSurfaceCubemapLayeredLayers = 68, 

        /// <summary>
        /// Largura máxima de texturas 1D lineares.
        /// </summary>
        CudaDevAttrMaxTexture1DLinearWidth = 69,

        /// <summary>
        /// Largura máxima de texturas 2D lineares.
        /// </summary>
        CudaDevAttrMaxTexture2DLinearWidth = 70,

        /// <summary>
        /// Altura máxima de texturas 2D lineares.
        /// </summary>
        CudaDevAttrMaxTexture2DLinearHeight = 71,

        /// <summary>
        /// Paso máximo de texturas 2D lineares em bytes.
        /// </summary>
        CudaDevAttrMaxTexture2DLinearPitch = 72,

        /// <summary>
        /// Largura máxima de texturas 2D mipmap.
        /// </summary>
        CudaDevAttrMaxTexture2DMipmappedWidth = 73,

        /// <summary>
        /// Altura máxima de texturas 2D mipmap.
        /// </summary>
        CudaDevAttrMaxTexture2DMipmappedHeight = 74, 

        /// <summary>
        /// Número máximo de versão para capacidade computacional.
        /// </summary>
        CudaDevAttrComputeCapabilityMajor = 75,

        /// <summary>
        /// Número mínimo de versão para capacidade computacional.
        /// </summary>
        CudaDevAttrComputeCapabilityMinor = 76,

        /// <summary>
        /// Largura máxima de texturas 1D mipmap.
        /// </summary>
        CudaDevAttrMaxTexture1DMipmappedWidth = 77,

        /// <summary>
        /// O dispositivo suporta prioridades de caudal.
        /// </summary>
        CudaDevAttrStreamPrioritiesSupported = 78, 

        /// <summary>
        /// O dispositivo suporta aprovisionamento de globais em L1.
        /// </summary>
        CudaDevAttrGlobalL1CacheSupported = 79, 

        /// <summary>
        /// O dispositivo suporta aprovisionamento de locais em L1.
        /// </summary>
        CudaDevAttrLocalL1CacheSupported = 80, 

        /// <summary>
        /// Memória máxima disponível por multiprocessador em bytes.
        /// </summary>
        CudaDevAttrMaxSharedMemoryPerMultiprocessor = 81, 

        /// <summary>
        /// Número máximo de registos 32-bit disponíveis por multiprocessador.
        /// </summary>
        CudaDevAttrMaxRegistersPerMultiprocessor = 82,

        /// <summary>
        /// O dispositivo pode alocar memória gerida neste sistema.
        /// </summary>
        CudaDevAttrManagedMemory = 83, 

        /// <summary>
        /// O dispositivo encontra-se numa placa multi-GPU.
        /// </summary>
        CudaDevAttrIsMultiGpuBoard = 84, 

        /// <summary>
        /// Identificador único para um grupo de dispositivos na mesma placa multi-GPU.
        /// </summary>
        CudaDevAttrMultiGpuBoardGroupID = 85  /**< Unique identifier for a group of devices on the same multi-GPU board */
    }
}
