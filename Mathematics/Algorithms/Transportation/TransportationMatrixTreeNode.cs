namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um nó na árvore de pesquisa do problema do transporte.
    /// </summary>
    internal class TransportationMatrixTreeNode
    {
        /// <summary>
        /// Obtém ou atribui o valor da linha.
        /// </summary>
        /// <value>A linha.</value>
        public int Line { get; set; }

        /// <summary>
        /// Obtém ou atribui o valor da coluna.
        /// </summary>
        /// <value>A coluna.</value>
        public int Column { get; set; }

        /// <summary>
        /// Obtém ou atribui o próximo nó.
        /// </summary>
        /// <value>O nó.</value>
        public TransportationMatrixTreeNode Next { get; set; }

        /// <summary>
        /// Determina se o objecto proporcionado é igual à instância corrente.
        /// </summary>
        /// <param name="obj">O objecto..</param>
        /// <returns>
        /// Verdadeiro caso o objecto seja igual e falso caso contrário.
        /// </returns>
        public override bool Equals(object obj)
        {
            var innerObj = obj as TransportationMatrixTreeNode;
            if (innerObj == null)
            {
                return false;
            }

            return this.Line == innerObj.Line && this.Column == innerObj.Column;
        }

        /// <summary>
        /// Retorna um código confuso para a instância actual.
        /// </summary>
        /// <returns>
        /// O código confuso da instância actual útil em alguns algoritmos.
        /// </returns>
        public override int GetHashCode()
        {
            return (this.Line.GetHashCode() ^ this.Column.GetHashCode()).GetHashCode();
        }
    }
}
