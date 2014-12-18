namespace Utilities.Cuda
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Permite fazer ligação estática às funções diponibilizadas pela API CUDA.
    /// </summary>
    public static class CudaApi
    {
        /// <summary>
        /// O nome da dll que exporta a API CUDA.
        /// </summary>
        internal const string DllName = "nvcuda";

        #region Gestão dos erros

        /// <summary>
        /// Obtém o nome do erro CUDA.
        /// </summary>
        /// <param name="error">O erro.</param>
        /// <param name="ptrStr">Apontador para um vector de mensagens de texto.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/> ou <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuGetErrorName")]
        public static extern ECudaResult CudaGetErrorName(
            ECudaResult error,
            ref IntPtr ptrStr);

        /// <summary>
        /// Obtém a descrição de um código de erro.
        /// </summary>
        /// <param name="error">O erro.</param>
        /// <param name="ptrStr">O apontador para a descrição.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/> ou <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuGetErrorString")]
        public static extern ECudaResult CudaGetErrorString(
            ECudaResult error,
            ref IntPtr ptrStr);

        #endregion Gestão dos erros

        #region Inicialização

        /// <summary>
        /// Inicializa o condutor de CUDA e de ser chamda antes de qualquer outra função da API. Actualmente,
        /// o parâmetro das marcas tem de ser 0. Se <see cref="CudaApi.CudaInit"/> não for chamada qualquer
        /// função da API retornará <see cref="ECudaResult.CudaErrorNotInitialized"/>.
        /// </summary>
        /// <param name="flags">As marcas de inicialização (deverá receber o valor 0).</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/> ou
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuInit")]
        public static extern ECudaResult CudaInit(uint flags);

        #endregion Inicialização

        #region Gestão de versões

        /// <summary>
        /// Retorna o número da versão do condutor CUDA instalado. Esta função retorna automaticamente
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/> se o argumento driverVersion for nulo.
        /// </summary>
        /// <param name="driverVersion">Irá conter o número da versão.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/> e <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuDriverGetVersion")]
        public static extern ECudaResult CudaDriverGetVersion(ref int driverVersion);

        #endregion Gestão de versões

        #region Gestão de dispositivos

        /// <summary>
        /// Retorna um manuseador para um dispositivo.
        /// </summary>
        /// <param name="device">O manuseador para o dispositivo returnado.</param>
        /// <param name="ordinal">O número do dispositivo do qual se pretende o manuseador.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuDeviceGet")]
        public static extern ECudaResult CudaDeviceGet(ref int device, int ordinal);

        /// <summary>
        /// Retorna informação sobre o dispositivo.
        /// </summary>
        /// <param name="pi">O valor do atributo do dispositivo retornado.</param>
        /// <param name="attrib">O atributo a ser consultado.</param>
        /// <param name="device">O manuseador para o dispositivo.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuDeviceGetAttribute")]
        public static extern ECudaResult CudaDeviceGetAttribute(
            ref int pi, 
            ECudaDeviceAttr attrib, 
            int device);

        /// <summary>
        /// Retorna o número de dispositivos capazes computação CUDA.
        /// </summary>
        /// <param name="count">O número de dispositivos retornado.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuDeviceGetCount")]
        public static extern ECudaResult CudaDeviceGetCount(ref int count);

        /// <summary>
        /// Retorna um valor textual que identifica o dispositivo.
        /// </summary>
        /// <remarks>
        /// Note-se que a função é ordenada por intermédio do tipo <see cref="System.Text.StringBuilder"/>.
        /// Poderia ser por <see cref="T:System.Char[]"/>. No entanto, a utilização de um objecto do tipo
        /// <see cref="System.Text.StringBuilder"/> permite construir directamente um objecto do tipo 
        /// <see cref="System.String"/> a partir do apontador no código não gerido, admitindo que se trata
        /// de uma cadeia de carácteres com terminação nula. No outro caso, seria necessário instanciar um
        /// vector de carácteres com o tamanho correcto para acomodar o texto.
        /// </remarks>
        /// <param name="name">O nome do dispositivo retornado.</param>
        /// <param name="len">O comprimento máximo a ser guardado no nome.</param>
        /// <param name="dev">O dispositivo do qual se pretende a identificação.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuDeviceGetName", CharSet= CharSet.Ansi)]
        public static extern ECudaResult CudaDeviceGetName(
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder name, 
            int len, 
            int dev);

        /// <summary>
        /// Retorna a memória total disponível no dispositivo.
        /// </summary>
        /// <param name="bytes">Memória disponível no dispositivo em bytes.</param>
        /// <param name="dev">O manuseador para o dispositivo.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuDeviceTotalMem")]
        public static extern ECudaResult CudaDeviceTotalMem(ref SizeT bytes, int dev);

        /// <summary>
        /// Retorna as capacidades computacionais do dispositivo.
        /// </summary>
        /// <remarks>
        /// A partir da versão CUDA 5.0, utilizar <see cref="CudaApi.CudaDeviceGetAttribute"/>.
        /// </remarks>
        /// <param name="major">O maior número da revisão.</param>
        /// <param name="minor">O menor número da revisão.</param>
        /// <param name="dev">O manuseador do dispositivo.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [Obsolete("Deprecated as of CUDA 5.0.")]
        [DllImport(DllName, EntryPoint = "cuDeviceComputeCapability")]
        public static extern ECudaResult CudaDeviceComputeCapability(
            ref int major,
            ref int minor,
            int dev);

        /// <summary>
        /// Retorna as propriedades do dispositivo especificado.
        /// </summary>
        /// <remarks>
        /// A partir da versão CUDA 5.0, utilizar <see cref="CudaApi.CudaDeviceGetAttribute"/>.
        /// </remarks>
        /// <param name="prop">A propriedade do dispositivo retornada.</param>
        /// <param name="dev">O manuseador do dispositivo.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [Obsolete("Deprecated as of CUDA 5.0.")]
        [DllImport(DllName, EntryPoint = "cuDeviceGetProperties")]
        public static extern ECudaResult CudaDeviceGetProperties(ref SCudaDevProp prop, int dev);

        #endregion Gestão de dispositivos

        #region Gestão de contextos

        /// <summary>
        /// Cria um contexto CUDA.
        /// </summary>
        /// <remarks>
        /// Cria um novo contexto CUDA e associa-o com a linha de fluxo de chamada. O contexto é criado com a
        /// contagem de utilização inicializada a 1 e o responsável pela chamada a 
        /// <see cref="CudaApi.CudaCtxCreate"/> deve também chanar <see cref="CudaApi.CudaCtxDestroy"/> ou
        /// quando estiver terminada a utilização do contexto. Se o contexto já for o contexto actual para a
        /// linha de fluxo corrente, este é suplantado pelo contexto recém criado e deve ser subsequentemente
        /// restaurado por intermédio da chamada à função <see cref="CudaApi.CudaCtxPopCurrent"/>. Os três LSBs
        /// do parâmetro das marcas podem ser utilizados para controlar a linha de fluxo do sistema operativo
        /// que é proprietária do contexto no instante de chamada da Api, interage com o calenderizador do
        /// sistema operativo quando se encontra a aguardar resultados do GPU. Apenas uma das marcas de
        /// calenderização pode ser atribuída durante a criação do contexto.
        /// Os valores permitidos para o parâmetro das marcas são:
        /// <list type="bullet">
        /// <term><see cref="ECudaContextFlags.SchedAuto"/></term>
        /// <description>
        /// O valor por defeito se o parâmetro das marcas receber o valor 0, utiliza uma heurística baseada
        /// no número de contextos CUDA activos no processo C e o número de processadores lógicos no sistema P.
        /// Se C>P, então o ambiente CUDA remete para outras linhas de fluxo enquanto espera pelo GPU, caso
        /// contário o ambiente CUDA não remete enquanto se encontra à espera de resultados e roda activamente
        /// no processador.
        /// </description>
        /// </list>
        /// <list type="bullet">
        /// <term><see cref="ECudaContextFlags.SchedSpin"/></term>
        /// <description>
        /// Instrui o ambiente CUDA para rodar activamente enquanto se encontrar à espera de resultados do
        /// GPU, mas pode diminuir o desempenho das linhas de fluxo do CPU se estas se encontrarem a realizar
        /// trabalho em paralelo com as linhas de fluxo CUDA.
        /// </description>
        /// </list>
        /// <list type="bullet">
        /// <term><see cref="ECudaContextFlags.ScheddYield"/></term>
        /// <description>
        /// Instrui o ambiente CUDA para remeter a sua linha de fluxo enquanto espera resultados do GPU. Isto pode
        /// aumentar a latência enquanto se encontrar à espera do GPU mas pode aumentar o desempenho das linhas
        /// de fluxo do CPU que se encontram a operar em paralelo com as de CUDA.
        /// </description>
        /// </list>
        /// <list type="bullet">
        /// <term><see cref="ECudaContextFlags.SchedBlockingSync"/></term>
        /// <description>
        /// Instrui o ambiente CUDA para bloquear a linha de fluxo do CPU numa primitiva de sincronização enquanto
        /// se enconta à espera que o GPU termine.
        /// </description>
        /// </list>
        /// <list type="bullet">
        /// <term><see cref="ECudaContextFlags.BlockingSync"/></term>
        /// <description>
        /// Instrui o ambiente CUDA para bloquear a linha de fluxo do CPU numa primitiva de sincronização enquanto
        /// se encontra à espera que o GPU termine. Esta marca encontra-se obsoleta desde a versão CUDA 4.0 e
        /// foi subsituída pela <see cref="ECudaContextFlags.SchedBlockingSync"/>.
        /// </description>
        /// </list>
        /// <list type="bullet">
        /// <term><see cref="ECudaContextFlags.MapHost"/></term>
        /// <description>
        /// Instrui o ambiente CUDA para suportar alocações mapeadas ao anfitrião. Esta marca deverá ser
        /// atribuída se se pretender mapear memória do anfitrião seleccionada para ser acedida pelo GPU.
        /// </description>
        /// </list>
        /// <list type="bullet">
        /// <term><see cref="ECudaContextFlags.LmemResizeToMax"/></term>
        /// <description>
        /// Instrui o ambiente CUDA para não reduzir a memória local após uma alteração da memória local para
        /// um kernel. Isto pode prevenir o entulhamento por alocações de memória locais durante o lançamento
        /// de vários kernels com uma grande utilização de memória local sob a pena de aumentar potencialmente
        /// a utilização de memória.
        /// </description>
        /// </list>
        /// A criação do contexto irã falhar com a mensagem <see cref="ECudaResult.CudaErrorUnknown"/> se o modo de
        /// computação do dispositivo for <see cref="ECudaComputeMode.Prohibited"/>. De um modo semelhante,
        /// a criação do contexto também falhará com o mesmo erro se o modo de computação para o dispositivo for
        /// <see cref="ECudaComputeMode.Exclusive"/> e já existir um contexto activo no dispositivo. A função
        /// <see cref="CudaApi.CudaDeviceGetAttribute"/> pode ser usada em conjunção com
        /// <see cref="ECudaDeviceAttr.ComputeMode"/> para determinar o modo de computaçaõ do dispositivo. A
        /// ferramenta nvidia-simi dpode ser usada para estabelecer o modo de computação dos dispositivos.
        /// </remarks>
        /// <param name="pctx">O manuseador para o contexto criado.</param>
        /// <param name="flags">As marcas para criação de contexto.</param>
        /// <param name="dev">O manuseador para o dispositivo.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxCreate")]
        public static extern ECudaResult CudaCtxCreate(
            ref SCudaContext pctx, 
            ECudaContextFlags flags, 
            int dev);

        /// <summary>
        /// Destrói um contexto CUDA.
        /// </summary>
        /// <param name="ctx">O contexto a ser destruído.</param>
        /// <returns>
        /// <remarks>
        /// Destói o contexto CUDA especificado pelo parâmetro ctx. O contexto será destruído
        /// independentemene do número de linhas de fluxo nas quais se encontra inserido. É da responsabilidade
        /// da função de chamada de se assegurar que não se dão chamadas à API enquanto se está a executar
        /// <see cref="CudaApi.CudaCtxDestroy"/>.
        /// Se o contexto for o corrente para a linha de fluxo de chamada então o contexto também será removido
        /// da pilha de contextos corrente (como em <see cref="CudaApi.CudaCtxPopCurrent"/>). Se o contexto for
        /// corrente para outras linhas de fluxo, então o contexto continuará a ser aí o corrente e a tentativa
        /// de o aceder irá resultar no erro <see cref="ECudaResult.CudaErrorContextIsDestroyed"/>.
        /// </remarks>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxDestroy")]
        public static extern ECudaResult CudaCtxDestroy(SCudaContext ctx);

        /// <summary>
        /// Obtém a versão da API do contexto.
        /// </summary>
        /// <remarks>
        /// Retorna num número de versão no parâmetro version que corresponde às capacidades do
        /// contexto (exemplo, 3010 ou 3020) cuja livraria os utilizadores podem direccionar os executadores
        /// para uma versão específica da API. Por exemplo, é válida para a versão 3020 enquanto a versão
        /// é 4020.
        /// </remarks>
        /// <param name="ctx">O contexto.</param>
        /// <param name="version">A versão.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxGetApiVersion")]
        public static extern ECudaResult CudaCtxGetApiVersion(SCudaContext ctx, ref uint version);

        /// <summary>
        /// Retorna a configuração da provisão.
        /// </summary>
        /// <remarks>
        /// Em dispositivos onde a provisão L1 e a memória partilhada usam os mesmos recursos, esta função
        /// retorna em pconfig a provisão preferida para o contexto actual. Isto é apenas uma
        /// preferência. O condutor irá utilizar a configuração requerida se possível mas é livre de esolher
        /// uma configuração diferente se necessário para executar as funções.
        /// Irá retornar o valor <see cref="ECudaFuncCache.None"/> em dispositivos onde o tamanho da provisão
        /// L1 e da memória partilhada são fixos.
        /// As configurações de provisão suportadas são:
        /// <list type="bullet">
        /// <item><see cref="ECudaFuncCache.None"/></item>
        /// <description>
        /// Nenhuma preferência para memória partilhada.
        /// </description>
        /// <item><see cref="ECudaFuncCache.Shared"/></item>
        /// <description>
        /// Preferência por uma grande quantidade de memória partilhada e menor provisão L1.
        /// </description>
        /// <item><see cref="ECudaFuncCache.L1"/></item>
        /// <description>
        /// Preferência por uma grande quantidade de provisão L1 e menor memória partilhada.
        /// </description>
        /// <item><see cref="ECudaFuncCache.Equal"/></item>
        /// <description>
        /// Preferência por iguais quantidades de memória partilhada e provisão L1.
        /// </description>
        /// </list>
        /// </remarks>
        /// <param name="pconfig">A configuração da provisão.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxGetCacheConfig")]
        public static extern ECudaResult CudaCtxGetCacheConfig(ref ECudaFuncCache pconfig);

        /// <summary>
        /// Retorna o contexto CUDA ligado à linha de fluxo corrente.
        /// </summary>
        /// <remarks>
        /// Se nenhum contexto estiver ligado à linha de fluxo corrente então pctx recebe o valor
        /// nulo e o valor <see cref="ECudaResult.CudaSuccess"/> é retornado.
        /// </remarks>
        /// <param name="pctx">O manuseador do contexto.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxGetCurrent")]
        public static extern ECudaResult CudaCtxGetCurrent(ref SCudaContext pctx);

        /// <summary>
        /// Retorna o ID do dispositivo para o contexto actual.
        /// </summary>
        /// <param name="device">O dispositivo.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxGetDevice")]
        public static extern ECudaResult CudaCtxGetDevice(ref int device);

        /// <summary>
        /// Retorna os limites dos recursos.
        /// </summary>
        /// <remarks>
        /// Retorna em pvalue o tamanho actual do limit. Os valores do limite
        /// suportados são:
        /// <list type="bullet">
        /// <item><see cref="ECudaLimit.StackSize"/></item>
        /// <description>
        /// Tamanho de pilha em bytes de cada linha de fluxo GPU.
        /// </description>
        /// <item><see cref="ECudaLimit.PrintFifoSize"/></item>
        /// <description>
        /// Tamanho em bytes da fila usada pela função de sistema "printf()" no dispositivo.
        /// </description>
        /// <item><see cref="ECudaLimit.MallocHeapSize"/></item>
        /// <description>
        /// Tamanho em bytes do acumulador utilizado pelas funções de sistema "malloc()" e "freee()" no
        /// dispositivo.
        /// </description>
        /// <item><see cref="ECudaLimit.DevRuntimeSyncDepth"/></item>
        /// <description>
        /// Profundidade máxima da grelha na qual uma linha de fluxo pode emitir uma chamada no dispositivo
        /// em tempo de execução "cudaDeviceSynchronize()" para esperar que as grelhas dependentes terminem.
        /// </description>
        /// <item><see cref="ECudaLimit.DevRuntimePendingLaunchCount"/></item>
        /// <description>
        /// Número máximo de lançamentos de dispositivo prominentes que podem ser realizados a partir do contexto
        /// corrente.
        /// </description>
        /// </list>
        /// </remarks>
        /// <param name="pvalue">O valor do tamanho do limite.</param>
        /// <param name="limit">O limite a ser consultado.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxGetLimit")]
        public static extern ECudaResult CudaCtxGetLimit(ref SizeT pvalue, ECudaLimit limit);

        /// <summary>
        /// Retorna a confifuração de memória partilhada para o contexto corrente.
        /// </summary>
        /// <param name="pconfig">A configuração da memória.</param>
        /// <returns>
        /// <remarks>
        /// A função retorna em pconfig o tamanho actual dos bancos de memória partilhada no
        /// contexto actual. Nos dispositivos onde os bancos de memória partilhada são configuráveis,
        /// <see cref="CudaApi.CudaCtxSetSharedMemConfig"/> pode ser usada para alterar esta configuração
        /// de tal modo que todos os lançamentos de kernel subsequentes utilizarão, por defeito, o novo
        /// tamanho do banco. Quando <see cref="CudaApi.CudaCtxGetSharedMemConfig"/> é chamada em dispositivos
        /// sem memória partilhada configurável, retornará o tamanho fixo dos bancos.
        /// </remarks>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxGetSharedMemConfig")]
        public static extern ECudaResult CudaCtxGetSharedMemConfig(ref ECudaSharedConfig pconfig);

        /// <summary>
        /// Retorna valores numéricos que correspondem às prioridades máxima e mínima de caudal.
        /// </summary>
        /// <remarks>
        /// Retorna em leastPriority e greatestPriority os valores numéricos que
        /// correspondem às prioridades de caudal mínima e máxima. O intervalo de prioridades de caudal é dado
        /// por [leastPriority, greatestPriority]. Se o utilizador tentar criar um
        /// caudal com um valor de prioridade fora do intervalo como se encontra especificado pela API, a
        /// prioridade é automaticamente reestabelecida em leastPriority ou
        /// greatestPriority respectivamente. Ver 
        /// <see cref="CudaApi.CudaStreamCreateWithPriority"/> para mais detalhes sobre a criação de caudais
        /// com prioridades. O valor nulo pode ser passado em leastPriority ou
        /// greatestPriority se esse valor não for desejado.
        /// A função irá retornar zero em ambos leastPriority e greatestPriority se o dispositivo do 
        /// contexto actual não suportar prioridades de caudal (ver <see cref="CudaApi.CudaDeviceGetAttribute"/>).
        /// </remarks>
        /// <param name="leastPriority">A prioridade mínima.</param>
        /// <param name="greatestPriority">A prioridade máxima.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxGetStreamPriorityRange")]
        public static extern ECudaResult CudaCtxGetStreamPriorityRange(
            ref int leastPriority, 
            ref int greatestPriority);

        /// <summary>
        /// Extrai o contexto CUDA actual da linha de fluxo do CPU corrente.
        /// </summary>
        /// <remarks>
        /// Extrai o contexto CUDA actual da linha de fluxo do CPU corrente e passa o manuseador do contexto
        /// antigo no parâmetro pctx. Este contexto pode ser tornado corrente numa linha de fluxo
        /// do CPU diferente por intermédio da chamada à função <see cref="CudaApi.CudaCtxPushCurrent"/>.
        /// Se um contexto já era corrente numa linha de fluxo do CPU antes da chamada às funções
        /// <see cref="CudaApi.CudaCtxCreate"/> ou <see cref="CudaApi.CudaCtxPushCurrent"/>, esta fução torna
        /// novamente corrente este contexto corrente para a linha de fluxo do CPU.
        /// </remarks>
        /// <param name="pctx">O contexto.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxPopCurrent")]
        public static extern ECudaResult CudaCtxPopCurrent(ref SCudaContext pctx);

        /// <summary>
        /// Insere um contexto na linha de fluxo do CPU corrente.
        /// </summary>
        /// <remarks>
        /// Insere o contexto proporcionado na pilha de contextos da linha de fluxo do CPU. O contexto
        /// especificado passa a ser o contexto corrente dessa linha de fluxo de modo que todas as funções CUDA
        /// que operam no contexto são afectadas.
        /// </remarks>
        /// <param name="ctx">O contexto.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxPushCurrent")]
        public static extern ECudaResult CudaCtxPushCurrent(SCudaContext ctx);

        /// <summary>
        /// Atribui a configuração de provisão preferencial para o contexto corrente.
        /// </summary>
        /// <remarks>
        /// Nos dispositivos onde a provisão L1 e a memória usam os mesmos recursos físicos, isto atribui
        /// por intermédio do parâmetro config a configuração de provisão preferencial para o
        /// contexto actual. Trata-se apenas de uma preferência. O condutor irá utilizar a configuração
        /// requrerida se possível, mas é livre de escolher uma diferente caso seja necessária para executar
        /// a função. Qualquer atribuição de função preferencial via <see cref="CudaApi.CudaFuncSetCacheConfig"/>
        /// será preferida sobre qualquer atribuição possível no contexto. Atribuindo o valor
        /// <see cref="ECudaFuncCache.None"/> irá causar com que os lançamentos de kernel subsequentes prefiram
        /// não alterar a configuração de provisão a menos que seja requerido pelo lançamento do kernel.
        /// Esta atribuição não tem qualquer efeito em dispositivos onde o tamanho da provisão L1 e da memória
        /// partilhada são fixo. Lançando um kernel com uma preferência diferente da actual pode inserir um
        /// ponto de sincronizaçao no lado do dispsitivo.
        /// As configurações de provisão suportadas são:
        /// <list type="bullet">
        /// <item><see cref="ECudaFuncCache.None"/></item>
        /// <description>
        /// Nenhuma preferência para provisão L1 ou memória partilhada (por defeito).
        /// </description>
        /// <item><see cref="ECudaFuncCache.Shared"/></item>
        /// <description>
        /// Preferência por maior memória partilhada e menor provisão L1.
        /// </description>
        /// <item><see cref="ECudaFuncCache.L1"/></item>
        /// <description>
        /// Preferência por maior provisão L1 e menor memória partilhada.
        /// </description>
        /// <item><see cref="ECudaFuncCache.Equal"/></item>
        /// <description>
        /// Preferência por iguais quantidades entre provisão L1 e memória partilhada.
        /// </description>
        /// </list>
        /// </remarks>
        /// <param name="config">A configuração de provisão requerida.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxSetCacheConfig")]
        public static extern ECudaResult CudaCtxSetCacheConfig(ECudaFuncCache config);

        /// <summary>
        /// Liga o contexto CUDA especificado à linha de fluxo de chamada do CPU.
        /// </summary>
        /// <remarks>
        /// Se o parâmetro ctx for nulo então o contexto CUDA previamente ligado à linha de fluxo
        /// de chamada no CPU é desligado e o valor <see cref="ECudaResult.CudaSuccess"/> é retornado.
        /// Se existir um contexto CUDA na pilha da linha de fluxo do CPU de chamada, este irá substituir o topo
        /// dessa pilha com ctx. Sendo o valor de ctx nulo, isto equivale a extrair
        /// o topo da pilha de contexto da linha de fluxo do CPU de chamada (ou a no-op se a pilha da
        /// linha CPU de chamada se encontrar vazia). 
        /// </remarks>
        /// <param name="ctx">O contexto a ser ligado à linha de fluxo de chamada do CPU.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxSetCurrent")]
        public static extern ECudaResult CudaCtxSetCurrent(SCudaContext ctx);

        /// <summary>
        /// Atribui os limites dos recursos.
        /// </summary>
        /// <remarks>
        /// A atribuição de value a limit consiste num requisito da aplicação para
        /// alterar o limite actual mantido pelo contexto. O condutor é livre de modificar o valor requerido
        /// de modo a ser coadunado com requisitos h/w (isto pode estar relacionado com a reatribuição a valores
        /// mínimos ou máximos, arredondamento para o tamanho de elemento mais próximo, etc). A aplicação pode
        /// usar <see cref="CudaApi.CudaCtxGetLimit"/> para determinar exactamente o valor ao qual foi atribuído
        /// o limite.
        /// A atribuição de cada um dos limites <see cref="ECudaLimit"/> tem as suas restrições específicas que
        /// serão brevemente resumidas de seguida.
        /// <list type="bullet">
        /// <item><see cref="ECudaLimit.StackSize"/></item>
        /// <description>
        /// Controla o tamanho da pilha em bytes de cada linha de fluxo GPU. Este limite é apenas aplicável a
        /// dispositivos com capacidade computacional 2.0 e maior. Tentanto atribuir este limite em dispositivos
        /// com capacidade computacional menor que 2.0 irá resultar no retorno do erro
        /// <see cref="ECudaResult.CudaErrorUnsupportedLimit"/>.
        /// </description>
        /// <item><see cref="ECudaLimit.PrintFifoSize"/></item>
        /// <description>
        /// Controla o tamanho em bytes da fila usada pela chamada de sistema da função "printf()". A atribuição 
        /// do limite <see cref="ECudaLimit.PrintFifoSize"/> deve ser realizada antes do lançamento do kernel que
        /// usa a função de sistema "printf()", caso contrário será retornado o erro
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>. Este limite é apenas aplicável a dispositivos com
        /// capacidade computacional 2.0 ou superior. A tentativa de atribuição deste limite em dispositivos com
        /// capacidade computacional inferior a 2.0 irá resultar no retorno do erro
        /// <see cref="ECudaResult.CudaErrorUnsupportedLimit"/>.
        /// </description>
        /// <item><see cref="ECudaLimit.MallocHeapSize"/></item>
        /// <description>
        /// Controla o tamanho em bytes do acumulador utilizado pelas chamadas Às funções de sistema "malloc()" e
        /// "free()". A atribuição de <see cref="ECudaLimit.MallocHeapSize"/> deve ser realizada antes do 
        /// lançamento de qualquer kernel que utilize as funções de sistema "malloc()" ou "free()" no dispositivo,
        /// caso contrário será retornado o erro <see cref="ECudaResult.CudaErrorInvalidValue"/>. Este limite é
        /// apenas aplicado em dispositivos com capacidade computacional 2.0 ou superior. A tentativa de
        /// atribuição deste limite em dispositivos com capacidade computacional inferior a 2.0 irá resultar no
        /// retorno do erro <see cref="ECudaResult.CudaErrorUnsupportedLimit"/>.
        /// contrário
        /// </description>
        /// <item><see cref="ECudaLimit.DevRuntimeSyncDepth"/></item>
        /// <description>
        /// Controla a profundidade aninhada máxima de uma grelha na qual uma linha de fluxo pode chamar com
        /// segurança a função "cudaDeviceSynchronize()". A atribuição deste limite deve ser realizada antes de
        /// quaqluer lançamento de kernel que utilize o dispositivo e chame a função "cudaDeviceSynchronize()"
        /// acima da profundidade de sincronização por defeito, dois níveis de grelha. Chamadas à função
        /// "cudaDeviceSynchronize()" irão falhar com o código de erro "cudaErrorSyncDepthExceeded" se a 
        /// limitação for violada. Este limite pode ser estabelecido com um valor menor do que o que se encontra
        /// por defeito ou até um máximo de profundidade de lançamento igual a 24. Quando é estabelecido este
        /// limite, convém notar que níveis adicionais de profundidade de sincronização requerem ao condutor
        /// que reserve grandes quantidaades de memória que deixam de ficar disponíveis para as alocações do
        /// utilizador. Se esta reserva de memória de dispositivo falhar, <see cref="CudaApi.CudaCtxSetLimit"/>
        /// irá retornar <see cref="ECudaResult.CudaErrorOutOfMemory"/> ee o limite pode ser reestabelecido
        /// num valor menor. Este limite é apenas aplicável a dispositivos com capacidade computacional 3.5 e
        /// superior. A tentativa de atribuir este limite em dispositivos com capacidade computacional inferior
        /// a 3.5 irá resultar no retorno do erro <see cref="ECudaResult.CudaErrorUnsupportedLimit"/>.
        /// </description>
        /// <item><see cref="ECudaLimit.DevRuntimePendingLaunchCount"/></item>
        /// <description>
        /// Controla o número máximo de lançamentos prominentes de dispositivos em execução que podem ser
        /// realizados a partir do contexto actual. Uma grelha é prominente do ponto de lançamento até que a
        /// grelha tenha ficado completa. Lançamentos em tempo de execução do dispositivo que violarem esta
        /// limitação falham e retornam "cudaErrorLaunchPendingCountExceed" quando é chamada a função
        /// "cudaGetLastError()" após o lançamento. Se mais lançamentos pendentes dos que está definido por
        /// defeito (2018 lançamentos) são necessários para um módulo que utilize o dispositivo em execução,
        /// este limite pode ser aumentado. Convém notar que sendo capaz de suster lançamentos pendentes 
        /// adicionais, será requerido ao condutor a reserva de grandes quantidades de memória de dispositivo
        /// que não poderá ser utilizada por alocações do utilizador. Se estas reservas falharem,
        /// <see cref="CudaApi.CudaCtxSetLimit"/> retornará o erro <see cref="ECudaResult.CudaErrorOutOfMemory"/>
        /// e o limite pode ser reatribuído para um menor valor. Este limite é apenas aplicável em dispositivos
        /// com capacidade computacional 3.5 e superior. A tentativa de atribuir este limite em dispositivos com
        /// capacidade computacional inferior a 3.5 irá resultar no retorno do erro 
        /// <see cref="ECudaResult.CudaErrorUnsupportedLimit"/>.
        /// </description>
        /// </list>
        /// </remarks>
        /// <param name="limit">O limite a ser atribuído.</param>
        /// <param name="value">O valor a atribuir.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxSetLimit")]
        public static extern ECudaResult CudaCtxSetLimit(ECudaLimit limit, SizeT value);

        /// <summary>
        /// Atribui a configuração de memória partilhada ao contexto corrente.
        /// </summary>
        /// <remarks>
        /// Em disposivos com bancos de memória reconfiguráveis, esta função irá atribuir o tamanho do banco
        /// de memória que é usado em lançamentos subsequentes do kernel. A alteração da configuração entre
        /// lançamentos pode introduzir um ponto de sincronização no lado do dispositivo entre os dois
        /// lançamentos. Alterando o tamanho do banco da memória partilhada não irá aumentar a utilização de
        /// memória partilhada ou afectar a ocupância dos kernels, mas pode ter efeitos consideráveis no
        /// desempenho. Maiores tamanhos dos bancos irão permitir um maior potencial de largura de banda para a
        /// memória partilhada mas irá alterar que tipo de acessos à memória partilhada irão resultar em
        /// conflitos de bancos. Esta função não terá efeito em dispositivos com um tamanho do banco de memória
        /// fixo. As configurações de banco suportadas são:
        /// <list type="bullet">
        /// <item><see cref="ECudaSharedConfig.DefaultBankSize"/></item>
        /// <description>
        /// Atribui a largura do banco para o valor inicial por defeito (actualmente, quatro bytes).
        /// </description>
        /// <item><see cref="ECudaSharedConfig.FourByteBankSize"/></item>
        /// <description>
        /// Atribui a largura do banco de memória partilhada para os quatro bytes nativos.
        /// </description>
        /// <item><see cref="ECudaSharedConfig.EightByteBankSize"/></item>
        /// <description>
        /// Atribui a largura do banco de memória partilhada para os oito bytes nativos.
        /// </description>
        /// </list>
        /// </remarks>
        /// <param name="config">A configuração de memória partilhada.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxSetSharedMemConfig")]
        public static extern ECudaResult CudaCtxSetSharedMemConfig(ECudaSharedConfig config);

        /// <summary>
        /// Bloqueia até que as tarefas do contexto completem.
        /// </summary>
        /// <remarks>
        /// Bloqueia até que o dispositivo tenha completado todas as tarefas precedentes. Retorna um erro
        /// se uma das tarefas falhar. Se o contxto foi criado com a marca 
        /// <see cref="ECudaContextFlags.SchedBlockingSync"/>, a linha de fluxo do CPU irá bloquear até que
        /// o contexto do GPU tenha terminado o seu trabalho.
        /// </remarks>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxSynchronize")]
        public static extern ECudaResult CudaCtxSynchronize();

        /// <summary>
        /// Incrementa a contagem de utilização de contexto.
        /// </summary>
        /// <param name="pctx">O manueseador do contexto actual retornado.</param>
        /// <param name="flgas">As marcas de anexação (deve ser 0).</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [Obsolete("This function is deprecated and should not be used.")]
        [DllImport(DllName, EntryPoint = "cuCtxAttach")]
        public static extern ECudaResult CudaCtxAttach(ref SCudaContext pctx, uint flgas);

        /// <summary>
        /// Decrementa a contagem de utilzação de contexto.
        /// </summary>
        /// <param name="ctx">O contexto.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>.
        /// </returns>
        [Obsolete("This function is deprecated and should not be used.")]
        [DllImport(DllName, EntryPoint = "cuCtxDetach")]
        public static extern ECudaResult CudaCtxDetach(SCudaContext ctx);

        #endregion Gestão de contextos

        #region Gestão de módulos
        
        /// <summary>
        /// Adiciona dados a uma invocação pendente do ligador.
        /// </summary>
        /// <param name="state">Uma acção do ligador pendente.</param>
        /// <param name="type">O tipo dos dados de entrada do ligador.</param>
        /// <param name="data">Os dados de entrada. O vector PTX deve ser terminado com um nulo.</param>
        /// <param name="size">O tamanho dos dados de entrada.</param>
        /// <param name="name">
        /// Um nome adicional para o conjunto de dados como deve aparecer nos diários de mensagens.
        /// </param>
        /// <param name="numOptions">O tamanho das opções.</param>
        /// <param name="options">
        /// As opçóes a serem aplicadas apenas para estes dados de entrada. Sobrecarrega as opções de
        /// <see cref="CudaApi.CudaLinkCreate"/>.
        /// </param>
        /// <param name="optionValues">Vector de opçoes de valores.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidImage"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidPtx"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorNoBinaryForGpu"/>
        /// </returns>
        [DllImport(DllName, EntryPoint="cuLinkAddData")]
        public static extern ECudaResult CudaLinkAddData(
            SCudaLinkState state,
            ECudaJitInputType type,
            IntPtr data,
            SizeT size,
            string name,
            uint numOptions,
            [Out] ECudaJitOption[] options,
            IntPtr optionValues);

        /// <summary>
        /// Adiciona um ficheiro de entrada a uma incovação pendente do ligador.
        /// </summary>
        /// <param name="state">Uma acção do ligador pendente.</param>
        /// <param name="type">O tipo dos dados de entrada.</param>
        /// <param name="path">O caminho para o ficheiro.</param>
        /// <param name="numbOptions">O tamanho das opções.</param>
        /// <param name="options">As opções a serem tomadas apenas para estes dados de entrada.</param>
        /// <param name="optionValues">Vector de valores de opções, cada uma promovida a void*.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorFileNotFound"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidImage"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidPtx"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorNoBinaryForGpu"/>
        /// </returns>
        [DllImport(DllName, EntryPoint="cuLinkAddFile")]
        public static extern ECudaResult CudaLinkAddFile(
            SCudaLinkState state,
            ECudaJitInputType type,
            string path,
            uint numbOptions,
            [Out] ECudaJitOption[] options,
            IntPtr optionValues);

        /// <summary>
        /// Completa uma invocação do ligador pendente.
        /// </summary>
        /// <param name="state">Uma ligação pendente do ligador.</param>
        /// <param name="cubinOut">Caso seja bem-sucedido, irá apontar para a imagem de saída.</param>
        /// <param name="sizeOut">Parâmetro opcional para receber o tamanho da imagem gerada.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint="cuLinkComplete")]
        public static extern ECudaResult CudaLinkComplete(
            SCudaLinkState state,
            IntPtr cubinOut,
            ref SizeT sizeOut);

        /// <summary>
        /// Cria uma invocação do ligador pendente.
        /// </summary>
        /// <param name="numOptions">O número de opçóes.</param>
        /// <param name="options">Vector de opções de compilador e ligador.</param>
        /// <param name="optionValues">Vector de valores de opçóes, cada uma promovida a void*.</param>
        /// <param name="stateOut">
        /// Em caso de sucesso, irá conter um estado <see cref="SCudaLinkState"/>.
        /// </param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint="cuLinkCreate")]
        public static extern ECudaResult CudaLinkCreate(
            uint numOptions,
            [Out] ECudaJitOption[] options,
            IntPtr optionValues,
            ref SCudaLinkState stateOut);

        /// <summary>
        /// Destrói o estado para invocações JIT do ligador.
        /// </summary>
        /// <param name="state">O objecto de estado para a invocação do ligador.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint="cuLinkDestroy")]
        public static extern ECudaResult CudaLinkDestroy(SCudaLinkState state);

        /// <summary>
        /// Retorna um manuseador para uma função.
        /// </summary>
        /// <param name="hfunc">O manuesador para a função retornado.</param>
        /// <param name="mod">O módulo do qual se pretende extrair a função.</param>
        /// <param name="name">O nome da função.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorNotFound"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint="cuModuleGetFunction")]
        public static extern ECudaResult CudaModuleGetFunction(
            ref SCudaFunction hfunc, 
            SCudaModule mod,
            string name);

        /// <summary>
        /// Retorna um apontador global a partir do módulo.
        /// </summary>
        /// <param name="dptr">O apontador global de dispositivo retornado.</param>
        /// <param name="bytes">O tamanho global em bytes retornado.</param>
        /// <param name="mod">O módulo do qual se pretende obter o apontador global.</param>
        /// <param name="name">Nome do global para obter.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorNotFound"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint="cuModuleGetGlobal")]
        public static extern ECudaResult CudaModuleGetGlobal(
            ref SCudaDevicePtr dptr,
            ref SizeT bytes,
            SCudaModule mod,
            string name);

        /// <summary>
        /// Retorna um manuseador para uma surface.
        /// </summary>
        /// <param name="psurfref">A referência da surface retornada.</param>
        /// <param name="mod">O módulo de onde se pretende obter a referência da surface.</param>
        /// <param name="name">O nome da referência da surface a ser obtida.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorNotFound"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint="cuModuleGetSurfRef")]
        public static extern ECudaResult CudaModuleGetSurfRef(
            ref SCudaSurfRef psurfref,
            SCudaModule mod,
            string name);

        /// <summary>
        /// Obtém um manuseador para uma referência de textura.
        /// </summary>
        /// <param name="ptextref">A referência de textura retornada.</param>
        /// <param name="mod">O módulo do qual se pretende obter a referência de textura.</param>
        /// <param name="name">O nome da referência de textura.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorNotFound"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint="cuModuleGetTexRef")]
        public static extern ECudaResult CudaModuleGetTextRef(
            ref SCudaTexRef ptextref,
            SCudaModule mod,
            string name);

        /// <summary>
        /// Carrega um módulo de computação.
        /// </summary>
        /// <param name="module">O módulo retornado.</param>
        /// <param name="name">O nome do ficheiro do módulo a ser carregado.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidPtx"/>,
        /// <see cref="ECudaResult.CudaErrorNotFound"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorFileNotFound"/>,
        /// <see cref="ECudaResult.CudaErrorNoBinaryForGpu"/>,
        /// <see cref="ECudaResult.CudaErrorSharedObjectSymbolNotFound"/>,
        /// <see cref="ECudaResult.CudaErrorSharedObjectInitFailed"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint="cuModuleLoad")]
        public static extern ECudaResult CudaModuleLoad(ref SCudaModule module, string name);

        /// <summary>
        /// Carrega os dados do módulo.
        /// </summary>
        /// <remarks>
        /// A partir de uma apontador para uma imagem, carrega o módulo correspondente no contexto actual. O
        /// apontador pode ser obtido por intermédio do mapeamento de um ficheiro cubin, de um PTX ou de um
        /// fatbin contendo um vector de carácteres terminado com o valor nulo ou ncorporando um cubin ou
        /// fatbin nos recursos do executável e utilzador funçóes do sistema operativo taiso como a função
        /// Windows "FindResource()" para obter o apontador.
        /// </remarks>
        /// <param name="module">O módulo retornado.</param>
        /// <param name="image">Os dados do módulo a serem carregados.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>
        /// <see cref="ECudaResult.CudaErrorInvalidPtx"/>,
        /// <see cref="ECudaResult.CudaErrorNotFound"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorNoBinaryForGpu"/>,
        /// <see cref="ECudaResult.CudaErrorSharedObjectSymbolNotFound"/>,
        /// <see cref="ECudaResult.CudaErrorSharedObjectInitFailed"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint="cuModuleLoadData")]
        public static extern ECudaResult CudaModuleLoadData(ref SCudaModule module, string image);

        /// <summary>
        /// Carrega os dados do módulo com opções.
        /// </summary>
        /// <param name="module">O módulo retornado.</param>
        /// <param name="image">Os dados do móudlo a serem carregados.</param>
        /// <param name="numOptions">O número de opções.</param>
        /// <param name="options">O vector de opçóes para o JIT.</param>
        /// <param name="optionValues">Os valores das opções para o JIT.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>
        /// <see cref="ECudaResult.CudaErrorInvalidPtx"/>,
        /// <see cref="ECudaResult.CudaErrorNotFound"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorNoBinaryForGpu"/>,
        /// <see cref="ECudaResult.CudaErrorSharedObjectSymbolNotFound"/>,
        /// <see cref="ECudaResult.CudaErrorSharedObjectInitFailed"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint="cuModuleLoadDataEx")]
        public static extern ECudaResult CudaModuleLoadDataEx(
            ref SCudaModule module,
            string image,
            uint numOptions,
            [Out] ECudaJitOption[] options,
            [Out] string[] optionValues);

        /// <summary>
        /// Carrega um módulo.
        /// </summary>
        /// <param name="module">O módulo retornado.</param>
        /// <param name="fatCubin">O binário fat a ser carregado.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>
        /// <see cref="ECudaResult.CudaErrorInvalidPtx"/>,
        /// <see cref="ECudaResult.CudaErrorNotFound"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorNoBinaryForGpu"/>,
        /// <see cref="ECudaResult.CudaErrorSharedObjectSymbolNotFound"/>,
        /// <see cref="ECudaResult.CudaErrorSharedObjectInitFailed"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint="cuModuleLoadFatBinary")]
        public static extern ECudaResult CudaModuleLoadFatBinary(ref SCudaModule module, string fatCubin);

        /// <summary>
        /// Descarrega um módulo.
        /// </summary>
        /// <param name="module">O módulo.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>
        /// <see cref="ECudaResult.CudaErrorInvalidPtx"/>,
        /// <see cref="ECudaResult.CudaErrorNotFound"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorNoBinaryForGpu"/>,
        /// <see cref="ECudaResult.CudaErrorSharedObjectSymbolNotFound"/>,
        /// <see cref="ECudaResult.CudaErrorSharedObjectInitFailed"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuModuleUnload")]
        public static extern ECudaResult CudaModuleUnload(SCudaModule module);

        #endregion Gestão de módulos

        #region Gestão de memória
        
        #endregion Gestão de memória

        #region Gestão de memória

        /// <summary>
        /// Cria um vector 3D de CUDA.
        /// </summary>
        /// <remarks>
        /// Cria um vector 3D de acordo com o objecto do tipo <see cref="SCudaArray3DDescriptor"/> passado
        /// no argumento ptrAllocateArray e retorna um manuseador no parâmetro de referência
        /// phandle.
        /// </remarks>
        /// <param name="phandle">O vector retornado.</param>
        /// <param name="ptrAllocateArray">O descritor do vector.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuArray3DCreate")]
        public static extern ECudaResult CudaArray3DCreate(
            ref  SCudaArray phandle,
            ref SCudaArray3DDescriptor ptrAllocateArray);

        /// <summary>
        /// Obtém um descritor do vector CUDA 3D.
        /// </summary>
        /// <remarks>
        /// Rertorna na referência ptrArrayDescriptor o descritor que contém informação do formato
        /// e dimensões do vector CUDA harray. É útil em sub-rotinas para as quais é passado um
        /// vector CUDA e onde é necessário conhecer os parâmetros do vector CUDA para validações ou outros
        /// propósitos. Esta função pode ser chamada em vectores 1D ou 2D nos quais os membros de altura e/ou 
        /// largura do descritor serão preenchidos com o valor 0.
        /// </remarks>
        /// <param name="ptrArrayDescriptor">O descritor retornado.</param>
        /// <param name="harray">O vector CUDA 3D.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuArray3DGetDescriptor")]
        public static extern ECudaResult CudaArray3DGetDescriptor(
            ref SCudaArray3DDescriptor ptrArrayDescriptor,
            SCudaArray harray);

        /// <summary>
        /// Cria um vector CUDA 1D ou 2D.
        /// </summary>
        /// <remarks>
        /// Cria um vector CUDA de acordo com o descritor e retorna um manuseador no parâmetro de referência
        /// phandle.
        /// </remarks>
        /// <param name="phandle">O vector retornado.</param>
        /// <param name="ptrAllocateArray">O descritor do vector.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuArrayCreate")]
        public static extern ECudaResult CudaArrayCreate(
            ref  SCudaArray phandle,
            ref SCudaArrayDescriptor ptrAllocateArray);

        /// <summary>
        /// Destrói o vector CUDA.
        /// </summary>
        /// <param name="harray">O vector CUDA a ser destruído.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        ///  <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        ///   <see cref="ECudaResult.CudaErrorArrayIsMapped"/>
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuArrayDestroy")]
        public static extern ECudaResult CudaArrayDestroy(SCudaArray harray);

        /// <summary>
        /// Obtém o descritor de um vector CUDA 1D ou 2D.
        /// </summary>
        /// <remarks>
        /// Retorna em ptrArrayDescriptor um descritor contendo informação do formato e dimensões
        /// do vector CUDA harray. É útil para sub-rotinas para as quais seja passado um vector
        /// CUDA mas onde é necessário o conhecimento dos parâmetros do vector CUDA para validações ou outros
        /// propósitos.
        /// </remarks>
        /// <param name="ptrArrayDescriptor">O descritor do vector retornado.</param>
        /// <param name="harray">O vector do qual se pretende obter o descritor.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuArrayGetDescriptor")]
        public static extern ECudaResult CudaArrayGetDescriptor(
            ref SCudaArrayDescriptor ptrArrayDescriptor,
            SCudaArray harray);

        /// <summary>
        /// Retona um manuseador para um dispositivo computacional.
        /// </summary>
        /// <param name="dev">O manuseador do dispositivo retornado.</param>
        /// <param name="pciBusId">
        /// Texto em qualuer uma das seguintes formas:
        /// <list type="bullet">
        /// <item>[domain]:[bus]:[device].[function]</item>
        /// <item> [domain]:[bus]:[device]</item>
        /// <item>[bus]:[device].[function]</item>
        /// </list>
        /// onde domain, bus, device e function são valores hexadecimais.
        /// </param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuDeviceGetByPCIBusId")]
        public static extern ECudaResult CudaDeviceGetByPCIBusId(
            ref int dev,
            string pciBusId);

        /// <summary>
        /// Retorna o identificador do barramento PCI.
        /// </summary>
        /// <remarks>
        /// Retorna uma cadeia de carácteres ASCII que identifica o dispositivo dev na
        /// referência pciBusId. O parâmetro len especifica o comprimento máximo
        /// da cadeia de carácters que pode ser retornado.
        /// </remarks>
        /// <param name="pciBusId">
        /// <list type="bullet">
        /// <item>[domain]:[bus]:[device].[function]</item>
        /// <item> [domain]:[bus]:[device]</item>
        /// <item>[bus]:[device].[function]</item>
        /// </list>
        /// onde domain, bus, device e function são valores hexadecimais.
        /// </param>
        /// <param name="len">Comprimento máximo do texto a ser armazenado como nome.</param>
        /// <param name="dev">O dispositivo do qual se pretende obter o identificador.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuDeviceGetPCIBusId")]
        public static extern ECudaResult CudaDeviceGetPCIBusId(
            ref string pciBusId,
            int len,
            int dev);

        /// <summary>
        /// Fecha a memória mapeada com a função <see cref="CudaApi.CudaIpcOpenMemHandle"/>.
        /// </summary>
        /// <remarks>
        /// A alocação original no processo de exportação bem como dos mapeamentos de importação em outros
        /// processos mantêm-se inalterados. Quaisquer recursos utilizados para habilitar o acesso de cais
        /// serão libertados se este for o último mapeamento que os utilize. A funcionalidade de IPC é restrita
        /// a dispositivos com suporte para endereçamento unificado em sistemas operativos Linux.
        /// </remarks>
        /// <param name="dptr">
        /// O apontador para o dispositivo retornado por <see cref="CudaApi.CudaIpcOpenMemHandle"/>.
        /// </param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuIpcCloseMemHandle")]
        public static extern ECudaResult CudaIpcCloseMemHandle(SCudaDevicePtr dptr);

        /// <summary>
        /// Obtém um manuseador interporcesso para um evento previamente alocado.
        /// </summary>
        /// <param name="phandle">
        /// Uma referência para um evento alocado pelo utilizador na qual é retornado o manuseador opaco do
        /// evento.
        /// </param>
        /// <param name="cudaEvent"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorMapFailed"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuIpcGetEventHandle")]
        public static extern ECudaResult CudaIpcGetEventHandle(
            ref SCudaIpcEventHandle phandle,
            SCudaEvent cudaEvent);

        /// <summary>
        /// Obtém um manuseador iterprocesso de memória para uma alocação de memória em dispositivo existente.
        /// </summary>
        /// <remarks>
        /// Por intermédio de uma referência para a base de memória de dispositivo alocada pela função
        /// <see cref="CudaApi.CudaMemAlloc"/> e exporta-o para ser utilizado em outro processo. Trata-se de uma
        /// operação leve e pode ser chamada várias vezes numa alocação sem incorrer em efeitos adversos.
        /// </remarks>
        /// <param name="phandle">A referência na qual é retornado o manuseador da memória.</param>
        /// <param name="dptr">A referência para a memória de dispositivo alocada.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorMapFailed"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuIpcGetMemHandle")]
        public static extern ECudaResult CudaIpcGetMemHandle(
            ref SCudaIpcMemHandle phandle,
            SCudaDevicePtr dptr);

        /// <summary>
        /// Abre um manuseador de evento interprocesso para ser utilizado no processo corrente.
        /// </summary>
        /// <param name="phEvent">Retorna o evento importado.</param>
        /// <param name="handle">O manuseador interporcesso a ser aberto.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorMapFailed"/>,
        /// <see cref="ECudaResult.CudaErrorPeerAccessUnsupported"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuIpcOpenEventHandle")]
        public static extern ECudaResult CudaIpcOpenEventHandle(
            ref SCudaEvent phEvent,
            SCudaIpcEventHandle handle);

        /// <summary>
        /// Abre um manuseador interprocesso de memória exportado de outro processo e retorna um apontador
        /// de dispositivo utilizável no processo local.
        /// </summary>
        /// <param name="pdptr">O apontador de dispositivo retornado.</param>
        /// <param name="handle">O manuseador da memória a ser aberto.</param>
        /// <param name="flags">A marca a ser utilizada.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorMapFailed"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorTooManyPeers"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuIpcOpenMemHandle")]
        public static extern ECudaResult CudaIpcOpenMemHandle(
            ref SCudaDevicePtr pdptr,
            SCudaIpcMemHandle handle,
            ECudaIpcMemFlags flags);

        /// <summary>
        /// Aloca memória de dispositivo.
        /// </summary>
        /// <remarks>
        /// Aloca o tamanho especificado em bytes de memória linear no dispositivo e retorna um apontador
        /// para a memória alocada. A memória alocada é adequadamente alinhada para qualquer tipo de variável.
        /// A memória não é limpa. Se o tamanho for 0, a função retorna 
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </remarks>
        /// <param name="dptr">O apontador de dispositivo retornado.</param>
        /// <param name="bytesize">O tamanho da alocação requerida em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemAlloc")]
        public static extern ECudaResult CudaMemAlloc(ref SCudaDevicePtr dptr, SizeT bytesize);

        /// <summary>
        /// Aloca memória de anfitrião com fechadura por página.
        /// </summary>
        /// <param name="pp">Referência para o apontador de anfitrião.</param>
        /// <param name="bytesize">O tamanho da alocação requerida em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemAllocHost")]
        public static extern ECudaResult CudaMemAllocHost(IntPtr pp, SizeT bytesize);

        /// <summary>
        /// Aloca memória que será automaticamente gerida pelo Sistema de Memória Unificada.
        /// </summary>
        /// <param name="dptr">O apontador do dispositivo retornado.</param>
        /// <param name="bytesize">O tamanho da memória alocada em bytes.</param>
        /// <param name="flags">A marca que indica o tipo de anexação.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorNotSupported"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemAllocManaged")]
        public static extern ECudaResult CudaMemAllocManaged(
            ref SCudaDevicePtr dptr,
            SizeT bytesize,
            ECudaMemAttachFlags flags);

        /// <summary>
        /// Aloca memória espaçada do dispositivo.
        /// </summary>
        /// <param name="dptr">O apontador retornado do dispositivo.</param>
        /// <param name="ptrPitch">O passo em bytes retornado da alocação.</param>
        /// <param name="widthInBytes">A lagrgura em bytes.</param>
        /// <param name="height">A altura em bytes.</param>
        /// <param name="elementSizeBytes">Tamanho dos maiores blocos de leitura/escrita.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemAllocPitch")]
        public static extern ECudaResult CudaMemAllocPitch(
            ref SCudaDevicePtr dptr,
            ref SizeT ptrPitch,
            SizeT widthInBytes,
            SizeT height,
            uint elementSizeBytes);

        /// <summary>
        /// Liberta a memória de dispositivo.
        /// </summary>
        /// <remarks>
        /// Liberta a memória apontada por dptr que deve ter sido retornado por uma chamada préiva às funções
        /// <see cref="CudaApi.CudaMemAlloc"/> ou <see cref="CudaApi.CudaMemAllocPitch"/>.
        /// </remarks>
        /// <param name="dptr">O apontador para a memória a ser libertada.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemFree")]
        public static extern ECudaResult CudaMemFree(SCudaDevicePtr dptr);

        /// <summary>
        /// Liberta memória de paginação do anfitrião.
        /// </summary>
        /// <remarks>
        /// O apontador para a memória a ser libertada deverá ser resultado de uma chamada prévia à função
        /// <see cref="CudaMemAllocHost"/>.
        /// </remarks>
        /// <param name="p">O apontador para a memória a ser libertada.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemFreeHost")]
        public static extern ECudaResult CudaMemFreeHost(IntPtr p);

        /// <summary>
        /// Obtém informação sobre alocações de memória.
        /// </summary>
        /// <remarks>
        /// Os parâmetros pbase e psize são opcionais. Se algum deles for nulo, será ignorado.
        /// </remarks>
        /// <param name="pbase">O endereço de base retornado.</param>
        /// <param name="psize">O tamanho retornado da alocação de memória de dispositivo.</param>
        /// <param name="dptr">O apontador para o dispositivo a ser consultado.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemGetAddressRange")]
        public static extern ECudaResult CudaMemGetAddressRange(
            ref SCudaDevicePtr pbase,
            ref SizeT psize,
            SCudaDevicePtr dptr);

        /// <summary>
        /// Obtém a memória livre e a memória total.
        /// </summary>
        /// <param name="free">A memória livre em bytes retornada.</param>
        /// <param name="total">A memória total em bytes retornada.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemGetInfo")]
        public static extern ECudaResult CudaMemGetInfo(ref SizeT free, ref SizeT total);

        /// <summary>
        /// Aloca memória de anfitrião fechada à paginação.
        /// </summary>
        /// <remarks>
        /// O parâmetro flags poderá receber um dos valores 
        /// <see cref="CudaConstants.CudaMemHostRegisterPortable"/>,
        /// <see cref="CudaConstants.CudaMemHostDeviceMap"/> ou 
        /// <see cref="CudaConstants.CudaMemHostAllocWritecombined"/>.
        /// </remarks>
        /// <param name="pp">Apontador de anfitrião para a memória de fecho por paginação.</param>
        /// <param name="bytesize">O tamanho da alocação requerida em bytes.</param>
        /// <param name="flags">Marcas para a requisição de alocação.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemHostAlloc")]
        public static extern ECudaResult CudaMemHostAlloc(IntPtr pp, SizeT bytesize, uint flags);

        /// <summary>
        /// Passa o apontador de memória mapeada.
        /// </summary>
        /// <param name="pdptr">O apontador de dispositivo retornado.</param>
        /// <param name="p">O apontador de anfitrião.</param>
        /// <param name="flags">Opções (terá de ser igual a 0).</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemHostGetDevicePointer")]
        public static extern ECudaResult CudaMemHostGetDevicePointer(
            ref SCudaDevicePtr pdptr,
            IntPtr p,
            uint flags);

        /// <summary>
        /// Passa as marcas que foram usadas para uma alocação mapeada.
        /// </summary>
        /// <remarks>
        /// Ver <see cref="CudaApi.CudaMemHostAlloc"/> para averiguar que marcas poderão ser retornadas.
        /// </remarks>
        /// <param name="ptrFlags">As marcas retornadas.</param>
        /// <param name="p">O apontador de anfitrião.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemHostGetFlags")]
        public static extern ECudaResult CudaMemHostGetFlags(ref uint ptrFlags, IntPtr p);

        /// <summary>
        /// Regista um intervalo de memória existente para ser utilizado com CUDA.
        /// </summary>
        /// <param name="p">A apontador de anfitrião para a memória a ser fechada por paginação.</param>
        /// <param name="bytesize">O tamanho em bytes do intervalo de endereços da paginação fechada.</param>
        /// <param name="flags">As marcas que descrevem o pedido de alocação.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorHostMemoryAlreadyRegistered"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemHostRegister")]
        public static extern ECudaResult CudaMemHostRegister(IntPtr p, SizeT bytesize, uint flags);

        /// <summary>
        /// Elimina o registo do intervalo de memória anteriormente registada.
        /// </summary>
        /// <param name="p">O apontador para a memória da qual se pretende eliminar o registo.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorHostMemoryNotRegistered"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemHostUnregister")]
        public static extern ECudaResult CudaMemHostUnregister(IntPtr p);

        /// <summary>
        /// Copia a memória.
        /// </summary>
        /// <remarks>
        /// Copia dados entre dois apontadores. Convém notar que esta função infere o tipo da
        /// transferência (anfitrião para anfitrião, afitrião para dispositivo, dispositivo para
        /// anfitrião ou dispositivo para dispositivo) a partir dos valores dos apontadores. Esta
        /// função é suportada apenas em contextos que suportem endereçamento unificado.
        /// </remarks>
        /// <param name="dst">O apontador para o endereço virtual unificado de detino.</param>
        /// <param name="src">O apontador para o endereço virtual unificado de partida.</param>
        /// <param name="byteCount">O tamanho da cópia em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpy")]
        public static extern ECudaResult CudaMemCpy(SCudaDevicePtr dst, SCudaDevicePtr src, SizeT byteCount);

        /// <summary>
        /// Copia memória para vectores 2D.
        /// </summary>
        /// <param name="ptrCopy">Os parâmetros para a cópia de memória.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpy2D")]
        public static extern ECudaResult CudaMemcpy2D(ref SCudaMemCpy2D ptrCopy);

        /// <summary>
        /// Copia memória para vectores 2D de forma assíncrona.
        /// </summary>
        /// <param name="ptrCopy">Os parâmetros para a cópia de memória.</param>
        /// <param name="hstream">O identificadorde caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpy2DAsync")]
        public static extern ECudaResult CudaMemcpy2DAsync(ref SCudaMemCpy2D ptrCopy, SCudaStream hstream);

        /// <summary>
        /// Copia memória para vectores 2D.
        /// </summary>
        /// <param name="ptrCopy">Os parâmetros para a cópia de memória.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpy2DUnaligned")]
        public static extern ECudaResult CudaMemcpy2DUnaligned(ref SCudaMemCpy2D ptrCopy);

        /// <summary>
        /// Copia memória para vectores 3D.
        /// </summary>
        /// <param name="ptrCopy">Os parâmetros para a cópia de memória.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpy3D")]
        public static extern ECudaResult CudaMemcpy3D(ref SCudaMemCpy3D ptrCopy);

        /// <summary>
        /// Copia memória para vectores 3D de forma assíncrona.
        /// </summary>
        /// <param name="ptrCopy">Os parâmetros para a cópia de memória.</param>
        /// <param name="hstream">O identificador de caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpy3DAsync")]
        public static extern ECudaResult CudaMemcpy3DAsync(ref SCudaMemCpy3D ptrCopy, SCudaStream hstream);

        /// <summary>
        /// Copia memória entre contextos.
        /// </summary>
        /// <remarks>
        /// Efectua a cópia de vectores 3D de acordo com os parâmetros especificados.
        /// </remarks>
        /// <param name="ptrCopy">Parâmetros para a cópia de memória.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpy3DPeer")]
        public static extern ECudaResult CudaMemcpy3DPeer(ref SCudaMemCpy3DPeer ptrCopy);

        /// <summary>
        /// Copia memória entre contextos de forma assíncrona.
        /// </summary>
        /// <param name="ptrCopy">Parâmetros para a cópia de memória.</param>
        /// <param name="hstream">O identificador de caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpy3DPeerAsync")]
        public static extern ECudaResult CudaMemcpy3DPeerAsync(ref SCudaMemCpy3DPeer ptrCopy, SCudaStream hstream);

        /// <summary>
        /// Copia memória de forma assíncrona.
        /// </summary>
        /// <remarks>
        /// Copia dados entre dois apontadores sendo uma função suportada apenas em contextos que
        /// suportem endereçamento unificado.
        /// </remarks>
        /// <param name="dst">O apontador para o endereço de espaço virtual de destino.</param>
        /// <param name="src">O apontador para o endereço de espaço virtual de origem.</param>
        /// <param name="byteCount">O tamanho da cópia de memória em bytes.</param>
        /// <param name="hstream">O identificador de caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyAsync")]
        public static extern ECudaResult CudaMemcpyAsync(
            SCudaDevicePtr dst,
            SCudaDevicePtr src,
            SizeT byteCount,
            SCudaStream hstream);

        /// <summary>
        /// Copia memória entre vectores.
        /// </summary>
        /// <remarks>
        /// Copia de um vector CUDA 1D para outro. Não é necessário que os elementos entre os vectores
        /// CUDA se encontrem no mesmo formato mas o respectivo tamanho deverá ser o mesmo.
        /// </remarks>
        /// <param name="dstArray">O vector de destino.</param>
        /// <param name="dstOffset">A deslocação em bytes do vector de destino.</param>
        /// <param name="srcArray">O vector de origem.</param>
        /// <param name="srcOffset">A deslocação em bytes do vector de origem.</param>
        /// <param name="byteCount">O tamanho da cópia de memória em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyAtoA")]
        public static extern ECudaResult CudaMemcpyAtoA(
            SCudaArray dstArray,
            SizeT dstOffset,
            SCudaArray srcArray,
            SizeT srcOffset,
            SizeT byteCount);

        /// <summary>
        /// Copia memória de um vector para um dispositivo.
        /// </summary>
        /// <param name="dstDevice">O apontador para o dispositivo de destino.</param>
        /// <param name="srcArray">O vector de origem.</param>
        /// <param name="srcOffset">A deslocação em bytes do vector de origem.</param>
        /// <param name="byteCount">O tamanho da memória a ser copiada em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyAtoD")]
        public static extern ECudaResult CudaMemcpyAtoD(
            SCudaDevicePtr dstDevice,
            SCudaArray srcArray,
            SizeT srcOffset,
            SizeT byteCount);

        /// <summary>
        /// Copia  memória de um vector para o anfitrião.
        /// </summary>
        /// <param name="dstHost">O anfitrião de destino.</param>
        /// <param name="srcArray">O vector de origem.</param>
        /// <param name="srcOffset">A deslocação em bytes do vector de origem.</param>
        /// <param name="byteCount">O tamanho da cópia de memória em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyAtoH")]
        public static extern ECudaResult CudaMemcpyAtoH(
            IntPtr dstHost,
            SCudaArray srcArray,
            SizeT srcOffset,
            SizeT byteCount);

        /// <summary>
        /// Copia memória de um vector para o anfitrião de forma assíncrona.
        /// </summary>
        /// <param name="dstHost">O anfitrião de destino.</param>
        /// <param name="srcArray">O vector de origem.</param>
        /// <param name="srcOffset">A deslocação em bytes do vector de origem.</param>
        /// <param name="byteCount">O tamanho da cópia de memória em bytes.</param>
        /// <param name="hstream">O identificador do caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyAtoHAsync")]
        public static extern ECudaResult CudaMemcpyAtoHAsync(
            IntPtr dstHost,
            SCudaArray srcArray,
            SizeT srcOffset,
            SizeT byteCount,
            SCudaStream hstream);

        /// <summary>
        /// Copia memória do dispositivo para um vector.
        /// </summary>
        /// <param name="dstArray">O vector de destino.</param>
        /// <param name="dstOffset">A deslocação em bytes do vector de destino.</param>
        /// <param name="srcDevice">O dispositivo de origem.</param>
        /// <param name="byteCount">O tamanho da cópia de memória.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyDtoA")]
        public static extern ECudaResult CudaMemcpyDtoA(
            SCudaArray dstArray,
            SizeT dstOffset,
            SCudaDevicePtr srcDevice,
            SizeT byteCount);

        /// <summary>
        /// Copia memória de um dispositivo para outro.
        /// </summary>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="srcDevice">O dispositivo de origem.</param>
        /// <param name="byteCount">O tamanho da cópia da memória em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyDtoD")]
        public static extern ECudaResult CudaMemcpyDtoD(
            SCudaDevicePtr dstDevice,
            SCudaDevicePtr srcDevice,
            SizeT byteCount);

        /// <summary>
        /// Copia memória de um dispositivo para outro de forma assíncrona.
        /// </summary>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="srcDevice">O dispositivo de origem.</param>
        /// <param name="byteCount">O tamanho da cópia da memória em bytes.</param>
        /// <param name="hstream">O identificador de caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyDtoDAsync")]
        public static extern ECudaResult CudaMemcpyDtoDAsync(
            SCudaDevicePtr dstDevice,
            SCudaDevicePtr srcDevice,
            SizeT byteCount,
            SCudaStream hstream);

        /// <summary>
        /// Copia memória do dispositivo para o anfitrião.
        /// </summary>
        /// <param name="dstHost">O anfitrião de destino.</param>
        /// <param name="srcDevice">O dispositivo de origem.</param>
        /// <param name="byteCount">O tamanho da cópia da memória em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyDtoH")]
        public static extern ECudaResult CudaMemcpyDtoH(
            IntPtr dstHost,
            SCudaDevicePtr srcDevice,
            SizeT byteCount);

        /// <summary>
        /// Copia memória do dispositivo para o anfitrião de forma assíncrona.
        /// </summary>
        /// <param name="dstHost">O anfitrião de destino.</param>
        /// <param name="srcDevice">O dispositivo de origem.</param>
        /// <param name="byteCount">O tamanho da cópia da memória em bytes.</param>
        /// <param name="hstream">O identificador de caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyDtoHAsync")]
        public static extern ECudaResult CudaMemcpyDtoHAsync(
            IntPtr dstHost,
            SCudaDevicePtr srcDevice,
            SizeT byteCount,
            SCudaStream hstream);

        /// <summary>
        /// Copia memória do anfitrião para o vector.
        /// </summary>
        /// <param name="dstArray">O vector de destino.</param>
        /// <param name="dstOffset">A deslocação em bytes no vector de destino.</param>
        /// <param name="srcHost">O anfitrião de origem.</param>
        /// <param name="byteCount">O tamanho da cópia da memória em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyHtoA")]
        public static extern ECudaResult CudaMemcpyHtoA(
            SCudaArray dstArray,
            SizeT dstOffset,
            IntPtr srcHost,
            SizeT byteCount);

        /// <summary>
        /// Copia memória do anfitrião para o vector de forma assíncrona.
        /// </summary>
        /// <param name="dstArray">O vector de destino.</param>
        /// <param name="dstOffset">A deslocação em bytes no vector de destino.</param>
        /// <param name="srcHost">O anfitrião de origem.</param>
        /// <param name="byteCount">O tamanho da cópia da memória em bytes.</param>
        /// <param name="hstream">O identificador de caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyHtoAAsync")]
        public static extern ECudaResult CudaMemcpyHtoAAsync(
            SCudaArray dstArray,
            SizeT dstOffset,
            IntPtr srcHost,
            SizeT byteCount,
            SCudaStream hstream);

        /// <summary>
        /// Copia memória do anfitrião para o dispositivo.
        /// </summary>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="srcHost">O anfitrião de origem.</param>
        /// <param name="byteCount">O tamanho da cópia de memória em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyHtoD")]
        public static extern ECudaResult CudaMemcpyHtoD(
            SCudaDevicePtr dstDevice,
            IntPtr srcHost,
            SizeT byteCount);

        /// <summary>
        /// Copia memória do anfitrião para o dispositivo de forma assíncrona.
        /// </summary>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="srcHost">O anfitrião de origem.</param>
        /// <param name="byteCount">O tamanho da cópia de memória em bytes.</param>
        /// <param name="hstream">O identificador de caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyHtoDAsync")]
        public static extern ECudaResult CudaMemcpyHtoDAsync(
            SCudaDevicePtr dstDevice,
            IntPtr srcHost,
            SizeT byteCount,
            SCudaStream hstream);

        /// <summary>
        /// Copia memória entre dois contextos.
        /// </summary>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="dstContext">O contexto de destino.</param>
        /// <param name="srcDevice">O dispositivo de origem.</param>
        /// <param name="srcContext">O contexto de origem.</param>
        /// <param name="byteCount">O tamanho da cópia de memória em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyPeer")]
        public static extern ECudaResult CudaMemcpyPeer(
            SCudaDevicePtr dstDevice,
            SCudaContext dstContext,
            SCudaDevicePtr srcDevice,
            SCudaContext srcContext,
            SizeT byteCount);

        /// <summary>
        /// Copia memória entre dois contextos de forma assíncrona.
        /// </summary>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="dstContext">O contexto de destino.</param>
        /// <param name="srcDevice">O dispositivo de origem.</param>
        /// <param name="srcContext">O contexto de origem.</param>
        /// <param name="byteCount">O tamanho da cópia de memória em bytes.</param>
        /// <param name="hstream">O identificador do caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemcpyPeerAsync")]
        public static extern ECudaResult CudaMemcpyPeerAsync(
            SCudaDevicePtr dstDevice,
            SCudaContext dstContext,
            SCudaDevicePtr srcDevice,
            SCudaContext srcContext,
            SizeT byteCount,
            SCudaStream hstream);

        /// <summary>
        /// Inicializa a memória do dispositivo.
        /// </summary>
        /// <remarks>
        /// Atribui o intervalo de memória com n valores de 16 bits com o valor especificado. O apontador
        /// de dispositivo deverá ter um alinhamento de dois bytes.
        /// </remarks>
        /// <param name="dstDevice">O apontador do dispositivo de destino.</param>
        /// <param name="us">O valor a ser atribuído.</param>
        /// <param name="n">O número de elementos.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemsetD16")]
        public static extern ECudaResult CudaMemsetD16(
            SCudaDevicePtr dstDevice,
            ushort us,
            SizeT n);

        /// <summary>
        /// Inicializa a memória do dispositivo de forma assíncrona.
        /// </summary>
        /// <remarks>
        /// Atribui o intervalo de memória com n valores de 16 bits com o valor especificado. O apontador
        /// de dispositivo deverá ter um alinhamento de dois bytes.
        /// </remarks>
        /// <param name="dstDevice">O apontador do dispositivo de destino.</param>
        /// <param name="us">O valor a ser atribuído.</param>
        /// <param name="n">O número de elementos.</param>
        /// <param name="hstream">O identificador de caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemsetD16Async")]
        public static extern ECudaResult CudaMemsetD16Async(
            SCudaDevicePtr dstDevice,
            ushort us,
            SizeT n,
            SCudaStream hstream);

        /// <summary>
        /// Inicializa a memória de dispositivo.
        /// </summary>
        /// <remarks>
        /// Atribui o valor especificado ao intervalo de memória 2D de 16 bits.
        /// </remarks>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="dstPitch">O passo de destino.</param>
        /// <param name="us">O valor a ser atribuído.</param>
        /// <param name="width">A largura da linha.</param>
        /// <param name="height">O número de linhas.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemsetD2D16")]
        public static extern ECudaResult CudaMemsetD2D16(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch, ushort us,
            SizeT width,
            SizeT height);

        /// <summary>
        /// Inicializa a memória de dispositivo de forma assíncrona.
        /// </summary>
        /// <remarks>
        /// Atribui o valor especificado ao intervalo de memória 2D de 16 bits.
        /// </remarks>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="dstPitch">O passo de destino.</param>
        /// <param name="us">O valor a ser atribuído.</param>
        /// <param name="width">A largura da linha.</param>
        /// <param name="height">O número de linhas.</param>
        /// <param name="hstream">O identificador do caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemsetD2D16Async")]
        public static extern ECudaResult CudaMemsetD2D16Async(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            ushort us,
            SizeT width,
            SizeT height,
            SCudaStream hstream);

        /// <summary>
        /// Inicializa a memória de dispositivo.
        /// </summary>
        /// <remarks>
        /// Atribui o valor ao intervalo de valores de 32 bits.
        /// </remarks>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="dstPitch">O passo do dispositivo de destino.</param>
        /// <param name="ui">O valor a ser atribuído.</param>
        /// <param name="width">A largura da linha.</param>
        /// <param name="height">O número de linhas.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemsetD2D32")]
        public static extern ECudaResult CudaMemsetD2D32(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            uint ui,
            SizeT width,
            SizeT height);

        /// <summary>
        /// Inicializa a memória de dispositivo de forma assíncrona.
        /// </summary>
        /// <remarks>
        /// Atribui o valor ao intervalo de valores de 32 bits.
        /// </remarks>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="dstPitch">O passo do dispositivo de destino.</param>
        /// <param name="ui">O valor a ser atribuído.</param>
        /// <param name="width">A largura da linha.</param>
        /// <param name="height">O número de linhas.</param>
        /// <param name="hstream">O identificador do caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemsetD2D32Async")]
        public static extern ECudaResult CudaMemsetD2D32Async(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            uint ui,
            SizeT width,
            SizeT height,
            SCudaStream hstream);

        /// <summary>
        /// Inicializa a memória de dispositivo.
        /// </summary>
        /// <remarks>
        /// Atribui o valor espeicificado ao intervalo de valores de 8-bits.
        /// </remarks>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="dstPitch">O passo de destino.</param>
        /// <param name="uc">O valor a ser atribuído.</param>
        /// <param name="width">A largura da linha.</param>
        /// <param name="height">O número de linhas.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemsetD2D8")]
        public static extern ECudaResult CudaMemsetD2D8(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            byte uc,
            SizeT width,
            SizeT height);

        /// <summary>
        /// Inicializa a memória de dispositivo de forma assíncrona.
        /// </summary>
        /// <remarks>
        /// Atribui o valor espeicificado ao intervalo de valores de 8-bits.
        /// </remarks>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="dstPitch">O passo de destino.</param>
        /// <param name="uc">O valor a ser atribuído.</param>
        /// <param name="width">A largura da linha.</param>
        /// <param name="height">O número de linhas.</param>
        /// <param name="hstream">O identificador do caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemsetD2D8Async")]
        public static extern ECudaResult CudaMemsetD2D8Async(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            byte uc,
            SizeT width,
            SizeT height,
            SCudaStream hstream);

        /// <summary>
        /// Inicializa a memória do dispositivo.
        /// </summary>
        /// <remarks>
        /// Atribui o valor ao intervalo de memória de valores de 32 bits.
        /// </remarks>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="ui">O valor a ser atribuído.</param>
        /// <param name="n">O número de elementos.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemsetD32")]
        public static extern ECudaResult CudaMemsetD32(SCudaDevicePtr dstDevice, uint ui, SizeT n);

        /// <summary>
        /// Inicializa a memória do dispositivo de forma assíncrona.
        /// </summary>
        /// <remarks>
        /// Atribui o valor ao intervalo de memória de valores de 32 bits.
        /// </remarks>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="ui">O valor a ser atribuído.</param>
        /// <param name="n">O número de elementos.</param>
        /// <param name="hstream">O identificador de caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemsetD32Async")]
        public static extern ECudaResult CudaMemsetD32Async(
            SCudaDevicePtr dstDevice,
            uint ui,
            SizeT n,
            SCudaStream hstream);

        /// <summary>
        /// Inicializa a memória de dispositivo.
        /// </summary>
        /// <remarks>
        /// Atribui o valor ao intervalo de memória de valores de 8 bits.
        /// </remarks>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="uc">O valor a ser atribuído.</param>
        /// <param name="n">O número de elementos.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemsetD8")]
        public static extern ECudaResult CudaMemsetD8(SCudaDevicePtr dstDevice, byte uc, SizeT n);

        /// <summary>
        /// Inicializa a memória de dispositivo de forma assíncrona.
        /// </summary>
        /// <remarks>
        /// Atribui o valor ao intervalo de memória de valores de 8 bits.
        /// </remarks>
        /// <param name="dstDevice">O dispositivo de destino.</param>
        /// <param name="uc">O valor a ser atribuído.</param>
        /// <param name="n">O número de elementos.</param>
        /// <param name="hstream">O identificador de caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMemsetD8Async")]
        public static extern ECudaResult CudaMemsetD8Async(
            SCudaDevicePtr dstDevice,
            byte uc,
            SizeT n,
            SCudaStream hstream);

        /// <summary>
        /// Cria um vector mipmapped.
        /// </summary>
        /// <param name="phandle">O vector mipmapped retornado.</param>
        /// <param name="pMipmappedArrayDesc">O descritor do vector mipmapped.</param>
        /// <param name="numMipmapLevels">O número de níveis mipmap.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMipmappedArrayCreate")]
        public static extern ECudaResult CudaMipmappedArrayCreate(
            ref SCudaMipmappedArray phandle,
            ref SCudaArray3DDescriptor pMipmappedArrayDesc,
            uint numMipmapLevels);

        /// <summary>
        /// Destrói um vector CUDA mipmapped.
        /// </summary>
        /// <param name="hndMipmappedArray">O vector mipmapped a ser destruído.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMipmappedArrayDestroy")]
        public static extern ECudaResult CudaMipmappedArrayDestroy(SCudaMipmappedArray hndMipmappedArray);

        /// <summary>
        /// Obtém o nível de um vector CUDA mipmapped.
        /// </summary>
        /// <param name="ptrLevelArray">O nível do vector CUDA mipmapped retornado.</param>
        /// <param name="hndMipmappedArray">O vector CUDA mipmapped.</param>
        /// <param name="level">O nível.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuMipmappedArrayGetLevel")]
        public static extern ECudaResult CudaMipmappedArrayGetLevel(
            ref  SCudaArray ptrLevelArray,
            SCudaMipmappedArray hndMipmappedArray, uint level);

        #endregion Gestão de memória

        #region Endereçamento unificado

        /// <summary>
        /// Retorna a informação sobre um apontador.
        /// </summary>
        /// <param name="data">A informação sobre o apontador retornada.</param>
        /// <param name="attribute">O atributo do apontador a ser consultado.</param>
        /// <param name="ptr">O apontador.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuPointerGetAttribute")]
        public static extern ECudaResult CudaPointerGetAttribute(
            IntPtr data,
            ECudaPointerAttribute attribute,
            SCudaDevicePtr ptr);

        /// <summary>
        /// Estaabelece os valores dos atributos numa região de memória privamente alocada.
        /// </summary>
        /// <param name="value">Apontador para a memória contendo o valor a ser atribuído.</param>
        /// <param name="attribute">O apontador para o atributo a ser estabelecido.</param>
        /// <param name="ptr">
        /// O apontador para uma região de memória alocada com o auxílio das
        /// API de alocação CUDA.
        /// </param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuPointerSetAttribute")]
        public static extern ECudaResult CudaPointerSetAttribute(
            IntPtr value,
            ECudaPointerAttribute attribute,
            SCudaDevicePtr ptr);

        #endregion Endereçamento unificado

        #region Gestão de caudal

        /// <summary>
        /// Adiciona a função que permite lidar com o evento despoletado quando
        /// todos os itens introduzidos terminarem ao caudal.
        /// </summary>
        /// <param name="hstream">O caudal.</param>
        /// <param name="callback">A função a ser chamada.</param>
        /// <param name="userData">Os dados de utilizador que deverão ser passados para a função.</param>
        /// <param name="flags">Reservado para uso futuro e, de momento, deverá receber o valor nulo.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorNotSupported"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuStreamAddCallback")]
        public static extern ECudaResult CudaStreamAddCallback(
            SCudaStream hstream,
            CudaStreamCallback callback,
            IntPtr userData,
            uint flags);

        /// <summary>
        /// Anexa memória a um caudal de forma assíncrona.
        /// </summary>
        /// <param name="hstream">Caudal onde adicionar a operação anexada.</param>
        /// <param name="dptr">
        /// O apontador para a memória (deverá ser um apontador para memória gerida.
        /// </param>
        /// <param name="length">O tamanho da memória (deverá ser zero).</param>
        /// <param name="flags">As marcas para a anexação de caudais.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorNotSupported"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuStreamAttachMemAsync")]
        public static extern ECudaResult CudaStreamAttachMemAsync(
            SCudaStream hstream,
            SCudaDevicePtr dptr,
            SizeT length,
            ECudaMemAttachFlags flags);

        /// <summary>
        /// Cria um caudal.
        /// </summary>
        /// <remarks>
        /// Cria um caudal e retorna um manuseador no parâmetros phstream. O argumento flags determina
        /// o comportamento do caudal. Valores válidos para este parâmetro são:
        /// <list type="bullet">
        /// <item>
        /// <see cref="ECudaStreamFlags.Default"/>: a marca de criação de caudal por defeito.
        /// </item>
        /// <item>
        /// <see cref="ECudaStreamFlags.NonBlocking"/>: especifica que o trabalho que se encontra em
        /// execução pode correr concorrentemente com o trabalho no caudal 0 (o caudal nulo) e o caudal
        /// criado não deve efectuar nenhuma sincronização com o caudal 0.
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="phstream">O caudal criado.</param>
        /// <param name="flags">Parâmetros passados para a criação de caudal (ver <see cref="ECudaStreamFlags"/>).</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuStreamCreate")]
        public static extern ECudaResult CudaStreamCreate(
            ref SCudaStream phstream,
            uint flags);

        /// <summary>
        /// Cria um caudal com a prioridade especificada.
        /// </summary>
        /// <remarks>
        /// As prioridades de caudal são apenas suportadas nas GPU Quadro e Tesla com capacidade
        /// computacional 3.5 ou superior. Na implementação actual, apenas kernels lançados em caudais
        /// com prioridade são afectados pelas prioridades de caudal. As prioridades de caudal não têm
        /// qualquer efeito em operações de memória anfitrião-para-dispositivo e de dispositivo-para-
        /// anfitrião.
        /// </remarks>
        /// <param name="phstream">O caudal criado.</param>
        /// <param name="flags">As marcas para a criação do caudal (ver <see cref="ECudaStreamFlags"/>).</param>
        /// <param name="priority">
        /// As prioridades de caudal. Números menores representam prioridades mais elevadas.
        /// </param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuStreamCreateWithPriority")]
        public static extern ECudaResult CudaStreamCreateWithPriority(
            ref SCudaStream phstream,
            uint flags,
            int priority);

        /// <summary>
        /// Destrói um caudal.
        /// </summary>
        /// <remarks>
        /// Destói o caudal especificado. No caso do dispositivo ainda se encontrar a executar algum
        /// trabalho no caudal especificado quando a função é chamada, a função irá retornar 
        /// imediatamente e os recursos associados com o caudal serão automaticamente libertados quando
        /// o dispositivo completar todos os trabalhos existentes nesse caudal.
        /// </remarks>
        /// <param name="hstream">O caudal a ser destruído.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuStreamDestroy")]
        public static extern ECudaResult CudaStreamDestroy(SCudaStream hstream);

        /// <summary>
        /// Consulta as marcas de um determinado caudal.
        /// </summary>
        /// <param name="hstream">O manuseador do caudal a ser consultado.</param>
        /// <param name="flags">
        /// Apontador para um enumerável onde as marcas serão armazenadas. O valor retornado
        /// é constituído por uma disjunção lógica de todas as marcas que se encontram em utilização.
        /// </param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuStreamGetFlags")]
        public static extern ECudaResult CudaStreamGetFlags(
            SCudaStream hstream,
            ref uint flags);

        /// <summary>
        /// Consulta a prioridade de um determinado caudal.
        /// </summary>
        /// <param name="hstream">Manuseador para o caudal a ser consultado.</param>
        /// <param name="priority">Apontador para um inteiro no qual a prioridade é retornada.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuStreamGetPriority")]
        public static extern ECudaResult CudaStreamGetPriority(SCudaStream hstream, ref int priority);

        /// <summary>
        /// Determina o estado de um caudal computacional.
        /// </summary>
        /// <param name="hstream">O caudal do qual se pretende obter o estado.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorNotReady"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuStreamQuery")]
        public static extern ECudaResult CudaStreamQuery(SCudaStream hstream);

        /// <summary>
        /// Espera até que todas as tarefas no caudal sejam concluídas.
        /// </summary>
        /// <remarks>
        /// Espera até que o disopsitivo complete todas as operações no caudal especificado. Se o
        /// contexto foi especificado com a marca <see cref="ECudaContextFlags.BlockingSync"/>, a linha
        /// de fluxo do CPU irá bloquear até o que o caudal termine com todas as tarefas.
        /// </remarks>
        /// <param name="hstream">O caudal a ser esperado.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuStreamSynchronize")]
        public static extern ECudaResult CudaStreamSynchronize(SCudaStream hstream);

        /// <summary>
        /// Faz com que um caudal computacional espere por um evento.
        /// </summary>
        /// <param name="hstream">O caudal a esperar.</param>
        /// <param name="hevent">O evento a esperar (pode não ser nulo).</param>
        /// <param name="flags">Parâmetros para a operação (deverá ser 0).</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuStreamWaitEvent")]
        public static extern ECudaResult CudaStreamWaitEvent(
            SCudaStream hstream,
            SCudaEvent hevent,
            uint flags);

        #endregion Gestão de caudal

        #region Gestão de eventos

        /// <summary>
        /// Cria um evento.
        /// </summary>
        /// <param name="phevent">Retorna o evento criado.</param>
        /// <param name="flags">As marcas para a criação de eventos.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuEventCreate")]
        public static extern ECudaResult CudaEventCreate(
            ref SCudaEvent phevent,
            ECudaEventFlags flags);

        /// <summary>
        /// Destrói um evento.
        /// </summary>
        /// <remarks>
        /// Se o evento tiver sido gravado mais ainda não tiver sido completado quando a função é chamada,
        /// esta retornará imeditamente e os recursos associados com o evento especificado serão automaticamente
        /// libertados assim que o dispositivo tenha terminado.
        /// </remarks>
        /// <param name="hevent">o evento a ser destruído.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuEventDestroy")]
        public static extern ECudaResult CudaEventDestroy(SCudaEvent hevent);

        /// <summary>
        /// Calcula o tempo decorrido entre dois eventos.
        /// </summary>
        /// <param name="milliseconds">Tempo entre o início e o final em ms.</param>
        /// <param name="hstart">O evento incial.</param>
        /// <param name="hend">O evento final.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorNotReady"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuEventElapsedTime")]
        public static extern ECudaResult CudaEventEllapsedTime(
            ref float milliseconds,
            SCudaEvent hstart,
            SCudaEvent hend);

        /// <summary>
        /// Consulta o estado de um evento.
        /// </summary>
        /// <param name="hevent">O evento a ser consultado.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorNotReady"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuEventQuery")]
        public static extern ECudaResult CudaEventQuery(SCudaEvent hevent);

        /// <summary>
        /// Grava um evento.
        /// </summary>
        /// <param name="hevent">O evento a ser gravado.</param>
        /// <param name="hstream">O caudal para onde gravar o evento.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuEventRecord")]
        public static extern ECudaResult CudaEventRecord(SCudaEvent hevent, SCudaStream hstream);

        /// <summary>
        /// Espera pela conclusão de um evento.
        /// </summary>
        /// <param name="hevent">O evento a esperar.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuEventSynchronize")]
        public static extern ECudaResult CudaEventSynchronize(SCudaEvent hevent);

        #endregion Gestão de eventos

        #region Controlo de execução

        /// <summary>
        /// Retorna informação sobre uma função.
        /// </summary>
        /// <param name="pi">O valor do atributo retornado.</param>
        /// <param name="attrib">O atributo pedido.</param>
        /// <param name="hfunc">A função da qual se pretende consultar o atributo.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuFuncGetAttribute")]
        public static extern ECudaResult CudaFuncGetAttribute(
            ref int pi,
            ECudaFuncAttribute attrib,
            SCudaFunction hfunc);

        /// <summary>
        /// Estabelece a configuração de provisão preferida.
        /// </summary>
        /// <remarks>
        /// Trata-se apenas de uma preferência. O condutor tenta utilizar a configuração definida. No entanto,
        /// pode alterá-la de modo a ser possível executar a função.
        /// </remarks>
        /// <param name="hfunc">O kernel para o qual se pretende configurar a provisão.</param>
        /// <param name="config">A configuração de provisão pretendida.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuFuncSetCacheConfig")]
        public static extern ECudaResult CudaFuncSetCacheConfig(SCudaFunction hfunc, ECudaFuncCache config);

        /// <summary>
        /// Estabelece a configuração de memória partilhada para uma função de dispositivo.
        /// </summary>
        /// <param name="hfunc">O kernel ao qual é atribuída uma configuração de memória partilhada.</param>
        /// <param name="config">A configuração de memória partilhada pretendida.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuFuncSetSharedMemConfig")]
        public static extern ECudaResult CudaFuncSetSharedMemConfig(SCudaFunction hfunc, ECudaSharedConfig config);

        /// <summary>
        /// Lança uma função CUDA.
        /// </summary>
        /// <remarks>
        /// Invoca um kernel numa grelha de gridDimX por gridDimY por gridDimZ de blocos. Cada bloco contém
        /// blockDimX por blocDimY por blocDimZ de linhas de fluxo.
        /// </remarks>
        /// <param name="hfunc">O kernel a ser lançado.</param>
        /// <param name="gridDimX">A largura da grelha em blocos.</param>
        /// <param name="gridDimY">A altura da grelha em blocos.</param>
        /// <param name="gridDimZ">A profundidade da grelha em blocos.</param>
        /// <param name="blockDimX">A dimensão X de cada bloco de execução.</param>
        /// <param name="blockDimY">A dimensão Y de cada bloco de execução.</param>
        /// <param name="blockDimZ">A dimensão Z de cada bloco de execução.</param>
        /// <param name="sharedMemBytes">
        /// O tamanho da memória partilhada dinâmica por bloco de execução em bytes.
        /// </param>
        /// <param name="hstream">O identificador do caudal.</param>
        /// <param name="kernelParams">Vector de apontadores para os parâmetros de kernel.</param>
        /// <param name="extra">Opções extra.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidImage"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchFailed"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchOutOfResources"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchTimeout"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchIncompatibleTexturing"/>,
        /// <see cref="ECudaResult.CudaErrorSharedObjectInitFailed"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuLaunchKernel")]
        public static extern ECudaResult CudaLaunchKernel(
            SCudaFunction hfunc,
            uint gridDimX,
            uint gridDimY,
            uint gridDimZ,
            uint blockDimX,
            uint blockDimY,
            uint blockDimZ,
            uint sharedMemBytes,
            SCudaStream hstream,
            IntPtr kernelParams,
            IntPtr extra);

        /// <summary>
        /// Estabelece as dimensões dos blocos para a função.
        /// </summary>
        /// <param name="hfunc">O kernel do qual se pretende especificar a dimensão.</param>
        /// <param name="x">A dimensão X.</param>
        /// <param name="y">A dimensão Y.</param>
        /// <param name="z">A dimensão Z.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [Obsolete("Dreprecated")]
        [DllImport(DllName, EntryPoint = "cuFuncSetBlockShape")]
        public static extern ECudaResult CudaFuncSetBlockShape(
            SCudaFunction hfunc,
            int x,
            int y,
            int z);

        /// <summary>
        /// Estabelece o tamanho da memória partilhada dinâmica.
        /// </summary>
        /// <param name="hfunc">
        /// O kernel no qual se pretende estabelecer o tamanho da memória partilhada dinâmica.
        /// </param>
        /// <param name="bytes">O tamanho da memória partilhada dinâmcia por linha de fluxo em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DllName, EntryPoint = "cuFuncSetSharedSize")]
        public static extern ECudaResult CudaFuncSetSharedSize(
            SCudaFunction hfunc,
            uint bytes);

        /// <summary>
        /// Lança uma função CUDA.
        /// </summary>
        /// <param name="hfunc">O kernel a ser lançado.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchFailed"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchOutOfResources"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchTimeout"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchIncompatibleTexturing"/>,
        /// <see cref="ECudaResult.CudaErrorSharedObjectInitFailed"/>.
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DllName, EntryPoint = "cuLaunch")]
        public static extern ECudaResult CudaLaunch(SCudaFunction hfunc);

        /// <summary>
        /// Lança uma função CUDA.
        /// </summary>
        /// <param name="hfunc">O kernel a ser lançado.</param>
        /// <param name="gridWidth">A largura da grelha em blocos.</param>
        /// <param name="gridHeight">A altura da grelha em blocos.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchFailed"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchOutOfResources"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchTimeout"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchIncompatibleTexturing"/>,
        /// <see cref="ECudaResult.CudaErrorSharedObjectInitFailed"/>.
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DllName, EntryPoint = "cuLaunchGrid")]
        public static extern ECudaResult CudaLaunchGrid(SCudaFunction hfunc, int gridWidth, int gridHeight);

        /// <summary>
        /// Lança uma função CUDA.
        /// </summary>
        /// <param name="hfunc">O kernel a ser lançado.</param>
        /// <param name="gridWidth">A largura da grelha em blocos.</param>
        /// <param name="gridHeight">A altura da grelha em blocos.</param>
        /// <param name="hstream">O identificador do caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchFailed"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchOutOfResources"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchTimeout"/>,
        /// <see cref="ECudaResult.CudaErrorLaunchIncompatibleTexturing"/>,
        /// <see cref="ECudaResult.CudaErrorSharedObjectInitFailed"/>.
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DllName, EntryPoint = "cuLaunchGridAsync")]
        public static extern ECudaResult CudaLaunchGridAsync(
            SCudaFunction hfunc,
            int gridWidth,
            int gridHeight,
            SCudaStream hstream);

        /// <summary>
        /// Estabelece o parâmetro de tamanho para a função.
        /// </summary>
        /// <param name="hfunc">O kernel no qual se pretende estabelecer o parâmetro de tamanho.</param>
        /// <param name="numbBytes">O tamanho da lista de parâmetros em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DllName, EntryPoint = "cuParamSetSize")]
        public static extern ECudaResult CudaParamSetSize(SCudaFunction hfunc, uint numbBytes);

        /// <summary>
        /// Adiciona uma referência de textura à lista de argumentos da função.
        /// </summary>
        /// <param name="hfunc">O kernel ao qual se pretende adicionar a referência de textura.</param>
        /// <param name="textUnit">
        /// A unidade de textura (tem de ser <see cref="CudaConstants.CudaParamTrDefault"/>).
        /// </param>
        /// <param name="htextRef">A referência da textura a ser adicionada à lista de argumentos.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DllName, EntryPoint = "cuParamSetTexRef")]
        public static extern ECudaResult CudaSetTexRef(SCudaFunction hfunc, int textUnit, SCudaTexRef htextRef);

        /// <summary>
        /// Adiciona um valor de vírgula flutuante à lista de argumentos da função.
        /// </summary>
        /// <param name="hfunc">O kernel ao qual se pretende adicionar o parâmetro.</param>
        /// <param name="offset">A deslocação do parâmetro na lista de parâmetros.</param>
        /// <param name="value">O valor do parâmetro.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DllName, EntryPoint = "cuParamSetf")]
        public static extern ECudaResult CudaParamSetf(SCudaFunction hfunc, int offset, float value);

        /// <summary>
        /// Adiciona um valor inteiro à lista de argumentos da função.
        /// </summary>
        /// <param name="hfunc">O kernel ao qual se pretende adicionar o parâmetro.</param>
        /// <param name="offset">A deslocação do parâmetro na lista de parâmetros.</param>
        /// <param name="value">O valor de parâmetro.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DllName, EntryPoint = "cuParamSeti")]
        public static extern ECudaResult CudaParamSeti(SCudaFunction hfunc, int offset, uint value);

        /// <summary>
        /// Adiciona dados arbitrários à lista de argumentos da função.
        /// </summary>
        /// <param name="hfunc">O kernel ao qual se pretende adicionar o parâmetro.</param>
        /// <param name="offset">A deslocação do parâmetro na lista de parâmetros.</param>
        /// <param name="data">Um apontador para dados arbitrários.</param>
        /// <param name="numBytes">O tamanho dos dados a serem copiados.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DllName, EntryPoint = "cuParamSetv")]
        public static extern ECudaResult CudaParamSetv(
            SCudaFunction hfunc,
            int offset,
            IntPtr data,
            uint numBytes);

        #endregion Controlo de execução

        #region Ocupação

        /// <summary>
        /// Retorna a ocupação de uma função.
        /// </summary>
        /// <remarks>
        /// Retorna em numBlocks o número máximo de blocos activos por multiprocessador de caudal.
        /// </remarks>
        /// <param name="numBlocks">O ocupação da função retornada.</param>
        /// <param name="hfunc">O kernel do qual a ocupação é calculada.</param>
        /// <param name="blockSize">O tamanho do kernel que se pretende lançar em blocos.</param>
        /// <param name="dynamicMemSize">
        /// A utilizadção pretendida da memória partilhada dinâmica por bloco em bytes.
        /// </param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuOccupancyMaxActiveBlocksPerMultiprocessor")]
        public static extern ECudaResult CudaOccupancyMaxActiveBlockPerMultiprocessor(
            ref int numBlocks,
            SCudaFunction hfunc,
            int blockSize,
            SizeT dynamicMemSize);

        /// <summary>
        /// Surgere uma configuração de lançamento com uma ocupação razoável.
        /// </summary>
        /// <param name="minGridSize">
        /// O menor tamanho da grelha necessário para atingir a ocupação máxima retornado.
        /// </param>
        /// <param name="blockSize">
        /// O maior tamanho dos blocos que permite atiginr a ocupação máxima retornado.
        /// </param>
        /// <param name="hfunc">O kernel para o qual a configuração de lançamento é calculada.</param>
        /// <param name="blockSizeToDynamicMemSize">
        /// Uma função que calcula quanta memória partilhada dinâmica por bloco a função func usa
        /// baseando-se no tamanho do bloco.
        /// </param>
        /// <param name="dynamicMemSize">
        /// A utilização de memória partilhada dinâmica pretendida, em bytes.
        /// </param>
        /// <param name="blockSizeLimit">O maior tamanho de bloco para o qual a função está arquitectada.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuOccupancyMaxPotentialBlockSize")]
        public static extern ECudaResult CudaOccupancyMaxPotentialBlockSize(
            ref int minGridSize,
            ref int blockSize,
            SCudaFunction hfunc,
            CudaOccupancyB2DSize blockSizeToDynamicMemSize,
            SizeT dynamicMemSize,
            int blockSizeLimit);

        #endregion Ocupação

        #region Gestão de referências para texturas

        /// <summary>
        /// Obtém o endereço associado com a referência de textura.
        /// </summary>
        /// <param name="pdptr">O endereço de dispositivo retornado.</param>
        /// <param name="htexRef">A referência de textura.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefGetAddress")]
        public static extern ECudaResult CudaTextRefGetAddress(ref SCudaDevicePtr pdptr, SCudaTexRef htexRef);

        /// <summary>
        /// Obtém o modo de endereçamento utilizado por uma referência de textura.
        /// </summary>
        /// <param name="pam">O modo de endereçamento retornado.</param>
        /// <param name="htexRef">A referência de textura.</param>
        /// <param name="dim">A dimensão.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefGetAddressMode")]
        public static extern ECudaResult CudaGetAddressMode(
            ref ECudaAddressMode pam,
            SCudaTexRef htexRef,
            int dim);

        /// <summary>
        /// Obtém uma ligação de vector para uma referência de textura.
        /// </summary>
        /// <param name="phandArray">O vector retornado.</param>
        /// <param name="htexRef">A referência de textura.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefGetArray")]
        public static extern ECudaResult CudaTexRefGetArray(ref SCudaArray phandArray, SCudaTexRef htexRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pfm"></param>
        /// <param name="htextRef"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefGetFilterMode")]
        public static extern ECudaResult CudaTexRefGetFilterMode(ref ECudaFilterMode pfm, SCudaTexRef htextRef);

        /// <summary>
        /// Obtém o modo de filtro utilizado por uma referência de textura.
        /// </summary>
        /// <param name="flags">As marcas retornadas.</param>
        /// <param name="htextRef">A referência de textura.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefGetFlags")]
        public static extern ECudaResult CudaTexRefGetFlags(ref uint flags, SCudaTexRef htextRef);

        /// <summary>
        /// Obtém o formato utilizado por uma referência de textura.
        /// </summary>
        /// <param name="pformat">O formato retornado.</param>
        /// <param name="pnumChannels">O número de componentes retornado.</param>
        /// <param name="htexRef">A referência de textura.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefGetFormat")]
        public static extern ECudaResult CudaTexRefGetFormat(
            ref ECudaArrayFormat pformat,
            ref int pnumChannels,
            SCudaTexRef htexRef);

        /// <summary>
        /// Obtém a anisotropia máxima para uma referência de textura.
        /// </summary>
        /// <param name="pmaxAniso">A anisotropia máxima retornada.</param>
        /// <param name="htextRef">A referência de textura.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefGetMaxAnisotropy")]
        public static extern ECudaResult CudaTextRefGetMaxAnisotropy(ref uint pmaxAniso, SCudaTexRef htextRef);

        /// <summary>
        /// Obtém o modo de filtragem para uma referência de textura mipmapped.
        /// </summary>
        /// <param name="pfm">O modo de filtragem retornado.</param>
        /// <param name="htexRef">A referência para a textura.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefGetMipmapFilterMode")]
        public static extern ECudaResult CudaTextRefGetMinmpaFilterMode(
            ref ECudaFilterMode pfm,
            SCudaTexRef htexRef);

        /// <summary>
        /// Obtém o nível de bias para uma referência de textura mipmapped.
        /// </summary>
        /// <param name="pbias">O nível de bias retornado.</param>
        /// <param name="htexRef">A referência da textura.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefGetMipmapLevelBias")]
        public static extern ECudaResult CudaTextRefGetMinmapLevelBias(ref float pbias, SCudaTexRef htexRef);

        /// <summary>
        /// Obtém o nível de "clamp" para uma referência de textura mipmapped.
        /// </summary>
        /// <param name="pminMipmapLevelClamp">O nível mínimo de "clamp" retornado.</param>
        /// <param name="pmaxMipmapLevelClamp">O nível máximo de "clamp" retornado.</param>
        /// <param name="htexRef">A referência para a textura.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefGetMipmapLevelClamp")]
        public static extern ECudaResult CudaTextRefGetMinmapLevelClamp(
            ref float pminMipmapLevelClamp,
            ref float pmaxMipmapLevelClamp,
            SCudaTexRef htexRef);

        /// <summary>
        /// Otbém o vector mipmapped ligado a uma referência de textura.
        /// </summary>
        /// <param name="phMipmappedArray">O vector mipmapped retornado.</param>
        /// <param name="htexRef">A referência da textura.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefGetMipmappedArray")]
        public static extern ECudaResult CudaTextRefGetMinmappedArray(
            ref SCudaMipmappedArray phMipmappedArray,
            SCudaTexRef htexRef);

        /// <summary>
        /// Liga o endereço a uma referência de textura.
        /// </summary>
        /// <param name="byteOffset">A deslocação retornada em bytes.</param>
        /// <param name="htexRef">A referência da textura.</param>
        /// <param name="dptr">O apontador do dispositivo a ser ligado.</param>
        /// <param name="bytes">Tamanho da memória a ser ligada em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefSetAddress")]
        public static extern ECudaResult CudaTextRefSetAddress(
            ref SizeT byteOffset,
            SCudaTexRef htexRef,
            SCudaDevicePtr dptr,
            SizeT bytes);

        /// <summary>
        /// Liga um endereço como sendo uma referência de textura 2D.
        /// </summary>
        /// <param name="htexRef">A referência de textura a ser ligada.</param>
        /// <param name="desc">O descritor do vector CUDA.</param>
        /// <param name="dptr">O apontador do dispositivo a ser ligado.</param>
        /// <param name="pitch">O passo de linhas em bytes.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefSetAddress2D")]
        public static extern ECudaResult CudaTextRefSetAddress2D(
            SCudaTexRef htexRef,
            ref SCudaArrayDescriptor desc,
            SCudaDevicePtr dptr,
            SizeT pitch);

        /// <summary>
        /// Estabelece o modo de endereçamento de uma referência de textura.
        /// </summary>
        /// <param name="htexRef">A referência de textura.</param>
        /// <param name="dim">A dimensão.</param>
        /// <param name="am">O modo de endereçamento a ser atribuído.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefSetAddressMode")]
        public static extern ECudaResult CudaTextRefSetAddressNode(
            SCudaTexRef htexRef,
            int dim,
            ECudaAddressMode am);

        /// <summary>
        /// Liga um vector a uma referência de textura.
        /// </summary>
        /// <param name="htexRef">A referência de textura a ser ligada.</param>
        /// <param name="handArray">O vector a ser ligado.</param>
        /// <param name="flags">Opcional (tem de ser <see cref="CudaConstants.CudaTrsaOverrideFormat"/>).</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefSetArray")]
        public static extern ECudaResult CudaTextRefSetArray(
            SCudaTexRef htexRef,
            SCudaArray handArray,
            uint flags);

        /// <summary>
        /// Estabelece o modo de filtragem de uma referência de textura.
        /// </summary>
        /// <param name="htexRef">A referência de textura.</param>
        /// <param name="fm">O modo de filtragem a ser atribuído.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefSetFilterMode")]
        public static extern ECudaResult CudaTextRefSetFilterMode(SCudaTexRef htexRef, ECudaFilterMode fm);

        /// <summary>
        /// Estabelece as marcas de uma referência de textura.
        /// </summary>
        /// <param name="htexRef">A referência de textura.</param>
        /// <param name="flags">
        /// As marcas a serem atribuídas (<see cref="CudaConstants.CudaTrsfReadAsInteger"/>
        /// ou <see cref="CudaConstants.CudaTrsfNormalizedCoordinates"/>).
        /// </param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefSetFlags")]
        public static extern ECudaResult CudaTextRefSetFlags(SCudaTexRef htexRef, uint flags);

        /// <summary>
        /// Estabelece o formato para uma referência de textura.
        /// </summary>
        /// <param name="htexRef">A referência de textura.</param>
        /// <param name="fmt">O formato a ser atribuído.</param>
        /// <param name="numPackedComponents">O número de componentes por vector.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefSetFormat")]
        public static extern ECudaResult CudaTextRefSetFormat(
            SCudaTexRef htexRef,
            ECudaArrayFormat fmt,
            int numPackedComponents);

        /// <summary>
        /// Estabelece o máximo de anisotropia para uma referência de textura.
        /// </summary>
        /// <param name="htexRef">A referência de textura.</param>
        /// <param name="maxAniso">A anisotropia máxima.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefSetMaxAnisotropy")]
        public static extern ECudaResult CudaTextRefSetMaxAnisotropy(SCudaTexRef htexRef, uint maxAniso);

        /// <summary>
        /// Estabelece o modo de filtragem mipmapped para uma referência de textura.
        /// </summary>
        /// <param name="htexRef">A referência de textura.</param>
        /// <param name="fm">O modo de filtragem a ser atribuído.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefSetMipmapFilterMode")]
        public static extern ECudaResult CudaTextRefSetMinmapFilterMode(
            SCudaTexRef htexRef,
            ECudaFilterMode fm);

        /// <summary>
        /// Estabelece o nível de bias mipmapped numa referência de textura.
        /// </summary>
        /// <param name="htexRef">A referência de textura.</param>
        /// <param name="bias">O nível de bias mipmapped.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefSetMipmapLevelBias")]
        public static extern ECudaResult CudaTextRefSetMinmapLevelBias(SCudaTexRef htexRef, float bias);

        /// <summary>
        /// Estabelece o nível mínimo/máximo de "clamp" numa referência de textura.
        /// </summary>
        /// <param name="htexRef">A referência de textura.</param>
        /// <param name="minMipmapLevelClamp">O nível de "clamp" mínimo.</param>
        /// <param name="maxMipmapLevelClamp">O nível de "clamp" máximo.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefSetMipmapLevelClamp")]
        public static extern ECudaResult CudaTextRefSetMipmapLevelClamp(
            SCudaTexRef htexRef,
            float minMipmapLevelClamp,
            float maxMipmapLevelClamp);

        /// <summary>
        /// Liga um vector mipmapped a uma referência de textura.
        /// </summary>
        /// <param name="htexRef">A referência de textura.</param>
        /// <param name="handMipmappedArray">O vector mipmapped a ser ligado.</param>
        /// <param name="flags">
        /// Opções (tem de ser <see cref="CudaConstants.CudaTrsaOverrideFormat"/>).
        /// </param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexRefSetMipmappedArray")]
        public static extern ECudaResult CudaTextRefSetMinmappedArray(
            SCudaTexRef htexRef,
            SCudaMipmappedArray handMipmappedArray,
            uint flags);

        /// <summary>
        /// Cria uma referência de textura.
        /// </summary>
        /// <param name="ptexRef">A referência da textura criada retornada.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DllName, EntryPoint = "cuTexRefCreate")]
        public static extern ECudaResult CudaTexRefCreate(ref SCudaTexRef ptexRef);

        /// <summary>
        /// Destrói uma referência de textura.
        /// </summary>
        /// <param name="htexRef">A referência de textura a ser destruída.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DllName, EntryPoint = "cuTexRefDestroy")]
        public static extern ECudaResult CudaTexRefDestroy(SCudaTexRef htexRef);

        #endregion Getão de referências para texturas

        #region Gestão de referências para surfaces

        /// <summary>
        /// Passa a ligação do vector CUDA para uma referência de surface.
        /// </summary>
        /// <param name="phArray">O manuseador do vector.</param>
        /// <param name="hsurfRef">O manuseador da referência de textura.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuSurfRefGetArray")]
        public static extern ECudaResult CudaSurfRefGetArray(ref SCudaArray phArray, SCudaSurfRef hsurfRef);

        /// <summary>
        /// Estabelece um vector CUDA numa referência de surface.
        /// </summary>
        /// <param name="hsurfRef">O manuseador da referência de surface.</param>
        /// <param name="harray">O manuseador do vector CUDA.</param>
        /// <param name="flags">Deverá receber o valor 0.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuSurfRefSetArray")]
        public static extern ECudaResult CudaSurfRefSetArray(
            SCudaSurfRef hsurfRef,
            SCudaArray harray,
            uint flags);

        #endregion Gestão de referências para surfaces

        #region Getsão de objectos de textura

        /// <summary>
        /// Cria um objecto de textura.
        /// </summary>
        /// <param name="ptexObject">O objecto de textura a ser criado.</param>
        /// <param name="presDesc">O descritor do recurso.</param>
        /// <param name="ptexDesc">O descritor da textura.</param>
        /// <param name="presViewDesc">O descritor da vista do recurso.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexObjectCreate")]
        public static extern ECudaResult CudaTexObjectCreate(
            ref SCudaTexObj ptexObject,
            ref SCudaResourceDesc presDesc,
            ref SCudaTextureDesc ptexDesc,
            ref SCudaResourceViewDesc presViewDesc);

        /// <summary>
        /// Destrói um objecto de textura.
        /// </summary>
        /// <param name="texObject">O objecto de textura a ser destruído.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexObjectDestroy")]
        public static extern ECudaResult CudaTexObjectDestroy(SCudaTexObj texObject);

        /// <summary>
        /// Obtém um descritor de recurso do objecto de textura.
        /// </summary>
        /// <param name="presDesc">O descritor do recurso.</param>
        /// <param name="texObject">O objecto de textura.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexObjectGetResourceDesc")]
        public static extern ECudaResult CudaTexObjectGetResourceDesc(
            ref SCudaResourceDesc presDesc,
            SCudaTexObj texObject);

        /// <summary>
        /// Obtém um descritor de vista de recurso do objecto de textura.
        /// </summary>
        /// <param name="presViewDesc">A vista de recurso retornada.</param>
        /// <param name="texObject">O objecto de textura.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexObjectGetResourceViewDesc")]
        public static extern ECudaResult CudaTexObjectGerResourceViewDesc(
            ref SCudaResourceViewDesc presViewDesc,
            SCudaTexObj texObject);

        /// <summary>
        /// Retorna um descritor de textura de um objecto de textura.
        /// </summary>
        /// <param name="ptexDesc">O descritor de textura.</param>
        /// <param name="texObject">O objecto de textura.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuTexObjectGetTextureDesc")]
        public static extern ECudaResult CudaTexObjectGetTextureDesc(
            ref SCudaTextureDesc ptexDesc,
            SCudaTexObj texObject);

        #endregion Gestão de objectos de textura

        #region Gestão de objectos de surfaces

        /// <summary>
        /// Cria um objecto de surface.
        /// </summary>
        /// <param name="psrufObject">O objecto de surface criado.</param>
        /// <param name="presDesc">O descritor de recurso.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuSurfObjectCreate")]
        public static extern ECudaResult CudaSurfObjectCreate(
            ref SCudaSurfObj psrufObject,
            ref SCudaResourceDesc presDesc);

        /// <summary>
        /// Destrói um objecto de surface.
        /// </summary>
        /// <param name="surfObject">O objecto de surface a ser destruído.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuSurfObjectDestroy")]
        public static extern ECudaResult CudaSurfObjectDestroy(SCudaSurfObj surfObject);

        /// <summary>
        /// Retorna o descritor de recurso do objecto de surface.
        /// </summary>
        /// <param name="presDesc">O descritor de recurso retornado.</param>
        /// <param name="surfObject">O objecto de surface.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuSurfObjectGetResourceDesc")]
        public static extern ECudaResult CudaSurfObjectGetResourceDesc(
            ref SCudaResourceDesc presDesc,
            SCudaSurfObj surfObject);

        #endregion Gestão de objectos de surfaces

        #region Contexto de cais para acesso de memória

        /// <summary>
        /// Desabilita o acesso directo a alocações de memória num contexto de cais e elimina o
        /// registo de alocações registadas.
        /// </summary>
        /// <param name="peerContext">
        /// O contexto de cais do qual se pretende desabilitar o acesso.
        /// </param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorPeerAccessNotEnabled"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxDisablePeerAccess")]
        public static extern ECudaResult CudaCtxDisablePeerAccess(SCudaContext peerContext);

        /// <summary>
        /// Habilita o acesso directo a alocações de memória em contextos de cais.
        /// </summary>
        /// <param name="peerContext">
        /// O contexto de cais a ser habilitado o acesso a partir do contexto actual.
        /// </param>
        /// <param name="flags">Reservado para uso futuro e deverá ser sempre 0.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorPeerAccessAlreadyEnabled"/>,
        /// <see cref="ECudaResult.CudaErrorTooManyPeers"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorPeerAccessUnsupported"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxEnablePeerAccess")]
        public static extern ECudaResult CudaCtxEnablePeerAccess(SCudaContext peerContext, uint flags);

        /// <summary>
        /// Consulta se um dispositivo pode aceder directamente à memória de um dispositivo de cais.
        /// </summary>
        /// <param name="canAccessPeer">A capacidade de acesso retornada.</param>
        /// <param name="dev">
        /// O dispositivo a partir do qual as alocações podem ser directamente acedidas.
        /// </param>
        /// <param name="peerDev"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuDeviceCanAccessPeer")]
        public static extern ECudaResult CudaDeviceCanAccessPeer(
            ref int canAccessPeer,
            int dev,
            int peerDev);

        #endregion Contexto de cais para acesso de memória

        #region Interoperabilidade gráfica

        /// <summary>
        /// Mapeia recursos gráficos para acesso CUDA.
        /// </summary>
        /// <param name="count">O número de recursos a serem mapeados.</param>
        /// <param name="resources">Os recursos a serem mapeados para utilização CUDA.</param>
        /// <param name="hstream">O caudal com o qual sincronizar.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorAlreadyMapped"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuGraphicsMapResources")]
        public static extern ECudaResult CudaGraphicsMapResources(
            uint count,
            ref SCudaGraphicsResource resources,
            SCudaStream hstream);

        /// <summary>
        /// Obtém um vector mipmapped através do qual se acede a um recurso gráfico mapeado.
        /// </summary>
        /// <param name="ptrMipmappedArray">
        /// O vector mipmapped retornado através do qual o recurso é acedido.
        /// </param>
        /// <param name="resource">O recurso mapeado a ser acedido.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorNotMapped"/>,
        /// <see cref="ECudaResult.CudaErrorNotMappedAsArray"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuGraphicsResourceGetMappedMipmappedArray")]
        public static extern ECudaResult CudaGraphicsResourceGetMappedMipmappedArray(
            ref SCudaMipmappedArray ptrMipmappedArray,
            SCudaGraphicsResource resource);

        /// <summary>
        /// Obtém um apontador de dispositivo através do qual um recurso gráfico é acedido.
        /// </summary>
        /// <param name="pdevPtr">
        /// O apontador de dispositivo retornado a partir do qual o recurso é acedido.
        /// </param>
        /// <param name="psize">O tamanho retornado do amortecedor acessível.</param>
        /// <param name="resource">O recurso mapeado a ser acedido.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorNotMapped"/>,
        /// <see cref="ECudaResult.CudaErrorNotMappedAsPointer"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuGraphicsResourceGetMappedPointer")]
        public static extern ECudaResult CudaGraphicsResourceGetMappedPointer(
            ref SCudaDevicePtr pdevPtr,
            ref SizeT psize,
            SCudaGraphicsResource resource);

        /// <summary>
        /// Estabelece as marcas de utilização para o mapeamento de recursos gráficos.
        /// </summary>
        /// <param name="resource">O recurso registado ao qual se pretende atribuir as marcas.</param>
        /// <param name="flags">Os parâmetros para o mapeamento de recursos.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorAlreadyMapped"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuGraphicsResourceSetMapFlags")]
        public static extern ECudaResult CudaGraphicsResourceSetMapFlags(
            SCudaGraphicsResource resource,
            ECudaGraphicsMapResourceFlags flags);

        /// <summary>
        /// Obtém um vector através do qual se pode aceder a um sub-recurso de um recurso gráfico
        /// mapeado.
        /// </summary>
        /// <param name="parray">
        /// Vector a partir do qual um sub-recurso de um recurso pode ser acedido.
        /// </param>
        /// <param name="resource">O recurso mapeado a ser acedido.</param>
        /// <param name="arrayIndex">
        /// Índice do vector de texturas ou índice de face cubemap como definido em
        /// <see cref="ECudaArrayCubemapFace"/> para texturas cubemap no sub-recurso a ser acedido.
        /// </param>
        /// <param name="mipLevel"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorNotMapped"/>,
        /// <see cref="ECudaResult.CudaErrorNotMappedAsArray"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuGraphicsSubResourceGetMappedArray")]
        public static extern ECudaResult CudaGraphicsSubResourceGetMappedArray(
            ref SCudaArray parray,
            SCudaGraphicsResource resource,
            uint arrayIndex,
            uint mipLevel);

        /// <summary>
        /// Elmina o mapeamento dos recursos.
        /// </summary>
        /// <param name="count">O número de recursos cujos mapeamentos serão eliminados.</param>
        /// <param name="resources">Os recursos dos quais se pretende eliminar os mapeamentos.</param>
        /// <param name="hstream">O caudal com o qual se vai sincronizar.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorNotMapped"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuGraphicsUnmapResources")]
        public static extern ECudaResult CudaGraphicsUnmapResource(
            uint count,
            [Out] SCudaGraphicsResource[] resources,
            SCudaStream hstream);

        /// <summary>
        /// Elimina o registo de um recurso ser acedido por CUDA.
        /// </summary>
        /// <param name="resource">O recurso cujo registo será eliminado.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuGraphicsUnregisterResource")]
        public static extern ECudaResult CudaGraphicsUnregisterResource(SCudaGraphicsResource resource);

        #endregion Interoperabilidade gráfica

        #region Análise de desempenho

        /// <summary>
        /// Inicializa o monitorizador.
        /// </summary>
        /// <param name="configFile">
        /// O nome do ficheiro de configuração que lista os contadores/opções para a monitorização.
        /// </param>
        /// <param name="outputFile">
        /// Nome do ficheiro de saída onde os dados serão armazenados.
        /// </param>
        /// <param name="outputMode">O modo de saída.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorProfilerDisabled"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuProfilerInitialize")]
        public static extern ECudaResult CudaProfilerInitialize(
            string configFile,
            string outputFile,
            ECudaOutputMode outputMode);

        /// <summary>
        /// Habilita a monitorização.
        /// </summary>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuProfilerStart")]
        public static extern ECudaResult CudaProfilerStart();

        /// <summary>
        /// Desabilita a monitorização.
        /// </summary>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuProfilerStop")]
        public static extern ECudaResult CudaProfilerStop();

        #endregion Análise de desempenho
    }
}
