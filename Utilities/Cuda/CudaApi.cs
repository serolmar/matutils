namespace Utilities.Cuda
{
    using System;
    using System.Runtime.InteropServices;

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
        public static extern ECudaResult CudaGetErrorName(ECudaResult error, ref string ptrStr);

        /// <summary>
        /// Obtém a descrição de um código de erro.
        /// </summary>
        /// <param name="error">O erro.</param>
        /// <param name="ptrStr">O apontador para a descrição.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/> ou <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuGetErrorString")]
        public static extern ECudaResult CudaGetErrorString(ECudaResult error, ref string ptrStr);

        #endregion Gestão dos erros

        #region Inicialização

        /// <summary>
        /// Inicializa o condutor de CUDA e de ser chamda antes de qualquer outra função da API. Actualmente,
        /// o parâmetro das marcas tem de ser 0. Se <see cref="CudaApi.CudaInit"/> não for chamada qualquer
        /// função da API retornará <see cref="ECudaResult.CudaErrorNotInitialized"/>.
        /// </summary>
        /// <remarks>
        /// Actuamente o parâmetro <see cref="flags"/> apenas suporta o valor 0.
        /// </remarks>
        /// <param name="flags">As marcas de inicialização.</param>
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
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/> se o argumento <see cref="driverVersion"/> for nulo.
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
        public static extern ECudaResult CudaDeviceGet(ref SCudaDevice device, int ordinal);

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
            SCudaDevice device);

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
        [DllImport(DllName, EntryPoint = "cuDeviceGetName")]
        public static extern ECudaResult CudaDeviceGetName([Out] char[] name, int len, SCudaDevice dev);

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
        public static extern ECudaResult CudaDeviceTotalMem(ref SizeT bytes, SCudaDevice dev);

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
            SCudaDevice dev);

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
        public static extern ECudaResult CudaDeviceGetProperties(ref SCudaDevProp prop, SCudaDevice dev);

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
        /// restaurado por intermédio da chamada à função <see cref="CudaApi.CudaCxtPopCurrent"/>. Os três LSBs
        /// do parâmetro das marcas podem ser utilizados para controlar a linha de fluxo do sistema operativo
        /// que é proprietária do contexto no instante de chamada da Api, interage com o calenderizador do
        /// sistema operativo quando se encontra a aguardar resultados do GPU. Apenas uma das marcas de
        /// calenderização pode ser atribuída durante a criação do contexto.
        /// Os valores permitidos para o parâmetro das marcas são:
        /// <list type="bullet">
        /// <term><see cref="ECudaContexFlags.SchedAuto"/></term>
        /// <description>
        /// O valor por defeito se o parâmetro das marcas receber o valor 0, utiliza uma heurística baseada
        /// no número de contextos CUDA activos no processo C e o número de processadores lógicos no sistema P.
        /// Se C>P, então o ambiente CUDA remete para outras linhas de fluxo enquanto espera pelo GPU, caso
        /// contário o ambiente CUDA não remete enquanto se encontra à espera de resultados e roda activamente
        /// no processador.
        /// </description>
        /// </list>
        /// <list type="bullet">
        /// <term><see cref="ECudaContexFlags.SchedSpin"/></term>
        /// <description>
        /// Instrui o ambiente CUDA para rodar activamente enquanto se encontrar à espera de resultados do
        /// GPU, mas pode diminuir o desempenho das linhas de fluxo do CPU se estas se encontrarem a realizar
        /// trabalho em paralelo com as linhas de fluxo CUDA.
        /// </description>
        /// </list>
        /// <list type="bullet">
        /// <term><see cref="ECudaContexFlags.ScheddYield"/></term>
        /// <description>
        /// Instrui o ambiente CUDA para remeter a sua linha de fluxo enquanto espera resultados do GPU. Isto pode
        /// aumentar a latência enquanto se encontrar à espera do GPU mas pode aumentar o desempenho das linhas
        /// de fluxo do CPU que se encontram a operar em paralelo com as de CUDA.
        /// </description>
        /// </list>
        /// <list type="bullet">
        /// <term><see cref="ECudaContexFlags.SchedBlockingSync"/></term>
        /// <description>
        /// Instrui o ambiente CUDA para bloquear a linha de fluxo do CPU numa primitiva de sincronização enquanto
        /// se enconta à espera que o GPU termine.
        /// </description>
        /// </list>
        /// <list type="bullet">
        /// <term><see cref="ECudaContexFlags.BlockingSync"/></term>
        /// <description>
        /// Instrui o ambiente CUDA para bloquear a linha de fluxo do CPU numa primitiva de sincronização enquanto
        /// se encontra à espera que o GPU termine. Esta marca encontra-se obsoleta desde a versão CUDA 4.0 e
        /// foi subsituída pela <see cref="ECudaContexFlags.SchedBlockingSync"/>.
        /// </description>
        /// </list>
        /// <list type="bullet">
        /// <term><see cref="ECudaContexFlags.MapHost"/></term>
        /// <description>
        /// Instrui o ambiente CUDA para suportar alocações mapeadas ao anfitrião. Esta marca deverá ser
        /// atribuída se se pretender mapear memória do anfitrião seleccionada para ser acedida pelo GPU.
        /// </description>
        /// </list>
        /// <list type="bullet">
        /// <term><see cref="ECudaContexFlags.LmemResizeToMax"/></term>
        /// <description>
        /// Instrui o ambiente CUDA para não reduzir a memória local após uma alteração da memória local para
        /// um kernel. Isto pode prevenir o entulhamento por alocações de memória locais durante o lançamento
        /// de vários kernels com uma grande utilização de memória local sob a pena de aumentar potencialmente
        /// a utilização de memória.
        /// </description>
        /// </list>
        /// A criação do contexto irã falhar com a mensagem <see cref="ECudaResult.Unknwn"/> se o modo de
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
            ECudaContexFlags flags, 
            SCudaDevice dev);

        /// <summary>
        /// Destrói um contexto CUDA.
        /// </summary>
        /// <param name="ctx">O contexto a ser destruído.</param>
        /// <returns>
        /// <remarks>
        /// Destói o contexto CUDA especificado pelo parâmetro <see cref="ctx"/>. O contexto será destruído
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
        /// Retorna num número de versão no parâmetro <see cref="version"/> que corresponde às capacidades do
        /// contexto (exemplo, 3010 ou 3020).
        /// </remarks>
        /// <param name="ctx">O contexto.</param>
        /// <param name="version">A versão.</param>
        /// <returns><see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuCtxGetApiVersion")]
        public static extern ECudaResult CudaGetApiVersion(SCudaContext ctx, ref uint version);

        [DllImport(DllName, EntryPoint = "cuCtxGetCacheConfig")]
        public static extern ECudaResult CudaCtxGetCacheConfig(ref ECudaFuncCache pconfig);

        [DllImport(DllName, EntryPoint = "cuCtxGetCurrent")]
        public static extern ECudaResult CudaCtxGetCurrent(ref SCudaContext pctx);

        [DllImport(DllName, EntryPoint = "cuCtxGetDevice")]
        public static extern ECudaResult CudaCtxGetDevice(ref SCudaDevice device);

        [DllImport(DllName, EntryPoint = "cuCtxGetLimit")]
        public static extern ECudaResult CudaCtxGetLimit(ref SizeT pvalue, ECudaLimit limit);

        [DllImport(DllName, EntryPoint = "cuCtxGetSharedMemConfig")]
        public static extern ECudaResult CudaCtxGetSharedMemConfig(ref ECudaSharedConfig pconfig);

        [DllImport(DllName, EntryPoint = "cuCtxGetStreamPriorityRange")]
        public static extern ECudaResult CudaCtxGetStreamPriorityRange(
            ref int leastPriority, 
            ref int greatestPriority);

        [DllImport(DllName, EntryPoint = "cuCtxPopCurrent")]
        public static extern ECudaResult CudaCtxPopCurrent(ref SCudaContext ctx);

        [DllImport(DllName, EntryPoint = "cuCtxPushCurrent")]
        public static extern ECudaResult CudaCtxPushCurrent(ref SCudaContext ctx);

        [DllImport(DllName, EntryPoint = "cuCtxSetCacheConfig")]
        public static extern ECudaResult CudaCtxSetCacheConfig(ECudaFuncCache config);

        [DllImport(DllName, EntryPoint = "cuCtxSetCurrent")]
        public static extern ECudaResult CudaCtxSetCurrent(SCudaContext ctx);

        [DllImport(DllName, EntryPoint = "cuCtxSetLimit")]
        public static extern ECudaResult CudaCtxSetLimit(ECudaLimit limit, SizeT value);

        [DllImport(DllName, EntryPoint = "cuCtxSetSharedMemConfig")]
        public static extern ECudaResult CudaCtxSetSharedMemConfig(ECudaSharedConfig config);

        [DllImport(DllName, EntryPoint = "cuCtxSynchronize")]
        public static extern ECudaResult CudaCtxSynchronize();

        #endregion Gestão de contextos
    }
}
