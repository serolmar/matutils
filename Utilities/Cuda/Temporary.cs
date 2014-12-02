namespace Utilities.Cuda
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class Temporary
    {
        internal const string DLLName = "nvcuda";

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
        [DllImport(DLLName, EntryPoint = "cuArray3DCreate")]
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
        [DllImport(DLLName, EntryPoint = "cuArray3DGetDescriptor")]
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
        [DllImport(DLLName, EntryPoint = "cuArrayCreate")]
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
        [DllImport(DLLName, EntryPoint = "cuArrayDestroy")]
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
        [DllImport(DLLName, EntryPoint = "cuArrayGetDescriptor")]
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
        [DllImport(DLLName, EntryPoint = "cuDeviceGetByPCIBusId")]
        public static extern ECudaResult CudaDeviceGetByPCIBusId(
            ref SCudaDevice dev,
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
        [DllImport(DLLName, EntryPoint = "cuDeviceGetPCIBusId")]
        public static extern ECudaResult CudaDeviceGetPCIBusId(
            ref string pciBusId,
            int len,
            SCudaDevice dev);

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
        [DllImport(DLLName, EntryPoint = "cuIpcCloseMemHandle")]
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
        [DllImport(DLLName, EntryPoint = "cuIpcGetEventHandle")]
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
        [DllImport(DLLName, EntryPoint = "cuIpcGetMemHandle")]
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
        [DllImport(DLLName, EntryPoint = "cuIpcOpenEventHandle")]
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
        [DllImport(DLLName, EntryPoint = "cuIpcOpenMemHandle")]
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
        [DllImport(DLLName, EntryPoint = "cuMemAlloc")]
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
        [DllImport(DLLName, EntryPoint = "cuMemAllocHost")]
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
        /// <see cref="ECudaResultCudaErrorNotSupported"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemAllocManaged")]
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
        [DllImport(DLLName, EntryPoint = "cuMemAllocPitch")]
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
        [DllImport(DLLName, EntryPoint = "cuMemFree")]
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
        [DllImport(DLLName, EntryPoint = "cuMemFreeHost")]
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
        [DllImport(DLLName, EntryPoint = "cuMemGetAddressRange")]
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
        [DllImport(DLLName, EntryPoint = "cuMemGetInfo")]
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
        /// <param name="pp"><Apontador de anfitrião para a memória de fecho por paginação./param>
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
        [DllImport(DLLName, EntryPoint = "cuMemHostAlloc")]
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
        [DllImport(DLLName, EntryPoint = "cuMemHostGetDevicePointer")]
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
        [DllImport(DLLName, EntryPoint = "cuMemHostGetFlags")]
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
        [DllImport(DLLName, EntryPoint = "cuMemHostRegister")]
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
        [DllImport(DLLName, EntryPoint = "cuMemHostUnregister")]
        public static extern ECudaResult CudaMemHostUnregister(IntPtr p);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="src"></param>
        /// <param name="byteCount"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpy")]
        public static extern ECudaResult CudaMemcpy(SCudaDevicePtr dst, SCudaDevicePtr src, SizeT byteCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptrCopy"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpy2D")]
        public static extern ECudaResult CudaMemcpy2D(ref SCudaMemCpy2D ptrCopy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptrCopy"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpy2DAsync")]
        public static extern ECudaResult CudaMemcpy2DAsync(ref SCudaMemCpy2D ptrCopy, SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptrCopy"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpy2DUnaligned")]
        public static extern ECudaResult CudaMemcpy2DUnaligned(ref SCudaMemCpy2D ptrCopy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptrCopy"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpy3D")]
        public static extern ECudaResult CudaMemcpy3D(ref SCudaMemCpy3D ptrCopy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptrCopy"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpy3DAsync")]
        public static extern ECudaResult CudaMemcpy3DAsync(ref SCudaMemCpy3D ptrCopy, SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptrCopy"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpy3DPeer")]
        public static extern ECudaResult CudaMemcpy3DPeer(ref SCudaMemCpy3DPeer ptrCopy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptrCopy"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpy3DPeerAsync")]
        public static extern ECudaResult CudaMemcpy3DPeerAsync(ref SCudaMemCpy3DPeer ptrCopy, SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="src"></param>
        /// <param name="byteCount"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyAsync")]
        public static extern ECudaResult CudaMemcpyAsync(
            SCudaDevicePtr dst,
            SCudaDevicePtr src,
            SizeT byteCount,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstArray"></param>
        /// <param name="dstOffset"></param>
        /// <param name="srcArray"></param>
        /// <param name="srcOffset"></param>
        /// <param name="byteCount"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyAtoA")]
        public static extern ECudaResult CudaMemcpyAtoA(
            SCudaArray dstArray,
            SizeT dstOffset,
            SCudaArray srcArray,
            SizeT srcOffset,
            SizeT byteCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="srcArray"></param>
        /// <param name="srcOffset"></param>
        /// <param name="byteCount"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyAtoD")]
        public static extern ECudaResult CudaMemcpyAtoD(
            SCudaDevicePtr dstDevice,
            SCudaArray srcArray,
            SizeT srcOffset,
            SizeT byteCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstHost"></param>
        /// <param name="srcArray"></param>
        /// <param name="srcOffset"></param>
        /// <param name="byteCount"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyAtoH")]
        public static extern ECudaResult CudaMemcpyAtoH(
            IntPtr dstHost,
            SCudaArray srcArray,
            SizeT srcOffset,
            SizeT byteCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstHost"></param>
        /// <param name="srcArray"></param>
        /// <param name="srcOffset"></param>
        /// <param name="byteCount"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyAtoHAsync")]
        public static extern ECudaResult CudaMemcpyAtoHAsync(
            IntPtr dstHost,
            SCudaArray srcArray,
            SizeT srcOffset,
            SizeT byteCount,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstArray"></param>
        /// <param name="dstOffset"></param>
        /// <param name="srcDevice"></param>
        /// <param name="byteCount"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoA")]
        public static extern ECudaResult CudaMemcpyDtoA(
            SCudaArray dstArray,
            SizeT dstOffset,
            SCudaDevicePtr srcDevice,
            SizeT byteCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="srcDevice"></param>
        /// <param name="byteCount"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoD")]
        public static extern ECudaResult CudaMemcpyDtoD(
            SCudaDevicePtr dstDevice,
            SCudaDevicePtr srcDevice,
            SizeT byteCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="srcDevice"></param>
        /// <param name="byteCount"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoDAsync")]
        public static extern ECudaResult CudaMemcpyDtoDAsync(
            SCudaDevicePtr dstDevice,
            SCudaDevicePtr srcDevice,
            SizeT byteCount,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstHost"></param>
        /// <param name="srcDevice"></param>
        /// <param name="byteCount"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoH")]
        public static extern ECudaResult CudaMemcpyDtoH(
            IntPtr dstHost,
            SCudaDevicePtr srcDevice,
            SizeT byteCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstHost"></param>
        /// <param name="srcDevice"></param>
        /// <param name="byteCount"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoHAsync")]
        public static extern ECudaResult CudaMemcpyDtoHAsync(
            IntPtr dstHost,
            SCudaDevicePtr srcDevice,
            SizeT byteCount,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstArray"></param>
        /// <param name="dstOffset"></param>
        /// <param name="srcHost"></param>
        /// <param name="byteCount"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyHtoA")]
        public static extern ECudaResult CudaMemcpyHtoA(
            SCudaArray dstArray,
            SizeT dstOffset,
            IntPtr srcHost,
            SizeT byteCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstArray"></param>
        /// <param name="dstOffset"></param>
        /// <param name="srcHost"></param>
        /// <param name="byteCount"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyHtoAAsync")]
        public static extern ECudaResult CudaMemcpyHtoAAsync(
            SCudaArray dstArray,
            SizeT dstOffset,
            IntPtr srcHost,
            SizeT byteCount,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="srcHost"></param>
        /// <param name="byteCount"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyHtoD")]
        public static extern ECudaResult CudaMemcpyHtoD(
            SCudaDevicePtr dstDevice,
            IntPtr srcHost,
            SizeT byteCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="srcHost"></param>
        /// <param name="byteCount"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyHtoDAsync")]
        public static extern ECudaResult CudaMemcpyHtoDAsync(
            SCudaDevicePtr dstDevice,
            IntPtr srcHost,
            SizeT byteCount,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="dstContext"></param>
        /// <param name="srcDevice"></param>
        /// <param name="srcContext"></param>
        /// <param name="byteCount"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyPeer")]
        public static extern ECudaResult CudaMemcpyPeer(
            SCudaDevicePtr dstDevice,
            SCudaContext dstContext,
            SCudaDevicePtr srcDevice,
            SCudaContext srcContext,
            SizeT byteCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="dstContext"></param>
        /// <param name="srcDevice"></param>
        /// <param name="srcContext"></param>
        /// <param name="byteCount"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemcpyPeerAsync")]
        public static extern ECudaResult CudaMemcpyPeerAsync(
            SCudaDevicePtr dstDevice,
            SCudaContext dstContext,
            SCudaDevicePtr srcDevice,
            SCudaContext srcContext,
            SizeT byteCount,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="us"></param>
        /// <param name="n"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemsetD16")]
        public static extern ECudaResult CudaMemsetD16(
            SCudaDevicePtr dstDevice,
            ushort us,
            SizeT n);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="us"></param>
        /// <param name="n"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemsetD16Async")]
        public static extern ECudaResult CudaMemsetD16Async(
            SCudaDevicePtr dstDevice,
            ushort us,
            SizeT n,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="dstPitch"></param>
        /// <param name="us"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemsetD2D16")]
        public static extern ECudaResult CudaMemsetD2D16(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch, ushort us,
            SizeT width,
            SizeT height);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="dstPitch"></param>
        /// <param name="us"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemsetD2D16Async")]
        public static extern ECudaResult CudaMemsetD2D16Async(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            ushort us,
            SizeT width,
            SizeT height,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="dstPitch"></param>
        /// <param name="ui"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemsetD2D32")]
        public static extern ECudaResult CudaMemsetD2D32(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            uint ui,
            SizeT width,
            SizeT height);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="dstPitch"></param>
        /// <param name="ui"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemsetD2D32Async")]
        public static extern ECudaResult CudaMemsetD2D32Async(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            uint ui,
            SizeT width,
            SizeT height,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="dstPitch"></param>
        /// <param name="uc"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemsetD2D8")]
        public static extern ECudaResult CudaMemsetD2D8(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            byte uc,
            SizeT width,
            SizeT height);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="dstPitch"></param>
        /// <param name="uc"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemsetD2D8Async")]
        public static extern ECudaResult CudaMemsetD2D8Async(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            byte uc,
            SizeT width,
            SizeT height,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="ui"></param>
        /// <param name="n"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemsetD32")]
        public static extern ECudaResult CudaMemsetD32(SCudaDevicePtr dstDevice, uint ui, SizeT n);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="ui"></param>
        /// <param name="n"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemsetD32Async")]
        public static extern ECudaResult CudaMemsetD32Async(
            SCudaDevicePtr dstDevice,
            uint ui,
            SizeT n,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="uc"></param>
        /// <param name="n"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemsetD8")]
        public static extern ECudaResult CudaMemsetD8(SCudaDevicePtr dstDevice, byte uc, SizeT n);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstDevice"></param>
        /// <param name="uc"></param>
        /// <param name="n"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMemsetD8Async")]
        public static extern ECudaResult CudaMemsetD8Async(
            SCudaDevicePtr dstDevice,
            byte uc,
            SizeT n,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phandle"></param>
        /// <param name="pMipmappedArrayDesc"></param>
        /// <param name="numMipmapLevels"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMipmappedArrayCreate")]
        public static extern ECudaResult CudaMipmappedArrayCreate(
            ref SCudaMipmappedArray phandle,
            ref SCudaArray3DDescriptor pMipmappedArrayDesc,
            uint numMipmapLevels);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hndMipmappedArray"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMipmappedArrayDestroy")]
        public static extern ECudaResult CudaMipmappedArrayDestroy(SCudaMipmappedArray hndMipmappedArray);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptrLevelArray"></param>
        /// <param name="hndMipmappedArray"></param>
        /// <param name="level"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuMipmappedArrayGetLevel")]
        public static extern ECudaResult CudaMipmappedArrayGetLevel(
            ref  SCudaArray ptrLevelArray,
            SCudaMipmappedArray hndMipmappedArray, uint level);

        #endregion Gestão de memória

        #region Endereçamento unificado

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="attribute"></param>
        /// <param name="ptr"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuPointerGetAttribute")]
        public static extern ECudaResult CudaPointerGetAttribute(
            IntPtr data,
            ECudaPointerAttribute attribute,
            SCudaDevicePtr ptr);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="attribute"></param>
        /// <param name="ptr"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuPointerSetAttribute")]
        public static extern ECudaResult CudaPointerSetAttribute(
            IntPtr value,
            ECudaPointerAttribute attribute,
            SCudaDevicePtr ptr);

        #endregion Endereçamento unificado

        #region Gestão de caudal

        /// <summary>
        /// O caudal ao qual se pretende adicionar a função que permite lidar com o evento despoletado quando
        /// todos os itens introduzidos terminarem.
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
        [DllImport(DLLName, EntryPoint = "cuStreamAddCallback")]
        public static extern ECudaResult CudaStreamAddCallback(
            SCudaStream hstream,
            CudaStreamCallback callback,
            IntPtr userData,
            uint flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hstream"></param>
        /// <param name="dptr"></param>
        /// <param name="length"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorNotSupported"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuStreamAttachMemAsync")]
        public static extern ECudaResult CudaStreamAttachMemAsync(
            SCudaStream hstream,
            SCudaDevicePtr dptr,
            SizeT length);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phstream"></param>
        /// <param name="flags"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuStreamCreate")]
        public static extern ECudaResult CudaStreamCreate(ref SCudaStream phstream, uint flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phstream"></param>
        /// <param name="flags"></param>
        /// <param name="priority"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuStreamCreateWithPriority")]
        public static extern ECudaResult CudaStreamCreateWithPriority(
            ref SCudaStream phstream,
            uint flags,
            int priority);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuStreamDestroy")]
        public static extern ECudaResult CudaStreamDestroy(SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hstream"></param>
        /// <param name="flags"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuStreamGetFlags")]
        public static extern ECudaResult CudaStreamGetFlags(SCudaStream hstream, ref uint flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hstream"></param>
        /// <param name="priority"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuStreamGetPriority")]
        public static extern ECudaResult CudaGetPriority(SCudaStream hstream, ref int priority);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorNotReady"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuStreamQuery")]
        public static extern ECudaResult CudaStreamQuery(SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hstream"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuStreamSynchronize")]
        public static extern ECudaResult CudaStreamSynchronize(SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hstream"></param>
        /// <param name="hevent"></param>
        /// <param name="flags"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuStreamWaitEvent")]
        public static extern ECudaResult CudaStreamWaitEvent(
            SCudaStream hstream,
            SCudaEvent hevent,
            uint flags);

        #endregion Gestão de caudal

        #region Gestão de eventos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phevent"></param>
        /// <param name="flags"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuEventCreate")]
        public static extern ECudaResult CudaEventCreate(ref SCudaEvent phevent, uint flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hevent"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuEventDestroy")]
        public static extern ECudaResult CudaEventDestroy(SCudaEvent hevent);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <param name="hstart"></param>
        /// <param name="hend"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuEventElapsedTime")]
        public static extern ECudaResult CudaEventEllapsedTime(
            ref float milliseconds,
            SCudaEvent hstart,
            SCudaEvent hend);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hevent"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuEventQuery")]
        public static extern ECudaResult CudaEventQuery(SCudaEvent hevent);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hevent"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuEventRecord")]
        public static extern ECudaResult CudaEventRecord(SCudaEvent hevent, SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hevent"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuEventSynchronize")]
        public static extern ECudaResult CudaEventSynchronize(SCudaEvent hevent);

        #endregion Gestão de eventos

        #region Controlo de execução

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="attrib"></param>
        /// <param name="hfunc"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuFuncGetAttribute")]
        public static extern ECudaResult CudaFuncGetAttribute(
            ref int pi,
            ECudaFuncAttribute attrib,
            SCudaFunction hfunc);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hfunc"></param>
        /// <param name="config"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuFuncSetCacheConfig")]
        public static extern ECudaResult CudaSetCacheConfig(SCudaFunction hfunc, ECudaFuncCache config);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hfunc"></param>
        /// <param name="config"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuFuncSetSharedMemConfig")]
        public static extern ECudaResult CudaSetSharedMemConfig(SCudaFunction hfunc, ECudaSharedConfig config);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hfunc"></param>
        /// <param name="gridDimX"></param>
        /// <param name="gridDimY"></param>
        /// <param name="freidDimZ"></param>
        /// <param name="blockDimX"></param>
        /// <param name="blockDimY"></param>
        /// <param name="blockDimZ"></param>
        /// <param name="sharedMemBytes"></param>
        /// <param name="hstream"></param>
        /// <param name="kernelParams"></param>
        /// <param name="extra"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuLaunchKernel")]
        public static extern ECudaResult CudaLaunchKernel(
            SCudaFunction hfunc,
            uint gridDimX,
            uint gridDimY,
            uint freidDimZ,
            uint blockDimX,
            uint blockDimY,
            uint blockDimZ,
            uint sharedMemBytes,
            SCudaStream hstream,
            IntPtr kernelParams,
            IntPtr extra);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hfunc"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns>
        /// 
        /// </returns>
        [Obsolete("Dreprecated")]
        [DllImport(DLLName, EntryPoint = "cuFuncSetBlockShape")]
        public static extern ECudaResult CudaFuncSetBlockShape(
            SCudaFunction hfunc,
            int x,
            int y,
            int z);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hfunc"></param>
        /// <param name="bytes"></param>
        /// <returns>
        /// 
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuFuncSetSharedSize")]
        public static extern ECudaResult CudaFuncSetSharedSize(
            SCudaFunction hfunc,
            uint bytes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hfunc"></param>
        /// <returns>
        /// 
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuLaunch")]
        public static extern ECudaResult CudaLaunch(SCudaFunction hfunc);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hfunc"></param>
        /// <param name="gridWidth"></param>
        /// <param name="gridHeight"></param>
        /// <returns>
        /// 
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuLaunchGrid")]
        public static extern ECudaResult CudaLaunchGrid(SCudaFunction hfunc, int gridWidth, int gridHeight);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hfunc"></param>
        /// <param name="gridWidth"></param>
        /// <param name="gridHeight"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// 
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuLaunchGridAsync")]
        public static extern ECudaResult CudaLaunchGridAsync(
            SCudaFunction hfunc,
            int gridWidth,
            int gridHeight,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hfunc"></param>
        /// <param name="numbBytes"></param>
        /// <returns>
        /// 
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuParamSetSize")]
        public static extern ECudaResult CudaParamSetSize(SCudaFunction hfunc, uint numbBytes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hfunc"></param>
        /// <param name="textUnit"></param>
        /// <param name="htextRef"></param>
        /// <returns>
        /// 
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuParamSetTexRef")]
        public static extern ECudaResult CudaSetTexRef(SCudaFunction hfunc, int textUnit, SCudaTexRef htextRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hfunc"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <returns>
        /// 
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuParamSetf")]
        public static extern ECudaResult CudaParamSetf(SCudaFunction hfunc, int offset, float value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hfunc"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <returns>
        /// 
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuParamSeti")]
        public static extern ECudaResult CudaParamSeti(SCudaFunction hfunc, int offset, uint value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hfunc"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <returns>
        /// 
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuParamSetv")]
        public static extern ECudaResult CudaParamSetv(SCudaFunction hfunc, int offset, int value);

        #endregion Controlo de execução

        #region Ocupação

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numBlocks"></param>
        /// <param name="hfunc"></param>
        /// <param name="blockSize"></param>
        /// <param name="dynamicMemSize"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuOccupancyMaxActiveBlocksPerMultiprocessor")]
        public static extern ECudaResult CudaOccupancyMaxActiveBlockPerMultiprocessor(
            ref int numBlocks,
            SCudaFunction hfunc,
            int blockSize,
            SizeT dynamicMemSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minGridSize"></param>
        /// <param name="blockSize"></param>
        /// <param name="hfunc"></param>
        /// <param name="blockSizeToDynamicMemSize"></param>
        /// <param name="dynamicMemSize"></param>
        /// <param name="blockSizeLimit"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuOccupancyMaxPotentialBlockSize")]
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
        /// 
        /// </summary>
        /// <param name="pdptr"></param>
        /// <param name="htexRef"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefGetAddress")]
        public static extern ECudaResult CudaTextRefGetAddress(ref SCudaDevicePtr pdptr, SCudaTexRef htexRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pam"></param>
        /// <param name="htexRef"></param>
        /// <param name="dim"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefGetAddressMode")]
        public static extern ECudaResult CudaGetAddressMode(
            ref ECudaAddressMode pam,
            SCudaTexRef htexRef,
            int dim);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phandArray"></param>
        /// <param name="htexRef"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefGetArray")]
        public static extern ECudaResult CudaTexRefGetArray(ref SCudaArray phandArray, SCudaTexRef htexRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pfm"></param>
        /// <param name="htextRef"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefGetFilterMode")]
        public static extern ECudaResult CudaTexRefGetFilterMode(ref ECudaFilterMode pfm, SCudaTexRef htextRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="htextRef"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefGetFlags")]
        public static extern ECudaResult CudaTexRefGetFlags(ref uint flags, SCudaTexRef htextRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pformat"></param>
        /// <param name="pnumChannels"></param>
        /// <param name="htexRef"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefGetFormat")]
        public static extern ECudaResult CudaTexRefGetFormat(
            ref ECudaArrayFormat pformat,
            ref int pnumChannels,
            SCudaTexRef htexRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pmaxAniso"></param>
        /// <param name="htextRef"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefGetMaxAnisotropy")]
        public static extern ECudaResult CudaTextRefGetMaxAnisotropy(ref uint pmaxAniso, SCudaTexRef htextRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pfm"></param>
        /// <param name="htexRef"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefGetMipmapFilterMode")]
        public static extern ECudaResult CudaTextRefGetMinmpaFilterMode(
            ref ECudaFilterMode pfm,
            SCudaTexRef htexRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pbias"></param>
        /// <param name="htexRef"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefGetMipmapLevelBias")]
        public static extern ECudaResult CudaTextRefGetMinmapLevelBias(ref float pbias, SCudaTexRef htexRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pminMipmapLevelClamp"></param>
        /// <param name="pmaxMipmapLevelClamp"></param>
        /// <param name="htexRef"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefGetMipmapLevelClamp")]
        public static extern ECudaResult CudaTextRefGetMinmapLevelClamp(
            ref float pminMipmapLevelClamp,
            ref float pmaxMipmapLevelClamp,
            SCudaTexRef htexRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phMipmappedArray"></param>
        /// <param name="htexRef"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefGetMipmappedArray")]
        public static extern ECudaResult CudaTextRefGetMinmappedArray(
            ref SCudaMipmappedArray phMipmappedArray,
            SCudaTexRef htexRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteOffset"></param>
        /// <param name="htexRef"></param>
        /// <param name="dptr"></param>
        /// <param name="bytes"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetAddress")]
        public static extern ECudaResult CudaTextRefSetAddress(
            ref SizeT byteOffset,
            SCudaTexRef htexRef,
            SCudaDevicePtr dptr,
            SizeT bytes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <param name="desc"></param>
        /// <param name="dptr"></param>
        /// <param name="pitch"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetAddress2D")]
        public static extern ECudaResult CudaTextRefSetAddress2D(
            SCudaTexRef htexRef,
            ref SCudaArrayDescriptor desc,
            SCudaDevicePtr dptr,
            SizeT pitch);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <param name="dim"></param>
        /// <param name="am"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetAddressMode")]
        public static extern ECudaResult CudaTextRefSetAddressNode(
            SCudaTexRef htexRef,
            int dim,
            ECudaAddressMode am);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <param name="handArray"></param>
        /// <param name="flags"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetArray")]
        public static extern ECudaResult CudaTextRefSetArray(
            SCudaTexRef htexRef,
            SCudaArray handArray,
            uint flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <param name="fm"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetFilterMode")]
        public static extern ECudaResult CudaTextRefSetFilterMode(SCudaTexRef htexRef, ECudaFilterMode fm);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <param name="flags"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetFlags")]
        public static extern ECudaResult CudaTextRefSetFlags(SCudaTexRef htexRef, uint flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <param name="fmt"></param>
        /// <param name="numPackedComponents"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetFormat")]
        public static extern ECudaResult CudaTextRefSetFormat(
            SCudaTexRef htexRef,
            ECudaArrayFormat fmt,
            int numPackedComponents);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <param name="maxAniso"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetMaxAnisotropy")]
        public static extern ECudaResult CudaTextRefSetMaxAnisotropy(SCudaTexRef htexRef, uint maxAniso);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <param name="fm"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetMipmapFilterMode")]
        public static extern ECudaResult CudaTextRefSetMinmapFilterMode(
            SCudaTexRef htexRef,
            ECudaFilterMode fm);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <param name="bias"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetMipmapLevelBias")]
        public static extern ECudaResult CudaTextRefSetMinmapLevelBias(SCudaTexRef htexRef, float bias);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <param name="minMipmapLevelClamp"></param>
        /// <param name="maxMipmapLevelClamp"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetMipmapLevelClamp")]
        public static extern ECudaResult CudaTextRefSetMipmapLevelClamp(
            SCudaTexRef htexRef,
            float minMipmapLevelClamp,
            float maxMipmapLevelClamp);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <param name="handMipmappedArray"></param>
        /// <param name="flags"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetMipmappedArray")]
        public static extern ECudaResult CudaTextRefSetMinmappedArray(
            SCudaTexRef htexRef,
            SCudaMipmappedArray handMipmappedArray,
            uint flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptexRef"></param>
        /// <returns>
        /// 
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuTexRefCreate")]
        public static extern ECudaResult CudaTexRefCreate(ref SCudaTexRef ptexRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <returns>
        /// 
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuTexRefDestroy")]
        public static extern ECudaResult CudaTexRefDestroy(SCudaTexRef htexRef);

        #endregion Getão de referências para texturas

        #region Gestão de referências para surfaces

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phArray"></param>
        /// <param name="hsurfRef"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuSurfRefGetArray")]
        public static extern ECudaResult CudaSurfRefGetArray(ref SCudaArray phArray, SCudaSurfRef hsurfRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hsurfRef"></param>
        /// <param name="harray"></param>
        /// <param name="flags"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuSurfRefSetArray")]
        public static extern ECudaResult CudaSurfRefSetArray(
            SCudaSurfRef hsurfRef,
            SCudaArray harray,
            uint flags);

        #endregion Gestão de referências para surfaces

        #region Getsão de objectos de textura

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptexObject"></param>
        /// <param name="presDesc"></param>
        /// <param name="ptexDesc"></param>
        /// <param name="presViewDesc"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexObjectCreate")]
        public static extern ECudaResult CudaTexObjectCreate(
            ref SCudaTexObj ptexObject,
            ref SCudaResourceDesc presDesc,
            ref SCudaTextureDesc ptexDesc,
            ref SCudaResourceViewDesc presViewDesc);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texObject"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexObjectDestroy")]
        public static extern ECudaResult CudaTexObjectDestroy(SCudaTexObj texObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="presDesc"></param>
        /// <param name="texObject"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexObjectGetResourceDesc")]
        public static extern ECudaResult CudaTexObjectGetResourceDesc(
            ref SCudaResourceDesc presDesc,
            SCudaTexObj texObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="presViewDesc"></param>
        /// <param name="texObject"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexObjectGetResourceViewDesc")]
        public static extern ECudaResult CudaTexObjectGerResourceViewMode(
            ref SCudaResourceViewDesc presViewDesc,
            SCudaTexObj texObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptexDesc"></param>
        /// <param name="texObject"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexObjectGetTextureDesc")]
        public static extern ECudaResult CudaTexObjectGetTextureDesc(
            ref SCudaTextureDesc ptexDesc,
            SCudaTexObj texObject);

        #endregion Gestão de objectos de textura

        #region Gestão de objectos de surfaces

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psrufObject"></param>
        /// <param name="presDesc"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuSurfObjectCreate")]
        public static extern ECudaResult CudaSurfObjectCreate(
            ref SCudaSurfObj psrufObject,
            ref SCudaResourceDesc presDesc);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surfObject"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuSurfObjectDestroy")]
        public static extern ECudaResult CudaSurfObjectDestroy(SCudaSurfObj surfObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="presDesc"></param>
        /// <param name="surfObject"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuSurfObjectGetResourceDesc")]
        public static extern ECudaResult CudaSurfObjectGetResourceDesc(
            ref SCudaResourceDesc presDesc,
            SCudaSurfObj surfObject);

        #endregion Gestão de objectos de surfaces

        #region Contexto de cais para acesso de memória

        /// <summary>
        /// 
        /// </summary>
        /// <param name="peerContext"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuCtxDisablePeerAccess")]
        public static extern ECudaResult CudaCtxDisablePeerAccess(SCudaContext peerContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="peerContext"></param>
        /// <param name="flags"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuCtxEnablePeerAccess")]
        public static extern ECudaResult CudaCtxEnablePeerAccess(SCudaContext peerContext, uint flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canAccessPeer"></param>
        /// <param name="dev"></param>
        /// <param name="peerDev"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuDeviceCanAccessPeer")]
        public static extern ECudaResult CudaDeviceCanAccessPeer(
            ref int canAccessPeer,
            SCudaDevice dev,
            SCudaDevice peerDev);

        #endregion Contexto de cais para acesso de memória

        #region Interoperabilidade gráfica

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="resources"></param>
        /// <param name="hstream"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuGraphicsMapResources")]
        public static extern ECudaResult CudaGraphicsResources(
            uint count,
            ref SCudaGraphicsResource resources,
            SCudaStream hstream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptrMipmappedArray"></param>
        /// <param name="resource"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuGraphicsResourceGetMappedMipmappedArray")]
        public static extern ECudaResult CudaGraphicsResourceGetMappedMipmappedArray(
            ref SCudaMipmappedArray ptrMipmappedArray,
            SCudaGraphicsResource resource);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdevPtr"></param>
        /// <param name="psize"></param>
        /// <param name="resource"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuGraphicsResourceGetMappedPointer")]
        public static extern ECudaResult CudaGraphicsResourceGetMappedPointer(
            ref SCudaDevicePtr pdevPtr,
            ref SizeT psize,
            SCudaGraphicsResource resource);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="flags"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuGraphicsResourceSetMapFlags")]
        public static extern ECudaResult CudaGraphicsResourceSetMapFlags(
            SCudaGraphicsResource resource,
            uint flags);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuGraphicsSubResourceGetMappedArray")]
        public static extern ECudaResult CudaGraphicsResourceGetMappedArray();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuGraphicsUnmapResources")]
        public static extern ECudaResult CudaGraphicsUnmapResource();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuGraphicsUnregisterResource")]
        public static extern ECudaResult CudaGraphicsUnregisterResource();

        #endregion Interoperabilidade gráfica

        #region Análise de desempenho

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="outputMode"></param>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuProfilerInitialize")]
        public static extern ECudaResult CudaProfilerInitialize(
            string configFile,
            string outputFile,
            ECudaOutputMode outputMode);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuProfilerStart")]
        public static extern ECudaResult CudaProfilerStart();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuProfilerStop")]
        public static extern ECudaResult CudaProfilerStop();

        #endregion Análise de desempenho
    }
}
