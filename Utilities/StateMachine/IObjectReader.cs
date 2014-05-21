namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define um leitor de objectos.
    /// </summary>
    public interface IObjectReader<out Object>
    {
        /// <summary>
        /// Obtém o objecto sem avançar o cursor.
        /// </summary>
        /// <returns>O objecto.</returns>
        Object Peek();

        /// <summary>
        /// Obtém o objecto e avança o cursor.
        /// </summary>
        /// <returns>O objecto.</returns>
        Object Get();

        /// <summary>
        /// Retrocede o cursor.
        /// </summary>
        void UnGet();

        /// <summary>
        /// Determina um valor que indica se o final do ficheiro foi atingido.
        /// </summary>
        /// <returns>Verdadeiro caso o final de ficheiro tenha sido atingido e falso caso contrário.</returns>
        bool IsAtEOF();
    }
}
