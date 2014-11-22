namespace Utilities.Cuda
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Tamanho do bloco relativo à memória dinâmica partilhada mapeada por bloco para um determinado kernel.
    /// </summary>
    /// <param name="blockSize">O tamanho do bloco.</param>
    /// <returns>O tamanho da memória dinâmica partilhada.</returns>
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    public delegate SizeT CudaOccupancyB2DSize(int blockSize);

    /// <summary>
    /// A função a ser chamada durante a ocorrência de determinados eventos no caudal CUDA.
    /// </summary>
    /// <param name="cudaStream">O caudal CUDA.</param>
    /// <param name="cudaResult">O resultado.</param>
    /// <param name="userData">Os dados de utilizador.</param>
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    public delegate void CudaStreamCallback(SCudaStream cudaStream, ECudaResult cudaResult, IntPtr userData);
}
