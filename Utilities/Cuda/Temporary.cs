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
        [DllImport(DLLName, EntryPoint = "cuMemcpy")]
        public static extern ECudaResult CudaMemcpy(SCudaDevicePtr dst, SCudaDevicePtr src, SizeT byteCount);

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
        [DllImport(DLLName, EntryPoint = "cuMemcpy2D")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpy2DAsync")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpy2DUnaligned")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpy3D")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpy3DAsync")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpy3DPeer")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpy3DPeerAsync")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyAsync")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyAtoA")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyAtoD")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyAtoH")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyAtoHAsync")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoA")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoD")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoDAsync")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoH")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoHAsync")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyHtoA")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyHtoAAsync")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyHtoD")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyHtoDAsync")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyPeer")]
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
        [DllImport(DLLName, EntryPoint = "cuMemcpyPeerAsync")]
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
        [DllImport(DLLName, EntryPoint = "cuMemsetD16")]
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
        [DllImport(DLLName, EntryPoint = "cuMemsetD16Async")]
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
        [DllImport(DLLName, EntryPoint = "cuMemsetD2D16")]
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
        [DllImport(DLLName, EntryPoint = "cuMemsetD2D16Async")]
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
        [DllImport(DLLName, EntryPoint = "cuMemsetD2D32")]
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
        [DllImport(DLLName, EntryPoint = "cuMemsetD2D32Async")]
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
		/// </remarls>
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
        [DllImport(DLLName, EntryPoint = "cuMemsetD2D8")]
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
		/// </remarls>
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
        [DllImport(DLLName, EntryPoint = "cuMemsetD2D8Async")]
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
        [DllImport(DLLName, EntryPoint = "cuMemsetD32")]
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
        [DllImport(DLLName, EntryPoint = "cuMemsetD32Async")]
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
        [DllImport(DLLName, EntryPoint = "cuMemsetD8")]
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
        [DllImport(DLLName, EntryPoint = "cuMemsetD8Async")]
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
        [DllImport(DLLName, EntryPoint = "cuMipmappedArrayCreate")]
        public static extern ECudaResult CudaMipmappedArrayCreate(
            ref SCudaMipmappedArray phandle,
            ref SCudaArray3DDescriptor pMipmappedArrayDesc,
            uint numMipmapLevels);

        /// <summary>
        /// Destrói um vector CUDA mipmapped.
        /// </summary>
        /// <param name="hndMipmappedArray">O vector mipmapped a ser destruído.</param>
        /// <param name="hndMipmappedArray">O vector mipmapped a ser destruído.</param>
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
        [DllImport(DLLName, EntryPoint = "cuMipmappedArrayGetLevel")]
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
        [DllImport(DLLName, EntryPoint = "cuPointerGetAttribute")]
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
        [DllImport(DLLName, EntryPoint = "cuPointerSetAttribute")]
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
        [DllImport(DLLName, EntryPoint = "cuStreamAddCallback")]
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
        [DllImport(DLLName, EntryPoint = "cuStreamAttachMemAsync")]
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
        /// <param name="flags">Parâmetros passados para a criação de caudal.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorOutOfMemory"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuStreamCreate")]
        public static extern ECudaResult CudaStreamCreate(
			ref SCudaStream phstream, 
			ECudaStreamFlags flags);

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
        /// <param name="flags">As marcas para a criação do caudal.</param>
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
        [DllImport(DLLName, EntryPoint = "cuStreamCreateWithPriority")]
        public static extern ECudaResult CudaStreamCreateWithPriority(
            ref SCudaStream phstream,
            ECudaStreamFlags flags,
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
        [DllImport(DLLName, EntryPoint = "cuStreamDestroy")]
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
        [DllImport(DLLName, EntryPoint = "cuStreamGetFlags")]
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
        [DllImport(DLLName, EntryPoint = "cuStreamGetPriority")]
        public static extern ECudaResult CudaGetPriority(SCudaStream hstream, ref int priority);

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
        [DllImport(DLLName, EntryPoint = "cuStreamQuery")]
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
        [DllImport(DLLName, EntryPoint = "cuStreamSynchronize")]
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
        [DllImport(DLLName, EntryPoint = "cuStreamWaitEvent")]
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
        [DllImport(DLLName, EntryPoint = "cuEventCreate")]
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
        [DllImport(DLLName, EntryPoint = "cuEventDestroy")]
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
        [DllImport(DLLName, EntryPoint = "cuEventElapsedTime")]
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
        [DllImport(DLLName, EntryPoint = "cuEventQuery")]
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
        [DllImport(DLLName, EntryPoint = "cuEventRecord")]
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
        [DllImport(DLLName, EntryPoint = "cuEventSynchronize")]
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
        [DllImport(DLLName, EntryPoint = "cuFuncGetAttribute")]
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
        [DllImport(DLLName, EntryPoint = "cuFuncSetCacheConfig")]
        public static extern ECudaResult CudaSetCacheConfig(SCudaFunction hfunc, ECudaFuncCache config);

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
        [DllImport(DLLName, EntryPoint = "cuFuncSetSharedMemConfig")]
        public static extern ECudaResult CudaSetSharedMemConfig(SCudaFunction hfunc, ECudaSharedConfig config);

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
        [DllImport(DLLName, EntryPoint = "cuLaunchKernel")]
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
        [DllImport(DLLName, EntryPoint = "cuFuncSetBlockShape")]
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
        [DllImport(DLLName, EntryPoint = "cuFuncSetSharedSize")]
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
        [DllImport(DLLName, EntryPoint = "cuLaunch")]
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
        [DllImport(DLLName, EntryPoint = "cuLaunchGrid")]
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
        [DllImport(DLLName, EntryPoint = "cuLaunchGridAsync")]
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
        [DllImport(DLLName, EntryPoint = "cuParamSetSize")]
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
        [DllImport(DLLName, EntryPoint = "cuParamSetTexRef")]
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
        [DllImport(DLLName, EntryPoint = "cuParamSetf")]
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
        [DllImport(DLLName, EntryPoint = "cuParamSeti")]
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
        [DllImport(DLLName, EntryPoint = "cuParamSetv")]
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
        [DllImport(DLLName, EntryPoint = "cuOccupancyMaxActiveBlocksPerMultiprocessor")]
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
        [DllImport(DLLName, EntryPoint = "cuTexRefGetAddress")]
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
        [DllImport(DLLName, EntryPoint = "cuTexRefGetAddressMode")]
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
        [DllImport(DLLName, EntryPoint = "cuTexRefGetArray")]
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
        [DllImport(DLLName, EntryPoint = "cuTexRefGetFilterMode")]
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
        [DllImport(DLLName, EntryPoint = "cuTexRefGetFlags")]
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
        [DllImport(DLLName, EntryPoint = "cuTexRefGetFormat")]
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
        [DllImport(DLLName, EntryPoint = "cuTexRefGetMaxAnisotropy")]
        public static extern ECudaResult CudaTextRefGetMaxAnisotropy(ref uint pmaxAniso, SCudaTexRef htextRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pfm"></param>
        /// <param name="htexRef"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetAddressMode")]
        public static extern ECudaResult CudaTextRefSetAddressNode(
            SCudaTexRef htexRef,
            int dim,
            ECudaAddressMode am);

        /// <summary>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetFilterMode")]
        public static extern ECudaResult CudaTextRefSetFilterMode(SCudaTexRef htexRef, ECudaFilterMode fm);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <param name="flags"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexRefSetMaxAnisotropy")]
        public static extern ECudaResult CudaTextRefSetMaxAnisotropy(SCudaTexRef htexRef, uint maxAniso);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <param name="fm"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuTexRefCreate")]
        public static extern ECudaResult CudaTexRefCreate(ref SCudaTexRef ptexRef);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htexRef"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuTexObjectDestroy")]
        public static extern ECudaResult CudaTexObjectDestroy(SCudaTexObj texObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="presDesc"></param>
        /// <param name="texObject"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuSurfObjectDestroy")]
        public static extern ECudaResult CudaSurfObjectDestroy(SCudaSurfObj surfObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="presDesc"></param>
        /// <param name="surfObject"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorPeerAccessNotEnabled"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuCtxDisablePeerAccess")]
        public static extern ECudaResult CudaCtxDisablePeerAccess(SCudaContext peerContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="peerContext"></param>
        /// <param name="flags"></param>
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
        [DllImport(DLLName, EntryPoint = "cuCtxEnablePeerAccess")]
        public static extern ECudaResult CudaCtxEnablePeerAccess(SCudaContext peerContext, uint flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canAccessPeer"></param>
        /// <param name="dev"></param>
        /// <param name="peerDev"></param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorAlreadyMapped"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorNotMapped"/>,
        /// <see cref="ECudaResult.CudaErrorNotMappedAsArray"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorNotMapped"/>,
        /// <see cref="ECudaResult.CudaErrorNotMappedAsPointer"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorAlreadyMapped"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuGraphicsResourceSetMapFlags")]
        public static extern ECudaResult CudaGraphicsResourceSetMapFlags(
            SCudaGraphicsResource resource,
            uint flags);

        /// <summary>
        /// 
        /// </summary>
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
        [DllImport(DLLName, EntryPoint = "cuGraphicsSubResourceGetMappedArray")]
        public static extern ECudaResult CudaGraphicsResourceGetMappedArray();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorNotMapped"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuGraphicsUnmapResources")]
        public static extern ECudaResult CudaGraphicsUnmapResource();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorDeinitialized"/>,
        /// <see cref="ECudaResult.CudaErrorNotInitialized"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidHandle"/>,
        /// <see cref="ECudaResult.CudaErrorUnknown"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/>,
        /// <see cref="ECudaResult.CudaErrorProfileDisabled"/>.
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
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuProfilerStart")]
        public static extern ECudaResult CudaProfilerStart();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidContext"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuProfilerStop")]
        public static extern ECudaResult CudaProfilerStop();

        #endregion Análise de desempenho
    }
}
