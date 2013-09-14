namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DualHeuristicAlgInput<ElementType>
    {
        /// <summary>
        /// Uma estimativa para a variável gama.
        /// </summary>
        private ElementType gamma;

        /// <summary>
        /// Uma estimativa para as variáveis lambda.
        /// </summary>
        private Dictionary<int, ElementType> lambdas;

        /// <summary>
        /// Uma estimativa para as variáveis tau.
        /// </summary>
        private Dictionary<int, ElementType> taus;

        /// <summary>
        /// O conjunto de valo
        /// </summary>
        private Dictionary<int, ElementType> cbar;

        public DualHeuristicAlgInput()
        {
            this.lambdas = new Dictionary<int, ElementType>();
            this.taus = new Dictionary<int, ElementType>();
            this.cbar = new Dictionary<int, ElementType>();
        }

        /// <summary>
        /// Obtém e atribui um valor que representa uma estimativa para a variável gama.
        /// </summary>
        public ElementType Gamma
        {
            get
            {
                return this.gamma;
            }
            set
            {
                this.gamma = value;
            }
        }

        /// <summary>
        /// Obtém o dicionário que contém as várias estimativas para as variáveis lamnda.
        /// </summary>
        public Dictionary<int, ElementType> Lambdas
        {
            get
            {
                return this.lambdas;
            }
        }

        /// <summary>
        /// Obtém o dicionário que contém as várias estimativas para as variáveis tau.
        /// </summary>
        public Dictionary<int, ElementType> Taus
        {
            get
            {
                return this.taus;
            }
        }

        /// <summary>
        /// Obtém o conjunto dos valores limite.
        /// </summary>
        public Dictionary<int, ElementType> Cbar
        {
            get
            {
                return this.cbar;
            }
        }
    }
}
