// -----------------------------------------------------------------------
// <copyright file="ReaderWriter.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um leitor de bits.
    /// </summary>
    public class BitReader
    {
        /// <summary>
        /// Máscara para filtro de bits.
        /// </summary>
        private static byte[] mask = new byte[]
        {
            0xFF,
            0x7F,
            0x3F,
            0x1F,
            0x0F,
            0x07,
            0x03,
            0x01,
        };
        /// <summary>
        /// Mantém o fluxo de onde serão lidos os bits.
        /// </summary>
        private Stream stream;

        /// <summary>
        /// O amortecedor interno.
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// O número de bytes lidos para o amortecedor.
        /// </summary>
        private int readedBytes;

        /// <summary>
        /// O índice do vector onde se encontra o cursor.
        /// </summary>
        private int currentVarIndex;

        /// <summary>
        /// O índice do bit na entrada do vector onde se encontra o cursor.
        /// </summary>
        private int bitPos;

        /// <summary>
        /// Indica se o leitor se encontra no final do ficheiro.
        /// </summary>
        private bool endOfStream;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="BitReader"/>.
        /// </summary>
        /// <param name="stream">O fluxo de onde são lidos os bits.</param>
        public BitReader(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            else
            {
                if (stream.CanRead)
                {
                    this.stream = stream;
                    this.buffer = new byte[8];

                    this.bitPos = 8;
                    this.currentVarIndex = -1;
                }
                else
                {
                    throw new NotSupportedException(
                        "The underlying stream doesn't support reads.");
                }
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="BitReader"/>.
        /// </summary>
        /// <param name="stream">O fluxo de onde são lidos os bits.</param>
        /// <param name="capacity">A capacidade em bytes do amortecedor interno.</param>
        public BitReader(Stream stream, int capacity)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            else if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    "capacity",
                    "Capacity must be a positive value.");
            }
            else
            {
                if (stream.CanRead)
                {
                    this.stream = stream;
                    this.buffer = new byte[capacity];

                    this.bitPos = 8;
                    this.currentVarIndex = -1;
                }
                else
                {
                    throw new NotSupportedException(
                        "The underlying stream doesn't support reads.");
                }
            }
        }

        /// <summary>
        /// Obtém o comprimento do fluxo em bits.
        /// </summary>
        public long Length
        {
            get
            {
                return this.stream.Length * 8;
            }
        }

        /// <summary>
        /// Obtém ou atribui a posição do fluxo em bits.
        /// </summary>
        public long Position
        {
            get
            {
                if (this.stream.CanSeek)
                {
                    if (this.currentVarIndex == -1)
                    {
                        return this.stream.Position * 8;
                    }
                    else
                    {
                        var streamPos = this.stream.Position;
                        streamPos -= (this.readedBytes - this.currentVarIndex);
                        return streamPos * 8 + this.bitPos;
                    }
                }
                else
                {
                    throw new NotSupportedException(
                        "Seek operations aren't supported by the underlying stream.");
                }
            }
            set
            {
                if (this.stream.CanSeek)
                {
                    if (value < 0)
                    {
                        throw new UtilitiesException("Position must be non negative.");
                    }
                    else
                    {
                        this.endOfStream = false;
                        var mainVal = value >> 3;
                        var rem = (int)(value & 7);
                        this.stream.Position = mainVal;
                        this.readedBytes = 0;
                        if (rem == 0)
                        {
                            this.currentVarIndex = -1;
                            this.bitPos = 8;
                        }
                        else
                        {
                            this.readedBytes = this.stream.Read(
                                this.buffer,
                                0,
                                this.buffer.Length);
                            if (this.readedBytes == 0)
                            {
                                this.endOfStream = true;
                            }
                            else
                            {
                                this.currentVarIndex = 0;
                            }

                            this.bitPos = rem;
                        }
                    }
                }
                else
                {
                    throw new NotSupportedException(
                        "Seek operations aren't supported by the underlying stream.");
                }
            }
        }

        /// <summary>
        /// Obtém um valor que indica se é possível mover o cursor no interior do
        /// fluxo.
        /// </summary>
        public bool CanSeek
        {
            get
            {
                return this.stream.CanSeek;
            }
        }

        /// <summary>
        /// Efectua a leitura de um bit.
        /// </summary>
        /// <returns>O bit lido ou -1 se se encontra no final.</returns>
        public int ReadBit()
        {
            if (this.endOfStream)
            {
                return -1;
            }
            else if (this.bitPos == 8)
            {
                ++this.currentVarIndex;
                if (this.currentVarIndex == this.readedBytes)
                {

                    this.readedBytes = this.stream.Read(
                        this.buffer,
                        0,
                        this.buffer.Length);
                    if (this.readedBytes == 0)
                    {
                        this.currentVarIndex = -1;
                        this.endOfStream = true;
                        return -1;
                    }
                    else
                    {
                        this.currentVarIndex = 0;
                        this.bitPos = 1;
                        return this.buffer[this.currentVarIndex] & 1;
                    }
                }
                else
                {
                    this.bitPos = 1;
                    return this.buffer[this.currentVarIndex] & 1;

                }
            }
            else
            {
                var current = this.buffer[this.currentVarIndex];
                var result = (current & (1 << this.bitPos)) >> this.bitPos;
                ++this.bitPos;
                return result;
            }
        }

        /// <summary>
        /// Efectua a leitura de um número especificado de bits.
        /// </summary>
        /// <remarks>
        /// A leitura dos bits é realizada de modo a que os bits menos significativos
        /// são encontrados ao início.
        /// </remarks>
        /// <param name="bitsBuffer">O vector que irá conter a leitura.</param>
        /// <param name="offset">A posição, em bits, onde será escrito o resultado da leitura.</param>
        /// <param name="count">O número de bits a ser lido.</param>
        /// <returns>O número de bits lido que irá depender do número de bits proporcionado
        /// e do número de bits disponível.
        /// </returns>
        public int ReadBits(byte[] bitsBuffer, int offset, int count)
        {
            if (bitsBuffer == null)
            {
                throw new ArgumentNullException("bitsBuffer");
            }
            else if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", "Offset must be non negative.");
            }
            else if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "Count must be non negative.");
            }
            else
            {
                var len = bitsBuffer.Length;
                if (offset + count > (len << 3))
                {
                    throw new ArgumentException("There's no enough space in bits buffer to the required operation.");
                }
                else
                {
                    if (this.endOfStream)
                    {
                        return 0;
                    }
                    else
                    {
                        var readed = 0;
                        if (this.bitPos == 8)
                        {
                            var mainWrite = offset >> 3;
                            var currPos = offset & 7;
                            if (currPos == 0)
                            {
                                readed = this.ReadAllAligned(bitsBuffer, mainWrite, count);
                            }
                            else
                            {
                                readed = this.ReadAlignedWriteUnaligned(
                                    bitsBuffer,
                                    mainWrite,
                                    currPos,
                                    count);
                            }
                        }
                        else
                        {
                            var mainWrite = offset >> 3;
                            var currPos = offset & 7;
                            var lastBits = 8 - this.bitPos;
                            if (count <= lastBits)
                            {
                                if (count <= 8 - currPos)
                                {
                                    var current = this.buffer[this.currentVarIndex];
                                    var write = (byte)(((current >> this.bitPos) & ((1 << count) - 1)) >> currPos);
                                    bitsBuffer[mainWrite] = write;
                                }
                                else
                                {
                                    var current = this.buffer[this.bitPos];
                                    var write = bitsBuffer[mainWrite];
                                    write |= (byte)((current << this.bitPos) >> currPos);
                                    bitsBuffer[mainWrite] = write;
                                    ++mainWrite;
                                    bitsBuffer[mainWrite] = (byte)(current << (this.bitPos - currPos));
                                }

                                this.bitPos += count;
                                readed = count;
                            }
                            else
                            {
                                // Alinha o vector de leitura.
                                if (lastBits < 8 - currPos)
                                {
                                    var current = this.buffer[this.currentVarIndex];
                                    var write = (byte)(((current >> this.bitPos) & ((1 << lastBits) - 1)) >> currPos);
                                    bitsBuffer[mainWrite] = write;
                                    currPos += lastBits;
                                }
                                else
                                {
                                    var current = this.buffer[this.currentVarIndex];
                                    var write = bitsBuffer[mainWrite];
                                    write |= (byte)((current >> this.bitPos) << currPos);
                                    bitsBuffer[mainWrite] = write;
                                    ++mainWrite;
                                    currPos -= this.bitPos;
                                    bitsBuffer[mainWrite] = (byte)(current >> (8 - currPos));
                                }

                                readed = lastBits;
                                var countTemp = count - lastBits;
                                this.bitPos = 8;
                                if (currPos == 0)
                                {
                                    readed += this.ReadAllAligned(
                                        bitsBuffer,
                                        mainWrite,
                                        countTemp);
                                }
                                else
                                {
                                    readed += this.ReadAlignedWriteUnaligned(
                                        bitsBuffer,
                                        mainWrite,
                                        currPos,
                                        countTemp);
                                }
                            }
                        }

                        return readed;
                    }
                }
            }
        }

        /// <summary>
        /// Estabelece a posição do cursor no fluxo.
        /// </summary>
        /// <param name="offset">O desvio em bits.</param>
        /// <param name="origin">A origem sobre a qual é aplicado o desvio.</param>
        /// <returns>A nova posição em bits no fluxo.</returns>
        public long Seek(long offset, SeekOrigin origin)
        {
            // TODO: Analisar o caso em que o movimento pode ser efectuado sem recarregar a variável buffer.
            // Isto implica verificar se o offset se encontra em alguma posição dos valores já carregados.
            // Efectuar o melhoramento após a execução dos testes.
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "offset",
                    "Offset value must be non negative.");
            }
            else if (this.stream.CanSeek)
            {
                this.endOfStream = false;
                if (origin == SeekOrigin.Begin)
                {
                    if (offset < 0)
                    {
                        throw new IOException("Can't set cursor position before start.");
                    }
                    else
                    {
                        var mainVal = offset >> 3;
                        var rem = (int)(mainVal & 7);
                        this.stream.Position = mainVal;
                        this.readedBytes = 0;
                        if (rem == 0)
                        {
                            this.currentVarIndex = -1;
                            this.bitPos = 8;
                        }
                        else
                        {
                            this.readedBytes = this.stream.Read(
                                this.buffer,
                                0,
                                this.buffer.Length);
                            if (this.readedBytes == 0)
                            {
                                this.endOfStream = true;
                            }
                            else
                            {
                                this.currentVarIndex = 0;
                            }

                            this.bitPos = rem;
                        }

                        return offset;
                    }
                }
                else if (origin == SeekOrigin.Current)
                {
                    var streamPosition = this.stream.Position;
                    if (offset > 0)
                    {
                        return this.SetAddPosition(streamPosition, offset);
                    }
                    else if (offset < 0)
                    {
                        return this.SetSubtractPosition(streamPosition, offset);
                    }
                    else
                    {
                        return this.Position;
                    }
                }
                else if (origin == SeekOrigin.End)
                {
                    var streamPosition = this.stream.Length - 1;
                    this.bitPos = 8;
                    if (offset > 0)
                    {
                        return this.SetAddPosition(streamPosition, offset);
                    }
                    else if (offset < 0)
                    {
                        return this.SetSubtractPosition(streamPosition, offset);
                    }
                    else
                    {
                        // Colocado no final do ficheiro.
                        this.stream.Seek(0, SeekOrigin.End);
                        this.bitPos = 8;
                        this.currentVarIndex = -1;
                        this.endOfStream = true;
                        return this.Position;
                    }
                }
                else
                {
                    throw new NotSupportedException(
                        "The provided seek origine type is not supported.");
                }
            }
            else
            {
                throw new NotSupportedException(
                        "Seek operations aren't supported by the underlying stream.");
            }
        }

        /// <summary>
        /// Efectua a leitura dos bits quando tanto os bytes de leitura como de escrita
        /// se encontram alinhados.
        /// </summary>
        /// <param name="bitsBuffer">O vector que irá conter a leitura.</param>
        /// <param name="offset">
        /// O deslocamento em bits relativamente ao início do vector que irá conter a leitura.
        /// </param>
        /// <param name="count">O número de bits a serem considerados.</param>
        /// <returns>O número de bits lido.</returns>
        private int ReadAllAligned(byte[] bitsBuffer, int offset, int count)
        {
            var readed = 0;
            ++this.currentVarIndex;
            if (this.currentVarIndex == this.readedBytes)
            {
                this.readedBytes = this.stream.Read(
                    this.buffer,
                    0,
                    this.buffer.Length);
                if (this.readedBytes == 0)
                {
                    this.currentVarIndex = -1;
                    this.endOfStream = true;
                    return readed;
                }
                else
                {
                    this.currentVarIndex = 0;
                }
            }

            var mainRead = count >> 3;
            var mainWrite = offset;
            var bytesReaded = 0;
            while (bytesReaded < mainRead)
            {
                var current = this.buffer[this.currentVarIndex];
                bitsBuffer[mainWrite] = current;
                ++mainWrite;
                readed += 8;
                ++bytesReaded;
                ++this.currentVarIndex;
                if (this.currentVarIndex == this.readedBytes)
                {
                    this.readedBytes = this.stream.Read(
                        this.buffer,
                        0,
                        this.buffer.Length);
                    if (this.readedBytes == 0)
                    {
                        this.endOfStream = true;
                        return readed;
                    }
                    else
                    {
                        this.currentVarIndex = 0;
                    }
                }
            }

            var remRead = count & 7;
            if (remRead > 0)
            {
                var temp = this.buffer[this.currentVarIndex];
                this.bitPos = remRead;
                bitsBuffer[mainWrite] = (byte)(temp & ((1 << remRead) - 1));
                readed += remRead;
            }
            else
            {
                --this.currentVarIndex;
            }

            return readed;
        }

        /// <summary>
        /// Efectua a leitura dos bits quando os bytes de leitura se encontram alinhados
        /// e os de escrita se encontram desalinhados.
        /// </summary>
        /// <param name="bitsBuffer">O vector que irá conter a leitura.</param>
        /// <param name="mainWrite">O deslocamento em bytes para a escrita.</param>
        /// <param name="writePos">O deslocamento adicional em bits par a escrita.</param>
        /// <param name="count">O número total de bits a serem lidos.</param>
        /// <returns>O número de bits lidos.</returns>
        private int ReadAlignedWriteUnaligned(
            byte[] bitsBuffer,
            int mainWrite,
            int writePos,
            int count)
        {
            var readed = 0;
            ++this.currentVarIndex;
            if (this.currentVarIndex == this.readedBytes)
            {
                this.readedBytes = this.stream.Read(
                    this.buffer,
                    0,
                    this.buffer.Length);
                if (this.readedBytes == 0)
                {
                    this.endOfStream = true;
                    return readed;
                }
                else
                {
                    this.currentVarIndex = 0;
                }
            }

            var mainRead = count >> 3;
            var bytesReaded = 0;
            while (bytesReaded < mainRead)
            {
                var current = this.buffer[this.currentVarIndex];
                var currentWrite = bitsBuffer[mainWrite];
                bitsBuffer[mainWrite++] = (byte)(currentWrite | (current << writePos));
                bitsBuffer[mainWrite] = (byte)(current >> (8 - writePos));

                readed += 8;
                ++bytesReaded;
                ++this.currentVarIndex;
                if (this.currentVarIndex == this.readedBytes)
                {
                    this.readedBytes = this.stream.Read(
                        this.buffer,
                        0,
                        this.buffer.Length);
                    if (this.readedBytes == 0)
                    {
                        this.endOfStream = true;
                        return readed;
                    }
                    else
                    {
                        this.currentVarIndex = 0;
                    }
                }
            }

            var remRead = count & 7;
            if (remRead > 0)
            {
                var temp = this.buffer[this.currentVarIndex];
                var lastBits = 8 - writePos;
                if (remRead <= lastBits)
                {
                    this.bitPos = remRead;
                    var currentWrite = bitsBuffer[mainWrite];
                    bitsBuffer[mainWrite] = (byte)(currentWrite | ((temp & ((1 << this.bitPos) - 1)) << writePos));
                }
                else
                {
                    var currentWrite = bitsBuffer[mainWrite];
                    bitsBuffer[mainWrite] = (byte)(currentWrite | ((temp & ((1 << this.bitPos) - 1)) << writePos));
                    ++mainWrite;
                    bitsBuffer[mainWrite] = (byte)((temp >> lastBits) & ((1 << (remRead - lastBits)) - 1));
                    this.bitPos = remRead;
                }

                readed += remRead;
            }

            return readed;
        }

        /// <summary>
        /// Estabelece a posição do cursor relativamente 
        /// à origem especificada após adição com a posição actual.
        /// </summary>
        /// <param name="currBytes">A origem especificada.</param>
        /// <param name="offset">A posição em bits a ser estabelecida.</param>
        /// <returns>O valor da posição.</returns>
        private long SetAddPosition(long currBytes, long offset)
        {
            var mainVal = offset >> 3;
            var rem = (int)(mainVal & 7);
            mainVal += currBytes + (rem >> 3);
            rem &= 7;
            this.stream.Seek(mainVal, SeekOrigin.Current);

            if (rem == 0)
            {
                this.readedBytes = 0;
                this.currentVarIndex = -1;
                this.bitPos = 8;
            }
            else
            {
                this.readedBytes = this.stream.Read(
                    this.buffer,
                    0,
                    this.buffer.Length);
                if (this.readedBytes == 0)
                {
                    this.endOfStream = true;
                }
                else
                {
                    this.currentVarIndex = 0;
                }

                this.currentVarIndex = 0;
                this.bitPos = rem;
            }

            return (currBytes >> 3) + offset;
        }

        /// <summary>
        /// Estabelece a posição do cursor relativamente 
        /// à origem especificada após diferença com a posição actual.
        /// </summary>
        /// <param name="currBytes">A origem especificada.</param>
        /// <param name="offset">A posição em bits a ser estabelecida.</param>
        /// <returns>O valor da posição</returns>
        private long SetSubtractPosition(long currBytes, long offset)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Implementa um escritor de bits.
    /// </summary>
    public class BitWriter : IDisposable
    {
        /// <summary>
        /// Mantém o fluxo de onde serão lidos os bits.
        /// </summary>
        private Stream stream;

        /// <summary>
        /// O amortecedor interno.
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// O índice do vector onde se encontra o cursor.
        /// </summary>
        private int currentVarIndex;

        /// <summary>
        /// O índice do bit na entrada do vector onde se encontra o cursor.
        /// </summary>
        private int bitPos;

        /// <summary>
        /// Valor que indica se o escritor foi eliminado.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipos <see cref="BitWriter"/>.
        /// </summary>
        /// <param name="stream">O fluxo subjacente.</param>
        public BitWriter(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            else
            {
                if (stream.CanWrite)
                {
                    this.buffer = new byte[8];
                    this.currentVarIndex = -1;
                    this.bitPos = 8;
                    this.stream = stream;
                    this.disposed = false;
                }
                else
                {
                    throw new NotSupportedException(
                           "The underlying stream doesn't support writes.");
                }
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipos <see cref="BitWriter"/>.
        /// </summary>
        /// <param name="stream">O fluxo subjacente.</param>
        /// <param name="capacity">A capacidade do contentor interno.</param>
        public BitWriter(Stream stream, int capacity)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            else if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    "capacity",
                    "Capacity must be a positive value.");
            }
            else
            {
                if (stream.CanWrite)
                {
                    this.buffer = new byte[capacity];
                    this.currentVarIndex = -1;
                    this.bitPos = 8;
                    this.stream = stream;
                    this.disposed = false;
                }
                else
                {
                    throw new NotSupportedException(
                        "The underlying stream doesn't support writes.");
                }
            }
        }

        /// <summary>
        /// Escreve um bit.
        /// </summary>
        /// <remarks>
        /// Qualquer valor diferente de zero corresponde ao bit 1.
        /// </remarks>
        /// <param name="bit">O bit a ser escrito.</param>
        public void WriteBit(byte bit)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("BitWriter");
            }
            else if (this.bitPos == 8)
            {
                ++this.currentVarIndex;
                if (this.currentVarIndex == this.buffer.Length)
                {
                    this.stream.Write(
                        this.buffer,
                        0,
                        this.buffer.Length);
                    this.stream.Flush();
                    this.currentVarIndex = 0;
                }

                if (bit == 0)
                {
                    this.buffer[this.currentVarIndex] = 0;
                }
                else
                {
                    this.buffer[this.currentVarIndex] = 1;
                }

                this.bitPos = 1;
            }
            else
            {
                var current = this.buffer[this.currentVarIndex];
                if (bit == 0)
                {
                    this.buffer[this.currentVarIndex] = (byte)(current & ((1 << this.bitPos) - 1));
                }
                else
                {
                    this.buffer[this.currentVarIndex] = (byte)(current | (1 << this.bitPos));
                }

                ++this.bitPos;
            }
        }

        /// <summary>
        /// Escreve os bits especificados.
        /// </summary>
        /// <param name="buffer">O contentor de bits.</param>
        /// <param name="offset">O desvio em bits sobre o início do vector.</param>
        /// <param name="bits">O número de bits a ser escrito.</param>
        public void WriteBits(byte[] buffer, int offset, int bits)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            else if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", "Offset must be non negative.");
            }
            else if (bits < 0)
            {
                throw new ArgumentOutOfRangeException("bits", "Bits must be non negative.");
            }
            else if (bits > 0)
            {
                var length = buffer.Length;
                if (bits + offset > (length << 3))
                {
                    throw new ArgumentOutOfRangeException(
                        "bits",
                        "The number of bits is bigger than the number supported by buffer within the provided offset.");
                }
                else
                {
                    var mainOffset = offset >> 3;
                    var remOffset = offset & 7;
                    var buffLen = this.buffer.Length;

                    if (this.bitPos == 8)
                    {
                        ++this.currentVarIndex;
                        if (this.currentVarIndex == this.buffer.Length)
                        {
                            this.stream.Write(
                                this.buffer,
                                0,
                                this.buffer.Length);
                            this.stream.Flush();
                            this.currentVarIndex = 0;
                        }

                        var mainLen = bits >> 3;
                        var mainRem = bits & 7;

                        if (remOffset == 0)
                        {
                            this.WriteAllAligned(
                                buffer,
                                mainOffset,
                                mainLen);

                            if (mainRem > 0)
                            {
                                ++this.currentVarIndex;
                                var read = buffer[mainLen] & ((1 << mainRem) - 1);
                                this.buffer[this.currentVarIndex] = (byte)read;
                                this.bitPos = mainRem;
                            }
                        }
                        else
                        {
                            this.WriteAlignedReadUnaligned(
                                buffer,
                                mainOffset,
                                remOffset,
                                mainLen);

                            if (mainRem > 0)
                            {
                                ++this.currentVarIndex;
                                this.buffer[0] = (byte)((buffer[mainLen] & ((1 << mainRem) - 1)));
                                var lastRem = mainRem + remOffset;
                                if (lastRem > 8)
                                {
                                    lastRem -= 8;
                                    ++mainLen;
                                    var write = this.buffer[0];
                                    var read = buffer[mainLen];
                                    write |= (byte)((read & ((1 << lastRem) - 1)) << (mainRem - lastRem));
                                    this.buffer[0] = write;
                                }

                                this.bitPos = mainRem;
                            }
                            else
                            {
                                this.bitPos = 8;
                            }
                        }
                    }
                    else
                    {
                        // Alinha a escrita
                        var lastBits = 8 - this.bitPos;
                        if (bits <= lastBits)
                        {
                            var lastRem = 8 - remOffset;
                            if (bits > lastRem)
                            {
                                var currWrite = this.buffer[this.currentVarIndex];
                                var currRead = buffer[mainOffset];
                                currWrite = (byte)(currWrite | ((currRead >> remOffset) << this.bitPos));
                                currRead = buffer[mainOffset + 1];
                                currRead &= (byte)((1 << (bits - lastRem) - 1));
                                currWrite |= (byte)(currRead << (this.bitPos + lastRem));

                                this.bitPos += bits;
                            }
                            else
                            {
                                var currWrite = this.buffer[this.currentVarIndex];
                                var currRead = buffer[mainOffset];
                                currRead = (byte)((currRead >> remOffset) & ((1 << bits) - 1));
                                this.buffer[this.currentVarIndex] = (byte)(currWrite | (currRead << this.bitPos));
                            }

                            this.bitPos += bits;
                        }
                        else
                        {
                            var lastRem = 8 - remOffset;
                            if (lastRem < lastBits)
                            {
                                var currWrite = this.buffer[this.currentVarIndex];
                                var currRead = buffer[mainOffset];
                                currWrite |= (byte)((currRead >> remOffset) << this.bitPos);
                                ++mainOffset;
                                currRead = buffer[mainOffset];
                                currWrite |= (byte)((currRead & ((1 << (lastBits - lastRem)) - 1)) << (this.bitPos + lastRem));
                                this.buffer[this.currentVarIndex] = currWrite;
                                remOffset = lastBits - lastRem;
                            }
                            else
                            {
                                var currWrite = this.buffer[this.currentVarIndex];
                                var currRead = buffer[mainOffset];
                                currRead = (byte)((currRead >> remOffset) & ((1 << lastBits) - 1));
                                currWrite |= (byte)(currRead << this.bitPos);
                                this.buffer[this.currentVarIndex] = currWrite;
                                remOffset += lastBits;
                            }

                            var newbits = bits - lastBits;
                            var mainLen = newbits >> 3;
                            var mainRem = newbits & 7;
                            ++this.currentVarIndex;
                            this.WriteAlignedReadUnaligned(
                                buffer,
                                mainOffset,
                                remOffset,
                                mainLen);

                            if (mainRem > 0)
                            {
                                ++this.currentVarIndex;
                                this.buffer[0] = (byte)(((buffer[mainLen] >> lastBits) & ((1 << mainRem) - 1)));
                                lastRem = mainRem + remOffset;
                                if (lastRem > 8)
                                {
                                    lastRem -= 8;
                                    ++mainLen;
                                    var write = this.buffer[0];
                                    var read = buffer[mainLen];
                                    write |= (byte)((read & ((1 << lastRem) - 1)) << (mainRem - lastRem));
                                    this.buffer[0] = write;
                                }

                                this.bitPos = mainRem;
                            }
                            else
                            {
                                this.bitPos = 8;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Envia a parte escrita do contentor para o fluxo, exeptuando
        /// os últimos bits.
        /// </summary>
        public void Flush()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("BitWriter");
            }
            else
            {
                this.InnerFlush();
            }
        }

        /// <summary>
        /// Envia a parte escrita do contentor para o fluxo, incluindo os últimos
        /// bits e anexando zeros para completar um byte.
        /// </summary>
        public void ForceFlush()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("BitWriter");
            }
            else
            {
                this.stream.Write(
                    this.buffer,
                    0,
                    this.currentVarIndex + 1);
                this.bitPos = 8;
                this.currentVarIndex = -1;
            }
        }

        /// <summary>
        /// Liberta os recursos associados ao escritor.
        /// </summary>
        public void Dispose()
        {
            this.InnerFlush();
            this.stream = null;
            this.disposed = true;
        }

        /// <summary>
        /// Escreve os bits quando estes se encontram alinhados.
        /// </summary>
        /// <param name="buffer">O contentor dos bits a serem escritos.</param>
        /// <param name="offset">O índice a partir do qual os bits serão escritos.</param>
        /// <param name="length">O comprimento em bytes a ser escrito.</param>
        private void WriteAllAligned(
            byte[] buffer,
            int offset,
            int length)
        {
            if (this.currentVarIndex > 0)
            {
                this.stream.Write(
                    this.buffer,
                    0,
                    this.currentVarIndex);
            }

            if (length > 0)
            {
                this.stream.Write(
                    buffer,
                    offset,
                    length);
            }

            this.currentVarIndex = -1;
        }

        /// <summary>
        /// Escreve os bits quando a escrita se encontra alinhada.
        /// </summary>
        /// <param name="buffer">O contentor dos bits a serem escritos.</param>
        /// <param name="mainOffset">O desvio em bytes principal.</param>
        /// <param name="remOffset">O resto do desvio em bits.</param>
        /// <param name="mainLen">O comprimento em bytes a ser escrito.</param>
        private void WriteAlignedReadUnaligned(
            byte[] buffer,
            int mainOffset,
            int remOffset,
            int mainLen)
        {
            if (this.currentVarIndex > 0)
            {
                this.stream.Write(
                    this.buffer,
                    0,
                    this.currentVarIndex);
            }

            this.currentVarIndex = -1;

            if (mainLen > 0)
            {
                var prev = buffer[mainOffset];
                var i = mainOffset + 1;
                while (i < mainLen)
                {
                    var curr = buffer[i];
                    var write = (byte)((prev >> remOffset) | (curr << (8 - remOffset)));
                    this.stream.WriteByte(write);
                    prev = curr;
                    ++i;
                }

                var writeOut = (byte)((prev >> remOffset) | (buffer[i] << (8 - remOffset)));
                this.stream.WriteByte(writeOut);
            }
        }

        /// <summary>
        /// Função interna para escrita do contentor para o fluxo.
        /// </summary>
        private void InnerFlush()
        {
            if (this.bitPos == 8)
            {
                this.stream.Write(
                    this.buffer,
                    0,
                    this.currentVarIndex + 1);
                this.currentVarIndex = -1;
            }
            else
            {
                this.stream.Write(
                    this.buffer,
                    0,
                    this.currentVarIndex);
                this.buffer[0] = this.buffer[this.currentVarIndex];
                this.currentVarIndex = 0;
            }

            this.stream.Flush();
        }
    }
}
