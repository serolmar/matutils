using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace Mathematics
{
    /// <summary>
    /// Clase responsável por gerenciar os mementos aferentes à leitura
    /// arbitrária de matrizes.
    /// </summary>
    class RangeReaderMementoManager
    {
        /// <summary>
        /// O memorizador.
        /// </summary>
        private IMemento memento;

        /// <summary>
        /// O nível.
        /// </summary>
        private int level;

        /// <summary>
        /// O número de elementos lidos.
        /// </summary>
        private int currentReadedElements;

        /// <summary>
        /// Obtém ou atribui o memorizador.
        /// </summary>
        /// <value>
        /// O memorizador.
        /// </value>
        public IMemento Memento
        {
            get
            {
                return this.memento;
            }
            set
            {
                this.memento = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o nível.
        /// </summary>
        /// <value>
        /// O nível.
        /// </value>
        public int Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o número de elementos lidos.
        /// </summary>
        /// <value>
        /// O número de elementos lidos.
        /// </value>
        public int CurrentReadedElements
        {
            get
            {
                return this.currentReadedElements;
            }
            set
            {
                this.currentReadedElements = value;
            }
        }
    }
}
