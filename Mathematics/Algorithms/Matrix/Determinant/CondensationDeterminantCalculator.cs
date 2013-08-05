using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.Algorithms
{
    /// <summary>
    /// Calcula o valor do determinante com o recurso ao método da condensação.
    /// </summary>
    /// <remarks>
    /// O método da condensação consiste num método mais eficaz no que concerne ao cálculo
    /// do valor do determinante de uma matriz. Porém, este método requer uma alteração do
    /// objecto sobre o qual actua, passando a ser necessária uma réplica do mesmo em memória.
    /// </remarks>
    /// <typeparam name="ObjectType">O tipo de objeto a ser processado.</typeparam>
    /// <typeparam name="EuclideanDomainType">O tipo do domínio que permite efectuar as operações sobre os elementos.</typeparam>
    public class CondensationDeterminantCalculator<ObjectType, EuclideanDomainType> : ADeterminant<ObjectType, EuclideanDomainType>
        where EuclideanDomainType : IEuclidenDomain<ObjectType>
    {
        public CondensationDeterminantCalculator(EuclideanDomainType domain)
            : base(domain)
        {
        }

        protected override ObjectType ComputeDeterminant(IMatrix<ObjectType> data)
        {
            var positiveResult = true;
            var determinantFactors = new List<ObjectType>();
            throw new NotImplementedException();
        }
    }
}
