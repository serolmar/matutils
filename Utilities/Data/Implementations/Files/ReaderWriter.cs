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
            else if (offset < 0 || count < 0)
            {
                throw new ArgumentOutOfRangeException("Offset and count must be non negative.");
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
                        return -1;
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
                                readed = this.ReadAllAligned(bitsBuffer, offset, count);
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
                                    var write = bitsBuffer[mainWrite];
                                    write |= (byte)(((current >> this.bitPos) & ((1 << count) - 1)) >> currPos);
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
                                    var write = bitsBuffer[mainWrite];
                                    write |= (byte)(((current >> this.bitPos) & ((1 << lastBits) - 1)) >> currPos);
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
                bitsBuffer[mainWrite] = (byte)(temp & ((1 << remRead)-1));
                readed += remRead;
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
    }
}
