using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class CoefficientDegreePair<CoeffType>
    {
        private int degree;

        private CoeffType coeff;

        public CoefficientDegreePair(int degree, CoeffType coeff)
        {
            this.degree = degree;
            this.coeff = coeff;
        }

        public int Degree
        {
            get
            {
                return this.degree;
            }
        }

        public CoeffType Coeff
        {
            get
            {
                return this.coeff;
            }
        }
    }
}
