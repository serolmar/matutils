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

    }
}
