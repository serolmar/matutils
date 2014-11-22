namespace Utilities.Cuda
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define os possíveis resultados de uma chamada CUDA.
    /// </summary>
    public enum ECudaResult
    {
        /// <summary>
        /// A chamada à API foi concluída sem erros. Indica também que uma consulta foi concluída.
        /// </summary>
        CudaSuccess = 0,

        /// <summary>
        /// Indica que uma ou mais parâmetros passados para a chamada à API não se encontram
        /// dentro dos limites aceitáveis de valores.
        /// </summary>
        CudaErrorInvalidValue = 1,

        /// <summary>
        /// A chamada à API falhou porque não foi possível alocar memória suficiente para
        /// executar a operação requerida.
        /// </summary>
        CudaErrorOutOfMemory = 2,

        /// <summary>
        /// Indica que o condutor CUDA não foi inicializado com a função <see cref="CudaApi.CudaInit"/>
        /// ou essa inicialização falhou.
        /// </summary>
        CudaErrorNotInitialized = 3,

        /// <summary>
        /// Indica que o condutor CUDA se encontra em processo de encerramento.
        /// </summary>
        CudaErrorDeinitialized = 4,

        /// <summary>
        /// O monitor de desempenho não se encontra inicializado para esta corrida. Isto pode acontecer quando
        /// a aplicação se enconta em execução monitorizada por ferramentas de desempenho externas tais como
        /// o Visual Profiler.
        /// </summary>
        CudaErrorProfilerDisabled = 5,

        /// <summary>
        /// Obsoleto: ver <see cref="CudaApi.CudaProfileStart"/> e <see cref="CudaApi.CudaProfileStop"/>.
        /// </summary>
        [Obsolete("Deprecated as of CUDA 5.0.")]
        CudaErrorProfilerNotInitialized = 6,

        /// <summary>
        /// Obsoleto: ver <see cref="CudaApi.CudaProfileStart"/>.
        /// </summary>
        [Obsolete("Deprecated as of CUDA 5.0.")]
        CudaErrorProfilerAlreadyStarted = 7,

        /// <summary>
        /// Obsoleto: ver <see cref="CudaApi.CudaProfileStop"/>.
        /// </summary>
        [Obsolete("Deprecated as of CUDA 5.0.")]
        CudaErrorProfilerAlreadyStopped = 8,

        /// <summary>
        /// Indica que os dispositivos que suportam CUDA foram detectados pelo condutor
        /// instalado.
        /// </summary>
        CudaErrorNoDevice = 100,

        /// <summary>
        /// Indica que o ordinal de dispositivo proporcionado pelo utilizador não corresonde a um
        /// dispositivo CUDA válido.
        /// </summary>
        CudaErrorInvalidDevice = 101,

        /// <summary>
        /// Indica que a imagem de kernel do dispositivo é inválida. Pode também indicar a existência
        /// de um módulo CUDA inválido.
        /// </summary>
        CudaErrorInvalidImage = 200,

        /// <summary>
        /// Frequentemente indica que não existen nenhum contexto ligado à linha de fluxo corrente.
        /// Também pode ser retornado se o contexto passado para a chamada à API não foi um manuseador
        /// válido (tal como um contexto sobre o qual se invocou <see cref="CudaApi.CudaContexDestroy"/>).
        /// Também pode ser retornado se um utilizador misturar diferentes versões da API (isto é, um contexto
        /// 3010 com chamadas à API 3020). Ver <see cref="CudaApi.CudaContextGetApiVersion"/> para mais detalhes.
        /// </summary>
        CudaErrorInvalidContext = 201,

        /// <summary>
        /// Obsoleto: ver <see cref="CudaApi.CudaContextPushCurrent"/>.
        /// </summary>
        [Obsolete("Deprecated as of CUDA 3.2.")]
        CudaErrorContextAlreadyCurrent = 202,

        /// <summary>
        /// Uma operação de mapeamento ou registo falhou.
        /// </summary>
        CudaErrorMapFailed = 205,

        /// <summary>
        /// Uma operação de eliminação de mapeamento ou registo falhou.
        /// </summary>
        CudaErrorUnmapFailed = 206,

        /// <summary>
        /// O vector corrente encontra-se mapeado e portanto não pode ser destruído.
        /// </summary>
        CudaErrorArrayIsMapped = 207,

        /// <summary>
        /// O recurso já se encontra mapeado.
        /// </summary>
        CudaErrorAlreadyMapped = 208,

        /// <summary>
        /// Não existe nenhuma imagem de kernel disponível que se adeque ao dispositivo. Isto pode ocorrer
        /// quando o utilizador especifica geração de código para um ficheiro de código fonte CUDA particular
        /// que não inclui a configuração de dispositivo correspondente.
        /// </summary>
        CudaErrorNoBinaryForGpu = 209,

        /// <summary>
        /// O recurso já foi adquirido.
        /// </summary>
        CudaErrorAlreadyAcquired = 210,

        /// <summary>
        /// O recurso não se encontra mapeado.
        /// </summary>
        CudaErrorNotMapped = 211,

        /// <summary>
        /// O recurso mapeado não se encontra disponível para acesso como sendo um vector.
        /// </summary>
        CudaErrorNotMappedAsArray = 212,

        /// <summary>
        /// O recurso mapeado não se encontra disponível para acesso como sendo um apontador.
        /// </summary>
        CudaErrorNotMappedAsPointer = 213,

        /// <summary>
        /// Ocorreu um erro ECC que não pôde ser corrigido.
        /// </summary>
        CudaErrorEccUncorrectable = 214,

        /// <summary>
        /// Indica que o <see cref="CudaLimit"/> passado para a chamada à API não é suportado
        /// pelo dispositivo corrente.
        /// </summary>
        CudaErrorUnsupportedLimit = 215,

        /// <summary>
        /// Indica que o <see cref="CudaContext"/> passado para a chamada à API pode apenas ser
        /// ligado a uma única linha de fluxo no CPU de cada vez mas encontra-se já ligado a uma linha
        /// de fluxo de CPU.
        /// </summary>
        CudaErrorContextAlreadyInUse = 216,

        /// <summary>
        /// Indica que o acesso de porto não é suportado entre dispositivos.
        /// </summary>
        CudaErrorPeerAccessUnsupported = 217,

        /// <summary>
        /// Indica que a compilação PTX JIT falhou.
        /// </summary>
        CudaErrorInvalidPtx = 218,

        /// <summary>
        /// Erro ao nível do OpenGL ou DirectX.
        /// </summary>
        CudaErrorInvalidGraphicsContext = 219,

        /// <summary>
        /// A fonte de kernel do dispositivo é inválida.
        /// </summary>
        CudaErrorInvalidSource = 300,

        /// <summary>
        /// O ficheiro especificado não foi encontrado.
        /// </summary>
        CudaErrorFileNotFound = 301,

        /// <summary>
        /// Indica que a ligação a um objecto partilhado não foi resolvida com sucesso.
        /// </summary>
        CudaErrorSharedObjectSymbolNotFound = 302,

        /// <summary>
        /// Falha na inicialização de um objecto partilhado.
        /// </summary>
        CudaErrorSharedObjectInitFailed = 303,

        /// <summary>
        /// Uma chamada ao sistema operativo falhou.
        /// </summary>
        CudaErrorOperatingSystem = 304,

        /// <summary>
        /// Um manuseador de recurso passado para a chamda à API não é válido. Manuseadores de recurso
        /// são tipos opacos tais como <see cref="CudaStream"/> e <see cref="CudaEvent"/>.
        /// </summary>
        CudaErrorInvalidHandler = 400,

        /// <summary>
        /// Indica que um símbolo denominado não foi encontrado. Exemplo de símbolos constituem as variáveis e
        /// constantes globais, nomes de texturas e nomes de surfaces.
        /// </summary>
        CudaErrorNotFound = 500,

        /// <summary>
        /// Indica que a operação emitida anteriormente ainda não terminou. Este resultado não consiste 
        /// exactamente num erro mas deve ser indicado de modo diferente do <see cref="CudaResult.CudaSuccess"/>
        /// (que indica que a operação terminou). Chamadas que poderão retornar este valor incluem 
        /// <see cref="CudaEventQuery"/> e <see cref="CudaStreamQuery"/>.
        /// </summary>
        CudaErrorNotReady = 600,

        /// <summary>
        /// Durante a execução de um kernel, o dispositivo entrou uma instrução de carga ou armazenamento numa
        /// zona inválida da memória. O contexto não pode ser mais usado e deve ser destruído (e num novo deverá
        /// ser criado). Todas as alocações de memória existentes neste contexto são inválidas e deverão ser
        /// resconstruídas se se pretender que o programa continue a utilizar CUDA.
        /// </summary>
        CudaErrorIlegalAddress = 700,

        /// <summary>
        /// Indica que um lançamento não ocorreu porque não dispunha de recursos suficientes. Usualmente indica
        /// que o utilizador tentou passar demasiados argumentos para o kernel do dispositivo ou o kernel
        /// especifica demasiadas threads para o contador de registo do kernel. Passando argumentos do tamanho
        /// errado (isto é, uma apontador de 64 bit quando um de 32 bit é esperado) é equivalente a passar
        /// muitos argumentos e pode resultar neste erro.
        /// </summary>
        CudaErrorLaunchOutOfResources = 701,

        /// <summary>
        /// Indica que o kernel do dispositivo levou demasiado tempo a executar. Isto só pode acontecer se
        /// limites de tempo estiverem activos - ver o atributo de dispositivo
        /// CU_DEVICE_ATTRIBUTE_KERNEL_EXEC_TIMEOUT para mais informação. O contexto não pode ser utilizado (e
        /// deverá ser destruído à semelhança do <see cref="CudaResult.CudaErrorLaunchFailed"/>). Todas as
        /// alocações de memória existentes no dispositivo neste contexto são inválidas e deverão ser
        /// reconstruídas se se pretender continuar a utilizar CUDA.
        /// </summary>
        CudaErrorLaunchTimeout = 702,

        /// <summary>
        /// Assinala um lançamento de um kernel que usa um modo de textura incompatível.
        /// </summary>
        CudaErrorLaunchIncompatibleTexturing = 703,

        /// <summary>
        /// Indica que a chamada à função <see cref="CudaApi.CudaContextEnablePeerAccess"/> se encontra a tentar
        /// reestabelecer acesso de porto a um contexto que já possui acesso de porto activo.
        /// </summary>
        CudaErrorPeerAccessAlreadyEnabled = 704,

        /// <summary>
        /// Indica que a função <see cref="CudaApi.CudaContextDisablePeerAccess"/> está a tentar desabilitar
        /// acesso de porto que ainda não foi habilitado via <see cref="CudaApi.CudaContextEnablePeerAccess"/>.
        /// </summary>
        CudaErrorPeerAccessNotEnabled = 705,

        /// <summary>
        /// O contexo primário para o dispositivo especificado já foi inicializado.
        /// </summary>
        CudaErrorPrimaryContextActive = 708,

        /// <summary>
        /// O contexto corrente na linha de fluxo que se encontra a realizar a chamada foi destruído via
        /// <see cref="CudaApi.CudaContextDestroy"/> ou é um contexto primário que ainda não foi inicializado.
        /// </summary>
        CudaErrorContextIsDestroyed = 709,

        /// <summary>
        /// Uma asseveraão foi despoletada do lado do dispositivo por um kernel em execução. O contexto não
        /// pode mais ser usado e deve ser destruído. Todas as alocações de memória existentes neste contexto
        /// deverão ser reconstruídas se se pretender continuar a utilizar CUDA.
        /// </summary>
        CudaErrorAssert = 710,

        /// <summary>
        /// Indica que os recursos requeridos para habilitar o acesso de porto foram exaurdios para um ou mais
        /// dispositivos passados para a função <see cref="CudaApi.CudaContextEnablePeerAccess"/>.
        /// </summary>
        CudaErrorTooManyPeers = 711,

        /// <summary>
        /// Indica que o intervalo de memória passada para a função <see cref="CudaApi.CudaMemoryHostRegister"/>
        /// não corresponde a nenhuma região de memória correntemente registada.
        /// </summary>
        CudaErrorHostMemoryAlreadyRegisteres = 712,

        /// <summary>
        /// Indica que o apontador passado para <see cref="CudaApi.CudaMemoryHostUnregister"/> não corresopnde
        /// a nenhuma região de memória correntemente registada.
        /// </summary>
        CudaErrorHostMemoryNotRegistered = 713,

        /// <summary>
        /// Durante a execução do kernel, o dispositivo encontrou um erro de pilha. Isto pode-se dever à
        /// corrupção ou ao transbordo do limite da pilha. O contexto não pode mais ser utilizado e deve ser
        /// destruído (e num novo deverá ser criado). Todas as alocações de memória existentes deverão ser
        /// reconstruídas se se prenteder continuar a utilizar CUDA.
        /// </summary>
        CudaErrorHardwareStackError = 714,

        /// <summary>
        /// Durante a execução do kernel, o dispositivo encontrou uma instrução ilegal. O contexto não pode mais
        /// ser utilizado e deverá ser destruído (e um novo deverá ser criado). Todas as alocações de memória
        /// existentes deverão ser reconstruídas se se pretender continuar a utilizar CUDA.
        /// </summary>
        CudaErrorIllegalInstruction = 715,

        /// <summary>
        /// Durante a execução do kernel, o dispositivo encontrou uma instrução de carga ou armazenamento num
        /// endereço de memória que não se encontra alinhado. O contexto não pode ser utilizado e deve ser
        /// destruído (e num novo deverá ser criado). Todas as alocações de memória existentes deverão ser
        /// reconstruídas se se pretender continuar a utilizar CUDA.
        /// </summary>
        CudaErrorMissalignedAddress = 716,

        /// <summary>
        /// Durante a execução do kernel, o dispositivo encontrou uma instrução que só pode operar em localizações
        /// de memória em certos espaços de endereços (glocal, partilhado ou local), mas foi fornecida um
        /// endereço de memória que não pertence ao espaço de endereços permitido. O contexto não pode ser
        /// utilizado e deve ser destruído (e um novo deverá ser criado). Todas as alocações de memória
        /// existentes deverão ser reconstruídas se se pretender continuar a utilizar CUDA.
        /// </summary>
        CudaErrorInvalidAddressSpace = 717,

        /// <summary>
        /// Durante a execução do kernel, o contador de programa do dispositivo encompassou o seu espaço de
        /// memória. O contexto não pode ser utilizado e deve ser destruído (e um novo deverá ser criado). Todas
        /// as alocações de memória existentes deverão ser reconstruídas se se pretender continuar a utilizaro
        /// CUDA.
        /// </summary>
        CudaErrorInvalidPc = 718,

        /// <summary>
        /// Uma excepção ocorreu no dispositivo durante a execução do kernel. Causas comuns incluem
        /// de-referenciação de um apontador de dispositivo inválido, acedendo a zonas de memória fora dos limites.
        /// O contexto não pode ser utilizado e deve ser destruído (e um novo deverá ser criado). Todas as
        /// alocações de memória existentes deverão ser reconstruídas se se pretender continuar a utilizar CUDA.
        /// </summary>
        CudaErrorLaunchFailed = 719,

        /// <summary>
        /// A operaçõa tentada não é permitida.
        /// </summary>
        CudaErrorNotPermited = 800,

        /// <summary>
        /// A operação tentada não é suportada no sistema corrente ou dispositivo.
        /// </summary>
        CudaErrorNotSupported = 801,

        /// <summary>
        /// Indica que aconteceu um erro interno desconhecido.
        /// </summary>
        CudaErrorUnknown = 999
    }

    /// <summary>
    /// Modos de endereçamento de texturas.
    /// </summary>
    public enum ECudaAddressMode
    {
        /// <summary>
        /// Modo de endereçamento envoltório.
        /// </summary>
        Wrap = 0,

        /// <summary>
        /// Modo de endereçamento preso à borda.
        /// </summary>
        Clamp = 1,

        /// <summary>
        /// Modo de endereçamento de espelho.
        /// </summary>
        Mirror = 2,

        /// <summary>
        /// Modo de endereço de borda.
        /// </summary>
        Border = 3
    }

    /// <summary>
    /// Índices de vector para faces de cubos.
    /// </summary>
    public enum ECudaArrayCubemapFace
    {
        /// <summary>
        /// Face X positiva do cubo.
        /// </summary>
        PositiveX = 0x00,

        /// <summary>
        /// Face X negativa do cubo.
        /// </summary>
        NegativeX = 0x01,

        /// <summary>
        /// Face Y positiva do cubo.
        /// </summary>
        PositiveY = 0x02,

        /// <summary>
        /// Face Y negativa do cubo.
        /// </summary>
        NegativeY = 0x03,

        /// <summary>
        /// Face Z positiva do cubo.
        /// </summary>
        PositiveZ = 0x04,

        /// <summary>
        /// Face Z negativa do cubo.
        /// </summary>
        NegativeZ = 0x05
    }

    /// <summary>
    /// Formatos dos vectores.
    /// </summary>
    public enum ECudaArrayFormat
    {
        /// <summary>
        /// Inteiros sem sinal de 8 bits.
        /// </summary>
        UnsignedInt8 = 0x01,

        /// <summary>
        /// Inteiros sem sinal de 16 bits.
        /// </summary>
        UnsignedInt16 = 0x02,

        /// <summary>
        /// Inteiros sem sinal de 32 bits.
        /// </summary>
        UnsignedInt32 = 0x03,

        /// <summary>
        /// Inteiros com sinal de 8 bits.
        /// </summary>
        SignedInt8 = 0x08,

        /// <summary>
        /// Inteiros com sinal de 16 bits.
        /// </summary>
        SignedInt16 = 0x09,

        /// <summary>
        /// Inteiros com sinal de 32 bits.
        /// </summary>
        SignedInt32 = 0x0A,

        /// <summary>
        /// Ponto flutuante de 16 bits.
        /// </summary>
        Half = 0x10,

        /// <summary>
        /// Ponto flutuante de 32 bits.
        /// </summary>
        Float = 0x20
    }

    /// <summary>
    /// Modos de computação.
    /// </summary>
    public enum ECudaComputeMode
    {
        /// <summary>
        /// Modo de computação por defeito (múltiplos contextos são permitidos por dispositivo).
        /// </summary>
        Default = 0,

        /// <summary>
        /// Computação exclusiva a linha de fluxo (apenas um contexto utilizador por uma única linha de fluxo
        /// pode estar presente no dispositivo ao mesmo tempo).
        /// </summary>
        Exclusive = 1,

        /// <summary>
        /// Modo de computação proibitiva (nenhum contexto pode ser criado de momento no dispositivo).
        /// </summary>
        Prohibited = 2,

        /// <summary>
        /// Modo de computação de processo exclusivo (apenas um contexto utilizado por um processo apenas
        /// pode estar presente no mesmo instante no dispositivo).
        /// </summary>
        ExclusiveProcess = 3
    }

    /// <summary>
    /// Marcas para criação de contexto.
    /// </summary>
    public enum ECudaContexFlags
    {
        /// <summary>
        /// Calenderização automática.
        /// </summary>
        SchedAuto = 0x00,

        /// <summary>
        /// Estabelece a rotação como calenderização por defeito.
        /// </summary>
        SchedSpin = 0x01,

        /// <summary>
        /// Estabelece a concessão ocmo calenderização por defeito.
        /// </summary>
        ScheddYield = 0x02,

        /// <summary>
        /// Estabelece a calenderização em bloco como calenderização por defeito.
        /// </summary>
        SchedBlockingSync = 0x04,

        /// <summary>
        /// Foi susbituído por <see cref="CudaContexFlags.SchedBlockingSync"/>.
        /// </summary>
        [Obsolete("Deprecated as of CUDA 4.0.")]
        BlockingSync = 0x04,

        /// <summary>
        /// Suporta máscaras.
        /// </summary>
        SchedMask = 0x07,

        /// <summary>
        /// Suporta alocações mapeadas.
        /// </summary>
        MapHost = 0x08,

        /// <summary>
        /// Mantém a alocação de memória após o lançamento.
        /// </summary>
        LmemResizeToMax = 0x10,

        /// <summary>
        /// Máscara de marcações.
        /// </summary>
        FlagsMask = 0x1F
    }

    /// <summary>
    /// Os atributos dos dispositivo.
    /// </summary>
    public enum ECudaDeviceAttr
    {
        /// <summary>
        /// Número máximo de threads por bloco.
        /// </summary>
        MaxThreadsPerBlock = 1,

        /// <summary>
        /// Dimensão de bloco X máxima.
        /// </summary>
        MaxBlockDimX = 2,

        /// <summary>
        /// Dimensão de bloco Y máxima.
        /// </summary>
        MaxBlockDimY = 3,

        /// <summary>
        /// Dimensão de bloco Z máxima.
        /// </summary>
        MaxBlockDimZ = 4,

        /// <summary>
        /// Dimensão de grelha X máxima.
        /// </summary>
        MaxGridDimX = 5,

        /// <summary>
        /// Dimensão de grelha Y máxima.
        /// </summary>
        MaxGridDimY = 6,

        /// <summary>
        /// Dimensão de grelha Z máxima.
        /// </summary>
        MaxGridDimZ = 7,

        /// <summary>
        /// Memória partilhada disponível por bloco em bytes.
        /// </summary>
        MaxSharedMemoryPerBlock = 8,

        /// <summary>
        /// Memória disponível no dispositivo para variáveis marcadas como constantes num kernel
        /// C de CUDA em bytes.
        /// </summary>
        TotalConstantMemory = 9,

        /// <summary>
        /// Tamanho do warp em threads.
        /// </summary>
        WarpSize = 10,

        /// <summary>
        /// Maior passo em bytes permitido em cópias de memória.
        /// </summary>
        MaxPitch = 11,

        /// <summary>
        /// Número máximo de registos 32-bit disponíveis por bloco.
        /// </summary>
        MaxRegistersPerBlock = 12,

        /// <summary>
        /// Frequência do relógio.
        /// </summary>
        ClockRate = 13,

        /// <summary>
        /// Alinhamento requerido para texturas.
        /// </summary>
        TextureAlignment = 14,

        /// <summary>
        /// O dispositivo pode possivelmente copiar memória e executar um kernel de forma concorrente.
        /// </summary>
        GpuOverlap = 15,

        /// <summary>
        /// Número de multiprocessadores no dispositov.
        /// </summary>
        MultiProcessorCount = 16,

        /// <summary>
        /// Especifica se existe um limite de execução dos kernels.
        /// </summary>
        KernelExecTimeout = 17,

        /// <summary>
        /// O dispositivo está integrado com a memória de anfitrião.
        /// </summary>
        Integrated = 18,

        /// <summary>
        /// O dispositivo pode mapear memória de anfitrião no espaço de endereçamento CUDA.
        /// </summary>
        CanMapHostMemory = 19,

        /// <summary>
        /// Nó de computação (ver <see cref="CudaApi.CudaComputeNode"/>).
        /// </summary>
        ComputeMode = 20,

        /// <summary>
        /// Máxima largura da textura 1D.
        /// </summary>
        MaxTexture1DWidth = 21,

        /// <summary>
        /// Máxima largura da textura 2D.
        /// </summary>
        MaxTexture2DWidth = 22,

        /// <summary>
        /// Máxima altura da textura 2D.
        /// </summary>
        MaxTexture2DHeight = 23,

        /// <summary>
        /// Máxima largura da textura 3D.
        /// </summary>
        MaxTexture3DWidth = 24,

        /// <summary>
        /// Máxima altura da textura 3D.
        /// </summary>
        MaxTexture3DHeight = 25,

        /// <summary>
        /// Máxima profundidade da textura 3D.
        /// </summary>
        MaxTexture3DDepth = 26,

        /// <summary>
        /// Máxima largura da textura 2D estratificável.
        /// </summary>
        MaxTexture2DLayeredWidth = 27,

        /// <summary>
        /// Máxima altura da textura 2D estratificável.
        /// </summary>
        MaxTexture2DLayeredHeight = 28,

        /// <summary>
        /// Número máximo de camadas numa textura 2D estratificável.
        /// </summary>
        MaxTexture2DLayeredLayers = 29,

        /// <summary>
        /// Requisitos de alinhamento para surfaces.
        /// </summary>
        SurfaceAlignment = 30,

        /// <summary>
        /// O dispositivo pode possivelmente executar múltiplos kernels concorrentemente.
        /// </summary>
        ConcurrentKernels = 31,

        /// <summary>
        /// O dispositivo tem supoyrte Ecc activo.
        /// </summary>
        EccEnabled = 32,

        /// <summary>
        /// O identificador do barramento do dispositivo.
        /// </summary>
        PciBusId = 33,

        /// <summary>
        /// Identificador do dispositivo.
        /// </summary>
        PciDeviceId = 34,

        /// <summary>
        /// O dispositivo encontra-se a usar o modelo de condutor TCC.
        /// </summary>
        TccDriver = 35,

        /// <summary>
        /// Frequência de relógio de memória.
        /// </summary>
        MemoryClockRate = 36,

        /// <summary>
        /// Largura de banda do barramento de memória global em bits.
        /// </summary>
        GlobalMemoryBusWidth = 37,

        /// <summary>
        /// Tamanho da provisão L2 em bytes.
        /// </summary>
        L2CacheSize = 38,

        /// <summary>
        /// Número máximo de threads residentes por multiprocessador.
        /// </summary>
        MaxThreadsPerMultiProcessor = 39,

        /// <summary>
        /// Número de motores assíncronos.
        /// </summary>
        AsyncEngineCount = 40,

        /// <summary>
        /// O dispositivo partilha um espaço de endereçamento unificado com o anfitrião.
        /// </summary>
        UnifiedAddressing = 41,

        /// <summary>
        /// Largura máxima da textura 1D estatificável.
        /// </summary>
        MaxTexture1DLayeredWidth = 42,

        /// <summary>
        /// Número máixmo de camadas numa textura 1D estratificável.
        /// </summary>
        MaxTexture1DLayeredLayers = 43,

        /// <summary>
        /// Largura máxima da textura 2D se estiver activa <see cref="CudaApi.CudaArrayTextureGather"/>.
        /// </summary>
        MaxTexture2DGatherWidth = 45,

        /// <summary>
        /// Altura máxima da textura 2D se estiver activa <see cref="CudaApi.CudaArrayTextureGather"/>.
        /// </summary>
        MaxTexture2DGatherHeight = 46,

        /// <summary>
        /// Máximo alternado da largura da textura 3D.
        /// </summary>
        MaxTexture3DWidthAlt = 47,

        /// <summary>
        /// Máximo alternado da altura da textura 3D.
        /// </summary>
        MaxTexture3DHeightAlt = 48,

        /// <summary>
        /// Máximo alternado da profundidade da textura 3D.
        /// </summary>
        MaxTexture3DDepthAlt = 49,

        /// <summary>
        /// Identificador do domínio do PCI do dispositivo.
        /// </summary>
        PciDomainId = 50,

        /// <summary>
        /// Requisito de alinhamento de passo para texturas.
        /// </summary>
        TexturePitchAlignment = 51,

        /// <summary>
        /// Máxima largura/altura das texturas Cubemap.
        /// </summary>
        MaxTextureCubemapWidth = 52,

        /// <summary>
        /// Máxima largura/altura das texturas Cubemap estratificáveis.
        /// </summary>
        MaxTextureCubemapLayeredWidth = 53,

        /// <summary>
        /// Número máximo de camadas nas texturas Cubemap estratificáveis.
        /// </summary>
        MaxTextureCubemapLayeredLayers = 54,

        /// <summary>
        /// Largura máxima de uma surface 1D.
        /// </summary>
        MaxSurface1DWidth = 55,

        /// <summary>
        /// Largura máxima de uma surface 2D.
        /// </summary>
        MaxSurface2DWidth = 56,

        /// <summary>
        /// Altura máxima de uma surface 2D.
        /// </summary>
        MaxSurface2DHeight = 57,

        /// <summary>
        /// Largura máxima de uma surface 3D.
        /// </summary>
        MaxSurface3DWidth = 58,

        /// <summary>
        /// Altura máxima de uma surface 3D.
        /// </summary>
        MaxSurface3DHeight = 59,

        /// <summary>
        /// Profundidade máxima de uma surface 3D.
        /// </summary>
        MaxSurface3DDepth = 60,

        /// <summary>
        /// Largura máxima de uma surface 1D estratificável.
        /// </summary>
        MaxSurface1DLayeredWidth = 61,

        /// <summary>
        /// Número máximo de camadas numa surface 1D estratificável.
        /// </summary>
        MaxSurface1DLayeredLayers = 62,

        /// <summary>
        /// Largura máxima de uma surface 2D estratificável.
        /// </summary>
        MaxSurface2DLayeredWidth = 63,

        /// <summary>
        /// Altura máxima de uma surface 1D estratificável.
        /// </summary>
        MaxSurface2DLayeredHeight = 64,

        /// <summary>
        /// Número máximo de camadas numa surface 2D estratificável.
        /// </summary>
        MaxSurface2DLayeredLayers = 65,

        /// <summary>
        /// Largura máxima de uma surface Cubemap.
        /// </summary>
        MaxSurfaceCubemapWidth = 66,

        /// <summary>
        /// Largura máxima de uma surface Cubemap estratificável.
        /// </summary>
        MaxSurfaceCubemapLayeredWidth = 67,

        /// <summary>
        /// Número máximo de camadas numa surface estratificável.
        /// </summary>
        MaxSurfaceCubemapLayeredLayers = 68,

        /// <summary>
        /// Largura máxima de texturas 1D lineares.
        /// </summary>
        MaxTexture1DLinearWidth = 69,

        /// <summary>
        /// Largura máxima de texturas 2D lineares.
        /// </summary>
        MaxTexture2DLinearWidth = 70,

        /// <summary>
        /// Altura máxima de texturas 2D lineares.
        /// </summary>
        MaxTexture2DLinearHeight = 71,

        /// <summary>
        /// Paso máximo de texturas 2D lineares em bytes.
        /// </summary>
        MaxTexture2DLinearPitch = 72,

        /// <summary>
        /// Largura máxima de texturas 2D mipmap.
        /// </summary>
        MaxTexture2DMipmappedWidth = 73,

        /// <summary>
        /// Altura máxima de texturas 2D mipmap.
        /// </summary>
        MaxTexture2DMipmappedHeight = 74,

        /// <summary>
        /// Número máximo de versão para capacidade computacional.
        /// </summary>
        ComputeCapabilityMajor = 75,

        /// <summary>
        /// Número mínimo de versão para capacidade computacional.
        /// </summary>
        ComputeCapabilityMinor = 76,

        /// <summary>
        /// Largura máxima de texturas 1D mipmap.
        /// </summary>
        MaxTexture1DMipmappedWidth = 77,

        /// <summary>
        /// O dispositivo suporta prioridades de caudal.
        /// </summary>
        StreamPrioritiesSupported = 78,

        /// <summary>
        /// O dispositivo suporta aprovisionamento de globais em L1.
        /// </summary>
        GlobalL1CacheSupported = 79,

        /// <summary>
        /// O dispositivo suporta aprovisionamento de locais em L1.
        /// </summary>
        LocalL1CacheSupported = 80,

        /// <summary>
        /// Memória máxima disponível por multiprocessador em bytes.
        /// </summary>
        MaxSharedMemoryPerMultiprocessor = 81,

        /// <summary>
        /// Número máximo de registos 32-bit disponíveis por multiprocessador.
        /// </summary>
        MaxRegistersPerMultiprocessor = 82,

        /// <summary>
        /// O dispositivo pode alocar memória gerida neste sistema.
        /// </summary>
        ManagedMemory = 83,

        /// <summary>
        /// O dispositivo encontra-se numa placa multi-GPU.
        /// </summary>
        IsMultiGpuBoard = 84,

        /// <summary>
        /// Identificador único para um grupo de dispositivos na mesma placa multi-GPU.
        /// </summary>
        MultiGpuBoardGroupID = 85,

        /// <summary>
        /// Máximo.
        /// </summary>
        Max = 86
    }

    /// <summary>
    /// As marcas dos eventos.
    /// </summary>
    public enum ECudaEventFlags
    {
        /// <summary>
        /// Marca de evento por defeito.
        /// </summary>
        Default = 0x0,

        /// <summary>
        /// O evento utiliza sincronização por blocos.
        /// </summary>
        BlockingSync = 0x1,

        /// <summary>
        /// O evento não irá guardar dados temporais.
        /// </summary>
        DisableTiming = 0x2,

        /// <summary>
        /// O evento é adequado para uso de interprocesso.
        /// O parâmetro <see cref="ECudaEventFlags.DisableTiming"/> deve estar atribuído.
        /// </summary>
        Interprocess = 0x4
    }

    /// <summary>
    /// O modo de filtros para referências de texturas.
    /// </summary>
    public enum ECudaFilterMode
    {
        /// <summary>
        /// O modo de filtro por pontos.
        /// </summary>
        Point = 0,

        /// <summary>
        /// O modo de filtro por linhas.
        /// </summary>
        Linear = 1
    }

    /// <summary>
    /// Configuração da provisão para funções.
    /// </summary>
    public enum ECudaFuncCache
    {
        /// <summary>
        /// Nenhuma preferência para memória partilhada ou L1 (por defeito).
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Preferência por grande memória partilhada e menor provisão L1.
        /// </summary>
        Shared = 0x01,

        /// <summary>
        /// Preferência por grande provisão L1 e menor memória partilhad.
        /// </summary>
        L1 = 0x02,

        /// <summary>
        /// Preferência por igual provisão L1 e memória partilhada.
        /// </summary>
        Equal = 0x03
    }

    /// <summary>
    /// Propriedades das funções.
    /// </summary>
    public enum ECudaFuncAttribute
    {
        /// <summary>
        /// O número máximo de linhas de fluxo por bloco, a partir do qual o lançamento da função irá
        /// falhar. Este número depende da função e do dispositivo onde a função se encontra actualmente
        /// carregada.
        /// </summary>
        MaxThreadsPerBlock = 0,

        /// <summary>
        /// O tamanho em bytes da memória partilhada estaticamente alocada requerida pela função. Não inclui
        /// memória partilhada dinamicamente alocada requerida pelo utilizador durante a execução.
        /// </summary>
        SharedSizeBytes = 1,

        /// <summary>
        /// O tamanho em bytes de memória constante alocada pelo utilizador requerida pela função.
        /// </summary>
        ConstSizeBytes = 2,

        /// <summary>
        /// O tamanho em bytes de memória local usada por cada linha de fluxo da função.
        /// </summary>
        LocalSizeBytes = 3,

        /// <summary>
        /// O número de registos utilizado por cada linha de fluxo da função.
        /// </summary>
        NumRegs = 4,

        /// <summary>
        /// A versão da arquitectura virtual PTX para a qual a função foi compilada. Este valor corresponde ao
        /// maior número de versão * 10 + o menor número de versão, portanto uma versão de função PTX 1.3
        /// retorna o valor 13. Note-se que pode retornar o valor 0 para cubins compilados numa versão CUDA
        /// anterior à 3.0.
        /// </summary>
        PtxVersion = 5,

        /// <summary>
        /// A arquitectura binária para a qual a função foi compilada. ESte valor consiste no maior valor da
        /// versão * 10 + o menor valor da versão, portanto para uma versão binária 1.3 irá retornar 13.
        /// Note-se que irá retornar o valor 10 para cubins de legado  que não possuem uma
        /// versão de arquitectura binária propriamente codificada.
        /// </summary>
        BinaryVersion = 6,

        /// <summary>
        /// Um atributo que indica se a função foi compilada com a opção de utilizador "-Xptxas --dlcm=ca".
        /// </summary>
        CacheModeCa = 7,

        /// <summary>
        /// Máximo.
        /// </summary>
        AttributeMax = 8
    }

    /// <summary>
    /// Marcas para mapear e desmapear recursos de interop.
    /// </summary>
    public enum ECudaGraphicsMapResourceFlags
    {
        /// <summary>
        /// Sem descrição de momento.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Sem descrição de momento.
        /// </summary>
        Only = 0x01,

        /// <summary>
        /// Sem descrição de momento.
        /// </summary>
        Discard = 0x02,
    }

    /// <summary>
    /// Marcas para registar um recurso gráfico.
    /// </summary>
    public enum ECudaGraphicsRegisterFlags
    {
        /// <summary>
        /// Sem descrição de momento.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Sem descrição de momento.
        /// </summary>
        Only = 0x01,

        /// <summary>
        /// Sem descrição de momento.
        /// </summary>
        Discard = 0x02,

        /// <summary>
        /// Sem descrição de momento.
        /// </summary>
        Ldst = 0x04,

        /// <summary>
        /// Sem descrição de momento.
        /// </summary>
        TextureGather = 0x08
    }

    /// <summary>
    /// Marcas para CUDA IPC.
    /// </summary>
    public enum CudaIpcMemFlags
    {
        /// <summary>
        /// Habilitar automaticamente o acesso de porto entre dispositivos remotos
        /// conforme é necessário.
        /// </summary>
        LazyEnablePeerAccess = 0x1
    }

    /// <summary>
    /// Códigos de formato de dispositivo.
    /// </summary>
    public enum CudaJitInputType
    {
        /// <summary>
        /// Nenhum código de classe compilada específica de dispositivo.
        /// </summary>
        Cubin = 0,

        /// <summary>
        /// Opção de código fonte PTX.
        /// </summary>
        Ptx,

        /// <summary>
        /// Agregação de múltiplos cubins e/ou PTX do mesmo código de dispositivo.
        /// </summary>
        FatBinary,

        /// <summary>
        /// Objecto de anfitrião com código de dispositivo embebido.
        /// </summary>
        Object,

        /// <summary>
        /// Arquivo de objectos de anfitrião com código de dispositivo embebido. 
        /// </summary>
        Library,

        /// <summary>
        /// Sem descrição de momento.
        /// </summary>
        InputTypes
    }

    /// <summary>
    /// Modos de provisão para DCLM.
    /// </summary>
    public enum CudaJitCacheMode
    {
        /// <summary>
        /// Compilar sem a marca -dclm especificada.
        /// </summary>
        None = 0,

        /// <summary>
        /// Compilar com a provisão L1 inactiva.
        /// </summary>
        Cg,

        /// <summary>
        /// Compilar com a provisão L1 activa.
        /// </summary>
        Ca
    }

    /// <summary>
    /// Estratégias de reserva para cubin.
    /// </summary>
    public enum CudaJitFallback
    {
        /// <summary>
        /// Preferível compilar ptx se nenhuma correspondência binária for encontrada.
        /// </summary>
        Ptx = 0,

        /// <summary>
        /// Preferível recuar para código binário se nenhuma correspondência exacta não for encontrada.
        /// </summary>
        Binary
    }

    /// <summary>
    /// Opções de compilador.
    /// </summary>
    public enum CudaJitOption
    {
        /// <summary>
        /// Número máximo de registos que uma linha de fluxo pode ocupar.
        /// </summary>
        /// <remarks>
        /// Tipo: uint
        /// Aplicação: apenas ao compilador
        /// </remarks>
        MaxRegisters = 0,

        /// <summary>
        /// Entrada: especifica o número mínimo de linhas de flux por bloco para resultado de compilação.
        /// Saída: retorna o número de linhas de fluxo que o resultou da execução do compilador. Isto restringe
        /// a utilização de recursos ao compilador (como por exemplo, o número máximo de registos) de modo que
        /// um bloco com um determinado número de linhsa de fluxo deva ser capaz de se lançar apesar das
        /// suas limitações em termos de registos. Note-se que esta opção não leva em conta qualquer outra
        /// limitação de recurso como é o caso da utilização da memória partilhada. Não pode ser combinado
        /// com <see cref="ECudaJitOption.Target"/>.
        /// </summary>
        /// <remarks>
        /// Tipo: uint
        /// Aplicação: apenas ao compilador
        /// </remarks>
        ThreadsPerBlock,

        /// <summary>
        /// Subrescreve o valor da opção com o valor total do relógio, em milissegundos, demorados pela
        /// aplicação do compilador e do ligador.
        /// </summary>
        /// <remarks>
        /// Tipo: float
        /// Aplicação: compilador e ligador
        /// </remarks>
        WallTime,

        /// <summary>
        /// Apontador para um amortecedor no qual são imprimidas mensagens de registo cuja natureza é
        /// informacional (o tamanho do amortecedor é especificado via 
        /// <see cref="ECudaJitOption.InfoLogBufferSizeBytes"/>).
        /// </summary>
        /// <remarks>
        /// Tipo: ref char
        /// Aplicação: compilador e ligador
        /// </remarks>
        InfoLogBuffer,

        /// <summary>
        /// Entrada: tamanho do amortecedor de mensagens de registo. As mensagens são ajustadas ou truncadas
        /// até este tamanho.
        /// Saída: Quantidade de amortecedor preenchido com as mensagens.
        /// </summary>
        /// <remarks>
        /// Tipo: uint
        /// Aplicação: apenas ao compilador
        /// </remarks>
        InfoLogBufferSizeBytes,

        /// <summary>
        /// Nível de optimização a ser aplicado ao código gerado (0-4), com 4 sendo o maior nível de optimização
        /// que é tomado por defeito.
        /// </summary>
        /// <remarks>
        /// Tipo: uint
        /// Aplicação: apenas ao compilador
        /// </remarks>
        OptimizationLevel,

        /// <summary>
        /// Nenhum valor de opção é requerido. Determina o alvo com base no contexto actual (por defeito).
        /// </summary>
        /// <remarks>
        /// Tipo: nenhum
        /// Aplicação: apenas ao compilador
        /// </remarks>
        TargetFromCudaContext,

        /// <summary>
        /// O alvo é escolhido com base no parâmetro <see cref="ECudaJitTarget"/>. Não pode ser combinado
        /// com <see cref="ECudaJitOption.ThreadsPerBlock"/>.
        /// </summary>
        /// <remarks>
        /// Tipo: ECudaJitTarget
        /// Aplicação: apenas ao compilador
        /// </remarks>
        Target,

        /// <summary>
        /// Especifica a escolha da estratégia de reserva se numa correspondência de cubin não for encontrada.
        /// A escolha é baseada em <see cref="ECudaJitFallback"/>. Esta opção não pode ser usada em comnjunção
        /// com as API CudaLink uma vez que o ligador requer correspondências exactas.
        /// </summary>
        /// /// <remarks>
        /// Tipo: ECudaFallback
        /// Aplicação: apenas ao compilador
        /// </remarks>
        FallbackStrategy,

        /// <summary>
        /// Especifica se se pretende criar informação de depuração na saída -g (0: false, valor por defeito).
        /// </summary>
        /// /// <remarks>
        /// Tipo: int
        /// Aplicação: compilador e ligador
        /// </remarks>
        GenerateDebugInfo,

        /// <summary>
        /// Gera mensagens de registo prolixas (0: falso, valor por defeito).
        /// </summary>
        /// /// <remarks>
        /// Tipo: int
        /// Aplicação: compilador e ligador
        /// </remarks>
        LogVerbose,

        /// <summary>
        /// Gera informação que contém o número da linha (-lineinfo) (0: falso, valor por defeito).
        /// </summary>
        /// /// <remarks>
        /// Tipo: int
        /// Aplicação: apenas ao compilador
        /// </remarks>
        GenerateLineInfo,

        /// <summary>
        /// Especifica se se pretende habilitar o suporte a provisão explicitamente (-dlcm). A escolha é
        /// baseada em <see cref="ECudaJitCacheModeEnum"/>
        /// </summary>
        /// /// <remarks>
        /// Tipo: /// <remarks>
        /// Tipo: nenhum
        /// Aplicação: apenas ao compilador
        /// </remarks>
        /// Aplicação: apenas ao compilador
        /// </remarks>
        CacheMode,

        /// <summary>
        /// Sem descrição de momento.
        /// </summary>
        NumOptions
    }

    /// <summary>
    /// Alvos de compilação.
    /// </summary>
    public enum ECudaJitTarget
    {
        /// <summary>
        /// Classe de dispositivo 1.0.
        /// </summary>
        Compute10 = 10,

        /// <summary>
        /// Classe de dispositivo 1.1.
        /// </summary>
        Compute11 = 11,

        /// <summary>
        /// Classe de dispositivo 1.2.
        /// </summary>
        Compute12 = 12,

        /// <summary>
        /// Classe de dispositivo 1.3.
        /// </summary>
        Compute13 = 13,

        /// <summary>
        /// Classe de dispositivo 2.0.
        /// </summary>
        Compute20 = 20,

        /// <summary>
        /// Classe de dispositivo 2.1.
        /// </summary>
        Compute21 = 21,

        /// <summary>
        /// Classe de dispositivo 3.0.
        /// </summary>
        Compute30 = 30,

        /// <summary>
        /// Classe de dispositivo 3.2.
        /// </summary>
        Compute32 = 32,

        /// <summary>
        /// Classe de dispositivo 3.5.
        /// </summary>
        Compute35 = 35,

        /// <summary>
        /// Classe de dispositivo 3.7.
        /// </summary>
        Compute37 = 37,

        /// <summary>
        /// Classe de dispositivo 5.0.
        /// </summary>
        Compute50 = 50
    }

    /// <summary>
    /// Limites CUDA.
    /// </summary>
    public enum ECudaLimit
    {
        /// <summary>
        /// Tamanho da pilha de linhas de fluxo.
        /// </summary>
        StackSize = 0x00,

        /// <summary>
        /// O tamanho da fila de impressão.
        /// </summary>
        PrintFifoSize = 0x01,

        /// <summary>
        /// Tamanho do acumolador de malloc.
        /// </summary>
        MallocHeapSize = 0x02,

        /// <summary>
        /// Profundidade da sincronização do lançamento no dispositivo.
        /// </summary>
        DevRuntimeSyncDepth = 0x03,

        /// <summary>
        /// Número de lançamentos pendentes no dispositivo em execução.
        /// </summary>
        DevRuntimePendingLaunchCount = 0x04,

        /// <summary>
        /// Sem descrição de momento.
        /// </summary>
        Max
    }

    public enum ECudaMemAttachFlags
    {
        /// <summary>
        /// A memória pode ser acedida a partir de qualquer caudal em qualquer dispositivo.
        /// </summary>
        Global = 0x01,

        /// <summary>
        /// A memória não pode ser acedida em nenhum caudal de qualquer dispositivo.
        /// </summary>
        Host = 0x02,

        /// <summary>
        /// A memória pode apenas ser acedida por um caudal no dispositivo associado.
        /// </summary>
        Single = 0x04
    }

    /// <summary>
    /// Tipos de memória.
    /// </summary>
    public enum ECudaMemoryType
    {
        /// <summary>
        /// Memória de anfitrião.
        /// </summary>
        Host = 0x01,

        /// <summary>
        /// Memória de dispositivo.
        /// </summary>
        Device = 0x02,

        /// <summary>
        /// Memória em vector.
        /// </summary>
        Array = 0x03,

        /// <summary>
        /// Memória unificada de dispositivo ou anfitrião.
        /// </summary>
        Unified = 0x04
    }

    /// <summary>
    /// Informação de apontador.
    /// </summary>
    public enum ECudaPointerAttribute
    {
        /// <summary>
        /// O <see cref="SCudaContext"/> onde o apontador foi alocado ou registado.
        /// </summary>
        Context = 1,

        /// <summary>
        /// O <see cref="ECudaMemoryType"/> descrevendo a localização física de um apontador.
        /// </summary>
        MemoryType =2,

        /// <summary>
        /// O endereço no qual a memória de apontaores pode ser acedida no dispositivo.
        /// </summary>
        DevicePointer = 3,

        /// <summary>
        /// O endereço no qual a memória de apontadores pode ser acedida no anfitrião.
        /// </summary>
        HostPointer = 4,

        /// <summary>
        /// Um par de símbolos para utilizar com a interface de kernel Linux nv-p2p.h.
        /// </summary>
        P2PTokens = 5,

        /// <summary>
        /// Sicroniza qualquer operação de memória síncrona iniciada nesta região.
        /// </summary>
        SyncMomps = 6,

        /// <summary>
        /// Um ID único por processo para uma região de memória alocada.
        /// </summary>
        BufferId = 7,

        /// <summary>
        /// Indica se o apontador se encontra a apontar para memória gerida.
        /// </summary>
        IsManaged = 8
    }

    /// <summary>
    /// Formato do recurso de vista.
    /// </summary>
    public enum ECudaResourceViewFormat
    {
        /// <summary>
        /// Nenhum formato do recurso de vista (usar o formato de recurso subjacente).
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Um canal a utilizar inteiros sem sinal de 8 bits.
        /// </summary>
        Uint1x8 = 0x01,

        /// <summary>
        /// Dois canais a utilizar inteiros sem sinal de 8 bits.
        /// </summary>
        Uint2x8 = 0x02,

        /// <summary>
        /// Quatro canais a utilizar inteiros sem sinal de 8 bits.
        /// </summary>
        Uint4x8 = 0x03,

        /// <summary>
        /// Um canal a utilizar inteiros de 8 bits.
        /// </summary>
        Sint1x8 = 0x04,

        /// <summary>
        /// Dois canais a utilizar inteiros de 8 bits.
        /// </summary>
        Sint2x8 = 0x05,

        /// <summary>
        /// Quatro canais a utilizar inteiros de 8 bits.
        /// </summary>
        Sint4x8 = 0x06,

        /// <summary>
        /// Um canal a utilizar inteiros sem sinal de 16 bits.
        /// </summary>
        Uint1x16 = 0x07,

        /// <summary>
        /// Dois canal a utilizar inteiros sem sinal de 16 bits.
        /// </summary>
        Uint2x16 = 0x08,

        /// <summary>
        /// Quatro canal a utilizar inteiros sem sinal de 16 bits.
        /// </summary>
        Uint4x16 = 0x09,

        /// <summary>
        /// Um canal a utilizar inteiros de 16 bits.
        /// </summary>
        Sint1x16 = 0x0A,

        /// <summary>
        /// Dois canal a utilizar inteiros de 16 bits.
        /// </summary>
        Sint2x16 = 0x0B,

        /// <summary>
        /// Quatro canal a utilizar inteiros de 16 bits.
        /// </summary>
        Sint4x16 = 0x0C,

        /// <summary>
        /// Um canal a utilizar inteiros sem sinal de 32 bits.
        /// </summary>
        Uint1x32 = 0x0D,

        /// <summary>
        /// Dois canal a utilizar inteiros sem sinal de 32 bits.
        /// </summary>
        Uint2x32 = 0x0E,

        /// <summary>
        /// Quatro canal a utilizar inteiros sem sinal de 32 bits.
        /// </summary>
        Uint4x32 = 0x0F,

        /// <summary>
        /// Um canal a utilizar inteiros de 32 bits.
        /// </summary>
        Sint1x32 = 0x10,

        /// <summary>
        /// Dois canal a utilizar inteiros de 32 bits.
        /// </summary>
        Sint2x32 = 0x11,

        /// <summary>
        /// Quatro canal a utilizar inteiros de 32 bits.
        /// </summary>
        Sint4x32 = 0x12,

        /// <summary>
        /// Um canal a utilizar valores de ponto flutuante de 16 bits.
        /// </summary>
        Float1x16 = 0x13,

        /// <summary>
        /// Dois canal a utilizar valores de ponto flutuante de 16 bits.
        /// </summary>
        Float2x16 = 0x14,

        /// <summary>
        /// Quatro canal a utilizar valores de ponto flutuante de 16 bits.
        /// </summary>
        Float4x16 = 0x15,

        /// <summary>
        /// Um canal a utilizar valores de ponto flutuante de 32 bits.
        /// </summary>
        Float1x32 = 0x16,

        /// <summary>
        /// Dois canal a utilizar valores de ponto flutuante de 32 bits.
        /// </summary>
        Float2x32 = 0x17,

        /// <summary>
        /// Quatro canal a utilizar valores de ponto flutuante de 32 bits.
        /// </summary>
        Float4x32 = 0x18,

        /// <summary>
        /// Compressão de bloco 1.
        /// </summary>
        UnsignedBc1 = 0x19,

        /// <summary>
        /// Compressão de bloco 2.
        /// </summary>
        UnsignedBc2 = 0x1A,

        /// <summary>
        /// Compressão de bloco 3.
        /// </summary>
        UnsignedBc3 = 0x1B,

        /// <summary>
        /// Compressão de bloco 4 sem sinal.
        /// </summary>
        UnsignedBc4 = 0x1C,

        /// <summary>
        /// Compressão de bloco 4 com sinal.
        /// </summary>
        SignedBc4 = 0x1D,

        /// <summary>
        /// Compressão de bloco 5 sem sinal.
        /// </summary>
        UnsignedBc5 = 0x1E,

        /// <summary>
        /// Compressão de bloco 5 com sinal.
        /// </summary>
        SignedBc5 = 0x1F,

        /// <summary>
        /// Compressão de bloco 6 com sinal semi ponto flutuante.
        /// </summary>
        UnsignedBc6h = 0x20,

        /// <summary>
        /// Compressão de bloco 6 sem sinal semi ponto flutuante.
        /// </summary>
        SignedBc6h = 0x21,

        /// <summary>
        /// Compressão de bloco 7.
        /// </summary>
        UnsignedBc7 = 0x22
    }

    /// <summary>
    /// Tipos de recursos.
    /// </summary>
    public enum ECudaResourceType
    {
        /// <summary>
        /// Recurso de vector.
        /// </summary>
        Array = 0x00,

        /// <summary>
        /// Recurso de vector mipmapped.
        /// </summary>
        MipmappedArray = 0x01,

        /// <summary>
        /// Recurso linear.
        /// </summary>
        Linear = 0x02,

        /// <summary>
        /// Recurso de passo 2D.
        /// </summary>
        Pitch2D = 0x03
    }

    /// <summary>
    /// Configurações de memória partilhada.
    /// </summary>
    public enum ECudaSharedConfig
    {
        /// <summary>
        /// Estabelece o tamanho do banco de memória por defeito.
        /// </summary>
        DefaultBankSize = 0x00,

        /// <summary>
        /// Estabelece a largura do banco de memória em 4 bytes.
        /// </summary>
        FourByteBankSize = 0x01,

        /// <summary>
        /// Estabelece a largura do banco de memória em 8 bytes.
        /// </summary>
        EightByteBankSize = 0x02
    }

    /// <summary>
    /// Marcas de caudal.
    /// </summary>
    public enum ECudaStreanFlags
    {
        /// <summary>
        /// Marca de caudal por defeito.
        /// </summary>
        Default = 0x0,

        /// <summary>
        /// O caudal não sincroniza com o caudal 0 (o caudal nulo).
        /// </summary>
        NonBlocking = 0x1
    }
}
