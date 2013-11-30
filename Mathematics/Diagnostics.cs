namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Mantém algums funções de diagnóstico.
    /// </summary>
    public static class Diagnostics
    {
        /// <summary>
        /// Obtém um valor textual com a representação de um determinado número inteiro
        /// em termos dos respectivos bits.
        /// </summary>
        /// <param name="value">O valor a ser analisado.</param>
        /// <param name="group">Permite configurar o número de bits que serão agrupados.</param>
        /// <returns>A representação textual.</returns>
        public static string GetBits(uint value, int group)
        {
            if (group < 0)
            {
                throw new ArgumentOutOfRangeException("group");
            }
            else
            {
                var result = string.Empty;
                var temp = value;
                var mask = (uint.MaxValue >> 1) + 1;
                for (int i = 0; i < 32; ++i)
                {
                    var separator = string.Empty;
                    if (group != 0 && i % group == 0)
                    {
                        separator = " ";
                    }

                    result += separator;
                    result += (temp & mask) == 0 ? "0" : "1";
                    mask >>= 1;
                }

                return result;
            }
        }
    }
}
