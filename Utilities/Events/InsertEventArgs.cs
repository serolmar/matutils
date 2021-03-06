﻿namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// O delegado responsável pela âncora de um evento de inserção a uma função.
    /// </summary>
    /// <typeparam name="T">O tipo de elemento a ser inserido.</typeparam>
    /// <param name="sender">O objecto que envia o evento.</param>
    /// <param name="eventArgs">Os argumentos do evento.</param>
    public delegate void InsertEventHandler<T>(object sender, InsertEventArgs<T> eventArgs);

    /// <summary>
    /// Representa os argumentos de um evento que ocorre aquando da inserção de um objecto.
    /// </summary>
    /// <typeparam name="T">O tipo de objecto.</typeparam>
    public class InsertEventArgs<T> : EventArgs
    {
        /// <summary>
        /// A posição onde o elemento será introduzido.
        /// </summary>
        private int insertionPosition;

        /// <summary>
        /// O objecto a ser introduzido.
        /// </summary>
        private T objectToInsert;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="InsertEventArgs{T}"/>.
        /// </summary>
        /// <param name="insertionPosition">A posição de inserção.</param>
        /// <param name="objectToInsert">O objecto a ser inserido.</param>
        public InsertEventArgs(int insertionPosition, T objectToInsert)
        {
            this.insertionPosition = insertionPosition;
            this.objectToInsert = objectToInsert;
        }

        /// <summary>
        /// Obtém a posição onde o objecto será introduzido.
        /// </summary>
        /// <value>A posição de inserção.</value>
        public int InsertionPosition
        {
            get
            {
                return this.insertionPosition;
            }
        }

        /// <summary>
        /// Obtém o objecto a ser introduzido.
        /// </summary>
        /// <value>O objecto.</value>
        public T ObjectToInsert
        {
            get
            {
                return this.objectToInsert;
            }
        }
    }
}
