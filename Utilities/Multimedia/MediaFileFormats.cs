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
        private BitReader bitReader;

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
                this.bitReader = new BitReader(stream);
            }
        }
    }
}
