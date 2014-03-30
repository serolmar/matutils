namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    public class LabelsReader
    {
        #region Carácteres Especiais
        /// <summary>
        /// O carácter de mudança de linha.
        /// </summary>
        private static int newLine = 12;

        /// <summary>
        /// O carácter de mudança de linha com arrasto.
        /// </summary>
        private static int carriageReturn = 15;

        /// <summary>
        /// O carácter que corresponde ao espaço.
        /// </summary>
        private static int space = 32;

        /// <summary>
        /// O carácter que corresponde ao ponto-e-vírgula.
        /// </summary>
        private static int semicollon = 59;

        # endregion Carácteres Especiais

        /// <summary>
        /// O formato sobre o qual são lidos os valores numéricos não poderá depender da cultura.
        /// </summary>
        private static NumberFormatInfo numberFormat = CultureInfo.InvariantCulture.NumberFormat;

        /// <summary>
        /// Permite fazer a leitura das referências a partir de um ficheiro de texto.
        /// </summary>
        /// <param name="stream">A representação do ficheiro.</param>
        /// <param name="encoding">A codificação do ficheiro.</param>
        /// <returns>O conjunto de referências lidas.</returns>
        public List<Label> ReadLabels(Stream stream, Encoding encoding)
        {
            if (stream == null)
            {

                throw new ArgumentException("stream");
            }
            else
            {
                var textReader = new StreamReader(stream, encoding);
                var currentLine = 1;
                var result = new List<Label>();
                if (!textReader.EndOfStream)
                {
                    var state = 0;
                    var readedBuffer = string.Empty;
                    var currentLabel = new Label();
                    var labelInsertion = new[] { currentLabel.MtdBits, currentLabel.MBits, currentLabel.TmBits };
                    var labelInsertionPointer = 0;
                    var bitsNumberValue = 0;
                    var bitValues = new List<ulong>();
                    var emtpyLine = true;
                    while (state != -1)
                    {
                        var readed = textReader.Read();
                        switch (state)
                        {
                            case 0:
                                while (state == 0)
                                {
                                    if (textReader.EndOfStream)
                                    {
                                        state = -1;
                                    }
                                    else
                                    {
                                        readed = textReader.Read();
                                        if (readed == newLine)
                                        {
                                            if (!emtpyLine)
                                            {
                                                throw new OdmpProblemException(string.Format(
                                                    "An error has occured at line {0}.",
                                                    currentLine));
                                            }
                                        }
                                        else if (readed == semicollon)
                                        {
                                            throw new OdmpProblemException(string.Format(
                                                "An error has occured at line {0}.",
                                                currentLine));
                                        }
                                        else if (readed == space)
                                        {
                                            if (!string.IsNullOrWhiteSpace(readedBuffer))
                                            {
                                                if (int.TryParse(readedBuffer, out bitsNumberValue))
                                                {
                                                    readedBuffer = string.Empty;
                                                    state = 1;
                                                }
                                                else
                                                {
                                                    throw new OdmpProblemException(string.Format(
                                                        "An error has occured at line {0}.",
                                                        currentLine));
                                                }
                                            }
                                        }
                                        else if(readed != carriageReturn)
                                        {
                                            readedBuffer += (char)readed;
                                            emtpyLine = false;
                                        }
                                    }
                                }

                                break;
                            case 1:
                                while (state == 1)
                                {
                                    if (textReader.EndOfStream)
                                    {
                                        throw new OdmpProblemException(string.Format(
                                            "An error has occured at line {0}.",
                                            currentLine));
                                    }
                                    else
                                    {
                                        readed = textReader.Read();
                                        if (readed == space)
                                        {
                                            if (!string.IsNullOrWhiteSpace(readedBuffer))
                                            {
                                                var value = default(ulong);
                                                if (ulong.TryParse(readedBuffer, out value))
                                                {
                                                    bitValues.Add(value);
                                                    readedBuffer = string.Empty;
                                                }
                                                else
                                                {
                                                    throw new OdmpProblemException(string.Format(
                                                        "An error has occured at line {0}.",
                                                        currentLine));
                                                }
                                            }
                                        }
                                        else if (readed == semicollon)
                                        {
                                            if (!string.IsNullOrWhiteSpace(readedBuffer))
                                            {
                                                var value = default(ulong);
                                                if (ulong.TryParse(readedBuffer, out value))
                                                {
                                                    bitValues.Add(value);
                                                    readedBuffer = string.Empty;
                                                }
                                                else
                                                {
                                                    throw new OdmpProblemException(string.Format(
                                                        "An error has occured at line {0}.",
                                                        currentLine));
                                                }
                                            }

                                            labelInsertion[labelInsertionPointer++].AddRange(
                                                bitValues.ToArray(),
                                                bitsNumberValue);
                                            bitsNumberValue = 0;
                                            bitValues.Clear();

                                            // Prepara o apontador para a linha seguinte aquando de três leituras.
                                            if (labelInsertionPointer == 3)
                                            {
                                                state = 2;
                                            }
                                            else
                                            {
                                                state = 0;
                                            }
                                        }
                                        else if (readed == newLine)
                                        {
                                            throw new OdmpProblemException(string.Format(
                                                "An error has occured at line {0}.",
                                                currentLine));
                                        }
                                        else if(readed != carriageReturn)
                                        {
                                            readedBuffer += (char)readed;
                                        }
                                    }
                                }

                                break;
                            case 2:
                                while (state == 2)
                                {
                                    if (textReader.EndOfStream)
                                    {
                                        throw new OdmpProblemException(string.Format(
                                            "An error has occured at line {0}.",
                                            currentLine));
                                    }
                                    else
                                    {
                                        readed = textReader.Read();
                                        if (readed == semicollon)
                                        {
                                            var value = default(double);
                                            if (double.TryParse(
                                                readedBuffer,
                                                NumberStyles.Number,
                                                numberFormat,
                                                out value))
                                            {
                                                currentLabel.Price = value;
                                                state = 3;
                                            }
                                            else
                                            {
                                                throw new OdmpProblemException(string.Format(
                                                    "An error has occured at line {0}.",
                                                    currentLine));
                                            }
                                        }
                                        else if (readed == newLine)
                                        {
                                            throw new OdmpProblemException(string.Format(
                                                "An error has occured at line {0}.",
                                                currentLine));
                                        }
                                        else if(readed != carriageReturn)
                                        {
                                            readedBuffer += (char)readed;
                                        }
                                    }
                                }

                                break;
                            case 3:
                                while (state == 3)
                                {
                                    if (textReader.EndOfStream)
                                    {
                                        state = -1;
                                    }
                                    else
                                    {
                                        readed = textReader.Read();
                                        if (readed == semicollon)
                                        {
                                            var value = default(double);
                                            if (double.TryParse(
                                                readedBuffer,
                                                NumberStyles.Number,
                                                numberFormat,
                                                out value))
                                            {
                                                currentLabel.Rate = value;
                                                state = 4;
                                            }
                                            else
                                            {
                                                throw new OdmpProblemException(string.Format(
                                                    "An error has occured at line {0}.",
                                                    currentLine));
                                            }
                                        }
                                        else if (readed == newLine)
                                        {
                                            throw new OdmpProblemException(string.Format(
                                                "An error has occured at line {0}.",
                                                currentLine));
                                        }
                                        else if(readed != carriageReturn)
                                        {
                                            readedBuffer += (char)readed;
                                        }
                                    }
                                }
                                break;
                            case 4:
                                while (state == 2)
                                {
                                    if (textReader.EndOfStream)
                                    {
                                        if (string.IsNullOrWhiteSpace(readedBuffer))
                                        {
                                            throw new OdmpProblemException(string.Format(
                                                "An error has occured at line {0}.",
                                                currentLine));
                                        }
                                        else
                                        {
                                            var value = default(double);
                                            if (double.TryParse(
                                                readedBuffer,
                                                NumberStyles.Number,
                                                numberFormat,
                                                out value))
                                            {
                                                currentLabel.CarsNumber = value;
                                                state = 0;
                                                emtpyLine = true;
                                            }
                                            else
                                            {
                                                throw new OdmpProblemException(string.Format(
                                                    "An error has occured at line {0}.",
                                                    currentLine));
                                            }

                                            state = -1;
                                        }
                                    }
                                    else
                                    {
                                        readed = textReader.Read();
                                        if (readed == semicollon)
                                        {
                                            throw new OdmpProblemException(string.Format(
                                                "An error has occured at line {0}.",
                                                currentLine));
                                        }
                                        else if (readed == newLine)
                                        {
                                            var value = default(double);
                                            if (double.TryParse(
                                                readedBuffer,
                                                NumberStyles.Number,
                                                numberFormat,
                                                out value))
                                            {
                                                currentLabel.CarsNumber = value;
                                                state = 0;
                                                emtpyLine = true;
                                            }
                                            else
                                            {
                                                throw new OdmpProblemException(string.Format(
                                                    "An error has occured at line {0}.",
                                                    currentLine));
                                            }

                                            result.Add(currentLabel);
                                            currentLabel = new Label();
                                            labelInsertion = new[] { currentLabel.MtdBits, currentLabel.MBits, currentLabel.TmBits };
                                            labelInsertionPointer = 0;

                                            ++currentLine;
                                            state = 0;
                                        }
                                        else
                                        {
                                            readedBuffer += (char)readed;
                                        }
                                    }
                                }

                                break;
                            default:
                                throw new OdmpProblemException("A internal error has occurred.");
                        }
                    }
                }

                return result;
            }
        }
    }
}
