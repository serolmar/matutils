// -----------------------------------------------------------------------
// <copyright file="MediaFileFormats.cs" company="Sérgio O. Marques">
// TODO: Update copyright text.
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
    /// Implementa um leitor de ficheiros FLAC.
    /// </summary>
    public class FlacReader
    {
        /// <summary>
        /// Mantém o leitor de bits.
        /// </summary>
        private Stream stream;

        /// <summary>
        /// Mantém o estado de leitura do FLAC.
        /// </summary>
        /// <remarks>
        /// 0 - Leitura do marcador
        /// 1 - Leitura do bloco principal
        /// 2 - Último bloco lido
        /// 3 - Existe próximo bloco
        /// 4 - Cabeçalho da moldura foi lido
        /// </remarks>
        private int state;

        private StreamInfoBlock streamInfo;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="FlacReader"/>.
        /// </summary>
        /// <param name="stream"></param>
        public FlacReader(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            else
            {
                this.stream = stream;
                this.state = 0;
            }
        }

        /// <summary>
        /// Efectua a leitura do marcador FLAC no início do ficheiro.
        /// </summary>
        /// <returns>
        /// Verdadeiro se se trata do marcador FLAC e falso caso contrário.
        /// </returns>
        public bool ReadMarker()
        {
            if (this.state == 0)
            {
                var buffer = new byte[4];
                var readed = this.stream.Read(buffer, 0, 4);
                if (readed != 4)
                {
                    return false;
                }
                else
                {
                    var marker = "fLaC";
                    var comparision = ASCIIEncoding.ASCII.GetBytes(
                        marker);
                    for (var i = 0; i < 4; ++i)
                    {
                        if (buffer[i] != comparision[i])
                        {
                            return false;
                        }
                    }

                    this.state = 1;
                    return true;
                }
            }
            else
            {
                throw new UtilitiesException("Read Marker was already processed.");
            }
        }

        /// <summary>
        /// Obtém o cabeçalho principal obrigatório.
        /// </summary>
        /// <returns>O cabeçalho.</returns>
        public IStreamInfoBlock GetStreamInfo()
        {
            if (this.state == 0)
            {
                throw new UtilitiesException("FLAC marker was not processed yet.");
            }
            else if (this.state == 1)
            {
                // Leitura do bloco
                var result = new StreamInfoBlock();
                result.Header = this.ReadBlockHeader();
                result.MinimumBlockSize = this.ReadUintValue(2);
                result.MaximumBlockSize = this.ReadUintValue(2);
                result.MinimumFrameSize = this.ReadUintValue(3);
                result.MaximumFrameSize = this.ReadUintValue(3);

                var buffer = new byte[16];
                var readed = this.stream.Read(buffer, 0, 8);
                if (readed == 8)
                {
                    this.ProcessStreamInfoFields(
                        buffer,
                        result);
                }
                else
                {
                    throw new UtilitiesDataException("File ended before reading of stream info block.");
                }

                readed = this.stream.Read(buffer, 0, 16);
                if (readed < 16)
                {
                    throw new UtilitiesDataException("File ended before reading of stream info.");
                }
                else
                {
                    for (var i = 0; i < 16; ++i)
                    {
                        result.InternalMD5Signature.Add(buffer[i]);
                    }
                }

                // Verifica se o número lido de bits coincide com o estabelecido no cabeçalho.
                if (34 != result.Header.BlockLength)
                {
                    throw new UtilitiesException(string.Format(
                         "Block length {0} doesn't match with header specification {1}.",
                         272,
                         (result.Header.BlockLength << 3)));
                }

                if (result.Header.IsLastBlock)
                {
                    this.state = 2;
                }
                else
                {
                    this.state = 3;
                }

                this.streamInfo = result;
                return result;
            }
            else
            {
                throw new UtilitiesException("Stream info header was already processed.");
            }
        }

        /// <summary>
        /// Obtém o próximo bloco.
        /// </summary>
        /// <returns>O bloco.</returns>
        public IBlock GetNextBlock()
        {
            if (this.state == 0)
            {
                throw new UtilitiesException("FLAC marker was not processed yet.");
            }
            else if (this.state == 1)
            {
                throw new UtilitiesException("Main block was not processed yet.");
            }
            else if (this.state == 2)
            {
                throw new UtilitiesException("Last block was already processed.");
            }
            if (this.state == 3)
            {
                var result = default(IBlock);
                var header = this.ReadBlockHeader();
                switch (header.BlockType)
                {
                    case EBlockType.Application:
                        throw new NotSupportedException("Application");
                    case EBlockType.CueSheet:
                        throw new NotSupportedException("CueSheet");
                    case EBlockType.Padding:
                        break;
                    case EBlockType.Picture:
                        result = new PictureBlock(header, this);
                        return result;
                    case EBlockType.SeekTable:
                        break;
                    case EBlockType.StreamInfo:
                        throw new UtilitiesException("Unexpected stream info.");
                    case EBlockType.VorbisComment:
                        result = new VorbisCommentBlock(header, this);
                        return result;
                    default:
                        throw new NotSupportedException("Block type not supported.");
                }

                if (result.Header.IsLastBlock)
                {
                    this.state = 2;
                }

                return result;
            }
            else
            {
                throw new UtilitiesException("Unknown error.");
            }
        }

        #region Funções privadas

        /// <summary>
        /// Efectua a leitura do cabeçalho da moldura.
        /// </summary>
        /// <returns>O cabeçalho da moldura.</returns>
        private IFrameHeader ReadFrameHeader()
        {
            if (this.state == 2)
            {
                var result = new FrameHeader();
                var buffer = new byte[1];
                var stat = 0;
                while (stat != -1)
                {
                    var x = buffer[0];
                    if (stat == 0)
                    {
                        var readed = this.stream.Read(buffer, 0, 1);
                        if (readed != 1)
                        {
                            throw new UtilitiesDataException("Can't find synchronization code.");
                        }

                        if (x == 0xFF)
                        {
                            stat = 1;
                        }
                    }
                    else if (stat == 1)
                    {
                        var readed = this.stream.Read(buffer, 0, 1);
                        if (readed != 1)
                        {
                            throw new UtilitiesDataException("Can't find synchronization code.");
                        }

                        if ((x >> 1) == 0x7C)
                        {
                            stat = 2;
                        }
                        else if (x != 0xFF)
                        {
                            // Falhou a determinação do código de sincronização.
                            stat = 0;
                        }
                    }
                    else if (stat == 2)
                    {
                        stat = this.ReadFrameHeader(result, buffer);
                    }
                }

                this.state = 4;
                return result;
            }
            else
            {
                throw new UtilitiesException(
                    "Frame header must be read just after all metadata blocks or other frames.");
            }
        }

        /// <summary>
        /// Efectua a leitura do cabeçalho da moldura.
        /// </summary>
        /// <param name="header">O cabeçalho que irá conter a leitura.</param>
        /// <param name="buffer">O amortecedor para a realização de leitura.</param>
        /// <returns>O estado da leitura.</returns>
        private int ReadFrameHeader(
            FrameHeader header,
            byte[] buffer)
        {
            var sync = buffer[0];
            header.IsUnparseable = false;
            if ((sync & 0x02) != 0)
            {
                header.IsUnparseable = true;
            }

            var readed = this.stream.Read(buffer, 0, 1);
            if (readed != 1)
            {
                throw new UtilitiesDataException("Premature end of stream reading header.");
            }

            var x = buffer[0];
            if (x == 0xFF)
            {
                // Procura pelo próximo código de sincronização
                return 1;
            }

            var blockHint = 0;
            var innerX = x >> 4;
            switch (innerX)
            {
                case 0:
                    header.IsUnparseable = true;
                    break;
                case 1:
                    header.BlockSizeInInterChannelSamples = 192;
                    break;
                case 2:
                    goto case 3;
                case 3:
                    goto case 4;
                case 4:
                    goto case 5;
                case 5:
                    header.BlockSizeInInterChannelSamples = (byte)(576 << (x - 2));
                    break;
                case 6:
                    goto case 7;
                case 7:
                    blockHint = innerX;
                    break;
                case 8:
                    goto case 9;
                case 9:
                    goto case 10;
                case 10:
                    goto case 11;
                case 11:
                    goto case 12;
                case 12:
                    goto case 13;
                case 13:
                    goto case 14;
                case 14:
                    goto case 15;
                case 15:
                    header.BlockSizeInInterChannelSamples = (byte)(256 << (x - 8));
                    break;
            }

            var sampleRateHint = 0;
            innerX = x & 0x0F;
            switch (innerX)
            {
                case 0:
                    if (this.stream == null)
                    {
                        header.IsUnparseable = true;
                    }
                    else
                    {
                        header.SampleRate = this.streamInfo.SampleRate;
                    }

                    break;
                case 1:
                    header.SampleRate = 88200;
                    break;
                case 2:
                    header.SampleRate = 176400;
                    break;
                case 3:
                    header.SampleRate = 192000;
                    break;
                case 4:
                    header.SampleRate = 8000;
                    break;
                case 5:
                    header.SampleRate = 16000;
                    break;
                case 6:
                    header.SampleRate = 22050;
                    break;
                case 7:
                    header.SampleRate = 24000;
                    break;
                case 8:
                    header.SampleRate = 32000;
                    break;
                case 9:
                    header.SampleRate = 44100;
                    break;
                case 10:
                    header.SampleRate = 48000;
                    break;
                case 11:
                    header.SampleRate = 96000;
                    break;
                case 12:
                    goto case 13;
                case 13:
                    goto case 14;
                case 14:
                    sampleRateHint = innerX;
                    break;
                case 15:
                    // Procura pelo próximo código de sincronização.
                    return 1;
            }

            readed = this.stream.Read(buffer, 0, 1);
            if (readed != 1)
            {
                throw new UtilitiesDataException("Premature end of stream reading header.");
            }

            x = buffer[0];
            innerX = x & 8;
            header.ChannelAssignement = (byte)innerX;

            innerX = (x & 0x0E) >> 1;
            switch (x)
            {
                case 0:
                    if (this.streamInfo == null)
                    {
                        header.IsUnparseable = true;
                    }
                    else
                    {
                        header.SampleSizeInBits = (byte)this.streamInfo.BitsPerSample;
                    }

                    break;
                case 1:
                    header.SampleSizeInBits = 8;
                    break;
                case 2:
                    header.SampleSizeInBits = 12;
                    break;
                case 4:
                    header.SampleSizeInBits = 16;
                    break;
                case 5:
                    header.SampleSizeInBits = 20;
                    break;
                case 6:
                    header.SampleSizeInBits = 24;
                    break;
                case 3:
                    goto case 7;
                case 7:
                    header.IsUnparseable = true;
                    break;
            }

            if ((x & 0x01) != 0)
            {
                header.IsUnparseable = true;
            }

            if ((sync & 0x01) != 0 ||
                (this.stream != null & this.streamInfo.MinimumBlockSize != this.streamInfo.MaximumBlockSize))
            {
                // Bloco de tamanho variável
                if (this.ReadCodedSampleNumber64(
                    header,
                    buffer))
                {
                    if (buffer[0] == 0xFF)
                    {
                        return 1;
                    }
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                // Bloco de tamanho fixo
                if (this.ReadCodedSampleNumber32(
                    header,
                    buffer))
                {
                    if (buffer[0] == 0xFF)
                    {
                        return 1;
                    }
                }
                else
                {
                    return 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Efectua a leitura do número codificado da amostra.
        /// </summary>
        /// <param name="header">O cabeçalho que irá conter a leitura.</param>
        /// <param name="buffer">O amortecedor.</param>
        /// <returns>
        /// Verdadeiro se a leitura for bem sucedida e falso casos contrário.
        /// </returns>
        private bool ReadCodedSampleNumber64(
            FrameHeader header,
            byte[] buffer)
        {
            var readed = this.stream.Read(buffer, 0, 1);
            if (readed != 1)
            {
                throw new UtilitiesDataException("Premature end of stream reading header.");
            }

            var v = default(ulong);
            var i = default(uint);
            var innerx = (uint)buffer[0];

            if ((innerx & 0x80) == 0)
            { /* 0xxxxxxx */
                v = innerx;
                i = 0;
            }
            else if ((innerx & 0xC0) != 0
                && (innerx & 0x20) == 0)
            { /* 110xxxxx */
                v = innerx & 0x1F;
                i = 1;
            }
            else if ((innerx & 0xE0) != 0
                && (innerx & 0x10) == 0)
            { /* 1110xxxx */
                v = innerx & 0x0F;
                i = 2;
            }
            else if ((innerx & 0xF0) != 0
                && (innerx & 0x08) == 0)
            { /* 11110xxx */
                v = innerx & 0x07;
                i = 3;
            }
            else if ((innerx & 0xF8) != 0
                && (innerx & 0x04) == 0)
            { /* 111110xx */
                v = innerx & 0x03;
                i = 4;
            }
            else if ((innerx & 0xFC) != 0
                && (innerx & 0x02) == 0)
            { /* 1111110x */
                v = innerx & 0x01;
                i = 5;
            }
            else if ((innerx & 0xFE) != 0
                && (innerx & 0x01) == 0)
            { /* 11111110 */
                v = 0;
                i = 6;
            }
            else
            {
                return true;
            }

            for (; i > 0; i--)
            {
                readed = this.stream.Read(buffer, 0, 1);
                if (readed != 1)
                {
                    throw new UtilitiesDataException("Premature end of stream reading header.");
                }

                innerx = buffer[0];
                if ((innerx & 0x80) == 0 || (innerx & 0x40) != 0)
                { /* 10xxxxxx */
                    buffer[0] = 0xFF;
                    return true;
                }

                v <<= 6;
                v |= (innerx & 0x3F);
            }

            header.CodedSampleNumber = v;
            return true;
        }

        /// <summary>
        /// Efectua a leitura do número codificado da amostra.
        /// </summary>
        /// <param name="header">O cabeçalho que irá conter a leitura.</param>
        /// <param name="buffer">O amortecedor.</param>
        /// <returns>
        /// Verdadeiro se a leitura for bem sucedida e falso casos contrário.
        /// </returns>
        private bool ReadCodedSampleNumber32(
            FrameHeader header,
            byte[] buffer)
        {
            var readed = this.stream.Read(buffer, 0, 1);
            if (readed != 1)
            {
                throw new UtilitiesDataException("Premature end of stream reading header.");
            }

            var v = default(uint);
            var i = default(uint);
            var innerx = (uint)buffer[0];

            if ((innerx & 0x80) == 0)
            { /* 0xxxxxxx */
                v = innerx;
                i = 0;
            }
            else if ((innerx & 0xC0) != 0 && (innerx & 0x20) == 0)
            { /* 110xxxxx */
                v = innerx & 0x1F;
                i = 1;
            }
            else if ((innerx & 0xE0) != 0 && (innerx & 0x10) == 0)
            { /* 1110xxxx */
                v = innerx & 0x0F;
                i = 2;
            }
            else if ((innerx & 0xF0) != 0 && (innerx & 0x08) == 0)
            { /* 11110xxx */
                v = innerx & 0x07;
                i = 3;
            }
            else if ((innerx & 0xF8) != 0 && (innerx & 0x04) == 0)
            { /* 111110xx */
                v = innerx & 0x03;
                i = 4;
            }
            else if ((innerx & 0xFC) != 0 && (innerx & 0x02) == 0)
            { /* 1111110x */
                v = innerx & 0x01;
                i = 5;
            }
            else
            {
                return true;
            }
            for (; i > 0; i--)
            {
                readed = this.stream.Read(buffer, 0, 1);
                if (readed != 1)
                {
                    throw new UtilitiesDataException("Premature end of stream reading header.");
                }

                innerx = buffer[0];
                if ((innerx & 0x80) == 0 || (innerx & 0x40) != 0)
                { /* 10xxxxxx */
                    buffer[0] = 0xFF;
                    return true;
                }

                v <<= 6;
                v |= (innerx & 0x3F);
            }

            header.CodedSampleNumber = v;
            return true;
        }

        /// <summary>
        /// Efectua a leitura do cabeçalho.
        /// </summary>
        /// <returns>
        /// O cabeçalho.
        /// </returns>
        private BlockHeader ReadBlockHeader()
        {
            var result = new BlockHeader();

            var buffer = new byte[3];
            var readedByte = this.stream.ReadByte();
            if (readedByte == -1)
            {
                throw new UtilitiesDataException("File ended before reading of header block.");
            }
            else
            {
                if ((readedByte & 0x80) != 0)
                {
                    result.IsLastBlock = true;
                }

                var val = readedByte & 0x7F;
                switch (val)
                {
                    case 0:
                        result.BlockType = EBlockType.StreamInfo;
                        break;
                    case 1:
                        result.BlockType = EBlockType.Padding;
                        break;
                    case 2:
                        result.BlockType = EBlockType.Application;
                        break;
                    case 3:
                        result.BlockType = EBlockType.Application;
                        break;
                    case 4:
                        result.BlockType = EBlockType.VorbisComment;
                        break;
                    case 5:
                        result.BlockType = EBlockType.CueSheet;
                        break;
                    case 6:
                        result.BlockType = EBlockType.Picture;
                        break;
                    default:
                        throw new UtilitiesException(
                            string.Format("Unexpected block type: {0}", val));
                }

                var count = this.stream.Read(buffer, 0, 3);
                if (count == 3)
                {
                    var blockLength = (((uint)buffer[0]) << 16) + (((uint)buffer[1]) << 8) + buffer[2];
                    result.BlockLength = blockLength;
                    return result;
                }
                else
                {
                    throw new UtilitiesDataException("File ended before reading of header block.");
                }
            }
        }

        /// <summary>
        /// Processa os restantes campos do bloco inicial.
        /// </summary>
        /// <param name="buffer">O vector temporário.</param>
        /// <param name="streamInfo">O bloco principal.</param>
        private void ProcessStreamInfoFields(
            byte[] buffer,
            StreamInfoBlock streamInfo)
        {
            var current = (uint)buffer[0];
            var result = current << 12;
            current = buffer[1];
            result |= (current << 4);
            current = buffer[2];
            result |= (current >> 4);
            streamInfo.SampleRate = result;

            result = (current & 0x7) >> 1;
            streamInfo.NumberOfChannels = (short)(result + 1);

            result = (current & 1) << 5;
            current = buffer[3];
            result |= (current >> 4);
            streamInfo.BitsPerSample = (short)(result + 1);

            var totalSamples = (ulong)(current & 0xF) << 32;
            current = buffer[4];
            totalSamples |= current << 24;
            current = buffer[5];
            totalSamples |= current << 16;
            current = buffer[6];
            totalSamples |= current << 8;
            current = buffer[7];
            totalSamples |= current;
            streamInfo.TotalSamplesInStream = totalSamples;
        }

        /// <summary>
        /// Efectua a leitura de um inteiro sem sinal a partir de
        /// um valor providenciado de bytes.
        /// </summary>
        /// <param name="bytes">O número de bytes no inteiro.</param>
        /// <returns>O inteiro sem sinal.</returns>
        private uint ReadUintValue(int bytes)
        {
            var buffer = new byte[bytes];
            var readed = this.stream.Read(buffer, 0, bytes);
            if (readed == bytes)
            {
                var result = (uint)buffer[0];
                for (var i = 1; i < bytes; ++i)
                {
                    result <<= 8;
                    result |= buffer[i];
                }

                return result;
            }
            else
            {
                throw new UtilitiesDataException("File ended before reading of header block.");
            }
        }

        #endregion

        #region Tipos internos

        /// <summary>
        /// Tipo de bloco.
        /// </summary>
        public enum EBlockType
        {
            /// <summary>
            /// Informação sobre todo o ficheiro.
            /// </summary>
            StreamInfo = 0,

            /// <summary>
            /// Bloco que indica o espaço livre reservado antes
            /// de serem escritos os dados.
            /// </summary>
            /// <remarks>
            /// O espaço livre permite facilitar futuras edições.
            /// </remarks>
            Padding = 1,

            /// <summary>
            /// Usado por aplicações externas.
            /// </summary>
            Application = 2,

            /// <summary>
            /// Armazena pontos de referência para zonas do ficheiro.
            /// </summary>
            SeekTable = 3,

            /// <summary>
            /// Lista de pares nome / valor.
            /// </summary>
            VorbisComment = 4,

            /// <summary>
            /// Bloco de sinalização.
            /// </summary>
            CueSheet = 5,

            /// <summary>
            /// Bloco para armazenamento de imagem.
            /// </summary>
            Picture = 6
        }

        /// <summary>
        /// O tipo de pista na folha de sinalização.
        /// </summary>
        public enum ECueTrackType
        {
            /// <summary>
            /// Trata-se de audio.
            /// </summary>
            Audio = 0,

            /// <summary>
            /// Não se trata de audio.
            /// </summary>
            NonAudio = 1
        }

        /// <summary>
        /// O tipo de figura.
        /// </summary>
        public enum EPictureType
        {
            /// <summary>
            /// Outro.
            /// </summary>
            Other = 0,

            /// <summary>
            /// Ícone PNG 32x32.
            /// </summary>
            PngFileIcon = 1,

            /// <summary>
            /// Outro tipo de ícone.
            /// </summary>
            OtherFileIcon = 2,

            /// <summary>
            /// Cobertura frontal.
            /// </summary>
            FrontCover = 3,

            /// <summary>
            /// Cobertura traseira.
            /// </summary>
            BackCover = 4,

            /// <summary>
            /// Folheto.
            /// </summary>
            LeafletPage = 5,

            /// <summary>
            /// Média.
            /// </summary>
            /// <remarks>
            /// Um exemplo constitui a etiqueta lateral de um CD.
            /// </remarks>
            Media = 6,

            /// <summary>
            /// Artista ou executante principal ou solista.
            /// </summary>
            LeadArtist = 7,

            /// <summary>
            /// Artista ou executante   .
            /// </summary>
            Artist = 8,

            /// <summary>
            /// Maestro.
            /// </summary>
            Conductor = 9,

            /// <summary>
            /// Orquestra ou banda.
            /// </summary>
            Orchestra = 10,

            /// <summary>
            /// Compositor.
            /// </summary>
            Composer = 11,

            /// <summary>
            /// Autor da letra.
            /// </summary>
            Lyricist = 12,

            /// <summary>
            /// Local de gravação.
            /// </summary>
            RecordingLocation = 13,

            /// <summary>
            /// Durante a gravação.
            /// </summary>
            DuringRecording = 14,

            /// <summary>
            /// Durante o desempenho.
            /// </summary>
            DuringPerformance = 15,

            /// <summary>
            /// Captura de ecrão de filme ou vídeo.
            /// </summary>
            MovieScreenCapture = 16,

            /// <summary>
            /// Símbolo colorido brilhante.
            /// </summary>
            BrightColouredFish = 17,

            /// <summary>
            /// Ilustração.
            /// </summary>
            Ilustration = 18,

            /// <summary>
            /// Logotipo da banda ou artista.
            /// </summary>
            BandLogotype = 19,

            /// <summary>
            /// Logotipo do editor ou do estúdio.
            /// </summary>
            PublisherLogotype = 20
        }

        /// <summary>
        /// O tipo de moldura.
        /// </summary>
        public enum ESubFrameType
        {
            /// <summary>
            /// Constante.
            /// </summary>
            Constant = 0,

            /// <summary>
            /// Verbatim.
            /// </summary>
            Verbatim = 1,

            /// <summary>
            /// Fixa.
            /// </summary>
            Fixed,

            /// <summary>
            /// LPC.
            /// </summary>
            Lpc
        }

        /// <summary>
        /// Tipos de codificação residual.
        /// </summary>
        public enum EResidualCodingMethod
        {
            /// <summary>
            /// Codificação do tipo 1.
            /// </summary>
            ResidualCodingMehtodPartitionedRice = 0,

            /// <summary>
            /// Codificação do tipo 2.
            /// </summary>
            ResisualCodingMehtodPartitionedRice2 = 1
        }

        /// <summary>
        /// Define o cabeçalho de um bloco.
        /// </summary>
        public interface IBlockHeader
        {
            /// <summary>
            /// Obtém um valor que indica se se trata do último bloco.
            /// </summary>
            bool IsLastBlock { get; }

            /// <summary>
            /// Obtém o tipo de bloco.
            /// </summary>
            EBlockType BlockType { get; }

            /// <summary>
            /// Obtém o tamanho em bytes dos dados que se seguem no bloco.
            /// </summary>
            uint BlockLength { get; }
        }

        /// <summary>
        /// Dispõe a interface para um bloco geral.
        /// </summary>
        public interface IBlock
        {
            /// <summary>
            /// Obtém o cabeçalho do bloco.
            /// </summary>
            IBlockHeader Header { get; }
        }

        /// <summary>
        /// Dispõe a interface para um bloco descritivo.
        /// </summary>
        public interface IStreamInfoBlock : IBlock
        {
            /// <summary>
            /// Obtém o tamanho mínimo dos blocos em número de amostras.
            /// </summary>
            uint MinimumBlockSize { get; }

            /// <summary>
            /// Obtém o tamanho máximo dos blocos em número de amostras.
            /// </summary>
            uint MaximumBlockSize { get; }

            /// <summary>
            /// Obtém o tamanho mínimo em bytes do sistema de amostras.
            /// </summary>
            uint MinimumFrameSize { get; }

            /// <summary>
            /// Obtém o tamanho máximo em bytes do sistema de amostras.
            /// </summary>
            uint MaximumFrameSize { get; }

            /// <summary>
            /// Obtém a taxa de amostragem em número de amostras por segundo.
            /// </summary>
            uint SampleRate { get; }

            /// <summary>
            /// Obtém o número de canais.
            /// </summary>
            short NumberOfChannels { get; }

            /// <summary>
            /// Obtém o número de bits por amostra.
            /// </summary>
            short BitsPerSample { get; }

            /// <summary>
            /// Obtém o número total de amostras no ficheiro.
            /// </summary>
            /// <remarks>
            /// O número de amostras aqui é considerado independentemente
            /// do número de canais, uma vez que está associado o mesmo número de
            /// amostras a cada canal.
            /// </remarks>
            ulong TotalSamplesInStream { get; }

            /// <summary>
            /// Obtém a assinatura MD5 dos dados descodificados.
            /// </summary>
            IList<byte> MD5Signature { get; }
        }

        /// <summary>
        /// Define uma margem para possíveis edições de metadados.
        /// </summary>
        public interface IPaddingBlock : IBlock
        {
            /// <summary>
            /// Obtém o número de bits.
            /// </summary>
            uint BitsNumber { get; }
        }

        /// <summary>
        /// Define um bloco de aplicação.
        /// </summary>
        public interface IApplicationBlock : IBlock
        {
            /// <summary>
            /// O ID da aplicação registado no FLAC.
            /// </summary>
            uint ApplicationID { get; }

            /// <summary>
            /// Obtém uma linha para os dados da aplicação.
            /// </summary>
            Stream Data { get; }
        }

        /// <summary>
        /// Um ponto de marcação de ficheiro.
        /// </summary>
        public interface ISeekPoint
        {
            /// <summary>
            /// Obtém o número da primeira amostra.
            /// </summary>
            /// <remarks>
            /// O valor 0xFFFFFFFFFFFFFFFF caso não seja necessário.
            /// </remarks>
            ulong FirstSampleNr { get; }

            /// <summary>
            /// Deslocamento em bytes a partir do primeiro byte da
            /// primeira moldura.
            /// </summary>
            ulong Offset { get; }

            /// <summary>
            /// Número de amostras na moldura de destino.
            /// </summary>
            short NrSamplesTargetFrame { get; }
        }

        /// <summary>
        /// Um bloco de pontos de marcação.
        /// </summary>
        public interface ISeekTableBlock :
            IBlock,
            IEnumerable<ISeekPoint>
        {
        }

        /// <summary>
        /// Bloco de comentários.
        /// </summary>
        public interface IVorbisCommentBlock
            : IBlock
        {
            /// <summary>
            /// Obtém o comentário.
            /// </summary>
            byte[] Comment { get; }
        }

        /// <summary>
        /// Define um ponto de indexação da pista.
        /// </summary>
        public interface IIndexPoint
        {
            /// <summary>
            /// Obtém o desvio em número de amostras relativamente
            /// ao início do ficheiro FLAC.
            /// </summary>
            ulong SamplesIndexPointOffet { get; }

            /// <summary>
            /// O número do ponto de indexação.
            /// </summary>
            byte IndexPointNr { get; }
        }

        /// <summary>
        /// Define uma pista da folha de sinalização.
        /// </summary>
        public interface ICueSheetTrack
        {
            /// <summary>
            /// Desvio em amostras relativo ao início do ficheiro FLAC.
            /// </summary>
            ulong SamplesTrackOffset { get; }

            /// <summary>
            /// Obtém o número da pista.
            /// </summary>
            /// <remarks>
            /// Este número tem de ser diferente de zero.
            /// </remarks>
            byte TrackNr { get; }

            /// <summary>
            /// Obtém o text alfanumérico para o ISRC.
            /// </summary>
            byte[] TrackISRC { get; }

            /// <summary>
            /// Obtém o tipo de pista.
            /// </summary>
            ECueTrackType TrackType { get; }

            /// <summary>
            /// Obtém um valor que indica se a pré-ênfase se encontra activa.
            /// </summary>
            bool PreEnphasisFlag { get; }

            /// <summary>
            /// Número de pontos de indexação na pista.
            /// </summary>
            byte NrTrackIndexPoints { get; }
        }

        /// <summary>
        /// Folha de sinalização.
        /// </summary>
        public interface ICueSheetBlock
            : IBlock,
            IEnumerable<ICueSheetTrack>
        {
            /// <summary>
            /// Obtém o número de catálogo do tipo de média.
            /// </summary>
            byte[] MediaCatalogNumber { get; }

            /// <summary>
            /// O número de amostras iniciais.
            /// </summary>
            /// <remarks>
            /// Tem significado apenas no caso de CD onde a pista inicial
            /// contém a tabela de conteúdos e é conhecida como a pista 00.
            /// </remarks>
            ulong LeadInSamplesNr { get; }

            /// <summary>
            /// Obtém um valor que indica se se trata de um ficheiro de CD.
            /// </summary>
            bool CompactDisk { get; }

            /// <summary>
            /// Obtém o número de pistas.
            /// </summary>
            byte NrOfTracks { get; }
        }

        /// <summary>
        /// Bloco para figura.
        /// </summary>
        public interface IPictureBlock : IBlock
        {
            /// <summary>
            /// Obtém o tipo de figura.
            /// </summary>
            EPictureType PictureType { get; }

            /// <summary>
            /// Obtém o identificador do tipo de média.
            /// </summary>
            byte[] MimeTypeString { get; }

            /// <summary>
            /// Obtém a descrição.
            /// </summary>
            byte[] DescriptionString { get; }

            /// <summary>
            /// Obtém a largura da imagem em pixeis.
            /// </summary>
            uint Width { get; }

            /// <summary>
            /// Obtém a altura da imagem em pixeis.
            /// </summary>
            uint Height { get; }

            /// <summary>
            /// Obtém a profundidade de cor em bits por pixel.
            /// </summary>
            uint ColourDepth { get; }

            /// <summary>
            /// Obtém o número de cores utilizado.
            /// </summary>
            uint NrColoursUsed { get; }

            /// <summary>
            /// Obtém os dados da figura.
            /// </summary>
            byte[] PictureData { get; }
        }

        /// <summary>
        /// Define o cabeçalho de uma moldura.
        /// </summary>
        public interface IFrameHeader
        {
            /// <summary>
            /// Obtém um valor que indica se o tamanho dos blocos é fixo.
            /// </summary>
            bool FixedBlockStream { get; }

            /// <summary>
            /// Obtém o tamanho dos blocos em amostras, sendo estas independentes
            /// do número de canais.
            /// </summary>
            byte BlockSizeInInterChannelSamples { get; }

            /// <summary>
            /// Obtém a taxa de amostragem.
            /// </summary>
            uint SampleRate { get; }

            /// <summary>
            /// Obtém o número de canais.
            /// </summary>
            byte ChannelAssignement { get; }

            /// <summary>
            /// Obtém o tamanho da amostra em bits.
            /// </summary>
            byte SampleSizeInBits { get; }

            /// <summary>
            /// Obtém o número da amostra codificado.
            /// </summary>
            ulong CodedSampleNumber { get; }

            /// <summary>
            /// Obtém o polinómio relativo à verificação de redundância cíclica
            /// de 8 bit.
            /// </summary>
            byte Crc8Pol { get; }
        }

        /// <summary>
        /// Define o rodapé de uma moldura.
        /// </summary>
        public interface IFrameFooter
        {
            /// <summary>
            /// O polinómio relativo à verificação de redundância cíclica
            /// de 16 bit.
            /// </summary>
            ushort Crc16Pol { get; }
        }

        /// <summary>
        /// Define o cabeçalho de uma sub-moldura.
        /// </summary>
        public interface ISubFrameHeader
        {
            /// <summary>
            /// O tipo da sub-moldura.
            /// </summary>
            ESubFrameType Type { get; }

            /// <summary>
            /// A ordem do tipo quando se trata de fixo ou LPC.
            /// </summary>
            ushort TypeOrder { get; }

            /// <summary>
            /// Bits desperdiçados por amostra.
            /// </summary>
            ushort WaistedBitsPerSample { get; }
        }

        /// <summary>
        /// Define uma sub-moldura com uma constante.
        /// </summary>
        public interface IConstantSubFrame : ISubFrame
        {
            /// <summary>
            /// Obtém a constante.
            /// </summary>
            uint UnencodedConstant { get; }
        }

        /// <summary>
        /// Define uma sub-moldura padrão.
        /// </summary>
        public interface VerbatimSubFrame : ISubFrame
        {
            /// <summary>
            /// Obém o sub-bloco descodificado.
            /// </summary>
            byte[] UnencodedSubBlock { get; }
        }

        /// <summary>
        /// Define o residual.
        /// </summary>
        public interface IResidual
        {
            /// <summary>
            /// Obtém o método de codificação.
            /// </summary>
            EResidualCodingMethod CodingMethod { get; }
        }

        /// <summary>
        /// Define uma partição.
        /// </summary>
        public interface IRicePartition
        {
            /// <summary>
            /// Obtém o parâmetro de codificação.
            /// </summary>
            byte EncodingParameter { get; }
        }

        /// <summary>
        /// Define o método de codificação da partição.
        /// </summary>
        public interface IResidualCodeMethodPartitionedRice
            : IResidual,
            IEnumerable<IRicePartition>
        {
            /// <summary>
            /// Obtém a ordem da partição.
            /// </summary>
            byte PartitionOrder { get; }

            /// <summary>
            /// Obtém o residual.
            /// </summary>
            Stream Residual { get; }
        }

        /// <summary>
        /// Define uma sub-moldura fixa.
        /// </summary>
        public interface IFixedSubFrame : ISubFrame
        {
            /// <summary>
            /// Obtém as amostras não codificadas.
            /// </summary>
            byte[] UnencodedWarmupSamples { get; }

            /// <summary>
            /// Obtém o residual.
            /// </summary>
            IResidual Residual { get; }
        }

        /// <summary>
        /// Define uma sub-moldura LPC.
        /// </summary>
        public interface ILpcSubFrame : ISubFrame
        {
            /// <summary>
            /// Obtém as amostras não codificadas.
            /// </summary>
            byte[] UnencodedWarmupSamples { get; }

            /// <summary>
            /// Obtém a precisão do previsor linear quantizado.
            /// </summary>
            byte QuantLinPredictorCoeffPrecision { get; }

            /// <summary>
            /// Obtém o desvio do previsor linear quantizado.
            /// </summary>
            byte QuantLinPredictorCoeffShift { get; }

            /// <summary>
            /// Obtém os previsores não codificados.
            /// </summary>
            byte[] UnencodedPredictorCoeffs { get; }

            /// <summary>
            /// Obtém o residual.
            /// </summary>
            IResidual Residual { get; }
        }

        /// <summary>
        /// Define uma sub-moldura.
        /// </summary>
        public interface ISubFrame
        {
            /// <summary>
            /// Obtém o cabeçalho da sub-moldura.
            /// </summary>
            ISubFrameHeader Header { get; }
        }

        /// <summary>
        /// Define o cabeçalho de uma moldura.
        /// </summary>
        private class FrameHeader : IFrameHeader
        {
            /// <summary>
            /// Obtém um valor que indica se o tamanho dos blocos é fixo.
            /// </summary>
            private bool fixedBlockStream;

            /// <summary>
            /// O tamanho dos blocos em amostras, sendo estas independentes
            /// do número de canais.
            /// </summary>
            private byte blockSizeInInterChannelSamples;

            /// <summary>
            /// A taxa de amostragem.
            /// </summary>
            private uint sampleRate;

            /// <summary>
            /// O número de canais.
            /// </summary>
            private byte channelAssignement;

            /// <summary>
            /// O tamanho da amostra em bits.
            /// </summary>
            private byte sampleSizeInBits;

            /// <summary>
            /// O número da amostra codificado.
            /// </summary>
            private ulong codedSampleNumber;

            /// <summary>
            /// O polinómio relativo à verificação de redundância cíclica
            /// de 8 bit.
            /// </summary>
            private byte crc8Pol;

            /// <summary>
            /// Valor que indica se o cabeçalho não pode ser correctamente lido.
            /// </summary>
            private bool isUnparseable;

            /// <summary>
            /// Obtém um valor que indica se o tamanho dos blocos é fixo.
            /// </summary>
            public bool FixedBlockStream
            {
                get
                {
                    return this.fixedBlockStream;
                }
                set
                {
                    this.fixedBlockStream = value;
                }
            }

            /// <summary>
            /// Obtém o tamanho dos blocos em amostras, sendo estas independentes
            /// do número de canais.
            /// </summary>
            public byte BlockSizeInInterChannelSamples
            {
                get
                {
                    return this.blockSizeInInterChannelSamples;
                }
                set
                {
                    this.blockSizeInInterChannelSamples = value;
                }
            }

            /// <summary>
            /// Obtém a taxa de amostragem.
            /// </summary>
            public uint SampleRate
            {
                get
                {
                    return this.sampleRate;
                }
                set
                {
                    this.sampleRate = value;
                }
            }

            /// <summary>
            /// Obtém o número de canais.
            /// </summary>
            public byte ChannelAssignement
            {
                get
                {
                    return this.channelAssignement;
                }
                set
                {
                    this.channelAssignement = value;
                }
            }

            /// <summary>
            /// Obtém o tamanho da amostra em bits.
            /// </summary>
            public byte SampleSizeInBits
            {
                get
                {
                    return this.sampleSizeInBits;
                }
                set
                {
                    this.sampleSizeInBits = value;
                }
            }

            /// <summary>
            /// Obém o número da amostra codificado.
            /// </summary>
            public ulong CodedSampleNumber
            {
                get
                {
                    return this.codedSampleNumber;
                }
                set
                {
                    this.codedSampleNumber = value;
                }
            }

            /// <summary>
            /// Obtém o polinómio relativo à verificação de redundância cíclica
            /// de 8 bit.
            /// </summary>
            public byte Crc8Pol
            {
                get
                {
                    return this.crc8Pol;
                }
                set
                {
                    this.crc8Pol = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui um valor que indica se o cabeçalho não pode
            /// ser lido correctamente.
            /// </summary>
            public bool IsUnparseable
            {
                get
                {
                    return this.isUnparseable;
                }
                set
                {
                    this.isUnparseable = value;
                }
            }
        }

        /// <summary>
        /// Define uma moldura.
        /// </summary>
        public interface IFrame : IEnumerable<ISubFrame>
        {
            /// <summary>
            /// Obtém o cabeçalho da moldura.
            /// </summary>
            IFrameHeader Header { get; }

            /// <summary>
            /// Obtém o rodapé da moldura.
            /// </summary>
            IFrameFooter Footer { get; }
        }

        /// <summary>
        /// Descreve o cabeçalho de um bloco.
        /// </summary>
        private class BlockHeader : IBlockHeader
        {
            /// <summary>
            /// Indica se se trata do último bloco.
            /// </summary>
            protected bool isLastBlock;

            /// <summary>
            /// Define o tipo de bloco.
            /// </summary>
            protected EBlockType blockType;

            /// <summary>
            /// Define o tamanho em bytes dos dados que se seguem.
            /// </summary>
            protected uint blockLength;

            /// <summary>
            /// Obtém um valor que indica se se trata do último bloco.
            /// </summary>
            public bool IsLastBlock
            {
                get
                {
                    return this.isLastBlock;
                }
                set
                {
                    this.isLastBlock = value;
                }
            }

            /// <summary>
            /// Obtém o tipo de bloco.
            /// </summary>
            public EBlockType BlockType
            {
                get
                {
                    return this.blockType;
                }
                set
                {
                    this.blockType = value;
                }
            }

            /// <summary>
            /// Obtém o tamanho em bytes dos dados que se seguem no bloco.
            /// </summary>
            public uint BlockLength
            {
                get
                {
                    return this.blockLength;
                }
                set
                {
                    this.blockLength = value;
                }
            }
        }

        /// <summary>
        /// Descreve um bloco.
        /// </summary>
        private class Block : IBlock
        {
            /// <summary>
            /// O cabeçalho do bloco.
            /// </summary>
            protected IBlockHeader header;

            /// <summary>
            /// Obtém o cabeçalho do bloco.
            /// </summary>
            public IBlockHeader Header
            {
                get
                {
                    return this.header;
                }
                set
                {
                    this.header = value;
                }
            }
        }

        /// <summary>
        /// Descreve o bloco principal obrigatório.
        /// </summary>
        private class StreamInfoBlock
            : Block,
            IStreamInfoBlock
        {
            /// <summary>
            /// O tamanho mínimo dos blocos em número de amostras.
            /// </summary>
            protected uint minimumBlockSize;

            /// <summary>
            /// O tamanho máximo dos blocos em número de amostras.
            /// </summary>
            protected uint maximumBlockSize;

            /// <summary>
            /// O tamanho mínimo em bytes do sistema de amostras.
            /// </summary>
            protected uint minimumFrameSize;

            /// <summary>
            /// O tamanho máximo em bytes do sistema de amostras.
            /// </summary>
            protected uint maximumFrameSize;

            /// <summary>
            /// A taxa de amostragem em número de amostras por segundo.
            /// </summary>
            protected uint sampleRate;

            /// <summary>
            /// O número de canais.
            /// </summary>
            protected short numberOfChannels;

            /// <summary>
            /// O número de bits por amostra.
            /// </summary>
            protected short bitsPerSample;

            /// <summary>
            /// O número total de amostras no ficheiro.
            /// </summary>
            /// <remarks>
            /// O número de amostras aqui é considerado independentemente
            /// do número de canais, uma vez que está associado o mesmo número de
            /// amostras a cada canal.
            /// </remarks>
            protected ulong totalSamplesInStream;

            /// <summary>
            /// A assinatura MD5 dos dados descodificados.
            /// </summary>
            protected List<byte> mdSignature;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="StreamInfoBlock"/>.
            /// </summary>
            public StreamInfoBlock()
            {
                this.mdSignature = new List<byte>();
            }

            /// <summary>
            /// Obtém o tamanho mínimo dos blocos em número de amostras.
            /// </summary>
            public uint MinimumBlockSize
            {
                get
                {
                    return this.minimumBlockSize;
                }
                set
                {
                    this.minimumBlockSize = value;
                }
            }

            /// <summary>
            /// Obtém o tamanho máximo dos blocos em número de amostras.
            /// </summary>
            public uint MaximumBlockSize
            {
                get
                {
                    return this.maximumBlockSize;
                }
                set
                {
                    this.maximumBlockSize = value;
                }
            }

            /// <summary>
            /// Obtém o tamanho mínimo em bytes do sistema de amostras.
            /// </summary>
            public uint MinimumFrameSize
            {
                get
                {
                    return this.minimumFrameSize;
                }
                set
                {
                    this.minimumFrameSize = value;
                }
            }

            /// <summary>
            /// Obtém o tamanho máximo em bytes do sistema de amostras.
            /// </summary>
            public uint MaximumFrameSize
            {
                get
                {
                    return this.maximumFrameSize;
                }
                set
                {
                    this.maximumFrameSize = value;
                }
            }

            /// <summary>
            /// Obtém a taxa de amostragem em número de amostras por segundo.
            /// </summary>
            public uint SampleRate
            {
                get
                {
                    return this.sampleRate;
                }
                set
                {
                    this.sampleRate = value;
                }
            }

            /// <summary>
            /// Obtém o número de canais.
            /// </summary>
            public short NumberOfChannels
            {
                get
                {
                    return this.numberOfChannels;
                }
                set
                {
                    this.numberOfChannels = value;
                }
            }

            /// <summary>
            /// Obtém o número de bits por amostra.
            /// </summary>
            public short BitsPerSample
            {
                get
                {
                    return this.bitsPerSample;
                }
                set
                {
                    this.bitsPerSample = value;
                }
            }

            /// <summary>
            /// Obtém o número total de amostras no ficheiro.
            /// </summary>
            /// <remarks>
            /// O número de amostras aqui é considerado independentemente
            /// do número de canais, uma vez que está associado o mesmo número de
            /// amostras a cada canal.
            /// </remarks>
            public ulong TotalSamplesInStream
            {
                get
                {
                    return this.totalSamplesInStream;
                }
                set
                {
                    this.totalSamplesInStream = value;
                }
            }

            /// <summary>
            /// Obtém a assinatura MD5 dos dados descodificados.
            /// </summary>
            public IList<byte> MD5Signature
            {
                get
                {
                    return this.mdSignature.AsReadOnly();
                }
            }

            /// <summary>
            /// Obtém a lista com a assinatura MD5.
            /// </summary>
            public List<byte> InternalMdSignature
            {
                get
                {
                    return this.mdSignature;
                }
            }

            /// <summary>
            /// Obtém a lista com a assinatura MD5.
            /// </summary>
            public List<byte> InternalMD5Signature
            {
                get
                {
                    return this.mdSignature;
                }
            }
        }

        /// <summary>
        /// Representa um comentário.
        /// </summary>
        private class VorbisCommentBlock
            : Block,
            IVorbisCommentBlock
        {
            /// <summary>
            /// O comentário.
            /// </summary>
            private byte[] comment;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="VorbisCommentBlock"/>.
            /// </summary>
            /// <param name="header">O cabeçalho do bloco.</param>
            /// <param name="parent">O leitor.</param>
            public VorbisCommentBlock(
                BlockHeader header,
                FlacReader parent)
            {
                this.header = header;
                this.comment = new byte[this.header.BlockLength];
                var readed = parent.stream.Read(
                    this.comment,
                    0,
                    (int)this.header.BlockLength);
                if (readed != this.header.BlockLength)
                {
                    throw new UtilitiesException("Found end of file before reading vorbis comment.");
                }
            }

            /// <summary>
            /// Obtém o comentário.
            /// </summary>
            public byte[] Comment
            {
                get
                {
                    return this.comment;
                }
            }
        }

        private class PictureBlock
            : Block,
            IPictureBlock
        {
            /// <summary>
            /// O leitor ao qual pertence o bloco.
            /// </summary>
            private FlacReader parent;

            /// <summary>
            /// O tipo de figura.
            /// </summary>
            private EPictureType pictureType;

            /// <summary>
            /// O identificador do tipo de média.
            /// </summary>
            private byte[] mimeTypeString;

            /// <summary>
            /// A descrição.
            /// </summary>
            private byte[] descriptionString;

            /// <summary>
            /// A largura da imagem em pixeis.
            /// </summary>
            private uint width;

            /// <summary>
            /// A altura da imagem em pixeis.
            /// </summary>
            private uint height;

            /// <summary>
            /// A profundidade de cor em bits por pixel.
            /// </summary>
            private uint colourDepth;

            /// <summary>
            /// O número de cores utilizado.
            /// </summary>
            private uint nrColoursUsed;

            /// <summary>
            /// Os dados da figura.
            /// </summary>
            private byte[] pictureData;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="PictureBlock"/>.
            /// </summary>
            /// <param name="header">O cabeçalho do bloco.</param>
            /// <param name="reader">O leitor.</param>
            public PictureBlock(
                BlockHeader header,
                FlacReader reader)
            {
                this.header = header;
                this.parent = reader;
                this.ReadFromStream();
            }

            /// <summary>
            /// Obtém ou atribui o tipo de figura.
            /// </summary>
            public EPictureType PictureType
            {
                get
                {
                    return this.pictureType;
                }
                set
                {
                    this.pictureType = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui o identificador do tipo de média.
            /// </summary>
            public byte[] MimeTypeString
            {
                get
                {
                    return this.mimeTypeString;
                }
                set
                {
                    this.mimeTypeString = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui a descrição.
            /// </summary>
            public byte[] DescriptionString
            {
                get
                {
                    return this.descriptionString;
                }
                set
                {
                    this.descriptionString = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui a largura da imagem em pixeis.
            /// </summary>
            public uint Width
            {
                get
                {
                    return this.width;
                }
                set
                {
                    this.width = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui a altura da imagem em pixeis.
            /// </summary>
            public uint Height
            {
                get
                {
                    return this.height;
                }
                set
                {
                    this.height = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui a profundidade de cor em bits por pixel.
            /// </summary>
            public uint ColourDepth
            {
                get
                {
                    return this.colourDepth;
                }
                set
                {
                    this.colourDepth = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui o número de cores utilizado.
            /// </summary>
            public uint NrColoursUsed
            {
                get
                {
                    return this.nrColoursUsed;
                }
                set
                {
                    this.nrColoursUsed = value;
                }
            }

            /// <summary>
            /// Obtém os dados da figura.
            /// </summary>
            public byte[] PictureData
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            /// Efectua a leitura dos campos a partir do fluxo de dados.
            /// </summary>
            private void ReadFromStream()
            {
                var track = this.header.BlockLength;
                var type = this.parent.ReadUintValue(4);
                track -= 4;
                this.pictureType = (EPictureType)type;
                var length = this.parent.ReadUintValue(4);
                track -= 4;
                this.mimeTypeString = new byte[length];
                var readed = this.parent.stream.Read(
                    this.mimeTypeString,
                    0,
                    (int)length);
                if (readed < length)
                {
                    throw new UtilitiesException("Found end of file before complete picture block.");
                }

                track -= length;
                length = this.parent.ReadUintValue(4);
                track -= 4;
                this.descriptionString = new byte[length];
                readed = this.parent.stream.Read(
                    this.descriptionString,
                    0,
                    (int)length);
                if (readed < length)
                {
                    throw new UtilitiesException("Found end of file before complete picture block.");
                }

                track -= length;
                this.width = this.parent.ReadUintValue(4);
                track -= 4;
                this.height = this.parent.ReadUintValue(4);
                track -= 4;
                this.colourDepth = this.parent.ReadUintValue(4);
                track -= 4;
                this.nrColoursUsed = this.parent.ReadUintValue(4);
                track -= 4;
                length = this.parent.ReadUintValue(4);
                track -= 4;
                this.pictureData = new byte[length];
                readed = this.parent.stream.Read(
                    this.pictureData,
                    0,
                    (int)length);
                if (readed != length)
                {
                    throw new UtilitiesException("Found end of file before complete picture block.");
                }

                track -= length;
                if (track != 0)
                {
                    throw new UtilitiesException("Invalid format.");
                }
            }
        }

        #endregion Tipos internos
    }
}
