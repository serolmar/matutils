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

        [DllImport(DLLName, EntryPoint = "cuArray3DCreate")]
        public static extern ECudaResult CudaArray3DCreate(
            ref  SCudaArray pHandle,
            ref SCudaArray3DDescriptor pAllocateArray);

        [DllImport(DLLName, EntryPoint = "cuArray3DGetDescriptor")]
        public static extern ECudaResult CudaArray3DGetDescriptor(
            ref SCudaArray3DDescriptor pArrayDescriptor,
            SCudaArray hArray);

        [DllImport(DLLName, EntryPoint = "cuArrayCreate")]
        public static extern ECudaResult CudaArrayCreate(
            ref  SCudaArray pHandle,
            ref SCudaArrayDescriptor pAllocateArray);

        [DllImport(DLLName, EntryPoint = "cuArrayDestroy")]
        public static extern ECudaResult CudaArrayDestroy(SCudaArray hArray);

        [DllImport(DLLName, EntryPoint = "cuArrayGetDescriptor")]
        public static extern ECudaResult CudaArrayGetDescriptor(
            ref SCudaArrayDescriptor pArrayDescriptor,
            SCudaArray hArray);

        [DllImport(DLLName, EntryPoint = "cuDeviceGetByPCIBusId")]
        public static extern ECudaResult CudaDeviceGetByPCIBusId(
            ref SCudaDevice dev,
            string pciBusId);

        [DllImport(DLLName, EntryPoint = "cuDeviceGetPCIBusId")]
        public static extern ECudaResult CudaDeviceGetPCIBusId(
            string pciBusId,
            int len,
            SCudaDevice dev);

        [DllImport(DLLName, EntryPoint = "cuIpcCloseMemHandle")]
        public static extern ECudaResult CudaIpcCloseMemHandle(SCudaDevicePtr dptr);

        [DllImport(DLLName, EntryPoint = "cuIpcGetEventHandle")]
        public static extern ECudaResult CudaIpcGetEventHandle(
            ref SCudaIpcEventHandle pHandle,
            SCudaEvent cudaEvent);

        [DllImport(DLLName, EntryPoint = "cuIpcGetMemHandle")]
        public static extern ECudaResult CudaIpcGetMemHandle(
            ref SCudaIpcMemHandle pHandle,
            SCudaDevicePtr dptr);

        [DllImport(DLLName, EntryPoint = "cuIpcOpenEventHandle")]
        public static extern ECudaResult CudaIpcOpenEventHandle(
            ref SCudaEvent phEvent,
            SCudaIpcEventHandle handle);

        [DllImport(DLLName, EntryPoint = "cuIpcOpenMemHandle")]
        public static extern ECudaResult CudaIpcOpenMemHandle(
            ref SCudaDevicePtr pdptr,
            SCudaIpcMemHandle handle,
            uint Flags);

        [DllImport(DLLName, EntryPoint = "cuMemAlloc")]
        public static extern ECudaResult CudaMemAlloc(ref SCudaDevicePtr dptr, SizeT bytesize);

        [DllImport(DLLName, EntryPoint = "cuMemAllocHost")]
        public static extern ECudaResult CudaMemAllocHost(IntPtr pp, SizeT bytesize);

        [DllImport(DLLName, EntryPoint = "cuMemAllocManaged")]
        public static extern ECudaResult CudaMemAllocManaged(
            ref SCudaDevicePtr dptr,
            SizeT bytesize,
            uint flags);

        [DllImport(DLLName, EntryPoint = "cuMemAllocPitch")]
        public static extern ECudaResult CudaMemAllocPitch(
            ref SCudaDevicePtr dptr,
            ref SizeT pPitch,
            SizeT WidthInBytes,
            SizeT Height,
            uint ElementSizeBytes);

        [DllImport(DLLName, EntryPoint = "cuMemFree")]
        public static extern ECudaResult CudaMemFree(SCudaDevicePtr dptr);

        [DllImport(DLLName, EntryPoint = "cuMemFreeHost")]
        public static extern ECudaResult CudaMemFreeHost(IntPtr p);

        [DllImport(DLLName, EntryPoint = "cuMemGetAddressRange")]
        public static extern ECudaResult CudaMemGetAddressRange(
            ref SCudaDevicePtr pbase,
            ref SizeT psize,
            SCudaDevicePtr dptr);

        [DllImport(DLLName, EntryPoint = "cuMemGetInfo")]
        public static extern ECudaResult CudaMemGetInfo(ref SizeT free, ref SizeT total);

        [DllImport(DLLName, EntryPoint = "cuMemHostAlloc")]
        public static extern ECudaResult CudaMemHostAlloc(IntPtr pp, SizeT bytesize, uint Flags);

        [DllImport(DLLName, EntryPoint = "cuMemHostGetDevicePointer")]
        public static extern ECudaResult CudaMemHostGetDevicePointer(
            ref SCudaDevicePtr pdptr,
            IntPtr p,
            uint Flags);

        [DllImport(DLLName, EntryPoint = "cuMemHostGetFlags")]
        public static extern ECudaResult CudaMemHostGetFlags(ref uint pFlags, IntPtr p);

        [DllImport(DLLName, EntryPoint = "cuMemHostRegister")]
        public static extern ECudaResult CudaMemHostRegister(IntPtr p, SizeT bytesize, uint Flags);

        [DllImport(DLLName, EntryPoint = "cuMemHostUnregister")]
        public static extern ECudaResult CudaMemHostUnregister(IntPtr p);

        [DllImport(DLLName, EntryPoint = "cuMemcpy")]
        public static extern ECudaResult CudaMemcpy(SCudaDevicePtr dst, SCudaDevicePtr src, SizeT ByteCount);

        [DllImport(DLLName, EntryPoint = "cuMemcpy2D")]
        public static extern ECudaResult CudaMemcpy2D(ref SCudaMemCpy2D pCopy);

        [DllImport(DLLName, EntryPoint = "cuMemcpy2DAsync")]
        public static extern ECudaResult CudaMemcpy2DAsync(ref SCudaMemCpy2D pCopy, SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemcpy2DUnaligned")]
        public static extern ECudaResult CudaMemcpy2DUnaligned(ref SCudaMemCpy2D pCopy);

        [DllImport(DLLName, EntryPoint = "cuMemcpy3D")]
        public static extern ECudaResult CudaMemcpy3D(ref SCudaMemCpy3D pCopy);

        [DllImport(DLLName, EntryPoint = "cuMemcpy3DAsync")]
        public static extern ECudaResult CudaMemcpy3DAsync(ref SCudaMemCpy3D pCopy, SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemcpy3DPeer")]
        public static extern ECudaResult CudaMemcpy3DPeer(ref SCudaMemCpy3DPeer pCopy);

        [DllImport(DLLName, EntryPoint = "cuMemcpy3DPeerAsync")]
        public static extern ECudaResult CudaMemcpy3DPeerAsync(ref SCudaMemCpy3DPeer pCopy, SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemcpyAsync")]
        public static extern ECudaResult CudaMemcpyAsync(
            SCudaDevicePtr dst,
            SCudaDevicePtr src,
            SizeT ByteCount,
            SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemcpyAtoA")]
        public static extern ECudaResult CudaMemcpyAtoA(
            SCudaArray dstArray,
            SizeT dstOffset,
            SCudaArray srcArray,
            SizeT srcOffset,
            SizeT ByteCount);

        [DllImport(DLLName, EntryPoint = "cuMemcpyAtoD")]
        public static extern ECudaResult CudaMemcpyAtoD(
            SCudaDevicePtr dstDevice,
            SCudaArray srcArray,
            SizeT srcOffset,
            SizeT ByteCount);

        [DllImport(DLLName, EntryPoint = "cuMemcpyAtoH")]
        public static extern ECudaResult CudaMemcpyAtoH(
            IntPtr dstHost,
            SCudaArray srcArray,
            SizeT srcOffset,
            SizeT ByteCount);

        [DllImport(DLLName, EntryPoint = "cuMemcpyAtoHAsync")]
        public static extern ECudaResult CudaMemcpyAtoHAsync(
            IntPtr dstHost,
            SCudaArray srcArray,
            SizeT srcOffset,
            SizeT ByteCount,
            SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoA")]
        public static extern ECudaResult CudaMemcpyDtoA(
            SCudaArray dstArray,
            SizeT dstOffset,
            SCudaDevicePtr srcDevice,
            SizeT ByteCount);

        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoD")]
        public static extern ECudaResult CudaMemcpyDtoD(
            SCudaDevicePtr dstDevice,
            SCudaDevicePtr srcDevice,
            SizeT ByteCount);

        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoDAsync")]
        public static extern ECudaResult CudaMemcpyDtoDAsync(
            SCudaDevicePtr dstDevice,
            SCudaDevicePtr srcDevice,
            SizeT ByteCount,
            SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoH")]
        public static extern ECudaResult CudaMemcpyDtoH(
            IntPtr dstHost,
            SCudaDevicePtr srcDevice,
            SizeT ByteCount);

        [DllImport(DLLName, EntryPoint = "cuMemcpyDtoHAsync")]
        public static extern ECudaResult CudaMemcpyDtoHAsync(
            IntPtr dstHost,
            SCudaDevicePtr srcDevice,
            SizeT ByteCount,
            SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemcpyHtoA")]
        public static extern ECudaResult CudaMemcpyHtoA(
            SCudaArray dstArray,
            SizeT dstOffset,
            IntPtr srcHost,
            SizeT ByteCount);

        [DllImport(DLLName, EntryPoint = "cuMemcpyHtoAAsync")]
        public static extern ECudaResult CudaMemcpyHtoAAsync(
            SCudaArray dstArray,
            SizeT dstOffset,
            IntPtr srcHost,
            SizeT ByteCount,
            SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemcpyHtoD")]
        public static extern ECudaResult CudaMemcpyHtoD(
            SCudaDevicePtr dstDevice,
            IntPtr srcHost,
            SizeT ByteCount);

        [DllImport(DLLName, EntryPoint = "cuMemcpyHtoDAsync")]
        public static extern ECudaResult CudaMemcpyHtoDAsync(
            SCudaDevicePtr dstDevice,
            IntPtr srcHost,
            SizeT ByteCount,
            SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemcpyPeer")]
        public static extern ECudaResult CudaMemcpyPeer(
            SCudaDevicePtr dstDevice,
            SCudaContext dstContext,
            SCudaDevicePtr srcDevice,
            SCudaContext srcContext,
            SizeT ByteCount);

        [DllImport(DLLName, EntryPoint = "cuMemcpyPeerAsync")]
        public static extern ECudaResult CudaMemcpyPeerAsync(
            SCudaDevicePtr dstDevice,
            SCudaContext dstContext,
            SCudaDevicePtr srcDevice,
            SCudaContext srcContext,
            SizeT ByteCount,
            SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemsetD16")]
        public static extern ECudaResult CudaMemsetD16(
            SCudaDevicePtr dstDevice,
            ushort us,
            SizeT N);

        [DllImport(DLLName, EntryPoint = "cuMemsetD16Async")]
        public static extern ECudaResult CudaMemsetD16Async(
            SCudaDevicePtr dstDevice,
            ushort us,
            SizeT N,
            SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemsetD2D16")]
        public static extern ECudaResult CudaMemsetD2D16(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch, ushort us,
            SizeT Width,
            SizeT Height);

        [DllImport(DLLName, EntryPoint = "cuMemsetD2D16Async")]
        public static extern ECudaResult CudaMemsetD2D16Async(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            ushort us,
            SizeT Width,
            SizeT Height,
            SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemsetD2D32")]
        public static extern ECudaResult CudaMemsetD2D32(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            uint ui,
            SizeT Width,
            SizeT Height);

        [DllImport(DLLName, EntryPoint = "cuMemsetD2D32Async")]
        public static extern ECudaResult CudaMemsetD2D32Async(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            uint ui,
            SizeT Width,
            SizeT Height,
            SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemsetD2D8")]
        public static extern ECudaResult CudaMemsetD2D8(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            byte uc,
            SizeT Width,
            SizeT Height);

        [DllImport(DLLName, EntryPoint = "cuMemsetD2D8Async")]
        public static extern ECudaResult CudaMemsetD2D8Async(
            SCudaDevicePtr dstDevice,
            SizeT dstPitch,
            byte uc,
            SizeT Width,
            SizeT Height,
            SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemsetD32")]
        public static extern ECudaResult CudaMemsetD32(SCudaDevicePtr dstDevice, uint ui, SizeT N);

        [DllImport(DLLName, EntryPoint = "cuMemsetD32Async")]
        public static extern ECudaResult CudaMemsetD32Async(
            SCudaDevicePtr dstDevice,
            uint ui,
            SizeT N,
            SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMemsetD8")]
        public static extern ECudaResult CudaMemsetD8(SCudaDevicePtr dstDevice, byte uc, SizeT N);

        [DllImport(DLLName, EntryPoint = "cuMemsetD8Async")]
        public static extern ECudaResult CudaMemsetD8Async(
            SCudaDevicePtr dstDevice,
            byte uc,
            SizeT N,
            SCudaStream hStream);

        [DllImport(DLLName, EntryPoint = "cuMipmappedArrayCreate")]
        public static extern ECudaResult CudaMipmappedArrayCreate(
            ref SCudaMipmappedArray pHandle,
            ref SCudaArray3DDescriptor pMipmappedArrayDesc,
            uint numMipmapLevels);

        [DllImport(DLLName, EntryPoint = "cuMipmappedArrayDestroy")]
        public static extern ECudaResult CudaMipmappedArrayDestroy(SCudaMipmappedArray hMipmappedArray);

        [DllImport(DLLName, EntryPoint = "cuMipmappedArrayGetLevel")]
        public static extern ECudaResult CudaMipmappedArrayGetLevel(
            ref  SCudaArray pLevelArray,
            SCudaMipmappedArray hMipmappedArray, uint level);

        #endregion Gestão de memória

        #region Endereçamento unificado

        [DllImport(DLLName, EntryPoint = "cuPointerGetAttribute")]
        public static extern ECudaResult CudaPointerGetAttribute(
            IntPtr data,
            ECudaPointerAttribute attribute,
            SCudaDevicePtr ptr);

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
        /// <see cref="ECudaResult.CudaErrorNorSupported"/>.
        /// </returns>
        [DllImport(DLLName, EntryPoint = "cuStreamAddCallback")]
        public static extern ECudaResult CudaStreamAddCallback(
            SCudaStream hstream,
            CudaStreamCallback callback,
            IntPtr userData,
            uint flags);

        [DllImport(DLLName, EntryPoint = "cuStreamAttachMemAsync")]
        public static extern ECudaResult CudaStreamAttachMemAsync(
            SCudaStream hstream,
            SCudaDevicePtr dptr,
            SizeT length);

        [DllImport(DLLName, EntryPoint = "cuStreamCreate")]
        public static extern ECudaResult CudaStreamCreate(ref SCudaStream phstream, uint flags);

        [DllImport(DLLName, EntryPoint = "cuStreamCreateWithPriority")]
        public static extern ECudaResult CudaStreamCreateWithPriority(
            ref SCudaStream phstream,
            uint flags,
            int priority);

        [DllImport(DLLName, EntryPoint = "cuStreamDestroy")]
        public static extern ECudaResult CudaStreamDestroy(SCudaStream hstream);

        [DllImport(DLLName, EntryPoint = "cuStreamGetFlags")]
        public static extern ECudaResult CudaStreamGetFlags(SCudaStream hstream, ref uint flags);

        [DllImport(DLLName, EntryPoint = "cuStreamGetPriority")]
        public static extern ECudaResult CudaGetPriority(SCudaStream hstream, ref int priority);

        [DllImport(DLLName, EntryPoint = "cuStreamQuery")]
        public static extern ECudaResult CudaStreamQuery(SCudaStream hstream);

        [DllImport(DLLName, EntryPoint = "cuStreamSynchronize")]
        public static extern ECudaResult CudaStreamSynchronize(SCudaStream hstream);

        [DllImport(DLLName, EntryPoint = "cuStreamWaitEvent")]
        public static extern ECudaResult CudaStreamWaitEvent(
            SCudaStream hstream, 
            SCudaEvent hevent, 
            uint flags);

        #endregion Gestão de caudal

        #region Gestão de eventos

        [DllImport(DLLName, EntryPoint = "cuEventCreate")]
        public static extern ECudaResult CudaEventCreate(ref SCudaEvent phevent, uint flags);

        [DllImport(DLLName, EntryPoint = "cuEventDestroy")]
        public static extern ECudaResult CudaEventDestroy(SCudaEvent hevent);

        [DllImport(DLLName, EntryPoint = "cuEventElapsedTime")]
        public static extern ECudaResult CudaEventEllapsedTime(
            ref float milliseconds, 
            SCudaEvent hstart, 
            SCudaEvent hend);

        [DllImport(DLLName, EntryPoint = "cuEventQuery")]
        public static extern ECudaResult CudaEventQuery(SCudaEvent hevent);

        [DllImport(DLLName, EntryPoint = "cuEventRecord")]
        public static extern ECudaResult CudaEventRecord(SCudaEvent hevent, SCudaStream hstream);

        [DllImport(DLLName, EntryPoint = "cuEventSynchronize")]
        public static extern ECudaResult CudaEventSynchronize(SCudaEvent hevent);

        #endregion Gestão de eventos

        #region Controlo de execução

        [DllImport(DLLName, EntryPoint = "cuFuncGetAttribute")]
        public static extern ECudaResult CudaFuncGetAttribute(
            ref int pi, 
            ECudaFuncAttribute attrib,
            SCudaFunction hfunc);

        [DllImport(DLLName, EntryPoint = "cuFuncSetCacheConfig")]
        public static extern ECudaResult CudaSetCacheConfig(SCudaFunction hfunc, ECudaFuncCache config);

        [DllImport(DLLName, EntryPoint = "cuFuncSetSharedMemConfig")]
        public static extern ECudaResult CudaSetSharedMemConfig(SCudaFunction hfunc, ECudaSharedConfig config);

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

        [Obsolete("Dreprecated")]
        [DllImport(DLLName, EntryPoint = "cuFuncSetBlockShape")]
        public static extern ECudaResult CudaFuncSetBlockShape(
            SCudaFunction hfunc,
            int x,
            int y,
            int z);

        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuFuncSetSharedSize")]
        public static extern ECudaResult CudaFuncSetSharedSize(
            SCudaFunction hfunc,
            uint bytes);

        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuLaunch")]
        public static extern ECudaResult CudaLaunch(SCudaFunction hfunc);

        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuLaunchGrid")]
        public static extern ECudaResult CudaLaunchGrid(SCudaFunction hfunc, int gridWidth, int gridHeight);

        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuLaunchGridAsync")]
        public static extern ECudaResult CudaLaunchGridAsync(
            SCudaFunction hfunc, 
            int gridWidth, 
            int gridHeight,
            SCudaStream hstream);

        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuParamSetSize")]
        public static extern ECudaResult CudaParamSetSize(SCudaFunction hfunc, uint numbBytes);

        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuParamSetTexRef")]
        public static extern ECudaResult CudaSetTexRef(SCudaFunction hfunc, int textUnit, SCudaTexRef htextRef);

        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuParamSetf")]
        public static extern ECudaResult CudaParamSetf(SCudaFunction hfunc, int offset, float value);

        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuParamSeti")]
        public static extern ECudaResult CudaParamSeti(SCudaFunction hfunc, int offset, uint value);

        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuParamSetv")]
        public static extern ECudaResult CudaParamSetv(SCudaFunction hfunc, int offset, int value);

        #endregion Controlo de execução

        #region Ocupação

        [DllImport(DLLName, EntryPoint = "cuOccupancyMaxActiveBlocksPerMultiprocessor")]
        public static extern ECudaResult CudaOccupancyMaxActiveBlockPerMultiprocessor(
            ref int numBlocks,
            SCudaFunction hfunc,
            int blockSize,
            SizeT dynamicMemSize);

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

        [DllImport(DLLName, EntryPoint = "cuTexRefGetAddress")]
        public static extern ECudaResult CudaTextRefGetAddress(ref SCudaDevicePtr pdptr, SCudaTexRef htexRef);

        [DllImport(DLLName, EntryPoint = "cuTexRefGetAddressMode")]
        public static extern ECudaResult CudaGetAddressMode(
            ref ECudaAddressMode pam, 
            SCudaTexRef htexRef, 
            int dim);

        [DllImport(DLLName, EntryPoint = "cuTexRefGetArray")]
        public static extern ECudaResult CudaTexRefGetArray(ref SCudaArray phandArray, SCudaTexRef htexRef);

        [DllImport(DLLName, EntryPoint = "cuTexRefGetFilterMode")]
        public static extern ECudaResult CudaTexRefGetFilterMode(ref ECudaFilterMode pfm, SCudaTexRef htextRef);

        [DllImport(DLLName, EntryPoint = "cuTexRefGetFlags")]
        public static extern ECudaResult CudaTexRefGetFlags(ref uint flags, SCudaTexRef htextRef);

        [DllImport(DLLName, EntryPoint = "cuTexRefGetFormat")]
        public static extern ECudaResult CudaTexRefGetFormat(
            ref ECudaArrayFormat pformat,
            ref int pnumChannels,
            SCudaTexRef htexRef);

        [DllImport(DLLName, EntryPoint = "cuTexRefGetMaxAnisotropy")]
        public static extern ECudaResult CudaTextRefGetMaxAnisotropy(ref uint pmaxAniso, SCudaTexRef htextRef);

        [DllImport(DLLName, EntryPoint = "cuTexRefGetMipmapFilterMode")]
        public static extern ECudaResult CudaTextRefGetMinmpaFilterMode(
            ref ECudaFilterMode pfm, 
            SCudaTexRef htexRef);

        [DllImport(DLLName, EntryPoint = "cuTexRefGetMipmapLevelBias")]
        public static extern ECudaResult CudaTextRefGetMinmapLevelBias(ref float pbias, SCudaTexRef htexRef);

        [DllImport(DLLName, EntryPoint = "cuTexRefGetMipmapLevelClamp")]
        public static extern ECudaResult CudaTextRefGetMinmapLevelClamp(
            ref float pminMipmapLevelClamp,
            ref float pmaxMipmapLevelClamp,
            SCudaTexRef htexRef);

        [DllImport(DLLName, EntryPoint = "cuTexRefGetMipmappedArray")]
        public static extern ECudaResult CudaTextRefGetMinmappedArray(
            ref SCudaMipmappedArray phMipmappedArray,
            SCudaTexRef htexRef);

        [DllImport(DLLName, EntryPoint = "cuTexRefSetAddress")]
        public static extern ECudaResult CudaTextRefSetAddress(
            ref SizeT byteOffset,
            SCudaTexRef htexRef,
            SCudaDevicePtr dptr,
            SizeT bytes);

        [DllImport(DLLName, EntryPoint = "cuTexRefSetAddress2D")]
        public static extern ECudaResult CudaTextRefSetAddress2D(
            SCudaTexRef htexRef,
            ref SCudaArrayDescriptor desc,
            SCudaDevicePtr dptr,
            SizeT pitch);

        [DllImport(DLLName, EntryPoint = "cuTexRefSetAddressMode")]
        public static extern ECudaResult CudaTextRefSetAddressNode(
            SCudaTexRef htexRef, 
            int dim, 
            ECudaAddressMode am);

        [DllImport(DLLName, EntryPoint = "cuTexRefSetArray")]
        public static extern ECudaResult CudaTextRefSetArray(
            SCudaTexRef htexRef,
            SCudaArray handArray,
            uint flags);

        [DllImport(DLLName, EntryPoint = "cuTexRefSetFilterMode")]
        public static extern ECudaResult CudaTextRefSetFilterMode(SCudaTexRef htexRef, ECudaFilterMode fm);

        [DllImport(DLLName, EntryPoint = "cuTexRefSetFlags")]
        public static extern ECudaResult CudaTextRefSetFlags(SCudaTexRef htexRef, uint flags);

        [DllImport(DLLName, EntryPoint = "cuTexRefSetFormat")]
        public static extern ECudaResult CudaTextRefSetFormat(
            SCudaTexRef htexRef,
            ECudaArrayFormat fmt,
            int numPackedComponents);

        [DllImport(DLLName, EntryPoint = "cuTexRefSetMaxAnisotropy")]
        public static extern ECudaResult CudaTextRefSetMaxAnisotropy(SCudaTexRef htexRef, uint maxAniso);

        [DllImport(DLLName, EntryPoint = "cuTexRefSetMipmapFilterMode")]
        public static extern ECudaResult CudaTextRefSetMinmapFilterMode(
            SCudaTexRef htexRef, 
            ECudaFilterMode fm);

        [DllImport(DLLName, EntryPoint = "cuTexRefSetMipmapLevelBias")]
        public static extern ECudaResult CudaTextRefSetMinmapLevelBias(SCudaTexRef htexRef, float bias);

        [DllImport(DLLName, EntryPoint = "cuTexRefSetMipmapLevelClamp")]
        public static extern ECudaResult CudaTextRefSetMipmapLevelClamp(
            SCudaTexRef htexRef,
            float minMipmapLevelClamp,
            float maxMipmapLevelClamp);

        [DllImport(DLLName, EntryPoint = "cuTexRefSetMipmappedArray")]
        public static extern ECudaResult CudaTextRefSetMinmappedArray(
            SCudaTexRef htexRef,
            SCudaMipmappedArray handMipmappedArray,
            uint flags);

        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuTexRefCreate")]
        public static extern ECudaResult CudaTexRefCreate(ref SCudaTexRef ptexRef);

        [Obsolete("Deprecated")]
        [DllImport(DLLName, EntryPoint = "cuTexRefDestroy")]
        public static extern ECudaResult CudaTexRefDestroy(SCudaTexRef htexRef);

        #endregion Getão de referências para texturas

        #region Gestão de referências para surfaces

        [DllImport(DLLName, EntryPoint = "cuSurfRefGetArray")]
        public static extern ECudaResult CudaSurfRefGetArray(ref SCudaArray phArray, SCudaSurfRef hsurfRef);

        [DllImport(DLLName, EntryPoint = "cuSurfRefSetArray")]
        public static extern ECudaResult CudaSurfRefSetArray(
            SCudaSurfRef hsurfRef,
            SCudaArray harray, 
            uint flags);

        #endregion Gestão de referências para surfaces

        #region Getsão de objectos de textura

        [DllImport(DLLName, EntryPoint = "cuTexObjectCreate")]
        public static extern ECudaResult CudaTexObjectCreate(
            ref SCudaTexObj ptexObject,
            ref SCudaResourceDesc presDesc,
            ref SCudaTextureDesc ptexDesc,
            ref SCudaResourceViewDesc presViewDesc);

        [DllImport(DLLName, EntryPoint = "cuTexObjectDestroy")]
        public static extern ECudaResult CudaTexObjectDestroy(SCudaTexObj texObject);

        [DllImport(DLLName, EntryPoint = "cuTexObjectGetResourceDesc")]
        public static extern ECudaResult CudaTexObjectGetResourceDesc(
            ref SCudaResourceDesc presDesc,
            SCudaTexObj texObject);

        [DllImport(DLLName, EntryPoint = "cuTexObjectGetResourceViewDesc")]
        public static extern ECudaResult CudaTexObjectGerResourceViewMode(
            ref SCudaResourceViewDesc presViewDesc,
            SCudaTexObj texObject);

        [DllImport(DLLName, EntryPoint = "cuTexObjectGetTextureDesc")]
        public static extern ECudaResult CudaTexObjectGetTextureDesc(
            ref SCudaTextureDesc ptexDesc,
            SCudaTexObj texObject);
        
        #endregion Gestão de objectos de textura

        #region Gestão de objectos de surfaces

        [DllImport(DLLName, EntryPoint = "cuSurfObjectCreate")]
        public static extern ECudaResult CudaSurfObjectCreate(
            ref SCudaSurfObj psrufObject,
            ref SCudaResourceDesc presDesc);

        [DllImport(DLLName, EntryPoint = "cuSurfObjectDestroy")]
        public static extern ECudaResult CudaSurfObjectDestroy(SCudaSurfObj surfObject);

        [DllImport(DLLName, EntryPoint = "cuSurfObjectGetResourceDesc")]
        public static extern ECudaResult CudaSurfObjectGetResourceDesc(
            ref SCudaResourceDesc presDesc,
            SCudaSurfObj surfObject);

        #endregion Gestão de objectos de surfaces

        #region Contexto de cais para acesso de memória

        [DllImport(DLLName, EntryPoint = "cuCtxDisablePeerAccess")]
        public static extern ECudaResult CudaCtxDisablePeerAccess(SCudaContext peerContext);

        [DllImport(DLLName, EntryPoint = "cuCtxEnablePeerAccess")]
        public static extern ECudaResult CudaCtxEnablePeerAccess(SCudaContext peerContext, uint flags);

        [DllImport(DLLName, EntryPoint = "cuDeviceCanAccessPeer")]
        public static extern ECudaResult CudaDeviceCanAccessPeer(
            ref int canAccessPeer,
            SCudaDevice dev,
            SCudaDevice peerDev);

        #endregion Contexto de cais para acesso de memória

        #region Interoperabilidade gráfica

        [DllImport(DLLName, EntryPoint = "cuGraphicsMapResources")]
        public static extern ECudaResult CudaGraphicsResources(
            uint count,
            ref SCudaGraphicsResource resources,
            SCudaStream hstream);

        [DllImport(DLLName, EntryPoint = "cuGraphicsResourceGetMappedMipmappedArray")]
        public static extern ECudaResult CudaGraphicsResourceGetMappedMipmappedArray(
            ref SCudaMipmappedArray ptrMipmappedArray,
            SCudaGraphicsResource resource);

        [DllImport(DLLName, EntryPoint = "cuGraphicsResourceGetMappedPointer")]
        public static extern ECudaResult CudaGraphicsResourceGetMappedPointer(
            ref SCudaDevicePtr pdevPtr,
            ref SizeT psize,
            SCudaGraphicsResource resource);

        [DllImport(DLLName, EntryPoint = "cuGraphicsResourceSetMapFlags")]
        public static extern ECudaResult CudaGraphicsResourceSetMapFlags(
            SCudaGraphicsResource resource,
            uint flags);

        [DllImport(DLLName, EntryPoint = "cuGraphicsSubResourceGetMappedArray")]
        public static extern ECudaResult CudaGraphicsResourceGetMappedArray();

        [DllImport(DLLName, EntryPoint = "cuGraphicsUnmapResources")]
        public static extern ECudaResult CudaGraphicsUnmapResource();

        [DllImport(DLLName, EntryPoint = "cuGraphicsUnregisterResource")]
        public static extern ECudaResult CudaGraphicsUnregisterResource();

        #endregion Interoperabilidade gráfica
    }
}
