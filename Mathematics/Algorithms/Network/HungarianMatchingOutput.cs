namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Contém os dados de saída resultantes da aplicação do algoritmo húngaro.
    /// </summary>
    public class HungarianMatchingOutput
    {
        /// <summary>
        /// As afectações.
        /// </summary>
        private int[] affectation;

        /// <summary>
        /// O maior domínio afectado.
        /// </summary>
        private int[] maximumDomainAffected;

        /// <summary>
        /// A maior imagem afectada.
        /// </summary>
        private int[] maximumTargetAffected;

        /// <summary>
        /// Valor que indica se existe uma solução.
        /// </summary>
        private bool hasSolution;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="HungarianMatchingOutput"/>.
        /// </summary>
        /// <param name="affectation">A afectação.</param>
        /// <param name="maximumDomainAffected">O domínio máximo afectado.</param>
        /// <param name="maximumTargetAffected">A imagem máxima afectada.</param>
        /// <param name="hasSolution">Um valor que indica se existe solução ou não.</param>
        internal HungarianMatchingOutput(
            int[] affectation,
            int[] maximumDomainAffected,
            int[] maximumTargetAffected,
            bool hasSolution)
        {
            this.affectation = affectation;
            this.maximumDomainAffected = maximumDomainAffected;
            this.maximumTargetAffected = maximumTargetAffected;
            this.hasSolution = hasSolution;
        }

        /// <summary>
        /// Obtém as afectações.
        /// </summary>
        /// <value>
        /// As afectações.
        /// </value>
        public int[] Affectation
        {
            get
            {
                return this.affectation;
            }
        }

        /// <summary>
        /// Obtém o domínio máximo afectado.
        /// </summary>
        /// <value>
        /// O domínio máximo afectado.
        /// </value>
        public int[] MaximumDomainAffected
        {
            get
            {
                return this.maximumDomainAffected;
            }
        }

        /// <summary>
        /// Obtém a imagem máxima afectada.
        /// </summary>
        /// <value>
        /// A imagem máxima afectada.
        /// </value>
        public int[] MaximumTargetAffected
        {
            get
            {
                return this.maximumTargetAffected;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se existe uma solução.
        /// </summary>
        /// <value>
        /// Verdadeiro caso a solução exista e falso caso contrário.
        /// </value>
        public bool HasSolution
        {
            get
            {
                return this.hasSolution;
            }
        }
    }
}
