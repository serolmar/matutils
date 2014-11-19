namespace Utilities.Cuda
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define os possíveis resultados de uma chamada CUDA.
    /// </summary>
    public enum CudaResult
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

    }
}
