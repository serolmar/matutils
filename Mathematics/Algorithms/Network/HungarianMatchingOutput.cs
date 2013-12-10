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
        private int[] affectation;

        private int[] maximumDomainAffected;

        private int[] maximumTargetAffected;

        private bool hasSolution;

        internal HungarianMatchingOutput(
            int[] affectation,
            int[] maximumDomainAffected,
            int[] maximumTargetAffected,
            bool hasSolution)
        {
            this.affectation = affectation;
            this.maximumDomainAffected = maximumDomainAffected;
            this.maximumTargetAffected = maximumTargetAffected;
        }

        public int[] Affectation
        {
            get
            {
                return this.affectation;
            }
        }

        public int[] MaximumDomainAffected
        {
            get
            {
                return this.maximumDomainAffected;
            }
        }

        public int[] MaximumTargetAffected
        {
            get
            {
                return this.maximumTargetAffected;
            }
        }

        public bool HasSolution
        {
            get
            {
                return this.hasSolution;
            }
        }
    }
}
